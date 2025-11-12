using System.Collections.Generic;
using System.Linq;
namespace JuegoDeCartas.Clases_Abstractas;
using JuegoDeCartas.Interfaces;
using static JuegoDeCartas.Interfaces.IEstrategiaJuego;

public abstract class Jugador
{
    public string Nombre { get; private set; }
    public List<Carta> Mano { get; private set; }

   
    protected IEstrategiaJuego? estrategia;

    public Jugador(string nombre, IEstrategiaJuego? estrategia)
    {
        this.Nombre = nombre;
        this.estrategia = estrategia;
        this.Mano = new List<Carta>();
    }

    public AccionJuego EjecutarTurno(Juego juego)
    {
        if (estrategia == null) throw new InvalidOperationException("No strategy configured for this player.");
        return estrategia.DecidirAccion(this, juego);
    }

    public Carta SeleccionarCarta(Juego juego)
    {
        if (estrategia == null) throw new InvalidOperationException("No strategy configured for this player.");
        return estrategia.SeleccionarCarta(this, juego);
    }

    // Exponer la estrategia para casos donde se necesite acceder desde juegos concretos
    public IEstrategiaJuego? Estrategia => estrategia;

    public void TomarCarta(Carta carta)
    {
        if (carta != null) Mano.Add(carta);
    }

    public void JugarCarta(Carta carta)
    {
        Mano.Remove(carta);
    }
}
