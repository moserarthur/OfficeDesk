using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaDados
{
    public class Acesso : Endereco
    {

        public int id_acesso { get; set; }
        public string email { get; set; }
        public string senha { get; set; }
        public int nivel { get; set; }
        public Acesso()
        {
        }

        public Acesso(string email, string senha, int nivel)
        {
            this.email = email;
            this.senha = senha;
            this.nivel = nivel;
        }
    }

}
