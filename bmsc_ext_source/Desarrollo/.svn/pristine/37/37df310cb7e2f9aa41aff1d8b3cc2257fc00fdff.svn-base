using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ex = Net.SourceForge.Koogra.Excel;
using ex2 = Net.SourceForge.Koogra.Excel2007;
using Net.SourceForge.Koogra;

namespace Efika.Crm.Negocio
{
    public class ArchivoImportacionExcel
    {
        public static string[] CABECERA_LISTA_MARKETING = new string[] { "TIPO_IDENTIFICACION", "IDENTIFICACION", "NOMBRE", 
            "PRIMER_APELLIDO", "SEGUNDO_APELLIDO", "TIPO_CLIENTE", "TELEFONO_PRINCIPAL", "TELEFONO_TRABAJO", "CORREO_ELECTRONICO_PRINCIPAL", 
            "CORREO_ELECTRONICO_TRABAJO", "CIUDAD", "EJECUTIVO" };
        public static string[] CABECERA_PREAPROBADOS = new string[] { "IDENTIFICACION", "CODIGO_SUBTIPO_PRODUCTO", "NOMBRE_PRODUCTO",
            "CUPO_MONTO","TASA","PLAZO_DIAS","COMISION","ATRIBUTO_1","VALOR_1","ATRIBUTO_2","VALOR_2","ATRIBUTO_3","VALOR_3" };

        public static bool LeerArchivoImportacion(String NombreArchivo, Stream stream, string[] textoCabecera, ref string ResultadoError)
        {
            bool resultado = false;
            try
            {
                IWorkbook libro;
                IWorksheet hoja;
                if (Path.GetExtension(NombreArchivo).ToUpper() == ".XLS")
                {
                    libro = new ex.Workbook(stream);
                    hoja = libro.Worksheets.GetWorksheetByIndex(0);
                }
                else
                {
                    libro = new ex2.Workbook((Stream)stream);
                    hoja = libro.Worksheets.GetWorksheetByIndex(0);
                }

                //Validamos la cabecera del archivo
                IRow cabecera = hoja.Rows.GetRow(0);
                bool validacionCabecera = ValidarCabeceras(cabecera, ref ResultadoError, textoCabecera);
                if (!validacionCabecera)
                {
                    return false;
                }
                resultado = true;
            }
            catch (Exception ex)
            {
                resultado = false;
                ResultadoError = ex.Message;
            }
            return resultado;
        }


        private static bool ValidarCabeceras(IRow cabecera, ref string mensajeError, string[] texto)
        {
            uint contador = 0;
            foreach (string s in texto)
            {
                if (!(cabecera.GetCell(contador) != null && cabecera.GetCell(contador).Value != null
                    && cabecera.GetCell(contador).Value.ToString().Trim().ToUpper().Equals(s)))
                {
                    mensajeError = "No se encontró cabecera de columna" + " " + s;
                    return false;
                }
                contador++;
            }
            return true;
        }
    }
}
