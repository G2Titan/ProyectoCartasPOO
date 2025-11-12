using JuegoDeCartas.Interfaces; 
using JuegoDeCartas.Clases_Abstractas;

public class EstrategiaCautelosa : IEstrategiaJuego
{
    private int puntoCorte;

    public EstrategiaCautelosa(int puntoCorte)
    {
        this.puntoCorte = puntoCorte;
    }

   
    public IEstrategiaJuego.AccionJuego DecidirAccion(Jugador jugador, Juego juego)
    {
        int puntosActuales = juego.CalcularPuntos(jugador);

        if (puntosActuales < puntoCorte)
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