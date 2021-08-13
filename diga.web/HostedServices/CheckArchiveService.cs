using diga.bl.Constants;
using diga.bl.Models;
using diga.bl.Models.Platform.Contracts;
using diga.dal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace diga.web.HostedServices
{
    public class CheckArchiveService : IHostedService, IDisposable
    {
        private readonly ILogger<CheckArchiveService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private Timer _timer;

        public CheckArchiveService(ILogger<CheckArchiveService> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Check Archive Service running.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromDays(1));

            return Task.CompletedTask;
        }

        private async void DoWork(object state)
        {
            try
            {
                using (IServiceScope scope = _scopeFactory.CreateScope())
                {
                    DigaContext db = scope.ServiceProvider.GetRequiredService<DigaContext>();

                    List<PlatformContract> contracts = await db.PlatformContracts.Where(c => c.Status == PlatformContractStatuses.Contractor &&
                        (!c.TenderEndDate.HasValue || DateTime.UtcNow > c.TenderEndDate)).ToListAsync();

                    foreach (PlatformContract contract in contracts)
                        contract.Status = PlatformContractStatuses.Archived;

                    db.PlatformContracts.UpdateRange(contracts);
                    await db.SaveChangesAsync();
                }

            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
            }
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Check Archive Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
