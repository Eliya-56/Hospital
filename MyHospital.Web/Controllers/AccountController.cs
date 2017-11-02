using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using MyHospital.BLL.DTO;
using MyHospital.BLL.Infrastructure;
using MyHospital.BLL.Interfaces;
using MyHospital.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MyHospital.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private IUserService UserService
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<IUserService>();
            }
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private IDeseaseService DeseaseService
        {
            get;
        }

        public int PageSize { get; private set; }

        public AccountController(IDeseaseService DeseaseService)
        {
            this.DeseaseService = DeseaseService;
            PageSize = 7;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Register()
        {
            return PartialView();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegistrationModel Model)
        {
            await SetInitialDataAsync();
            if(Model.Role == "doctor" && string.IsNullOrWhiteSpace(Model.Specialization))
            {
                ModelState.AddModelError("Specialization", "Укажите специализацию");
            }
            if (Model.Role == "patient" && Model.TaxCode == null)
            {
                ModelState.AddModelError("Specialization", "Укажите идентификационный код");
            }
            if (ModelState.IsValid)
            {
                UserDTO userDto;
                if (Model.Role == "doctor")
                    userDto = new DoctorDTO
                    {
                        Email = Model.Email,
                        Password = Model.Password,
                        Name = Model.Name,
                        Role = Model.Role,
                        Specialization = Model.Specialization
                    };
                else
                    userDto = new PatientDTO
                    {
                        Email = Model.Email,
                        Password = Model.Password,
                        Name = Model.Name,
                        Role = Model.Role,
                        TaxCode = (int)Model.TaxCode
                    };

                OperationDetails operationDetails = await UserService.CreateAsync(userDto);
                if (operationDetails.Succedeed)
                {
                    return RedirectToAction("StartPage", "Home");
                }
                else
                    ModelState.AddModelError(operationDetails.Property, operationDetails.Message);
            }
            return View(Model);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            return PartialView();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel Model)
        {
            await SetInitialDataAsync();
            if (ModelState.IsValid)
            {
                UserDTO userDto = new UserDTO { Name = Model.Name, Password = Model.Password };
                ClaimsIdentity claim = await UserService.Authenticate(userDto);
                if (claim == null)
                {
                    ModelState.AddModelError("", "Неверный логин или пароль.");
                }
                else
                {
                    AuthenticationManager.SignOut();
                    AuthenticationManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = true
                    }, claim);
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(Model);
        }

        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("StartPage", "Home");
        }

        private async Task SetInitialDataAsync()
        {
            await UserService.SetInitialData(
                new List<UserDTO>
                {
                    new UserDTO
                    {
                        Name = "Admin",
                        Password = "123Zxc",
                        Role = "admin"
                    },
                    new DoctorDTO
                    {
                        Name = "Doctor1",
                        Password = "123Zxc",
                        Role = "doctor",
                        Specialization = "Хирург"
                    },
                    new DoctorDTO
                    {
                        Name = "Doctor2",
                        Password = "123Zxc",
                        Role = "doctor",
                        Specialization = "Терапевт"
                    },
                    new PatientDTO
                    {
                        Name = "Patient1",
                        Password = "123Zxc",
                        Role = "patient",
                        TaxCode = 111
                    },
                    new PatientDTO
                    {
                        Name = "Patient2",
                        Password = "123Zxc",
                        Role = "patient",
                        TaxCode = 222
                    }
                }
                , new List<string> { "doctor", "admin", "patient" }
                );
            DeseaseService.SetInitialData(
                new List<string> { "Прибыл", "Больной", "Здоровый" }
                );
        }

        public ActionResult UserProfile()
        {
            ViewBag.Role = GetRole();
            var Id = User.Identity.GetUserId();
            var userData = UserService.GetUsersById(Id);
            return View(userData);
        }

        [HttpGet]
        public ActionResult UpdateProfile()
        {
            var updateModel = new UpdateProfileModel();
            updateModel.Id = User.Identity.GetUserId();
            var userData = UserService.GetUsersById(updateModel.Id);
            updateModel.Name = userData.Name;
            updateModel.Email = userData.Email;
            updateModel.Role = userData.Role.ToString();
            if (userData is PatientDTO)
                ViewBag.TaxCode = ((PatientDTO)userData).TaxCode;
            if (userData is DoctorDTO)
                ViewBag.Specialization = ((DoctorDTO)userData).Specialization;
            return PartialView(updateModel);
        }

        [HttpPost]
        public async Task<ActionResult> UpdateProfile(UpdateProfileModel Model)
        {
            var specialization = Request.Params["Specialization"];
            var taxCode = Request.Params["TaxCode"];
            if (Model.Role == "doctor" && string.IsNullOrWhiteSpace(specialization))
            {
                ModelState.AddModelError("Specialization", "Укажите специализацию");
            }
            if (Model.Role == "patient" && string.IsNullOrWhiteSpace(taxCode))
            {
                ModelState.AddModelError("Specialization", "Укажите идентификационный код");
            }
            if (ModelState.IsValid)
            {
                UserDTO userDto;
                if (Model.Role == "doctor")
                    userDto = new DoctorDTO
                    {
                        Id = Model.Id,
                        Email = Model.Email,
                        Name = Model.Name,
                        Role = Model.Role,
                        Specialization = specialization
                    };
                else
                    userDto = new PatientDTO
                    {
                        Id = Model.Id,
                        Email = Model.Email,
                        Name = Model.Name,
                        Role = Model.Role,
                        TaxCode = int.Parse(taxCode)
                    };
                OperationDetails operationDetails = await UserService.UpdateAsync(userDto);
                if (operationDetails.Succedeed)
                {
                    return PartialView("SuccessUpdateProfile");
                }
                else
                    ModelState.AddModelError(operationDetails.Property, operationDetails.Message);
            }
            return PartialView(Model);
        }
        
        [HttpGet]
        public ActionResult Doctors(int Page = 0,string Name = "")
        {
            ViewBag.Role = GetRole();
            IEnumerable<DoctorDTO> doctors;
            if (string.IsNullOrWhiteSpace(Name))
                doctors = UserService.GetAllUsers().OfType<DoctorDTO>();
            else
                doctors = UserService.GetAllUsers().OfType<DoctorDTO>().Where(x => x.Name == Name);
            ViewBag.Page = Page;
            var count = doctors.Count();
            ViewBag.PageCount = count / PageSize;
            if (count % PageSize != 0)
                ViewBag.PageCount += 1;
            ViewBag.Name = Name;
            return View(doctors.Skip(Page * PageSize).Take(PageSize));
        }

        [HttpGet]
        [Authorize(Roles = "admin, doctor")]
        public ActionResult Patients(int Page = 0, string Name = "")
        {
            ViewBag.Role = GetRole();
            IEnumerable<PatientDTO> patients;
            if (string.IsNullOrWhiteSpace(Name))
                patients = UserService.GetAllUsers().OfType<PatientDTO>();
            else
                patients = UserService.GetAllUsers().OfType<PatientDTO>().Where(x => x.Name == Name);
            ViewBag.Page = Page;
            var count = patients.Count();
            ViewBag.PageCount = count / PageSize;
            if (count % PageSize != 0)
                ViewBag.PageCount += 1;
            ViewBag.Name = Name;
            return View(patients.Skip(Page * PageSize).Take(PageSize));
        }

        private string GetRole()
        {
            var userId = User.Identity.GetUserId();
            if (UserService.IsInRole(userId, "doctor"))
                return "doctor";
            else if (UserService.IsInRole(userId, "patient"))
                return "patient";
            else if (UserService.IsInRole(userId, "admin"))
                return "admin";
            else
                return "norole";
        }
    }
}