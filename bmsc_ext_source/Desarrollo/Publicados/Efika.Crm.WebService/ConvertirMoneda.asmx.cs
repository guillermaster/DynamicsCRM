using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Configuration;
using System.Web.Services;
using Efika.Crm.Entidades;
using Efika.Crm.Negocio;
using Efika.Crm.AccesoServicios;
using System.Data;
using System.Data.SqlClient;
namespace Efika.Crm.WebService
{
    /// <summary>
    /// Summary description for ConvertirMoneda
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class ConvertirMoneda : System.Web.Services.WebService
    {

        [WebMethod]
        public bool NuevoValor(string cliente)
        {
            SqlCommand sqlCmd = null;
            SqlConnection conn = null;
            bool NuevoValor = false;

            try
            {
                sqlCmd = new SqlCommand("sp_calcula_salario_considerar");
                sqlCmd.CommandType = CommandType.StoredProcedure;

                sqlCmd.Parameters.Add(new SqlParameter("@c_clienteId", SqlDbType.VarChar));
                sqlCmd.Parameters["@c_clienteId"].Value = cliente;
                string connStr = string.Empty;

                conn = new SqlConnection(connStr);

                conn.Open();
                sqlCmd.ExecuteNonQuery();
                conn.Close();

                NuevoValor = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                sqlCmd.Dispose();
                conn.Close();
            }

            return NuevoValor;

        }
    }
}
