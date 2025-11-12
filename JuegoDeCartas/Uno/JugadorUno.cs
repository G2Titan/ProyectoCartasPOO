using JuegoDeCartas.Clases_Abstractas;
using JuegoDeCartas.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace JuegoDeCartas.Uno
{
  public class JugadorUno : Jugador
  {
    protected IEstrategiaUno estrategiaUno;

    // Mantener compatibilidad con constructor anterior
    public JugadorUno(string nombre, IEstrategiaJuego estrategia) : base(nombre, estrategia) { }

    // Nuevo constructor para estrategias de Uno
    public JugadorUno(string nombre, IEstrategiaUno estrategiaUno) : base(nombre, null)
    {
      this.estrategiaUno = estrategiaUno;
    }

    // Delegar la selección de carta a la estrategia de Uno si está presente
    public virtual CartaUno SeleccionarCarta(CartaUno cartaSuperior, Jugador siguienteJugador)
    {
      if (estrategiaUno != null)
      {
        return estrategiaUno.SeleccionarCarta(Mano.AsReadOnly(), cartaSuperior, siguienteJugador);
      }

      throw new System.NotImplementedException("No hay estrategia de Uno configurada para este jugador.");
    }
  }
}