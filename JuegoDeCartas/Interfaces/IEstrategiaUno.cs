using System.Collections.Generic;
using JuegoDeCartas.Clases_Abstractas;

namespace JuegoDeCartas.Interfaces;

public interface IEstrategiaUno
{
    // Selecciona una carta de la mano para jugar, o devuelve null para robar
    CartaUno? SeleccionarCarta(IReadOnlyList<Carta> mano, CartaUno cartaSuperior, Jugador siguienteJugador);
}