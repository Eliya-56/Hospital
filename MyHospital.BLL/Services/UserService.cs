using MyHospital.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyHospital.BLL.DTO;
using MyHospital.BLL.Infrastructure;
using System.Security.Claims;
using MyHospital.DAL.Entities;
using MyHospital.DAL.Interfaces;
using Microsoft.AspNet.Identity;

namespace MyHospital.BLL.Services
{
    public class UserService : IUserService
    {
        IUnitOfWork Database { get; set; }

        public UserService(IUnitOfWork uow)
        {
            Database = uow;
        }

        public async Task<ClaimsIdentity> Authenticate(UserDTO user)
        {
            ClaimsIdentity claim = null;
            MyHospitalUser dbUser = await Database.UserManager.FindAsync(user.Name, user.Password);
            if (dbUser != null)
                claim = await Database.UserManager.CreateIdentityAsync(dbUser, DefaultAuthenticationTypes.ApplicationCookie);
            return claim;
        }

        public async Task<OperationDetails> CreateAsync(UserDTO user)
        {
            MyHospitalUser dbUser = await Database.UserManager.FindByNameAsync(user.Name);
            if (dbUser == null)
            {
                if (user is DoctorDTO)
                {
                    dbUser = new Doctor { Email = user.Email, UserName = user.Name, Specialization = ((DoctorDTO)user).Specialization, RegistrationDate = DateTime.UtcNow };
                }
                else if (user is PatientDTO)
                {
                    dbUser = new Patient { Email = user.Email, UserName = user.Name, TaxCode = ((PatientDTO)user).TaxCode, RegistrationDate = DateTime.UtcNow };
                }
                else
                    dbUser = new MyHospitalUser { Email = user.Email, UserName = user.Name, RegistrationDate = DateTime.UtcNow };
                var result = await Database.UserManager.CreateAsync(dbUser, user.Password);
                if (result.Errors.Count() > 0)
                    return new OperationDetails(false, result.Errors.FirstOrDefault(), "");
                await Database.UserManager.AddToRoleAsync(dbUser.Id, user.Role);
                await Database.SaveAsync();
                return new OperationDetails(true, "Регистрация успешно пройдена", "");
            }
            else
            {
                return new OperationDetails(false, "Пользователь с таким логином уже существует", "UserName");
            }
        }

        public void Dispose()
        {
            Database.Dispose();
        }

        public IEnumerable<UserDTO> GetAllUsers()
        {
            List<UserDTO> users = new List<UserDTO>();
            foreach (var user in Database.UserManager.Users.ToList())
            {
                var role = Database.RoleManager.FindById(user.Roles.First().RoleId).Name;
                UserDTO userDTO;
                if (user is Doctor)
                {
                    userDTO = new DoctorDTO { Id = user.Id, Email = user.Email, Name = user.UserName, Role = role, Specialization = ((Doctor)user).Specialization, RegistrationDate = user.RegistrationDate };
                }
                else if (user is Patient)
                {
                    userDTO = new PatientDTO { Id = user.Id, Email = user.Email, Name = user.UserName, Role = role, TaxCode = ((Patient)user).TaxCode, RegistrationDate = user.RegistrationDate };
                }
                else
                    userDTO = new UserDTO { Id = user.Id, Email = user.Email, Name = user.UserName, Role = role, RegistrationDate = user.RegistrationDate };
                users.Add(userDTO);
            }
            return users;
        }

        public UserDTO GetUsersById(string Id)
        {
            MyHospitalUser dbUser = Database.UserManager.FindById(Id);
            var role = Database.RoleManager.FindById(dbUser.Roles.First().RoleId).Name;
            if (dbUser == null)
            {
                return null;
            }
            else
            {
                if (dbUser is Doctor)
                {
                    return new DoctorDTO { Email = dbUser.Email, Id = dbUser.Id, Name = dbUser.UserName, Role = role, Specialization = ((Doctor)dbUser).Specialization, RegistrationDate = dbUser.RegistrationDate };
                }
                else if (dbUser is Patient)
                {
                    return new PatientDTO { Email = dbUser.Email, Id = dbUser.Id, Name = dbUser.UserName, Role = role, TaxCode = ((Patient)dbUser).TaxCode, RegistrationDate = dbUser.RegistrationDate };
                }
                else
                    return new UserDTO { Email = dbUser.Email, Id = dbUser.Id, Name = dbUser.UserName, Role = role, RegistrationDate = dbUser.RegistrationDate };
            }
        }

        public async Task SetInitialData(List<UserDTO> users, List<string> roles)
        {
            foreach (string roleName in roles)
            {
                var role = await Database.RoleManager.FindByNameAsync(roleName);
                if (role == null)
                {
                    role = new MyHospitalRole { Name = roleName };
                    await Database.RoleManager.CreateAsync(role);
                }
            }
            foreach (var user in users)
            {
                await CreateAsync(user);
            }
        }

        public async Task<OperationDetails> UpdateAsync(UserDTO user)
        {
            MyHospitalUser dbUser = await Database.UserManager.FindByIdAsync(user.Id);
            if(dbUser == null)
            {
                return new OperationDetails(false, "Пользователя с таким логином не существует", "UserName");
            }
            else
            {
                if(user is DoctorDTO)
                {
                    dbUser.UserName = user.Name;
                    ((Doctor)dbUser).Specialization = ((DoctorDTO)user).Specialization;
                    dbUser.Email = user.Email;
                }
                else if(user is PatientDTO)
                {
                    dbUser.UserName = user.Name;
                    ((Patient)dbUser).TaxCode = ((PatientDTO)user).TaxCode;
                    dbUser.Email = user.Email;
                }
                await Database.UserManager.UpdateAsync(dbUser);
                await Database.SaveAsync();
                return new OperationDetails(true, "Пользователь успешно обновлён", "");
            }
        }

        public bool IsInRole(string id, string role)
        {
            return Database.UserManager.IsInRole(id, role);
        }
    }
}
