using BAL;
using DAL;
using MetaDados;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VetOnTrack.Controllers
{
    public class CadastrosController : Controller
    {
        /// <summary>
        /// Retorna a view CadastroFuncionario
        /// </summary>
        /// <returns></returns>
        public IActionResult CadastroFuncionario()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View();
            }
            else
            {
                return Unauthorized();
            }
        }

        /// <summary>
        /// Retorna a view CadastroCliente
        /// </summary>
        /// <returns></returns>
        public IActionResult CadastroCliente()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View();
            }
            else
            {
                return Unauthorized();
            }
        }

        /// <summary>
        /// Recebe os campos da view CadastroCliente e executa os comandos
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AuthCadastroCliente()
        {
            Cliente cliente = new Cliente();
            cliente.email = Request.Form["input_email"];
            cliente.senha = Request.Form["input_senha"];
            cliente.nome = Request.Form["input_name"];
            cliente.data_nascimento = Request.Form["input_data_de_nascimento"];
            cliente.rg = Request.Form["input_rg"];
            cliente.cpf = Request.Form["input_cpf"];
            cliente.telefone_1 = Request.Form["input_tel_1"];
            cliente.telefone_2 = Request.Form["input_tel_2"];
            cliente.cep = Request.Form["input_cep"];
            cliente.cidade = Request.Form["input_cidade"];
            cliente.estado = Request.Form["input_estado"];
            cliente.rua = Request.Form["input_rua"];
            cliente.numero_residencia = Convert.ToInt32(Request.Form["input_numero_residencia"]);
            cliente.bairro = Request.Form["input_bairro"];
            cliente.complemento = Request.Form["input_complemento"];
            cliente.nivel = 0;

            Response res = ClienteBAL.InsertClient(cliente);

            if (res.Executed)
            {
                //Retorna para a página de cadastro concluído
                return RedirectToAction("CadastroOk", "Extra");
            }
            else
            {
                //Retorna para a página de cadastro não concluído
                ViewData["ErrorLog"] = res.ErrorMessage;
                return RedirectToAction("CadastroNaoOk", "Extra");
            }
        }

        /// <summary>
        /// Recebe os campos da view CadastroFuncionario e executa os comandos
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AuthCadastroFuncionario()
        {
            Funcionario func = new Funcionario();
            func.email = Request.Form["input_email"];
            func.senha = Request.Form["input_senha"];
            func.nome = Request.Form["input_name"];
            func.data_nascimento = Request.Form["input_data_de_nascimento"];
            func.cpf = Request.Form["input_cpf"];
            func.rg = Request.Form["input_rg"];
            func.telefone_1 = Request.Form["input_tel_1"];
            func.telefone_2 = Request.Form["input_tel_2"];
            func.cep = Request.Form["input_cep"];
            func.cidade = Request.Form["input_cidade"];
            func.estado = Request.Form["input_estado"];
            func.rua = Request.Form["input_rua"];
            func.numero_residencia = Convert.ToInt32(Request.Form["input_numero_residencia"]);
            func.bairro = Request.Form["input_bairro"];
            func.cargo = Request.Form["input_cargo"];
            func.complemento = Request.Form["input_complemento"];
            func.crmv = Request.Form["input_crmv"];
            string salario = Checker.StringCleaner(Request.Form["input_salario"]);
            func.salario = Convert.ToDouble(salario);
            func.nivel = Convert.ToInt32(Request.Form["input_nivel"]);

            Response res = FuncionarioBAL.InsertEmployee(func);

            if (res.Executed)
            {
                //Retorna para a página de cadastro concluído
                return RedirectToAction("CadastroOk", "Extra");
            }
            else
            {
                //Retorna para a página de cadastro não concluído
                ViewData["ErrorLog"] = res.ErrorMessage;
                return RedirectToAction("CadastroNaoOk", "Extra");
            }
        }
    }

}