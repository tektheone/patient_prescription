import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { PrescriptionService } from '../../services/prescription.service';

@Component({
  selector: 'app-add-prescription',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './add-prescription.component.html',
  styleUrls: ['./add-prescription.component.scss']
})
export class AddPrescriptionComponent {
  prescriptionForm: FormGroup;
  loading = false;
  error = '';
  success = '';

  constructor(
    private fb: FormBuilder,
    private prescriptionService: PrescriptionService,
    private router: Router
  ) {
    this.prescriptionForm = this.fb.group({
      patientId: ['', [Validators.required, Validators.pattern('^[0-9]+$')]],
      patientName: ['', [Validators.required, Validators.minLength(2)]],
      medicationName: ['', [Validators.required, Validators.minLength(2)]],
      dosage: ['', [Validators.required]],
      frequency: ['', [Validators.required]],
      prescriptionDate: [new Date().toISOString().substring(0, 10), [Validators.required]],
      endDate: [''],
      prescribedBy: ['', [Validators.required, Validators.minLength(2)]],
      notes: ['']
    });
  }

  onSubmit(): void {
    if (this.prescriptionForm.invalid) {
      this.markFormGroupTouched(this.prescriptionForm);
      return;
    }

    this.loading = true;
    this.error = '';
    this.success = '';

    const prescriptionData = this.prescriptionForm.value;
    
    // Convert string dates to Date objects
    if (prescriptionData.prescriptionDate) {
      prescriptionData.prescriptionDate = new Date(prescriptionData.prescriptionDate);
    }
    
    if (prescriptionData.endDate) {
      prescriptionData.endDate = new Date(prescriptionData.endDate);
    }

    this.prescriptionService.addPrescription(prescriptionData).subscribe({
      next: () => {
        this.success = 'Prescription added successfully!';
        this.loading = false;
        
        // Navigate back to the list after a short delay
        setTimeout(() => {
          this.router.navigate(['/']);
        }, 1500);
      },
      error: (err) => {
        this.error = 'Failed to add prescription. Please try again.';
        console.error('Error adding prescription:', err);
        this.loading = false;
      }
    });
  }

  // Helper method to mark all controls as touched
  markFormGroupTouched(formGroup: FormGroup): void {
    Object.values(formGroup.controls).forEach(control => {
      control.markAsTouched();
      
      if ((control as any).controls) {
        this.markFormGroupTouched(control as FormGroup);
      }
    });
  }

  // Reset the form
  resetForm(): void {
    this.prescriptionForm.reset({
      prescriptionDate: new Date().toISOString().substring(0, 10)
    });
    this.error = '';
    this.success = '';
  }

  // Navigate back to the list
  goBack(): void {
    this.router.navigate(['/']);
  }
}
