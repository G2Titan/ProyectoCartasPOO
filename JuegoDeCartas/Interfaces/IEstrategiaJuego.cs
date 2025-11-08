using JuegoDeCartas.Clases_Abstractas;
namespace JuegoDeCartas.Interfaces;
public interface IEstrategiaJuego
{
  AccionJuego DecidirAccion(Jugador jugador, Juego juego);
  Carta SeleccionarCarta(Jugador jugador, Juego juego);
  public enum AccionJuego { 
    //Blackjack
    PedirCarta,
    Quedarse,
    //Uno
    JugarCarta,
    RobarCarta 
    }
}