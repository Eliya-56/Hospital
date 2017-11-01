using Microsoft.AspNet.Identity;
using MyHospital.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHospital.DAL.Identity
{
    public class MyHospitalRoleManager : RoleManager<MyHospitalRole>
    {
        public MyHospitalRoleManager(IRoleStore<MyHospitalRole, string> store) : base(store)
        {
        }
    }
}
