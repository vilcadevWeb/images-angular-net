import { Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { CommonModule } from '@angular/common';

import { InputTextModule } from 'primeng/inputtext';
import { NgxDropzoneModule } from 'ngx-dropzone';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { ProductoService } from './services/producto.service';
import Swal from 'sweetalert2';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { Producto } from './interfaces/producto.interface';

import { environmentAzure } from './environment/env';



@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule,TableModule,ButtonModule,DialogModule,InputTextModule, NgxDropzoneModule, ReactiveFormsModule, HttpClientModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
  providers:[
    ProductoService, HttpClient
  ]
})
export class AppComponent implements OnInit{
  title = 'azureblobapp';


  visible: boolean = false;
  visibleEditar: boolean = false;

  files: File[] = [];
  disableZone: boolean = false;

  form!: FormGroup;

  productos: Producto[] =[];

  envAzure = '';
  varUrl: string='';


  constructor( public fb: FormBuilder, private productoService: ProductoService) {

    this.form = this.fb.group({
      id:[''],
      nombre:[''],
    });
  }

  ngOnInit() {
    this.listarProductos();
    this.envAzure = environmentAzure.url;
  }


listarProductos(){
  this.productoService.listarProductos().subscribe(
    (response)=>{

      this.productos = response;
    })
}

agregarProducto(){

  this.visible = false;
  const formData = new FormData();
      formData.append(
        'Nombre',
        this.form.get('nombre')?.value
    );

    if(this.files){
      formData.append(
          'ImagenUrl', this.files[0]
      )
  }

  console.log(formData);

  this.productoService.agregarProducto(formData).subscribe(
    () =>{
      Swal.fire('Producto Agregado Exitosamente','', 'success');
      this.listarProductos();

      return;

    }
  )
}

cargarProducto(producto:Producto){

  this.visibleEditar = true;

  this.form.patchValue({
    id:producto.id,
    nombre: producto.nombre,
  })

  const imagen = `${environmentAzure.url}${producto.imagenUrl}`;

  if(imagen && producto.imagenUrl){
    fetch(imagen)
    .then(response => response.blob())
    .then(blob => {
      const file = new File([blob], producto.nombre, { type: blob.type });
      this.files.push(file);


    });

    this.varUrl = producto.imagenUrl;
    } else{
      console.log("Hubo un error");
    }
}


editarProducto(){

  this.visibleEditar = false;

  const formData = new FormData();

  const productoId = this.form.get("id")?.value;

  formData.append('Nombre',this.form.get('nombre')?.value);

  if(this.files){
    formData.append('ImagenUrl',this.files[0]);
  }

  formData.append('Url',this.varUrl);

  this.productoService.editarProducto(formData, productoId).subscribe(
    () =>{
      Swal.fire('Producto Editado exitosamente','','success')
      this.listarProductos();
      this.varUrl ='';
    }
  )
}


eliminarProducto(productoId:number){

  Swal.fire({
    title: '¿Estas seguro que deseas eliminar?',
    showDenyButton: true,
    confirmButtonText: 'Eliminar',
    denyButtonText: `Cancelar`,
  }).then((result)=>{
    if(result.isConfirmed){
      this.productoService.eliminarProducto(productoId).subscribe(
        ()=>{
          Swal.fire('Producto eliminado exitosamente','','success');
          this.listarProductos();
        }
      )
    }
    else{
      return;
    }
  })
}




showDialog() {
  this.visible = true;
}


onSelect(event: any) {
  if (this.files.length === 1) {
    // Reemplazar el archivo existente con el nuevo archivo cargado
    this.files.splice(0, 1, ...event.addedFiles);
    this.disableZone = true;
  } else if (this.files.length === 0) {
    // Si no hay archivos, añadir el nuevo archivo
    this.files.push(...event.addedFiles);
    this.disableZone = true;
  } else {
    console.log('Ya hay más de un archivo');
  }
}

onRemove(event: any) {
  this.files.splice(this.files.indexOf(event), 1);
  this.disableZone = false;
}



resetFormProducto() {
  this.form.reset();
  this.files = [];
  this.disableZone = false;
}

}
