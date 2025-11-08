using System.Collections.Generic;
using System.Linq;
namespace JuegoDeCartas.Clases_Abstractas;
using JuegoDeCartas.Interfaces;
using static JuegoDeCartas.Interfaces.IEstrategiaJuego;

public class Jugador
{
    public string Nombre { get; private set; }
    public List<Carta> Mano { get; private set; }
    
   
    private IEstrategiaJuego estrategia;

    public Jugador(string nombre, IEstrategiaJuego estrategia)
    {
        this.Nombre = nombre;
        this.estrategia = estrategia;
        this.Mano = new List<Carta>();
    }

   
    public AccionJuego EjecutarTurno(ContextoJuego contexto)
    {
        return estrategia.DecidirAccion(this.Mano, contexto);
    }

    public void TomarCarta(Carta carta)
    {
        if (carta != null)
        {
            Mano.Add(carta);
        }
    }

    public void JugarCarta(Carta carta)
    {
        Mano.Remove(carta);
    }

   
    public int CalcularPuntosBlackjack()
    {
        int total = 0;
        int ases = 0;

        foreach (Carta c in Mano)
        {
            CartaPoker cartaPoker = (CartaPoker)c;
            total += cartaPoker.GetPuntosBlackjack();
            if (cartaPoker.Valor == ValorPoker.As)
            {
                ases++;
            }
        }

       
        while (total > 21 && ases > 0)
        {
            total -= 10;
            ases--;
        }

        return total;
    }
}