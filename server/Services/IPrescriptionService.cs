using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using server.Models;

namespace server.Services
{
    public interface IPrescriptionService
    {
        Task<IEnumerable<Prescription>> GetAllPrescriptionsAsync();
        Task<Prescription> GetPrescriptionByIdAsync(int id);
        Task<Prescription> CreatePrescriptionAsync(Prescription prescription);
        Task<bool> UpdatePrescriptionAsync(int id, Prescription prescription);
        Task<bool> DeletePrescriptionAsync(int id);
        bool IsDosageValid(int dosage);
    }
}
