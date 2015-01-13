using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace Efika.Crm.Entidades.Common
{
    public class MyCertificatePolicy 
    {
        public static bool ValidateCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            // policy code
            bool validationResult = true ;
            return validationResult;
        }
    }
}
