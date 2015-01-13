using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace Efika.Crm.Negocio
{
    public class Utilidades
    {
        public string GetCodIsoMoneda(string codMonedaBMSC)
        {
            string codISO = string.Empty;

            try
            {
                XmlTextReader xmlReader = new XmlTextReader(HttpContext.Current.Server.MapPath("Divisas.xml"));
                bool found = false;
                

                while(!found && xmlReader.Read())
                {
                    if(xmlReader.NodeType == XmlNodeType.Element && xmlReader.GetAttribute("CodBMSC") == codMonedaBMSC)
                    {
                        codISO = xmlReader.GetAttribute("CodISO");
                        found = true;
                    }
                }
            }
            catch(Exception ex)
            {
                throw new Exception("Error al obtener código ISO de moneda.", ex);
            }

            return codISO;
        }

        public static string GetCodBmscMoneda(string codMonedaISO)
        {
            string codBMSC = string.Empty;

            try
            {
                XmlTextReader xmlReader = new XmlTextReader(HttpContext.Current.Server.MapPath("Divisas.xml"));
                bool found = false;


                while (!found && xmlReader.Read())
                {
                    if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.GetAttribute("CodISO") == codMonedaISO)
                    {
                        codBMSC = xmlReader.GetAttribute("CodBMSC");
                        found = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener código ISO de moneda.", ex);
            }

            return codBMSC;
        }
    }
}