use BDI_CRM 
go


-- ***********************************************
-- Add nuevo campo en producto del activo
-- ***********************************************

ALTER TABLE [dbo].[BDI_ProdActivosComex]                  add iDiasMora int
ALTER TABLE [dbo].[BDI_ProdActivosCreditos]               add iDiasMora int
ALTER TABLE [dbo].[BDI_ProdActivosGarantias]              add iDiasMora int
ALTER TABLE [dbo].[BDI_ProdActivosLineasCredito]          add iDiasMora int
ALTER TABLE [dbo].[BDI_ProdActivosTDC]	                  add iDiasMora int
ALTER TABLE [dbo].[BDI_CRM_ProdActivosComex_ACT]          add iDiasMora int
ALTER TABLE [dbo].[BDI_CRM_ProdActivosComex_DIF]          add iDiasMora int
ALTER TABLE [dbo].[BDI_CRM_ProdActivosCreditos_ACT]       add iDiasMora int
ALTER TABLE [dbo].[BDI_CRM_ProdActivosCreditos_DIF]       add iDiasMora int
ALTER TABLE [dbo].[BDI_CRM_ProdActivosGarantias_ACT]      add iDiasMora int
ALTER TABLE [dbo].[BDI_CRM_ProdActivosGarantias_DIF]      add iDiasMora int
ALTER TABLE [dbo].[BDI_CRM_ProdActivosLineasCredito_ACT]  add iDiasMora int
ALTER TABLE [dbo].[BDI_CRM_ProdActivosLineasCredito_DIF]  add iDiasMora int
ALTER TABLE [dbo].[BDI_CRM_ProdActivosTDC_ACT]            add iDiasMora int
ALTER TABLE [dbo].[BDI_CRM_ProdActivosTDC_DIF]            add iDiasMora int
go

-- ***********************************************
-- Add nuevo campo en cliente
-- ***********************************************

ALTER TABLE [dbo].[BDI_InfoAdicionalClientes]        add iCantidadServicio int
ALTER TABLE [dbo].[BDI_CRM_InfoAdicionalClientes_ACT]    add iCantidadServicio int
ALTER TABLE [dbo].[BDI_CRM_InfoAdicionalClientes_DIF]    add iCantidadServicio int
go

ALTER TABLE [dbo].[BDI_InfoAdicionalClientes]        add cVolumenTotalNegocioActualActivos money
ALTER TABLE [dbo].[BDI_CRM_InfoAdicionalClientes_ACT]    add cVolumenTotalNegocioActualActivos money
ALTER TABLE [dbo].[BDI_CRM_InfoAdicionalClientes_DIF]    add cVolumenTotalNegocioActualActivos money
go

ALTER TABLE [dbo].[BDI_InfoAdicionalClientes]        add cVolumenTotalNegocioActualContingentes money
ALTER TABLE [dbo].[BDI_CRM_InfoAdicionalClientes_ACT]    add cVolumenTotalNegocioActualContingentes money
ALTER TABLE [dbo].[BDI_CRM_InfoAdicionalClientes_DIF]    add cVolumenTotalNegocioActualContingentes money
go

ALTER TABLE [dbo].[BDI_InfoAdicionalClientes]        add cVolumenTotalNegocioActualpasivos money
ALTER TABLE [dbo].[BDI_CRM_InfoAdicionalClientes_ACT]    add cVolumenTotalNegocioActualpasivos money
ALTER TABLE [dbo].[BDI_CRM_InfoAdicionalClientes_DIF]    add cVolumenTotalNegocioActualpasivos money
go

ALTER TABLE [dbo].[BDI_InfoAdicionalClientes]        add dIndiceReciprocidad decimal
ALTER TABLE [dbo].[BDI_CRM_InfoAdicionalClientes_ACT]    add dIndiceReciprocidad decimal
ALTER TABLE [dbo].[BDI_CRM_InfoAdicionalClientes_DIF]    add dIndiceReciprocidad decimal
go

ALTER TABLE [dbo].[BDI_InfoAdicionalClientes]        add sCaedecCod nvarchar(10)
ALTER TABLE [dbo].[BDI_CRM_InfoAdicionalClientes_ACT]    add sCaedecCod nvarchar(10)
ALTER TABLE [dbo].[BDI_CRM_InfoAdicionalClientes_DIF]    add sCaedecCod nvarchar(10)
go

