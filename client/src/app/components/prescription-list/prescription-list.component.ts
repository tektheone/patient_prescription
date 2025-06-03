import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { Prescription } from '../../models/prescription.model';
import { PrescriptionService } from '../../services/prescription.service';
// Using CSS animations instead of Angular animations
declare var bootstrap: any;

@Component({
  selector: 'app-prescription-list',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './prescription-list.component.html',
  styleUrls: ['./prescription-list.component.scss'],
  // Using CSS animations instead
})
export class PrescriptionListComponent implements OnInit {
  prescriptions: Prescription[] = [];
  filteredPrescriptions: Prescription[] = [];
  patientIdFilter: string = '';
  loading = false;
  error = '';
  success = '';
  prescriptionToDelete: Prescription | null = null;
  showDeleteModal = false;
  modalClosing = false;

  constructor(
    private prescriptionService: PrescriptionService,
    private router: Router
  ) { }

  ngOnInit(): void {
    // Check if we're returning from a successful add/edit operation
    const operation = localStorage.getItem('prescriptionOperation');
    if (operation) {
      this.loading = true;
      // Clear the flag immediately
      localStorage.removeItem('prescriptionOperation');
    }
    
    this.loadPrescriptions();
  }

  loadPrescriptions(): void {
    this.loading = true;
    this.error = '';
    
    this.prescriptionService.getAllPrescriptions().subscribe({
      next: (data) => {
        this.prescriptions = data;
        this.filteredPrescriptions = data;
        this.loading = false;
      },
      error: (err) => {
        console.log(err)
        this.error = 'Failed to load prescriptions. Please try again later.';
        console.error('Error loading prescriptions:', err);
        this.loading = false;
      }
    });
  }

  filterByPatientId(): void {
    if (this.patientIdFilter && this.patientIdFilter.trim() !== '') {
      // Convert filter to string for comparison and trim whitespace
      const filterValue = this.patientIdFilter.trim().toLowerCase();
      
      // Filter client-side with partial matching
      this.filteredPrescriptions = this.prescriptions.filter(prescription => {
        // Convert patientId to string for partial matching
        const patientIdStr = prescription.patientId.toString();
        return patientIdStr.includes(filterValue);
      });
      
      // Show a message if no prescriptions match the filter
      if (this.filteredPrescriptions.length === 0) {
        this.error = `No prescriptions found for Patient ID containing: ${this.patientIdFilter}`;
      } else {
        this.error = '';
      }
    } else {
      // If no filter is applied, show all prescriptions
      this.filteredPrescriptions = this.prescriptions;
      this.error = '';
    }
  }

  clearFilter(): void {
    this.patientIdFilter = '';
    this.filteredPrescriptions = this.prescriptions;
    this.error = '';
  }

  formatDate(date: Date): string {
    return new Date(date).toLocaleDateString();
  }

  editPrescription(id: number): void {
    if (id) {
      this.router.navigate(['/edit', id]);
    }
  }

  openDeleteModal(prescription: Prescription): void {
    this.prescriptionToDelete = prescription;
    this.showDeleteModal = true;
  }
  
  cancelDelete(): void {
    // Start closing animation
    this.modalClosing = true;
    
    // Wait for animation to complete before hiding the modal
    setTimeout(() => {
      this.showDeleteModal = false;
      this.modalClosing = false;
      this.prescriptionToDelete = null;
    }, 500);
  }

  confirmDelete(): void {
    if (this.prescriptionToDelete && this.prescriptionToDelete.id) {
      this.loading = true;
      
      // Start closing animation
      this.modalClosing = true;
      
      // Store the ID before clearing the prescription object
      const idToDelete = this.prescriptionToDelete.id;
      
      // Wait for animation to complete before hiding the modal and making the API call
      setTimeout(() => {
        this.showDeleteModal = false;
        this.modalClosing = false;
        this.prescriptionService.deletePrescription(idToDelete).subscribe({
          next: () => {
            this.loadPrescriptions();
            this.prescriptionToDelete = null;
          },
          error: (err) => {
            this.error = 'Failed to delete prescription. Please try again later.';
            console.error('Error deleting prescription:', err);
            this.loading = false;
            this.prescriptionToDelete = null;
          }
        });
      }, 300);
    }
  }
}
