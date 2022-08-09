using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaDados
{
    public class Funcionario : Pessoa
    {
        public int id_funcionario { get; set; }
        public double salario { get; set; }
        public string cargo { get; set; }
        public string crmv { get; set; }

        public Funcionario()
        {
        }

        public Funcionario(double salario, string cargo, string crmv)
        {
            this.salario = salario;
            this.cargo = cargo;
            this.crmv = crmv;
        }
    }
}