ALTER TABLE [dbo].[BDI_InfoAdicionalClientes]        add sCaedecDesc nvarchar(100)
ALTER TABLE [dbo].[BDI_CRM_InfoAdicionalClientes_ACT]    add sCaedecDesc nvarchar(100)
ALTER TABLE [dbo].[BDI_CRM_InfoAdicionalClientes_DIF]    add sCaedecDesc nvarchar(100)
go

ALTER TABLE [dbo].[BDI_InfoAdicionalClientes]        add iIVC int
ALTER TABLE [dbo].[BDI_CRM_InfoAdicionalClientes_ACT]    add iIVC int
ALTER TABLE [dbo].[BDI_CRM_InfoAdicionalClientes_DIF]    add iIVC int
go

ALTER TABLE BDI_InfoAdicionalClientes ADD cTotalActivos MONEY NULL;
ALTER TABLE BDI_InfoAdicionalClientes ADD cTotalPasivos MONEY NULL;
go

ALTER TABLE BDI_CRM_InfoAdicionalClientes_ACT ADD cTotalActivos MONEY NULL;
ALTER TABLE BDI_CRM_InfoAdicionalClientes_ACT ADD cTotalPasivos MONEY NULL;
go

ALTER TABLE BDI_CRM_InfoAdicionalClientes_DIF ADD cTotalActivos MONEY NULL;
ALTER TABLE BDI_CRM_InfoAdicionalClientes_DIF ADD cTotalPasivos MONEY NULL;
go

-- ***********************************************
-- Add nuevo campo en producto pasivos
-- ***********************************************

ALTER TABLE [dbo].[BDI_ProdPasivos]         add sFirmaAutorizada nvarchar(120)
ALTER TABLE [dbo].[BDI_CRM_ProdPasivos_ACT] add sFirmaAutorizada nvarchar(120)
ALTER TABLE [dbo].[BDI_CRM_ProdPasivos_DIF] add sFirmaAutorizada nvarchar(120)
go

ALTER TABLE [dbo].[BDI_ProdPasivos]         add sTipoCuentaCorriente nvarchar(120)
ALTER TABLE [dbo].[BDI_CRM_ProdPasivos_ACT] add sTipoCuentaCorriente nvarchar(120)
ALTER TABLE [dbo].[BDI_CRM_ProdPasivos_DIF] add sTipoCuentaCorriente nvarchar(120)
go

ALTER TABLE [dbo].[BDI_ProdPasivos]         add sCodigoTipoCuentaCorriente nvarchar(50)
ALTER TABLE [dbo].[BDI_CRM_ProdPasivos_ACT] add sCodigoTipoCuentaCorriente nvarchar(50)
ALTER TABLE [dbo].[BDI_CRM_ProdPasivos_DIF] add sCodigoTipoCuentaCorriente nvarchar(50)

go

ALTER TABLE BDI_ProdPasivos ADD fPromedioDepositos6M DECIMAL NULL;
ALTER TABLE BDI_ProdPasivos ADD fPromedioRetiros6M   DECIMAL NULL;
ALTER TABLE BDI_ProdPasivos ADD bDebitosAutomaticos  BIT     NULL;
go

ALTER TABLE BDI_CRM_ProdPasivos_ACT ADD fPromedioDepositos6M DECIMAL NULL;
ALTER TABLE BDI_CRM_ProdPasivos_ACT ADD fPromedioRetiros6M   DECIMAL NULL;
ALTER TABLE BDI_CRM_ProdPasivos_ACT ADD bDebitosAutomaticos  BIT     NULL;
go

ALTER TABLE BDI_CRM_ProdPasivos_DIF ADD fPromedioDepositos6M DECIMAL NULL;
ALTER TABLE BDI_CRM_ProdPasivos_DIF ADD fPromedioRetiros6M   DECIMAL NULL;
ALTER TABLE BDI_CRM_ProdPasivos_DIF ADD bDebitosAutomaticos  BIT     NULL;
go

