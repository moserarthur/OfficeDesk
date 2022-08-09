using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetaDados;

namespace DAL
{
    public static class FuncionarioDB
    {
        /// <summary>
        /// Insere no dbo.funcionario -> Objeto funcionario (id_pessoa_fk, salario, cargo, crmv)
        /// </summary>
        /// <param name="func"></param>
        /// <returns>Response</returns>
        public static Response InsertEmployee(Funcionario func)
        {
            Response resp = new Response();

            resp = GenericDB.InsertData(new Dado(func.nome, func.data_nascimento));
            func.id_dado = resp.ElementId;
            if (func.id_dado == -1)
            {
                resp = GenericDB.RollbackInsertsEmployee(func);
                if (resp.Executed)
                {
                    return new Response()
                    {
                        Executed = false,
                        ErrorMessage = "Erro",
                        Exception = resp.Exception

                    };
                }
                else
                {
                    return resp;
                }
            }

            resp = GenericDB.InsertAddress(new Endereco(func.rua, func.numero_residencia, func.bairro, func.estado, func.cidade, func.cep, func.complemento));
            func.id_endereco = resp.ElementId;
            if (func.id_endereco == -1)
            {
                resp = GenericDB.RollbackInsertsEmployee(func);
                if (resp.Executed)
                {
                    return new Response()
                    {
                        Executed = false,
                        ErrorMessage = "Erro",
                        Exception = resp.Exception

                    };
                }
                else
                {
                    return resp;
                }
            }

            resp = GenericDB.InsertAccess(new Acesso(func.email, func.senha, func.nivel));
            func.id_acesso = resp.ElementId;
            if (func.id_acesso == -1)
            {
                resp = GenericDB.RollbackInsertsEmployee(func);
                if (resp.Executed)
                {
                    return new Response()
                    {
                        Executed = false,
                        ErrorMessage = "Erro",
                        Exception = resp.Exception

                    };
                }
                else
                {
                    return resp;
                }
            }

            resp = GenericDB.InsertPersonEmployee(new Pessoa(func.rg,func.cpf, func.telefone_1, func.telefone_2), func);
            func.id_pessoa = resp.ElementId;
            if (func.id_pessoa == -1)
            {
                resp = GenericDB.RollbackInsertsEmployee(func);
                if (resp.Executed)
                {
                    return new Response()
                    {
                        Executed = false,
                        ErrorMessage = "Erro",
                        Exception = resp.Exception

                    };
                }
                else
                {
                    return resp;
                }
            }

            //
            string insert = "insert into dbo.funcionario (id_pessoa_fk,salario,cargo,crmv) values (@id_pessoa_fk,@salario,@cargo,@crmv)";
            resp = new Response();
            SqlCommand cmd = new SqlCommand(insert, ConnectionString.Connection);
            try
            {
                ConnectionString.Connection.Open();
                cmd.Parameters.AddWithValue("@id_pessoa_fk", func.id_pessoa);
                cmd.Parameters.AddWithValue("@salario", func.salario);
                cmd.Parameters.AddWithValue("@cargo", func.cargo);
                cmd.Parameters.AddWithValue("@crmv", func.crmv);
                cmd.ExecuteNonQuery();
                ConnectionString.Connection.Close();
                resp.Executed = true;

            }
            //Erro encontrado
            catch (Exception e)
            {
                //Retorna o erro 
                resp.Executed = false;
                resp.ErrorMessage = "Erro";
                resp.Exception = e;
            }
            // Fecha a conexão caso esteja aberta
            if (ConnectionString.Connection.State == System.Data.ConnectionState.Open)
            {
                ConnectionString.Connection.Close();
            }
            return resp;

        }
        /// <summary>
        /// Seleciona o perfil do funcionário logado
        /// </summary>
        /// <param name="id_funcionario"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static Response SelectProfileEmployee(int id_funcionario, out Funcionario func)
        {

            Response res = new Response();
            Pessoa pessoa = new Pessoa();
            Acesso acesso = new Acesso();
            Endereco endereco = new Endereco();
            func = new Funcionario();

            int idPessoa = -1;
            int idAcesso = -1;
            int idEndereco = -1;

            try
            {
                idPessoa = GenericDB.SelectIdWithClause("id_pessoa_fk", "funcionario", "id_funcionario", $"{id_funcionario}").ElementId;
                idAcesso = GenericDB.SelectIdWithClause("id_acesso_fk", "pessoa", "id_pessoa", $"{idPessoa}").ElementId;
                idEndereco = GenericDB.SelectIdWithClause("id_endereco_fk", "pessoa", "id_pessoa", $"{idPessoa}").ElementId;

                if (idPessoa != -1 && idAcesso != -1 && idEndereco != -1)
                {
                    res = GenericDB.SelectPerson(out pessoa, idPessoa);

                    if (res.Executed)
                    {
                        res = GenericDB.SelectAccess(out acesso, idAcesso);

                        if (res.Executed)
                        {
                            res = GenericDB.SelectAddress(out endereco, idEndereco);

                            if (res.Executed)
                            {
                                func.email = acesso.email;
                                func.telefone_1 = pessoa.telefone_1;
                                func.telefone_2 = pessoa.telefone_2;
                                func.cep = endereco.cep;
                                func.cidade = endereco.cidade;
                                func.estado = endereco.estado;
                                func.rua = endereco.rua;
                                func.numero_residencia = endereco.numero_residencia;
                                func.bairro = endereco.bairro;
                                func.complemento = endereco.complemento;

                            }
                            else
                            {
                                res.Executed = false;
                            }
                        }
                        else
                        {
                            res.Executed = false;
                        }
                    }
                    else
                    {
                        res.Executed = false;
                    }
                }
                else
                {
                    res.Executed = false;
                }
            }
            catch (Exception e)
            {
                res.Executed = false;
                res.ErrorMessage = "Não foi possível efetuar a consulta";
                res.Exception = e;
            }

            if (ConnectionString.Connection.State.Equals(System.Data.ConnectionState.Open))
            {
                ConnectionString.Connection.Close();
            }

            return res;

        }
        /// <summary>
        /// Atualiza o perfil do funcionário com  as novas informações
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public static Response UpdateEmployee(Funcionario func)
        {
            Response res = new Response();

            int idPessoa = -1;
            int idAcesso = -1;
            int idEndereco = -1;

            try
            {
                idPessoa = GenericDB.SelectIdWithClause("id_pessoa_fk", "funcionario", "id_funcionario", $"{func.id_funcionario}").ElementId;
                idAcesso = GenericDB.SelectIdWithClause("id_acesso_fk", "pessoa", "id_pessoa", $"{idPessoa}").ElementId;
                idEndereco = GenericDB.SelectIdWithClause("id_endereco_fk", "pessoa", "id_pessoa", $"{idPessoa}").ElementId;

                if (idPessoa != -1 && idAcesso != -1 && idEndereco != -1)
                {
                    if (func.email != null)
                    {
                        if (func.senha != null)
                        {
                            res = GenericDB.UpdateAccess(new Acesso() { id_acesso = idAcesso, email = func.email, senha = func.senha });
                        }
                        else
                        {
                            res = GenericDB.UpdateAccess(new Acesso() { id_acesso = idAcesso, email = func.email });
                        }

                        if (res.Executed)
                        {
                            res.Executed = true;
                        }
                        else
                        {
                            return new Response() { Executed = false };
                        }
                    }

                    res = GenericDB.UpdatePerson(new Pessoa() { id_pessoa = idPessoa, telefone_1 = func.telefone_1, telefone_2 = func.telefone_2 });

                    if (res.Executed)
                    {
                        res = GenericDB.UpdateAddress(new Endereco() { id_endereco = idEndereco, rua = func.rua, numero_residencia = func.numero_residencia, bairro = func.bairro, estado = func.estado, cidade = func.cidade, cep = func.cep, complemento = func.complemento });

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
                    }

                }
                else
                {
                    res.Executed = false;
                }
            }
            catch (Exception e)
            {
                res.Executed = false;
                res.ErrorMessage = "Não foi possível atualizar";
                res.Exception = e;
            }

            if (ConnectionString.Connection.State.Equals(System.Data.ConnectionState.Open))
            {
                ConnectionString.Connection.Close();
            }

            return res;
        }
    }
}
