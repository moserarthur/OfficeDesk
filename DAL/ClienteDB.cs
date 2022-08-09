using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetaDados;

namespace DAL
{
    public static class ClienteDB
    {
        /// <summary>
        /// Insere no dbo.cliente -> Objeto cliente (id_pessoa_fk)
        /// </summary>
        /// <param name="cliente"></param>
        /// <returns>Response</returns>
        public static Response InsertClient(Cliente cliente)
        {
            Response resp = new Response();

            resp = GenericDB.InsertData(new Dado(cliente.nome, cliente.data_nascimento));
            cliente.id_dado = resp.ElementId;
            if (cliente.id_dado == -1)
            {
                resp = GenericDB.RollbackInsertsClient(cliente);
                if (resp.Executed)
                {
                    return new Response()
                    {
                        Executed = false,
                        ErrorMessage = "Insert Inválido",
                        Exception = resp.Exception

                    };
                }
                else
                {
                    return resp;
                }
            }

            resp = GenericDB.InsertAddress(new Endereco(cliente.rua, cliente.numero_residencia, cliente.bairro, cliente.estado, cliente.cidade, cliente.cep, cliente.complemento));
            cliente.id_endereco = resp.ElementId;
            if (cliente.id_endereco == -1)
            {
                resp = GenericDB.RollbackInsertsClient(cliente);
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

            resp = GenericDB.InsertAccess(new Acesso(cliente.email, cliente.senha, cliente.nivel));
            cliente.id_acesso = resp.ElementId;
            if (cliente.id_acesso == -1)
            {
                resp = GenericDB.RollbackInsertsClient(cliente);
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

            resp = GenericDB.InsertPersonClient(new Pessoa(cliente.rg, cliente.cpf, cliente.telefone_1, cliente.telefone_2), cliente);
            cliente.id_pessoa = resp.ElementId;
            if (cliente.id_pessoa == -1)
            {
                resp = GenericDB.RollbackInsertsClient(cliente);
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


            string insert = "insert into dbo.Cliente (id_pessoa_fk) values (@id_pessoa_fk)";
            resp = new Response();
            SqlCommand cmd = new SqlCommand(insert, ConnectionString.Connection);
            try
            {
                ConnectionString.Connection.Open();
                cmd.Parameters.AddWithValue("@id_pessoa_fk", cliente.id_pessoa);
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
        /// Seleciona a lista de todos os clientes
        /// </summary>
        /// <param name="lista_cliente"></param>
        /// <param name="id_cliente"></param>
        /// <returns>Response | List (Cliente) </returns>
        //public static Response SelectListNameClient(out List<Cliente> lista_cliente, string nome)
        //{
        //    lista_cliente = new List<Cliente>();
        //    string select;
        //    bool filtro = false;
        //    if (nome.Length > 0)
        //    {
        //        select = "select * from dbo.dado where nome like '%@nome%';";
        //        filtro = true;
        //    }
        //    else
        //    {
        //        select = "select * from dbo.cliente";
        //    }
        //    Response resp = new Response();
        //    SqlCommand cmd = new SqlCommand(select, ConnectionString.Connection);
        //    try
        //    {
        //        ConnectionString.Connection.Open();
        //        if (filtro)
        //        {
        //            cmd.Parameters.AddWithValue("@id_cliente", id_cliente);
        //        }

        //        SqlDataReader dr = cmd.ExecuteReader();
        //    }
        //    }
        public static Response SelectListClient(out List<Cliente> lista_cliente, int id_cliente)
        {
            lista_cliente = new List<Cliente>();
            string select;
            bool filtro = false;
            if (id_cliente > 0)
            {
                select = "select * from dbo.cliente where id_cliente = @id_cliente";
                filtro = true;
            }
            else
            {
                select = "select * from dbo.cliente";
            }

            Response resp = new Response();
            SqlCommand cmd = new SqlCommand(select, ConnectionString.Connection);

            try
            {
                ConnectionString.Connection.Open();
                if (filtro)
                {
                    cmd.Parameters.AddWithValue("@id_cliente", id_cliente);
                }

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    //colocar o que precisa no cliente
                    Cliente cliente = new Cliente();
                    cliente.id_pessoa = Convert.ToInt32(dr["id_pessoa_fk"]);
                    cliente.id_cliente = Convert.ToInt32(dr["id_cliente"]);

                    lista_cliente.Add(cliente);
                }
                dr.Close();
                ConnectionString.Connection.Close();

                foreach (var item in lista_cliente)
                {
                    Pessoa pessoa = new Pessoa();
                    try
                    {
                        resp = GenericDB.SelectPerson(out pessoa, item.id_pessoa);
                        if (!resp.Executed)
                        {
                            return resp;
                        }

                        item.id_acesso = pessoa.id_acesso;
                        item.id_dado = pessoa.id_dado;
                        item.id_endereco = pessoa.id_endereco;
                        item.rg = pessoa.rg;
                        item.cpf = pessoa.cpf;
                        item.telefone_1 = pessoa.telefone_1;
                        item.telefone_2 = pessoa.telefone_2;
                    }
                    //Erro encontrado
                    catch (Exception e)
                    {
                        // Fecha a conexão caso esteja aberta
                        if (ConnectionString.Connection.State == System.Data.ConnectionState.Open)
                        {
                            ConnectionString.Connection.Close();
                        }
                        //Retorna o erro 
                        resp.Executed = false;
                        resp.ErrorMessage = "Erro";
                        resp.Exception = e;

                        return resp;
                    }


                    try
                    {
                        Dado dado = new Dado();
                        resp = GenericDB.SelectData(out dado, item.id_dado);
                        if (!resp.Executed)
                        {
                            return resp;
                        }

                        item.nome = dado.nome;
                        item.data_nascimento = dado.data_nascimento;
                    }
                    //Erro encontrado
                    catch (Exception e)
                    {
                        // Fecha a conexão caso esteja aberta
                        if (ConnectionString.Connection.State == System.Data.ConnectionState.Open)
                        {
                            ConnectionString.Connection.Close();
                        }
                        //Retorna o erro 
                        resp.Executed = false;
                        resp.ErrorMessage = "Erro";
                        resp.Exception = e;

                        return resp;
                    }

                    try
                    {
                        Endereco endereco = new Endereco();
                        resp = GenericDB.SelectAddress(out endereco, item.id_endereco);
                        if (!resp.Executed)
                        {
                            return resp;
                        }

                        item.rua = endereco.rua;
                        item.numero_residencia = endereco.numero_residencia;
                        item.bairro = endereco.bairro;
                        item.estado = endereco.estado;
                        item.cidade = endereco.cidade;
                        item.cep = endereco.cep;
                        item.complemento = endereco.complemento;

                    }
                    //Erro encontrado
                    catch (Exception e)
                    {
                        // Fecha a conexão caso esteja aberta
                        if (ConnectionString.Connection.State == System.Data.ConnectionState.Open)
                        {
                            ConnectionString.Connection.Close();
                        }
                        //Retorna o erro 
                        resp.Executed = false;
                        resp.ErrorMessage = "Erro";
                        resp.Exception = e;

                        return resp;
                    }

                    try
                    {
                        Acesso acesso = new Acesso();
                        resp = GenericDB.SelectAccess(out acesso, item.id_acesso);
                        if (!resp.Executed)
                        {
                            return resp;
                        }

                        item.email = acesso.email;
                        item.senha = acesso.senha;
                        item.nivel = acesso.nivel;

                    }
                    //Erro encontrado
                    catch (Exception e)
                    {
                        // Fecha a conexão caso esteja aberta
                        if (ConnectionString.Connection.State == System.Data.ConnectionState.Open)
                        {
                            ConnectionString.Connection.Close();
                        }
                        //Retorna o erro 
                        resp.Executed = false;
                        resp.ErrorMessage = "Erro";
                        resp.Exception = e;

                        return resp;
                    }
                }

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
        /// Deleta um cliente e todos os pets vinculados a ele
        /// </summary>
        /// <param name="id_cliente"></param>
        /// <returns>Response</returns>
        public static Response DeleteClient(int id_cliente)
        {
            Response resp = new Response();
            try
            {

                List<Cliente> lista_cliente = new List<Cliente>();
                SelectListClient(out lista_cliente, id_cliente);

                foreach (var item in lista_cliente)
                {
                    resp = GenericDB.DeleteAccess(item.id_acesso);
                    if (!resp.Executed)
                    {
                        return resp;
                    }
                    resp = GenericDB.DeleteAddress(item.id_endereco);
                    if (!resp.Executed)
                    {
                        return resp;
                    }
                    resp = GenericDB.DeleteData(item.id_dado);
                    if (!resp.Executed)
                    {
                        return resp;
                    }
                }
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



    }
}
