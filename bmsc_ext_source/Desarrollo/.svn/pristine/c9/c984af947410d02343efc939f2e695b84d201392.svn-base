/****** Object:  StoredProcedure [dbo].[spDatosAddServiUnikClient]    Script Date: 08/27/2013 11:35:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--***********************************************************************************
--******** Procedimiento para el cÃ¡lculo de la Oferta de Valor **********************
--ParÃ¡metros:
--Fecha: 
CREATE Procedure [dbo].[spDatosAddServiUnikClient] (@codtipoper     varchar(20) = null,
                                           @codtipodoc          varchar(20) = null,                                   
                                           @error         varchar(200) output)
As 
begin

select 
'COD_TIPO_DOC' as 'campo',
sCodigoValorBdi as 'valor'
from dbo.BDI_CRM_MapeoPicklistValores 
where iCodigoMapeo = 12 and iCodigoValorCrm = @codtipodoc
union
select 
'TIPO_PERSONA' as 'campo',
sCodigoValorBdi as 'valor'
from dbo.BDI_CRM_MapeoPicklistValores 
where iCodigoMapeo = 10 and iCodigoValorCrm = @codtipoper

end;
GO