ALTER TABLE BDI_ProdPasivos ADD sEstadoCuenta    NVARCHAR(100) NULL;
ALTER TABLE BDI_ProdPasivos ADD bSobregiroActivo BIT           NULL;
ALTER TABLE BDI_ProdPasivos ADD cMontoSobregiro  MONEY         NULL;
go

ALTER TABLE BDI_CRM_ProdPasivos_ACT ADD sEstadoCuenta    NVARCHAR(100) NULL;
ALTER TABLE BDI_CRM_ProdPasivos_ACT ADD bSobregiroActivo BIT           NULL;
ALTER TABLE BDI_CRM_ProdPasivos_ACT ADD cMontoSobregiro  MONEY         NULL;
go

ALTER TABLE BDI_CRM_ProdPasivos_DIF ADD sEstadoCuenta    NVARCHAR(100) NULL;
ALTER TABLE BDI_CRM_ProdPasivos_DIF ADD bSobregiroActivo BIT           NULL;
ALTER TABLE BDI_CRM_ProdPasivos_DIF ADD cMontoSobregiro  MONEY         NULL;
go

-- ***********************************************
-- Add nuevo campo en garantías
-- ***********************************************
ALTER TABLE BDI_ProdActivosGarantias ADD cValorPrestamos MONEY NULL;
ALTER TABLE BDI_ProdActivosGarantias ADD bPolizasVencidas BIT NULL;
ALTER TABLE BDI_ProdActivosGarantias ADD dFechaLegalizacionGarantia DATETIME NULL; 
ALTER TABLE BDI_ProdActivosGarantias ADD cMontoNoUtilizado MONEY    NULL;
go

ALTER TABLE BDI_CRM_ProdActivosGarantias_ACT ADD cValorPrestamos  MONEY    NULL;
ALTER TABLE BDI_CRM_ProdActivosGarantias_ACT ADD bPolizasVencidas BIT      NULL;
ALTER TABLE BDI_CRM_ProdActivosGarantias_ACT ADD dFechaLegalizacionGarantia DATETIME NULL;
ALTER TABLE BDI_CRM_ProdActivosGarantias_ACT ADD cMontoNoUtilizado MONEY   NULL;
go

ALTER TABLE BDI_CRM_ProdActivosGarantias_DIF ADD cValorPrestamos  MONEY  NULL;
ALTER TABLE BDI_CRM_ProdActivosGarantias_DIF ADD bPolizasVencidas BIT    NULL;
ALTER TABLE BDI_CRM_ProdActivosGarantias_DIF ADD dFechaLegalizacionGarantia DATETIME NULL;
ALTER TABLE BDI_CRM_ProdActivosGarantias_DIF ADD cMontoNoUtilizado          MONEY    NULL;
go

--*****************************--
--** GARANTIAS SEGUNDA PARTE **--
--*****************************--

ALTER TABLE BDI_ProdActivosGarantias ADD bLegalizacion BIT NULL;
ALTER TABLE BDI_ProdActivosGarantias ADD sNumeroFolio  NVARCHAR(100) NULL;
ALTER TABLE BDI_ProdActivosGarantias ADD dFechaAvaluo  DATETIME NULL;
ALTER TABLE BDI_ProdActivosGarantias ADD sAvaluador    NVARCHAR(120) NULL;
ALTER TABLE BDI_ProdActivosGarantias ADD sNumeroProtocolo  NVARCHAR(100) NULL;
ALTER TABLE BDI_ProdActivosGarantias ADD sNotaria	  NVARCHAR(100) NULL;

ALTER TABLE BDI_CRM_ProdActivosGarantias_ACT ADD bLegalizacion BIT NULL;
ALTER TABLE BDI_CRM_ProdActivosGarantias_ACT ADD sNumeroFolio  NVARCHAR(100) NULL;
ALTER TABLE BDI_CRM_ProdActivosGarantias_ACT ADD dFechaAvaluo  DATETIME NULL;
ALTER TABLE BDI_CRM_ProdActivosGarantias_ACT ADD sAvaluador    NVARCHAR(120) NULL;
ALTER TABLE BDI_CRM_ProdActivosGarantias_ACT ADD sNumeroProtocolo  NVARCHAR(100) NULL;
ALTER TABLE BDI_CRM_ProdActivosGarantias_ACT ADD sNotaria	  NVARCHAR(100) NULL;

