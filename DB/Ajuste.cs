using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace DB
{
    public class Ajuste
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int AjusteId { get; set; }
        public int ClienteId { get; set; }
        [ForeignKey("ClienteId")]
        public virtual Cliente? Clientes { get; set; }
        public string? MontoTotal { get; set; }
        public string? Adeudo { get; set; }

        public static string CalcularAdeudo (string srtMontoSolicitado, string strMontoTotal)
        {
            float adeudo;
            float montoSolicitado;
            float montoTotal;
     
            montoTotal = float.Parse(strMontoTotal.Split("$")[1]);
            montoSolicitado = float.Parse(srtMontoSolicitado.Split("$")[1]);

            adeudo = montoSolicitado - montoTotal;

            return "$" + adeudo.ToString("0.00");
        }
    }
}
