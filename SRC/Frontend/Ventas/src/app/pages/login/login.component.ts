import { Component, inject } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';

import { InputTextModule } from 'primeng/inputtext';
import { Password } from 'primeng/password';
import { ButtonModule } from 'primeng/button';
import { CheckboxModule } from 'primeng/checkbox';
import { Card } from 'primeng/card';
import { Toast } from 'primeng/toast';
import { MessageService } from 'primeng/api';

import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    ReactiveFormsModule, InputTextModule, Password, ButtonModule,
    CheckboxModule, Card, Toast
  ],
  providers: [MessageService],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  private fb = inject(FormBuilder);
  private auth = inject(AuthService);
  private router = inject(Router);
  private msg = inject(MessageService);

  loading = false;


  form = this.fb.group({
    nombre: ['', [Validators.required]],      
    pass: ['', [Validators.required]],        
    remember: [true]
  });

  submit() {
  if (this.form.invalid || this.loading) {
    this.form.markAllAsTouched();
    return;
  }
  this.loading = true;

  const { nombre, pass, remember } = this.form.getRawValue();

  this.auth.login(nombre!, pass!, remember!).subscribe({
    next: (ok) => {
      if (ok) {
        this.msg.add({ severity: 'success', summary: 'Bienvenido', detail: 'Login exitoso' });
        this.router.navigateByUrl('/empleados');
      } else {
        this.msg.add({ severity: 'warn', summary: 'Atención', detail: 'Respuesta sin token' });
      }
      this.loading = false;
    },
    error: (err) => {
      console.error('Login error:', err);
      this.msg.add({ severity: 'error', summary: 'Error', detail: 'No se pudo iniciar sesión' });
      this.loading = false;
    }
  });
}


}
