/****** Object:  StoredProcedure [dbo].[spConciliacionOportunidad]    Script Date: 08/27/2013 11:35:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[spConciliacionOportunidad]
as

--Proceso de conciliación de oportunidades

/**********************************************************************************************
Paso 1: Seleccionamos todas las oportunidades que cumplan con las siguientes condiciones.
A: Aquellas que se encuentren Abiertas, pero que la fecha de cierre estimado sea menor a la fecha actual
B: Auqellas que se ecuentran en ESTADO_CIERRE "En espera de conciliacion" o ESTADO_CIERRE "Liquidada" y RAZON_ESTADO_CIERRE "Cierre parcial", 
además en ambos casos se debe tomar en cuenta que los productos se encuentren dentro del tiempo de conciliación.

Estados de cierre de oportunidad:
 100000000 "En espera de conciliación"
 100000001 "Liquidada"
 100000002 "Perdida"

Razón de estado de cierre de oportunidad:
 100000000 "En espera de conciliación"
 100000001 "Cierre efectivo"
 100000002 "Cierre parcial"
 100000003 "Conciliacion automatica"
 100000004 "No conciliada"

Estados de cierre de producto de oportunidad:
 100000000 "En espera de conciliación"
 100000001 "Liquidada"
 100000002 "Perdida"
**********************************************************************************************/

/**********************************************************************************************/
/**********************************************************************************************/

--Obtenemos la fecha actual que servirá de referencia en la evaluación de fechas de las oportunidades
declare @FechaActual as datetime

set @FechaActual=convert(date,GETUTCDATE()) 

--Obtenemos todas las oportunidades que deben ser evaluadas en la conciliación
-------------IMPORTANTE: Todas las fechas en CRM en la BD están en UTC
select 
op.OpportunityId OpportunityId,
ac.efk_codigo_cliente as efk_codigo_cliente,
ac.AccountId as CustomerId,
op.CustomerIdType,
oprod.OpportunityProductId OpportunityProductId,
p.ProductId ProductId,
p.name,
p.efk_tipo_productoid efk_tipo_productoid,
op.statecode statecode,
op.statuscode statuscode,
0 ActualizarRegistroOportunidad,
0 ActualizarRegistroProductoOportunidad,
op.createdon FechaReferencia, --Todo lo trabajamos en UTC, ya que asi es como CRM tiene guardada la información
op.estimatedvalue MontoOportunidad,
DATEADD(dd,IsNull(p.efk_dias_conciliacion,0),(case op.statecode when 1 then op.ActualCloseDate else op.EstimatedCloseDate end)) FechaMaximaConciliacionProducto,
@FechaActual FechaMaximaConciliacionOportunidad,
@FechaActual FechaMinimaConciliacionOportunidad,
oprod.efk_numero_operacion NumeroOperacion, --Colocamos el nombre temporalmente, ya que esto reemplazariamos con el número de la operación
100000000 EstadoConciliacionOportunidad, --todos comienzan con el estado "En espera de conciliacion"
100000000 RazonEstadoConciliacionOportunidad, --todos comienzan con el estado "En espera de conciliación"
100000000 EstadoConciliacionProducto --todos comienzan con el estado "En espera de conciliacion"
into #TMP_OportunidadesConciliar
from BMSC_MSCRM..opportunity op
inner join BMSC_MSCRM..opportunityproduct oprod on op.opportunityid=oprod.OpportunityId
inner join BMSC_MSCRM..Product p on p.ProductId=oprod.ProductId
inner join BMSC_MSCRM..Account ac on ac.AccountId=op.CustomerId
where (ac.efk_codigo_cliente is not null and ac.AccountId is not null)
and (oprod.efk_estado_cierre=100000000 or oprod.efk_estado_cierre is null) --solo incluimos aquellos productos no conciliados
and ( --las 3 condiciones
(op.StateCode=1 and op.efk_estado_cierre=100000000)
or (op.StateCode=1 and op.efk_estado_cierre=100000001 and op.efk_razon_cierre_estado=100000002)
or (op.StateCode=0 and convert(date,DATEADD(dd,IsNull(p.efk_dias_conciliacion,0),(case op.statecode when 1 then op.ActualCloseDate else op.EstimatedCloseDate end)))<=@FechaActual)
)
/***********************************************************************************************************************/
/***********************************************************************************************************************/

