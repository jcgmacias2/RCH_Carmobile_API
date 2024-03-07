using ADDESAPI.Core.DespachosCQRS.DTO;
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

        public DespachosService(IDespachosResource despachosResource, IImpuestoResource resourceImpuesto)
        {
            _resource = despachosResource;
            _resourceImpuesto = resourceImpuesto;
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

                string despachos = "";
                foreach (var item in Despachos)
                {
                    despachos += $"{item.Transaccion},";
                }
                despachos = despachos.TrimEnd(',');
                var ResultDetalle = await _resource.GetDespachosApp(despachos);
                if (ResultDetalle.Success)
                {
                    foreach (var item in Despachos)
                    {
                        item.Detalle = new List<DespachoDetalleAppDTO>();
                        item.Detalle = ResultDetalle.Data.Where(i => i.Despacho == item.Despacho && i.Total > 0).ToList();
                        item.Total = ResultDetalle.Data.Where(i => i.Despacho == item.Despacho).Sum(i => i.Total);
                    }
                }
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
        public async Task<ResultSingle<DespachoDTO>> GetDespachoByTransacion(RequestTransaccionDTO requestDTO)
        {
            ResultSingle<DespachoDTO> Result = new ResultSingle<DespachoDTO>();
            DespachoDTO Despacho = new DespachoDTO();
            try
            {
                Validator validator = new Validator();
                List<Validate> valuesToValidate = new List<Validate>();
                valuesToValidate.Add(new Validate { DataType = typeof(int), ParameterName = "Transaccion", Value = requestDTO.Transaccion.ToString() });

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

                int transaccion  = int.Parse(requestDTO.Transaccion.ToString().Remove(requestDTO.Transaccion.ToString().Length - 1));
                

                var ResultDespacho = await _resource.GetDespacho(transaccion);

                if (!ResultDespacho.Success)
                {
                    Result.Success = ResultDespacho.Success;
                    Result.Error = ResultDespacho.Error;
                    Result.Message = ResultDespacho.Message;
                    return Result;
                }

                var d = ResultDespacho.Data;

                Despacho.Transaccion = int.Parse(requestDTO.Transaccion);
                Despacho.Gasolinera = d.Gasolinera;
                Despacho.NoEstacion = d.NoEstacion;
                Despacho.Estacion = d.Estacion;
                Despacho.Ticket = $"{d.NoEstacion.ToString().PadLeft(5, '0')}{requestDTO.Transaccion}";
                Despacho.RFC = d.RFC;
                Despacho.RazonSocial = d.RazonSocial;
                Despacho.Direccion = d.Direccion;
                Despacho.DomicilioFiscal = d.DomicilioFiscal;
                Despacho.PermisoCre = d.PermisoCRE;
                Despacho.Turno = d.Turno;
                Despacho.FechaCG = d.FechaCG;
                Despacho.Fecha = d.Fecha;
                Despacho.Hora = d.Hora;
                Despacho.Bomba = d.Bomba;
                Despacho.Total = d.Total;
                Despacho.IdTipoPago = d.IdTipoPago;
                Despacho.TipoPago = d.TipoPago;
                Despacho.FormaPagoSAT = d.FormaPagoSAT;
                Despacho.WebID = String.Format("{0:X}", d.Transaccion.GetHashCode());
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

                //if (Despacho.IdTipoPago == 51 || Despacho.IdTipoPago == 52)
                //{
                //    GenericResponseObject<PresetGATEWAY> ResultPreset = DespachoRepository.GetPresetGATEWAY(despacho);
                //    if (ResultPreset.Success)
                //    {
                //        PresetGATEWAY presetGATEWAY = ResultPreset.response;
                //        if (presetGATEWAY.QR != "")
                //        {
                //            Ticket.Descuento = presetGATEWAY.Descuento;
                //            Ticket.TipoPagoApp = presetGATEWAY.TipoPago;
                //            Ticket.VentaApp = true;
                //        }
                //    }
                //}

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
    }
}
