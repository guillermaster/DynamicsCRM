using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Description;
using Testing.CRMSDK;

namespace Testing
{
    class Program
    {
        static void Main(string[] args)
        {
            System.ServiceModel.Description.ClientCredentials credenciales = new ClientCredentials();
            credenciales.Windows.ClientCredential = (System.Net.NetworkCredential)System.Net.CredentialCache.DefaultCredentials;

            OrganizationServiceClient servicio = new OrganizationServiceClient("CustomBinding_IOrganizationService", "http://crmserver:5555/BMSC/XRMServices/2011/Organization.svc");
            servicio.ClientCredentials.Windows.ClientCredential = credenciales.Windows.ClientCredential;

            OrganizationRequest req = new OrganizationRequest();
            req.RequestName = "RetrieveOptionSet";
            req.Parameters = new ParameterCollection();
            req.Parameters.Add(new KeyValuePair<string, object>("Name", ""));
            req.Parameters.Add(new KeyValuePair<string, object>("RetrieveAsIfPublished", true));

            OrganizationResponse res= servicio.Execute(req);

            string s = "";
        }
    }
}
