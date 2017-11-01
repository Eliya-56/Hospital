using MyHospital.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyHospital.BLL.DTO;
using MyHospital.BLL.Infrastructure;
using MyHospital.DAL.Entities;
using MyHospital.DAL.Interfaces;
using Microsoft.AspNet.Identity;

namespace MyHospital.BLL.Services
{
    public class DeseaseService : IDeseaseService
    {
        IUnitOfWork Database { get; set; }

        public DeseaseService(IUnitOfWork uow)
        {
            Database = uow;
        }

        public async Task<OperationDetails> AddRecord(int deseaseId, string text)
        {
            var dbDesease = Database.DeseaseManager.GetDeseaseById(deseaseId);
            if (dbDesease == null)
                return new OperationDetails(false, "Неверно указанная болезнь", "Id");
            dbDesease.DeseaseRecords.Add(new DeseaseRecord { Date = DateTime.UtcNow, Text = text });
            Database.DeseaseManager.UpdateDesease(dbDesease);
            await Database.SaveAsync();
            return new OperationDetails(true, "Запись успешно добавлена", "");
        }

        public IEnumerable<DeseaseRecordDTO> GetAllRecords(int deseaseId)
        {
            var dbDesease = Database.DeseaseManager.GetDeseaseById(deseaseId);
            if (dbDesease == null)
                return null;
            var records = new List<DeseaseRecordDTO>();
            foreach (var record in dbDesease.DeseaseRecords)
            {
                records.Add(new DeseaseRecordDTO { Date = record.Date, Text = record.Text });
            }
            return records;
        }

        public async Task<OperationDetails> ChangeDoctor(int deseaseId, string doctorId)
        {
            var dbDesease = Database.DeseaseManager.GetDeseaseById(deseaseId);
            if (dbDesease == null)
                return new OperationDetails(false, "Неверно указанная болезнь", "Id");
            var dbDoctor = await Database.UserManager.FindByIdAsync(doctorId); if (dbDoctor == null || !(dbDoctor is Doctor))
                return new OperationDetails(false, "Неверно указанный доктор", "Id");
            dbDesease.Doctor = (Doctor)dbDoctor;
            Database.DeseaseManager.UpdateDesease(dbDesease);
            await Database.SaveAsync();
            return new OperationDetails(true, "Доктор успешно изменён", "");
        }

        public async Task<OperationDetails> ChangeStatus(int deseaseId, string status)
        {
            var dbDesease = Database.DeseaseManager.GetDeseaseById(deseaseId);
            var dbStatus = Database.DeseaseManager.GetAllStatuses().FirstOrDefault(x => x.Name == status);
            if (dbDesease == null)
                return new OperationDetails(false, "Неверно указанная болезнь", "Id");
            if (dbStatus == null)
                return new OperationDetails(false, "Неверно указанный статус", "Name");
            dbDesease.StatusId = dbStatus.Id;
            Database.DeseaseManager.UpdateDesease(dbDesease);
            await Database.SaveAsync();
            return new OperationDetails(true, "Статус успешно изменён", "");
        }

        public async Task<OperationDetails> ChangeName(int id, string name)
        {
            var dbDesease = Database.DeseaseManager.GetDeseaseById(id);
            if (dbDesease == null)
                return new OperationDetails(false, "Неверно указанная болезнь", "Id");
            dbDesease.Name = name;
            Database.DeseaseManager.UpdateDesease(dbDesease);
            await Database.SaveAsync();
            return new OperationDetails(true, "Имя успешно измененно", "");
        }

        public async Task<OperationDetails> CloseDesease(DeseaseDTO desease)
        {
            return await CloseDesease(desease.Id);
        }

        public async Task<OperationDetails> DeleteDesease(int id)
        {
            var dbDesease = Database.DeseaseManager.GetDeseaseById(id);
            if (dbDesease == null)
                return new OperationDetails(false, "Неверно указанная болезнь", "Id");
            Database.DeseaseManager.DeleteDesease(dbDesease);
            await Database.SaveAsync();
            return new OperationDetails(true, "Болезнь удаленна", "");
        }

        public async Task<OperationDetails> CloseDesease(int id)
        {
            var dbDesease = Database.DeseaseManager.GetDeseaseById(id);
            var dbStatus = Database.DeseaseManager.GetAllStatuses().FirstOrDefault(x => x.Name == "Здоровый");
            if (dbDesease == null)
                return new OperationDetails(false, "Неверно указанная болезнь", "Id");
            dbDesease.StatusId = dbStatus.Id;
            dbDesease.EndDate = DateTime.UtcNow;
            Database.DeseaseManager.UpdateDesease(dbDesease);
            await Database.SaveAsync();
            return new OperationDetails(true, "Болезнь успешно закрыта", "");
        }

