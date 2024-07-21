import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../environment/env';
import { Observable } from 'rxjs';
import { Producto } from '../interfaces/producto.interface';

@Injectable({
  providedIn: 'root'
})
export class ProductoService {
  private endpoint: string ;

  constructor(private http: HttpClient) {

    this.endpoint = environment.endPoint
  }


  listarProductos():Observable<Producto[]>{
    return this.http.get<Producto[]>(`${this.endpoint}/api/Producto`);
  }

  agregarProducto(producto: FormData){
    return this.http.post<any>(`${this.endpoint}/api/Producto`,producto)
  }

  editarProducto(producto:FormData, productoId: number):Observable<any>{

    return this.http.put<any>(`${this.endpoint}/api/Producto/${productoId}`,producto);
  }

  eliminarProducto(productoId:number):Observable<any>{
    return this.http.delete(`${this.endpoint}/api/Producto/${productoId}`);
  }
}

