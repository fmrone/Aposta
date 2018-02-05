using FredericoRibeiro.Prova.Dominio.Contrato;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FredericoRibeiro.Prova.Dominio
{
    public class Megasena : Jogo
    {
        public Megasena()
        {
            Quantidade = 6;
            MenorNumero = 1;
            MaiorNumero = 60;
            PermiteRepetirNumero = false;
        }
    }
}
