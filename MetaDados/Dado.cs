using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaDados
{
    public class Dado
    {
        public int id_dado { get; set; }
        public string nome { get; set; }
        public string data_nascimento { get; set; }
        public Dado()
        {

        }
        public Dado(string nome, string data_nascimento)
        {
            this.nome = nome;
            this.data_nascimento = data_nascimento;
        }
    }

}
