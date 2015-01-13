using System;
using System.Web;
using System.IO;
using System.Configuration;
using System.Reflection;
using Efika.Crm.Negocio;
using Efika.Crm.AccesoServicios;
using Efika.Crm.Entidades;


namespace Efika.Crm.Web
{
    public partial class ImportarListaMarketing : System.Web.UI.Page
    {

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnImportar.OnClientClick = "if(Page_ClientValidate()){this.disabled=true; this.value='Importando...';" + 
                ClientScript.GetPostBackEventReference(this.btnImportar, "").ToString() + "}";
            if (!Page.IsPostBack)
            {
                if (!Utilidades.ValidarAccesosWeb(Request.QueryString[Utilidades.NombresParametros.Usuario], Privilegios.NombresPrivilegios.ImportacionListaMarketing))
                {
                    Response.Write(@"<script type=""text/javascript""> alert('No tiene privilegios para acceder a este formulario.'); window.close(); </script>");
                    return;
                }
                EnableControls();
            }
        }

        protected void btnImportar_Click(object sender, EventArgs e)
        {
            try
            {
                ProcesarArchivo();
            }
            catch (Exception ex)
            {
                Utilidades.ReportarError(this.Page, Request.QueryString[Utilidades.NombresParametros.Usuario], "Error al procesar archivo", "IMPORTACION_LISTAMARKETING", ex);
            }

        }

        #endregion

        #region Metodos

        private void ProcesarArchivo()
        {
            try
            {
                txtMensaje.Text = string.Empty;
                HttpPostedFile archivo = fupArchivo.PostedFile;
                string resultado = string.Empty;
                string NombreArchivo = archivo.FileName.Substring(archivo.FileName.LastIndexOf("\\"));
                string NombreCompletoArchivo = NombreArchivo;
                EscribirLog(string.Format("Guardando archivo {0}", Path.GetExtension(NombreArchivo)));
                
                MemoryStream copia = new MemoryStream();
                archivo.InputStream.CopyTo(copia);
                copia.Position = 0;

                EscribirLog("Validando archivo recibido");

                // leer archivo excel
                if (!ArchivoImportacionExcel.LeerArchivoImportacion(NombreCompletoArchivo, copia, 
                            ArchivoImportacionExcel.CABECERA_LISTA_MARKETING, ref resultado))
                {
                    // si no se pudo leer el archivo o no tiene la estructura correcta
                    EscribirLog("El archivo no tiene la estructura esperada");
                    EscribirLog("Ocurrieron errores en el proceso: ");
                    EscribirLog(resultado);
                    EscribirLog("La importación del archivo ha sido cancelada");
                    this.lbResultado.Text = "Error";
                    this.lbResultado.ForeColor = System.Drawing.Color.Red;
                    this.lbResultado.Visible = true;
                    return;
                }

                archivo.InputStream.Position = 0; //Retornamos al inicio para que el archivo pueda ser guardado
                EscribirLog("Grabando registro en lista de importación de marketing.");

                Entidades.ImportacionListaMarketing datosImpListMark = new Entidades.ImportacionListaMarketing
                {
                    Nombre = txtNombre.Text,
                    FechaInicioEjecucion = DateTime.Now,
                    RutaArchivoCarga = NombreCompletoArchivo,
                    IdPropietario = Request.QueryString["strCodigoUsuario"]
                };

                CredencialesCRM credenciales = Credenciales.ObtenerCredenciales();

                Negocio.ImportacionListaMarketing impListaMark = new Negocio.ImportacionListaMarketing(credenciales);

                Guid idRegistroImportacion = impListaMark.GuardarImpListaMarketing(datosImpListMark, ref resultado);
                EscribirLog("Registro almacenado. Iniciando proceso en segundo plano.");
                EscribirLog("El archivo está siendo procesado en segundo plano. Puede cerrar esta ventana.");

                Guid idNota = impListaMark.AdjuntarArchivoRegistroImportacion(idRegistroImportacion, archivo.InputStream, NombreArchivo);
                impListaMark.ActualizarListaMarketing(idRegistroImportacion, idNota);

                this.lbResultado.Text = "Correcto";
                this.lbResultado.ForeColor = System.Drawing.Color.Blue;
                this.lbResultado.Visible = true;
            }
            catch (Exception ex)
            {
                Utilidades.ReportarError(this.Page, Request.QueryString[Utilidades.NombresParametros.Usuario], 
                    "Error al procesar archivo de importación de lista de marketing.", "IMPORTAC_LISTAMARKETING", ex);
            }
        }

        private void EscribirLog(string Mensaje)
        {
            txtMensaje.Text += Mensaje + "\r\n";
        }

        

        #endregion

        #region Metodos Interfaz
        private void EnableControls()
        {
            fupArchivo.Enabled = true;
            txtNombre.Enabled = true;
            btnImportar.Enabled = true;
        }
        #endregion

    }
}