--Preparamos tabla de clientes con sus respectivos productos
--Solo se incluyen aquellos clientes que poseen oportunidades calificadas para la conciliación
select AccountId CustomerId, productid, efk_producto_coreId, efk_clase_producto_banco, efk_numero, montoOperacion, CodigoModulo, CodigoProducto, CodigoTipo, CodigoProductoCore, CodigoUnicoProducto, FechaCreacion into #TMP_Clientes from(
select co.AccountId, p.productid, pc.efk_producto_coreId, pa.efk_clase_producto_banco, pa.efk_numero, (Isnull(pa.efk_monto_desembolsado,0)+Isnull(pa.efk_limite_financiamiento,0)) montoOperacion, pa.efk_codigo_modulo_producto CodigoModulo, pa.efk_codigo_producto CodigoProducto, pa.efk_codigo_tipo CodigoTipo, pa.efk_codigo_producto_core CodigoProductoCore, pa.efk_codigo_unico_producto CodigoUnicoProducto, pa.createdon FechaCreacion
from BMSC_MSCRM..Account co
inner join #TMP_OportunidadesConciliar tmp on tmp.CustomerId=co.AccountId
inner join BMSC_MSCRM..efk_producto_activo pa on co.AccountId=pa.efk_cliente_juridico_Id
inner join BMSC_MSCRM..efk_producto_core pc on pc.efk_producto_coreId=pa.efk_producto_core_Id
inner join BMSC_MSCRM..Product p on pc.efk_productid=p.ProductId
where pa.statecode=0
union
select co.AccountId, p.productid, pc.efk_producto_coreId, pp.efk_clase_producto_banco, pp.efk_numero_cuenta, 0 montoOperacion,pp.efk_codigo_modulo CodigoModulo, pp.efk_codigo_producto CodigoProducto, pp.efk_codigo_tipo CodigoTipo, pp.efk_codigo_producto_core CodigoProductoCore, pp.efk_codigo_unico_producto CodigoUnicoProducto, pp.createdon FechaCreacion
from BMSC_MSCRM..Account co
inner join #TMP_OportunidadesConciliar tmp on tmp.CustomerId=co.AccountId
inner join BMSC_MSCRM..efk_producto_pasivo pp on co.AccountId=pp.efk_cliente_juridico_Id
inner join BMSC_MSCRM..efk_producto_core pc on pc.efk_producto_coreId=pp.efk_producto_core_Id
inner join BMSC_MSCRM..Product p on pc.efk_productid=p.ProductId
where pp.statecode=0
union
select co.AccountId, p.productid, pc.efk_producto_coreId, sc.efk_clase_producto_banco, null efk_numero, 0 montoOperacion,null CodigoModulo,null CodigoProducto,null CodigoTipo,null CodigoProductoCore,sc.efk_codigo_unico_producto CodigoUnicoProducto, sc.createdon FechaCreacion
from BMSC_MSCRM..Account co
inner join #TMP_OportunidadesConciliar tmp on tmp.CustomerId=co.AccountId
inner join BMSC_MSCRM..efk_servicios_canales sc on co.AccountId=sc.efk_accountid
inner join BMSC_MSCRM..efk_producto_core pc on pc.efk_producto_coreId=sc.efk_producto_coreid
inner join BMSC_MSCRM..Product p on pc.efk_productid=p.ProductId
where sc.statecode=0
) Clientes


