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

                if (montosPagados is not null)
                {

                    clienteAjuste.MontoTotal = Pago.SumarMontos(montosPagados);
                    _context.Ajustes.Update(clienteAjuste);
                    _context.SaveChanges();
                }

                return StatusCode(StatusCodes.Status200OK, new { message = "Ok" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status304NotModified, new { message = ex });
            }
        }
    }
}
