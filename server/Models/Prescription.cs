using System;
using System.ComponentModel.DataAnnotations;

namespace server.Models
{
    [EndDateAfterPrescriptionDate]
    public class Prescription
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Patient ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Patient ID must be a positive number")]
        [Display(Name = "Patient ID")]
        public int PatientId { get; set; }

        [Required(ErrorMessage = "Patient name is required")]
        [StringLength(100, ErrorMessage = "Patient name cannot exceed 100 characters")]
        [Display(Name = "Patient Name")]
        public string PatientName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Medication name is required")]
        [StringLength(100, ErrorMessage = "Medication name cannot exceed 100 characters")]
        [Display(Name = "Medication Name")]
        public string MedicationName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Dosage is required")]
        [StringLength(20, ErrorMessage = "Dosage cannot exceed 20 characters")]
        public string Dosage { get; set; } = string.Empty;

        [Required(ErrorMessage = "Frequency is required")]
        [StringLength(50, ErrorMessage = "Frequency cannot exceed 50 characters")]
        public string Frequency { get; set; } = string.Empty;

        [Required(ErrorMessage = "Prescription date is required")]
        [Display(Name = "Prescription Date")]
        [DataType(DataType.Date)]
        public DateTime PrescriptionDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "End date is required")]
        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Prescriber name is required")]
        [StringLength(100, ErrorMessage = "Prescriber name cannot exceed 100 characters")]
        [Display(Name = "Prescribed By")]
        public string PrescribedBy { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Notes cannot exceed 500 characters")]
        public string? Notes { get; set; }
    }
}
