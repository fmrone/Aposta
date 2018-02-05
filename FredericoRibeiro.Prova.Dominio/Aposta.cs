using FredericoRibeiro.Prova.Dominio.Contrato;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FredericoRibeiro.Prova.Dominio
{
    public class Aposta
    {
        private IList<byte> _numeros;
        private IList<string> _notificacaoes;

        public Aposta(Jogo jogo)
        {
            Jogo = jogo;
            Identificador = Guid.NewGuid();
            Data = DateTime.Now;

            _numeros = new List<byte>();
            _notificacaoes = new List<string>();
        }

        public Jogo Jogo { get; private set; }
        public Guid Identificador { get; private set; }
        public DateTime Data { get; private set; }
        public byte Acertos { get; private set; }

        public IReadOnlyList<byte> Numeros { get { return _numeros.ToList(); } }
        public IReadOnlyList<string> Notificacoes { get { return _notificacaoes.ToList(); } }

        public void Realiza()
        {
            _numeros.Clear();
            _notificacaoes.Clear();

            _numeros = Jogo.Aposta();
        }

        public void Realiza(IList<byte> numeros)
        {
            _numeros.Clear();
            _notificacaoes.Clear();

            _numeros = Jogo.Aposta(numeros);
            _notificacaoes = Jogo.Notificacoes.ToList();
        }

        public void Apura(IList<byte> numerosApostados, IList<byte> numerosSorteados)
        {
            var acertos = (from na in numerosApostados join ns in numerosSorteados on na equals ns
                           select ns).ToList().Count();

            Acertos = (byte)acertos;
        }
    }
}
