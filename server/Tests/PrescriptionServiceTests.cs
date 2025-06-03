using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.Services;
using server.Models;

namespace server.Tests
{
    [TestFixture]
    public class PrescriptionServiceTests
    {
        private IPrescriptionService _prescriptionService;

        [SetUp]
        public void Setup()
        {
            // Arrange - Setup
            _prescriptionService = new PrescriptionService();
        }

        [TestCase(1, true)]      // Minimum valid value
        [TestCase(250, true)]    // Middle valid value
        [TestCase(500, true)]    // Maximum valid value
        [TestCase(0, false)]     // Below minimum boundary
        [TestCase(-10, false)]   // Negative value
        [TestCase(501, false)]   // Just above maximum boundary
        [TestCase(1000, false)]  // Well above maximum boundary
        public void IsDosageValid_WithVariousDosages_ReturnsExpectedResult(int dosage, bool expectedResult)
        {
            // Act
            bool result = _prescriptionService.IsDosageValid(dosage);

            // Assert
            Assert.AreEqual(expectedResult, result, $"Dosage {dosage} validation failed");
        }

        [Test]
        public void IsDosageValid_WithValidDosage_ReturnsTrue()
        {
            // Arrange
            int validDosage = 250;

            // Act
            bool result = _prescriptionService.IsDosageValid(validDosage);

            // Assert
            Assert.IsTrue(result, "Valid dosage should return true");
        }

        [Test]
        public void IsDosageValid_WithZeroDosage_ReturnsFalse()
        {
            // Arrange
            int zeroDosage = 0;

            // Act
            bool result = _prescriptionService.IsDosageValid(zeroDosage);

            // Assert
            Assert.IsFalse(result, "Zero dosage should return false");
        }

        [Test]
        public void IsDosageValid_WithNegativeDosage_ReturnsFalse()
        {
            // Arrange
            int negativeDosage = -5;

            // Act
            bool result = _prescriptionService.IsDosageValid(negativeDosage);

            // Assert
            Assert.IsFalse(result, "Negative dosage should return false");
        }

        [Test]
        public void IsDosageValid_WithExcessiveDosage_ReturnsFalse()
        {
            // Arrange
            int excessiveDosage = 501;

            // Act
            bool result = _prescriptionService.IsDosageValid(excessiveDosage);

            // Assert
            Assert.IsFalse(result, "Excessive dosage should return false");
        }
    }
}
