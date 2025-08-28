import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Observable } from 'rxjs';
import { IEmpleado } from '../../interfaces/IEmpleado';
import { GenericService } from '../../services/generic.service';
import { environment } from '../../enviroments/environment';
import { TableModule } from 'primeng/table';
import { TagModule } from 'primeng/tag';
import { RatingModule } from 'primeng/rating';
import { DialogService, DynamicDialogRef } from 'primeng/dynamicdialog';
import { Toast } from 'primeng/toast';
import { ButtonModule } from 'primeng/button';
import { AltaEmpleadoComponent } from './alta/alta-empleado/alta-empleado.component';
import { MessageService } from 'primeng/api';
import { HttpClient } from '@angular/common/http';
@Component({
  selector: 'app-empleado',
  standalone: true,
  providers: [DialogService, MessageService],
  imports: [CommonModule, ButtonModule, TableModule, TagModule, RatingModule, Toast],
  templateUrl: './empleado.component.html',
  styleUrls: ['./empleado.component.css']
})
export class EmpleadoComponent implements OnInit {
  listaEmpleados: IEmpleado[] = [];
  totalRecords = 0;
  loading = true;
  ref?: DynamicDialogRef;
  apiDummyResult: any;
  loadingDummy = false;
  constructor(private genericService: GenericService,
    private dialogService: DialogService, private messageService: MessageService, private http: HttpClient) { }

  ngOnInit(): void {
    this.loadEmpleados();
  }

  loadDummy() {
    if (this.loadingDummy) return;
    this.loadingDummy = true;

    this.messageService.add({ severity: 'info', summary: 'API pública', detail: 'Consultando...' });

    this.http.get('https://svct.cartasur.com.ar/api/dummy', { responseType: 'text' })
      .subscribe({
        next: (resp) => {
          let detail = '';
          try { detail = JSON.stringify(JSON.parse(resp), null, 2); }
          catch { detail = resp; }

          this.messageService.add({
            severity: 'success',
            summary: 'API pública',
            detail: detail.length > 300 ? detail.slice(0, 300) + '…' : detail
          });
          this.loadingDummy = false;
        },
        error: (err) => {
          console.error('Error Dummy API:', err);
          this.messageService.add({
            severity: 'error',
            summary: 'API pública',
            detail: 'No se pudo consultar el servicio'
          });
          this.loadingDummy = false;
        }
      });
  }

  getSeverity(activo: boolean): "success" | "danger" {
    return activo ? "success" : "danger";
  }

  showNuevo() {
    this.ref = this.dialogService.open(AltaEmpleadoComponent, {
      header: 'Nuevo empleado',
      width: '40rem',
      modal: true,
      closable: true,
      breakpoints: { '960px': '90vw', '640px': '95vw' },
      styleClass: 'p-3'
    });

    this.ref.onClose.subscribe((empleadoCreado?: IEmpleado) => {
      if (empleadoCreado) {
        this.messageService.add({
          severity: 'success',
          summary: 'Alta exitosa',
          detail: `Empleado ${empleadoCreado.nombre} creado`
        });
        this.loadEmpleados();
      } else {

        this.messageService.add({ severity: 'info', summary: 'Cancelado', detail: 'No se creó el empleado' });
      }
    });
  }

  ngOnDestroy() {
    this.ref?.close();
  }


  getAll(search?: string, page = 1, size = 20, activo?: boolean) {
    const params: any = { page, size };

    if (search?.trim()) {
      params.search = search.trim();
    }
    if (activo !== undefined) {
      params.activo = activo;  
    }

    console.log('URL:', environment.baseUrl + 'empleados', 'params:', params);
    return this.genericService.getAll('empleados', params);
  }


  loadEmpleados(search?: string, page: number = 1, size: number = 20, activo?: boolean): void {
    this.loading = true;
    this.getAll(search, page, size, activo).subscribe({
      next: (response: any) => {
        // Si la API devuelve array plano:
        this.listaEmpleados = Array.isArray(response) ? response : (response.data ?? []);
        this.totalRecords = response?.meta?.total_registros ?? this.listaEmpleados.length;
        this.loading = false;
        console.log('Empleados cargados:', this.listaEmpleados);
      },
      error: (err) => {
        console.error('Error al traer empleados:', err);
        this.listaEmpleados = [];
        this.totalRecords = 0;
        this.loading = false;
      }
    });
  }

}
