using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PruebasCRM.CrmSDK;

namespace PruebasCRM
{
    class Program
    {
        static void Main(string[] args)
        {
            int cantidadDias = 5;
            DateTime fechaInicial = new DateTime(2012, 10, 3);
            bool considerarDomingos = false;
            bool considerarSabados = false;

            CrmAuthenticationToken token = new CrmAuthenticationToken();
            token.OrganizationName = "BMSC";

            CrmService service = new CrmService();
            service.Credentials = System.Net.CredentialCache.DefaultCredentials;
            service.CrmAuthenticationTokenValue = token;

            RetrieveMultipleRequest _request = new RetrieveMultipleRequest();
            QueryByAttribute query = new QueryByAttribute();
            query.EntityName = "calendar";
            query.Attributes = new string[] { "name" };
            query.Values = new string[] { "Business Closure Calendar" };
            query.ColumnSet = new AllColumns(); ;

            BusinessEntityCollection bec = service.RetrieveMultiple(query);

            if (bec.BusinessEntities.Length > 0)
            {
                calendar calendario = (calendar)bec.BusinessEntities[0];

                Console.WriteLine(DateTime.Parse(calendario.calendarrules[0].effectiveintervalstart.Value).ToUniversalTime().ToString("dd//MM/yyyy"));
                Console.WriteLine(DateTime.Parse(calendario.calendarrules[1].effectiveintervalstart.Value).ToUniversalTime().ToString("dd//MM/yyyy"));
                Console.WriteLine(DateTime.Parse(calendario.calendarrules[2].effectiveintervalstart.Value).ToUniversalTime().ToString("dd//MM/yyyy"));

                Console.ReadLine();
            }
            
        }
    }
}
