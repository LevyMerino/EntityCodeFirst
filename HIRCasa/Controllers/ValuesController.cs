using DB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HIRCasa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private HirCasaContext _context;

        public ValuesController(HirCasaContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<Cliente> Get()
        {
            return _context.Clientes.ToList();

        }

        //[HttpPut]
        //public ActionResult LimpiarNombres()
        //{
        //    return Json("ok");

        //}

    }
}
