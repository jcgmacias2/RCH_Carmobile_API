using ADDESAPI.Core.DespachosCQRS.DTO;
using ADDESAPI.Core.GTCQRS;
using ADDESAPI.Core.ImpuestoCQRS;
using ADDESAPI.Core.ImpuestoCQRS.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADDESAPI.Core.DespachosCQRS
{
    public class DespachosService : IDespachosService
    {
        private readonly IDespachosResource _resource;
        private readonly IImpuestoResource _resourceImpuesto;
        private readonly IGTResource _resourceGT;
        public DespachosService(IDespachosResource despachosResource, IImpuestoResource resourceImpuesto, IGTResource resourceGT)
        {
            _resource = despachosResource;
            _resourceImpuesto = resourceImpuesto;
            _resourceGT = resourceGT;
        }

        //public async Task<ResultMultiple<DespachoDTO>> GetDespachos(RequestTransaccionesDTO request)
        //{
        //    ResultMultiple <DespachoDTO> Result = new ResultMultiple<DespachoDTO> ();
        //    List<DespachoDTO> DespachosDTO = new List<DespachoDTO> ();
        //    try
        //    {
        //        Validator validator = new Validator();
        //        List<Validate> valuesToValidate = new List<Validate>();
        //        valuesToValidate.Add(new Validate { DataType = typeof(int), ParameterName = "Bomba", Value = request.Bomba.ToString() });

        //        Result ResultValidate = validator.GetValidate(valuesToValidate);
        //        if (!ResultValidate.Success)
        //        {
        //            Result.Success = ResultValidate.Success;
        //            Result.Error = ResultValidate.Error;
        //            Result.Message = ResultValidate.Message;
        //            return Result;
        //        }

        //        var ResultDespachos = await _resource.GetDespachos(int.Parse(request.Bomba));
        //        if (!ResultDespachos.Success)
        //        {
        //            Result.Success = ResultDespachos.Success;
        //            Result.Error = ResultDespachos.Error;
        //            Result.Message = ResultDespachos.Message;
        //            return Result;
        //        }
        //        var despachos = ResultDespachos.Data;
        //        foreach (var despacho in despachos)
        //        {
        //            var d = await GetDespachoByTransacion(new RequestTransaccionDTO { Transaccion = despacho.Transaccion.ToString() });
        //            if (d.Success)
        //            {
        //                d.Data.WebID = String.Format("{0:X}", despacho.Transaccion.GetHashCode());
        //                DespachosDTO.Add(d.Data);
        //            }
        //        }
        //        Result.Success = true;
        //        Result.Error = "";
        //        Result.Message = "";
        //        Result.Data = DespachosDTO.ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        Result.Success = false;
        //        Result.Error = "Excepcion";
        //        Result.Message = ex.Message;
        //    }
        //    return Result;
        //}
        public async Task<ResultMultiple<DespachoAppDTO>> GetDespachos(RequestTransaccionesDTO request)
        {
            ResultMultiple<DespachoAppDTO> Result = new ResultMultiple<DespachoAppDTO>();

            try
            {
                Validator validator = new Validator();
                List<Validate> valuesToValidate = new List<Validate>();
                valuesToValidate.Add(new Validate { DataType = typeof(int), ParameterName = "Bomba", Value = request.Bomba.ToString() });

                Result ResultValidate = validator.GetValidate(valuesToValidate);
                if (!ResultValidate.Success)
                {
                    Result.Success = ResultValidate.Success;
                    Result.Error = ResultValidate.Error;
                    Result.Message = ResultValidate.Message;
                    return Result;
                }

                var ResultDespachos = await _resource.GetDespachos(int.Parse(request.Bomba));
                if (!ResultDespachos.Success)
                {
                    Result.Success = ResultDespachos.Success;
                    Result.Error = ResultDespachos.Error;
                    Result.Message = ResultDespachos.Message;
                    return Result;
                }
                var Despachos = ResultDespachos.Data;

                Despachos.ToList().ForEach(d => { d.Transaccion = $"{d.Transaccion}0"; });
                Result.Success = true;
                Result.Data = Despachos;

            }
            catch (Exception ex)
            {
                Result.Success = false;
                Result.Error = "Excepcion";
                Result.Message = ex.Message;
            }
            return Result;
        }
        public async Task<ResultSingle<DespachoDTO>> GetDespachoByTransaccion(RequestTransaccionDTO req)
        {
            ResultSingle<DespachoDTO> Result = new ResultSingle<DespachoDTO>();
            DespachoDTO Despacho = new DespachoDTO();
            try
            {
                Validator validator = new Validator();
                List<Validate> valuesToValidate = new List<Validate>();
                valuesToValidate.Add(new Validate { DataType = typeof(int), ParameterName = "Transaccion", Value = req.Transaccion.ToString() });

                Result ResultValidate = validator.GetValidate(valuesToValidate);
                if (!ResultValidate.Success)
                {
                    Result.Success = ResultValidate.Success;
                    Result.Error = ResultValidate.Error;
                    Result.Message = ResultValidate.Message;
                    return Result;
                }

                double subtotal = 0, iva = 0, ieps = 0;
                double tasaIVA = 0, cuotaIEPS = 0;

                int transaccion  = int.Parse(req.Transaccion.ToString().Remove(req.Transaccion.ToString().Length - 1));
                

                var ResultDespacho = await _resource.GetDespacho(transaccion);

                if (!ResultDespacho.Success)
                {
                    Result.Success = ResultDespacho.Success;
                    Result.Error = ResultDespacho.Error;
                    Result.Message = ResultDespacho.Message;
                    return Result;
                }

                var d = ResultDespacho.Data;

                Despacho.Transaccion = int.Parse(req.Transaccion);
                Despacho.Gasolinera = d.Gasolinera;
                Despacho.NoEstacion = d.NoEstacion;
                Despacho.Estacion = d.Estacion;
                Despacho.Ticket = $"{d.NoEstacion.ToString().PadLeft(5, '0')}{req.Transaccion}";
                Despacho.RFC = d.RFC;
                Despacho.RazonSocial = d.RazonSocial;
                Despacho.Direccion = d.Direccion;
                Despacho.DomicilioFiscal = d.DomicilioFiscal;
                Despacho.PermisoCre = d.PermisoCRE;
                Despacho.Turno = d.Turno;
                Despacho.FechaCG = d.FechaCG;
                Despacho.FchCor = d.FchCor;
                Despacho.Fecha = d.Fecha;
                Despacho.FechaCorte = d.FechaCorte;
                Despacho.Hora = d.Hora;
                Despacho.Bomba = d.Bomba;
                Despacho.IslaID = d.IslaID;
                Despacho.Total = d.Total;
                Despacho.IdTipoPago = d.IdTipoPago;
                Despacho.TipoPago = d.TipoPago;
                Despacho.FormaPagoSAT = d.FormaPagoSAT;
                Despacho.WebID = String.Format("{0:X}", d.Transaccion.GetHashCode());
                Despacho.NoEmpleado = d.NoEmpleado;
                Despacho.Vendedor = d.Vendedor;
                Despacho.Detalle = new List<DespachoDetalleDTO>();

                var ResultDetalle = await _resource.GetDespachoDetalle(transaccion);
                if (!ResultDetalle.Success)
                {
                    Result.Success = ResultDetalle.Success;
                    Result.Error = ResultDetalle.Error;
                    Result.Message = ResultDetalle.Message;
                    return Result;
                }
                var detalles = ResultDetalle.Data;
                foreach (var producto in detalles)
                {
                    if (producto.Producto > 0)
                    {
                        DespachoDetalleDTO detalle = new DespachoDetalleDTO();
                        detalle.Producto = producto.Producto;
                        detalle.Cantidad = producto.Cantidad;
                        detalle.Descripcion = producto.Descripcion;
                        detalle.Precio = producto.Precio;
                        detalle.Total = producto.Total;
                        detalle.Transaccion = producto.Transaccion;
                        detalle.Unidad = producto.Unidad;
                        detalle.ClaveProdServ = producto.ClaveProdServ;
                        detalle.ClaveUnidad = producto.ClaveUnidad;

                        var ResultImpuesto = await _resourceImpuesto.GetImpuestoProducto(Despacho.FechaCG, detalle.Producto);
                        if (!ResultImpuesto.Success)
                        {
                            Result.Success = ResultImpuesto.Success;
                            Result.Error = ResultImpuesto.Error;
                            Result.Message = ResultImpuesto.Message;
                            return Result;
                        }
                        var impuesto = ResultImpuesto.Data;
                        iva = 0; ieps = 0;
                        tasaIVA = impuesto.TasaIVA;
                        cuotaIEPS = impuesto.CuotaIEPS;
                        detalle.TasaIVA = impuesto.TasaIVA;
                        detalle.CuotaIEPS = impuesto.CuotaIEPS;

                        if (impuesto.CuotaIEPS > 0)
                        {
                            iva = Math.Round((producto.Total - (producto.Cantidad * cuotaIEPS) - ((producto.Total / producto.Precio * (((producto.Precio - cuotaIEPS) / (tasaIVA + 1)) * tasaIVA)))) * tasaIVA, 2);
                            ieps = Math.Round(detalle.Cantidad * double.Parse(impuesto.CuotaIEPS.ToString()), 2);
                            detalle.IEPS = ieps;
                            detalle.IVA = iva;
                            detalle.Subtotal = Math.Round(detalle.Total - iva - ieps, 2);
                        }
                        else
                        {
                            subtotal = detalle.Total / (1 + tasaIVA);
                            subtotal = Math.Round(subtotal, 2);
                            iva = detalle.Total - subtotal;
                            iva = Math.Round(iva, 2);
                            detalle.IEPS = ieps;
                            detalle.IVA = iva;
                            detalle.Subtotal = subtotal;
                        }
                        detalle.ValorUnitario = Math.Round(detalle.Subtotal / producto.Cantidad, 4);
                        Despacho.Detalle.Add(detalle);
                    }
                }

                Despacho.Descuento = 0;
                Despacho.TipoPagoApp = 0;
                Despacho.VentaApp = false;

                var ResultDiscount = await _resourceGT.GetDescuento(req.Transaccion);
                if (ResultDiscount.Success)
                {
                    var Discount = ResultDiscount.Data;
                    Despacho.Descuento = Discount.Descuento;
                    Despacho.PromoDesc = Discount.PromoDesc;
                    Despacho.CardNumber = Discount.CardNumber;
                    Despacho.litrosRedimidos = Discount.litrosRedimidos;
                    Despacho.PromoCode = Discount.PromoCode;
                    Despacho.NombreCliente = Discount.NombreCliente;
                }

                Despacho.Total = Despacho.Detalle.Sum(t => t.Total);
                Despacho.IVA = Despacho.Detalle.Sum(t => t.IVA);
                Despacho.IEPS = Despacho.Detalle.Sum(t => t.IEPS);
                Despacho.Subtotal = Despacho.Detalle.Sum(t => t.Subtotal);

                Result.Data = Despacho;
                Result.Success = true;

            }
            catch (Exception ex)
            {
                Result.Success = false;
                Result.Error = "Excepcion";
                Result.Message = ex.Message;
            }
            return Result;
        }
        public async Task<Result> SetTipoPago(ReqTransaccionTpDTO requestDTO)
        {
            Result Result = new Result();
            try
            {
                Validator validator = new Validator();
                List<Validate> valuesToValidate = new List<Validate>();
                valuesToValidate.Add(new Validate { DataType = typeof(int), ParameterName = "Transaccion", Value = requestDTO.Transaccion.ToString() });
                valuesToValidate.Add(new Validate { DataType = typeof(int), ParameterName = "TipoPago", Value = requestDTO.TipoPago.ToString() });

                Result ResultValidate = validator.GetValidate(valuesToValidate);
                if (!ResultValidate.Success)
                {
                    Result.Success = ResultValidate.Success;
                    Result.Error = ResultValidate.Error;
                    Result.Message = ResultValidate.Message;
                    return Result;
                }

                Result = await _resource.SetTipoPago(requestDTO.Transaccion, requestDTO.TipoPago);
            }
            catch (Exception ex)
            {
                Result.Success = false;
                Result.Error = "Excepcion";
                Result.Message = ex.Message;
            }
            return Result;
        }
        public async Task<ResultSingle<RedemptionDTO>> Redemption(RedemptionReq req)
        {
            ResultSingle<RedemptionDTO> Result = new ResultSingle<RedemptionDTO>();
            try
            {
                Validator validator = new Validator();
                List<Validate> valuesToValidate = new List<Validate>();
                valuesToValidate.Add(new Validate { DataType = typeof(int), ParameterName = "Transacción", Value = req.Transaccion.ToString() });
                valuesToValidate.Add(new Validate { DataType = typeof(int), ParameterName = "Estación", Value = req.Estacion.ToString() });
                valuesToValidate.Add(new Validate { DataType = typeof(int), ParameterName = "Bomba", Value = req.Bomba.ToString() });
                valuesToValidate.Add(new Validate { DataType = typeof(string), ParameterName = "CardNumber", Value = req.CardNumber });
                valuesToValidate.Add(new Validate { DataType = typeof(int), ParameterName = "LitrosRedimir", Value = req.LitrosRedimir.ToString() });
                valuesToValidate.Add(new Validate { DataType = typeof(double), ParameterName = "Cantidad", Value = req.Cantidad.ToString() });
                valuesToValidate.Add(new Validate { DataType = typeof(double), ParameterName = "Total", Value = req.Total.ToString() });
                valuesToValidate.Add(new Validate { DataType = typeof(int), ParameterName = "NoEmpleado", Value = req.NoEmpleado.ToString() });
                valuesToValidate.Add(new Validate { DataType = typeof(string), ParameterName = "Vendedor", Value = req.Vendedor });
                valuesToValidate.Add(new Validate { DataType = typeof(double), ParameterName = "Descuento", Value = req.Descuento.ToString() });
                valuesToValidate.Add(new Validate { DataType = typeof(double), ParameterName = "Precio", Value = req.Precio.ToString() });
                valuesToValidate.Add(new Validate { DataType = typeof(string), ParameterName = "brandId", Value = req.BrandId });
                valuesToValidate.Add(new Validate { DataType = typeof(string), ParameterName = "ProgramId", Value = req.ProgramId });

                Result ResultValidate = validator.GetValidate(valuesToValidate);
                if (!ResultValidate.Success)
                {
                    Result.Success = ResultValidate.Success;
                    Result.Error = ResultValidate.Error;
                    Result.Message = ResultValidate.Message;
                    return Result;
                }
                var ResultRedemption = await _resource.Redemption(req);
                if (!ResultRedemption.Success)
                {
                    Result.Success = false;
                    Result.Error = ResultRedemption.Error;
                    Result.Message = ResultRedemption.Message;
                    return Result;
                }
                var Redemption = ResultRedemption.Data;
                Result.Success = true;
                Result.Error = "";
                Result.Message = ResultValidate.Message;
                Result.Data = Redemption;

                string promotion = "";

                var ResultToken = await _resourceGT.GetToken();
                if (!ResultToken.Success)
                {
                    promotion = "Se realizo la redención de saldo pero ocurrio un error al agregar el anticipo. Error al generar el login de GT";
                    //var ResultSetDiscount = await _resourceGT.SetDiscount(req.Transaccion, req.Descuento, req.CardNumber, promotion, req.Producto, req.LitrosRedimir, req.Cliente);
                    var ResultSetDiscount = await _resourceGT.SetDescuento(req.Transaccion, req.Descuento, req.CardNumber, promotion, req.Producto, req.LitrosRedimir, req.Cliente, req.NoEmpleado, req.Vendedor);
                    Result.Success = false;
                    Result.Error = "Error al egnerar el anticipo";
                    Result.Message = "Se realizo la redención de saldo pero ocurrio un error al agregar el anticipo";                    
                    return Result;
                }
                string token = ResultToken.Data;

                int codval = -269;
                if (req.IdTipoPago == 51)
                    codval = -267;
                else if (req.IdTipoPago == 52)
                    codval = -268;

                var data = new { fch = req.FechaCG, nrotur = req.Turno, codisl = req.IslaId, codres = req.NoEmpleado, fchcor = req.FchCor, codval = codval, can = req.Descuento, mto = req.Descuento };
                string jsonString = System.Text.Json.JsonSerializer.Serialize(data);

                var ResultAnticipo = await _resourceGT.AddAnticipo(token, jsonString);
                if (!ResultAnticipo.Success)
                {
                    promotion = $"Se realizo la redención de saldo pero ocurrio un error al agregar el anticipo. Respuesta: {ResultAnticipo.Error}. {ResultAnticipo.Message}";
                    
                    Result.Success = false;
                    Result.Error = "Error al generar el anticipo";
                    Result.Message = $"Se realizo la redención de saldo pero ocurrio un error al agregar el anticipo. {ResultAnticipo.Message}";
                    //var ResultSetDiscount = await _resourceGT.SetDiscount(req.Transaccion, req.Descuento, req.CardNumber, promotion, req.Producto, req.LitrosRedimir, req.Cliente);
                    var ResultSetDiscount = await _resourceGT.SetDescuento(req.Transaccion, req.Descuento, req.CardNumber, promotion, req.Producto, req.LitrosRedimir, req.Cliente, req.NoEmpleado, req.Vendedor);
                }
                else
                {
                    promotion = req.PromoDesc;
                    
                    Result.Success = true;
                    Result.Error = "";
                    Result.Message = "Descuento aplicado";
                    //var ResultSetDiscount = await _resourceGT.SetDiscount(req.Transaccion, req.Descuento, req.CardNumber, promotion, req.Producto, req.LitrosRedimir, req.Cliente);
                    var ResultSetDiscount = await _resourceGT.SetDescuento(req.Transaccion, req.Descuento, req.CardNumber, promotion, req.Producto, req.LitrosRedimir, req.Cliente, req.NoEmpleado, req.Vendedor);
                }
            }
            catch (Exception ex)
            {
                Result.Success = false;
                Result.Error = "Error al Redimir";
                Result.Message = ex.Message;
            }
            return Result;
        }
        public async Task<ResultSingle<RedemptionDTO>> RewardRedemption(RedemptionReq req)
        {
            ResultSingle<RedemptionDTO> Result = new ResultSingle<RedemptionDTO>();
            try
            {
                Validator validator = new Validator();
                List<Validate> valuesToValidate = new List<Validate>();
                valuesToValidate.Add(new Validate { DataType = typeof(int), ParameterName = "Transacción", Value = req.Transaccion.ToString() });
                valuesToValidate.Add(new Validate { DataType = typeof(int), ParameterName = "Estación", Value = req.Estacion.ToString() });
                valuesToValidate.Add(new Validate { DataType = typeof(int), ParameterName = "Bomba", Value = req.Bomba.ToString() });
                valuesToValidate.Add(new Validate { DataType = typeof(string), ParameterName = "CardNumber", Value = req.CardNumber });
                valuesToValidate.Add(new Validate { DataType = typeof(int), ParameterName = "LitrosRedimir", Value = req.LitrosRedimir.ToString() });
                valuesToValidate.Add(new Validate { DataType = typeof(double), ParameterName = "Cantidad", Value = req.Cantidad.ToString() });
                valuesToValidate.Add(new Validate { DataType = typeof(double), ParameterName = "Total", Value = req.Total.ToString() });
                valuesToValidate.Add(new Validate { DataType = typeof(int), ParameterName = "NoEmpleado", Value = req.NoEmpleado.ToString() });
                valuesToValidate.Add(new Validate { DataType = typeof(string), ParameterName = "Vendedor", Value = req.Vendedor });
                valuesToValidate.Add(new Validate { DataType = typeof(double), ParameterName = "Descuento", Value = req.Descuento.ToString() });
                valuesToValidate.Add(new Validate { DataType = typeof(double), ParameterName = "Precio", Value = req.Precio.ToString() });
                valuesToValidate.Add(new Validate { DataType = typeof(string), ParameterName = "brandId", Value = req.BrandId });
                valuesToValidate.Add(new Validate { DataType = typeof(string), ParameterName = "ProgramId", Value = req.ProgramId });

                Result ResultValidate = validator.GetValidate(valuesToValidate);
                if (!ResultValidate.Success)
                {
                    Result.Success = ResultValidate.Success;
                    Result.Error = ResultValidate.Error;
                    Result.Message = ResultValidate.Message;
                    return Result;
                }
                var ResultImpuesto = await _resourceImpuesto.GetImpuestoProducto(req.Producto);
                if (!ResultImpuesto.Success)
                {
                    Result.Success = ResultImpuesto.Success;
                    Result.Error = "No se encontro el precio del producto";
                    Result.Message = ResultImpuesto.Message;
                    return Result;
                }
                var Impuesto = ResultImpuesto.Data;
                req.Precio = Impuesto.Precio;
                req.Descuento = Impuesto.Precio;

                var ResultRedemption = await _resource.RewardRedemption(req);
                if (!ResultRedemption.Success)
                {
                    Result.Success = false;
                    Result.Error = ResultRedemption.Error;
                    Result.Message = ResultRedemption.Message;
                    return Result;
                }
                var Redemption = ResultRedemption.Data;
                Result.Success = true;
                Result.Error = "";
                Result.Message = ResultValidate.Message;
                Result.Data = Redemption;

                var ResultSetDiscount = await _resourceGT.SetDescuento(req.Transaccion, req.Descuento, req.CardNumber, req.PromoDesc, req.Producto, req.LitrosRedimir, req.Cliente, req.NoEmpleado, req.Vendedor);

            }
            catch (Exception ex)
            {
                Result.Success = false;
                Result.Error = "Error al Redimir";
                Result.Message = ex.Message;
            }
            return Result;
        }
    }
}