--Armamos una tabla de todos las operaciones de productos que se encuentran en la BDI con sus
--respectivas fecha de creación/emisión
--Solo incluimos los productos de los clientes que poseen oportunidades calificadas para conciliar
select CustomerId, CustomerIdType, sCodigoTipo, sCodigoProducto, snrocuenta, montoOperacion, convert(datetime,DATEADD(hh,datediff(hh,getdate(),getutcdate()),dFechaInicioOperacionEmision)) dFechaInicioOperacionEmision, sCodigoUnicoProducto into #TMP_ProductosClienteBDI from
(select tmp.CustomerId, tmp.CustomerIdType, p.sCodigoTipo, p.sCodigoProducto, cast(p.snrocuenta as nvarchar(30)) snrocuenta, cMontoDeLaOperacionCapital montoOperacion, p.dFechaInicioOperacionEmision, p.sCodigoUnicoProducto from dbo.BDI_ProdActivosCreditos p inner join #TMP_OportunidadesConciliar tmp
on p.iCodigoClienteBanco=tmp.efk_codigo_cliente
union 
select tmp.CustomerId, tmp.CustomerIdType, p.sCodigoTipo, p.sCodigoProducto, cast(p.nnrocuenta as nvarchar(30)) snrocuenta, cMontoDeLaOperacionCapital montoOperacion, p.dFechaInicioOperacionEmision, p.sCodigoUnicoProducto from dbo.BDI_ProdActivosComex p inner join #TMP_OportunidadesConciliar tmp
on p.iCodigoClienteBanco=tmp.efk_codigo_cliente
union 
select tmp.CustomerId, tmp.CustomerIdType, p.sCodigoTipo, p.sCodigoProducto, cast(p.sNroOperacion as nvarchar(30)) snrocuenta, cMontoAprobado montoOperacion,p.dFechaAperturaOperacion, p.sCodigoUnicoProducto from dbo.BDI_ProdActivosLineasCredito p inner join #TMP_OportunidadesConciliar tmp
on p.iCodigoClienteBanco=tmp.efk_codigo_cliente
union 
select tmp.CustomerId, tmp.CustomerIdType, p.sCodigoTipo, p.sCodigoProducto, cast(p.snrocuenta as nvarchar(30)) snrocuenta, cLimiteFinanciamiento montoOperacion, dFechaVencimientoOperacion, p.sCodigoUnicoProducto from dbo.BDI_ProdActivosTDC p inner join #TMP_OportunidadesConciliar tmp
on p.iCodigoClienteBanco=tmp.efk_codigo_cliente
union 
select tmp.CustomerId, tmp.CustomerIdType, p.sCodigoTipo, p.sCodigoProducto sSubTipoProducto, cast(p.nnrocuenta as nvarchar(30)) snrocuenta, cMontoOper montoOperacion, p.dFechaAperturaEmision, p.sCodigoUnicoProducto from dbo.BDI_ProdPasivos p inner join #TMP_OportunidadesConciliar tmp
on p.iCodigoClienteBanco=tmp.efk_codigo_cliente
union 
select tmp.CustomerId, tmp.CustomerIdType, null sTipoProducto, null sSubTipoProducto, null ,0 montoOperacion,p.dFechaApertura, p.sCodigoUnicoProducto from dbo.BDI_Servicios p inner join #TMP_OportunidadesConciliar tmp
on p.iCodigoClienteBanco=tmp.efk_codigo_cliente
union 
 select tmp.CustomerId, tmp.CustomerIdType, null sTipoProducto, null sSubTipoProducto, null ,0 montoOperacion, p.dFechaAperturaEmision, p.sCodigoUnicoProducto from dbo.BDI_Canales p inner join #TMP_OportunidadesConciliar tmp
on p.iCodigoClienteBanco=tmp.efk_codigo_cliente
) ProductosClienteBDI


--Actualizamos la fecha de creación de los productos que posee el cliente en base a la fecha de
--inicio/emisión de los productos que estan en la BDI.
update #TMP_Clientes 
set FechaCreacion=dFechaInicioOperacionEmision from #TMP_Clientes 
tmpCl inner join #TMP_ProductosClienteBDI tmpPrd on 
tmpCl.CustomerId=tmpPrd.CustomerId and --cruzamos por el cliente 
--tmpCl.CodigoProductoCore=tmpPrd.sCodigoUnicoProducto collate database_default and
tmpCl.CodigoUnicoProducto = tmpPrd.sCodigoUnicoProducto collate database_default and
(tmpCl.efk_numero is null or tmpCl.efk_numero=tmpPrd.snrocuenta collate database_default) and
tmpPrd.dFechaInicioOperacionEmision is not null

drop table #TMP_ProductosClienteBDI


