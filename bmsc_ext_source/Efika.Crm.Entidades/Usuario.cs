using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Efika.Crm.Entidades
{
    public class Usuario
    {
        private Guid systemuserid;
        private string fullName;
        private string title;
        private string efk_sucursalid;
        private string efk_oficina_id;

        public Guid SystemUserId
        {
            get { return systemuserid; }
            set { systemuserid = value; }
        }
        public string FullName
        {
            get { return fullName; }
            set { fullName = value; }
        }
        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        public string SucursalID
        {
            get { return efk_sucursalid; }
            set { efk_sucursalid = value; }
        }
        public string OficinaId
        {
            get { return efk_oficina_id; }
            set { efk_oficina_id = value; }
        }



    }
}
