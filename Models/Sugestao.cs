using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PamTcc.Models
{
    public class Sugestao
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }
    }
}
