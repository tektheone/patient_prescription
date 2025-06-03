using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using server.Controllers;
using server.Models;
using server.Services;

namespace server.Tests
{
    [TestFixture]
    public class PrescriptionControllerTests
    {
        private Mock<IPrescriptionService> _mockService;
        private PrescriptionsController _controller;
        private List<Prescription> _samplePrescriptions;

        [SetUp]
        public void Setup()
        {
            // Create mock service
            _mockService = new Mock<IPrescriptionService>();
            
            // Create controller with mock service
            _controller = new PrescriptionsController(_mockService.Object);
            
            // Setup sample data
            _samplePrescriptions = new List<Prescription>
            {
                new Prescription 
                { 
                    Id = 1, 
                    PatientId = 12345, 
                    PatientName = "John Doe", 
                    MedicationName = "Aspirin", 
                    Dosage = "100", 
                    Frequency = "1", 
                    PrescriptionDate = DateTime.Now.AddDays(-10), 
                    EndDate = DateTime.Now.AddDays(20), 
                    PrescribedBy = "Dr. Smith", 
                    Notes = "Take with food" 
                },
                new Prescription 
                { 
                    Id = 2, 
                    PatientId = 67890, 
                    PatientName = "Jane Smith", 
                    MedicationName = "Tylenol", 
                    Dosage = "500", 
                    Frequency = "2", 
                    PrescriptionDate = DateTime.Now.AddDays(-5), 
                    EndDate = DateTime.Now.AddDays(25), 
                    PrescribedBy = "Dr. Johnson", 
                    Notes = "Take as needed for pain" 
                }
            };
        }

        [Test]
        public async Task GetAll_ReturnsAllPrescriptions()
        {
            // Arrange
            _mockService.Setup(service => service.GetAllPrescriptionsAsync())
                .ReturnsAsync(_samplePrescriptions);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult, "Result should be OkObjectResult");
            
            var prescriptions = okResult.Value as IEnumerable<Prescription>;
            Assert.IsNotNull(prescriptions, "Value should be IEnumerable<Prescription>");
            Assert.AreEqual(2, prescriptions.Count(), "Should return 2 prescriptions");
        }

        [Test]
        public async Task GetById_WithValidId_ReturnsPrescription()
        {
            // Arrange
            int prescriptionId = 1;
            _mockService.Setup(service => service.GetPrescriptionByIdAsync(prescriptionId))
                .ReturnsAsync(_samplePrescriptions.First());

            // Act
            var result = await _controller.GetById(prescriptionId);

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult, "Result should be OkObjectResult");
            
            var prescription = okResult.Value as Prescription;
            Assert.IsNotNull(prescription, "Value should be Prescription");
            Assert.AreEqual(prescriptionId, prescription.Id, "Should return prescription with ID 1");
        }

        [Test]
        public async Task GetById_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            int invalidId = 999;
            _mockService.Setup(service => service.GetPrescriptionByIdAsync(invalidId))
                .ReturnsAsync((Prescription)null);

            // Act
            var result = await _controller.GetById(invalidId);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result.Result, "Should return NotFound for invalid ID");
        }

        [Test]
        public async Task Create_WithValidPrescription_ReturnsCreatedAtAction()
        {
            // Arrange
            var newPrescription = new Prescription
            {
                PatientId = 54321,
                PatientName = "New Patient",
                MedicationName = "New Medication",
                Dosage = "250",
                Frequency = "3",
                PrescriptionDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(30),
                PrescribedBy = "Dr. New",
                Notes = "New prescription notes"
            };
            
            var createdPrescription = new Prescription
            {
                Id = 3,
                PatientId = newPrescription.PatientId,
                PatientName = newPrescription.PatientName,
                MedicationName = newPrescription.MedicationName,
                Dosage = newPrescription.Dosage,
                Frequency = newPrescription.Frequency,
                PrescriptionDate = newPrescription.PrescriptionDate,
                EndDate = newPrescription.EndDate,
                PrescribedBy = newPrescription.PrescribedBy,
                Notes = newPrescription.Notes
            };
            
            _mockService.Setup(service => service.CreatePrescriptionAsync(It.IsAny<Prescription>()))
                .ReturnsAsync(createdPrescription);

            // Act
            var result = await _controller.Create(newPrescription);

            // Assert
            var createdAtActionResult = result.Result as CreatedAtActionResult;
            Assert.IsNotNull(createdAtActionResult, "Result should be CreatedAtActionResult");
            Assert.AreEqual("GetById", createdAtActionResult.ActionName, "Action name should be GetById");
            
            var returnedPrescription = createdAtActionResult.Value as Prescription;
            Assert.IsNotNull(returnedPrescription, "Value should be Prescription");
            Assert.AreEqual(3, returnedPrescription.Id, "Should return prescription with ID 3");
        }

        [Test]
        public async Task Update_WithValidIdAndPrescription_ReturnsNoContent()
        {
            // Arrange
            int prescriptionId = 1;
            var prescriptionToUpdate = _samplePrescriptions.First();
            prescriptionToUpdate.Dosage = "200";
            
            _mockService.Setup(service => service.GetPrescriptionByIdAsync(prescriptionId))
                .ReturnsAsync(prescriptionToUpdate);
            
            _mockService.Setup(service => service.UpdatePrescriptionAsync(prescriptionId, It.IsAny<Prescription>()))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.Update(prescriptionId, prescriptionToUpdate);

            // Assert
            Assert.IsInstanceOf<NoContentResult>(result, "Should return NoContent for successful update");
        }

        [Test]
        public async Task Update_WithInvalidId_ReturnsBadRequest()
        {
            // Arrange
            int invalidId = 999;
            var prescriptionToUpdate = _samplePrescriptions.First();
            prescriptionToUpdate.Id = 1; // Different from the route id
            
            // Act
            var result = await _controller.Update(invalidId, prescriptionToUpdate);

            // Assert
            Assert.IsInstanceOf<BadRequestResult>(result, "Should return BadRequest when route id doesn't match prescription id");
        }

        [Test]
        public async Task Update_WithNonExistentId_ReturnsNotFound()
        {
            // Arrange
            int nonExistentId = 999;
            var prescriptionToUpdate = new Prescription { Id = nonExistentId };
            
            _mockService.Setup(service => service.UpdatePrescriptionAsync(nonExistentId, It.IsAny<Prescription>()))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.Update(nonExistentId, prescriptionToUpdate);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result, "Should return NotFound for non-existent ID");
        }

        [Test]
        public async Task Delete_WithValidId_ReturnsNoContent()
        {
            // Arrange
            int prescriptionId = 1;
            
            _mockService.Setup(service => service.GetPrescriptionByIdAsync(prescriptionId))
                .ReturnsAsync(_samplePrescriptions.First());
            
            _mockService.Setup(service => service.DeletePrescriptionAsync(prescriptionId))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.Delete(prescriptionId);

            // Assert
            Assert.IsInstanceOf<NoContentResult>(result, "Should return NoContent for successful delete");
        }

        [Test]
        public async Task Delete_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            int invalidId = 999;
            
            _mockService.Setup(service => service.GetPrescriptionByIdAsync(invalidId))
                .ReturnsAsync((Prescription)null);

            // Act
            var result = await _controller.Delete(invalidId);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result, "Should return NotFound for invalid ID");
        }
    }
}
