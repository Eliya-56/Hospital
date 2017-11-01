using MyHospital.BLL.DTO;
using MyHospital.BLL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MyHospital.BLL.Interfaces
{
    public interface IUserService : IDisposable
    {
        Task<OperationDetails> CreateAsync(UserDTO user);
        Task<OperationDetails> UpdateAsync(UserDTO user);
        IEnumerable<UserDTO> GetAllUsers();
        UserDTO GetUsersById(string Id);
        Task<ClaimsIdentity> Authenticate(UserDTO user);
        bool IsInRole(string id, string role);
        Task SetInitialData(List<UserDTO> users, List<string> roles);
    }
}