        public async Task<OperationDetails> ReOpenDesease(int id)
        {
            var dbDesease = Database.DeseaseManager.GetDeseaseById(id);
            var dbStatus = Database.DeseaseManager.GetAllStatuses().FirstOrDefault(x => x.Name == "Здоровый");
            if (dbDesease == null)
                return new OperationDetails(false, "Неверно указанная болезнь", "Id");
            dbDesease.StatusId = dbStatus.Id;
            dbDesease.EndDate = null;
            Database.DeseaseManager.UpdateDesease(dbDesease);
            await Database.SaveAsync();
            return new OperationDetails(true, "Болезнь снова отрыта", "");
        }

        public async Task<OperationDetails> CreateAsync(string patientId, string doctorId, string complaint)
        {
            var dbDoctor = await Database.UserManager.FindByIdAsync(doctorId);
            var dbPatient = await Database.UserManager.FindByIdAsync(patientId);
            var dbStatus = Database.DeseaseManager.GetAllStatuses().First(x => x.Name == "Прибыл");
            if (dbDoctor == null || !(dbDoctor is Doctor))
                return new OperationDetails(false, "Неверно указанный доктор", "Id");
            else if (dbPatient == null || !(dbPatient is Patient))
                return new OperationDetails(false, "Неверно указанный пациент", "Id");
            else
            {
                var dbDesease = new Desease { Doctor = (Doctor)dbDoctor, Patient = (Patient)dbPatient, Name = "Новое заболевание", Complaint = complaint, StartDate = DateTime.UtcNow, StatusId = dbStatus.Id };
                Database.DeseaseManager.CreateDesease(dbDesease);
                await Database.SaveAsync();
                return new OperationDetails(true, "Болезнь создана успешно", "");
            }
        }   

        public void Dispose()
        {
            Database.Dispose();
        }

        public IEnumerable<DeseaseDTO> GetAllDesease()
        {
            var deseases = new List<DeseaseDTO>();
            foreach (var desease in Database.DeseaseManager.GetAllDeseases())
            {
                List<DeseaseRecordDTO> records = new List<DeseaseRecordDTO>();
                foreach (var record in desease.DeseaseRecords)
                {
                    records.Add(new DeseaseRecordDTO { Date = record.Date, Text = record.Text });
                }
                deseases.Add(new DeseaseDTO { Id = desease.Id, Complaint = desease.Complaint, Name = desease.Name, StartDate = desease.StartDate, Status = desease.Status.Name, EndDate = desease.EndDate, DoctorId = desease.DoctorId, PatientId = desease.PatientId, Records = records, DoctorName = desease.Doctor.UserName, PatientName = desease.Patient.UserName });
            }
            return deseases;
        }

        public IEnumerable<DeseaseDTO> GetAllDesease(string UserId)
        {
            if(Database.UserManager.IsInRole(UserId, "doctor"))
                return GetAllDesease().Where(x => x.DoctorId == UserId);
            else if (Database.UserManager.IsInRole(UserId, "patient"))
                return GetAllDesease().Where(x => x.PatientId == UserId);
            else if (Database.UserManager.IsInRole(UserId, "admin"))
                return GetAllDesease();
            return null;
        }

        public DeseaseDTO GetDeseaseById(int id)
        {
            var dbDesease = Database.DeseaseManager.GetDeseaseById(id);
            return new DeseaseDTO { Id = dbDesease.Id, Name = dbDesease.Name, Complaint = dbDesease.Complaint, StartDate = dbDesease.StartDate, Status = dbDesease.Status.Name, EndDate = dbDesease.EndDate, DoctorId =dbDesease.DoctorId, PatientId = dbDesease.PatientId, DoctorName = dbDesease.Doctor.UserName, PatientName = dbDesease.Patient.UserName };
        }
        
        public void SetInitialData(List<string> statuses)
        {
            foreach (var status in statuses)
            {
                var dbStatus = Database.DeseaseManager.GetAllStatuses().FirstOrDefault(x => x.Name == status);
                if(dbStatus == null)
                    Database.DeseaseManager.CreateStatus(status);
            }
        }

        public IEnumerable<string> GetAllStatuses()
        {
            var statuses = Database.DeseaseManager.GetAllStatuses().Select(x => x.Name).ToList();
            return statuses;
        }
    }
}
