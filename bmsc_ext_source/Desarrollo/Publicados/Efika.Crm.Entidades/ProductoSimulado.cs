using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Efika.Crm.Entidades
{
    public class ProductoSimulado
    {
        public string Nombre { get; set; }
        public Guid Id { get; set; }
        public Guid ClienteId { get; set; }
        public Guid ProductoId { get; set; }
        public string ProductoNombre { get; set; }
        public Guid ProductoTipoId { get; set; }
        public Guid ProductoFamiliaId { get; set; }
        public int ProductoFamiliaCod { get; set; }
        public Guid SimulacionCreditoId { get; set; }
        public int NumeroCuotas { get; set; }
        public int MonedaOperacionCod { get; set; }
        public decimal TasaFija { get; set; }
        public decimal TreSemana { get; set; }
        public decimal SpreadFijo { get; set; }
        public int FrecuenciaAmortizacion { get; set; }
        public int FrecuenciaAmortizacionCod { get; set; }
        public int TasaVariableDesde { get; set; }
        public bool SeguroDesgravamen { get; set; }
        public bool SeguroCesantia { get; set; }
        public int TipoPolizaCod { get; set; }
        public decimal MontoMaximo { get; set; }
        public decimal MontoSolicitado { get; set; }
        public decimal CuotaPactada { get; set; }
        public decimal CuotaMaxima { get; set; }
        public int Orden {get; set; }
        public decimal CuotaSobrante { get; set; }
        public int NumeroOferta { get; set; }
        public Guid CampaniaId { get; set; }
        public Guid OwnerId { get; set; }
        public decimal NuevoCemAjustadoVivienda { get; set; }//cem ajustado de vivienda para el producto actual
        public string MonedaCodISO { get; set; }
        public bool GeneradaSimulacion { get; set; }

        public ProductoSimulado()
        {
        }

        public ProductoSimulado(Guid id, string nombre, int numCuotas, string codIsoMoneda, decimal tasaFija, decimal treSemana, decimal spreadFijo,
            int frecAmort, int frecAmortCod, int tasaVarDesde, bool segDesgr, bool segCes, int tipoPoliza, decimal montoMax, decimal montoSol,
            Guid prodId, Guid prodTipoId, Guid prodFamId, Guid clienteId, Guid simulacCredId, string prodNombre, int numOferta, int orden)
        {
            Id = id;
            Nombre = nombre;
            NumeroCuotas = numCuotas;
            MonedaCodISO = codIsoMoneda;
            TasaFija = tasaFija;
            TreSemana = treSemana;
            SpreadFijo = spreadFijo;
            FrecuenciaAmortizacion = frecAmort;
            FrecuenciaAmortizacionCod = frecAmortCod;
            TasaVariableDesde = tasaVarDesde;
            SeguroDesgravamen = segDesgr;
            SeguroCesantia = segCes;
            TipoPolizaCod = tipoPoliza;
            MontoMaximo = montoMax;
            MontoSolicitado = montoSol;
            ProductoId = prodId;
            ProductoTipoId = prodTipoId;
            ProductoFamiliaId = prodFamId;
            ClienteId = clienteId;
            SimulacionCreditoId = simulacCredId;
            ProductoNombre = prodNombre;
            NumeroOferta = numOferta;
            Orden = orden;
        }

        public ProductoSimulado(Guid id, string nombre, int numCuotas, string codIsoMoneda, decimal tasaFija, decimal treSemana, decimal spreadFijo,
            int frecAmort, int frecAmortCod, int tasaVarDesde, bool segDesgr, bool segCes, int tipoPoliza,
            Guid prodId, Guid prodTipoId, Guid prodFamId, Guid clienteId, Guid simulacCredId, string prodNombre, int numOferta, int orden)
        {
            Id = id;
            Nombre = nombre;
            NumeroCuotas = numCuotas;
            MonedaCodISO = codIsoMoneda;
            TasaFija = tasaFija;
            TreSemana = treSemana;
            SpreadFijo = spreadFijo;
            FrecuenciaAmortizacion = frecAmort;
            FrecuenciaAmortizacionCod = frecAmortCod;
            TasaVariableDesde = tasaVarDesde;
            SeguroDesgravamen = segDesgr;
            SeguroCesantia = segCes;
            TipoPolizaCod = tipoPoliza;
            ProductoId = prodId;
            ProductoTipoId = prodTipoId;
            ProductoFamiliaId = prodFamId;
            ClienteId = clienteId;
            SimulacionCreditoId = simulacCredId;
            ProductoNombre = prodNombre;
            NumeroOferta = numOferta;
            Orden = orden;
        }

        public static string FrecAmortizacionText()
        {
            return "30";
        }

        public static string FrecAmortizacionValue()
        {
            return "221220000";
        }
    }
}
