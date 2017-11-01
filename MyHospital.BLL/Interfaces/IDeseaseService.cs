using MyHospital.BLL.DTO;
using MyHospital.BLL.Infrastructure;
using MyHospital.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHospital.BLL.Interfaces
{
    public interface IDeseaseService : IDisposable
    {
        Task<OperationDetails> CreateAsync(string patientId, string doctorId, string complaint);
        Task<OperationDetails> ChangeDoctor(int deseaseId, string doctorId);
        Task<OperationDetails> ChangeStatus(int deseaseId, string status);
        Task<OperationDetails> CloseDesease(DeseaseDTO desease);
        Task<OperationDetails> CloseDesease(int id);
        Task<OperationDetails> DeleteDesease(int id);
        Task<OperationDetails> ReOpenDesease(int id);
        Task<OperationDetails> AddRecord(int deseaseId, string name);
        IEnumerable<DeseaseRecordDTO> GetAllRecords(int deseaseId);
        Task<OperationDetails> ChangeName(int id, string text);
        IEnumerable<DeseaseDTO> GetAllDesease();
        IEnumerable<DeseaseDTO> GetAllDesease(string UserId);
        DeseaseDTO GetDeseaseById(int id);
        void SetInitialData(List<string> statuses);
        IEnumerable<string> GetAllStatuses();
    }
}
