using diga.bl.Models.Platform.Contracts;
using diga.dal.DbServices.DefaultDbServices;

namespace diga.dal.DbServices.PlatformContractReviewMarkDbServices
{
    public class PlatformContractReviewMarkDbService : DefaultDbService<int, PlatformContractReviewMark>, IPlatformContractReviewMarkDbService
    {
        private readonly DigaContext _db;

        public PlatformContractReviewMarkDbService(DigaContext db)
            : base(db)
        {
            _db = db;
        }
    }
}
