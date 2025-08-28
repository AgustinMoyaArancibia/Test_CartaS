
import { Component, inject } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { InputTextModule } from 'primeng/inputtext';
import { CheckboxModule } from 'primeng/checkbox';
import { ButtonModule } from 'primeng/button';
import { DynamicDialogRef, DynamicDialogConfig } from 'primeng/dynamicdialog';
import { GenericService } from '../../../../services/generic.service';

@Component({
  selector: 'app-alta-empleado',
  standalone: true,
  imports: [ReactiveFormsModule, InputTextModule, CheckboxModule, ButtonModule],
  templateUrl: './alta-empleado.component.html',
  styleUrls: ['./alta-empleado.component.css']
})
export class AltaEmpleadoComponent {
  private fb = inject(FormBuilder);
  ref = inject(DynamicDialogRef);    
  config = inject(DynamicDialogConfig); 
  private genericService = inject(GenericService);
  form = this.fb.group({
    nombre: ['', [Validators.required, Validators.maxLength(100)]],
    activo: [true]
  });

  guardar() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
     this.genericService.post('Empleados', this.form.value).subscribe({
      next: (empleadoCreado) => {
        console.log('Empleado creado:', empleadoCreado);
        this.ref.close(empleadoCreado); 
      },
      error: (err) => {
        console.error('Error creando empleado:', err);
      }
    });
  }

  cancelar() {
    this.ref.close(); 
  }
}
