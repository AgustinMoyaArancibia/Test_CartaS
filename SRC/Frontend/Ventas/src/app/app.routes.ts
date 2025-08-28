import { Routes } from '@angular/router';
import { LoginComponent } from './pages/login/login.component';
import { EmpleadoComponent } from './pages/empleado/empleado.component';

export const routes: Routes = [
    { path: '', redirectTo: 'login', pathMatch: 'full' },
    { path: 'login', component: LoginComponent },
    { path: 'empleados', component: EmpleadoComponent },
];
