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
    public class DeferredService : IHostedService, IDisposable
    {
        private readonly ILogger<DeferredService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private Timer timer;

        public DeferredService(ILogger<DeferredService> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Deferred Service running.");

            new Thread(async () =>
            {
                DateTime now = DateTime.UtcNow;
                DateTime tomorrow = now.AddDays(1);
                DateTime neededStartDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 0, 1, 0);

                await Task.Delay(neededStartDate - now);
                timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromDays(1));
            }).Start();

            return Task.CompletedTask;
        }

        private async void DoWork(object state)
        {
            try
            {
                using (IServiceScope scope = _scopeFactory.CreateScope())
                {
                    DigaContext db = scope.ServiceProvider.GetRequiredService<DigaContext>();

                    List<PlatformContract> contracts = await db.PlatformContracts.Where(c => c.Status == PlatformContractStatuses.Deferred &&
                        c.PublishDate <= DateTime.UtcNow).ToListAsync();

                    foreach (PlatformContract contract in contracts)
                        contract.Status = PlatformContractStatuses.Contractor;

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
            _logger.LogInformation("Deferred Service is stopping.");

            timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            timer?.Dispose();
        }
    }
}
