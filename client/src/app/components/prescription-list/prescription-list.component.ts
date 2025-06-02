import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { Prescription } from '../../models/prescription.model';
import { PrescriptionService } from '../../services/prescription.service';

@Component({
  selector: 'app-prescription-list',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './prescription-list.component.html',
  styleUrls: ['./prescription-list.component.scss']
})
export class PrescriptionListComponent implements OnInit {
  prescriptions: Prescription[] = [];
  filteredPrescriptions: Prescription[] = [];
  patientIdFilter: number | null = null;
  loading = false;
  error = '';

  constructor(private prescriptionService: PrescriptionService) { }

  ngOnInit(): void {
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
    if (this.patientIdFilter) {
      this.loading = true;
      this.error = '';
      
      this.prescriptionService.getPrescriptionsByPatientId(this.patientIdFilter).subscribe({
        next: (data) => {
          this.filteredPrescriptions = data;
          this.loading = false;
        },
        error: (err) => {
          this.error = 'Failed to filter prescriptions. Please try again later.';
          console.error('Error filtering prescriptions:', err);
          this.loading = false;
        }
      });
    } else {
      this.filteredPrescriptions = this.prescriptions;
    }
  }

  clearFilter(): void {
    this.patientIdFilter = null;
    this.filteredPrescriptions = this.prescriptions;
  }

  formatDate(date: Date): string {
    return new Date(date).toLocaleDateString();
  }

  deletePrescription(id: number): void {
    if (confirm('Are you sure you want to delete this prescription?')) {
      this.loading = true;
      this.prescriptionService.deletePrescription(id).subscribe({
        next: () => {
          this.loadPrescriptions();
        },
        error: (err) => {
          this.error = 'Failed to delete prescription. Please try again later.';
          console.error('Error deleting prescription:', err);
          this.loading = false;
        }
      });
    }
  }
}
