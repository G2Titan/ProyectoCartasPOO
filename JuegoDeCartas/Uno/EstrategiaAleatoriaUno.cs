using System;
using System.Linq;
using System.Collections.Generic;
using JuegoDeCartas.Clases_Abstractas;
using JuegoDeCartas.Interfaces;

namespace JuegoDeCartas.Uno
{
    public class EstrategiaAleatoriaUno : IEstrategiaJuego, IEstrategiaUno
    {
        private readonly Random rng = new Random();

    public IEstrategiaJuego.AccionJuego DecidirAccion(Jugador jugador, Juego juego)
    {
      throw new NotImplementedException();
    }

    public CartaUno SeleccionarCarta(IReadOnlyList<Carta> mano, CartaUno cartaSuperior, Jugador siguienteJugador)
        {
      var manoUno = mano.OfType<CartaUno>().ToList();

      // Preferencia ante cartas no comodin
      var validNoWild = manoUno
        .Where(c => c.Color != ColorUno.Comodin && (c.Color == cartaSuperior.Color || c.Valor == cartaSuperior.Valor))
        .ToList();

      if (validNoWild.Count > 0)
      {
        if (rng.NextDouble() < 0.3) return null; // 30% chance de pasar
        return validNoWild[rng.Next(validNoWild.Count)];
      }

      //wilds hace referencia a las cartas comodin para saber que los comodines siempre son salvajes y que casi explota mi laptop
      var wilds = manoUno.Where(c => c.Color == ColorUno.Comodin).ToList();
      if (wilds.Count == 0) return null;
      if (rng.NextDouble() < 0.3) return null; // pequeña probabilidad de pasar incluso con comodin
      return wilds[rng.Next(wilds.Count)];
        }

    public Carta SeleccionarCarta(Jugador jugador, Juego juego)
    {
      throw new NotImplementedException();
    }
  }
}