using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DB
{
    public class Pago
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int PagoId { get; set; }
        public int ClienteId { get; set; }
        [ForeignKey("ClienteId")]
        public virtual Cliente? Clientes { get; set; }
        public string? MontoPagado { get; set; }
        public int Aplicado { get; set; }
        public DateTime FechaPago { get; set; }

        public static string SumarMontos(List<string?> montos)
        {

            float totalMontos = 0;

            foreach (var monto in montos)
            {
                if(monto is not null)
                {
                    var split = monto.Split("$");
                    totalMontos += float.Parse(split[1]);
                }
            }

            return "$" + totalMontos.ToString("0.00");
        }

    }
}
