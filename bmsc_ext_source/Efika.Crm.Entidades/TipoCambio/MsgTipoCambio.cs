using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Efika.Crm.Entidades.TipoCambio
{
    public class MsgTipoCambio
    {
        public MsgTipoCambio()
        { }

        public MsgTipoCambio(string user,string pwd,string dominio)
        {
            this.Usuario = user;
            this.Contrasena = pwd;
            this.Dominio = dominio;
        }


        public string Usuario { get; set; }
        public string Contrasena { get; set; }
        public string Dominio { get; set; }


    }



}