ALTER TABLE BDI_CRM_ProdActivosGarantias_DIF ADD bLegalizacion BIT NULL;
ALTER TABLE BDI_CRM_ProdActivosGarantias_DIF ADD sNumeroFolio  NVARCHAR(100) NULL;
ALTER TABLE BDI_CRM_ProdActivosGarantias_DIF ADD dFechaAvaluo  DATETIME NULL;
ALTER TABLE BDI_CRM_ProdActivosGarantias_DIF ADD sAvaluador    NVARCHAR(120) NULL;
ALTER TABLE BDI_CRM_ProdActivosGarantias_DIF ADD sNumeroProtocolo  NVARCHAR(100) NULL;
ALTER TABLE BDI_CRM_ProdActivosGarantias_DIF ADD sNotaria	  NVARCHAR(100) NULL;


-- ***********************************************
-- Add nuevo campo en Servicios
-- ***********************************************
ALTER TABLE [dbo].[BDI_Servicios]                  add sTipoDebitoAutomatico nvarchar(120)
ALTER TABLE [dbo].[BDI_CRM_Servicios_ACT]          add sTipoDebitoAutomatico nvarchar(120)
ALTER TABLE [dbo].[BDI_CRM_Servicios_DIF]          add sTipoDebitoAutomatico nvarchar(120)

ALTER TABLE [dbo].[BDI_Servicios]                  add sCodigoTipoDebitoAutomatico nvarchar(50)
ALTER TABLE [dbo].[BDI_CRM_Servicios_ACT]          add sCodigoTipoDebitoAutomatico nvarchar(50)
ALTER TABLE [dbo].[BDI_CRM_Servicios_DIF]          add sCodigoTipoDebitoAutomatico nvarchar(50)

go

-- **************************************************************
-- Add nuevo mapeo  para el picklist de tipo debito automatico
-- **************************************************************
insert into BDI_CRM_MapeoPicklistCampos 
(
sNombreEsquemaCampoCrm             ,sNombreEsquemaEntidadCrm ,sNombreCampoCodigoBdi         ,sNombreCampoValorBdi    ,sNombreTablaBdi)
values('efk_tipo_debito_automatico',null                     ,'sCodigoTipoDebitoAutomatico' ,'sTipoDebitoAutomatico' ,'BDI_Servicios')
go

insert into BDI_CRM_MapeoPicklistCampos 
(
sNombreEsquemaCampoCrm             ,sNombreEsquemaEntidadCrm ,sNombreCampoCodigoBdi         ,sNombreCampoValorBdi    ,sNombreTablaBdi)
values('efk_tipo_cuenta_corriente',null                     ,'sCodigoTipoCuentaCorriente'   ,'sTipoCuentaCorriente'  ,'BDI_ProdPasivos')
go

-- **************************************************************
-- Add nuevo mapeo  firma autoriza en representante legal
-- **************************************************************
ALTER TABLE [dbo].[BDI_RepresentantesLegales]         add bFirmaAutorizada bit
ALTER TABLE [dbo].[BDI_CRM_RepresentantesLegales_ACT] add bFirmaAutorizada bit
ALTER TABLE [dbo].[BDI_CRM_RepresentantesLegales_DIF] add bFirmaAutorizada bit
go

ALTER TABLE [dbo].[BDI_RepresentantesLegales]         add sTelefono nvarchar(100)
ALTER TABLE [dbo].[BDI_CRM_RepresentantesLegales_ACT] add sTelefono nvarchar(100)
ALTER TABLE [dbo].[BDI_CRM_RepresentantesLegales_DIF] add sTelefono nvarchar(100)
go

-- **************************************************************
-- Add nuevo mapeo  firma autoriza en representante legal
-- **************************************************************
ALTER TABLE [dbo].[BDI_CentralRiesgo]         add mRiesgoCartera money
ALTER TABLE [dbo].[BDI_CentralRiesgo]         add mRiesgoContingente money

