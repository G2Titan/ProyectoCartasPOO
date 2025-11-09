using System;
using System.Collections.Generic;
namespace JuegoDeCartas.Clases_Abstractas;
using JuegoDeCartas.Interfaces;
public abstract class Juego
{
   
    protected List<Jugador> jugadores;
    protected Baraja? baraja;
    protected List<Carta> pilaDescarte;
    protected bool juegoTerminado;

    public Juego()
    {
        this.jugadores = new List<Jugador>();
        this.pilaDescarte = new List<Carta>();
        this.juegoTerminado = false;
    }

  
    public void IniciarSimulacion()
    {
        LoggearAccion("--- Iniciando Simulación ---");
        
     
        while (!juegoTerminado)
        {
          
            EjecutarLogicaJuego();
            
            
            juegoTerminado = VerificarCondicionFinJuego();
        }

        LoggearAccion("--- Simulación Terminada ---");
        
        AnunciarGanador();
    }

   
    public abstract void ConfigurarJuego(List<IEstrategiaJuego> estrategiasJugadores);

    protected abstract void EjecutarLogicaJuego();


    public abstract int CalcularPuntos(Jugador jugador);

    
    protected abstract bool VerificarCondicionFinJuego();

   
    protected abstract void AnunciarGanador();

    
    public void LoggearAccion(string mensaje)
    {
        Console.WriteLine($"[SIM]: {mensaje}");
    }
}