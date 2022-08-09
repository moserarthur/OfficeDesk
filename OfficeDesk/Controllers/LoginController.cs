using BAL;
using DAL;
using MetaDados;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using System.Text;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace VetOnTrack.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Login(string returnUrl)
        {
            return View();
        }

        [HttpPost("login/authenticate")] //todos acessam este metodo
        public async Task<IActionResult> Authenticate([FromForm] Acesso acesso)
        {
            int idFuncionario;

            Response response = AcessoBAL.CheckAccess(acesso, out acesso, out idFuncionario);

            if (acesso.nivel != 0)
            {

                if (response.Executed)
                {

                    byte[] idAcesso = Encoding.ASCII.GetBytes(acesso.id_acesso.ToString());
                    byte[] idFunc = Encoding.ASCII.GetBytes(idFuncionario.ToString());

                    HttpContext.Session.Set("idAcesso", idAcesso);
                    HttpContext.Session.Set("idFuncionario", idFunc);

                    var claims = new List<Claim>();
                    claims.Add(new Claim(ClaimTypes.Role, acesso.nivel.ToString()));
                    claims.Add(new Claim(ClaimTypes.Name, acesso.email));

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme); //o tipo de autentcação sera de cookies


                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity); //crio uma Claim ou direito baseado em Identity

                    await HttpContext.SignInAsync(claimsPrincipal);

                    // return RedirectToAction("Index", "Home");
                    return Redirect("/home");
                }
                else
                {
                    string message = response.ErrorMessage != null ? response.ErrorMessage : "Usuário ou Senha incorretos";

                    TempData["erro"] = message;

                    return Redirect("/login");
                }
            }
            else
            {
                return Unauthorized();
            }

        }
    }
}
