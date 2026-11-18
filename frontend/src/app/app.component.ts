import { CommonModule } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';
import { Festivo, FestivoAnual, FestivoRequest, Pais, TipoFestivo, Verificacion } from './models';

type Pagina = 'inicio'|'paises'|'tipos'|'festivos'|'consulta';
@Component({selector:'app-root',standalone:true,imports:[CommonModule,FormsModule],templateUrl:'./app.component.html'})
export class AppComponent implements OnInit {
 api=inject(ApiService);
 pagina:Pagina='inicio'; menu:{key:Pagina; label:string; icon:string}[]=[
 {key:'inicio',label:'Inicio',icon:'⌂'},{key:'paises',label:'Países',icon:'◎'},
 {key:'tipos',label:'Tipos de festivos',icon:'▦'},{key:'festivos',label:'Festivos',icon:'▣'},
 {key:'consulta',label:'Consultar fechas',icon:'◷'}];
 paises:Pais[]=[]; tipos:TipoFestivo[]=[]; festivos:Festivo[]=[]; anuales:FestivoAnual[]=[];
 cargando=false; guardando=false; error=''; exito=''; filtro='';
 paisForm:{id:number|null;nombre:string}={id:null,nombre:''};
 tipoForm:{id:number|null;tipo:string}={id:null,tipo:''};
 festivoForm:FestivoRequest & {id:number|null}={id:null,idPais:1,nombre:'',idTipo:1,dia:null,mes:null,diasPascua:null};
 mostrarForm=false;
 consultaPais=1; consultaFecha=new Date().toISOString().slice(0,10); consultaAnio=new Date().getFullYear(); resultado:Verificacion|null=null;
 ngOnInit(){this.actualizar();}
 ir(p:Pagina){this.pagina=p;this.error='';this.exito='';this.filtro='';this.mostrarForm=false;this.resultado=null;this.actualizar();}
 actualizar(){this.cargando=true;this.api.paises().subscribe({next:x=>{this.paises=x;if(x.length && !x.some(p=>p.id===this.consultaPais))this.consultaPais=x[0].id;},error:e=>this.falla(e)});
 this.api.tipos().subscribe({next:x=>this.tipos=x,error:e=>this.falla(e)});
 this.api.festivos().subscribe({next:x=>{this.festivos=x;this.cargando=false;},error:e=>{this.cargando=false;this.falla(e)}});}
 falla(e:unknown){const err=e as HttpErrorResponse;this.guardando=false;this.error=err.error?.detail || err.error?.title || (err.status===0?'No se pudo conectar con la API. Ejecuta el backend en localhost:5080.':'Ocurrió un error en la solicitud.');this.exito='';}
 ok(msg:string){this.exito=msg;this.error='';this.guardando=false;this.mostrarForm=false;this.actualizar();}
 get paisesFiltrados(){return this.paises.filter(x=>x.nombre.toLowerCase().includes(this.filtro.toLowerCase()));}
 get tiposFiltrados(){return this.tipos.filter(x=>x.tipo.toLowerCase().includes(this.filtro.toLowerCase()));}
 get festivosFiltrados(){return this.festivos.filter(x=>(x.nombre+' '+x.pais+' '+x.tipo).toLowerCase().includes(this.filtro.toLowerCase()));}
 nuevo(){this.mostrarForm=true;this.error='';this.exito='';this.paisForm={id:null,nombre:''};this.tipoForm={id:null,tipo:''};this.festivoForm={id:null,idPais:this.paises[0]?.id ?? 1,nombre:'',idTipo:1,dia:null,mes:null,diasPascua:null};}
 editarPais(x:Pais){this.paisForm={...x};this.mostrarForm=true;}
 editarTipo(x:TipoFestivo){this.tipoForm={...x};this.mostrarForm=true;}
 editarFestivo(x:Festivo){this.festivoForm={id:x.id,idPais:x.idPais,nombre:x.nombre,dia:x.dia,mes:x.mes,diasPascua:x.diasPascua,idTipo:x.idTipo};this.mostrarForm=true;}
 guardarPais(){const x=this.paisForm;if(!x.nombre.trim())return;this.guardando=true;const op=x.id===null?this.api.crearPais(x.nombre):this.api.editarPais(x.id,x.nombre);op.subscribe({next:()=>this.ok('País guardado correctamente.'),error:e=>this.falla(e)});}
 guardarTipo(){const x=this.tipoForm;if(!x.tipo.trim())return;this.guardando=true;const op=x.id===null?this.api.crearTipo(x.tipo):this.api.editarTipo(x.id,x.tipo);op.subscribe({next:()=>this.ok('Tipo guardado correctamente.'),error:e=>this.falla(e)});}
 get usaPascua(){return this.festivoForm.idTipo===3||this.festivoForm.idTipo===4;}
 cambiaTipo(){if(this.usaPascua){this.festivoForm.dia=null;this.festivoForm.mes=null;}else{this.festivoForm.diasPascua=null;}}
 guardarFestivo(){const {id,...datos}=this.festivoForm;if(!datos.nombre.trim()||!datos.idPais||!datos.idTipo)return;this.guardando=true;
 const op=id===null?this.api.crearFestivo(datos):this.api.editarFestivo(id,datos);op.subscribe({next:()=>this.ok('Festivo guardado correctamente.'),error:e=>this.falla(e)});}
 eliminar(recurso:'país'|'tipo'|'festivo',id:number,nombre:string){if(!confirm(`¿Eliminar ${recurso} «${nombre}»?`))return;
 const op:Observable<void>=recurso==='país'?this.api.borrarPais(id):recurso==='tipo'?this.api.borrarTipo(id):this.api.borrarFestivo(id);
 op.subscribe({next:()=>this.ok(`${recurso} eliminado correctamente.`),error:e=>this.falla(e)});}
 verificar(){if(!this.consultaFecha||!this.consultaPais)return;this.resultado=null;this.error='';this.api.verificar(this.consultaPais,this.consultaFecha).subscribe({next:x=>this.resultado=x,error:e=>this.falla(e)});}
 listarAnio(){this.error='';this.anuales=[];this.api.anuales(this.consultaPais,this.consultaAnio).subscribe({next:x=>this.anuales=x,error:e=>this.falla(e)});}
}