ALTER TABLE [dbo].[BDI_CRM_CentralRiesgo_ACT] add mRiesgoCartera money
ALTER TABLE [dbo].[BDI_CRM_CentralRiesgo_ACT] add mRiesgoContingente money

ALTER TABLE [dbo].[BDI_CRM_CentralRiesgo_DIF] add mRiesgoCartera money
ALTER TABLE [dbo].[BDI_CRM_CentralRiesgo_DIF] add mRiesgoContingente money

go





-- **************************************************
-- ***************   JORGE SALAZAR    ***************

--creacion de campos Tiene TC, Tiene BxI, Tiene Movil
ALTER TABLE dbo.BDI_InfoAdicionalClientes 
		ADD btiene_tc    bit NULL, 
			btiene_bxi   bit NULL,
			btiene_movil bit NULL; 
--creacion de campos zona y region
ALTER TABLE dbo.BDI_Ejecutivos
		ADD sCodigoZona         nvarchar(4)    NULL, 
			sDescripcionZona    nvarchar (100) NULL,
			sCodigoRegion       nvarchar(4)    NULL, 
			sDescripcionRegion  nvarchar (100) NULL;

--insert campos dbo.BDI_CRM_MapeoPicklistCampos
		
insert into dbo.BDI_CRM_MapeoPicklistCampos(sNombreEsquemaCampoCrm,sNombreEsquemaEntidadCrm,sNombreCampoCodigoBdi,sNombreCampoValorBdi,	sNombreTablaBdi) 
values ('efk_zona',NULL,'sCodigoZona','sDescripcionZona','BDI_Ejecutivos');

insert into dbo.BDI_CRM_MapeoPicklistCampos(sNombreEsquemaCampoCrm,sNombreEsquemaEntidadCrm,sNombreCampoCodigoBdi,sNombreCampoValorBdi,	sNombreTablaBdi) 
values ('efk_region',NULL,'sCodigoRegion','sDescripcionRegion','BDI_Ejecutivos');



-- ***********************************************
-- Add nuevo campo en Clientes  fase II Prioridad 0
-- ***********************************************
-- ***********************************************
-- Add nuevo campo en BDI_InfoAdicionalClientes
-- ***********************************************

ALTER TABLE [dbo].[BDI_InfoAdicionalClientes] add fLNIngresoMensual  float;
ALTER TABLE [dbo].[BDI_InfoAdicionalClientes] add fLNSaldoCaptaciones  float;
ALTER TABLE [dbo].[BDI_InfoAdicionalClientes] add fLNSaldoCartera  float;
ALTER TABLE [dbo].[BDI_InfoAdicionalClientes] add fLNSaldoCarteraTC float;
ALTER TABLE [dbo].[BDI_InfoAdicionalClientes] add fLNSaldoCarteraVivienda float;
ALTER TABLE [dbo].[BDI_InfoAdicionalClientes] add fLNSaldoPromedioMensualCaptaciones float;
ALTER TABLE [dbo].[BDI_InfoAdicionalClientes] add fLNVariacionSaldoPromedioMensualCaptaciones float;


ALTER TABLE [dbo].[BDI_InfoAdicionalClientes] add fEdadCliente float;
ALTER TABLE [dbo].[BDI_InfoAdicionalClientes] add fVecesVigente2TC  float;
ALTER TABLE [dbo].[BDI_InfoAdicionalClientes] add fNroAdicionales  float;
ALTER TABLE [dbo].[BDI_InfoAdicionalClientes] add fNroReposiciones  float;
ALTER TABLE [dbo].[BDI_InfoAdicionalClientes] add fNroIniciales  float;
ALTER TABLE [dbo].[BDI_InfoAdicionalClientes] add fNroSinSolicitud  float;
ALTER TABLE [dbo].[BDI_InfoAdicionalClientes] add fTasaCredito  float;

ALTER TABLE [dbo].[BDI_InfoAdicionalClientes] add bTieneAlarmaProbable  bit;
ALTER TABLE [dbo].[BDI_InfoAdicionalClientes] add dFechaAlarmaProbable  datetime;
ALTER TABLE [dbo].[BDI_InfoAdicionalClientes] add fRatioAlarma  decimal(10,2);

