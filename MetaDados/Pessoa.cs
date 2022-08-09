using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaDados
{

    public class Pessoa : Acesso
    {
        public int id_pessoa { get; set; }
        public string rg { get; set; }
        public string cpf { get; set; }
        public string telefone_1 { get; set; }
        public string telefone_2 { get; set; }
        public Pessoa()
        {

        }
        public Pessoa(string rg,string cpf, string telefone_1, string telefone_2)
        {
            this.rg = rg;
            this.cpf = cpf;
            this.telefone_1 = telefone_1;
            this.telefone_2 = telefone_2;
        }
    }

}
