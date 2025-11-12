using JuegoDeCartas.Interfaces; 
using JuegoDeCartas.Clases_Abstractas;

public class EstrategiaTemeraria : IEstrategiaJuego
{
   
    public IEstrategiaJuego.AccionJuego DecidirAccion(Jugador jugador, Juego juego)
    {
        int puntosActuales = juego.CalcularPuntos(jugador);
        
        if (puntosActuales < 21)
        {
            return IEstrategiaJuego.AccionJuego.PedirCarta;
        }
        else
        {
            return IEstrategiaJuego.AccionJuego.Quedarse;
        }
    }

   
    public Carta SeleccionarCarta(Jugador jugador, Juego juego)
    {
        return null;
    }
}