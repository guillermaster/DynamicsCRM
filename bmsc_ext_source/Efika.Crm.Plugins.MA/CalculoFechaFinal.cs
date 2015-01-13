using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using System.ServiceModel;
using System.Collections;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Workflow;

namespace Efika.Crm.Plugins.MA
{
    internal class CalculoFechaFinal
    {
        internal static DateTime CalcularFechaFinal(DateTime fechaInicial, int cantidadDias, bool excluirSabados, bool excluirDomingos,
            bool excluirFeriados, IOrganizationService servicio)
        {
            QueryByAttribute query = new QueryByAttribute();
            query.EntityName = "calendar";
            query.Attributes.Add("name");
            query.Values.Add("Business Closure Calendar");
            query.ColumnSet.AllColumns = true;

            EntityCollection bec = servicio.RetrieveMultiple(query);
            Entity calendario = null;


            if (bec.Entities.Count > 0)
            {
                calendario = (Entity)bec.Entities[0];
            }

            DateTime fechaaux = DateTime.Today;

            // inicio validacion
            bool ResValFec = EsFeriado(fechaInicial, calendario, excluirSabados, excluirDomingos);
            if (ResValFec == true && excluirFeriados)
            {
                while (ResValFec == true)
                {
                    fechaInicial = fechaInicial.AddDays(1);
                    ResValFec = EsFeriado(fechaInicial, calendario, excluirSabados, excluirDomingos);
                }
            }

            // fin validacion
            fechaaux = fechaInicial;
            int i = 0;
            while (i < cantidadDias)
            {
                i++;
                fechaaux = fechaaux.AddDays(1);

                if (excluirFeriados)
                {
                    foreach (Entity c in ((EntityCollection)calendario.Attributes["calendarrules"]).Entities)
                    {

                        if (DateTime.Parse(fechaaux.ToShortDateString()) >= 
                            DateTime.Parse(DateTime.Parse(c.Attributes["effectiveintervalstart"].ToString()).ToUniversalTime().AddSeconds(1).ToShortDateString())
                            && DateTime.Parse(fechaaux.ToShortDateString()) <= 
                            DateTime.Parse(DateTime.Parse(c.Attributes["effectiveintervalend"].ToString()).ToUniversalTime().AddSeconds(-1).ToShortDateString()))
                        {
                            i--;
                        }
                    }
                }

                if (fechaaux.DayOfWeek == DayOfWeek.Saturday && excluirSabados)
                {
                    i--;
                    continue;
                }

                if (fechaaux.DayOfWeek == DayOfWeek.Sunday && excluirDomingos)
                {
                    i--;
                    continue;
                }

            }

            return fechaaux;
        }

        internal static bool EsFeriado(DateTime FechaEvaluada, Entity calendario, bool excsab, bool excdom)
        {

            bool EsFeriado1 = false;
            foreach (Entity c in ((EntityCollection)calendario.Attributes["calendarrules"]).Entities)
            {
                if ((FechaEvaluada.DayOfWeek == DayOfWeek.Saturday && excsab) ||
                        (DateTime.Parse(FechaEvaluada.ToShortDateString()) >= 
                        DateTime.Parse(DateTime.Parse(c.Attributes["effectiveintervalstart"].ToString()).ToUniversalTime().AddSeconds(1).ToShortDateString())
                        && DateTime.Parse(FechaEvaluada.ToShortDateString()) <= 
                        DateTime.Parse(DateTime.Parse(c.Attributes["effectiveintervalend"].ToString()).ToUniversalTime().AddSeconds(-1).ToShortDateString()))
                        || (FechaEvaluada.DayOfWeek == DayOfWeek.Sunday && excdom))
                {
                    EsFeriado1 = true;
                }
            }

            return EsFeriado1;
        }
    }
}
