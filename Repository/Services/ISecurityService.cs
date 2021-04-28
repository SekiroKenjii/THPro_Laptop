using Model.DTOs;
using System.Threading.Tasks;

namespace Repository.Services
{
    public interface ISecurityService
    {
        Task<object> Authenticate(LoginDto request);
    }
}
