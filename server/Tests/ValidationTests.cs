using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using server.Models;

namespace server.Tests
{
    [TestFixture]
    public class ValidationTests
    {
        [Test]
        public void Prescription_WithValidData_PassesValidation()
        {
            // Arrange
            var prescription = new Prescription
            {
                PatientId = 12345,
                PatientName = "John Doe",
                MedicationName = "Aspirin",
                Dosage = "100",
                Frequency = "1",
                PrescriptionDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(30),
                PrescribedBy = "Dr. Smith",
                Notes = "Take with food"
            };

            // Act
            var validationResults = ValidateModel(prescription);

            // Assert
            Assert.IsEmpty(validationResults, "Validation should pass for valid prescription");
        }

        [Test]
        public void Prescription_WithMissingPatientId_FailsValidation()
        {
            // Arrange
            var prescription = new Prescription
            {
                // PatientId is missing
                PatientName = "John Doe",
                MedicationName = "Aspirin",
                Dosage = "100",
                Frequency = "1",
                PrescriptionDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(30),
                PrescribedBy = "Dr. Smith",
                Notes = "Take with food"
            };

            // Act
            var validationResults = ValidateModel(prescription);

            // Assert
            Assert.IsNotEmpty(validationResults, "Validation should fail for missing PatientId");
            Assert.AreEqual(1, validationResults.Count, "Should have exactly one validation error");
            Assert.AreEqual("PatientId", validationResults[0].MemberNames.First(), "Error should be for PatientId");
        }

        [Test]
        public void Prescription_WithMissingPatientName_FailsValidation()
        {
            // Arrange
            var prescription = new Prescription
            {
                PatientId = 12345,
                // PatientName is missing
                MedicationName = "Aspirin",
                Dosage = "100",
                Frequency = "1",
                PrescriptionDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(30),
                PrescribedBy = "Dr. Smith",
                Notes = "Take with food"
            };

            // Act
            var validationResults = ValidateModel(prescription);

            // Assert
            Assert.IsNotEmpty(validationResults, "Validation should fail for missing PatientName");
            Assert.AreEqual(1, validationResults.Count, "Should have exactly one validation error");
            Assert.AreEqual("PatientName", validationResults[0].MemberNames.First(), "Error should be for PatientName");
        }

        [Test]
        public void Prescription_WithMissingMedicationName_FailsValidation()
        {
            // Arrange
            var prescription = new Prescription
            {
                PatientId = 12345,
                PatientName = "John Doe",
                // MedicationName is missing
                Dosage = "100",
                Frequency = "1",
                PrescriptionDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(30),
                PrescribedBy = "Dr. Smith",
                Notes = "Take with food"
            };

            // Act
            var validationResults = ValidateModel(prescription);

            // Assert
            Assert.IsNotEmpty(validationResults, "Validation should fail for missing MedicationName");
            Assert.AreEqual(1, validationResults.Count, "Should have exactly one validation error");
            Assert.AreEqual("MedicationName", validationResults[0].MemberNames.First(), "Error should be for MedicationName");
        }

        [Test]
        public void Prescription_WithMissingDosage_FailsValidation()
        {
            // Arrange
            var prescription = new Prescription
            {
                PatientId = 12345,
                PatientName = "John Doe",
                MedicationName = "Aspirin",
                // Dosage is missing
                Frequency = "1",
                PrescriptionDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(30),
                PrescribedBy = "Dr. Smith",
                Notes = "Take with food"
            };

            // Act
            var validationResults = ValidateModel(prescription);

            // Assert
            Assert.IsNotEmpty(validationResults, "Validation should fail for missing Dosage");
            Assert.AreEqual(1, validationResults.Count, "Should have exactly one validation error");
            Assert.AreEqual("Dosage", validationResults[0].MemberNames.First(), "Error should be for Dosage");
        }

        [Test]
        public void Prescription_WithMissingFrequency_FailsValidation()
        {
            // Arrange
            var prescription = new Prescription
            {
                PatientId = 12345,
                PatientName = "John Doe",
                MedicationName = "Aspirin",
                Dosage = "100",
                // Frequency is missing
                PrescriptionDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(30),
                PrescribedBy = "Dr. Smith",
                Notes = "Take with food"
            };

            // Act
            var validationResults = ValidateModel(prescription);

            // Assert
            Assert.IsNotEmpty(validationResults, "Validation should fail for missing Frequency");
            Assert.AreEqual(1, validationResults.Count, "Should have exactly one validation error");
            Assert.AreEqual("Frequency", validationResults[0].MemberNames.First(), "Error should be for Frequency");
        }

        [Test]
        public void Prescription_WithMissingPrescribedBy_FailsValidation()
        {
            // Arrange
            var prescription = new Prescription
            {
                PatientId = 12345,
                PatientName = "John Doe",
                MedicationName = "Aspirin",
                Dosage = "100",
                Frequency = "1",
                PrescriptionDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(30),
                // PrescribedBy is missing
                Notes = "Take with food"
            };

            // Act
            var validationResults = ValidateModel(prescription);

            // Assert
            Assert.IsNotEmpty(validationResults, "Validation should fail for missing PrescribedBy");
            Assert.AreEqual(1, validationResults.Count, "Should have exactly one validation error");
            Assert.AreEqual("PrescribedBy", validationResults[0].MemberNames.First(), "Error should be for PrescribedBy");
        }

        [Test]
        public void Prescription_WithPatientNameTooLong_FailsValidation()
        {
            // Arrange
            var prescription = new Prescription
            {
                PatientId = 12345,
                PatientName = new string('A', 101), // Exceeds max length of 100
                MedicationName = "Aspirin",
                Dosage = "100",
                Frequency = "1",
                PrescriptionDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(30),
                PrescribedBy = "Dr. Smith",
                Notes = "Take with food"
            };

            // Act
            var validationResults = ValidateModel(prescription);

            // Assert
            Assert.IsNotEmpty(validationResults, "Validation should fail for PatientName too long");
            Assert.AreEqual(1, validationResults.Count, "Should have exactly one validation error");
            Assert.AreEqual("PatientName", validationResults[0].MemberNames.First(), "Error should be for PatientName");
        }

        [Test]
        public void Prescription_WithEndDateBeforePrescriptionDate_FailsValidation()
        {
            // Arrange
            var prescription = new Prescription
            {
                PatientId = 12345,
                PatientName = "John Doe",
                MedicationName = "Aspirin",
                Dosage = "100",
                Frequency = "1",
                PrescriptionDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(-1), // End date before prescription date
                PrescribedBy = "Dr. Smith",
                Notes = "Take with food"
            };

            // Act
            var validationResults = ValidateModel(prescription);

            // Assert
            Assert.IsNotEmpty(validationResults, "Validation should fail for EndDate before PrescriptionDate");
            Assert.AreEqual(1, validationResults.Count, "Should have exactly one validation error");
            Assert.AreEqual("EndDate", validationResults[0].MemberNames.First(), "Error should be for EndDate");
        }

        // Helper method to validate model and return validation results
        private List<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(model);
            Validator.TryValidateObject(model, validationContext, validationResults, true);
            return validationResults;
        }
    }
}
