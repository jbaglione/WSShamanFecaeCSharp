using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WSShamanFECAE_CSharp.Models.RecetaModels
{
    public class ClientesRecetas
    {
        public long Id { get; set; }
        public string ClienteId { get; set; }
        public string RazonSocial { get; set; }
    }
}