using System;
using System.Web;
using System.IO;
using Microsoft.Xrm.Sdk;
using System.Configuration;
using System.Reflection;
using Efika.Crm.Negocio;
using Efika.Crm.AccesoServicios;
using Efika.Crm.Entidades;


namespace Efika.Crm.Web
{
    public partial class ImportarPreAprobado : System.Web.UI.Page
    {

        #region Propiedades

        public Guid IdCampania { get; set; }

        #endregion

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            btnImportar.OnClientClick = "if(Page_ClientValidate()){this.disabled=true; this.value='Importando...';" + 
                ClientScript.GetPostBackEventReference(btnImportar, "").ToString() + "}";
            if (!Page.IsPostBack)
            {
                if (!Utilidades.ValidarAccesosWeb(Request.QueryString[Utilidades.NombresParametros.Usuario], Privilegios.NombresPrivilegios.ImportacionPreAprobado))
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
                Utilidades.AgregarErrorAlLog(ex.Message, this.GetType().Name, MethodBase.GetCurrentMethod().Name,
                        User.Identity.Name, ex.StackTrace);
                Utilidades.ReportarError(this.Page, Request.QueryString[Utilidades.NombresParametros.Usuario], "Error al procesar archivo de importación.", "IMPORTAC_PREAPROBADO", ex);
            }

        }

        #endregion

        #region Métodos

        private void ProcesarArchivo()
        {
            try
            {
                // en producción, habilitar esta la siguiente líne y borrar la que el sigue
                IdCampania = new Guid(Request.QueryString["IdCampania"]);

                txtMensaje.Text = string.Empty;
                HttpPostedFile archivo = fupArchivo.PostedFile;
                string resultado = string.Empty;
                string NombreCompletoArchivo = archivo.FileName.Substring(archivo.FileName.LastIndexOf("\\"));
                EscribirLog(string.Format("Guardando archivo {0}", Path.GetExtension(NombreCompletoArchivo)));
                                
                MemoryStream copia = new MemoryStream();
                archivo.InputStream.CopyTo(copia);
                copia.Position = 0;

                EscribirLog("Validando archivo recibido");

                if(!ArchivoImportacionExcel.LeerArchivoImportacion(NombreCompletoArchivo, copia, 
                            ArchivoImportacionExcel.CABECERA_PREAPROBADOS, ref resultado))
                {
                    // si no se puede abrir el archivo o el formato es incorrecto, salir.
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
                EscribirLog("Grabando registro.");

                AccesoServicios.CRMSDK.campaign campania = new AccesoServicios.CRMSDK.campaign();
                CredencialesCRM credenciales = Credenciales.ObtenerCredenciales();
                AccesoServicios.CRMSDK.CrmService Servicio = AccesoServicios.ServicioCRM.ObtenerServicioCRM(credenciales);

                AccesoServicios.CRMSDK.ColumnSet campaignCols = new AccesoServicios.CRMSDK.ColumnSet();
                campaignCols.Attributes = new string[] {"name"};

                campania = (AccesoServicios.CRMSDK.campaign)Servicio.Retrieve(AccesoServicios.CRMSDK.EntityName.campaign.ToString(), 
                                IdCampania, campaignCols);

                Entidades.ImportacionDatosPreAprobados datosPreAp = new Entidades.ImportacionDatosPreAprobados
                {
                    Nombre = string.Format("{0} - {1}", campania.name, DateTime.Now.ToString(System.Globalization.CultureInfo.InvariantCulture)),
                    FechaInicioEjecucion = DateTime.Now,
                    RutaArchivoCarga = NombreCompletoArchivo,
                    IdCampania = IdCampania,
                    IdPropietario = Request.QueryString["strCodigoUsuario"]
                };

                Negocio.ImportacionDatosPreAprobados impDatPreAp = new Negocio.ImportacionDatosPreAprobados(credenciales);
                Guid idRegistroImportacion = impDatPreAp.GuardarListaPreAprobado(datosPreAp, ref resultado);
                EscribirLog("Registro almacenado. Iniciando proceso en segundo plano.");
                EscribirLog("El archivo está siendo procesado en segundo plano. Puede cerrar esta ventana.");

                Guid idNota = impDatPreAp.AdjuntarArchivoRegistroImportacion(idRegistroImportacion, archivo.InputStream, NombreCompletoArchivo);
                impDatPreAp.ActualizarImportacionDatosPreAprobados(idRegistroImportacion, idNota);

                this.lbResultado.Text = "Correcto";
                this.lbResultado.ForeColor = System.Drawing.Color.Blue;
                this.lbResultado.Visible = true;
            }
            catch (Exception ex)
            {
                throw ex;
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
            btnImportar.Enabled = true;
        }
        #endregion


    }
}