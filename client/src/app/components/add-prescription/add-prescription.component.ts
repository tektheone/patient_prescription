import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators, ValidationErrors } from '@angular/forms';
import { PrescriptionService } from '../../services/prescription.service';

@Component({
  selector: 'app-add-prescription',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './add-prescription.component.html',
  styleUrls: ['./add-prescription.component.scss']
})
export class AddPrescriptionComponent implements OnInit {
  prescriptionForm: FormGroup;
  loading = false;
  error = '';
  success = '';
  isEditMode = false;
  prescriptionId: number | null = null;
  pageTitle = 'Add New Prescription';
  submitButtonText = 'Save Prescription';

  constructor(
    private fb: FormBuilder,
    private prescriptionService: PrescriptionService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.prescriptionForm = this.fb.group({
      patientId: ['', [Validators.required, Validators.pattern('^[0-9]+$'), Validators.maxLength(10)]],
      patientName: ['', [Validators.required, Validators.minLength(2), Validators.pattern('^[a-zA-Z \.-]+$')]],
      medicationName: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(20), Validators.pattern('^[a-zA-Z0-9 \.-]+$')]],
      dosage: ['', [Validators.required, Validators.pattern('^[0-9]+(\.[0-9]+)?$'), Validators.maxLength(10)]],
      frequency: ['', [Validators.required, Validators.pattern('^[0-9]+$'), Validators.maxLength(10)]],
      prescriptionDate: [new Date().toISOString().substring(0, 10), [Validators.required]],
      endDate: [new Date().toISOString().substring(0, 10), [Validators.required]],
      prescribedBy: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(20), Validators.pattern('^[a-zA-Z \.-]+$')]],
      notes: ['', [Validators.maxLength(500)]]
    }, { validators: this.validateDates });
  }

  ngOnInit(): void {
    // Check if we're in edit mode by looking for an ID parameter in the route
    this.route.paramMap.subscribe(params => {
      const id = params.get('id');
      if (id) {
        this.isEditMode = true;
        this.prescriptionId = +id;
        this.pageTitle = 'Edit Prescription';
        this.submitButtonText = 'Update Prescription';
        this.loadPrescription(+id);
      }
    });
  }

  loadPrescription(id: number): void {
    this.loading = true;
    this.prescriptionService.getPrescriptionById(id).subscribe({
      next: (prescription) => {
        // Format dates for the form
        const formattedPrescription = {
          ...prescription,
          prescriptionDate: new Date(prescription.prescriptionDate).toISOString().substring(0, 10),
          endDate: prescription.endDate ? new Date(prescription.endDate).toISOString().substring(0, 10) : ''
        };
        
        this.prescriptionForm.patchValue(formattedPrescription);
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Failed to load prescription. Please try again.';
        console.error('Error loading prescription:', err);
        this.loading = false;
      }
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
    
    // Ensure dosage is a string (numeric value only, no units)
    if (prescriptionData.dosage) {
      // Make sure it's a string and remove any non-numeric characters
      prescriptionData.dosage = prescriptionData.dosage.toString().trim();
    }
    
    // Format dates as ISO strings for proper serialization
    if (prescriptionData.prescriptionDate) {
      const date = new Date(prescriptionData.prescriptionDate);
      prescriptionData.prescriptionDate = date.toISOString();
    }
    
    if (prescriptionData.endDate) {
      const date = new Date(prescriptionData.endDate);
      prescriptionData.endDate = date.toISOString();
    }

    if (this.isEditMode && this.prescriptionId) {
      // Ensure the ID is set correctly for update
      prescriptionData.id = this.prescriptionId;
      
      this.prescriptionService.updatePrescription(prescriptionData).subscribe({
        next: () => {
          this.success = 'Prescription updated successfully!';
          
          
          // Set flag in localStorage to indicate successful update
          localStorage.setItem('prescriptionOperation', 'updated');
          
          // Navigate back to the list after a short delay
          setTimeout(() => {
            this.router.navigate(['/']);
            this.loading = false;
          }, 1000);
          
        },
        error: (err) => {
          this.error = 'Failed to update prescription. Please try again.';
          console.error('Error updating prescription:', err);
          this.loading = false;
        }
      });
    } else {
      this.prescriptionService.addPrescription(prescriptionData).subscribe({
        next: () => {
          this.success = 'Prescription added successfully!';
          
          
          // Set flag in localStorage to indicate successful add
          localStorage.setItem('prescriptionOperation', 'added');
          
          // Navigate back to the list after a short delay
          setTimeout(() => {
            this.router.navigate(['/']);
            this.loading = false;
          }, 1000);
      },
      error: (err) => {
        this.error = 'Failed to add prescription. Please try again.';
        console.error('Error adding prescription:', err);
        this.loading = false;
      }
    });
    }
  }

  // Helper method to mark all controls as touched
  markFormGroupTouched(formGroup: FormGroup): void {
    Object.values(formGroup.controls).forEach((control: any) => {
      control.markAsTouched();
      
      if (control.controls) {
        this.markFormGroupTouched(control as FormGroup);
      }
    });
  }

  // Reset the form
  resetForm(): void {
    this.prescriptionForm.reset({
      prescriptionDate: new Date().toISOString().substring(0, 10),
      endDate: new Date().toISOString().substring(0, 10)
    });
    this.error = '';
    this.success = '';
  }

  // Navigate back to the list
  goBack(): void {
    // Set flag to show loading state when navigating back
    localStorage.setItem('prescriptionOperation', 'canceled');
    this.router.navigate(['/']);
  }

  // Validate Patient Name and Prescribed By to only allow letters, spaces, and punctuation
  onLettersOnlyInput(event: KeyboardEvent, fieldName: string): boolean {
    const input = event.target as HTMLInputElement;
    const maxLengths = {
      'patientName': 20,
      'medicationName': 20,
      'prescribedBy': 20
    };
    
    // Check max length - prevent typing if max length reached
    if (input.value.length >= maxLengths[fieldName as keyof typeof maxLengths] && 
        !['Backspace', 'Delete', 'ArrowLeft', 'ArrowRight', 'Tab'].includes(event.key)) {
      event.preventDefault();
      return false;
    }
    
    // Allow only letters, spaces, periods, and hyphens
    const allowedKeys = /[A-Za-z\s.-]|Backspace|Delete|ArrowLeft|ArrowRight|Tab/;
    
    if (!allowedKeys.test(event.key)) {
      event.preventDefault();
      return false;
    }
    return true;
  }

  // Validate Patient ID and Frequency to only allow numbers
  onNumbersOnlyInput(event: KeyboardEvent, fieldName: string): boolean {
    const input = event.target as HTMLInputElement;
    const maxLengths = {
      'patientId': 10,
      'frequency': 2
    };
    
    // Check max length - prevent typing if max length reached
    if (input.value.length >= maxLengths[fieldName as keyof typeof maxLengths] && 
        !['Backspace', 'Delete', 'ArrowLeft', 'ArrowRight', 'Tab'].includes(event.key)) {
      event.preventDefault();
      return false;
    }
    
    // Allow only numbers and navigation keys
    const allowedKeys = /[0-9]|Backspace|Delete|ArrowLeft|ArrowRight|Tab/;
    
    if (!allowedKeys.test(event.key)) {
      event.preventDefault();
      return false;
    }
    return true;
  }
  
  // Validate Dosage to allow numbers with optional decimal point
  onDosageInput(event: KeyboardEvent): boolean {
    const input = event.target as HTMLInputElement;
    const maxLength = 5;
    
    // Check max length - prevent typing if max length reached
    if (input.value.length >= maxLength && 
        !['Backspace', 'Delete', 'ArrowLeft', 'ArrowRight', 'Tab'].includes(event.key)) {
      event.preventDefault();
      return false;
    }
    
    // Allow only numbers, decimal point, and navigation keys
    const allowedKeys = /[0-9.]|Backspace|Delete|ArrowLeft|ArrowRight|Tab/;
    
    // Check if the key is allowed
    if (!allowedKeys.test(event.key)) {
      event.preventDefault();
      return false;
    }
    
    // Special handling for decimal point
    if (event.key === '.' && input.value.includes('.')) {
      // Prevent adding a second decimal point
      event.preventDefault();
      return false;
    }
    
    return true;
  }

  // Validate notes field max length
  onNotesInput(event: KeyboardEvent): boolean {
    const input = event.target as HTMLInputElement;
    const maxLength = 500;
    
    // Check max length - prevent typing if max length reached
    if (input.value.length >= maxLength && 
        !['Backspace', 'Delete', 'ArrowLeft', 'ArrowRight', 'Tab'].includes(event.key)) {
      event.preventDefault();
      return false;
    }
    
    return true;
  }

  // Validate medication name to allow both letters and numbers
  onMedicationNameInput(event: KeyboardEvent): boolean {
    const input = event.target as HTMLInputElement;
    const maxLength = 20;
    
    // Check max length - prevent typing if max length reached
    if (input.value.length >= maxLength && 
        !['Backspace', 'Delete', 'ArrowLeft', 'ArrowRight', 'Tab'].includes(event.key)) {
      event.preventDefault();
      return false;
    }
    
    // Allow letters, numbers, spaces, periods, and hyphens
    const allowedKeys = /[A-Za-z0-9\s.-]|Backspace|Delete|ArrowLeft|ArrowRight|Tab/;
    
    if (!allowedKeys.test(event.key)) {
      event.preventDefault();
      return false;
    }
    return true;
  }

  // Custom validator to ensure endDate is not before prescriptionDate
  validateDates(formGroup: FormGroup): ValidationErrors | null {
    const prescriptionDate = formGroup.get('prescriptionDate')?.value;
    const endDate = formGroup.get('endDate')?.value;

    if (prescriptionDate && endDate) {
      const startDate = new Date(prescriptionDate);
      const end = new Date(endDate);

      if (end < startDate) {
        return { 'endDateBeforePrescriptionDate': true };
      }
    }

    return null;
  }
}
