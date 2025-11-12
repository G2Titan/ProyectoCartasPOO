using System.Linq;
using System.Collections.Generic;
using JuegoDeCartas.Clases_Abstractas;
using JuegoDeCartas.Interfaces;

namespace JuegoDeCartas.Uno
{
    public class EstrategiaCalculadoraUno : IEstrategiaUno, IEstrategiaJuego
    {
    public IEstrategiaJuego.AccionJuego DecidirAccion(Jugador jugador, Juego juego)
    {
      throw new NotImplementedException();
    }

    public CartaUno SeleccionarCarta(IReadOnlyList<Carta> mano, CartaUno cartaSuperior, Jugador siguienteJugador)
        {
            var manoUno = mano.OfType<CartaUno>().ToList();

            var cartasValidas = manoUno
                .Where(c => c.Color == cartaSuperior.Color || c.Valor == cartaSuperior.Valor || c.Color == ColorUno.Comodin)
                .ToList();

            if (cartasValidas.Count == 0) return null;

            bool siguienteConUna = siguienteJugador.Mano.Count == 1;

            // Si el siguiente tiene una carta entonces intenta lanzar carta especial
            if (siguienteConUna)
            {
                var cartasEspeciales = cartasValidas.Where(c =>
                    c.Valor == ValorUno.TomaDos || c.Valor == ValorUno.ComodinTomaCuatro || c.Valor == ValorUno.Salta || c.Valor == ValorUno.Reversa).ToList();
                // Priorizar cartas que hacen robar
                var cartasToma = cartasEspeciales.Where(c => c.Valor == ValorUno.TomaDos || c.Valor == ValorUno.ComodinTomaCuatro).ToList();
                if (cartasToma.Any()) return cartasToma.First();
                if (cartasEspeciales.Any()) return cartasEspeciales.First();
                // Si no tiene ninguna especial entonces roba una
                return null;
            }
            // Juega una carta NO especial si es posible por default
            var cartasNormales = cartasValidas.Where(c =>
                c.Valor <= ValorUno.Nueve).ToList();
            if (cartasNormales.Any()) return cartasNormales.First();
            // Si solo quedan especiales, usa una
            return cartasValidas.First();
        }

    public Carta SeleccionarCarta(Jugador jugador, Juego juego)
    {
      throw new NotImplementedException();
    }
  }
}