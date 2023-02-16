using DB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public IEnumerable<Cliente> Get()
        {
            return _context.Clientes.ToList();

        }

        [HttpPut]
        public IActionResult ActionCleanName()
        {
            try
            {

                foreach (var item in _context.Clientes.ToList())
                {
                    if (!String.IsNullOrEmpty(item.Nombre))
                    {
                        item.Nombre = Cliente.CleanName(item.Nombre);
                        _context.Clientes.Update(item);
                        _context.SaveChanges();
                    }
                }

                var montosPagados = _context.Pagos.Where(c => c.ClienteId == 8).Select(m => m.MontoPagado).ToList();
                Ajuste clienteAjuste = _context.Ajustes.Where(m => m.ClienteId == 8).First();

                // 2

                if (montosPagados is not null)
                {

                    clienteAjuste.MontoTotal = Pago.SumarMontos(montosPagados);
                    _context.Ajustes.Update(clienteAjuste);
                    _context.SaveChanges();
                }

                // 3 

                Ajuste clienteAjuste2 = _context.Ajustes.Include(c => c.Clientes).Where(c => c.ClienteId == 8).First();

                if (clienteAjuste2.Clientes is not null)
                {
                    if (clienteAjuste2.Clientes.MontoSolicitud is not null && clienteAjuste2.MontoTotal is not null)
                    {

                        clienteAjuste2.Adeudo = Ajuste.CalcularAdeudo(clienteAjuste2.Clientes.MontoSolicitud, clienteAjuste2.MontoTotal);

                        _context.Ajustes.Update(clienteAjuste2);
                        _context.SaveChanges();

                    }
                }





                // 4

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
                        else
                        {
                            cliente.Estatus = "Cancelación";
                        }

                        _context.Clientes.Update(cliente);
                        _context.SaveChanges();

                    }
                }

                // 5






                return StatusCode(StatusCodes.Status200OK, new { message = "Ok" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status304NotModified, new { message = ex });
            }
        }
    }
}
