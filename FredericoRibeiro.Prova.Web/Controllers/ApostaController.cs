using FredericoRibeiro.Prova.Dominio;
using FredericoRibeiro.Prova.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FredericoRibeiro.Prova.Web.Controllers
{
    public class ApostaController : Controller
    {
        // GET: Aposta
        public ActionResult Index()
        {
            if (TempData["apostas"] != null)
                TempData.Keep("apostas");

            return View(new ApostaViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ApostarNumeros(ApostaViewModel model)
        {
            if (TempData["numerosSorteados"] == null)
                TempData.Remove("numerosSorteados");

            if (TempData["apostas"] == null)
                TempData["apostas"] = new List<Aposta>();

            TempData.Keep("apostas");

            if (!ModelState.IsValid)
                return View("Index", model);

            //valida os números postados
            var _numero = long.MinValue;
            if (!long.TryParse(model.Numeros.Replace(" ", ""), out _numero))
                ModelState.AddModelError("Numeros", "Foram informados valores inválidos para a aposta");

            if (!ModelState.IsValid)
                return View("Index", model);

            var numeros = new List<byte>();

            try
            {
                foreach (var numero in model.Numeros.Trim().Split(' ').ToList())
                {
                    numeros.Add(byte.Parse(numero));
                }
            }
            catch
            {
                ModelState.AddModelError("Numeros", "Foram informados valores inválidos para a aposta");
            }

            if (!ModelState.IsValid)
                return View("Index", model);

            var aposta = Apostar(numeros);

            if (aposta.Numeros.Count > 0)
            {
                var apostas = TempData["apostas"] as List<Aposta>;
                apostas.Add(aposta);

                TempData["apostas"] = apostas;
            }
            else
            { 
                foreach (var notificacao in aposta.Notificacoes)
                    ModelState.AddModelError("", notificacao);
            }

            if (!ModelState.IsValid)
                return View("Index", model);

            return View("Index", new ApostaViewModel());
        }

        private Aposta Apostar(List<byte> numeros)
        {
            var megasena = new Megasena();
            var aposta = new Aposta(new Megasena());
            aposta.Realiza(numeros);

            return aposta;
        }

        public ActionResult ApostarSurpresinha()
        {
            if (TempData["numerosSorteados"] == null)
                TempData.Remove("numerosSorteados");

            if (TempData["apostas"] == null)
                TempData["apostas"] = new List<Aposta>();

            TempData.Keep("apostas");

            var numeros = new List<byte>();

            var aposta = Apostar();

            var apostas = TempData["apostas"] as List<Aposta>;
            apostas.Add(aposta);

            TempData["apostas"] = apostas;

            return View("Index", new ApostaViewModel());
        }

        private Aposta Apostar()
        {
            var megasena = new Megasena();
            var aposta = new Aposta(new Megasena());
            aposta.Realiza();

            return aposta;
        }

        public ActionResult Sortear()
        {
            if (TempData["apostas"] == null)
                TempData["apostas"] = new List<Aposta>();

            TempData.Keep("apostas");

            var megasena = new Megasena();
            TempData["numerosSorteados"] = megasena.Sorteia();

            return View("Index", new ApostaViewModel());
        }

        [OutputCache(Duration = 1)]
        public PartialViewResult _Lista()
        {
            if (TempData["apostas"] == null)
                TempData["apostas"] = new List<Aposta>();

            var apostas = TempData["apostas"] as List<Aposta>;

            return PartialView("_Lista", apostas);
        }
    }
}