ALTER TABLE [dbo].[BDI_InfoAdicionalClientes] add sRangoSaldoInicial  nvarchar(1);
ALTER TABLE [dbo].[BDI_InfoAdicionalClientes] add cSumatoriaCuotasMaximas  money;
ALTER TABLE [dbo].[BDI_InfoAdicionalClientes] add cSumatoriaCuotaVigente  money;

--COB
ALTER TABLE [dbo].[BDI_InfoAdicionalClientes] add sCodigoMoneda  nvarchar(4);


-- ***********************************************
-- Add nuevo campo en BDI_CRM_InfoAdicionalClientes_ACT
-- ***********************************************
ALTER TABLE [dbo].[BDI_CRM_InfoAdicionalClientes_ACT] add fLNIngresoMensual  float;
ALTER TABLE [dbo].[BDI_CRM_InfoAdicionalClientes_ACT] add fLNSaldoCaptaciones  float;
ALTER TABLE [dbo].[BDI_CRM_InfoAdicionalClientes_ACT] add fLNSaldoCartera  float;
ALTER TABLE [dbo].[BDI_CRM_InfoAdicionalClientes_ACT] add fLNSaldoCarteraTC float;
ALTER TABLE [dbo].[BDI_CRM_InfoAdicionalClientes_ACT] add fLNSaldoCarteraVivienda float;
ALTER TABLE [dbo].[BDI_CRM_InfoAdicionalClientes_ACT] add fLNSaldoPromedioMensualCaptaciones float;
ALTER TABLE [dbo].[BDI_CRM_InfoAdicionalClientes_ACT] add fLNVariacionSaldoPromedioMensualCaptaciones float;

ALTER TABLE [dbo].[BDI_CRM_InfoAdicionalClientes_ACT] add fEdadCliente  float;
ALTER TABLE [dbo].[BDI_CRM_InfoAdicionalClientes_ACT] add fVecesVigente2TC  float;
ALTER TABLE [dbo].[BDI_CRM_InfoAdicionalClientes_ACT] add fNroAdicionales  float;
ALTER TABLE [dbo].[BDI_CRM_InfoAdicionalClientes_ACT] add fNroReposiciones  float;
ALTER TABLE [dbo].[BDI_CRM_InfoAdicionalClientes_ACT] add fNroIniciales  float;
ALTER TABLE [dbo].[BDI_CRM_InfoAdicionalClientes_ACT] add fNroSinSolicitud  float;
ALTER TABLE [dbo].[BDI_CRM_InfoAdicionalClientes_ACT] add fTasaCredito  float;

ALTER TABLE [dbo].[BDI_CRM_InfoAdicionalClientes_ACT] add bTieneAlarmaProbable  bit;
ALTER TABLE [dbo].[BDI_CRM_InfoAdicionalClientes_ACT] add dFechaAlarmaProbable  datetime;
ALTER TABLE [dbo].[BDI_CRM_InfoAdicionalClientes_ACT] add fRatioAlarma  decimal(10,2);

ALTER TABLE [dbo].[BDI_CRM_InfoAdicionalClientes_ACT] add sRangoSaldoInicial  nvarchar(1);

ALTER TABLE [dbo].[BDI_CRM_InfoAdicionalClientes_ACT] add cSumatoriaCuotasMaximas  money;
ALTER TABLE [dbo].[BDI_CRM_InfoAdicionalClientes_ACT] add cSumatoriaCuotaVigente  money;

