using JuegoDeCartas.Clases_Abstractas;
using JuegoDeCartas.Interfaces;
using System;
using System.Linq;

namespace JuegoDeCartas.Uno
{
  public class JugadorAleatorio : JugadorUno
  {
    private static Random rng = new Random();
    public JugadorAleatorio(string nombre, IEstrategiaJuego estrategia) : base(nombre, estrategia) { }
    public override CartaUno SeleccionarCarta(CartaUno cartaSuperior, Jugador siguienteJugador)
    {
      var cartasValidas = Mano.OfType<CartaUno>()
      .Where(c => c.Color == cartaSuperior.Color || c.Valor == cartaSuperior.Valor || c.Color == ColorUno.Comodin)
      .ToList();
      if (cartasValidas.Count == 0) return null;

      return cartasValidas[rng.Next(cartasValidas.Count)];
    }
  }
}
