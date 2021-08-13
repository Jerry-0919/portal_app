using diga.bl.Models.Platform.Users;
using diga.dal.DbServices.DefaultDbServices;

namespace diga.dal.DbServices.PlatformUserPhoneNumberDbServices
{
    public interface IPlatformUserPhoneNumberDbService : IDefaultDbService<int, PlatformUserPhoneNumber>
    {
    }
}