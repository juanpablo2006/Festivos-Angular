export interface Pais { id: number; nombre: string; }
export interface TipoFestivo { id: number; tipo: string; }
export interface Festivo { id: number; idPais: number; pais: string; nombre: string; dia: number|null; mes: number|null; diasPascua: number|null; idTipo: number; tipo: string; }
export type FestivoRequest = Pick<Festivo,'idPais'|'nombre'|'dia'|'mes'|'diasPascua'|'idTipo'>;
export interface Verificacion { fecha: string; esFestivo: boolean; festivos: string[]; mensaje: string; }
export interface FestivoAnual { idFestivo: number; nombre: string; fecha: string; idTipo: number; tipo: string; }
