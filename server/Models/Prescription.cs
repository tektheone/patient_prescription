using System;
using System.ComponentModel.DataAnnotations;

namespace server.Models
{
    public class Prescription
    {
        public int Id { get; set; }

        [Required]
        public int PatientId { get; set; }

        [Required]
        public string PatientName { get; set; } = string.Empty;

        [Required]
        public string MedicationName { get; set; } = string.Empty;

        [Required]
        public string Dosage { get; set; } = string.Empty;

        [Required]
        public string Frequency { get; set; } = string.Empty;

        public DateTime PrescriptionDate { get; set; } = DateTime.Now;

        public DateTime EndDate { get; set; } = DateTime.Now;

        [Required]
        public string PrescribedBy { get; set; } = string.Empty;

        public string? Notes { get; set; }
    }
}
