using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MimicAPI.V1.Models.DTO
{
    public class LinkDTO
    {
        public string Legenda { get; set; }
        public string Href { get; set; }
        public string Metodo { get; set; }

        public LinkDTO(string legenda, string href, string metodo)
        {
            Legenda = legenda;
            Href = href;
            Metodo = metodo;
        }
    }
}
