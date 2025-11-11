using System;
using System.Collections.Generic;
using System.Linq;
using JuegoDeCartas.Interfaces; 

public class JuegoBlackjack : Juego
{
    private Jugador dealer;
    private int rondasTotales;
    private int rondaActual;
    private Dictionary<string, int> victorias;

    public JuegoBlackjack(int rondas)
    {
        this.rondasTotales = rondas;
        this.rondaActual = 0;
        this.victorias = new Dictionary<string, int>();
        
        this.dealer = new JugadorConcreto("Dealer", new EstrategiaCautelosa(17));
    }

    public override void ConfigurarJuego(List<IEstrategiaJuego> estrategiasJugadores)
    {
        baraja = new BarajaPoker();
        jugadores.Clear();
        victorias.Clear();

        int i = 1;
        foreach (var estrategia in estrategiasJugadores)
        {
            string nombre = $"Jugador {i++}";
            
            jugadores.Add(new JugadorConcreto(nombre, estrategia));
            victorias[nombre] = 0;
        }
        
        LoggearAccion($"Juego configurado con {jugadores.Count} jugadores.");
    }

    
    public override int CalcularPuntos(Jugador jugador)
    {
        int total = 0;
        int ases = 0;

        foreach (Carta c in jugador.Mano)
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

    protected override void EjecutarLogicaJuego()
    {
        rondaActual++;
        LoggearAccion($"\n--- Iniciando Ronda {rondaActual}/{rondasTotales} ---");

        
        baraja.InicializarBaraja();
        baraja.Barajear();
        pilaDescarte.Clear();
        dealer.Mano.Clear();
        foreach (var j in jugadores) { j.Mano.Clear(); }

        RepartirCarta(dealer);
        RepartirCarta(dealer);
        foreach (var j in jugadores)
        {
            RepartirCarta(j);
            RepartirCarta(j);
        }
        

        foreach (var jugador in jugadores)
        {
            EjecutarTurnoJugador(jugador);
        }

        EjecutarTurnoDealer();

        DeterminarGanadoresRonda();
    }

    private void EjecutarTurnoJugador(Jugador jugador)
    {
        bool turnoActivo = true;
        while (turnoActivo)
        {
            int puntos = CalcularPuntos(jugador);
            
            if (puntos > 21)
            {
                LoggearAccion($"{jugador.Nombre} tiene {puntos} y se ha pasado.");
                turnoActivo = false;
                continue;
            }

            LoggearAccion($"Turno de {jugador.Nombre} (Puntos: {puntos})");
            
            
            IEstrategiaJuego.AccionJuego accion = jugador.EjecutarTurno(this); 

         
            if (accion == IEstrategiaJuego.AccionJuego.PedirCarta)
            {
                LoggearAccion($"{jugador.Nombre} pide carta.");
                RepartirCarta(jugador);
            }
            else 
            {
                LoggearAccion($"{jugador.Nombre} se queda con {puntos}.");
                turnoActivo = false;
            }
            
        }
    }

    private void EjecutarTurnoDealer()
    {
        LoggearAccion("--- Turno del Dealer ---");
        bool turnoActivo = true;
        while (turnoActivo)
        {
            int puntos = CalcularPuntos(dealer);
            
           
            IEstrategiaJuego.AccionJuego accion = dealer.EjecutarTurno(this); 

            if (puntos > 21)
            {
                LoggearAccion($"Dealer tiene {puntos} y se ha pasado.");
                turnoActivo = false;
            }
            else if (accion == IEstrategiaJuego.AccionJuego.PedirCarta)
            {
                LoggearAccion($"Dealer pide carta (Puntos: {puntos}).");
                RepartirCarta(dealer);
            }
            else 
            {
                LoggearAccion($"Dealer se queda con {puntos}.");
                turnoActivo = false;
            }
            
        }
    }

    private void DeterminarGanadoresRonda()
    {
       
        LoggearAccion("--- Fin de la Ronda ---");
        int puntosDealer = CalcularPuntos(dealer);

        foreach (var jugador in jugadores)
        {
            int puntosJugador = CalcularPuntos(jugador);
            
            if (puntosDealer > 21 && puntosJugador <= 21)
            {
                LoggearAccion($"{jugador.Nombre} GANA (Dealer se pasó).");
                victorias[jugador.Nombre]++;
            }
            else if (puntosJugador > puntosDealer && puntosJugador <= 21)
            {
                LoggearAccion($"{jugador.Nombre} GANA ({puntosJugador} vs {puntosDealer}).");
                victorias[jugador.Nombre]++;
            }
            else
            {
                LoggearAccion($"{jugador.Nombre} PIERDE ({puntosJugador} vs {puntosDealer}).");
            }
        }
    }

    private void RepartirCarta(Jugador jugador)
    {
        // ... (Sin cambios aquí) ...
        if (baraja.EstaVacia())
        {
            LoggearAccion("¡La baraja se ha quedado vacía!");
            return;
        }
        Carta carta = baraja.RepartirCarta();
        jugador.TomarCarta(carta);
    }

    protected override bool VerificarCondicionFinJuego()
    {
       
        return rondaActual >= rondasTotales;
    }

    protected override void AnunciarGanador()
    {
       
        LoggearAccion("--- FIN DEL JUEGO ---");
        if (victorias.Count == 0)
        {
            LoggearAccion("No hubo jugadores.");
            return;
        }

        int maxVictorias = victorias.Values.Max();
        
        if (maxVictorias == 0)
        {
            LoggearAccion("¡No hubo ganadores!");
            return;
        }

        List<string> ganadores = victorias
            .Where(kvp => kvp.Value == maxVictorias)
            .Select(kvp => kvp.Key)
            .ToList();

        if (ganadores.Count > 1)
        {
            LoggearAccion($"¡Empate! Ganadores: {string.Join(", ", ganadores)} con {maxVictorias} victorias.");
        }
        else
        {
            LoggearAccion($"¡Ganador: {ganadores[0]} con {maxVictorias} victorias!");
        }
    }
}