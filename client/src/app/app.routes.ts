import { Routes } from '@angular/router';
import { PrescriptionListComponent } from './components/prescription-list/prescription-list.component';
import { AddPrescriptionComponent } from './components/add-prescription/add-prescription.component';

export const routes: Routes = [
  { path: '', component: PrescriptionListComponent },
  { path: 'add', component: AddPrescriptionComponent },
  { path: '**', redirectTo: '' }
];
