using Microsoft.AspNetCore.Mvc;
using server.Models;
using server.Services;
using System.Collections.Generic;

namespace server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PrescriptionsController : ControllerBase
    {
        private readonly PrescriptionService _prescriptionService;

        public PrescriptionsController(PrescriptionService prescriptionService)
        {
            _prescriptionService = prescriptionService;
        }

        // GET: api/prescriptions
        [HttpGet]
        public ActionResult<IEnumerable<Prescription>> GetAll()
        {
            return Ok(_prescriptionService.GetAll());
        }

        // GET: api/prescriptions/5
        [HttpGet("{id}")]
        public ActionResult<Prescription> GetById(int id)
        {
            var prescription = _prescriptionService.GetById(id);
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
            var prescriptions = _prescriptionService.GetByPatientId(patientId);
            return Ok(prescriptions);
        }

        // POST: api/prescriptions
        [HttpPost]
        public ActionResult<Prescription> Create(Prescription prescription)
        {
            Console.WriteLine($"------------ {prescription}");
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdPrescription = _prescriptionService.Add(prescription);
            return CreatedAtAction(nameof(GetById), new { id = createdPrescription.Id }, createdPrescription);
        }

        // PUT: api/prescriptions/5
        [HttpPut("{id}")]
        public IActionResult Update(int id, Prescription prescription)
        {
            if (id != prescription.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var success = _prescriptionService.Update(prescription);
            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }

        // DELETE: api/prescriptions/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var success = _prescriptionService.Delete(id);
            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
