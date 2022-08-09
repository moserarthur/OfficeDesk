using DAL;
using MetaDados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL
{
    public static class ClienteBAL
    {
        /// <summary>
        /// Ligação para a função InsertClient do banco de dados
        /// </summary>
        /// <param name="cli"></param>
        /// <returns>Response</returns>
        public static Response InsertClient(Cliente cli)
        {
            //-----Tratamento de dados-----//
            cli.data_nascimento = Checker.StringCleaner(cli.data_nascimento);
            cli.rg = Checker.StringCleaner(cli.rg);
            cli.cpf = Checker.StringCleaner(cli.cpf);
            cli.telefone_1 = Checker.StringCleaner(cli.telefone_1);
            cli.telefone_2 = Checker.StringCleaner(cli.telefone_2);
            cli.cep = Checker.StringCleaner(cli.cep);

            Response resp = ClienteDB.InsertClient(cli);
            return resp;

        }
        //public static List<Cliente> SelectListNameClient(string nome)
        //{
        //    List<Cliente> list_client = new List<Cliente>();

        //    Response resp = ClienteDB.SelectListClient(out list_client, id_cliente);
        //    return list_client;
        //}
        /// <summary>
        /// Ligação para a função SelectListClient do banco de dados
        /// </summary>
        /// <param name="id_cliente"></param>
        /// <returns>Response</returns>
        public static List<Cliente> SelectListClient(int id_cliente)
        {
            List<Cliente> list_client = new List<Cliente>();

            Response resp = ClienteDB.SelectListClient(out list_client, id_cliente);
            return list_client;
        }

        /// <summary>
        /// Ligação para a função DeleteClient do banco de dados
        /// </summary>
        /// <param name="id_cliente"></param>
        /// <returns>Response</returns>
        public static Response DeleteClient(int id_cliente)
        {
            Response resp = ClienteDB.DeleteClient(id_cliente);
            return resp;

        }
    }
}
