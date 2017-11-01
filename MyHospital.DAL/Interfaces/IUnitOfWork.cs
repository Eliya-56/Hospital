using MyHospital.DAL.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHospital.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        MyHospitalUserManager UserManager { get; }
        MyHospitalRoleManager RoleManager { get; }
        IDeseaseManager DeseaseManager { get; }
        Task SaveAsync();
    }
}
