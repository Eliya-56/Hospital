using MyHospital.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHospital.DAL.Interfaces
{
    public interface IDeseaseManager : IDisposable
    {
        void CreateDesease(Desease desease);
        void UpdateDesease(Desease desease);
        IEnumerable<Desease> GetAllDeseases();
        Desease GetDeseaseById(int deseaseId);
        void DeleteDesease(Desease desease);
        IEnumerable<DeseaseStatus> GetAllStatuses();
        void CreateStatus(string name);
    }
}
