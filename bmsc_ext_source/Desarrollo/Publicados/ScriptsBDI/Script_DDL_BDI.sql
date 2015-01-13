USE [BDI_CRM]
GO
/****** Object:  User [BERCANT\Usr_CRM_Carga]    Script Date: 08/27/2013 11:36:01 ******/
CREATE USER [BERCANT\Usr_CRM_Carga] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  Table [dbo].[BDI_CRM_LogError]    Script Date: 08/27/2013 11:35:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BDI_CRM_LogError](
	[dFecha] [datetime] NULL,
	[sProcPrincipal] [varchar](50) NULL,
	[sProcCarga] [varchar](2000) NULL,
	[sUsuario] [varchar](50) NULL,
	[sTipoEntrada] [char](1) NULL,
	[sMensaje] [varchar](5000) NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BDI_CRM_InfoAdicionalClientes_DIF]    Script Date: 08/27/2013 11:35:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BDI_CRM_InfoAdicionalClientes_DIF](
	[iCodigoClienteBanco] [int] NULL,
	[sIndiceRelacionActual] [varchar](10) NULL,
	[iCantidadProductosDelActivo] [int] NULL,
	[iCantidadProductosDelPasivo] [int] NULL,
	[iCantidadOperacionesDelActivoCanceladas] [int] NULL,
	[bRetencionesJudiciales] [bit] NULL,
	[bCuentasCorrientesClausuradas] [bit] NULL,
	[cTotalCreditoVivienda] [money] NULL,
	[cTotalCreditoVehicular] [money] NULL,
	[cTotalCreditoConsumo] [money] NULL,
	[cTotalCreditoComercial] [money] NULL,
	[cTotalProductosDelActivo] [money] NULL,
	[cMontoMaximoDeuda24mesesBco] [money] NULL,
	[cMontoMaximoDeuda24mesesSF] [money] NULL,
	[cVolumenTotalNegocio] [money] NULL,
	[cVolumenPromedioNegocio24meses] [money] NULL,
	[cVolumenMaximoNegocio24meses] [money] NULL,
	[cTotalGarantias] [money] NULL,
	[cTotalTarjetaCredito] [money] NULL,
	[cTotalCuentaCorriente] [money] NULL,
	[cTotalCuentaAhorro] [money] NULL,
	[cTotalDPF] [money] NULL,
	[cTotalProductosPasivo] [money] NULL,
	[cTotalContingentes] [money] NULL,
	[cTotalCentralRiesgoUltimoMes] [money] NULL,
	[sTipoReg] [char](1) NULL,
	[dFechaUltimoError] [datetime] NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BDI_CRM_InfoAdicionalClientes_ACT]    Script Date: 08/27/2013 11:35:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BDI_CRM_InfoAdicionalClientes_ACT](
	[iCodigoClienteBanco] [int] NULL,
	[sIndiceRelacionActual] [varchar](10) NULL,
	[iCantidadProductosDelActivo] [int] NULL,
	[iCantidadProductosDelPasivo] [int] NULL,
	[iCantidadOperacionesDelActivoCanceladas] [int] NULL,
	[bRetencionesJudiciales] [bit] NULL,
	[bCuentasCorrientesClausuradas] [bit] NULL,
	[cTotalCreditoVivienda] [money] NULL,
	[cTotalCreditoVehicular] [money] NULL,
	[cTotalCreditoConsumo] [money] NULL,
	[cTotalCreditoComercial] [money] NULL,
	[cTotalProductosDelActivo] [money] NULL,
	[cMontoMaximoDeuda24mesesBco] [money] NULL,
	[cMontoMaximoDeuda24mesesSF] [money] NULL,
	[cVolumenTotalNegocio] [money] NULL,
	[cVolumenPromedioNegocio24meses] [money] NULL,
	[cVolumenMaximoNegocio24meses] [money] NULL,
	[cTotalGarantias] [money] NULL,
	[cTotalTarjetaCredito] [money] NULL,
	[cTotalCuentaCorriente] [money] NULL,
	[cTotalCuentaAhorro] [money] NULL,
	[cTotalDPF] [money] NULL,
	[cTotalProductosPasivo] [money] NULL,
	[cTotalContingentes] [money] NULL,
	[cTotalCentralRiesgoUltimoMes] [money] NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BDI_CRM_Direcciones_ERROR]    Script Date: 08/27/2013 11:35:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BDI_CRM_Direcciones_ERROR](
	[iCodigoClienteBanco] [int] NULL,
	[iCodigoDireccion] [numeric](10, 0) NULL,
	[sCodigoCiudad] [nvarchar](4) NULL,
	[sCiudad] [nvarchar](100) NULL,
	[sCodigoBarrio] [nvarchar](4) NULL,
	[sBarrio] [nvarchar](100) NULL,
	[sCodigoZona] [nvarchar](4) NULL,
	[sZona] [nvarchar](100) NULL,
	[sCalle] [nvarchar](400) NULL,
	[sTipoDireccion] [nvarchar](100) NULL,
	[sCodigoTipoDireccion] [nvarchar](4) NULL,
	[sTipoReg] [char](1) NULL,
	[dFechaUltimoError] [datetime] NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BDI_CRM_Direcciones_DIF]    Script Date: 08/27/2013 11:35:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BDI_CRM_Direcciones_DIF](
	[iCodigoClienteBanco] [int] NULL,
	[iCodigoDireccion] [numeric](10, 0) NULL,
	[sCodigoCiudad] [nvarchar](4) NULL,
	[sCiudad] [nvarchar](100) NULL,
	[sCodigoBarrio] [nvarchar](4) NULL,
	[sBarrio] [nvarchar](100) NULL,
	[sCodigoZona] [nvarchar](4) NULL,
	[sZona] [nvarchar](100) NULL,
	[sCalle] [nvarchar](400) NULL,
	[sTipoDireccion] [nvarchar](100) NULL,
	[sCodigoTipoDireccion] [nvarchar](4) NULL,
	[sTipoReg] [char](1) NULL,
	[bError] [bit] NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BDI_CRM_Direcciones_ACT]    Script Date: 08/27/2013 11:35:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BDI_CRM_Direcciones_ACT](
	[iCodigoClienteBanco] [int] NULL,
	[iCodigoDireccion] [numeric](10, 0) NULL,
	[sCodigoCiudad] [nvarchar](4) NULL,
	[sCiudad] [nvarchar](100) NULL,
	[sCodigoBarrio] [nvarchar](4) NULL,
	[sBarrio] [nvarchar](100) NULL,
	[sCodigoZona] [nvarchar](4) NULL,
	[sZona] [nvarchar](100) NULL,
	[sCalle] [nvarchar](400) NULL,
	[sTipoDireccion] [nvarchar](100) NULL,
	[sCodigoTipoDireccion] [nvarchar](4) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BDI_CRM_DatosConciliacion]    Script Date: 08/27/2013 11:35:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BDI_CRM_DatosConciliacion](
	[uOpportunityId] [uniqueidentifier] NOT NULL,
	[uOpportunityProductId] [uniqueidentifier] NOT NULL,
	[iOpportunityStateCode] [int] NULL,
	[iOpportunityStatusCode] [int] NULL,
	[dFechaMaximaConciliacionProducto] [datetime] NULL,
	[dFechaMaximaConciliacionOportunidad] [datetime] NULL,
	[dFechaMinimaConciliacionOportunidad] [datetime] NULL,
	[iEstadoConciliacionOportunidad] [int] NULL,
	[iRazonEstadoConciliacionOportunidad] [int] NULL,
	[iEstadoConciliacionProducto] [int] NULL,
	[uOpportunityCloseId] [uniqueidentifier] NULL,
	[iDatoConciliacion] [int] IDENTITY(1,1) NOT NULL,
	[sOperacionProducto] [varchar](50) NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BDI_CRM_Clientes_ERROR]    Script Date: 08/27/2013 11:35:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BDI_CRM_Clientes_ERROR](
	[sTipoCliente] [nvarchar](15) NULL,
	[sCodigoTipoCliente] [nvarchar](4) NULL,
	[sFuenteDeIngresos] [nvarchar](15) NULL,
	[sCodigoFuenteIngresos] [nvarchar](4) NULL,
	[sBancaCliente] [nvarchar](100) NULL,
	[sCodigoBancaCliente] [nvarchar](4) NULL,
	[sCodigoTipoIdentificacion] [nvarchar](1) NULL,
	[sTipoIdentificacion] [nvarchar](100) NULL,
	[sNumeroIdentificacion] [nvarchar](19) NULL,
	[sNumeroNit] [nvarchar](19) NULL,
	[iCodigoClienteBanco] [int] NULL,
	[sNombreCompleto] [nvarchar](80) NULL,
	[sNombre] [nvarchar](30) NULL,
	[sPrimerApellido] [nvarchar](20) NULL,
	[sSegundoApellido] [nvarchar](20) NULL,
	[cPatromonio] [money] NULL,
	[sTipoCompania] [nvarchar](100) NULL,
	[sCodigoTipoCompania] [nvarchar](8) NULL,
	[sTamanioActividad] [nvarchar](200) NULL,
	[sCodigoTamanioActividad] [nvarchar](40) NULL,
	[iNumeroEmpleados] [nvarchar](100) NULL,
	[sAgenciaManejo] [nvarchar](25) NULL,
	[sCodigoAgenciaManejo] [nvarchar](5) NULL,
	[dFechaCreacionCliente] [datetime] NULL,
	[dFechaConstitucionEmpresa] [datetime] NULL,
	[sCalificacionClienteBco] [nvarchar](50) NULL,
	[sNombreEjecutivo] [nvarchar](70) NULL,
	[sLoginEjecutivo] [nvarchar](10) NULL,
	[sCodigoEjecutivo] [nvarchar](4) NULL,
	[sActividadEconomica] [nvarchar](80) NULL,
	[sCodigoActividadEconomica] [nvarchar](4) NULL,
	[sDetalleActividadEconomica] [nvarchar](200) NULL,
	[sCodigoDetalleActividadEconomica] [nvarchar](8) NULL,
	[sAgenciaCreacion] [nvarchar](25) NULL,
	[sCodigoAgenciaCreacion] [nvarchar](25) NULL,
	[sCodigoSegmento] [nvarchar](4) NULL,
	[bAtencionPreferente] [bit] NULL,
	[sDescripcionAtencionPreferente] [nvarchar](100) NULL,
	[sTelefono] [nvarchar](40) NULL,
	[sCorreoElectronico] [nvarchar](40) NULL,
	[sTelefonoMovil] [nvarchar](15) NULL,
	[cIngresosAnuales] [money] NULL,
	[sCiudad] [nvarchar](100) NULL,
	[sBarrio] [nvarchar](100) NULL,
	[sZona] [nvarchar](100) NULL,
	[iAniosExperienciaRubro] [smallint] NULL,
	[sSexo] [nvarchar](12) NULL,
	[sCodigoSexo] [nvarchar](4) NULL,
	[sEstadoCivil] [nvarchar](100) NULL,
	[sCodigoEstadoCivil] [nvarchar](4) NULL,
	[sPaisOrigen] [nvarchar](100) NULL,
	[dFechaNacimiento] [datetime] NULL,
	[iNumeroHijos] [int] NULL,
	[sNivelDeEstudios] [nvarchar](100) NULL,
	[sCodigoNivelEstudios] [nvarchar](4) NULL,
	[sProfesion] [nvarchar](100) NULL,
	[sEstadoCliente] [nvarchar](100) NULL,
	[sCodigoEstadoCliente] [nvarchar](2) NULL,
	[sCargoActual] [nvarchar](30) NULL,
	[cIngresosMensuales] [money] NULL,
	[sTipoReg] [char](1) NULL,
	[dFechaUltimoError] [datetime] NULL,
	[sEmpresa] [nvarchar](60) NULL,
	[dFechaIngresoEmpresa] [datetime] NULL,
	[sCodigoSucursal] [nvarchar](3) NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BDI_CRM_Clientes_DIF]    Script Date: 08/27/2013 11:35:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BDI_CRM_Clientes_DIF](
	[sTipoCliente] [nvarchar](15) NULL,
	[sCodigoTipoCliente] [nvarchar](4) NULL,
	[sFuenteDeIngresos] [nvarchar](15) NULL,
	[sCodigoFuenteIngresos] [nvarchar](4) NULL,
	[sBancaCliente] [nvarchar](100) NULL,
	[sCodigoBancaCliente] [nvarchar](4) NULL,
	[sCodigoTipoIdentificacion] [nvarchar](1) NULL,
	[sTipoIdentificacion] [nvarchar](100) NULL,
	[sNumeroIdentificacion] [nvarchar](19) NULL,
	[sNumeroNit] [nvarchar](19) NULL,
	[iCodigoClienteBanco] [int] NULL,
	[sNombreCompleto] [nvarchar](80) NULL,
	[sNombre] [nvarchar](30) NULL,
	[sPrimerApellido] [nvarchar](20) NULL,
	[sSegundoApellido] [nvarchar](20) NULL,
	[cPatromonio] [money] NULL,
	[sTipoCompania] [nvarchar](100) NULL,
	[sCodigoTipoCompania] [nvarchar](8) NULL,
	[sTamanioActividad] [nvarchar](200) NULL,
	[sCodigoTamanioActividad] [nvarchar](40) NULL,
	[iNumeroEmpleados] [int] NULL,
	[sAgenciaManejo] [nvarchar](25) NULL,
	[sCodigoAgenciaManejo] [nvarchar](5) NULL,
	[dFechaCreacionCliente] [datetime] NULL,
	[dFechaConstitucionEmpresa] [datetime] NULL,
	[sCalificacionClienteBco] [nvarchar](50) NULL,
	[sNombreEjecutivo] [nvarchar](70) NULL,
	[sLoginEjecutivo] [nvarchar](10) NULL,
	[sCodigoEjecutivo] [nvarchar](4) NULL,
	[sActividadEconomica] [nvarchar](80) NULL,
	[sCodigoActividadEconomica] [nvarchar](4) NULL,
	[sDetalleActividadEconomica] [nvarchar](200) NULL,
	[sCodigoDetalleActividadEconomica] [nvarchar](8) NULL,
	[sAgenciaCreacion] [nvarchar](25) NULL,
	[sCodigoAgenciaCreacion] [nvarchar](25) NULL,
	[sCodigoSegmento] [nvarchar](4) NULL,
	[bAtencionPreferente] [bit] NULL,
	[sDescripcionAtencionPreferente] [nvarchar](100) NULL,
	[sTelefono] [nvarchar](40) NULL,
	[sCorreoElectronico] [nvarchar](40) NULL,
	[sTelefonoMovil] [nvarchar](15) NULL,
	[cIngresosAnuales] [money] NULL,
	[sCiudad] [nvarchar](100) NULL,
	[sBarrio] [nvarchar](100) NULL,
	[sZona] [nvarchar](100) NULL,
	[iAniosExperienciaRubro] [smallint] NULL,
	[sSexo] [nvarchar](12) NULL,
	[sCodigoSexo] [nvarchar](4) NULL,
	[sEstadoCivil] [nvarchar](100) NULL,
	[sCodigoEstadoCivil] [nvarchar](4) NULL,
	[sPaisOrigen] [nvarchar](100) NULL,
	[dFechaNacimiento] [datetime] NULL,
	[iNumeroHijos] [int] NULL,
	[sNivelDeEstudios] [nvarchar](100) NULL,
	[sCodigoNivelEstudios] [nvarchar](4) NULL,
	[sProfesion] [nvarchar](100) NULL,
	[sEstadoCliente] [nvarchar](100) NULL,
	[sCodigoEstadoCliente] [nvarchar](2) NULL,
	[sCargoActual] [nvarchar](30) NULL,
	[cIngresosMensuales] [money] NULL,
	[sTipoReg] [char](1) NULL,
	[bError] [bit] NULL,
	[sEmpresa] [nvarchar](60) NULL,
	[dFechaIngresoEmpresa] [datetime] NULL,
	[sCodigoSucursal] [nvarchar](3) NULL,
	[sSiglaDiminutivo] [nvarchar](40) NULL,
	[sSujetoImponible] [nvarchar](100) NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BDI_CRM_Clientes_ACT]    Script Date: 08/27/2013 11:35:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BDI_CRM_Clientes_ACT](
	[sTipoCliente] [nvarchar](15) NULL,
	[sCodigoTipoCliente] [nvarchar](4) NULL,
	[sFuenteDeIngresos] [nvarchar](15) NULL,
	[sCodigoFuenteIngresos] [nvarchar](4) NULL,
	[sBancaCliente] [nvarchar](100) NULL,
	[sCodigoBancaCliente] [nvarchar](4) NULL,
	[sCodigoTipoIdentificacion] [nvarchar](1) NULL,
	[sTipoIdentificacion] [nvarchar](100) NULL,
	[sNumeroIdentificacion] [nvarchar](19) NULL,
	[sNumeroNit] [nvarchar](19) NULL,
	[iCodigoClienteBanco] [int] NULL,
	[sNombreCompleto] [nvarchar](80) NULL,
	[sNombre] [nvarchar](30) NULL,
	[sPrimerApellido] [nvarchar](20) NULL,
	[sSegundoApellido] [nvarchar](20) NULL,
	[cPatromonio] [money] NULL,
	[sTipoCompania] [nvarchar](100) NULL,
	[sCodigoTipoCompania] [nvarchar](8) NULL,
	[sTamanioActividad] [nvarchar](200) NULL,
	[sCodigoTamanioActividad] [nvarchar](40) NULL,
	[iNumeroEmpleados] [int] NULL,
	[sAgenciaManejo] [nvarchar](25) NULL,
	[sCodigoAgenciaManejo] [nvarchar](5) NULL,
	[dFechaCreacionCliente] [datetime] NULL,
	[dFechaConstitucionEmpresa] [datetime] NULL,
	[sCalificacionClienteBco] [nvarchar](50) NULL,
	[sNombreEjecutivo] [nvarchar](70) NULL,
	[sLoginEjecutivo] [nvarchar](10) NULL,
	[sCodigoEjecutivo] [nvarchar](4) NULL,
	[sActividadEconomica] [nvarchar](80) NULL,
	[sCodigoActividadEconomica] [nvarchar](4) NULL,
	[sDetalleActividadEconomica] [nvarchar](200) NULL,
	[sCodigoDetalleActividadEconomica] [nvarchar](8) NULL,
	[sAgenciaCreacion] [nvarchar](25) NULL,
	[sCodigoAgenciaCreacion] [nvarchar](25) NULL,
	[sCodigoSegmento] [nvarchar](4) NULL,
	[bAtencionPreferente] [bit] NULL,
	[sDescripcionAtencionPreferente] [nvarchar](100) NULL,
	[sTelefono] [nvarchar](40) NULL,
	[sCorreoElectronico] [nvarchar](40) NULL,
	[sTelefonoMovil] [nvarchar](15) NULL,
	[cIngresosAnuales] [money] NULL,
	[sCiudad] [nvarchar](100) NULL,
	[sBarrio] [nvarchar](100) NULL,
	[sZona] [nvarchar](100) NULL,
	[iAniosExperienciaRubro] [smallint] NULL,
	[sSexo] [nvarchar](12) NULL,
	[sCodigoSexo] [nvarchar](4) NULL,
	[sEstadoCivil] [nvarchar](100) NULL,
	[sCodigoEstadoCivil] [nvarchar](4) NULL,
	[sPaisOrigen] [nvarchar](100) NULL,
	[dFechaNacimiento] [datetime] NULL,
	[iNumeroHijos] [int] NULL,
	[sNivelDeEstudios] [nvarchar](100) NULL,
	[sCodigoNivelEstudios] [nvarchar](4) NULL,
	[sProfesion] [nvarchar](100) NULL,
	[sEstadoCliente] [nvarchar](100) NULL,
	[sCodigoEstadoCliente] [nvarchar](2) NULL,
	[sCargoActual] [nvarchar](30) NULL,
	[cIngresosMensuales] [money] NULL,
	[sEmpresa] [nvarchar](60) NULL,
	[dFechaIngresoEmpresa] [datetime] NULL,
	[sCodigoSucursal] [nvarchar](3) NULL,
	[sSiglaDiminutivo] [nvarchar](40) NULL,
	[sSujetoImponible] [nvarchar](100) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BDI_CRM_CentralRiesgo_DIF]    Script Date: 08/27/2013 11:35:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BDI_CRM_CentralRiesgo_DIF](
	[iCodigoClienteBanco] [int] NULL,
	[sInstitucionFinanciera] [nvarchar](80) NULL,
	[sCalificacion] [nvarchar](50) NULL,
	[cMontoRiesgo] [money] NULL,
	[sTipoReg] [char](1) NULL,
	[bError] [bit] NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BDI_CRM_CentralRiesgo_ACT]    Script Date: 08/27/2013 11:35:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BDI_CRM_CentralRiesgo_ACT](
	[iCodigoClienteBanco] [int] NULL,
	[sInstitucionFinanciera] [nvarchar](80) NULL,
	[sCalificacion] [nvarchar](50) NULL,
	[cMontoRiesgo] [money] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BDI_CRM_Canales_DIF]    Script Date: 08/27/2013 11:35:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BDI_CRM_Canales_DIF](
	[iCodigoClienteBanco] [int] NULL,
	[sCodigoUnicoProducto] [nvarchar](13) NULL,
	[fCantPromTrimestralTransUltMeses] [float] NULL,
	[fPorcPromUsoCanalUltMeses] [float] NULL,
	[fFrecuenciaUsoDiarioUltMeses] [float] NULL,
	[dFechaUltimoUso] [datetime] NULL,
	[cMontoPromTrimestralTransUltMeses] [money] NULL,
	[dFechaAperturaEmision] [datetime] NULL,
	[sCodigoEjecutivoCreacionEmision] [nvarchar](5) NULL,
	[sCodigoAgenciaCreacionEmision] [nvarchar](5) NULL,
	[sTipoReg] [char](1) NULL,
	[bError] [bit] NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BDI_CRM_Canales_ACT]    Script Date: 08/27/2013 11:35:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BDI_CRM_Canales_ACT](
	[iCodigoClienteBanco] [int] NULL,
	[sCodigoUnicoProducto] [nvarchar](13) NULL,
	[fCantPromTrimestralTransUltMeses] [float] NULL,
	[fPorcPromUsoCanalUltMeses] [float] NULL,
	[fFrecuenciaUsoDiarioUltMeses] [float] NULL,
	[dFechaUltimoUso] [datetime] NULL,
	[cMontoPromTrimestralTransUltMeses] [money] NULL,
	[dFechaAperturaEmision] [datetime] NULL,
	[sCodigoEjecutivoCreacionEmision] [nvarchar](5) NULL,
	[sCodigoAgenciaCreacionEmision] [nvarchar](5) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BDI_CodigosTipo_VehiculosVivienda]    Script Date: 08/27/2013 11:35:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BDI_CodigosTipo_VehiculosVivienda](
	[iCodigoTipoProducto] [int] NULL,
	[iCodigoValorCrm] [nvarchar](20) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BDI_ClientesServiciosCanales]    Script Date: 08/27/2013 11:35:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BDI_ClientesServiciosCanales](
	[iCodigoClienteBanco] [int] NULL,
	[sCodigoUnicoProducto] [nvarchar](13) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BDI_Clientes]    Script Date: 08/27/2013 11:35:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BDI_Clientes](
	[sTipoCliente] [nvarchar](15) NULL,
	[sCodigoTipoCliente] [nvarchar](4) NULL,
	[sFuenteDeIngresos] [nvarchar](15) NULL,
	[sCodigoFuenteIngresos] [nvarchar](4) NULL,
	[sBancaCliente] [nvarchar](100) NULL,
	[sCodigoBancaCliente] [nvarchar](4) NULL,
	[sCodigoTipoIdentificacion] [nvarchar](1) NULL,
	[sTipoIdentificacion] [nvarchar](100) NULL,
	[sNumeroIdentificacion] [nvarchar](19) NULL,
	[sNumeroNit] [nvarchar](19) NULL,
	[iCodigoClienteBanco] [int] NULL,
	[sNombreCompleto] [nvarchar](80) NULL,
	[sNombre] [nvarchar](30) NULL,
	[sPrimerApellido] [nvarchar](20) NULL,
	[sSegundoApellido] [nvarchar](20) NULL,
	[cPatromonio] [money] NULL,
	[sTipoCompania] [nvarchar](100) NULL,
	[sCodigoTipoCompania] [nvarchar](8) NULL,
	[sTamanioActividad] [nvarchar](200) NULL,
	[sCodigoTamanioActividad] [nvarchar](40) NULL,
	[iNumeroEmpleados] [int] NULL,
	[sAgenciaManejo] [nvarchar](25) NULL,
	[sCodigoAgenciaManejo] [nvarchar](5) NULL,
	[dFechaCreacionCliente] [datetime] NULL,
	[dFechaConstitucionEmpresa] [datetime] NULL,
	[sCalificacionClienteBco] [nvarchar](50) NULL,
	[sNombreEjecutivo] [nvarchar](70) NULL,
	[sLoginEjecutivo] [nvarchar](10) NULL,
	[sCodigoEjecutivo] [nvarchar](4) NULL,
	[sActividadEconomica] [nvarchar](80) NULL,
	[sCodigoActividadEconomica] [nvarchar](4) NULL,
	[sDetalleActividadEconomica] [nvarchar](200) NULL,
	[sCodigoDetalleActividadEconomica] [nvarchar](8) NULL,
	[sAgenciaCreacion] [nvarchar](25) NULL,
	[sCodigoAgenciaCreacion] [nvarchar](25) NULL,
	[sCodigoSegmento] [nvarchar](4) NULL,
	[bAtencionPreferente] [bit] NULL,
	[sDescripcionAtencionPreferente] [nvarchar](100) NULL,
	[sTelefono] [nvarchar](40) NULL,
	[sCorreoElectronico] [nvarchar](40) NULL,
	[sTelefonoMovil] [nvarchar](15) NULL,
	[cIngresosAnuales] [money] NULL,
	[sCiudad] [nvarchar](100) NULL,
	[sBarrio] [nvarchar](100) NULL,
	[sZona] [nvarchar](100) NULL,
	[iAniosExperienciaRubro] [smallint] NULL,
	[sSexo] [nvarchar](12) NULL,
	[sCodigoSexo] [nvarchar](4) NULL,
	[sEstadoCivil] [nvarchar](100) NULL,
	[sCodigoEstadoCivil] [nvarchar](4) NULL,
	[sPaisOrigen] [nvarchar](100) NULL,
	[dFechaNacimiento] [datetime] NULL,
	[iNumeroHijos] [int] NULL,
	[sNivelDeEstudios] [nvarchar](100) NULL,
	[sCodigoNivelEstudios] [nvarchar](4) NULL,
	[sProfesion] [nvarchar](100) NULL,
	[sEstadoCliente] [nvarchar](100) NULL,
	[sCodigoEstadoCliente] [nvarchar](2) NULL,
	[sCargoActual] [nvarchar](30) NULL,
	[cIngresosMensuales] [money] NULL,
	[sEmpresa] [nvarchar](60) NULL,
	[dFechaIngresoEmpresa] [datetime] NULL,
	[sCodigoSucursal] [nvarchar](3) NULL,
	[sSiglaDiminutivo] [nvarchar](40) NULL,
	[sSujetoImponible] [nvarchar](100) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BDI_CentralRiesgo]    Script Date: 08/27/2013 11:35:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BDI_CentralRiesgo](
	[iCodigoClienteBanco] [int] NULL,
	[sInstitucionFinanciera] [nvarchar](80) NULL,
	[sCalificacion] [nvarchar](50) NULL,
	[cMontoRiesgo] [money] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BDI_CatalogoRelacionesCliente]    Script Date: 08/27/2013 11:35:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BDI_CatalogoRelacionesCliente](
	[sCodigoRelacion] [nvarchar](5) NULL,
	[sTextoRolIzq] [nvarchar](100) NULL,
	[sTextoRolDer] [nvarchar](100) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BDI_CargaProcesosLog]    Script Date: 08/27/2013 11:35:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BDI_CargaProcesosLog](
	[sProcesoBDI] [varchar](100) NULL,
	[iCodigoProceso] [int] NULL,
	[dFechaUltimaCarga] [datetime] NULL,
	[sEstadoBDICargaProcesos] [varchar](20) NULL,
	[sEstadoFinal] [varchar](20) NULL,
	[dFechaInicioEjecucion] [datetime] NULL,
	[dFechaFinEjecucion] [datetime] NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BDI_CargaProcesos]    Script Date: 08/27/2013 11:35:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BDI_CargaProcesos](
	[sProcesoBDI] [varchar](100) NULL,
	[iCodigoProceso] [int] NULL,
	[dFechaUltimaCarga] [datetime] NULL,
	[sEstado] [varchar](20) NULL,
	[dFechaInicioEjecucion] [datetime] NULL,
	[dFechaFinEjecucion] [datetime] NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BDI_Canales]    Script Date: 08/27/2013 11:35:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BDI_Canales](
	[iCodigoClienteBanco] [int] NULL,
	[sCodigoUnicoProducto] [nvarchar](13) NULL,
	[fCantPromTrimestralTransUltMeses] [float] NULL,
	[fPorcPromUsoCanalUltMeses] [float] NULL,
	[fFrecuenciaUsoDiarioUltMeses] [float] NULL,
	[dFechaUltimoUso] [datetime] NULL,
	[cMontoPromTrimestralTransUltMeses] [money] NULL,
	[dFechaAperturaEmision] [datetime] NULL,
	[sCodigoEjecutivoCreacionEmision] [nvarchar](5) NULL,
	[sCodigoAgenciaCreacionEmision] [nvarchar](5) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BDI_Agencias]    Script Date: 08/27/2013 11:35:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BDI_Agencias](
	[sCodigoAgencia] [nvarchar](5) NULL,
	[sNombreAgencia] [nvarchar](25) NULL,
	[sCodigoRegion] [nvarchar](3) NULL,
	[sRegion] [nvarchar](25) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DWH_BDI_LogError]    Script Date: 08/27/2013 11:35:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DWH_BDI_LogError](
	[dFecha] [datetime] NULL,
	[sProcPrincipal] [varchar](50) NULL,
	[sProcCarga] [varchar](2000) NULL,
	[sUsuario] [varchar](50) NULL,
	[sTipoEntrada] [char](1) NULL,
	[sMensaje] [varchar](5000) NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BDI_Telefonos]    Script Date: 08/27/2013 11:35:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BDI_Telefonos](
	[iCodigoClienteBanco] [int] NULL,
	[iCodigoTelefono] [nvarchar](30) NULL,
	[sNumeroTelefonico] [nvarchar](15) NULL,
	[sTipoTelefono] [nvarchar](20) NULL,
	[sCodigoTipoTelefono] [nvarchar](4) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BDI_Servicios]    Script Date: 08/27/2013 11:35:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BDI_Servicios](
	[iCodigoClienteBanco] [int] NULL,
	[sCodigoUnicoProducto] [nvarchar](13) NULL,
	[sNumeroOperacion] [nvarchar](100) NULL,
	[sEstadoOperacion] [nvarchar](5) NULL,
	[iCantTransaccionesUltMes] [int] NULL,
	[cMontoTotalTansaccionesUltMes] [money] NULL,
	[fCantPromTrimestralTransUltMeses] [float] NULL,
	[cMontoTotalPromTrimestralTransUltMeses] [money] NULL,
	[cMontoPromxTransaccionUltMeses] [money] NULL,
	[cPromTrimestralMontoComisionxMesUltMeses] [money] NULL,
	[cComisionPactada] [money] NULL,
	[dFechaApertura] [datetime] NULL,
	[fFrecuenciaUsoDiarioUltMes] [float] NULL,
	[sCodigoEjecutivoCreacionEmision] [nvarchar](5) NULL,
	[sCodigoAgenciaCreacionEmision] [nvarchar](5) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BDI_Segmentos]    Script Date: 08/27/2013 11:35:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BDI_Segmentos](
	[sSegmento] [nvarchar](100) NULL,
	[sCodigoSegmento] [nvarchar](4) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BDI_RepresentantesLegales]    Script Date: 08/27/2013 11:35:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BDI_RepresentantesLegales](
	[iCodigoClienteBanco] [int] NULL,
	[sNombreCompleto] [nvarchar](40) NULL,
	[sCargo] [nvarchar](30) NULL,
	[sCodigoTipoIdentificacion] [nvarchar](1) NULL,
	[sTipoIdentificacion] [nvarchar](100) NULL,
	[sNumeroIdentificacion] [nvarchar](19) NULL,
	[iCodigoBancoRepLegal] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BDI_RelacionesCliente]    Script Date: 08/27/2013 11:35:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BDI_RelacionesCliente](
	[iCodigoClienteBancoIzq] [int] NULL,
	[iCodigoClienteBancoDer] [int] NULL,
	[sCodigoRelacion] [nvarchar](5) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BDI_ProductoConciliar]    Script Date: 08/27/2013 11:35:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BDI_ProductoConciliar](
	[iCodigoClienteBanco] [int] NULL,
	[iNumSolicitud] [int] NULL,
	[sNumOperacion] [nvarchar](10) NULL,
	[sCodigoModulo] [nvarchar](4) NULL,
	[sCodigoProducto] [nvarchar](6) NULL,
	[sCodigoTipo] [nvarchar](8) NULL,
	[sCodigoProductoFisa] [nvarchar](18) NULL,
	[sCodigoUnicoProducto] [nvarchar](13) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BDI_ProdPasivos]    Script Date: 08/27/2013 11:35:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BDI_ProdPasivos](
	[iCodigoClienteBanco] [int] NULL,
	[sCodigoModulo] [nvarchar](4) NULL,
	[sCodigoProducto] [nvarchar](6) NULL,
	[sCodigoTipo] [nvarchar](8) NULL,
	[sCodigoProductoFisa] [nvarchar](18) NULL,
	[sCodigoUnicoProducto] [nvarchar](13) NULL,
	[nNroCuenta] [numeric](10, 0) NULL,
	[sEstadoOperacion] [nvarchar](20) NULL,
	[sCodigoMoneda] [nvarchar](4) NULL,
	[cMontoOper] [money] NULL,
	[dFechaVencimiento] [datetime] NULL,
	[iPlazoDias] [int] NULL,
	[cSaldoDisponible] [money] NULL,
	[cSaldoContableActual] [money] NULL,
	[cPromedioTrimestral] [money] NULL,
	[dFechaAperturaEmision] [datetime] NULL,
	[iDiasSobregiro] [int] NULL,
	[fTasaActual] [float] NULL,
	[bCuentaBloqueada] [bit] NULL,
	[bRetencionJudicial] [bit] NULL,
	[sManejoDeCuenta] [nvarchar](100) NULL,
	[sTipoPagoIntereses] [nvarchar](20) NULL,
	[cInteresesVencimiento] [money] NULL,
	[bPignorado] [bit] NULL,
	[cMontoTotalDepMensuales] [money] NULL,
	[cMontoTotalTansfCtasPropias] [money] NULL,
	[cMontoTotalTansfTercerosBMSC] [money] NULL,
	[cMontoTotalTansfTercerosOtros] [money] NULL,
	[cSaldoBloqueado] [money] NULL,
	[iNumeroRenovacion] [int] NULL,
	[bPoseeTarjetaDebito] [bit] NULL,
	[sCodigoEjecutivoCreacionEmision] [nvarchar](5) NULL,
	[sCodigoAgenciaCreacionEmision] [nvarchar](5) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BDI_ProdActivosTDC]    Script Date: 08/27/2013 11:35:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BDI_ProdActivosTDC](
	[iCodigoClienteBanco] [int] NULL,
	[sCodigoMoneda] [nvarchar](4) NULL,
	[sCodigoModulo] [nvarchar](4) NULL,
	[sCodigoProducto] [nvarchar](6) NULL,
	[sCodigoTipo] [nvarchar](8) NULL,
	[sCodigoProductoFISA] [nvarchar](18) NULL,
	[sCodigoUnicoProducto] [nvarchar](13) NULL,
	[sNroTarjeta] [nvarchar](20) NULL,
	[sNroCuenta] [numeric](10, 0) NULL,
	[sEstadoTarjeta] [nvarchar](50) NULL,
	[sEstadoCuenta] [nvarchar](20) NULL,
	[iCantidadDIvidendosVencidos] [smallint] NULL,
	[dFechaVencimientoOperacion] [datetime] NULL,
	[cDeudaCapital] [money] NULL,
	[cLimiteCompra] [money] NULL,
	[cMontoUtilizado] [money] NULL,
	[cGastosGenerados] [money] NULL,
	[cLimiteFinanciamiento] [money] NULL,
	[bRealizaCOmprasExterior] [bit] NULL,
	[bTieneTarjetasAdicionales] [bit] NULL,
	[cMontoPagoMinimo] [money] NULL,
	[cMontoVencidoPagar] [money] NULL,
	[bDebitoAutomatico] [bit] NULL,
	[sTipoPagoDebitoAutomatico] [nvarchar](20) NULL,
	[dFechaVencimientoPlastico] [datetime] NULL,
	[sTipoTDC] [nvarchar](100) NULL,
	[sCodigoEjecutivoCreacionEmision] [nvarchar](5) NULL,
	[sCodigoAgenciaCreacionEmision] [nvarchar](5) NULL,
	[sNombreTarjetahabiente] [nvarchar](25) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BDI_ProdActivosLineasCredito]    Script Date: 08/27/2013 11:35:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BDI_ProdActivosLineasCredito](
	[iCodigoClienteBanco] [int] NULL,
	[sCodigoMoneda] [nvarchar](4) NULL,
	[sCodigoModulo] [nvarchar](4) NULL,
	[sCodigoProducto] [nvarchar](6) NULL,
	[sCodigoTipo] [nvarchar](8) NULL,
	[sCodigoProductoFisa] [nvarchar](18) NULL,
	[sCodigoUnicoProducto] [nvarchar](13) NULL,
	[sNroOperacion] [nvarchar](15) NULL,
	[dFechaAperturaOperacion] [datetime] NULL,
	[dFechaVencimientoOperacion] [datetime] NULL,
	[cMontoAprobado] [money] NULL,
	[cMontoUtilizado] [money] NULL,
	[cMontoDisponible] [money] NULL,
	[fPorcUso] [decimal](10, 2) NULL,
	[sSubLimiteLineaCredito] [nvarchar](40) NULL,
	[sCodigoEjecutivoCreacionEmision] [nvarchar](5) NULL,
	[sCodigoAgenciaCreacionEmision] [nvarchar](5) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BDI_ProdActivosGarantiasOperaciones]    Script Date: 08/27/2013 11:35:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BDI_ProdActivosGarantiasOperaciones](
	[sCodigoUnicoProductoCredito] [nvarchar](13) NULL,
	[sNroOperacionCredito] [nvarchar](10) NULL,
	[iCodigoClienteBancoCredito] [int] NULL,
	[sNumeroTarjetaCredito] [nvarchar](30) NULL,
	[sCodigoUnicoProductoGarantia] [nvarchar](13) NULL,
	[sNroOperacionGarantia] [nvarchar](10) NULL,
	[iCodigoClienteBancoGarantia] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BDI_ProdActivosGarantias]    Script Date: 08/27/2013 11:35:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BDI_ProdActivosGarantias](
	[iCodigoClienteBanco] [int] NULL,
	[sCodigoMoneda] [nvarchar](4) NULL,
	[sCodigoModulo] [nvarchar](4) NULL,
	[sCodigoProducto] [nvarchar](6) NULL,
	[sCodigoTipo] [nvarchar](8) NULL,
	[sCodigoProductoFISA] [nvarchar](18) NULL,
	[sCodigoUnicoProducto] [nvarchar](13) NULL,
	[nNroCuenta] [nvarchar](10) NULL,
	[sEstadoOperacion] [nvarchar](25) NULL,
	[cValorLiquidacion] [money] NULL,
	[cValorComercial] [money] NULL,
	[sDescripcionGarantia] [nvarchar](500) NULL,
	[sOperacionesGarantizadas] [nvarchar](4000) NULL,
	[sNumeroPoliza] [nvarchar](20) NULL,
	[dFechaVencimientoPoliza] [datetime] NULL,
	[sCompaniaPoliza] [nvarchar](100) NULL,
	[sCodigoLineaCredito] [nvarchar](10) NULL,
	[cSaldoDisponible] [money] NULL,
	[dFechaUltimoAvaluo] [datetime] NULL,
	[sCodigoEjecutivoCreacionEmision] [nvarchar](5) NULL,
	[sCodigoAgenciaCreacionEmision] [nvarchar](5) NULL,
	[iCodigoClienteGarante] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BDI_ProdActivosCreditos]    Script Date: 08/27/2013 11:35:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BDI_ProdActivosCreditos](
	[iCodigoClienteBanco] [int] NULL,
	[sCodigoMoneda] [nvarchar](4) NULL,
	[sCodigoModulo] [nvarchar](4) NULL,
	[sCodigoProducto] [nvarchar](6) NULL,
	[sCodigoTipo] [nvarchar](8) NULL,
	[sCodigoProductoFisa] [nvarchar](18) NULL,
	[sCodigoUnicoProducto] [nvarchar](13) NULL,
	[sNroCuenta] [numeric](10, 0) NULL,
	[sEstadoOperacion] [nvarchar](20) NULL,
	[iCantidadDIvidendosVencidos] [smallint] NULL,
	[cMontoDeLaOperacionCapital] [money] NULL,
	[cMontoPagadoCapital] [money] NULL,
	[cMontoVencidoCapital] [money] NULL,
	[dFechaVencimientoOperacion] [datetime] NULL,
	[dFechaInicioOperacionEmision] [datetime] NULL,
	[cSaldoOperacion] [money] NULL,
	[iPlazoDias] [int] NULL,
	[fTasaActual] [float] NULL,
	[fTasaTRE] [float] NULL,
	[dFechaPagoDividendoVigente] [datetime] NULL,
	[cMontoPagoDividendoVigente] [money] NULL,
	[bDebitoAutomatico] [bit] NULL,
	[cMontoVencidoPagar] [money] NULL,
	[cValorComercialVivienda] [money] NULL,
	[cValorComercialVehiculos] [money] NULL,
	[sCodigoLineaCredito] [nvarchar](10) NULL,
	[bReprogramado] [bit] NULL,
	[iNumeroCuotas] [int] NULL,
	[sCodigoEjecutivoCreacionEmision] [nvarchar](5) NULL,
	[sCodigoAgenciaCreacionEmision] [nvarchar](5) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BDI_ProdActivosComex]    Script Date: 08/27/2013 11:35:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BDI_ProdActivosComex](
	[iCodigoClienteBanco] [int] NULL,
	[sCodigoMoneda] [nvarchar](4) NULL,
	[sCodigoModulo] [nvarchar](4) NULL,
	[sCodigoProducto] [nvarchar](6) NULL,
	[sCodigoTipo] [nvarchar](8) NULL,
	[sCodigoProductoFISA] [nvarchar](18) NULL,
	[sCodigoUnicoProducto] [nvarchar](13) NULL,
	[nNroCuenta] [numeric](10, 0) NULL,
	[sEstadoOperacion] [nvarchar](20) NULL,
	[cMontoDeLaOperacionCapital] [money] NULL,
	[dFechaVencimientoOperacion] [datetime] NULL,
	[dFechaInicioOperacionEmision] [datetime] NULL,
	[cSaldoOperacion] [money] NULL,
	[iPlazo] [int] NULL,
	[sBeneficiario] [nvarchar](80) NULL,
	[sCodigoLineaCredito] [nvarchar](10) NULL,
	[cMontoUtilizacionCartaCredito] [money] NULL,
	[bPermiteEmbarquesParciales] [bit] NULL,
	[fToleranciaMaxima] [decimal](10, 2) NULL,
	[fToleranciaMinima] [decimal](10, 2) NULL,
	[sTipoGarantia] [nvarchar](40) NULL,
	[sOrdenante] [nvarchar](60) NULL,
	[sCodigoEjecutivoCreacionEmision] [nvarchar](5) NULL,
	[sCodigoAgenciaCreacionEmision] [nvarchar](5) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BDI_PolizasGarantias]    Script Date: 08/27/2013 11:35:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BDI_PolizasGarantias](
	[sNumeroPoliza] [nvarchar](16) NULL,
	[sNroOperacionGarantia] [nvarchar](10) NULL,
	[iCodigoClienteBancoGarantia] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BDI_Polizas]    Script Date: 08/27/2013 11:35:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BDI_Polizas](
	[iCodigoClienteBanco] [int] NULL,
	[sNumeroPoliza] [nvarchar](16) NULL,
	[sCompaniaAseguradora] [nvarchar](100) NULL,
	[dFechaVencimiento] [datetime] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BDI_InfoAdicionalClientes]    Script Date: 08/27/2013 11:35:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BDI_InfoAdicionalClientes](
	[iCodigoClienteBanco] [int] NULL,
	[sIndiceRelacionActual] [int] NULL,
	[iCantidadProductosDelActivo] [int] NULL,
	[iCantidadProductosDelPasivo] [int] NULL,
	[iCantidadOperacionesDelActivoCanceladas] [int] NULL,
	[bRetencionesJudiciales] [bit] NULL,
	[bCuentasCorrientesClausuradas] [bit] NULL,
	[cTotalCreditoVivienda] [money] NULL,
	[cTotalCreditoVehicular] [money] NULL,
	[cTotalCreditoConsumo] [money] NULL,
	[cTotalCreditoComercial] [money] NULL,
	[cTotalProductosDelActivo] [money] NULL,
	[cMontoMaximoDeuda24mesesBco] [money] NULL,
	[cMontoMaximoDeuda24mesesSF] [money] NULL,
	[cVolumenTotalNegocio] [money] NULL,
	[cVolumenPromedioNegocio24meses] [money] NULL,
	[cVolumenMaximoNegocio24meses] [money] NULL,
	[cTotalGarantias] [money] NULL,
	[cTotalTarjetaCredito] [money] NULL,
	[cTotalCuentaCorriente] [money] NULL,
	[cTotalCuentaAhorro] [money] NULL,
	[cTotalDPF] [money] NULL,
	[cTotalProductosPasivo] [money] NULL,
	[cTotalContingentes] [money] NULL,
	[cTotalCentralRiesgoUltimoMes] [money] NULL,
	[cTotalValorLiquidableGarantias] [money] NULL,
	[cTotalSaldoArrastreGarantias] [money] NULL,
	[fCoberturaGarantiasVigentes] [decimal](10, 2) NULL,
	[cTotalLineasCreditos] [money] NULL,
	[cTotalCoberturaLineasCreditos] [money] NULL,
	[fUtilizacionLineasCreditos] [decimal](10, 2) NULL,
	[cTotalRiesgoClienteBMSC] [money] NULL,
	[cTotalRiesgoClienteSF] [money] NULL,
	[cTotalRiesgoContingenteSF] [money] NULL,
	[sCalificacionSF] [nvarchar](50) NULL,
	[fParticipacionBMSCRiesgoTotal] [decimal](10, 2) NULL,
	[fIndiceReciprocidad] [decimal](23, 6) NULL,
	[fIndiceCLV] [decimal](23, 6) NULL,
	[cTotalComGarantiasHipotecarias] [money] NULL,
	[cTotalComGarantiasDepositos] [money] NULL,
	[cTotalComGarantiasPersonales] [money] NULL,
	[cTotalComGarantiasPrendarias] [money] NULL,
	[cTotalComGarantiasOtros] [money] NULL,
	[cTotalRiesgoGrupoEconomico] [money] NULL,
	[bPoseeCreditosMora] [bit] NULL,
	[fIndiceLiquidezCortoPlazo] [decimal](23, 6) NULL,
	[fROE] [decimal](23, 6) NULL,
	[fROA] [decimal](23, 6) NULL,
	[fROS] [decimal](23, 6) NULL,
	[fApalancamientoFinanciero] [decimal](23, 6) NULL,
	[fApalancamientoTotal] [decimal](23, 6) NULL,
	[cTotalDepositosUltMes] [money] NULL,
	[cTotalTransferenciasCuentasPropiasUltMes] [money] NULL,
	[cTotalTransferenciasTercerosBancoUltMes] [money] NULL,
	[cTotalTransferenciasTercerosOtrosUltMes] [money] NULL,
	[cTotalDepositosUltTrim] [money] NULL,
	[cTotalTransferenciasCuentasPropiasUltTrim] [money] NULL,
	[cTotalTransferenciasTercerosBancoUltTrim] [money] NULL,
	[cTotalTransferenciasTercerosOtrosUltTrim] [money] NULL,
	[sClubPuntosCodigo] [nvarchar](10) NULL,
	[iClubPuntosAcumuladoMes1] [int] NULL,
	[iClubPuntosAcumuladoMes2] [int] NULL,
	[iClubPuntosAcumuladoMes3] [int] NULL,
	[sClubPuntosEstado] [nvarchar](10) NULL,
	[iClbPuntosTotalPuntos] [int] NULL,
	[iCantidadGirosComexUltMes] [int] NULL,
	[cMontoTotaslGirosComexUltMes] [money] NULL,
	[bPoseeCreditosVencidos] [bit] NULL,
	[bPoseePagoSueldosSalarios] [bit] NULL,
	[bPoseePagoProveedores] [bit] NULL,
	[bPoseePagoColegios] [bit] NULL,
	[bPoseeDepositosNumerados] [bit] NULL,
	[bPoseePagosAutomaticos] [bit] NULL,
	[bPoseeDebitosAutomaticos] [bit] NULL,
	[sPeorCalificacionSF_x_12_meses] [nvarchar](50) NULL,
	[iNroVecesCalificacionDistinta_12_meses] [int] NULL,
	[iNroVecesEnEjecucion_12_meses] [int] NULL,
	[iNroVecesEnVigente_mas_14_días] [int] NULL,
	[cValorLiquidableGarantias] [money] NULL,
	[iNroVecesVencido_x_insolvencia] [int] NULL,
	[cTotal_Cartera_Garantia_Hipotecaria] [money] NULL,
	[cTotal_Cartera_Garantia_Personal] [money] NULL,
	[cTtotal_Catera_A_Sola_Firma] [money] NULL,
	[cTotal_CuotasBMSC] [money] NULL,
	[cTtotalRiesgoClienteSF] [money] NULL,
	[cTtotalRiesgoContingenteSF] [money] NULL,
	[iNroVecesVencido_x_Insolvencia_Garante] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BDI_GruposEconomicosClientes]    Script Date: 08/27/2013 11:35:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BDI_GruposEconomicosClientes](
	[iCodigoClienteBanco] [int] NULL,
	[sCodigoGrupoEconomico] [nvarchar](3) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BDI_GruposEconomicos]    Script Date: 08/27/2013 11:35:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BDI_GruposEconomicos](
	[sCodigoGrupoEconomico] [nvarchar](3) NULL,
	[sGrupoEconomico] [nvarchar](100) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BDI_Ejecutivos]    Script Date: 08/27/2013 11:35:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BDI_Ejecutivos](
	[sLoginEjecutivo] [nvarchar](10) NULL,
	[sNombreEjecutivo] [nvarchar](70) NULL,
	[sCodigoBanca] [nvarchar](4) NULL,
	[sDescripcionBanca] [nvarchar](100) NULL,
	[sCodigoAgencia] [nvarchar](5) NULL,
	[sCodigoRegional] [nvarchar](3) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BDI_Direcciones]    Script Date: 08/27/2013 11:35:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BDI_Direcciones](
	[iCodigoClienteBanco] [int] NULL,
	[iCodigoDireccion] [numeric](10, 0) NULL,
	[sCodigoCiudad] [nvarchar](4) NULL,
	[sCiudad] [nvarchar](100) NULL,
	[sCodigoBarrio] [nvarchar](4) NULL,
	[sBarrio] [nvarchar](100) NULL,
	[sCodigoZona] [nvarchar](4) NULL,
	[sZona] [nvarchar](100) NULL,
	[sCalle] [nvarchar](400) NULL,
	[sTipoDireccion] [nvarchar](100) NULL,
	[sCodigoTipoDireccion] [nvarchar](4) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BDI_CRM_UsuariosAplicacion]    Script Date: 08/27/2013 11:35:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BDI_CRM_UsuariosAplicacion](
	[iCodigo] [int] IDENTITY(1,1) NOT NULL,
	[sUsuarioDominio] [nvarchar](20) NULL,
	[sTipoUsuario] [char](1) NULL,
	[sNombre] [nvarchar](100) NULL,
	[sDescripcion] [nvarchar](500) NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BDI_CRM_TMP_Reasignacion]    Script Date: 08/27/2013 11:35:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BDI_CRM_TMP_Reasignacion](
	[iCodigoClienteBanco] [int] NULL,
	[sLoginEjecutivo] [nvarchar](10) NULL,
	[uClienteId] [uniqueidentifier] NULL,
	[uOwnerId] [uniqueidentifier] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BDI_CRM_Telefonos_ERROR]    Script Date: 08/27/2013 11:35:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BDI_CRM_Telefonos_ERROR](
	[iCodigoClienteBanco] [int] NULL,
	[iCodigoTelefono] [nvarchar](30) NULL,
	[sNumeroTelefonico] [nvarchar](15) NULL,
	[sTipoTelefono] [nvarchar](20) NULL,
	[sCodigoTipoTelefono] [nvarchar](4) NULL,
	[sTipoReg] [char](1) NULL,
	[dFechaUltimoError] [datetime] NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BDI_CRM_Telefonos_DIF]    Script Date: 08/27/2013 11:35:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BDI_CRM_Telefonos_DIF](
	[iCodigoClienteBanco] [int] NULL,
	[iCodigoTelefono] [nvarchar](30) NULL,
	[sNumeroTelefonico] [nvarchar](15) NULL,
	[sTipoTelefono] [nvarchar](20) NULL,
	[sCodigoTipoTelefono] [nvarchar](4) NULL,
	[sTipoReg] [char](1) NULL,
	[bError] [bit] NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BDI_CRM_Telefonos_ACT]    Script Date: 08/27/2013 11:35:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BDI_CRM_Telefonos_ACT](
	[iCodigoClienteBanco] [int] NULL,
	[iCodigoTelefono] [nvarchar](30) NULL,
	[sNumeroTelefonico] [nvarchar](15) NULL,
	[sTipoTelefono] [nvarchar](20) NULL,
	[sCodigoTipoTelefono] [nvarchar](4) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BDI_CRM_Servicios_DIF]    Script Date: 08/27/2013 11:35:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BDI_CRM_Servicios_DIF](
	[iCodigoClienteBanco] [int] NULL,
	[sCodigoUnicoProducto] [nvarchar](13) NULL,
	[sNumeroOperacion] [nvarchar](100) NULL,
	[sEstadoOperacion] [nvarchar](5) NULL,
	[iCantTransaccionesUltMes] [int] NULL,
	[cMontoTotalTansaccionesUltMes] [money] NULL,
	[fCantPromTrimestralTransUltMeses] [float] NULL,
	[cMontoTotalPromTrimestralTransUltMeses] [money] NULL,
	[cMontoPromxTransaccionUltMeses] [money] NULL,
	[cPromTrimestralMontoComisionxMesUltMeses] [money] NULL,
	[cComisionPactada] [money] NULL,
	[dFechaApertura] [datetime] NULL,
	[fFrecuenciaUsoDiarioUltMes] [float] NULL,
	[sCodigoEjecutivoCreacionEmision] [nvarchar](5) NULL,
	[sCodigoAgenciaCreacionEmision] [nvarchar](5) NULL,
	[sTipoReg] [char](1) NULL,
	[bError] [bit] NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BDI_CRM_Servicios_ACT]    Script Date: 08/27/2013 11:35:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BDI_CRM_Servicios_ACT](
	[iCodigoClienteBanco] [int] NULL,
	[sCodigoUnicoProducto] [nvarchar](13) NULL,
	[sNumeroOperacion] [nvarchar](100) NULL,
	[sEstadoOperacion] [nvarchar](5) NULL,
	[iCantTransaccionesUltMes] [int] NULL,
	[cMontoTotalTansaccionesUltMes] [money] NULL,
	[fCantPromTrimestralTransUltMeses] [float] NULL,
	[cMontoTotalPromTrimestralTransUltMeses] [money] NULL,
	[cMontoPromxTransaccionUltMeses] [money] NULL,
	[cPromTrimestralMontoComisionxMesUltMeses] [money] NULL,
	[cComisionPactada] [money] NULL,
	[dFechaApertura] [datetime] NULL,
	[fFrecuenciaUsoDiarioUltMes] [float] NULL,
	[sCodigoEjecutivoCreacionEmision] [nvarchar](5) NULL,
	[sCodigoAgenciaCreacionEmision] [nvarchar](5) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BDI_CRM_RepresentantesLegales_DIF]    Script Date: 08/27/2013 11:35:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BDI_CRM_RepresentantesLegales_DIF](
	[iCodigoClienteBanco] [int] NULL,
	[sNombreCompleto] [nvarchar](40) NULL,
	[sCargo] [nvarchar](30) NULL,
	[sCodigoTipoIdentificacion] [nvarchar](1) NULL,
	[sTipoIdentificacion] [nvarchar](100) NULL,
	[sNumeroIdentificacion] [nvarchar](19) NULL,
	[sTipoReg] [char](1) NULL,
	[bError] [bit] NULL,
	[iCodigoBancoRepLegal] [int] NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BDI_CRM_RepresentantesLegales_ACT]    Script Date: 08/27/2013 11:35:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BDI_CRM_RepresentantesLegales_ACT](
	[iCodigoClienteBanco] [int] NULL,
	[sNombreCompleto] [nvarchar](40) NULL,
	[sCargo] [nvarchar](30) NULL,
	[sCodigoTipoIdentificacion] [nvarchar](1) NULL,
	[sTipoIdentificacion] [nvarchar](100) NULL,
	[sNumeroIdentificacion] [nvarchar](19) NULL,
	[iCodigoBancoRepLegal] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BDI_CRM_RelacionesCliente_DIF]    Script Date: 08/27/2013 11:35:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BDI_CRM_RelacionesCliente_DIF](
	[iCodigoClienteBancoIzq] [int] NULL,
	[iCodigoClienteBancoDer] [int] NULL,
	[sCodigoRelacion] [nvarchar](5) NULL,
	[sTipoReg] [char](1) NULL,
	[bError] [bit] NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BDI_CRM_RelacionesCliente_ACT]    Script Date: 08/27/2013 11:35:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BDI_CRM_RelacionesCliente_ACT](
	[iCodigoClienteBancoIzq] [int] NULL,
	[iCodigoClienteBancoDer] [int] NULL,
	[sCodigoRelacion] [nvarchar](5) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BDI_CRM_ProdPasivos_DIF]    Script Date: 08/27/2013 11:35:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BDI_CRM_ProdPasivos_DIF](
	[iCodigoClienteBanco] [int] NULL,
	[sCodigoModulo] [nvarchar](4) NULL,
	[sCodigoProducto] [nvarchar](6) NULL,
	[sCodigoTipo] [nvarchar](8) NULL,
	[sCodigoProductoFisa] [nvarchar](18) NULL,
	[sCodigoUnicoProducto] [nvarchar](13) NULL,
	[nNroCuenta] [numeric](10, 0) NULL,
	[sEstadoOperacion] [nvarchar](20) NULL,
	[sCodigoMoneda] [nvarchar](4) NULL,
	[cMontoOper] [money] NULL,
	[dFechaVencimiento] [datetime] NULL,
	[iPlazoDias] [int] NULL,
	[cSaldoDisponible] [money] NULL,
	[cSaldoContableActual] [money] NULL,
	[cPromedioTrimestral] [money] NULL,
	[dFechaAperturaEmision] [datetime] NULL,
	[iDiasSobregiro] [int] NULL,
	[fTasaActual] [float] NULL,
	[bCuentaBloqueada] [bit] NULL,
	[bRetencionJudicial] [bit] NULL,
	[sManejoDeCuenta] [nvarchar](100) NULL,
	[sTipoPagoIntereses] [nvarchar](20) NULL,
	[cInteresesVencimiento] [money] NULL,
	[sTipoReg] [char](1) NULL,
	[bError] [bit] NULL,
	[bPignorado] [bit] NULL,
	[cMontoTotalDepMensuales] [money] NULL,
	[cMontoTotalTansfCtasPropias] [money] NULL,
	[cMontoTotalTansfTercerosBMSC] [money] NULL,
	[cMontoTotalTansfTercerosOtros] [money] NULL,
	[cSaldoBloqueado] [money] NULL,
	[iNumeroRenovacion] [int] NULL,
	[bPoseeTarjetaDebito] [bit] NULL,
	[sCodigoEjecutivoCreacionEmision] [nvarchar](5) NULL,
	[sCodigoAgenciaCreacionEmision] [nvarchar](5) NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BDI_CRM_ProdPasivos_ACT]    Script Date: 08/27/2013 11:35:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BDI_CRM_ProdPasivos_ACT](
	[iCodigoClienteBanco] [int] NULL,
	[sCodigoModulo] [nvarchar](4) NULL,
	[sCodigoProducto] [nvarchar](6) NULL,
	[sCodigoTipo] [nvarchar](8) NULL,
	[sCodigoProductoFisa] [nvarchar](18) NULL,
	[sCodigoUnicoProducto] [nvarchar](13) NULL,
	[nNroCuenta] [numeric](10, 0) NULL,
	[sEstadoOperacion] [nvarchar](20) NULL,
	[sCodigoMoneda] [nvarchar](4) NULL,
	[cMontoOper] [money] NULL,
	[dFechaVencimiento] [datetime] NULL,
	[iPlazoDias] [int] NULL,
	[cSaldoDisponible] [money] NULL,
	[cSaldoContableActual] [money] NULL,
	[cPromedioTrimestral] [money] NULL,
	[dFechaAperturaEmision] [datetime] NULL,
	[iDiasSobregiro] [int] NULL,
	[fTasaActual] [float] NULL,
	[bCuentaBloqueada] [bit] NULL,
	[bRetencionJudicial] [bit] NULL,
	[sManejoDeCuenta] [nvarchar](100) NULL,
	[sTipoPagoIntereses] [nvarchar](20) NULL,
	[cInteresesVencimiento] [money] NULL,
	[bPignorado] [bit] NULL,
	[cMontoTotalDepMensuales] [money] NULL,
	[cMontoTotalTansfCtasPropias] [money] NULL,
	[cMontoTotalTansfTercerosBMSC] [money] NULL,
	[cMontoTotalTansfTercerosOtros] [money] NULL,
	[cSaldoBloqueado] [money] NULL,
	[iNumeroRenovacion] [int] NULL,
	[bPoseeTarjetaDebito] [bit] NULL,
	[sCodigoEjecutivoCreacionEmision] [nvarchar](5) NULL,
	[sCodigoAgenciaCreacionEmision] [nvarchar](5) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BDI_CRM_ProdActivosTDC_DIF]    Script Date: 08/27/2013 11:35:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BDI_CRM_ProdActivosTDC_DIF](
	[iCodigoClienteBanco] [int] NULL,
	[sCodigoMoneda] [nvarchar](4) NULL,
	[sCodigoModulo] [nvarchar](4) NULL,
	[sCodigoProducto] [nvarchar](6) NULL,
	[sCodigoTipo] [nvarchar](8) NULL,
	[sCodigoProductoFISA] [nvarchar](18) NULL,
	[sCodigoUnicoProducto] [nvarchar](13) NULL,
	[sNroTarjeta] [nvarchar](20) NULL,
	[sNroCuenta] [numeric](10, 0) NULL,
	[sEstadoTarjeta] [nvarchar](50) NULL,
	[sEstadoCuenta] [nvarchar](20) NULL,
	[iCantidadDIvidendosVencidos] [smallint] NULL,
	[dFechaVencimientoOperacion] [datetime] NULL,
	[cDeudaCapital] [money] NULL,
	[cLimiteCompra] [money] NULL,
	[cMontoUtilizado] [money] NULL,
	[cGastosGenerados] [money] NULL,
	[cLimiteFinanciamiento] [money] NULL,
	[bRealizaCOmprasExterior] [bit] NULL,
	[bTieneTarjetasAdicionales] [bit] NULL,
	[cMontoPagoMinimo] [money] NULL,
	[cMontoVencidoPagar] [money] NULL,
	[sTipoReg] [char](1) NULL,
	[bError] [bit] NULL,
	[bDebitoAutomatico] [bit] NULL,
	[sTipoPagoDebitoAutomatico] [nvarchar](20) NULL,
	[dFechaVencimientoPlastico] [datetime] NULL,
	[sTipoTDC] [nvarchar](100) NULL,
	[sCodigoEjecutivoCreacionEmision] [nvarchar](5) NULL,
	[sCodigoAgenciaCreacionEmision] [nvarchar](5) NULL,
	[sNombreTarjetahabiente] [nvarchar](25) NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BDI_CRM_ProdActivosTDC_ACT]    Script Date: 08/27/2013 11:35:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BDI_CRM_ProdActivosTDC_ACT](
	[iCodigoClienteBanco] [int] NULL,
	[sCodigoMoneda] [nvarchar](4) NULL,
	[sCodigoModulo] [nvarchar](4) NULL,
	[sCodigoProducto] [nvarchar](6) NULL,
	[sCodigoTipo] [nvarchar](8) NULL,
	[sCodigoProductoFISA] [nvarchar](18) NULL,
	[sCodigoUnicoProducto] [nvarchar](13) NULL,
	[sNroTarjeta] [nvarchar](20) NULL,
	[sNroCuenta] [numeric](10, 0) NULL,
	[sEstadoTarjeta] [nvarchar](50) NULL,
	[sEstadoCuenta] [nvarchar](20) NULL,
	[iCantidadDIvidendosVencidos] [smallint] NULL,
	[dFechaVencimientoOperacion] [datetime] NULL,
	[cDeudaCapital] [money] NULL,
	[cLimiteCompra] [money] NULL,
	[cMontoUtilizado] [money] NULL,
	[cGastosGenerados] [money] NULL,
	[cLimiteFinanciamiento] [money] NULL,
	[bRealizaCOmprasExterior] [bit] NULL,
	[bTieneTarjetasAdicionales] [bit] NULL,
	[cMontoPagoMinimo] [money] NULL,
	[cMontoVencidoPagar] [money] NULL,
	[bDebitoAutomatico] [bit] NULL,
	[sTipoPagoDebitoAutomatico] [nvarchar](20) NULL,
	[dFechaVencimientoPlastico] [datetime] NULL,
	[sTipoTDC] [nvarchar](100) NULL,
	[sCodigoEjecutivoCreacionEmision] [nvarchar](5) NULL,
	[sCodigoAgenciaCreacionEmision] [nvarchar](5) NULL,
	[sNombreTarjetahabiente] [nvarchar](25) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BDI_CRM_ProdActivosLineasCredito_DIF]    Script Date: 08/27/2013 11:35:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BDI_CRM_ProdActivosLineasCredito_DIF](
	[iCodigoClienteBanco] [int] NULL,
	[sCodigoMoneda] [nvarchar](4) NULL,
	[sCodigoModulo] [nvarchar](4) NULL,
	[sCodigoProducto] [nvarchar](6) NULL,
	[sCodigoTipo] [nvarchar](8) NULL,
	[sCodigoProductoFisa] [nvarchar](18) NULL,
	[sCodigoUnicoProducto] [nvarchar](13) NULL,
	[sNroOperacion] [nvarchar](15) NULL,
	[dFechaAperturaOperacion] [datetime] NULL,
	[dFechaVencimientoOperacion] [datetime] NULL,
	[cMontoAprobado] [money] NULL,
	[cMontoUtilizado] [money] NULL,
	[cMontoDisponible] [money] NULL,
	[fPorcUso] [decimal](10, 2) NULL,
	[sSubLimiteLineaCredito] [nvarchar](40) NULL,
	[sCodigoEjecutivoCreacionEmision] [nvarchar](5) NULL,
	[sCodigoAgenciaCreacionEmision] [nvarchar](5) NULL,
	[sTipoReg] [char](1) NULL,
	[bError] [bit] NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BDI_CRM_ProdActivosLineasCredito_ACT]    Script Date: 08/27/2013 11:35:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BDI_CRM_ProdActivosLineasCredito_ACT](
	[iCodigoClienteBanco] [int] NULL,
	[sCodigoMoneda] [nvarchar](4) NULL,
	[sCodigoModulo] [nvarchar](4) NULL,
	[sCodigoProducto] [nvarchar](6) NULL,
	[sCodigoTipo] [nvarchar](8) NULL,
	[sCodigoProductoFisa] [nvarchar](18) NULL,
	[sCodigoUnicoProducto] [nvarchar](13) NULL,
	[sNroOperacion] [nvarchar](15) NULL,
	[dFechaAperturaOperacion] [datetime] NULL,
	[dFechaVencimientoOperacion] [datetime] NULL,
	[cMontoAprobado] [money] NULL,
	[cMontoUtilizado] [money] NULL,
	[cMontoDisponible] [money] NULL,
	[fPorcUso] [decimal](10, 2) NULL,
	[sSubLimiteLineaCredito] [nvarchar](40) NULL,
	[sCodigoEjecutivoCreacionEmision] [nvarchar](5) NULL,
	[sCodigoAgenciaCreacionEmision] [nvarchar](5) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BDI_CRM_ProdActivosGarantias_DIF]    Script Date: 08/27/2013 11:35:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BDI_CRM_ProdActivosGarantias_DIF](
	[iCodigoClienteBanco] [int] NULL,
	[sCodigoMoneda] [nvarchar](4) NULL,
	[sCodigoModulo] [nvarchar](4) NULL,
	[sCodigoProducto] [nvarchar](6) NULL,
	[sCodigoTipo] [nvarchar](8) NULL,
	[sCodigoProductoFISA] [nvarchar](18) NULL,
	[sCodigoUnicoProducto] [nvarchar](13) NULL,
	[nNroCuenta] [nvarchar](10) NULL,
	[sEstadoOperacion] [nvarchar](25) NULL,
	[cValorLiquidacion] [money] NULL,
	[cValorComercial] [money] NULL,
	[sDescripcionGarantia] [nvarchar](500) NULL,
	[sOperacionesGarantizadas] [nvarchar](4000) NULL,
	[sNumeroPoliza] [nvarchar](20) NULL,
	[dFechaVencimientoPoliza] [datetime] NULL,
	[sCompaniaPoliza] [nvarchar](100) NULL,
	[sTipoReg] [char](1) NULL,
	[bError] [bit] NULL,
	[sCodigoLineaCredito] [nvarchar](10) NULL,
	[cSaldoDisponible] [money] NULL,
	[dFechaUltimoAvaluo] [datetime] NULL,
	[sCodigoEjecutivoCreacionEmision] [nvarchar](5) NULL,
	[sCodigoAgenciaCreacionEmision] [nvarchar](5) NULL,
	[iCodigoClienteGarante] [int] NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BDI_CRM_ProdActivosGarantias_ACT]    Script Date: 08/27/2013 11:35:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BDI_CRM_ProdActivosGarantias_ACT](
	[iCodigoClienteBanco] [int] NULL,
	[sCodigoMoneda] [nvarchar](4) NULL,
	[sCodigoModulo] [nvarchar](4) NULL,
	[sCodigoProducto] [nvarchar](6) NULL,
	[sCodigoTipo] [nvarchar](8) NULL,
	[sCodigoProductoFISA] [nvarchar](18) NULL,
	[sCodigoUnicoProducto] [nvarchar](13) NULL,
	[nNroCuenta] [nvarchar](10) NULL,
	[sEstadoOperacion] [nvarchar](25) NULL,
	[cValorLiquidacion] [money] NULL,
	[cValorComercial] [money] NULL,
	[sDescripcionGarantia] [nvarchar](500) NULL,
	[sOperacionesGarantizadas] [nvarchar](4000) NULL,
	[sNumeroPoliza] [nvarchar](20) NULL,
	[dFechaVencimientoPoliza] [datetime] NULL,
	[sCompaniaPoliza] [nvarchar](100) NULL,
	[sCodigoLineaCredito] [nvarchar](10) NULL,
	[cSaldoDisponible] [money] NULL,
	[dFechaUltimoAvaluo] [datetime] NULL,
	[sCodigoEjecutivoCreacionEmision] [nvarchar](5) NULL,
	[sCodigoAgenciaCreacionEmision] [nvarchar](5) NULL,
	[iCodigoClienteGarante] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BDI_CRM_ProdActivosCreditos_DIF]    Script Date: 08/27/2013 11:35:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BDI_CRM_ProdActivosCreditos_DIF](
	[iCodigoClienteBanco] [int] NULL,
	[sCodigoMoneda] [nvarchar](4) NULL,
	[sCodigoModulo] [nvarchar](4) NULL,
	[sCodigoProducto] [nvarchar](6) NULL,
	[sCodigoTipo] [nvarchar](8) NULL,
	[sCodigoProductoFisa] [nvarchar](18) NULL,
	[sCodigoUnicoProducto] [nvarchar](13) NULL,
	[sNroCuenta] [numeric](10, 0) NULL,
	[sEstadoOperacion] [nvarchar](20) NULL,
	[iCantidadDIvidendosVencidos] [smallint] NULL,
	[cMontoDeLaOperacionCapital] [money] NULL,
	[cMontoPagadoCapital] [money] NULL,
	[cMontoVencidoCapital] [money] NULL,
	[dFechaVencimientoOperacion] [datetime] NULL,
	[dFechaInicioOperacionEmision] [datetime] NULL,
	[cSaldoOperacion] [money] NULL,
	[iPlazoDias] [int] NULL,
	[fTasaActual] [float] NULL,
	[fTasaTRE] [float] NULL,
	[dFechaPagoDividendoVigente] [datetime] NULL,
	[cMontoPagoDividendoVigente] [money] NULL,
	[bDebitoAutomatico] [bit] NULL,
	[cMontoVencidoPagar] [money] NULL,
	[cValorComercialVivienda] [money] NULL,
	[cValorComercialVehiculos] [money] NULL,
	[sTipoReg] [char](1) NULL,
	[bError] [bit] NULL,
	[sCodigoLineaCredito] [nvarchar](10) NULL,
	[bReprogramado] [bit] NULL,
	[iNumeroCuotas] [int] NULL,
	[sCodigoEjecutivoCreacionEmision] [nvarchar](5) NULL,
	[sCodigoAgenciaCreacionEmision] [nvarchar](5) NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BDI_CRM_ProdActivosCreditos_ACT]    Script Date: 08/27/2013 11:35:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BDI_CRM_ProdActivosCreditos_ACT](
	[iCodigoClienteBanco] [int] NULL,
	[sCodigoMoneda] [nvarchar](4) NULL,
	[sCodigoModulo] [nvarchar](4) NULL,
	[sCodigoProducto] [nvarchar](6) NULL,
	[sCodigoTipo] [nvarchar](8) NULL,
	[sCodigoProductoFisa] [nvarchar](18) NULL,
	[sCodigoUnicoProducto] [nvarchar](13) NULL,
	[sNroCuenta] [numeric](10, 0) NULL,
	[sEstadoOperacion] [nvarchar](20) NULL,
	[iCantidadDIvidendosVencidos] [smallint] NULL,
	[cMontoDeLaOperacionCapital] [money] NULL,
	[cMontoPagadoCapital] [money] NULL,
	[cMontoVencidoCapital] [money] NULL,
	[dFechaVencimientoOperacion] [datetime] NULL,
	[dFechaInicioOperacionEmision] [datetime] NULL,
	[cSaldoOperacion] [money] NULL,
	[iPlazoDias] [int] NULL,
	[fTasaActual] [float] NULL,
	[fTasaTRE] [float] NULL,
	[dFechaPagoDividendoVigente] [datetime] NULL,
	[cMontoPagoDividendoVigente] [money] NULL,
	[bDebitoAutomatico] [bit] NULL,
	[cMontoVencidoPagar] [money] NULL,
	[cValorComercialVivienda] [money] NULL,
	[cValorComercialVehiculos] [money] NULL,
	[sCodigoLineaCredito] [nvarchar](10) NULL,
	[bReprogramado] [bit] NULL,
	[iNumeroCuotas] [int] NULL,
	[sCodigoEjecutivoCreacionEmision] [nvarchar](5) NULL,
	[sCodigoAgenciaCreacionEmision] [nvarchar](5) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BDI_CRM_ProdActivosComex_DIF]    Script Date: 08/27/2013 11:35:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BDI_CRM_ProdActivosComex_DIF](
	[iCodigoClienteBanco] [int] NULL,
	[sCodigoMoneda] [nvarchar](4) NULL,
	[sCodigoModulo] [nvarchar](4) NULL,
	[sCodigoProducto] [nvarchar](6) NULL,
	[sCodigoTipo] [nvarchar](8) NULL,
	[sCodigoProductoFISA] [nvarchar](18) NULL,
	[sCodigoUnicoProducto] [nvarchar](13) NULL,
	[nNroCuenta] [numeric](10, 0) NULL,
	[sEstadoOperacion] [nvarchar](20) NULL,
	[cMontoDeLaOperacionCapital] [money] NULL,
	[dFechaVencimientoOperacion] [datetime] NULL,
	[dFechaInicioOperacionEmision] [datetime] NULL,
	[cSaldoOperacion] [money] NULL,
	[iPlazo] [int] NULL,
	[sBeneficiario] [nvarchar](80) NULL,
	[sTipoReg] [char](1) NULL,
	[bError] [bit] NULL,
	[sCodigoLineaCredito] [nvarchar](10) NULL,
	[cMontoUtilizacionCartaCredito] [money] NULL,
	[bPermiteEmbarquesParciales] [bit] NULL,
	[fToleranciaMaxima] [decimal](10, 2) NULL,
	[fToleranciaMinima] [decimal](10, 2) NULL,
	[sTipoGarantia] [nvarchar](40) NULL,
	[sOrdenante] [nvarchar](60) NULL,
	[sCodigoEjecutivoCreacionEmision] [nvarchar](5) NULL,
	[sCodigoAgenciaCreacionEmision] [nvarchar](5) NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BDI_CRM_ProdActivosComex_ACT]    Script Date: 08/27/2013 11:35:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BDI_CRM_ProdActivosComex_ACT](
	[iCodigoClienteBanco] [int] NULL,
	[sCodigoMoneda] [nvarchar](4) NULL,
	[sCodigoModulo] [nvarchar](4) NULL,
	[sCodigoProducto] [nvarchar](6) NULL,
	[sCodigoTipo] [nvarchar](8) NULL,
	[sCodigoProductoFISA] [nvarchar](18) NULL,
	[sCodigoUnicoProducto] [nvarchar](13) NULL,
	[nNroCuenta] [numeric](10, 0) NULL,
	[sEstadoOperacion] [nvarchar](20) NULL,
	[cMontoDeLaOperacionCapital] [money] NULL,
	[dFechaVencimientoOperacion] [datetime] NULL,
	[dFechaInicioOperacionEmision] [datetime] NULL,
	[cSaldoOperacion] [money] NULL,
	[iPlazo] [int] NULL,
	[sBeneficiario] [nvarchar](80) NULL,
	[sCodigoLineaCredito] [nvarchar](10) NULL,
	[cMontoUtilizacionCartaCredito] [money] NULL,
	[bPermiteEmbarquesParciales] [bit] NULL,
	[fToleranciaMaxima] [decimal](10, 2) NULL,
	[fToleranciaMinima] [decimal](10, 2) NULL,
	[sTipoGarantia] [nvarchar](40) NULL,
	[sOrdenante] [nvarchar](60) NULL,
	[sCodigoEjecutivoCreacionEmision] [nvarchar](5) NULL,
	[sCodigoAgenciaCreacionEmision] [nvarchar](5) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BDI_CRM_ProcesosCargaCRM]    Script Date: 08/27/2013 11:35:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BDI_CRM_ProcesosCargaCRM](
	[iCodigoProceso] [nchar](10) NOT NULL,
	[sNombreProceso] [nchar](50) NOT NULL,
	[bDesactivado] [bit] NOT NULL,
	[sEstado] [nchar](20) NOT NULL,
	[iPrioridad] [tinyint] NULL,
	[sDependencia] [nchar](100) NULL,
	[bSubproceso] [bit] NULL,
	[sDependenciaEjecucion] [nchar](100) NULL,
 CONSTRAINT [PK_BDI_ProcesosCargaCRM] PRIMARY KEY CLUSTERED 
(
	[iCodigoProceso] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BDI_CRM_Polizas_DIF]    Script Date: 08/27/2013 11:35:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BDI_CRM_Polizas_DIF](
	[iCodigoClienteBanco] [int] NULL,
	[sNumeroPoliza] [nvarchar](16) NULL,
	[sCompaniaAseguradora] [nvarchar](100) NULL,
	[dFechaVencimiento] [datetime] NULL,
	[sTipoReg] [char](1) NULL,
	[bError] [bit] NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BDI_CRM_Polizas_ACT]    Script Date: 08/27/2013 11:35:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BDI_CRM_Polizas_ACT](
	[iCodigoClienteBanco] [int] NULL,
	[sNumeroPoliza] [nvarchar](16) NULL,
	[sCompaniaAseguradora] [nvarchar](100) NULL,
	[dFechaVencimiento] [datetime] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BDI_CRM_MapeoPicklistValores]    Script Date: 08/27/2013 11:35:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BDI_CRM_MapeoPicklistValores](
	[iCodigoMapeo] [int] NULL,
	[iCodigoValorCrm] [int] NULL,
	[sNombreValorCrm] [nvarchar](200) NULL,
	[sCodigoValorBdi] [nvarchar](8) NULL,
	[sNombreValorBdi] [nvarchar](200) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BDI_CRM_MapeoPicklistCampos]    Script Date: 08/27/2013 11:35:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BDI_CRM_MapeoPicklistCampos](
	[iCodigoMapeo] [int] IDENTITY(1,1) NOT NULL,
	[sNombreEsquemaCampoCrm] [nvarchar](100) NULL,
	[sNombreEsquemaEntidadCrm] [nvarchar](100) NULL,
	[sNombreCampoCodigoBdi] [nvarchar](100) NULL,
	[sNombreCampoValorBdi] [nvarchar](100) NULL,
	[sNombreTablaBdi] [nvarchar](100) NULL
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[BDI_CRM_MapeoPicklist]    Script Date: 08/27/2013 11:35:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create view [dbo].[BDI_CRM_MapeoPicklist]
as
select
campos.iCodigoMapeo,
campos.sNombreEsquemaCampoCrm,
valores.sCodigoValorBdi,
valores.sNombreValorBdi,
valores.sNombreValorCrm,
valores.iCodigoValorCrm
from BDI_CRM_MapeoPicklistValores valores inner join BDI_CRM_MapeoPicklistCampos campos
on valores.iCodigoMapeo=campos.iCodigoMapeo
GO
/****** Object:  StoredProcedure [dbo].[sp_porc_cuotas_ingreso_actual_consumo]    Script Date: 08/27/2013 11:35:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO