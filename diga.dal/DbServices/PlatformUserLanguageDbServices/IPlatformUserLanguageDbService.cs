using diga.bl.Models.Platform.Users;
using diga.dal.DbServices.ManyToManyDbServices;

namespace diga.dal.DbServices.PlatformUserLanguageDbServices
{
    public interface IPlatformUserLanguageDbService : IManyToManyDbService<PlatformUserLanguage>
    {
    }
}