import { VentaDetalle } from "./IventaDetalle";

export interface Venta {
  idVenta: number;
  fechaVenta: string;     // ISO string (ej: "2025-08-22T09:10:00"). 
                          // Si preferís Date en el front: Date | string
  idCliente: number;
  cliente: string;
  idEmpleado: number;
  empleado: string;
  idSucursal: number;
  sucursal: string;
  importeTotal: number;   // si viene como string -> cambiar a string
  detalles: VentaDetalle[];
}