-- ***********************************************
-- Add nuevo campo en BDI_CRM_InfoAdicionalClientes_DIF
-- ***********************************************
ALTER TABLE [dbo].[BDI_CRM_InfoAdicionalClientes_DIF] add fLNIngresoMensual  float;
ALTER TABLE [dbo].[BDI_CRM_InfoAdicionalClientes_DIF] add fLNSaldoCaptaciones  float;
ALTER TABLE [dbo].[BDI_CRM_InfoAdicionalClientes_DIF] add fLNSaldoCartera  float;
ALTER TABLE [dbo].[BDI_CRM_InfoAdicionalClientes_DIF] add fLNSaldoCarteraTC float;
ALTER TABLE [dbo].[BDI_CRM_InfoAdicionalClientes_DIF] add fLNSaldoCarteraVivienda float;
ALTER TABLE [dbo].[BDI_CRM_InfoAdicionalClientes_DIF] add fLNSaldoPromedioMensualCaptaciones float;
ALTER TABLE [dbo].[BDI_CRM_InfoAdicionalClientes_DIF] add fLNVariacionSaldoPromedioMensualCaptaciones float;
ALTER TABLE [dbo].[BDI_CRM_InfoAdicionalClientes_DIF] add fEdadCliente  float;
ALTER TABLE [dbo].[BDI_CRM_InfoAdicionalClientes_DIF] add fVecesVigente2TC  float;
ALTER TABLE [dbo].[BDI_CRM_InfoAdicionalClientes_DIF] add fNroAdicionales  float;
ALTER TABLE [dbo].[BDI_CRM_InfoAdicionalClientes_DIF] add fNroReposiciones  float;
ALTER TABLE [dbo].[BDI_CRM_InfoAdicionalClientes_DIF] add fNroIniciales  float;
ALTER TABLE [dbo].[BDI_CRM_InfoAdicionalClientes_DIF] add fNroSinSolicitud  float;
ALTER TABLE [dbo].[BDI_CRM_InfoAdicionalClientes_DIF] add fTasaCredito  float;
ALTER TABLE [dbo].[BDI_CRM_InfoAdicionalClientes_DIF] add bTieneAlarmaProbable  bit;
ALTER TABLE [dbo].[BDI_CRM_InfoAdicionalClientes_DIF] add dFechaAlarmaProbable  datetime;
ALTER TABLE [dbo].[BDI_CRM_InfoAdicionalClientes_DIF] add fRatioAlarma  decimal(10,2);
ALTER TABLE [dbo].[BDI_CRM_InfoAdicionalClientes_DIF] add sRangoSaldoInicial  nvarchar(1);
ALTER TABLE [dbo].[BDI_CRM_InfoAdicionalClientes_DIF] add cSumatoriaCuotasMaximas  money;
ALTER TABLE [dbo].[BDI_CRM_InfoAdicionalClientes_DIF] add cSumatoriaCuotaVigente  money;

-------------------------------
--CREACION DE TABLA BDI_DIVISAS
------------------------------

USE [BDI_CRM]
GO

/****** Object:  Table [dbo].[BDI_Divisas]    Script Date: 02/11/2014 09:54:19 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BDI_Divisas]') AND type in (N'U'))
DROP TABLE [dbo].[BDI_Divisas]
GO

USE [BDI_CRM]
GO

/****** Object:  Table [dbo].[BDI_Divisas]    Script Date: 02/11/2014 09:54:19 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BDI_Divisas](
	[cTasaCambio] [money] NULL,
	[sCodigoMoneda] [nvarchar](4) NULL
	)
ON [PRIMARY]

GO

-------------------------------
--CREACION DE TABLA BDI_DIVISAS_LOG
------------------------------
USE [BDI_CRM]
GO

/****** Object:  Table [dbo].[BDI_Divisas_Log]    Script Date: 02/11/2014 14:01:03 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BDI_Divisas_Log]') AND type in (N'U'))
DROP TABLE [dbo].[BDI_Divisas_Log]
GO

USE [BDI_CRM]
GO

/****** Object:  Table [dbo].[BDI_Divisas_Log]    Script Date: 02/11/2014 14:01:04 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BDI_Divisas_Log](
	[cTasaCambio] [money] NULL,
	[sCodigoMoneda] [nvarchar](4) NULL,
	[dFechaCarga] [datetime]
) ON [PRIMARY]

GO
--------------------------
/*campos servicios canales*/
--------------------------
ALTER TABLE [dbo].[BDI_Canales] add sCodigoMoneda   nvarchar(4);
ALTER TABLE [dbo].[BDI_CRM_Canales_DIF] add sCodigoMoneda   nvarchar(4);
ALTER TABLE [dbo].[BDI_CRM_Canales_ACT] add sCodigoMoneda   nvarchar(4);
ALTER TABLE [dbo].[BDI_Servicios] add sCodigoMoneda   nvarchar(4);
ALTER TABLE [dbo].[BDI_CRM_Servicios_DIF] add sCodigoMoneda   nvarchar(4);
ALTER TABLE [dbo].[BDI_CRM_Servicios_ACT] add sCodigoMoneda   nvarchar(4);


