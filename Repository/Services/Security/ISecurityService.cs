using Model.DTOs;
using System.Threading.Tasks;

namespace Repository.Services.Security
{
    public interface ISecurityService
    {
        Task<object> Authenticate(LoginDto request);
    }
}
