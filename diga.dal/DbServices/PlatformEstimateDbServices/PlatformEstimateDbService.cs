using diga.bl.Constants;
using diga.bl.Models.Platform.Contracts;
using diga.bl.Models.Platform.Estimates;
using diga.dal.DbServices.DefaultDbServices;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diga.dal.DbServices.PlatformEstimateDbServices
{
    public class PlatformEstimateDbService : DefaultDbService<int, PlatformEstimate>, IPlatformEstimateDbService
    {
        private readonly DigaContext _db;

        public PlatformEstimateDbService(DigaContext db)
            : base(db)
        {
            _db = db;
        }

        public async Task<PlatformEstimate> Get(int contractId, int userId)
        {
            return await _db.PlatformEstimates.OrderByDescending(e => e.VersionNumber)
                .FirstOrDefaultAsync(e => e.PlatformContractId == contractId && e.CreatorId == userId);
        }

        public async Task<PlatformEstimate> GetFull(int estimateId)
        {
            return await _db.PlatformEstimates.Include(e => e.PlatformEstimateVATs).ThenInclude(v => v.PlatformVAT).Include(e => e.PlatformContract).FirstOrDefaultAsync(e => e.Id == estimateId);
        }

        public async Task<PlatformEstimate> GetWithSections(int estimateId)
        {
            return await _db.PlatformEstimates.Include(e => e.PlatformEstimateVATs).Include(e => e.Sections).ThenInclude(s => s.Positions).ThenInclude(p => p.PlatformMeasurementReportPositions).FirstOrDefaultAsync(e => e.Id == estimateId);
        }

        public async Task<List<PlatformEstimate>> GetFull(List<int> estimateIds)
        {
            return await _db.PlatformEstimates.Include(e => e.PlatformEstimateVATs).Include(e => e.Creator)
                .Include(e => e.Sections).ThenInclude(s => s.Positions)
                .Where(e => estimateIds.Contains(e.Id)).ToListAsync();
        }

        public async Task<PlatformEstimate> GetActual(int contractId)
        {
            return (await _db.PlatformContracts.Include(c => c.MainEstimate).FirstOrDefaultAsync(c => c.Id == contractId)).MainEstimate;
            //PlatformContractBid bid = await _db.PlatformContractBids.FirstOrDefaultAsync(b =>
            //    b.PlatformContractId == contractId && b.Status == PlatformContractBidStatuses.Accepted);

            //if (bid != null)
            //{
            //    return await _db.PlatformEstimates.Include(e => e.PlatformEstimateVATs).Include(e => e.PlatformContract).OrderBy(e => e.VersionNumber)
            //        .LastOrDefaultAsync(e => e.PlatformContractId == contractId);
            //}
            //else
            //{
            //    return await _db.PlatformEstimates.Include(e => e.PlatformEstimateVATs).Include(e => e.PlatformContract).OrderBy(e => e.VersionNumber)
            //        .LastOrDefaultAsync(e => e.PlatformContractId == contractId && e.PlatformContract.UserId == e.CreatorId);
            //}
        }

        public async Task<PlatformEstimate> GetApprove(int estimateId)
        {
            return await _db.PlatformEstimates.Include(e => e.PlatformEstimateVATs).Include(e => e.PlatformContract)
                .ThenInclude(e => e.Bids).FirstOrDefaultAsync(e => e.Id == estimateId);
        }

        public async Task<PlatformEstimate> GetApproval(int estimateId)
        {
            return await _db.PlatformEstimates.Include(e => e.PlatformEstimateVATs).Include(e => e.PlatformContract).ThenInclude(e => e.Bids)
                .Include(e => e.Sections).ThenInclude(s => s.Positions)
                .FirstOrDefaultAsync(e => e.Id == estimateId);
        }

        public async Task<PlatformEstimate> GetActualByEstimateId(int estimateId)
        {
            return await _db.PlatformEstimates.Where(e => e.Id == estimateId || e.OriginalId == estimateId)
                .OrderBy(e => e.VersionNumber).LastOrDefaultAsync();
        }

        public async Task<PlatformEstimate> GetMyBidEstimate(int userId, int contractId)
        {
            return await _db.PlatformContracts.Include(c => c.PlatformEstimates)
                .Select(e => e.PlatformEstimates.FirstOrDefault(e => e.CreatorId == userId))
                .OrderByDescending(e => e.Id)
                .FirstOrDefaultAsync(c => c.PlatformContract.Id == contractId);
        }

        public async Task<List<int>> GetExecutorVersionIds(int contractId)
        {
            int executorId = (await _db.PlatformContracts.Include(c => c.Bids).SelectMany(b => b.Bids).FirstOrDefaultAsync(
                b => b.PlatformContractId == contractId && b.Status == PlatformContractBidStatuses.Accepted)).ApplicationUserId;

            List<PlatformEstimate> estimates = await _db.PlatformEstimates.Where(e => e.PlatformContractId == contractId)
                .OrderBy(e => e.Id).ToListAsync();
            List<int> result = new List<int> { };

            bool temp = false;
            foreach (PlatformEstimate estimate in estimates)
            {
                if (!temp && estimate.CreatorId == executorId)
                    temp = true;

                if (temp)
                    result.Add(estimate.Id);
            }

            return result;
        }

        public async Task<List<PlatformEstimate>> List(List<int> estimateIds)
        {
            return await _db.PlatformEstimates.Where(e => estimateIds.Contains(e.Id)).ToListAsync();
        }

        public async Task<List<PlatformEstimate>> ExecutorsList(int contractId)
        {
            int executorId = (await _db.PlatformContracts.Include(c => c.Bids).SelectMany(b => b.Bids).FirstOrDefaultAsync(
                b => b.PlatformContractId == contractId && b.Status == PlatformContractBidStatuses.Accepted)).ApplicationUserId;

            List<PlatformEstimate> estimates = await _db.PlatformEstimates.Where(e => e.PlatformContractId == contractId)
                .OrderBy(e => e.Id).ToListAsync();
            List<PlatformEstimate> result = new List<PlatformEstimate> { };

            bool temp = false;
            foreach (PlatformEstimate estimate in estimates)
            {
                if (!temp && estimate.CreatorId == executorId)
                    temp = true;

                if (temp)
                    result.Add(estimate);
            }

            return result;
        }
    }
}
