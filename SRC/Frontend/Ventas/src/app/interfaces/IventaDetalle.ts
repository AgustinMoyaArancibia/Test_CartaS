export interface VentaDetalle {
  idProducto: number;
  producto: string;
  cantidad: number;
  precioUnitario: number; // si tu backend manda string para decimales, cambiá a string
  subTotal: number;       // idem arriba
}