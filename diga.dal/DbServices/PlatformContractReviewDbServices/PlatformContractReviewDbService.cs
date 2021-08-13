using diga.bl.Constants;
using diga.bl.Models.Platform.Contracts;
using diga.bl.OutModels.PlatformContractReviews;
using diga.dal.DbServices.DefaultDbServices;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diga.dal.DbServices.PlatformContractReviewDbServices
{
    public class PlatformContractReviewDbService : DefaultDbService<int, PlatformContractReview>, IPlatformContractReviewDbService
    {
        private readonly DigaContext _db;

        public PlatformContractReviewDbService(DigaContext db)
            : base(db)
        {
            _db = db;
        }

        public async Task<int> CustomerCount(int userId, int categoryId)
        {
            IQueryable<PlatformContractReview> query =
                _db.PlatformContractReviews.Include(r => r.PlatformContract).ThenInclude(c => c.Categories);

            if (categoryId != 0)
                query = query.Where(r => r.PlatformContract.Categories.Any(c => c.PlatformCategoryId == categoryId));

            return await query.CountAsync(r => r.PlatformContract.UserId == userId);
        }

        public async Task<List<PlatformContractReview>> CustomerList(int userId, int skip, int take, int categoryId)
        {
            IQueryable<PlatformContractReview> query = _db.PlatformContractReviews
                .Include(r => r.PlatformContract).ThenInclude(c => c.Categories).ThenInclude(c => c.PlatformCategory)
                .Include(r => r.ApplicationUser).Include(r => r.Marks).Include(r => r.Documents)
                .Where(r => r.PlatformContract.UserId == userId).Skip(skip).Take(take);

            if (categoryId != 0)
                query = query.Where(r => r.PlatformContract.Categories.Any(c => c.PlatformCategoryId == categoryId));

            return await query.ToListAsync();
        }

        public async Task<int> ExecutorCount(int userId, int categoryId)
        {
            IQueryable<PlatformContractReview> query = _db.PlatformContractReviews.Include(r => r.PlatformContract).ThenInclude(c => c.Bids);

            if (categoryId != 0)
                query = query.Where(r => r.PlatformContract.Categories.Any(c => c.PlatformCategoryId == categoryId));

            return await query.CountAsync(r => r.PlatformContract.Bids.Any(b => b.ApplicationUserId == userId && b.Status == PlatformContractBidStatuses.Accepted));
        }

        public async Task<List<PlatformContractReview>> ExecutorList(int userId, int skip, int take, int categoryId)
        {
            IQueryable<PlatformContractReview> query = _db.PlatformContractReviews
                .Include(r => r.PlatformContract).ThenInclude(c => c.Categories).ThenInclude(c => c.PlatformCategory)
                .Include(r => r.PlatformContract).ThenInclude(c => c.Bids)
                .Include(r => r.ApplicationUser).Include(r => r.Marks)
                .Where(r => r.PlatformContract.Bids.Any(b => b.ApplicationUserId == userId && b.Status == PlatformContractBidStatuses.Accepted))
                .Skip(skip).Take(take);

            if (categoryId != 0)
                query = query.Where(r => r.PlatformContract.Categories.Any(c => c.PlatformCategoryId == categoryId));

            return await query.ToListAsync();
        }

        public async Task<List<PlatformContractReview>> List(int userId)
        {
            return await _db.PlatformContractReviews.Include(r => r.PlatformContract).Include(r => r.Marks)
                .Where(r => r.PlatformContract.UserId == userId).ToListAsync();
        }

        public async Task<(int, int)> Count(int userId)
        {
            List<bool> reviews = await _db.PlatformContractReviews.Include(r => r.PlatformContract).ThenInclude(c => c.Bids).Include(r => r.Marks)
                .Where(r => r.ApplicationUserId == userId /*||
                        r.PlatformContract.Bids.Any(b => b.ApplicationUserId == userId && b.Status == PlatformContractBidStatuses.Accepted)*/)
                .Select(r => r.Marks.Average(m => m.Value) >= 2.5)
                .ToListAsync();

            return (reviews.Count(r => r), reviews.Count(r => !r));
        }

        public async Task<List<PlatformContractReviewUser>> Count(List<int> userIds)
        {
            return await _db.PlatformContractReviews.Include(r => r.PlatformContract).ThenInclude(c => c.Bids).Include(r => r.Marks)
                .Where(r => userIds.Contains(r.PlatformContract.UserId) ||
                r.PlatformContract.Bids.Any(b => userIds.Contains(b.ApplicationUserId) && b.Status == PlatformContractBidStatuses.Accepted))
                .Select(r => new PlatformContractReviewUser
                {
                    CustomerId = r.PlatformContract.UserId,
                    ExecutorId = r.PlatformContract.Bids.First(b => b.Status == PlatformContractBidStatuses.Accepted).ApplicationUserId,
                    IsGoodReview = r.Marks.Average(m => m.Value) >= 2.5
                }).ToListAsync();
        }

        public async Task<bool> Any(int contractId, int userId)
        {
            return await _db.PlatformContractReviews.AnyAsync(r => r.PlatformContractId == contractId && r.ApplicationUserId == userId);
        }

        public async Task<PlatformContractReview> Get(int contractId, int userId)
        {
            return await _db.PlatformContractReviews.Include(r => r.Documents).Include(r => r.Marks).Include(r => r.PlatformContract).ThenInclude(pc => pc.Categories).ThenInclude(c => c.PlatformCategory).Include(r => r.ApplicationUser)
                .FirstOrDefaultAsync(r => r.PlatformContractId == contractId && r.ApplicationUserId == userId);
        }

        public async Task<List<PlatformContractReview>> GetAll(int userId)
        {
            return await _db.PlatformContractReviews.Include(r => r.Documents).Include(r => r.Marks).Where(r => r.ApplicationUserId == userId)
                .ToListAsync();
        }

        public async Task<List<PlatformContractReview>> AllList(int userId, int skip, int take)
        {
            return await _db.PlatformContractReviews
                .Include(r => r.PlatformContract).ThenInclude(c => c.Categories).ThenInclude(c => c.PlatformCategory)
                .Include(r => r.PlatformContract).ThenInclude(c => c.Bids)
                .Include(r => r.ApplicationUser).Include(r => r.Marks)
                .Where(r => r.PlatformContract.UserId == userId || r.PlatformContract.Bids.Any(b => b.ApplicationUserId == userId && b.Status == PlatformContractBidStatuses.Accepted))
                .OrderByDescending(r => r.PublishDate)
                .Skip(skip).Take(take).ToListAsync();
        }

        public async Task<int> AllCount(int userId)
        {
            return await _db.PlatformContractReviews.Include(r => r.PlatformContract).ThenInclude(c => c.Bids).CountAsync(r => r.PlatformContract.UserId == userId || r.PlatformContract.Bids.Any(b => b.ApplicationUserId == userId && b.Status == PlatformContractBidStatuses.Accepted));
        }
    }
}
