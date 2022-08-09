using MetaDados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace BAL
{
    public static class AcessoBAL
    {
        /// <summary>
        /// Verifica se é um acesso válido ou não
        /// </summary>
        /// <param name="acesso"></param>
        /// <param name="acesso1"></param>
        /// <returns>Response</returns>
        public static Response CheckAccess(Acesso acesso, out Acesso acesso1, out int idFuncionario)
        {
            idFuncionario = 0;
            acesso1 = new Acesso();
            Response resp = new Response();
            if (!Checker.CheckerNullOrEmpty(new List<string> { acesso.email }))
            {
                if (!Checker.CheckerNullOrEmpty(new List<string> { acesso.senha }))
                {
                    resp = GenericDB.CheckAccess(acesso, out acesso1, out idFuncionario);
                }
                else
                {
                    //Comando não executado 
                    resp.Executed = true;
                    resp.ErrorMessage = "Senha Incorreta";
                }
            }
            else
            {
                //Comando não executado 
                resp.Executed = true;
                resp.ErrorMessage = "Usuario Incorreto";
            }
            return resp;
        }
    }
}
