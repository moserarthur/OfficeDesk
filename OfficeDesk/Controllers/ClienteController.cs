using BAL;
using DAL;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VetOnTrack.Controllers
{
    public class ClienteController : Controller
    {
        public IActionResult DeleteClient(int id_cliente)
        {
            Response res = ClienteBAL.DeleteClient(id_cliente);
            if (res.Executed)
            {
                //Retorna para a página de cadastro concluído
                return RedirectToAction("ClienteDeletado", "Extra");
            }
            else
            {
                //Retorna para a página de cadastro não concluído
                ViewData["ErrorLog"] = res.ErrorMessage;
                return RedirectToAction("ClienteNaoDeletado", "Extra");
            }
        }

        public IActionResult SearchClient()
        {
            return View();
        }
    }
}
