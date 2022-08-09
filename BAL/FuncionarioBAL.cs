using DAL;
using MetaDados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL
{
    public static class FuncionarioBAL
    {
        /// <summary>
        /// Ligação para a função InsertEmployee do banco de dados
        /// </summary>
        /// <param name="func"></param>
        /// <returns>Response</returns>
        public static Response InsertEmployee(Funcionario func)
        {
            func.data_nascimento = Checker.StringCleaner(func.data_nascimento);
            func.rg = Checker.StringCleaner(func.rg);
            func.cpf = Checker.StringCleaner(func.cpf);
            func.telefone_1 = Checker.StringCleaner(func.telefone_1);
            func.telefone_2 = Checker.StringCleaner(func.telefone_2);
            func.cep = Checker.StringCleaner(func.cep);

            Response resp = FuncionarioDB.InsertEmployee(func);
            return resp;

        }
        public static Response UpdateEmployee(Funcionario func, string novaSenha, string confSenha)
        {
            Response res = new Response();
            List<string> campos = new List<string>() { func.id_funcionario.ToString(), func.telefone_1, func.rua, func.numero_residencia.ToString(), func.bairro, func.estado, func.cidade, func.cep, func.complemento };

            if (novaSenha != null && novaSenha != "")
            {
                if (novaSenha.Equals(confSenha))
                {
                    func.senha = novaSenha;
                    campos.Add(func.senha);
                }
                else
                {
                    return new Response() { Executed = false, ErrorMessage = "As senha devem ser iguais" };
                }

            }

            if (func.email != null)
            {
                campos.Add(func.email);
            }

            if (func.telefone_2 != null)
            {
                campos.Add(func.telefone_2);
            }

            if (!Checker.CheckerNullOrEmpty(campos))
            {
                res = FuncionarioDB.UpdateEmployee(func);

                if (res.Executed)
                {


                    res.Executed = true;
                }
                else
                {
                    res.Executed = false;
                }
            }
            else
            {
                res.Executed = false;
                res.ErrorMessage = "Campos não podem ser vazios";
            }

            return res;
        }

        public static Response SelectProfileEmployee(int id_funcionario, out Funcionario func)
        {
            func = new Funcionario();

            Response res = FuncionarioDB.SelectProfileEmployee(id_funcionario, out func);

            if (res.Executed)
            {
                return new Response()
                {
                    Executed = true
                };
            }
            else
            {
                return res;
            }

        }
    }
}

