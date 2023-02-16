using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB
{
    public class Cliente
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ClienteId { get; set; }
        public string? Nombre { get; set; }
        public string? Telefono { get; set; }
        public string? Correo { get; set; }
        public int Edad { get; set; }
        public string? MontoSolicitud { get; set; }
        public string? Estatus { get; set; }
        public int Aprobación { get; set; }
        public DateTime FechaAlta { get; set; }
        public virtual ICollection<Pago>? Pagos { get; set; }
        public virtual ICollection<Ajuste>? Ajustes { get; set; }

        public static string CleanName(string Nombre)
        {
            List<string> nombres = new();
            var splitNombre = Nombre.Split(" ");

            foreach (var item in splitNombre)
            {
                if (!String.IsNullOrEmpty(item))
                {
                    nombres.Add(item);
                }
            }

            return $"{nombres[0].Trim()} {nombres[1].Trim()} {nombres[2].Trim()}";
        }
    }
}
