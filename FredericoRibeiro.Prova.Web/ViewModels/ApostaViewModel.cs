using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FredericoRibeiro.Prova.Web.ViewModels
{
    public class ApostaViewModel
    {
        [Required(ErrorMessage = "Os números são de preenchimento obrigatório")]
        [Display(Name = "Números")]
        public string Numeros { get; set; }
    }
}