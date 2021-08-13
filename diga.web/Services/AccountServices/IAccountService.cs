using diga.web.Models.Account;
using diga.web.Models.Status;
using System.Threading.Tasks;

namespace diga.web.Services.AccountServices
{
    public interface IAccountService
    {
        Task<ResponseData<SignUpResponseDto>> SignUp(SignUpRequestDto request);
        Task<ResponseData> ForgotPassword(PasswordForgotRequestDto request);
        Task<ResponseData> CheckForgotPassword(PasswordForgotCheckRequestDto request);
    }
}
