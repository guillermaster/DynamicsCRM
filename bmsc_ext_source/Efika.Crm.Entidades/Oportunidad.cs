using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Efika.Crm.Entidades
{
    public class Oportunidad
    {
        public Guid Id { get; set; }
        public Guid ClienteId { get; set; }
        public Guid ProductoSimuladoId { get; set; }
        public int NumCuotas { get; set; }
        public int InicioTasaVariable { get; set; }
        public decimal MontoMaximo { get; set; }
        public decimal MontoSolicitado { get; set; }
        public decimal Spread { get; set; }
        public decimal TasaFija { get; set; }
        public decimal TRE { get; set; }
        public int NumeroSolicitud { get; set; }
        public int Orden { get; set; }
        public string Nombre { get; set; }
        public int NumeroOferta { get; set; }
        //Se agrego Moneda para enviar al sp y calcular monto
        public Guid transactioncurrencyid { get; set; }
        public string MonedaCodISO { get; set; }

        public Oportunidad()
        {
        }

        public Oportunidad(Guid id, Guid clienteId, Guid prodSimId, string nombre, int numCuotas, int inicTasaVar,
            decimal montoMax, decimal montoSol, decimal spread, decimal tasaFija, decimal tre, int orden, Guid idMoneda, string codIsoMoneda)
        {
            Id = id;
            NumCuotas = numCuotas;
            InicioTasaVariable = inicTasaVar;
            Nombre = nombre;
            MontoMaximo = montoMax;
            MontoSolicitado = montoSol;
            Spread = spread;
            TasaFija = tasaFija;
            TRE = tre;
            ClienteId = clienteId;
            ProductoSimuladoId = prodSimId;
            Orden = orden;
            transactioncurrencyid = idMoneda;
            MonedaCodISO = codIsoMoneda;
        }
    }
}