--Marcamos el producto de oportunidad como "liquidado" para aquellos productos que están dentro del periodo de
--conciliación y cuya fecha de referencia es menor a la fecha de creación
update #TMP_OportunidadesConciliar 
set EstadoConciliacionProducto = 100000001,
NumeroOperacion = coalesce((select top 1 efk_numero from #TMP_Clientes cl 
where
op.CustomerId=cl.CustomerId and op.productid=cl.productid and
convert(datetime,convert(varchar,op.FechaReferencia,101)) <= convert(datetime,convert(varchar,cl.FechaCreacion,101)) 
and cl.efk_clase_producto_banco in (221220006,221220007,221220008,221220010,221220011) order by cl.FechaCreacion desc),null)
from #TMP_OportunidadesConciliar op where 
exists (select 1 from #TMP_Clientes cl 
where
op.CustomerId=cl.CustomerId and op.productid=cl.productid and
convert(datetime,convert(varchar,op.FechaReferencia,101))<=convert(datetime,convert(varchar,cl.FechaCreacion,101)) 
and cl.efk_clase_producto_banco in (221220006,221220007,221220008,221220010,221220011))
--------------------------------------------------------------------------------------------------------------------------

--Actualizamos el estado de cierre de los productos de oportunidad a "Perdido" cuando ya ha llegado su fecha maxima de conciliacion y no se han conciliado.
update #TMP_OportunidadesConciliar set EstadoConciliacionProducto=100000002 from #TMP_OportunidadesConciliar tmpOp
where convert(date,tmpOp.FechaMaximaConciliacionProducto)<@FechaActual and EstadoConciliacionProducto <> 100000001 --solo preguntamos "menor" debido a que ya se le habia aumentado un dia en la seleccion de oportunidades

drop table #TMP_Clientes

/**********************************************************************************************/
/**********************************************************************************************/

