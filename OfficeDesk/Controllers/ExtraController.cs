using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VetOnTrack.Controllers
{
    public class ExtraController : Controller
    {
        public IActionResult CadastroOk()
        {
            return View();
        }
        public IActionResult CadastroNaoOk()
        {
            return View();
        }

        public IActionResult ClienteDeletado()
        {
            return View();
        }

        public IActionResult ClienteNaoDeletado()
        {
            return View();
        }

    }
}
