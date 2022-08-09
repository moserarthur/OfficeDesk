using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaDados
{
    public class Endereco : Dado
    {
        public int id_endereco { get; set; }
        public string rua { get; set; }
        public int numero_residencia { get; set; }
        public string bairro { get; set; }
        public string estado { get; set; }
        public string cidade { get; set; }
        public string cep { get; set; }
        public string complemento { get; set; }

        public Endereco()
        {

        }

        public Endereco(string rua, int numero_residencia, string bairro, string estado, string cidade, string cep, string complemento)
        {
            this.rua = rua;
            this.numero_residencia = numero_residencia;
            this.bairro = bairro;
            this.estado = estado;
            this.cidade = cidade;
            this.cep = cep;
            this.complemento = complemento;
        }
    }

}
