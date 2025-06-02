using System;
using System.Collections.Generic;
using System.Linq;
using server.Models;

namespace server.Services
{
    public class PrescriptionService
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

        public IEnumerable<Prescription> GetAll()
        {
            return _prescriptions;
        }

        public IEnumerable<Prescription> GetByPatientId(int patientId)
        {
            return _prescriptions.Where(p => p.PatientId == patientId);
        }

        public Prescription? GetById(int id)
        {
            return _prescriptions.FirstOrDefault(p => p.Id == id);
        }

        public Prescription Add(Prescription prescription)
        {
            prescription.Id = _nextId++;
            _prescriptions.Add(prescription);
            return prescription;
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

        public bool Delete(int id)
        {
            var prescription = _prescriptions.FirstOrDefault(p => p.Id == id);
            if (prescription == null)
            {
                return false;
            }

            return _prescriptions.Remove(prescription);
        }
    }
}