--------------------------
/*fase II prioridad 0*/
--------------------------

--creacion de las variables calculadas
ALTER TABLE dbo.BDI_InfoAdicionalClientes 
		ADD ianos_como_cliente			float NULL, 
			icantidad_tx				float NULL,
			icantidad_tx_td				float NULL,
			ieduacion					float NULL,
			itotat_productos_activos	float NULL,
			icartera_sistema			float NULL,
			icapacidad_ahorro			float NULL,
			iplazo_transcurrido_credito float NULL,
			ialarma_vivienda            float NULL;
			
--creacion de las variables dummies
ALTER TABLE dbo.BDI_InfoAdicionalClientes 
		ADD bdm_capital_inversion			float null,
			bdm_capital_operaciones			float null,
			bdm_comerciante					float null,
			bdm_conyuge						float null,
			bdm_cred_consumo				float null,
			bdm_cred_vehicular				float null,
			bdm_cred_vivienda				float null,
			bdm_dependiente					float null,
			bdm_edad_022					float null,
			bdm_edad_027					float null,
			bdm_edad_029					float null,
			bdm_edad_039					float null,
			bdm_edad_041					float null,
			bdm_edad_2334					float null,
			bdm_edad_2833					float null,
			bdm_edad_3036					float null,
			bdm_edad_3057					float null,
			bdm_edad_3439					float null,
			bdm_edad_3748					float null,
			bdm_edad_4047					float null,
			bdm_edad_4059					float null,
			bdm_edad_4256					float null,
			bdm_edad_4859					float null,
			bdm_edad_4965					float null,
			bdm_edad_5765					float null,
			bdm_ingresos_0300				float null,
			bdm_ingresos_14562908			float null,
			bdm_ingresos_29095000			float null,
			bdm_ingresos_3011455			float null,
			bdm_juridico					float null,
			bdm_masculino					float null,
			bdm_productivo					float null,
			bdm_region_centro				float null,
			bdm_region_occidente			float null,
			bdm_region_oriente				float null,
			bdm_servicios					float null,
			bdm_tarjeta_credito				float null,
			bdm_utilizacion_025				float null,
			bdm_utilizacion_26335			float null,
			bdm_utilizacion_456755			float null,
			bdm_utilizacion_mas_755			float null;

----creacion de las variables fecha de cumpleaños
ALTER TABLE dbo.BDI_InfoAdicionalClientes 
		ADD dfechacumpleanos			datetime null;




-- ***********************************************
-- Add nuevo campo en BDI_ProdActivosCreditos,BDI_CRM_ProdActivosCreditos_ACT,BDI_CRM_ProdActivosCreditos_DIF
-- ***********************************************
  ALTER TABLE [dbo].[BDI_ProdActivosCreditos]               add iUltimaCuotaPagada int
  ALTER TABLE [dbo].[BDI_CRM_ProdActivosCreditos_ACT]       add iUltimaCuotaPagada int
  ALTER TABLE [dbo].[BDI_CRM_ProdActivosCreditos_DIF]               add iUltimaCuotaPagada int

-------------------
--TABLA PARAMETRO
-------------------

USE [BDI_CRM]
GO

/****** Object:  Table [dbo].[BDI_Parametros]    Script Date: 02/24/2014 14:23:45 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BDI_Parametros]') AND type in (N'U'))
DROP TABLE [dbo].[BDI_Parametros]
GO

USE [BDI_CRM]
GO

/****** Object:  Table [dbo].[BDI_Parametros]    Script Date: 02/24/2014 14:23:45 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BDI_Parametros](
	[sParametro] [nvarchar](100) NULL,
	[sValor] [nvarchar](70) NULL
) ON [PRIMARY]

GO

-------------------
--REGISTRO PARAMETRO
-------------------
INSERT INTO [BDI_CRM].[dbo].[BDI_Parametros]
           ([sParametro]
           ,[sValor])
     VALUES
           ('estado_operacion'
           ,'VIGENTE')
