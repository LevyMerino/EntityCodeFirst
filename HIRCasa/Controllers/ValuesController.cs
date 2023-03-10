using DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace HIRCasa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly HirCasaContext _context;

        public ValuesController(HirCasaContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("TodosLosDatos")]
        public IEnumerable<Cliente> Get()
        {
            return _context.Clientes.Include(p => p.Pagos).Include(a => a.Ajustes).ToList();
        }

        //Punto 1
        [HttpPut]
        [Route("LimpiarNombres")]
        public IActionResult ActionCleanName()
        {
            try
            {
                foreach (var cliente in _context.Clientes.ToList())
                {
                    if (!String.IsNullOrEmpty(cliente.Nombre))
                    {
                        cliente.Nombre = Cliente.QuitarEspacios(cliente.Nombre); // El metodo QuitarEspacios se encuentra en la clase Cliente
                        _context.Clientes.Update(cliente);
                        _context.SaveChanges();
                    }
                }

                return StatusCode(StatusCodes.Status200OK, new
                {
                    message = "Ok",
                    data = _context.Clientes.Include(p => p.Pagos).Include(a => a.Ajustes).ToList() 
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status304NotModified, new { message = ex });
            }
        }

        //Punto 2
        [HttpPut]
        [Route("ActualizarMontoTotal")]
        public IActionResult MontoTotal(int ClienteId = 8)
        {
            try
            {
                List<string?> montosPagados = new();

                montosPagados = _context.Pagos.Where(c => c.ClienteId == ClienteId).Select(m => m.MontoPagado).ToList();

                if (!montosPagados.IsNullOrEmpty())
                {
                    Ajuste ajuste = _context.Ajustes.Where(m => m.ClienteId == ClienteId).First();

                    ajuste.MontoTotal = Pago.SumarMontos(montosPagados); // El metodo SumarMontos se encuentra en la clase Pago
                    _context.Ajustes.Update(ajuste);
                    _context.SaveChanges();
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new { message = "El cliente no tiene montos generados" });
                }

                return StatusCode(StatusCodes.Status200OK, new
                {
                    message = "Ok",
                    data = _context.Ajustes.Where(a => a.ClienteId == ClienteId)
                });

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status304NotModified, new { message = ex });
            }
        }

        //Punto 3
        [HttpPut]
        [Route("ActualizarAdeudo")]
        public IActionResult Adeudo(int ClienteId = 8)
        {
            try
            {
                Ajuste clienteAjuste = _context.Ajustes.Include(c => c.Clientes).Where(c => c.ClienteId == ClienteId).First();

                if (clienteAjuste.Clientes is not null)
                {
                    if (clienteAjuste.Clientes.MontoSolicitud is not null && clienteAjuste.MontoTotal is not null)
                    {

                        clienteAjuste.Adeudo = Ajuste.CalcularAdeudo(clienteAjuste.Clientes.MontoSolicitud, clienteAjuste.MontoTotal); // El metodo CalcularAdeudo se encuentra en la clase Ajuste como miembro estatico

                        _context.Ajustes.Update(clienteAjuste);
                        _context.SaveChanges();

                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Faltan datos para actualizar el adeudo" });
                    }
                }


                return StatusCode(StatusCodes.Status200OK, new { message = "Ok" });

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status304NotModified, new { message = ex });
            }
        }

        //Punto 4
        [HttpPut]
        [Route("ActualizarEstatus")]
        public IActionResult Estatus()
        {
            try
            {
                List<Cliente> clientes = _context.Clientes.Include(a => a.Ajustes).ToList();

                foreach (var cliente in clientes)
                {
                    if (cliente.Ajustes is not null)
                    {
                        Ajuste ajuste = cliente.Ajustes.First();
                        string? strAdeudo = ajuste.Adeudo;

                        if (!String.IsNullOrEmpty(strAdeudo))
                        {
                            if (float.Parse(strAdeudo.Split("$")[1]) < 0)
                            {
                                cliente.Estatus = "Adeudo";

                            }
                            else if (float.Parse(strAdeudo.Split("$")[1]) > 0)
                            {
                                cliente.Estatus = "Al corriente";
                            }
                            else
                            {
                                cliente.Estatus = "Cancelación";
                            }
                        }

                        _context.Clientes.Update(cliente);
                        _context.SaveChanges();

                    }
                }


                return StatusCode(StatusCodes.Status200OK, new
                {
                    message = "Ok",
                    data = _context.Clientes.ToList()
                });

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status304NotModified, new { message = ex });
            }
        }

        //Punto 5
        [HttpPut]
        [Route("ActualizarAprobacion")]
        public IActionResult Aprobacion()
        {
            try
            {
                List<Cliente> clientes = _context.Clientes.ToList();

                foreach (var cliente in clientes)
                {
                    if (!String.IsNullOrEmpty(cliente.Estatus))
                    {
                        if (cliente.Estatus.ToLower().Contains("adeudo") || cliente.Estatus.ToLower().Contains("corriente"))
                        {
                            cliente.Aprobación = 1;
                        }
                        else
                        {
                            cliente.Aprobación = 0;
                        }

                        _context.Clientes.Update(cliente);
                        _context.SaveChanges();

                    }
                }

                return StatusCode(StatusCodes.Status200OK, new
                {
                    message = "Ok",
                    data = _context.Clientes.ToList()
                });

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status304NotModified, new { message = ex });
            }
        }

    }
}
