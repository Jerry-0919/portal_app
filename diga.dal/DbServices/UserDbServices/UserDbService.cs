using diga.bl.Constants;
using diga.bl.Models;
using diga.dal.DbServices.DefaultDbServices;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diga.dal.DbServices.UserDbServices
{
    public class UserDbService : DefaultDbService<int, ApplicationUser>, IUserDbService
    {
        private readonly DigaContext _db;

        public UserDbService(DigaContext db)
            : base(db)
        {
            _db = db;
        }

        public async Task<bool> Any(string email)
        {
            return await _db.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<ApplicationUser> Get(string email)
        {
            return await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<ApplicationUser> GetWithVerifications(int id)
        {
            return await _db.Users.Include(u => u.PlatformUserVerifications).ThenInclude(v => v.Photos)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<ApplicationUser> GetByPasswordCode(string code)
        {
            return await _db.Users.FirstOrDefaultAsync(u => u.PasswordCode == code);
        }

        public async Task<ApplicationUser> GetFull(string email)
        {
            return await _db.Users.Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<ApplicationUser> GetFull(int id)
        {
            return await _db.Users.Include(u => u.UserRoles).ThenInclude(ur => ur.Role).Include(u => u.Nationality).Include(u => u.ResidenceCountry).Include(u => u.PlatformMangoPayUserBankAccounts)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<ApplicationUser> GetProfile(int id)
        {
            return await _db.Users.Include(u => u.PhoneNumbers).Include(u => u.Languages).ThenInclude(l => l.PlatformLanguage).Include(u => u.Nationality).Include(u => u.PlatformMangoPayUserBankAccounts)
                .Include(u => u.PlatformUserVerifications)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<List<ApplicationUser>> List(int skip, int take)
        {
            return await _db.Users.Skip(skip).Take(take).ToListAsync();
        }

        public async Task<List<ApplicationUser>> ListByRole(string roleName)
        {
            return await _db.Users.Where(u => u.UserRoles.Any(ur => ur.Role.Name == roleName)).ToListAsync();
        }

        private IQueryable<ApplicationUser> GetVerificationQuery(bool verificationRequested)
        {
            IQueryable<ApplicationUser> query = _db.Users.Include(u => u.PlatformUserVerifications).ThenInclude(u => u.Photos);

            if (verificationRequested)
                query = query.Where(q => q.PlatformUserVerifications.OrderBy(v => v.Created)
                    .First().Status == PlatformUserVerificationStatuses.UnderConsideration);

            return query;
        }

        public async Task<List<ApplicationUser>> ListWithVerifications(int skip, int take,
            bool verificationRequested = false)
        {
            return await GetVerificationQuery(verificationRequested).Skip(skip).Take(take).ToListAsync();
        }

        public async Task<int> Count(bool verificationRequested = false)
        {
            return await GetVerificationQuery(verificationRequested).CountAsync();
        }
    }
}
