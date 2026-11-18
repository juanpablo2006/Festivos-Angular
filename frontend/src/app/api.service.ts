import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Festivo, FestivoAnual, FestivoRequest, Pais, TipoFestivo, Verificacion } from './models';
@Injectable({providedIn:'root'})
export class ApiService {
 private http=inject(HttpClient);
 paises(){return this.http.get<Pais[]>('/api/paises');}
 crearPais(nombre:string){return this.http.post<Pais>('/api/paises',{nombre});}
 editarPais(id:number,nombre:string){return this.http.put<Pais>(`/api/paises/${id}`,{nombre});}
 borrarPais(id:number){return this.http.delete<void>(`/api/paises/${id}`);}
 tipos(){return this.http.get<TipoFestivo[]>('/api/tipos-festivo');}
 crearTipo(tipo:string){return this.http.post<TipoFestivo>('/api/tipos-festivo',{tipo});}
 editarTipo(id:number,tipo:string){return this.http.put<TipoFestivo>(`/api/tipos-festivo/${id}`,{tipo});}
 borrarTipo(id:number){return this.http.delete<void>(`/api/tipos-festivo/${id}`);}
 festivos(){return this.http.get<Festivo[]>('/api/festivos');}
 crearFestivo(datos:FestivoRequest){return this.http.post<Festivo>('/api/festivos',datos);}
 editarFestivo(id:number,datos:FestivoRequest){return this.http.put<Festivo>(`/api/festivos/${id}`,datos);}
 borrarFestivo(id:number){return this.http.delete<void>(`/api/festivos/${id}`);}
 verificar(idPais:number,fecha:string){const [y,m,d]=fecha.split('-').map(Number);return this.http.get<Verificacion>(`/api/festivos/verificar/${y}/${m}/${d}`,{params:{idPais}});}
 anuales(idPais:number,anio:number){return this.http.get<FestivoAnual[]>(`/api/festivos/obtener/${anio}`,{params:{idPais}});}
}
