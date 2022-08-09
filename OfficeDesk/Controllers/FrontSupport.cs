using BAL;
using DAL;
using MetaDados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VetOnTrack.Controllers
{
    public static class FrontSupport
    {
        public static List<Cliente> SelectListClient()
        {


            List<Cliente> lista = new List<Cliente>();

            lista = BAL.ClienteBAL.SelectListClient(0);

            return lista;
        }

        public static List<Cliente> SelectListClient(int id_cliente)
        {


            List<Cliente> lista = new List<Cliente>();

            lista = BAL.ClienteBAL.SelectListClient(id_cliente);

            return lista;
        }
    }
}