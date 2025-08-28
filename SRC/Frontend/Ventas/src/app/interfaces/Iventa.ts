import { VentaDetalle } from "./IventaDetalle";

export interface Venta {
  idVenta: number;
  fechaVenta: string;     
                         
  idCliente: number;
  cliente: string;
  idEmpleado: number;
  empleado: string;
  idSucursal: number;
  sucursal: string;
  importeTotal: number;  
  detalles: VentaDetalle[];
}