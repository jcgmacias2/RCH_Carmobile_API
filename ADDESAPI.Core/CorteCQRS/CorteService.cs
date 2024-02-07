﻿using ADDESAPI.Core.CorteCQRS.DTO;
using ADDESAPI.Core.DespachosCQRS;
using ADDESAPI.Core.FajillaCQRS;
using ADDESAPI.Core.GetnetCQRS;
using ADDESAPI.Core.TipoCambioDTO;
using ADDESAPI.Core.VentaCQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADDESAPI.Core.CorteCQRS
{
    public class CorteService : ICorteService
    {
        private readonly ICorteResource _resource;
        private readonly IVentaResource _resourceVenta;
        private readonly IDespachosResource _resourceDespachos;
        private readonly IFajillaResource _resourceFajilla;
        private readonly ITipoCambioResource _resourceTC; 
        private readonly IGetnetResource _resourceGetnet;
        public CorteService(ICorteResource resource, IVentaResource resourceVenta, IDespachosResource resourceDespachos, IFajillaResource resourceFajilla, ITipoCambioResource resourceTC, IGetnetResource resourceGetnet)
        {
            _resource = resource;
            _resourceVenta = resourceVenta;
            _resourceDespachos = resourceDespachos;
            _resourceFajilla = resourceFajilla;
            _resourceTC = resourceTC;
            _resourceGetnet = resourceGetnet;
        }
        public async Task<ResultSingle<CorteDTO>> GetCorteColaborador(RequestCorteDTO req)
        {
            ResultSingle<CorteDTO> Result = new ResultSingle<CorteDTO>();
            CorteDTO Corte = new CorteDTO();
            try
            {

                Validator validator = new Validator();
                List<Validate> valuesToValidate = new List<Validate>();
                valuesToValidate.Add(new Validate { DataType = typeof(string), ParameterName = "Fecha", Value = req.Fecha.ToString() });
                valuesToValidate.Add(new Validate { DataType = typeof(int), ParameterName = "NoEmpleado", Value = req.NoEmpleado.ToString() });
                valuesToValidate.Add(new Validate { DataType = typeof(int), ParameterName = "Bomba", Value = req.Bomba.ToString() });
                valuesToValidate.Add(new Validate { DataType = typeof(int), ParameterName = "Turno", Value = req.Turno.ToString() });

                Result ResultValidate = validator.GetValidate(valuesToValidate);
                if (!ResultValidate.Success)
                {
                    Result.Success = ResultValidate.Success;
                    Result.Error = ResultValidate.Error;
                    Result.Message = ResultValidate.Message;
                    return Result;
                }

                int turno = int.Parse(req.Turno.ToString() + "1");

                //var ResultVentas = await _resourceVenta.GetVentaTurno(req.Fecha, req.Turno, req.Bomba);
                var ResultVentas = await _resourceVenta.GetVentaDespachosTurno(req.Fecha, req.Turno, req.Bomba);
                if (!ResultVentas.Success)
                {
                    Result.Success = ResultVentas.Success;
                    Result.Error = ResultVentas.Error;
                    Result.Message = ResultVentas.Message;
                    return Result;
                }

                Corte.CantidadCombustible = ResultVentas.Data.Where(v => v.IdProducto == 62 || v.IdProducto == 63 || v.IdProducto == 64).Sum(v => v.Cantidad);
                Corte.ImporteCombustible = ResultVentas.Data.Where(v => v.IdProducto == 62 || v.IdProducto == 63 || v.IdProducto == 64).Sum(v => v.Total);
                Corte.CantidadProductos = ResultVentas.Data.Where(v => v.IdProducto != 62 && v.IdProducto != 63 && v.IdProducto != 64 && v.IdProducto != 0 && v.IdProducto != 65 && v.IdProducto != 66 && v.IdProducto != 67 && v.IdProducto != 68 && v.IdProducto != 69).Sum(v => v.Cantidad);
                Corte.ImporteProductos = ResultVentas.Data.Where(v => v.IdProducto != 62 && v.IdProducto != 63 && v.IdProducto != 64 && v.IdProducto != 0 && v.IdProducto != 65 && v.IdProducto != 66 && v.IdProducto != 67 && v.IdProducto != 68 && v.IdProducto != 69).Sum(v => v.Total);
                Corte.ImporteTotal = ResultVentas.Data.Where(v => v.IdProducto != 0 && v.IdProducto != 65 && v.IdProducto != 66 && v.IdProducto != 67 && v.IdProducto != 68 && v.IdProducto != 69).Sum(v => v.Total);

                var ResultFajilla = await _resourceFajilla.GetFajillasColaborador(req.Fecha, req.NoEmpleado, req.Estacion, req.Turno);
                if (ResultFajilla.Success)
                {
                    Corte.ImporteFajillasMXN = ResultFajilla.Data.Where(f => f.Moneda == "Pesos").Sum(f => f.Monto);
                    Corte.ImporteFajillasUSD = ResultFajilla.Data.Where(f => f.Moneda == "Dlls").Sum(f => f.Monto);
                }

                DateTime date = DateTime.ParseExact(req.Fecha, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                var ResulTC = await _resourceTC.GetTipoCambio(req.Estacion, req.Fecha);
                if (ResulTC.Success)
                {
                    Corte.TipoCambio = ResulTC.Data.TC;
                }

                var ResultGetnet = await _resourceGetnet.GetTransaccionesTurnoVendedor(date.ToString("dd/MM/yyyy"), req.Estacion, turno, req.NoEmpleado);
                if (ResultGetnet.Success)
                {
                    Corte.ImporteTDC = ResultGetnet.Data.Credito;
                    Corte.ImporteTDD = ResultGetnet.Data.Debito;
                    Corte.ImporteAMEX = ResultGetnet.Data.AMEX;
                    Corte.ImporteTarjetasDiesel = ResultGetnet.Data.Diesel;
                    Corte.ImporteDevolucionesTarjetas = ResultGetnet.Data.Devoluciones;
                }

                Result.Success = true;
                Result.Error = "";
                Result.Message = "";
                Result.Data = Corte;
            }
            catch (Exception ex)
            {
                Result.Success = false;
                Result.Error = "";
                Result.Message = ex.Message;
            }
            return Result;
        }
    }
}
