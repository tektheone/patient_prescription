export interface Prescription {
  id?: number;
  patientId: number;
  patientName: string;
  medicationName: string;
  dosage: string;
  frequency: string;
  prescriptionDate: Date;
  endDate?: Date;
  prescribedBy: string;
  notes?: string;
}
