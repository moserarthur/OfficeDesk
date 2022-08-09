using BAL;
using DAL;
using MetaDados;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VetOnTrack.Controllers
{
    public class FuncionarioController : Controller
    {
        
        [HttpPost]
        public void UpdateEmployee([FromForm] Funcionario funcionario)
        {
            byte[] id;

            HttpContext.Session.TryGetValue("idFuncionario", out id);

            string idFuncionario = Encoding.ASCII.GetString(id);

            string novaSenha = Request.Form["novaSenha"].ToString();
            string confSenha = Request.Form["confirmaSenha"].ToString();

            funcionario.id_funcionario = Convert.ToInt32( idFuncionario);
            Response res = FuncionarioBAL.UpdateEmployee(funcionario, novaSenha, confSenha);

            if (res.Executed)
            {
                TempData["executed"] = "Atualizado com sucesso";

                Response.Redirect("/perfil");
            }
            else if (res.Exception != null)
            {
                TempData["erro"] = res.Exception;
                Response.Redirect("/perfil");
            }
            else
            {
                TempData["erro"] = res.ErrorMessage;
                Response.Redirect("/perfil");
            }
        }
    }
}
