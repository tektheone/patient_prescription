using Microsoft.AspNetCore.Mvc;
using server.Models;
using server.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PrescriptionsController : ControllerBase
    {
        private readonly IPrescriptionService _prescriptionService;

        public PrescriptionsController(IPrescriptionService prescriptionService)
        {
            _prescriptionService = prescriptionService;
        }

        // GET: api/prescriptions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Prescription>>> GetAll()
        {
            var prescriptions = await _prescriptionService.GetAllPrescriptionsAsync();
            return Ok(prescriptions);
        }

        // GET: api/prescriptions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Prescription>> GetById(int id)
        {
            var prescription = await _prescriptionService.GetPrescriptionByIdAsync(id);
            if (prescription == null)
            {
                return NotFound();
            }
            return Ok(prescription);
        }

        // GET: api/prescriptions/patient/5
        [HttpGet("patient/{patientId}")]
        public ActionResult<IEnumerable<Prescription>> GetByPatientId(int patientId)
        {
            // This method isn't part of the interface yet, so we'll keep it as is for now
            // In a real application, we would add this to the interface and implement it
            var prescriptions = ((PrescriptionService)_prescriptionService).GetByPatientId(patientId);
            return Ok(prescriptions);
        }

        // POST: api/prescriptions
        [HttpPost]
        public async Task<ActionResult<Prescription>> Create(Prescription prescription)
        {
            Console.WriteLine($"------------ {prescription}");
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdPrescription = await _prescriptionService.CreatePrescriptionAsync(prescription);
            return CreatedAtAction(nameof(GetById), new { id = createdPrescription.Id }, createdPrescription);
        }

        // PUT: api/prescriptions/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Prescription prescription)
        {
            if (id != prescription.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var success = await _prescriptionService.UpdatePrescriptionAsync(id, prescription);
            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }

        // DELETE: api/prescriptions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _prescriptionService.DeletePrescriptionAsync(id);
            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
