using MyHospital.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyHospital.DAL.Entities;
using MyHospital.DAL.EF;

namespace MyHospital.DAL.Repositories
{
    public class DeseaseManager : IDeseaseManager
    {
        public MyHospitalContext Context { get; set; }
        public DeseaseManager(MyHospitalContext context)
        {
            Context = context;
        }

        public void CreateDesease(Desease desease)
        {
            Context.Deseases.Add(desease);
            Context.SaveChanges();
        }

        public void Dispose()
        {
            Context.Dispose();
        }

        public IEnumerable<Desease> GetAllDeseases()
        {
            return Context.Deseases.Include("DeseaseRecords").Include("Doctor").Include("Patient").Include("Status").ToList();
        }

        public Desease GetDeseaseById(int deseaseId)
        {
            return Context.Deseases.Include("DeseaseRecords").Include("Doctor").Include("Patient").Include("Status").First(x => x.Id == deseaseId);
        }

        public void UpdateDesease(Desease desease)
        {
            Context.Entry(desease).State = System.Data.Entity.EntityState.Modified;
            Context.SaveChanges();
        }

        public void DeleteDesease(Desease desease)
        {
            Context.Deseases.Remove(desease);
            Context.SaveChanges();
        }

        public IEnumerable<DeseaseStatus> GetAllStatuses()
        {
            return Context.DeseaseStatuses.ToList();
        }

        public void CreateStatus(string name)
        {
            Context.DeseaseStatuses.Add(new DeseaseStatus { Name = name });
            Context.SaveChanges();
        }
    }
}
