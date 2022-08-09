using MetaDados;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace DAL
{
    public static class GenericDB
    {
        /// <summary>
        /// Select dbo.acesso -> Objeto acesso (usuario, senha, nivel) | Verifica se o usuario e senha confere com o registrado no banco de dados
        /// </summary>
        /// <param name="acesso"></param>
        /// <param name="acesso1"></param>
        /// <returns>Response</returns>
        public static Response CheckAccess(Acesso acesso, out Acesso acesso1, out int idFuncionario)
        {
            idFuncionario = 0;

            acesso1 = new Acesso();

            string select = "select id_acesso, email, senha, nivel from dbo.acesso where email = @email";

            Response resp = new Response();

            SqlCommand cmd = new SqlCommand(select, ConnectionString.Connection);
            try
            {
                ConnectionString.Connection.Open();
                cmd.Parameters.AddWithValue("@email", acesso.email);


                SqlDataReader dr = cmd.ExecuteReader();         //Executa a leitura do banco de dados


                if (dr.Read() && dr.GetString(2) == acesso.senha)
                {
                    acesso1.id_acesso = dr.GetInt32(0);
                    acesso1.email = dr.GetString(1);            // Atribui o email e o nível ao acesso1
                    acesso1.nivel = dr.GetInt32(3);

                    dr.Close();
                    //Comando executado corretamente
                    ConnectionString.Connection.Close();

                    resp = SelectIdWithClause("id_pessoa", "pessoa", "id_acesso_fk", acesso1.id_acesso.ToString());

                    int idPessoa = resp.ElementId;

                    resp = SelectIdWithClause("id_funcionario", "funcionario", "id_pessoa_fk", idPessoa.ToString());

                    idFuncionario = resp.ElementId;

                    resp.Executed = true;
                }
                else
                {
                    resp.Executed = false;
                }

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
        /// Insere no dbo.acesso -> Objeto acesso (email, senha, nivel)
        /// </summary>
        /// <param name="acesso"></param>
        /// <returns>Response int(id_acesso)</returns>
        public static Response InsertAccess(Acesso acesso)
        {
            string insert = "insert into dbo.acesso (email,senha,nivel) values (@email,@senha,@nivel)";
            Response resp = new Response();
            SqlCommand cmd = new SqlCommand(insert, ConnectionString.Connection);
            try
            {
                ConnectionString.Connection.Open();
                cmd.Parameters.AddWithValue("@email", acesso.email);        //   Adiciona os parâmetros atribuindo os valores ao objeto
                cmd.Parameters.AddWithValue("@senha", acesso.senha);        //   (Método para evitar SQL Injection)
                cmd.Parameters.AddWithValue("@nivel", acesso.nivel);
                cmd.ExecuteNonQuery();                                      // Executa o comando imediatamente
                ConnectionString.Connection.Close();
            }
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
                resp.ElementId = -1;
                return resp;
            }
            return SelectId("id_acesso", "acesso");                         // Retorna o id_acesso       
        }

        /// <summary>
        /// Insere no dbo.dado -> Objeto dado (nome, data_nascimento)
        /// </summary>
        /// <param name="dado"></param>
        /// <returns>Response(int id_dado)</returns>
        public static Response InsertData(Dado dado)
        {
            string insert = "insert into dbo.dado (nome,data_nascimento) values (@nome,@data_nascimento)";
            Response resp = new Response();
            SqlCommand cmd = new SqlCommand(insert, ConnectionString.Connection);
            try
            {
                ConnectionString.Connection.Open();
                cmd.Parameters.AddWithValue("@nome", dado.nome);                            // Adiciona os parâmetros atribuindo os valores ao objeto
                cmd.Parameters.AddWithValue("@data_nascimento", dado.data_nascimento);       // (Método para evitar SQL Injection)
                cmd.ExecuteNonQuery();                                                      // Executa o comando imediatamente
                ConnectionString.Connection.Close();
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
                resp.ElementId = -1;
                return resp;
            }
            return SelectId("id_dado", "dado");                                             // Retorna o id_dado  
        }

        /// <summary>
        /// Insere no dbo.endereco -> Objeto endereco (rua, numero, bairro, estado, cidade, cep, complemento)
        /// </summary>
        /// <param name="endereco"></param>
        /// <returns>Response(int id_endereco)</returns>
        public static Response InsertAddress(Endereco endereco)
        {
            string insert = "insert into dbo.endereco (rua,numero,bairro,estado,cidade,cep,complemento) values (@rua,@numero,@bairro,@estado,@cidade,@cep,@complemento)";
            Response resp = new Response();
            SqlCommand cmd = new SqlCommand(insert, ConnectionString.Connection);
            try
            {
                ConnectionString.Connection.Open();
                cmd.Parameters.AddWithValue("@rua", endereco.rua);
                cmd.Parameters.AddWithValue("@numero", endereco.numero_residencia);             // Adiciona os parâmetros atribuindo os valores ao objeto
                cmd.Parameters.AddWithValue("@bairro", endereco.bairro);                        // (Método para evitar SQL Injection)
                cmd.Parameters.AddWithValue("@estado", endereco.estado);
                cmd.Parameters.AddWithValue("@cidade", endereco.cidade);
                cmd.Parameters.AddWithValue("@cep", endereco.cep);
                cmd.Parameters.AddWithValue("@complemento", endereco.complemento);
                cmd.ExecuteNonQuery();                                                          // Executa o comando imediatamente
                ConnectionString.Connection.Close();
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
                resp.ElementId = -1;
                return resp;
            }
            return SelectId("id_endereco", "endereco");                                         // Retorna o id_endereco

        }

        /// <summary>
        /// Insere no dbo.pessoa -> Objeto pessoa (id_acesso_fk,id_dado_fk,id_endereco_fk,cpf,telefone_1,telefone_2)
        /// </summary>
        /// <param name="pessoa"></param>
        /// <param name="cliente"></param>
        /// <returns> Response int(id_pessoa)</returns>
        public static Response InsertPersonClient(Pessoa pessoa, Cliente cliente)
        {
            pessoa.id_acesso = cliente.id_acesso;
            pessoa.id_dado = cliente.id_dado;
            pessoa.id_endereco = cliente.id_endereco;
            string insert = "insert into dbo.pessoa (id_acesso_fk,id_dado_fk,id_endereco_fk,rg,cpf,telefone_1,telefone_2) values (@id_acesso_fk,@id_dado_fk,@id_endereco_fk,@rg,@cpf,@telefone_1,@telefone_2)";
            Response resp = new Response();
            SqlCommand cmd = new SqlCommand(insert, ConnectionString.Connection);
            try
            {
                ConnectionString.Connection.Open();
                cmd.Parameters.AddWithValue("@id_acesso_fk", pessoa.id_acesso);                    // Adiciona os parâmetros atribuindo os valores ao objeto
                cmd.Parameters.AddWithValue("@id_dado_fk", pessoa.id_dado);                        // (Método para evitar SQL Injection)
                cmd.Parameters.AddWithValue("@id_endereco_fk", pessoa.id_endereco);
                cmd.Parameters.AddWithValue("@rg", pessoa.rg);
                cmd.Parameters.AddWithValue("@cpf", pessoa.cpf);
                cmd.Parameters.AddWithValue("@telefone_1", pessoa.telefone_1);
                cmd.Parameters.AddWithValue("@telefone_2", pessoa.telefone_2);

                cmd.ExecuteNonQuery();                                                             // Executa o comando imediatamente
                ConnectionString.Connection.Close();
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
                resp.ElementId = -1;
                return resp;
            }
            return SelectId("id_pessoa", "pessoa");                                                 // Retorna o id_pessoa
        }

        /// <summary>
        /// Insere no dbo.pessoa -> Objeto pessoa (id_acesso_fk,id_dado_fk,id_endereco_fk,cpf,telefone_1,telefone_2)
        /// </summary>
        /// <param name="pessoa"></param>
        /// <param name="func"></param>
        /// <returns> Response int(id_pessoa)</returns>
        public static Response InsertPersonEmployee(Pessoa pessoa, Funcionario func)
        {
            pessoa.id_acesso = func.id_acesso;
            pessoa.id_dado = func.id_dado;
            pessoa.id_endereco = func.id_endereco;
            string insert = "insert into dbo.pessoa (id_acesso_fk,id_dado_fk,id_endereco_fk,rg,cpf,telefone_1,telefone_2) values (@id_acesso_fk,@id_dado_fk,@id_endereco_fk,@rg,@cpf,@telefone_1,@telefone_2)";
            Response resp = new Response();
            SqlCommand cmd = new SqlCommand(insert, ConnectionString.Connection);
            try
            {
                ConnectionString.Connection.Open();
                cmd.Parameters.AddWithValue("@id_acesso_fk", pessoa.id_acesso);
                cmd.Parameters.AddWithValue("@id_dado_fk", pessoa.id_dado);
                cmd.Parameters.AddWithValue("@id_endereco_fk", pessoa.id_endereco);
                cmd.Parameters.AddWithValue("@rg", pessoa.rg);
                cmd.Parameters.AddWithValue("@cpf", pessoa.cpf);
                cmd.Parameters.AddWithValue("@telefone_1", pessoa.telefone_1);
                cmd.Parameters.AddWithValue("@telefone_2", pessoa.telefone_2);

                cmd.ExecuteNonQuery();
                ConnectionString.Connection.Close();
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
                resp.ElementId = -1;
                return resp;
            }
            return SelectId("id_pessoa", "pessoa");
        }

        /// <summary>
        /// Deleta os dados de um (cliente |ou| funcionario |ou| pet) e consequentemente todo o cadastro do mesmo
        /// </summary>
        /// <param name="id_dado"></param>
        /// <returns>Response</returns>
        public static Response DeleteData(int id_dado)
        {
            string delete = "delete from dbo.dado where id_dado = @id_dado";
            Response resp = new Response();
            SqlCommand cmd = new SqlCommand(delete, ConnectionString.Connection);
            try
            {
                ConnectionString.Connection.Open();
                cmd.Parameters.AddWithValue("@id_dado", id_dado);

                cmd.ExecuteNonQuery();
                ConnectionString.Connection.Close();
                //Comando executado corretamente
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
        /// Deleta o endereço de um (cliente |ou| funcionario) e consequentemente todo o cadastro do mesmo
        /// </summary>
        /// <param name="id_endereco"></param>
        /// <returns>Response</returns>
        public static Response DeleteAddress(int id_endereco)
        {
            string delete = "delete from dbo.endereco where id_endereco = @id_endereco";
            Response resp = new Response();
            SqlCommand cmd = new SqlCommand(delete, ConnectionString.Connection);
            try
            {
                ConnectionString.Connection.Open();
                cmd.Parameters.AddWithValue("@id_endereco", id_endereco);

                cmd.ExecuteNonQuery();
                ConnectionString.Connection.Close();


                //Comando executado corretamente
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
        /// Deleta o acesso de um (cliente |ou| funcionario) e consequentemente todo o cadastro do mesmo
        /// </summary>
        /// <param name="id_acesso"></param>
        /// <returns>Response</returns>
        public static Response DeleteAccess(int id_acesso)
        {
            string delete = "delete from dbo.acesso where id_acesso = @id_acesso";
            Response resp = new Response();
            SqlCommand cmd = new SqlCommand(delete, ConnectionString.Connection);
            try
            {
                ConnectionString.Connection.Open();
                cmd.Parameters.AddWithValue("@id_acesso", id_acesso);

                cmd.ExecuteNonQuery();
                ConnectionString.Connection.Close();
                //Comando executado corretamente
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
        /// Caso alguma etapa do insert (data |ou| address |ou| access) para inserir uma pessoa, de errado, reverte todos os  inserts antes efetuado por esse cadastro incorreto
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public static Response RollbackInsertsClient(Cliente client)
        {

            Response resp = new Response();


            if (client.id_acesso > 0)
            {
                resp = DeleteAccess(client.id_acesso);
                if (!resp.Executed)
                {
                    resp.ErrorMessage = "Erro"; //mudar os nomes dos erros e colocar um código pra cada um, pra poder se achar depois
                    return resp;
                }

            }

            if (client.id_endereco > 0)
            {
                resp = DeleteAddress(client.id_endereco);
                if (!resp.Executed)
                {
                    resp.ErrorMessage = "Erro"; //mudar os nomes dos erros e colocar um código pra cada um, pra poder se achar depois


                    return resp;
                }
            }

            if (client.id_dado > 0)
            {
                resp = DeleteData(client.id_dado);
                if (!resp.Executed)
                {
                    resp.ErrorMessage = "Erro"; //mudar os nomes dos erros e colocar um código pra cada um, pra poder se achar depois


                    return resp;
                }
            }


            return new Response()
            {
                Executed = true
            };
        }

        /// <summary>
        /// Caso alguma etapa do insert (data |ou| address |ou| access) para inserir uma pessoa, de errado, reverte todos os  inserts antes efetuado por esse cadastro incorreto
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public static Response RollbackInsertsEmployee(Funcionario funcionario)
        {
            Response resp = new Response();

            if (funcionario.id_acesso > 0)
            {
                resp = DeleteAccess(funcionario.id_acesso);
                if (!resp.Executed)
                {
                    resp.ErrorMessage = "Erro"; //mudar os nomes dos erros e colocar um código pra cada um, pra poder se achar depois


                    return resp;
                }

            }

            if (funcionario.id_endereco > 0)
            {
                resp = DeleteAddress(funcionario.id_endereco);
                if (!resp.Executed)
                {
                    resp.ErrorMessage = "Erro"; //mudar os nomes dos erros e colocar um código pra cada um, pra poder se achar depois


                    return resp;
                }
            }

            if (funcionario.id_dado > 0)
            {
                resp = DeleteData(funcionario.id_dado);
                if (!resp.Executed)
                {
                    resp.ErrorMessage = "Erro"; //mudar os nomes dos erros e colocar um código pra cada um, pra poder se achar depois


                    return resp;
                }
            }


            return new Response()
            {
                Executed = true
            };
        }

        /// <summary>
        /// Seleciona Id -> (nome_campo_id, nome_tabela) | Seleciona o último elemento inserido do campo {nome_campo_id} na tabela {nome_tabela}
        /// </summary>
        /// <param name="nome_campo_id"></param>
        /// <param name="nome_tabela">dfg</param>
        /// <returns>Response int(ElementId)</returns>
        public static Response SelectId(string nome_campo_id, string nome_tabela)
        {
            string select = $"select top 1 {nome_campo_id} from  dbo.{nome_tabela} order by {nome_campo_id} desc";
            Response resp = new Response();
            SqlCommand cmd = new SqlCommand(select, ConnectionString.Connection);
            int id = -1;
            try
            {

                ConnectionString.Connection.Open();

                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    id = dr.GetInt32(0);

                }
                dr.Close();
                ConnectionString.Connection.Close();
                //Comando executado corretamente
                resp.Executed = true;
                resp.ElementId = id;

            }
            //Erro encontrado
            catch (Exception e)
            {
                //Retorna o erro 
                resp.Executed = false;
                resp.ErrorMessage = "Erro";
                resp.Exception = e;
                resp.ElementId = -1;
            }
            // Fecha a conexão caso esteja aberta
            if (ConnectionString.Connection.State == System.Data.ConnectionState.Open)
            {
                ConnectionString.Connection.Close();
            }
            return resp;
        }

        /// <summary>
        /// Seleciona a pessoa | Dica: Depois de selecionar a pessoa, lembre de selecionar o (dados, endereço, acesso) da mesma
        /// </summary>
        /// <param name="pessoa"></param>
        /// <param name="id_pessoa"></param>
        /// <returns>Response (Objeto pessoa)</returns>
        public static Response SelectPerson(out Pessoa pessoa, int id_pessoa)
        {
            pessoa = new Pessoa();

            string select = $"select * from dbo.pessoa where id_pessoa = @id_pessoa";
            Response resp = new Response();
            SqlCommand cmd = new SqlCommand(select, ConnectionString.Connection);

            try
            {
                ConnectionString.Connection.Open();
                cmd.Parameters.AddWithValue("@id_pessoa", id_pessoa);

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    pessoa.id_acesso = Convert.ToInt32(dr["id_acesso_fk"]);
                    pessoa.id_dado = Convert.ToInt32(dr["id_dado_fk"]);
                    pessoa.id_endereco = Convert.ToInt32(dr["id_endereco_fk"]);
                    pessoa.rg = dr["rg"].ToString();
                    pessoa.cpf = dr["cpf"].ToString();
                    pessoa.telefone_1 = dr["telefone_1"].ToString();
                    pessoa.telefone_2 = dr["telefone_2"].ToString();
                }

                dr.Close();
                ConnectionString.Connection.Close();
                //Comando executado corretamente
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
        /// Seleciona os dados de determinada pessoa | Dica: Depois de selecionar os dados, lembre de selecionar o (endereço, acesso) da mesma
        /// </summary>
        /// <param name="dado"></param>
        /// <param name="id_dado"></param>
        /// <returns>Response (Objeto dado)</returns>
        public static Response SelectData(out Dado dado, int id_dado)
        {
            dado = new Dado();

            string select = $"select * from dbo.dado where id_dado = @id_dado";
            Response resp = new Response();
            SqlCommand cmd = new SqlCommand(select, ConnectionString.Connection);

            try
            {
                ConnectionString.Connection.Open();
                cmd.Parameters.AddWithValue("@id_dado", id_dado);

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    dado.nome = dr["nome"].ToString();
                    dado.data_nascimento = dr["data_nascimento"].ToString();
                }

                dr.Close();
                ConnectionString.Connection.Close();
                //Comando executado corretamente
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
        /// Seleciona o endereço de determinada pessoa | Dica: Depois de selecionar o endereco, lembre de selecionar o (acesso) da mesma
        /// </summary>
        /// <param name="endereco"></param>
        /// <param name="id_endereco"></param>
        /// <returns>Response (Objeto endereco)</returns>
        public static Response SelectAddress(out Endereco endereco, int id_endereco)
        {
            endereco = new Endereco();

            string select = $"select * from dbo.endereco where id_endereco = @id_endereco";
            Response resp = new Response();
            SqlCommand cmd = new SqlCommand(select, ConnectionString.Connection);

            try
            {
                ConnectionString.Connection.Open();
                cmd.Parameters.AddWithValue("@id_endereco", id_endereco);

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    endereco.rua = dr["rua"].ToString();
                    endereco.numero_residencia = Convert.ToInt32(dr["numero"]);
                    endereco.bairro = dr["bairro"].ToString();
                    endereco.estado = dr["estado"].ToString();
                    endereco.cidade = dr["cidade"].ToString();
                    endereco.cep = dr["cep"].ToString();
                    endereco.complemento = dr["complemento"].ToString();
                }

                dr.Close();
                ConnectionString.Connection.Close();
                //Comando executado corretamente
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
        /// Seleciona o acesso de determinada pessoa | Dica: Agora o objeto pessoa, está preenchido
        /// </summary>
        /// <param name="acesso"></param>m
        /// <param name="id_acesso"></param>
        /// <returns>Response (Objeto acesso)</returns>
        public static Response SelectAccess(out Acesso acesso, int id_acesso)
        {
            acesso = new Acesso();

            string select = $"select * from dbo.acesso where id_acesso = @id_acesso";
            Response resp = new Response();
            SqlCommand cmd = new SqlCommand(select, ConnectionString.Connection);

            try
            {
                ConnectionString.Connection.Open();
                cmd.Parameters.AddWithValue("@id_acesso", id_acesso);

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    acesso.email = dr["email"].ToString();
                    acesso.senha = dr["senha"].ToString();
                    acesso.nivel = Convert.ToInt32(dr["nivel"]);
                }

                dr.Close();
                ConnectionString.Connection.Close();
                //Comando executado corretamente
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
        /// Seleciona o Id de forma dinamica
        /// </summary>
        /// <param name="nome_campo_id"></param>
        /// <param name="nome_tabela"></param>
        /// <param name="clause"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Response SelectIdWithClause(string nome_campo_id, string nome_tabela, string clause, string value)
        {
            string select = $"select {nome_campo_id} from dbo.{nome_tabela} where {clause} = {value}";

            SqlCommand cmd = new SqlCommand(select, ConnectionString.Connection);
            int id = -1;

            try
            {
                ConnectionString.Connection.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    id = dr.GetInt32(0);
                }

                dr.Close();
                ConnectionString.Connection.Close();

            }
            catch (Exception e)
            {

                Response res = new Response()
                {
                    Executed = false,
                    ErrorMessage = "Falha na Consulta",
                    Exception = e
                };
            }

            if (ConnectionString.Connection.State == System.Data.ConnectionState.Open)
            {
                ConnectionString.Connection.Close();
            }

            return new Response() { Executed = true, ElementId = id };
        }
        /// <summary>
        /// Atualiza o acesso
        /// </summary>
        /// <param name="acesso"></param>
        /// <returns></returns>
        public static Response UpdateAccess(Acesso acesso)
        {
            string update = "update dbo.acesso set";
            string campos = "";

            if (acesso.email != null)
            {
                campos += " email = @email";
            }
            if (acesso.senha != null)
            {
                if (campos != "")
                {
                    campos += ", senha = @senha";
                }
                else
                {
                    campos += " senha = @senha";
                }
            }

            update += campos;
            update += " where id_acesso = @id_acesso";

            Response resp = new Response();

            SqlCommand cmd = new SqlCommand(update, ConnectionString.Connection);

            try
            {
                ConnectionString.Connection.Open();

                if (acesso.email != null)
                {
                    cmd.Parameters.AddWithValue("@email", acesso.email);
                }
                if (acesso.senha != null)
                {
                    cmd.Parameters.AddWithValue("@senha", acesso.senha);
                }

                cmd.Parameters.AddWithValue("@id_acesso", acesso.id_acesso);
                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected == 1)
                {
                    resp.Executed = true;
                }
                else
                {
                    resp.Executed = false;
                }

                ConnectionString.Connection.Close();

            }
            catch (Exception e)
            {
                resp.Executed = false;
                resp.ErrorMessage = "Erro";
                resp.Exception = e;
            }

            return resp;
        }
        /// <summary>
        /// Atualiza a pessoa
        /// </summary>
        /// <param name="pessoa"></param>
        /// <returns></returns>
        public static Response UpdatePerson(Pessoa pessoa)
        {
            string update = "update dbo.pessoa set telefone_1 = @telefone_1";
            string clause = " where id_pessoa = @id_pessoa";

            if (pessoa.telefone_2 != null)
            {
                update += ", telefone_2 = @telefone_2";
            }

            update += clause;

            Response resp = new Response();

            SqlCommand cmd = new SqlCommand(update, ConnectionString.Connection);

            try
            {
                ConnectionString.Connection.Open();
                cmd.Parameters.AddWithValue("@telefone_1", pessoa.telefone_1);

                if (pessoa.telefone_2 != null)
                {
                    cmd.Parameters.AddWithValue("@telefone_2", pessoa.telefone_2);
                }

                cmd.Parameters.AddWithValue("@id_pessoa", pessoa.id_pessoa);
                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected == 1)
                {
                    resp.Executed = true;
                }
                else
                {
                    resp.Executed = false;
                }

                ConnectionString.Connection.Close();
            }
            catch (Exception e)
            {
                resp.Executed = false;
                resp.ErrorMessage = "Erro";
                resp.Exception = e;
            }

            return resp;
        }
        /// <summary>
        /// Atualiza endereço
        /// </summary>
        /// <param name="endereco"></param>
        /// <returns></returns>
        public static Response UpdateAddress(Endereco endereco)
        {
            string update = "update dbo.endereco set rua = @rua, numero = @numero_residencia, bairro = @bairro, estado = @estado, cidade = @cidade, cep = @cep, complemento = @complemento where id_endereco = @id_endereco";
            Response resp = new Response();

            SqlCommand cmd = new SqlCommand(update, ConnectionString.Connection);

            try
            {
                ConnectionString.Connection.Open();
                cmd.Parameters.AddWithValue("@rua", endereco.rua);
                cmd.Parameters.AddWithValue("@numero_residencia", endereco.numero_residencia);
                cmd.Parameters.AddWithValue("@bairro", endereco.bairro);
                cmd.Parameters.AddWithValue("@estado", endereco.estado);
                cmd.Parameters.AddWithValue("@cidade", endereco.cidade);
                cmd.Parameters.AddWithValue("@cep", endereco.cep.Replace("-", ""));
                cmd.Parameters.AddWithValue("@complemento", endereco.complemento);
                cmd.Parameters.AddWithValue("@id_endereco", endereco.id_endereco);
                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected == 1)
                {
                    resp.Executed = true;
                }
                else
                {
                    resp.Executed = false;
                }

                ConnectionString.Connection.Close();
            }
            catch (Exception e)
            {
                resp.Executed = false;
                resp.ErrorMessage = "Erro";
                resp.Exception = e;
            }

            return resp;
        }
    }

}

