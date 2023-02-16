using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB
{
    public class Pago
    {
        [Key]
        public int PagoId { get; set; }
        public int ClienteId { get; set; }
        [ForeignKey("ClienteId")]
        public virtual Cliente? Clientes { get; set; }
        public string? MontoPagado { get; set; }
        public int Aplicado { get; set; }
        public DateTime FechaPago { get; set; }
    }
}
