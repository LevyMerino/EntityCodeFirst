using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB
{
    public class Ajuste
    {
        [Key]
        public int AjusteId { get; set; }
        public int ClienteId { get; set; }
        [ForeignKey("ClienteId")]
        public virtual Cliente? Clientes { get; set; }
        public string? MontoTotal { get; set; }
        public string? Adeudo { get; set; }
    }
}
