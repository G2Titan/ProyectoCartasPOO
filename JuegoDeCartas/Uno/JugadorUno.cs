using JuegoDeCartas.Clases_Abstractas;
using JuegoDeCartas.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace JuegoDeCartas.Uno
{
    public abstract class JugadorUno : Jugador
    {
        public JugadorUno(string nombre, IEstrategiaJuego estrategia) : base(nombre, estrategia) { }

        public abstract CartaUno SeleccionarCarta(CartaUno cartaSuperior, Jugador siguienteJugador);
    }
}