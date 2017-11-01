using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using MyHospital.BLL.DTO;
using MyHospital.BLL.Infrastructure;
using MyHospital.BLL.Interfaces;
using MyHospital.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MyHospital.Web.Controllers
{
    [Authorize]
    public class DeseaseController : Controller
    {
        private IUserService UserService
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<IUserService>();
            }
        }

        private IDeseaseService DeseaseService
        {
            get;
        }

        public DeseaseController(IDeseaseService DeseaseService)
        {
            this.DeseaseService = DeseaseService;
            //ServiceCreator = new ServiceCreator();
            PageSize = 7;
        }

        public int PageSize { get; private set; }

        [HttpGet]
        public ActionResult Deseases(int Page = 0, string Name = "")
        {
            ViewBag.Role = GetRole();
            IEnumerable<DeseaseDTO> deseases;
            if (string.IsNullOrWhiteSpace(Name))
                deseases = DeseaseService.GetAllDesease(User.Identity.GetUserId());
            else
                deseases = DeseaseService.GetAllDesease().Where(x => x.Name == Name);
            ViewBag.Page = Page;
            var count = deseases.Count();
            ViewBag.PageCount = count / PageSize;
            if (count % PageSize != 0)
                ViewBag.PageCount += 1;
            ViewBag.Name = Name;
            return View(deseases.Skip(Page * PageSize).Take(PageSize));
        }

        [HttpGet]
        [Authorize(Roles = "patient")]
        public ActionResult CreateDesease(string DoctorId)
        {
            ViewBag.DoctorId = DoctorId;
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "patient")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateDesease(CreateDeseaseModel Model)
        {
            var patientId = User.Identity.GetUserId();
            if (ModelState.IsValid)
            {
                OperationDetails operationDetails = await DeseaseService.CreateAsync(patientId, Model.DoctorId, Model.Complaint);
                if (operationDetails.Succedeed)
                {
                    return PartialView("SuccessCreateDesease");
                }
                else
                    ModelState.AddModelError(operationDetails.Property, operationDetails.Message);
            }
            return View(Model);
        }
        
        [HttpGet]
        public ActionResult UpdateDesease(int Id, string Message)
        {
            ViewBag.Role = GetRole();
            var desease = DeseaseService.GetDeseaseById(Id);
            var deseaseModel = new UpdateDeseaseModel { Id = Id, DoctorId = desease.DoctorId, DoctorName = desease.DoctorName, Status = desease.Status, Name = desease.Name, Message = Message, Complaint = desease.Complaint };
            var doctorsList = new SelectList(UserService.GetAllUsers().Where(x => x.Role == "doctor").Select(x => new { Id = x.Id, Name = x.Name, Specialization = ((DoctorDTO)x).Specialization }), "Id", "Name");
            var statusesList = new SelectList(DeseaseService.GetAllStatuses());
            ViewBag.doctor = doctorsList;
            ViewBag.statuses = statusesList;
            if (desease.EndDate == null)
                ViewBag.IsClosed = false;
            else
                ViewBag.IsClosed = true;
            return View(deseaseModel);
        }

        [HttpPost]
        [Authorize(Roles = "doctor, admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangeName(string Name, int DeseaseId)
        {
            string message = "";
            if (string.IsNullOrWhiteSpace(Name))
            {
                message = "Имя не может быть пустым";
            }
            else
            {
                OperationDetails operationDetails = await DeseaseService.ChangeName(DeseaseId, Name);
                message = operationDetails.Message;
            }
            return RedirectToAction("UpdateDesease", new { id = DeseaseId, message = message });
        }

        [HttpPost]
        [Authorize(Roles = "doctor, admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangeDoctor(string DoctorId, int DeseaseId)
        {
            string message = "";
            OperationDetails operationDetails = await DeseaseService.ChangeDoctor(DeseaseId, DoctorId);
            message = operationDetails.Message;
            return RedirectToAction("UpdateDesease", new { id = DeseaseId, message = message });
        }

        [HttpPost]
        [Authorize(Roles = "doctor, admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangeStatus(string Status, int DeseaseId)
        {
            string message = "";
            OperationDetails operationDetails = await DeseaseService.ChangeStatus(DeseaseId, Status);
            message = operationDetails.Message;
            return RedirectToAction("UpdateDesease", new { id = DeseaseId, message = message });
        }

        [HttpPost]
        [Authorize(Roles = "doctor, admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CloseDesease(int DeseaseId)
        {
            string message = "";
            OperationDetails operationDetails = await DeseaseService.CloseDesease(DeseaseId);
            message = operationDetails.Message;
            return RedirectToAction("UpdateDesease", new { id = DeseaseId, message = message });
        }

        [HttpPost]
        [Authorize(Roles = "doctor, admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ReOpenDesease(int DeseaseId)
        {
            string message = "";
            OperationDetails operationDetails = await DeseaseService.ReOpenDesease(DeseaseId);
            message = operationDetails.Message;
            return RedirectToAction("UpdateDesease", new { id = DeseaseId, message = message });
        }

        [HttpPost]
        [Authorize(Roles = "doctor, admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteDesease(int DeseaseId)
        {
            string message = "";
            OperationDetails operationDetails = await DeseaseService.DeleteDesease(DeseaseId);
            if (!operationDetails.Succedeed)
            {
                message = operationDetails.Message;
                return RedirectToAction("UpdateDesease", new { id = DeseaseId, message = message });
            }
            return RedirectToAction("Deseases");
        }

        [HttpGet]
        public ActionResult DeseaseHistory(int Id, string Message)
        {
            ViewBag.Role = GetRole();
            var desease = DeseaseService.GetDeseaseById(Id);
            var history = DeseaseService.GetAllRecords(Id);
            ViewBag.Id = Id;
            if (desease.EndDate == null)
                ViewBag.IsClosed = false;
            else
                ViewBag.IsClosed = true;
            return View(history);
        }

        [HttpPost]
        [Authorize(Roles = "doctor, admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddRecord(int DeseaseId, string Text)
        {
            string message = "";
            OperationDetails operationDetails = await DeseaseService.AddRecord(DeseaseId, Text);
            message = operationDetails.Message;
            return RedirectToAction("DeseaseHistory", new { id = DeseaseId, message = message });
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