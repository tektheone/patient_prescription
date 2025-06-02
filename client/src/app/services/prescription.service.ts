import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Prescription } from '../models/prescription.model';

@Injectable({
  providedIn: 'root'
})
export class PrescriptionService {
  private apiUrl = 'http://localhost:5049/api/prescriptions';

  constructor(private http: HttpClient) { }

  // Get all prescriptions
  getAllPrescriptions(): Observable<Prescription[]> {
    return this.http.get<Prescription[]>(this.apiUrl);
  }

  // Get prescription by ID
  getPrescriptionById(id: number): Observable<Prescription> {
    return this.http.get<Prescription>(`${this.apiUrl}/${id}`);
  }

  // Get prescriptions by patient ID
  getPrescriptionsByPatientId(patientId: number): Observable<Prescription[]> {
    return this.http.get<Prescription[]>(`${this.apiUrl}/patient/${patientId}`);
  }

  // Add new prescription
  addPrescription(prescription: Prescription): Observable<Prescription> {
    return this.http.post<Prescription>(this.apiUrl, prescription);
  }

  // Update prescription
  updatePrescription(prescription: Prescription): Observable<any> {
    return this.http.put(`${this.apiUrl}/${prescription.id}`, prescription);
  }

  // Delete prescription
  deletePrescription(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}
