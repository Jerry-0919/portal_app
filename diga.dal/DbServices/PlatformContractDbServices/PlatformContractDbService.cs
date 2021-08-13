using diga.bl.Constants;
using diga.bl.Enums.PlatformContracts;
using diga.bl.Models;
using diga.bl.Models.Platform.Contracts;
using diga.dal.DbServices.DefaultDbServices;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diga.dal.DbServices.PlatformContractDbServices
{
    public class PlatformContractDbService : DefaultDbService<int, PlatformContract>, IPlatformContractDbService
    {
        private readonly DigaContext _db;

        public PlatformContractDbService(DigaContext db)
            : base(db)
        {
            _db = db;
        }

        public async Task<bool> Any(int contractId, int userId)
        {
            return await _db.PlatformContracts.AnyAsync(c => c.Id == contractId && c.UserId == userId);
        }

        public async Task<PlatformContract> GetFull(int contractId)
        {
            return await _db.PlatformContracts
                .Include(c => c.User)
                .Include(c => c.Approval)
                .Include(c => c.ContractType)
                .Include(pc => pc.Categories).ThenInclude(c => c.PlatformCategory)
                .Include(pc => pc.Tags).ThenInclude(t => t.PlatformTag)
                .Include(pc => pc.Files)
                .Include(pc => pc.PlatformEstimates)
                .Include(c => c.City)
                .FirstOrDefaultAsync(c => c.Id == contractId);
        }

        public async Task<int> Count(int userId, string filter, List<string> statuses)
        {
            IQueryable<PlatformContract> query = _db.PlatformContracts.Include(c => c.ContractType)
                .Include(c => c.Categories).ThenInclude(c => c.PlatformCategory)
                .Where(c => c.UserId == userId && (c.Name.ToLower().Contains(filter.ToLower()) || c.Description.ToLower().Contains(filter)));

            if (statuses.Count != 0)
                query = query.Where(c => statuses.Contains(c.Status));

            return await query.CountAsync();
        }

        public async Task<List<PlatformContract>> List(int userId, int skip, int take, string filter, List<string> statuses, SortType sortType, bool isActual)
        {
            IQueryable<PlatformContract> query = _db.PlatformContracts.Include(c => c.ContractType)
                .Include(c => c.Bids)
                .Include(c => c.Categories).ThenInclude(c => c.PlatformCategory)
                .Include(c => c.PlatformChatRooms)
                .Where(c => c.UserId == userId || c.Bids.Any(b => b.ApplicationUserId == userId));

            if (!string.IsNullOrEmpty(filter))
            {
                string queryFilter = filter.ToLower();
                query = query.Where(c => c.Name.ToLower().Contains(queryFilter) || c.Description.ToLower().Contains(filter));
            }

            if (statuses.Count != 0)
                query = query.Where(c => statuses.Contains(c.Status));

            if (sortType == SortType.DateAsc)
                query = query.OrderBy(c => c.CreatedDate);
            else if (sortType == SortType.DateDesc)
                query = query.OrderByDescending(c => c.CreatedDate);

            if (isActual == true)
            {
                query = query.Where(c => c.VersionStatus == "Actual");
            }

            return await query.Skip(skip).Take(take).ToListAsync();
        }

        public async Task<int> CountPublished(int userId, string filter, List<int> categories, List<string> tags,
            bool hideMyBidsProjects, int? balanceId)
        {
            IQueryable<PlatformContract> query = _db.PlatformContracts.Include(c => c.Categories)
                .Where(c => c.Status == PlatformContractStatuses.Contractor);

            if (!string.IsNullOrEmpty(filter))
            {
                filter = filter.ToLower();
                query = query.Where(c => c.Name.ToLower().Contains(filter) || c.Description.ToLower().Contains(filter));
            }

            if (categories.Count != 0)
                query = query.Where(c => c.Categories.Any(c => categories.Contains(c.PlatformCategoryId)));

            if (tags.Count != 0)
                query = query.Where(c => c.Tags.All(t => tags.Contains(t.PlatformTag.Name)));

            if (hideMyBidsProjects)
                query = query.Where(c => !c.Bids.Any(b => b.ApplicationUserId == userId));

            if (balanceId.HasValue)
                query = query.Where(c => c.BalanceId == balanceId);

            return await query.CountAsync();
        }

        public async Task<List<PlatformContract>> ListPublished(int userId, int skip, int take, string filter,
            List<int> categories, List<string> tags, bool hideMyBidsProjects, int? balanceId, SortType sortType)
        {
            IQueryable<PlatformContract> query = _db.PlatformContracts
                .Include(pc => pc.User).ThenInclude(u => u.Ratings)
                .Include(pc => pc.User).ThenInclude(u => u.Reviews).ThenInclude(r => r.Marks)
                .Include(pc => pc.PlatformEstimates)
                .Include(pc => pc.Categories)
                .Include(pc => pc.Bids)
                .Include(pc => pc.Discussions)
                .Include(pc => pc.Tags).ThenInclude(t => t.PlatformTag)
                .Where(c => c.Status == PlatformContractStatuses.Contractor);

            if (!string.IsNullOrEmpty(filter))
                query = query.Where(c => c.Name.ToLower().Contains(filter) || c.Description.ToLower().Contains(filter));

            if (categories.Count != 0)
                query = query.Where(c => c.Categories.Any(c => categories.Contains(c.PlatformCategoryId)));

            if (tags.Count != 0)
                query = query.Where(c => c.Tags.All(t => tags.Contains(t.PlatformTag.Name)));

            if (hideMyBidsProjects)
                query = query.Where(c => !c.Bids.Any(b => b.ApplicationUserId == userId));

            if (balanceId.HasValue)
                query = query.Where(c => c.BalanceId == balanceId);

            if (sortType == SortType.DateAsc)
                query = query.OrderBy(c => c.UpdatedDate);
            else if (sortType == SortType.DateDesc)
                query = query.OrderByDescending(c => c.UpdatedDate);

            return await query.Skip(skip).Take(take).ToListAsync();
        }

        public async Task<List<PlatformContract>> List(int skip, int take)
        {
            return await _db.PlatformContracts.Include(pc => pc.City).Include(pc => pc.ContractPriority)
                .Include(pc => pc.ContractType).Include(pc => pc.User).Include(pc => pc.PlatformEstimates)
                .Include(pc => pc.Categories).ThenInclude(c => c.PlatformCategory)
                .Skip(skip).Take(take).ToListAsync();
        }

        public async Task<int> GetLastVersion(int contractId)
        {
            return await _db.PlatformContracts.Where(c => c.Id == contractId || c.OriginalId == contractId).MaxAsync(c => c.VersionNumber);
        }

        public async Task UpdateNotActual(int contractId)
        {
            List<PlatformContract> contracts = await _db.PlatformContracts.Where(c => c.Id == contractId || c.OriginalId == contractId).ToListAsync();
            foreach (PlatformContract contract in contracts)
            {
                contract.VersionStatus = PlatformContractVersionStatuses.NotActual;
            }

            _db.PlatformContracts.UpdateRange(contracts);
            await _db.SaveChangesAsync();
        }

        public async Task<int> CountVersions(int contractId)
        {
            return await _db.PlatformContracts.CountAsync(c => c.Id == contractId || c.OriginalId == contractId);
        }

        public async Task<List<PlatformContract>> ListVersions(int contractId, int skip, int take)
        {
            return await _db.PlatformContracts
                .Include(pc => pc.ContractType)
                .Include(pc => pc.Categories).ThenInclude(c => c.PlatformCategory)
                .Where(c => c.Id == contractId || c.OriginalId == contractId)
                .Skip(skip).Take(take).ToListAsync();
        }

        public async Task<bool> IsCustomerOrExecutor(int contractId, int userId)
        {
            return await _db.PlatformContracts.Include(c => c.Bids).AnyAsync(c => c.Id == contractId && (c.UserId == userId
            || c.Bids.Any(b => b.ApplicationUserId == userId && b.Status == PlatformContractBidStatuses.Accepted)));
        }

        public async Task<bool> IsCustomer(int contractId, int userId)
        {
            return await _db.PlatformContracts.Include(c => c.Bids).AnyAsync(c => c.Id == contractId && c.UserId == userId);
        }

        public async Task<bool> IsExecutor(int contractId, int userId)
        {
            return await _db.PlatformContracts.Include(c => c.Bids).AnyAsync(c => c.Id == contractId &&
                c.Bids.Any(b => b.ApplicationUserId == userId && b.Status == PlatformContractBidStatuses.Accepted));
        }

        public async Task<ApplicationUser> GetExecutor(int contractId)
        {
            return (await _db.PlatformContractBids.Include(b => b.ApplicationUser).FirstOrDefaultAsync(b => b.PlatformContractId == contractId &&
                b.Status == PlatformContractBidStatuses.Accepted))?.ApplicationUser;
        }

        public async Task<PlatformContract> GetApproval(int contractId)
        {
            return await _db.PlatformContracts.Include(c => c.User).Include(c => c.Bids)
                .Include(c => c.Parts).Include(c => c.Approval)
                .FirstOrDefaultAsync(c => c.Id == contractId);
        }

        public async Task<PlatformContract> GetWithEstimates(int contractId)
        {
            return await _db.PlatformContracts.Include(c => c.PlatformEstimates)
                .ThenInclude(e => e.Sections).ThenInclude(s => s.Positions)
                .FirstOrDefaultAsync(c => c.Id == contractId);
        }

        public async Task<int> CountFinished(int userId)
        {
            return await _db.PlatformContracts.Include(c => c.Bids)
                .Where(c => c.Status == PlatformContractStatuses.Finished && (c.Bids.Any(b => b.Status == PlatformContractBidStatuses.Accepted && b.ApplicationUserId == userId) || c.UserId == userId)).CountAsync();
        }

        public async Task<int> CountCurrent(int userId)
        {
            return await _db.PlatformContracts.Include(c => c.Bids)
                .Where(c => c.Status == PlatformContractStatuses.Executing && (c.Bids.Any(b => b.Status == PlatformContractBidStatuses.Accepted && b.ApplicationUserId == userId) || c.UserId == userId)).CountAsync();
        }
    }
}
