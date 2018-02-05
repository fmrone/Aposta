using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FredericoRibeiro.Prova.Dominio.Contrato
{
    public abstract class Jogo
    {
        private IList<string> _notificacoes;

        public Jogo()
        {
            //Quantidade = quantidade;
            //MenorNumero = menorNumero;
            //MaiorNumero = maiorNumero;
            //PermiteRepetirNumero = permiteRepetirNumero;

            _notificacoes = new List<string>();
        }

        protected int Quantidade { get; set; }
        protected int MenorNumero { get; set; }
        protected int MaiorNumero { get; set; }
        protected bool PermiteRepetirNumero { get; set; }

        public IReadOnlyList<string> Notificacoes { get { return _notificacoes.ToList(); } }

        public IList<byte> Aposta(IList<byte> numeros)
        {
            if (numeros.Count != Quantidade)
                _notificacoes.Add("Quantidade de números inválido");

            if (numeros.Where(n => n < MenorNumero).Count() > 0)
                _notificacoes.Add(string.Format("Você informou número menor que {0}", MenorNumero));

            if (numeros.Where(n => n > MaiorNumero).Count() > 0)
                _notificacoes.Add(string.Format("Você informou número maior que {0}", MaiorNumero));

            if (!PermiteRepetirNumero)
                if (numeros.GroupBy(g => g).Select(n => n.Count() > 1).Any(n => n))
                    _notificacoes.Add("Você informou números repetidos");

            if (_notificacoes.Count > 0)
            {
                _notificacoes.Add("Aposte novamente");

                return new List<byte>();
            }

            return numeros.OrderBy(o => o).ToList();
        }

        public IList<byte> Aposta()
        {
            return GeraNumeros();
        }

        public IList<byte> Sorteia()
        {
            return GeraNumeros();
        }

        private IList<byte> GeraNumeros()
        {
            var numeros = new List<byte>();

            do
            {
                var numero = GeraNumero();

                if (!PermiteRepetirNumero)
                {
                    if (numeros.Contains(numero))
                        continue;
                    else
                        numeros.Add(numero);
                }
                else
                {
                    numeros.Add(numero);
                }
            } while (numeros.Count < Quantidade);

            return numeros.OrderBy(o => o).ToList();
        }

        private byte GeraNumero()
        {
            return (byte)(new Random().Next(MenorNumero, MaiorNumero));
        }
    }

   
}