--Obtenemos el estado actual de todas las oportunidades
select op.opportunityid, Op.statecode, op.efk_estado_cierre bb1_estado_cierre_op, op.efk_razon_cierre_estado,
op.efk_fecha_maxima_conciliacion, op.efk_fecha_minima_conciliacion, oprod1.opportunityproductid, oprod1.efk_estado_cierre bb1_estado_cierre_prod
into #TMP_OportunidadesActuales
from
BMSC_MSCRM..Opportunity op 
inner join BMSC_MSCRM..OpportunityProduct oprod1 on op.opportunityid=oprod1.opportunityid
where exists (select 1 from #TMP_OportunidadesConciliar tmpOp where tmpOp.OpportunityId=op.opportunityid)

--La tabla anterior la vamos a utilizar para comparar al final, por lo que copiamos en otra tabla temporal
select * into #TMP_OportunidadesActualesActualizar from #TMP_OportunidadesActuales

--Actualizamos el estado de los productos en la tabla anterior en base al cálculo ya realizado de las oportunidades a conciliar
update #TMP_OportunidadesActualesActualizar set bb1_estado_cierre_prod=tmpOp.EstadoConciliacionProducto from #TMP_OportunidadesActualesActualizar tmpAc inner join
#TMP_OportunidadesConciliar tmpOp on tmpAc.opportunityid=tmpOp.OpportunityId and
tmpAc.opportunityproductid=tmpOp.OpportunityProductId

--Ahora averiguamos cual debe ser el estado de conciliacion de la oportunidad
select oprod1.opportunityid,
COUNT(oprod1.OpportunityProductid) CantidadProductos, 
COUNT(oprod2.OpportunityProductid) CantidadProductosConciliados,
COUNT(oprod3.OpportunityProductid) CantidadProductosPerdididos
into #TMP_AgrupadoOportunidadesActuales
from
#TMP_OportunidadesActualesActualizar oprod1
left outer join #TMP_OportunidadesActualesActualizar oprod2
on oprod1.opportunityproductid=oprod2.opportunityproductid and oprod2.bb1_estado_cierre_prod=100000001
left outer join #TMP_OportunidadesActualesActualizar oprod3
on oprod1.opportunityproductid=oprod3.opportunityproductid and oprod3.bb1_estado_cierre_prod=100000002
group by oprod1.opportunityid

drop table #TMP_OportunidadesActualesActualizar

--Actualizamos la tabla principal de oportunidades colocando el estado de conciliacion que se debe colocar en la oportunidad

--CASO 1: Cuando no se concilia ningun producto se deja el valor de la oportunidad en "Espera de conciliacion"
update #TMP_OportunidadesConciliar set EstadoConciliacionOportunidad=100000000, 
RazonEstadoConciliacionOportunidad=100000000 from #TMP_OportunidadesConciliar tmpOp
where exists(select 1 from #TMP_AgrupadoOportunidadesActuales tmpAc where tmpAc.opportunityid=tmpOp.OpportunityId and 
tmpAc.CantidadProductosConciliados=0)

--CASO 2: Cuando se concilia mas de 1 producto y menos que todos, se deja el valor en "Liquidada", "Cierre paracial"
update #TMP_OportunidadesConciliar set EstadoConciliacionOportunidad=100000001, 
RazonEstadoConciliacionOportunidad=100000002 from #TMP_OportunidadesConciliar tmpOp
where exists(select 1 from #TMP_AgrupadoOportunidadesActuales tmpAc where tmpAc.opportunityid=tmpOp.OpportunityId and 
tmpAc.CantidadProductosConciliados>0 and tmpAc.CantidadProductosConciliados<tmpAc.CantidadProductos)

--CASO 3: Cuando se concilian todos los productos, el valor queda "Liquidada", "Cierre efectivo"
update #TMP_OportunidadesConciliar set EstadoConciliacionOportunidad=100000001, 
RazonEstadoConciliacionOportunidad=100000001 from #TMP_OportunidadesConciliar tmpOp
where exists(select 1 from #TMP_AgrupadoOportunidadesActuales tmpAc where tmpAc.opportunityid=tmpOp.OpportunityId and 
tmpAc.CantidadProductosConciliados>0 and tmpAc.CantidadProductosConciliados=tmpAc.CantidadProductos)

--CASO 4: Cuando todos los productos de uns oportunidad estan perdidos, la oportunidad se cierra como perdida
update #TMP_OportunidadesConciliar set EstadoConciliacionOportunidad=100000002, 
RazonEstadoConciliacionOportunidad=100000004 from #TMP_OportunidadesConciliar tmpOp
where exists(select 1 from #TMP_AgrupadoOportunidadesActuales tmpAc where tmpAc.opportunityid=tmpOp.OpportunityId and 
tmpAc.CantidadProductosPerdididos>0 and tmpAc.CantidadProductosPerdididos=tmpAc.CantidadProductos) 

--Actualizamos la fecha maxima y minima de conciliacion
select OpportunityId, MAX(FechaMaximaConciliacionProducto) FechaMaxima, 
MIN(FechaMaximaConciliacionProducto) FechaMinima
into
#TMP_AgrupadoOportunidadesConciliar
from #TMP_OportunidadesConciliar
group by OpportunityId

update #TMP_OportunidadesConciliar set 
FechaMaximaConciliacionOportunidad=tmpAgrupado.FechaMaxima,
FechaMinimaConciliacionOportunidad=tmpAgrupado.FechaMinima
from #TMP_OportunidadesConciliar tmpOp inner join #TMP_AgrupadoOportunidadesConciliar tmpAgrupado on tmpOp.opportunityid=tmpAgrupado.opportunityid

--Actualizamos el estado ActualizarRegistroOportunidad a todos aquellos registros de oportunidad que estan abiertas 
--y que estan totalmente conciliadas
update #TMP_OportunidadesConciliar set ActualizarRegistroOportunidad=1, ActualizarRegistroProductoOportunidad=1,
RazonEstadoConciliacionOportunidad=100000003 --La razon de cierre es "Conciliacion automatica"
from #TMP_OportunidadesConciliar tmpOp where tmpOp.statecode=0
and tmpOp.EstadoConciliacionOportunidad=100000001 and tmpOp.RazonEstadoConciliacionOportunidad=100000001

--Comparamos con los datos que actualmente existen en CRM para saber que registros se debe actualizar
update #TMP_OportunidadesConciliar set ActualizarRegistroOportunidad=1
from #TMP_OportunidadesConciliar tmpOp where tmpOp.statecode=1 and not exists
(select 1 from #TMP_OportunidadesActuales op where op.opportunityid=tmpOp.OpportunityId
and op.efk_fecha_maxima_conciliacion=tmpOp.FechaMaximaConciliacionOportunidad
and op.efk_fecha_minima_conciliacion=tmpOp.FechaMinimaConciliacionOportunidad
and op.bb1_estado_cierre_op=tmpOp.EstadoConciliacionOportunidad
and op.efk_razon_cierre_estado=tmpOp.RazonEstadoConciliacionOportunidad
)

--Actualizamos los productos de oportunidad
update #TMP_OportunidadesConciliar set ActualizarRegistroProductoOportunidad=1
from #TMP_OportunidadesConciliar tmpOp where tmpOp.statecode=1 and not exists
(select 1 from #TMP_OportunidadesActuales op where op.opportunityproductid=tmpOp.opportunityproductid and
op.bb1_estado_cierre_prod=tmpOp.EstadoConciliacionProducto)

drop table #TMP_AgrupadoOportunidadesActuales
drop table #TMP_AgrupadoOportunidadesConciliar
drop table #TMP_OportunidadesActuales

--Obtenemos los datos de los cierre de oportunidades, los cuales nos servirán para posteriormente cerrar nuevamente las oportunidades
--Esto es debido a que para actualizar las oportunidades si estan cerradas (ganadas o perdidas) es necesario reabrirlas y volverlas a cerrar
select
oc.opportunityid,
oc.ActivityId
into #TMP_OpportunityClose
from BMSC_MSCRM..OpportunityClose oc
where exists (select 1 from #TMP_OportunidadesConciliar tmpOp where tmpOp.OpportunityId=oc.opportunityid)
and oc.ActivityId=(select top 1 oc2.ActivityId from BMSC_MSCRM..OpportunityClose oc2
where oc2.opportunityid=oc.OpportunityId order by oc2.createdon desc)
                                                                                                                                                
--Guardamos los datos en la tabla final
--Primero borramos la tabla
truncate table dbo.BDI_CRM_DatosConciliacion

insert into dbo.BDI_CRM_DatosConciliacion
(uOpportunityId,uOpportunityProductId,iOpportunityStateCode,iOpportunityStatusCode,dFechaMaximaConciliacionProducto,
dFechaMaximaConciliacionOportunidad,dFechaMinimaConciliacionOportunidad,iEstadoConciliacionOportunidad,
iRazonEstadoConciliacionOportunidad,iEstadoConciliacionProducto,uOpportunityCloseId,sOperacionProducto)
select tmpOp.OpportunityId,tmpOp.OpportunityProductId,tmpOp.statecode, tmpOp.statuscode,
tmpOp.FechaMaximaConciliacionProducto,tmpOp.FechaMaximaConciliacionOportunidad,
tmpOp.FechaMinimaConciliacionOportunidad,tmpOp.EstadoConciliacionOportunidad,tmpOp.RazonEstadoConciliacionOportunidad,
tmpOp.EstadoConciliacionProducto,oc.activityid, tmpOp.NumeroOperacion  from #TMP_OportunidadesConciliar tmpOp
left outer join #TMP_OpportunityClose oc on tmpop.OpportunityId=oc.opportunityid
where tmpOP.ActualizarRegistroOportunidad=1 or tmpOp.ActualizarRegistroProductoOportunidad=1

--Todas las oportunidades que fueron enviadas a OnBase y han sido procesadas.
--Solo seleccionamos las oportunidades que estén pendientes de conciliación
select c.iCodigoClienteBanco,
       c.sNumOperacion,
       op.OpportunityId,
       op.CustomerIdType,
       op.statecode statecode,
       op.statuscode statuscode,
       op.createdon FechaReferencia, 
       op.estimatedvalue MontoOportunidad,
       oprod.OpportunityProductId OpportunityProductId,
       oprod.efk_numero_operacion,
       DATEADD(dd,IsNull(p.efk_dias_conciliacion,0),(case op.statecode when 1 then op.ActualCloseDate else op.EstimatedCloseDate end)) FechaMaximaConciliacionProducto,
       op.efk_estado_cierre
 into #TMP_OportunidadesConciliarOnBase
 from BDI_ProductoConciliar c left join BMSC_MSCRM..opportunity op
 on c.iNumSolicitud = op.efk_nrosolicitud inner join BMSC_MSCRM..opportunityproduct oprod 
 on op.opportunityid=oprod.OpportunityId inner join BMSC_MSCRM..Product p 
 on p.ProductId=oprod.ProductId
 where op.efk_estado_cierre is null or op.efk_estado_cierre = 100000000

Declare @iCodigoClienteBanco int = 0
Declare @sNumOperacion nvarchar(10) = null
Declare @OpportunityId uniqueidentifier = null
Declare @CustomerIdType int = 0
Declare @statecode int = 0
Declare @statuscode int = 0
Declare @FechaReferencia datetime = null 
Declare @MontoOportunidad money = null
Declare @OpportunityProductId uniqueidentifier = null  
Declare @efk_numero_operacion nvarchar(50) = null
Declare @FechaMaximaConciliacionProducto datetime = null
Declare @FechaMaxima datetime = null
Declare @FechaMinima datetime = null
Declare @ActivityId uniqueidentifier = null 
Declare @NumReg int = 0
Declare @estado_cierre int = 0
Declare @razon_estado_cierre int = 0


DECLARE OportunidadesConciliarOnBase CURSOR FOR 
SELECT  iCodigoClienteBanco, sNumOperacion, OpportunityId, 	
        statecode, statuscode, efk_estado_cierre,
        FechaReferencia, MontoOportunidad, OpportunityProductId,
		efk_numero_operacion, FechaMaximaConciliacionProducto 
from #TMP_OportunidadesConciliarOnBase

Open OportunidadesConciliarOnBase		
fetch OportunidadesConciliarOnBase into @iCodigoClienteBanco,@sNumOperacion,@OpportunityId, @statecode,@statuscode,@estado_cierre,
                                                  @FechaReferencia, @MontoOportunidad, @OpportunityProductId, @efk_numero_operacion, @FechaMaximaConciliacionProducto
While @@fetch_status = 0
Begin
	if (not @estado_cierre is null and not @statecode is null)
	begin
		if (@estado_cierre = 100000001 and @statecode = 1)
		begin
			Set @razon_estado_cierre = 100000001
		end
		else
		begin
			Set @razon_estado_cierre = 100000003
		end	
	end	
	else
	begin
		Set @razon_estado_cierre = 100000003
	end	
    
	Select @FechaMaxima = MAX(FechaMaximaConciliacionProducto), 
	       @FechaMinima = MIN(FechaMaximaConciliacionProducto) 
	from #TMP_OportunidadesConciliar
	group by OpportunityId
	having OpportunityId = @OpportunityId
	   
	Select @ActivityId = ActivityId
    from BMSC_MSCRM..OpportunityClose where opportunityid = @OpportunityId
    
    Set @NumReg = 0
    select @NumReg = COUNT(1) from BDI_CRM_DatosConciliacion where uOpportunityId = @OpportunityId
	If (@NumReg = 0)
	begin	   
	   Insert Into BDI_CRM_DatosConciliacion
	   values (@OpportunityId, @OpportunityProductId, @statecode, @statuscode, @FechaMaximaConciliacionProducto,@FechaMaxima, @FechaMinima, 100000001, @razon_estado_cierre, 100000001, @ActivityId, @sNumOperacion)
    end
    else
    begin
       Update BDI_CRM_DatosConciliacion set iEstadoConciliacionOportunidad = 100000001,
       iRazonEstadoConciliacionOportunidad = @razon_estado_cierre, iEstadoConciliacionProducto = 100000001,
       uOpportunityCloseId = @ActivityId, sOperacionProducto = @sNumOperacion
       where uOpportunityId = @OpportunityId
    end
    
    fetch OportunidadesConciliarOnBase into @iCodigoClienteBanco,@sNumOperacion,@OpportunityId,@statecode,@statuscode,@estado_cierre,
                                                      @FechaReferencia, @MontoOportunidad, @OpportunityProductId, @efk_numero_operacion, @FechaMaximaConciliacionProducto
End
Close OportunidadesConciliarOnBase                                                  
Deallocate OportunidadesConciliarOnBase

drop table #TMP_OpportunityClose
drop table #TMP_OportunidadesConciliar
drop table #TMP_OportunidadesConciliarOnBase

select * from BDI_CRM_DatosConciliacion
GO
