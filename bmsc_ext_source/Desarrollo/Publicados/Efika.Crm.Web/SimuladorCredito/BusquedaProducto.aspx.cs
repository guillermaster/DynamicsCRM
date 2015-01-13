using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Reflection;
using Efika.Crm.AccesoServicios.CRMSDK;
using Efika.Crm.Negocio;


namespace Efika.Crm.Web.SimuladorCredito
{
    public partial class BusquedaProducto : System.Web.UI.Page
    {
        private const string SESSION_NOMBRES_PRODUCTOS = "nombres_productos_encontrados";


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!Utilidades.ValidarAccesosWeb(Request.QueryString[Utilidades.NombresParametros.Usuario], Privilegios.NombresPrivilegios.SimulacionCredito))
                {
                    Response.Write(@"<script type=""text/javascript""> alert('No tiene privilegios para acceder a este formulario.'); window.close(); </script>");
                    return;
                }
                //almacenar en hidden fields los nombres de los campos de la ventana padre a los que se les asignará los valores del producto seleccionado
                Workspace1.ShowLeftPanel = false;
                LeerQueryStringsProducto();
                //mostrar todos los productos de una familia y tipo
                BuscarProducto(string.Empty, FamiliasProductos(), TiposProductos());
                Page.SetFocus(txtProducto);
            }
        }



        private void BuscarProducto(string nombreProducto, string[] familiasProductos, string[] tiposProductos)
        {
            try
            {
                ColumnSet csProd = new ColumnSet();
                csProd.Attributes = new string[] { "productid", "name", "efk_familia_productosid", "efk_tipo_productoid", "efk_habilitado_comercializar" };

                Negocio.Producto negProd = new Negocio.Producto(Credenciales.ObtenerCredenciales());
                List<Entidades.Producto> productosEncontrados;
                if (hdnValHomologac.Value == "1")
                    productosEncontrados = negProd.BuscarProducto(nombreProducto, familiasProductos, tiposProductos, csProd, true);
                else
                    productosEncontrados = negProd.BuscarProducto(nombreProducto, familiasProductos, tiposProductos, csProd);
                AgregarProductosAGrid(productosEncontrados);
            }
            catch (Exception ex)
            {
                Utilidades.ReportarError(this, Request.QueryString[Utilidades.NombresParametros.Usuario], "No se pudieron cargar los productos.", "ErrorBusquedaProducto", ex);
            }
        }


        private void SeleccionarProducto()
        {
            DataTable dtNombProds = (DataTable)Session[SESSION_NOMBRES_PRODUCTOS];
            string nombSelProd = dtNombProds.Rows[gvProdServSim.SelectedIndex][0].ToString();
            string idSelProd = gvProdServSim.SelectedRow.Cells[4].Text;
            string idSelProdTipo = gvProdServSim.SelectedRow.Cells[5].Text;
            string idSelProdFamilia = gvProdServSim.SelectedRow.Cells[6].Text;
            RetornarValores(nombSelProd, idSelProd, idSelProdTipo, idSelProdFamilia);
        }

        //Asigna los valores del producto seleccionado al los campos de la ventana padre, retorna S para indicar éxito y cierra la ventana
        private void RetornarValores(string nombreProd, string idProd, string idTipoProd, string idFamProd)
        {
            string nombreProducto = Utilidades.ReemplazarCaracteresEspeciales(nombreProd);

            string scriptProdId = "window.opener.document.getElementById(\"" + hdnCampoProdId_parent.Value + "\").value = \"" + idProd + "\"; ";
            string scriptProdTipoId = "window.opener.document.getElementById(\"" + hdnCampoProdTipoId_parent.Value + "\").value = \"" + idTipoProd + "\"; ";
            string scriptProdFamId = "window.opener.document.getElementById(\"" + hdnCampoFamTipoId_parent.Value + "\").value = \"" + idFamProd + "\"; ";
            string scriptProdFamNombHdn = "window.opener.document.getElementById(\"" + hdnFieldProdNombHdn.Value + "\").value = \"" +
                nombreProducto + "\"; ";
            string scriptProdNombreHTML = "window.opener.document.getElementById(\"" + hdnCampoProdNombre_parent.Value + "\").innerHTML = \"" +
                nombreProducto + "\"; ";
            string scriptProdNombre = "window.opener.document.getElementById(\"" + hdnCampoProdNombre_parent.Value + "\").innerText = \"" +
                nombreProducto + "\"; ";

            Response.Write("<script type=\"text/javascript\"> " + scriptProdNombre + scriptProdId + scriptProdTipoId +
                scriptProdFamId + scriptProdFamNombHdn + scriptProdNombreHTML + " window.returnValue='S'; window.close(); </script>");
        }

        private void AgregarProductosAGrid(List<Entidades.Producto> productos)
        {
            DataTable dtProductos = new DataTable("Productos encontrados");
            dtProductos.Columns.Add("Subtipo de producto");
            dtProductos.Columns.Add("Tipo de producto");
            dtProductos.Columns.Add("Familia");
            dtProductos.Columns.Add("Comercializar");
            dtProductos.Columns.Add("id_producto");
            dtProductos.Columns.Add("id_tipo_producto");
            dtProductos.Columns.Add("id_familia_producto");

            DataTable dtNombresProductos = new DataTable();
            dtNombresProductos.Columns.Add("nombre");
            
            foreach (Entidades.Producto producto in productos)
            {
                DataRow row = dtProductos.NewRow();
                row[0] = producto.Nombre;
                row[1] = producto.TipoProductoNombre;
                row[2] = producto.FamiliaProductosNombre;
                if (producto.HabilitadoComercializar)
                    row[3] = "Sí";
                else
                    row[3] = "No";
                row[4] = producto.Id;
                row[5] = producto.TipoProductoId;
                row[6] = producto.FamiliaProductosId;
                dtProductos.Rows.Add(row);
                DataRow rowNombre = dtNombresProductos.NewRow();
                rowNombre[0] = producto.Nombre;
                dtNombresProductos.Rows.Add(rowNombre);
            }
            gvProdServSim.DataSource = dtProductos;
            gvProdServSim.DataBind();
            gvProdServSim.Visible = true;
            Session[SESSION_NOMBRES_PRODUCTOS] = dtNombresProductos;
        }


        private string[] TiposProductos()
        {
            string[] tiposProds = hdnTiposProdIds.Value.Split('/');
            return tiposProds;
        }


        private string[] FamiliasProductos()
        {
            string[] famsProds = hdnFamiliaProdId.Value.Split('/');
            return famsProds;
        }


        private void LeerQueryStringsProducto()
        {
            hdnCampoProdId_parent.Value = Request.QueryString["ctrlProdId"];
            hdnCampoProdNombre_parent.Value = Request.QueryString["ctrlProdNomb"];
            hdnCampoProdTipoId_parent.Value = Request.QueryString["ctrlProdTipoId"];
            hdnCampoFamTipoId_parent.Value = Request.QueryString["ctrlProdFamId"];
            hdnFieldProdNombHdn.Value = Request.QueryString["ctrlProdNombHdn"];
            hdnFamiliaProdId.Value = Request.QueryString["strCodsFamsProd"];
            hdnTiposProdIds.Value = Request.QueryString["strCodsTiposProd"];
            hdnValHomologac.Value = Request.QueryString["valHomologacion"];
        }

        #region Evento Botón de Búsqueda
        protected void btnBuscar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                BuscarProducto(txtProducto.Text, FamiliasProductos(), TiposProductos());
            }
            catch (Exception ex)
            {
                Utilidades.ReportarError(this, Request.QueryString[Utilidades.NombresParametros.Usuario], "Error al buscar producto.", "BUSQUEDA_PRODUCTO_SIMULACION_CREDITO", ex);
            }
        }
        #endregion

        #region Eventos de Botones del Ribbon
        protected void Seleccionar_Click(object sender, EventArgs e)
        {
            if (gvProdServSim.SelectedIndex == -1)
            {
                string mensaje = @"<script type=""text/javascript""> alert(""No ha seleccionado ningún producto."");</script>";
                this.ClientScript.RegisterStartupScript(this.GetType(), "BUSQUEDA", mensaje);
                return;
            }

            SeleccionarProducto();
            Response.Write("<script type=\"text/javascript\"> window.returnValue = 1; </script>");

        }
        protected void btnCerrar_Click(object sender, EventArgs e)
        {
            Response.Write("<script type=\"text/javascript\"> window.close(); </script>");
        }
        #endregion

        #region Eventos del GridView
        protected void gvProdServSim_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }
        
       
        protected void gvProdServSim_RowDataBound(Object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // javascript function to call on row-click event
                e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(gvProdServSim, "Select$" + e.Row.RowIndex.ToString()));
                e.Row.Style.Add(HtmlTextWriterStyle.Cursor, "pointer");
                e.Row.Attributes.Add("title", "Seleccionar");
            }
        }

        protected void gvProdServSim_RowCreated(object sender, GridViewRowEventArgs e)
        {
            //Just changed the index of cells based on your requirements
            foreach (TableRow row in gvProdServSim.Controls[0].Controls)
            {
                row.Cells[4].Visible = false;
                row.Cells[5].Visible = false;
                row.Cells[6].Visible = false;
            }

        }
        #endregion


    }
}