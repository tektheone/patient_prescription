using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.Models;

namespace server.Services
{
    public class PrescriptionService : IPrescriptionService
    {
        private readonly List<Prescription> _prescriptions;
        private int _nextId = 1;

        public PrescriptionService()
        {
            _prescriptions = new List<Prescription>
            {
                // Add some sample data
                new Prescription
                {
                    Id = _nextId++,
                    PatientId = 1001,
                    PatientName = "John Doe",
                    MedicationName = "Amoxicillin",
                    Dosage = "500mg",
                    Frequency = "3 times daily",
                    PrescriptionDate = DateTime.Now.AddDays(-5),
                    EndDate = DateTime.Now.AddDays(5),
                    PrescribedBy = "Dr. Smith",
                    Notes = "Take with food"
                },
                new Prescription
                {
                    Id = _nextId++,
                    PatientId = 1002,
                    PatientName = "Jane Smith",
                    MedicationName = "Lisinopril",
                    Dosage = "10mg",
                    Frequency = "Once daily",
                    PrescriptionDate = DateTime.Now.AddDays(-10),
                    EndDate = DateTime.Now.AddDays(20),
                    PrescribedBy = "Dr. Johnson",
                    Notes = "Take in the morning"
                }
            };
        }

        public async Task<IEnumerable<Prescription>> GetAllPrescriptionsAsync()
        {
            return await Task.FromResult(_prescriptions);
        }

        public IEnumerable<Prescription> GetAll()
        {
            return _prescriptions;
        }

        public IEnumerable<Prescription> GetByPatientId(int patientId)
        {
            return _prescriptions.Where(p => p.PatientId == patientId);
        }

        public async Task<Prescription> GetPrescriptionByIdAsync(int id)
        {
            return await Task.FromResult(_prescriptions.FirstOrDefault(p => p.Id == id));
        }

        public Prescription? GetById(int id)
        {
            return _prescriptions.FirstOrDefault(p => p.Id == id);
        }

        public async Task<Prescription> CreatePrescriptionAsync(Prescription prescription)
        {
            prescription.Id = _nextId++;
            _prescriptions.Add(prescription);
            return await Task.FromResult(prescription);
        }

        public Prescription Add(Prescription prescription)
        {
            prescription.Id = _nextId++;
            _prescriptions.Add(prescription);
            return prescription;
        }

        public async Task<bool> UpdatePrescriptionAsync(int id, Prescription prescription)
        {
            var existingPrescription = _prescriptions.FirstOrDefault(p => p.Id == id);
            if (existingPrescription == null)
            {
                return await Task.FromResult(false);
            }

            var index = _prescriptions.IndexOf(existingPrescription);
            _prescriptions[index] = prescription;
            return await Task.FromResult(true);
        }

        public bool Update(Prescription prescription)
        {
            var existingPrescription = _prescriptions.FirstOrDefault(p => p.Id == prescription.Id);
            if (existingPrescription == null)
            {
                return false;
            }

            var index = _prescriptions.IndexOf(existingPrescription);
            _prescriptions[index] = prescription;
            return true;
        }

        public async Task<bool> DeletePrescriptionAsync(int id)
        {
            var prescription = _prescriptions.FirstOrDefault(p => p.Id == id);
            if (prescription == null)
            {
                return await Task.FromResult(false);
            }

            return await Task.FromResult(_prescriptions.Remove(prescription));
        }

        public bool Delete(int id)
        {
            var prescription = _prescriptions.FirstOrDefault(p => p.Id == id);
            if (prescription == null)
            {
                return false;
            }

            return _prescriptions.Remove(prescription);
        }

        public bool IsDosageValid(int dosage)
        {
            // Validate that dosage is between 1 and 500
            return dosage > 0 && dosage <= 500;
        }
    }
}
