using MyHospital.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyHospital.DAL.Identity;
using MyHospital.DAL.EF;
using MyHospital.DAL.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace MyHospital.DAL.Repositories
{
    public class MyHospitalUnitOfWork : IUnitOfWork
    {
        private MyHospitalContext _context;

        private MyHospitalUserManager _userManager;
        private MyHospitalRoleManager _roleManager;
        private IDeseaseManager _deseaseManager;

        public MyHospitalUnitOfWork(string connectionString)
        {
            _context = new MyHospitalContext(connectionString);
            _userManager = new MyHospitalUserManager(new UserStore<MyHospitalUser>(_context));
            _roleManager = new MyHospitalRoleManager(new RoleStore<MyHospitalRole>(_context));
            _deseaseManager = new DeseaseManager(_context);
        }

        public MyHospitalUserManager UserManager
        {
            get { return _userManager; }
        }

        public MyHospitalRoleManager RoleManager
        {
            get { return _roleManager; }
        }

        public IDeseaseManager DeseaseManager
        {
            get { return _deseaseManager; }
        }

        public void Dispose()
        {
            _deseaseManager.Dispose();
            _roleManager.Dispose();
            _userManager.Dispose();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
