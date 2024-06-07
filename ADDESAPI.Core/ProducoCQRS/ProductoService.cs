using ADDESAPI.Core.DespachosCQRS;
using ADDESAPI.Core.GTCQRS;
using ADDESAPI.Core.ImpuestoCQRS;
using ADDESAPI.Core.ProducoCQRS.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADDESAPI.Core.ProducoCQRS
{
    public class ProductoService : IProductoService
    {

        private readonly IProductoResource _resource;
        private readonly IGTResource _resourceGT;
        private readonly IImpuestoResource _resourceImpuesto;
        private readonly IDespachosResource _resourceDespacho;
        public ProductoService(IProductoResource resource, IGTResource resourceGT, IImpuestoResource resourceImpuesto, IDespachosResource resourceDespacho)
        {
            _resource = resource;
            _resourceGT = resourceGT;
            _resourceImpuesto = resourceImpuesto;
            _resourceDespacho = resourceDespacho;
        }

        public async Task<ResultMultiple<FamiliaDTO>> GetFamilias()
        {
            ResultMultiple<FamiliaDTO> Result = new ResultMultiple<FamiliaDTO>();
            try
            {
                var ResultToken = await _resourceGT.GetToken();
                if (!ResultToken.Success)
                {
                    Result.Success = ResultToken.Success;
                    Result.Error = ResultToken.Error;
                    Result.Message = ResultToken.Message;
                    return Result;
                }
                string token = ResultToken.Data;

                var ResultFamilias = await _resource.GetFamilias(token);
                if (!ResultFamilias.Success)
                {
                    Result.Success = false;
                    Result.Error = ResultFamilias.Error;
                    Result.Message = ResultFamilias.Message;
                    return Result;
                }
                var familias = ResultFamilias.Data;
                Result.Success = true;
                Result.Error = "";
                Result.Message = "";
                Result.Data = familias.Select(x => new FamiliaDTO { Codigo = x.cod, Descripcion = x.den}).ToList();
            }
            catch (Exception ex)
            {
                Result.Success = false;
                Result.Error = "";
                Result.Message = ex.Message;
            }
            return Result;
        }
        public async Task<ResultMultiple<ProductoDTO>> GetProductosFamilia(ProductoReqDTO req)
        {
            ResultMultiple<ProductoDTO> Result = new ResultMultiple<ProductoDTO>();
            try
            {
                Validator validator = new Validator();
                List<Validate> valuesToValidate = new List<Validate>();
                valuesToValidate.Add(new Validate { DataType = typeof(int), ParameterName = "Familia", Value = req.Familia.ToString() });

                Result ResultValidate = validator.GetValidate(valuesToValidate);
                if (!ResultValidate.Success)
                {
                    Result.Success = ResultValidate.Success;
                    Result.Error = ResultValidate.Error;
                    Result.Message = ResultValidate.Message;
                    return Result;
                }

                var ResultToken = await _resourceGT.GetToken();
                if (!ResultToken.Success)
                {
                    Result.Success = ResultToken.Success;
                    Result.Error = ResultToken.Error;
                    Result.Message = ResultToken.Message;
                    return Result;
                }
                string token = ResultToken.Data;

                var ResultProductos = await _resource.GetProductosFamilia(token, req.Familia);
                if (!ResultProductos.Success)
                {
                    Result.Success = false;
                    Result.Error = ResultProductos.Error;
                    Result.Message = ResultProductos.Message;
                    return Result;
                }
                var productos = ResultProductos.Data;
                Result.Success = true;
                Result.Error = "";
                Result.Message = "";

                Result.Data = productos.Where(x => x.precioProducto != null).Select(x => new ProductoDTO { 
                    Codigo = x.codigo, 
                    Descripcion = x.descripcion, 
                    Unidad = x.unidad,
                    CodigoExterno = x.codigoExterno,
                    CodigoBarras = x.codigoBarras,
                    Habilitado = x.habilitado,
                    Precio = x.precioProducto.precioVenta,
                    TasaIVA = x.precioProducto.tasaIVA,
                    IdFamilia = req.Familia
                }).ToList();
            }
            catch (Exception ex)
            {
                Result.Success = false;
                Result.Error = "";
                Result.Message = ex.Message;
            }
            return Result;
        }
        public async Task<ResultSingle<ProductoDTO>> GetProductoCB(ProductoCodigoReqDTO req)
        {
            ResultSingle<ProductoDTO> Result = new ResultSingle<ProductoDTO>();
            try
            {
                Validator validator = new Validator();
                List<Validate> valuesToValidate = new List<Validate>();
                valuesToValidate.Add(new Validate { DataType = typeof(string), ParameterName = "Codigo", Value = req.Codigo.ToString() });

                Result ResultValidate = validator.GetValidate(valuesToValidate);
                if (!ResultValidate.Success)
                {
                    Result.Success = ResultValidate.Success;
                    Result.Error = ResultValidate.Error;
                    Result.Message = ResultValidate.Message;
                    return Result;
                }

                var ResultToken = await _resourceGT.GetToken();
                if (!ResultToken.Success)
                {
                    Result.Success = ResultToken.Success;
                    Result.Error = ResultToken.Error;
                    Result.Message = ResultToken.Message;
                    return Result;
                }
                string token = ResultToken.Data;

                var ResultProductos = await _resource.GetProductoCB(token, req.Codigo);
                if (!ResultProductos.Success)
                {
                    Result.Success = false;
                    Result.Error = ResultProductos.Error;
                    Result.Message = ResultProductos.Message;
                    return Result;
                }
                var producto = ResultProductos.Data;

                var ResultImpuesto = await _resourceImpuesto.GetImpuestoProducto(producto.Codigo);
                if (!ResultImpuesto.Success)
                {
                    Result.Success = false;
                    Result.Error = "Error";
                    Result.Message = "No se encontro el precio para el producto";
                    return Result;
                }

                Result.Success = true;
                Result.Error = "";
                Result.Message = "";
                Result.Data =  new ProductoDTO
                {
                    Codigo = producto.Codigo,
                    Descripcion = producto.Descripcion,
                    Unidad = producto.Unidad,
                    CodigoExterno = producto.CodigoExterno,
                    CodigoBarras = producto.CodigoBarras,
                    Habilitado = producto.Habilitado,
                    IdFamilia = producto.IdFamilia,
                    Precio = ResultImpuesto.Data.Precio,
                    TasaIVA = ResultImpuesto.Data.TasaIVA
                };
            }
            catch (Exception ex)
            {
                Result.Success = false;
                Result.Error = "";
                Result.Message = ex.Message;
            }
            return Result;
        }
        public async Task<Result> SetProductoTicket(ProductoRequestDTO req)
        {
            Result Result = new Result();
            ProductosGTDTO Productos = new ProductosGTDTO();
            Productos.products = new List<ProductoGTDTO>();
            try
            {
                var ResultToken = await _resourceGT.GetToken();
                if (!ResultToken.Success)
                {
                    Result.Success = ResultToken.Success;
                    Result.Error = ResultToken.Error;
                    Result.Message = ResultToken.Message;
                    return Result;
                }
                string token = ResultToken.Data;                

                foreach (var item in req.Productos)
                {
                    ProductoGTDTO p = new ProductoGTDTO();
                    p.amount = item.Cantidad;
                    p.total = item.Total;
                    p.product = new ProductoApiGT { 
                        indoct = 0,
                        codigo = item.Codigo,
                        descripcion = item.Descripcion,
                        unidad = item.Unidad,
                        codigoExterno = item.CodigoExterno,
                        codigoBarras = "",
                        codigoBarrasAlterno = "",
                        habilitado = 1,
                        externo = 0,
                        precioProducto = new PrecioGT { precioVenta = item.Precio, tasaIVA = item.TasaIVA, costoVenta = 0, cuotaIEPS = 0 }
                    };
                    Productos.products.Add(p);
                }

                Productos.detailsPayment = null;
                if (req.TipoPago == 49)
                {
                    Productos.tipoPago = 0;
                }
                else
                {
                    switch (req.TipoPago)
                    {
                        case 51:
                            Productos.tipoPago = 3;
                            break;
                        case 52:
                            Productos.tipoPago = 4;
                            break;
                        case 53:
                            Productos.tipoPago = 7;
                            break;
                        case 65:
                            Productos.tipoPago = 9;
                            break;
                        case 74:
                            Productos.tipoPago = 8;
                            break;
                    }
                    
                }

                string jsonString = JsonConvert.SerializeObject(Productos);

                var ResultProducto = await _resource.SetProductoTicket(token, req.Bomba, jsonString, req.NoEmpleado);
                Result.Success = ResultProducto.Success;
                Result.Error = ResultProducto.Error;
                Result.Message = ResultProducto.Message;
            }
            catch (Exception ex)
            {
                Result.Success = false;
                Result.Error = "";
                Result.Message = ex.Message;
            }
            return Result;
        }
        public async Task<ResultSingle<int>> SetProducto(ProductoRequestDTO req)
        {
            ResultSingle<int> Result = new ResultSingle<int>();
            ProductosGTDTO Productos = new ProductosGTDTO();
            Productos.products = new List<ProductoGTDTO>();
            try
            {
                var ResultToken = await _resourceGT.GetToken();
                if (!ResultToken.Success)
                {
                    Result.Success = ResultToken.Success;
                    Result.Error = ResultToken.Error;
                    Result.Message = ResultToken.Message;
                    return Result;
                }
                string token = ResultToken.Data;

                foreach (var item in req.Productos)
                {
                    ProductoGTDTO p = new ProductoGTDTO();
                    p.amount = item.Cantidad;
                    p.total = item.Total;
                    p.product = new ProductoApiGT
                    {
                        indoct = 0,
                        codigo = item.Codigo,
                        descripcion = item.Descripcion,
                        unidad = item.Unidad,
                        codigoExterno = item.CodigoExterno,
                        codigoBarras = "",
                        codigoBarrasAlterno = "",
                        habilitado = 1,
                        externo = 0,
                        precioProducto = new PrecioGT { precioVenta = item.Precio, tasaIVA = item.TasaIVA, costoVenta = 0, cuotaIEPS = 0 }
                    };
                    Productos.products.Add(p);
                }

                Productos.detailsPayment = null;
                if (req.TipoPago == 49)
                {
                    Productos.tipoPago = 0;
                }
                else
                {
                    switch (req.TipoPago)
                    {
                        case 51:
                            Productos.tipoPago = 3;
                            Productos.detailsPayment = new DetailsPayment { AuthNumber = "", Card = "" };
                            break;
                        case 52:
                            Productos.tipoPago = 4;
                            Productos.detailsPayment = new DetailsPayment { AuthNumber = "", Card = "" };
                            break;
                        case 53:
                            Productos.tipoPago = 7;
                            Productos.detailsPayment = new DetailsPayment { AuthNumber = "", Card = "" };
                            break;
                        case 65:
                            Productos.tipoPago = 9;
                            break;
                        case 74:
                            Productos.tipoPago = 8;
                            break;
                    }
                }

                string jsonString = JsonConvert.SerializeObject(Productos);

                var ResultProducto = await _resource.SetProducto(token, req.Bomba, jsonString, req.NoEmpleado);
                Result.Success = ResultProducto.Success;
                Result.Error = ResultProducto.Error;
                Result.Message = ResultProducto.Message;
                if (ResultProducto.Success)
                {
                    Result.Data = ResultProducto.Data.folio;
                }
                
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
