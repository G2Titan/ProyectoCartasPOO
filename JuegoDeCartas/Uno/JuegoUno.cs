using System;
using System.Collections.Generic;
using System.Linq;
using JuegoDeCartas.Clases_Abstractas;
using JuegoDeCartas.Interfaces;
namespace JuegoDeCartas.Uno;

public class JuegoUno : Juego
{
  private new BarajaUno baraja;
  private List<CartaUno> mesaDeJuego;
  private int indiceJugadorActual;
  private bool sentidoHorario;
  public JuegoUno(List<Jugador> jugadores)
  {
    this.jugadores = jugadores;
    this.baraja = new BarajaUno();
    this.mesaDeJuego = new List<CartaUno>();
    this.indiceJugadorActual = 0;
    this.sentidoHorario = true;
    this.juegoTerminado = false;
  }
  public void InicializarJuego()
  {     
    baraja.InicializarBaraja(); 
    // Repartir 3 cartas a cada jugador
    foreach (Jugador jugador in jugadores)
    {
      for (int i = 0; i < 3; i++)
      {
        Carta carta = baraja.RepartirCarta();
        if (carta != null)
        {
          jugador.TomarCarta(carta);
        }
      }
    }
    // Carta inicial en la mesa
    Carta cartaInicial = baraja.RepartirCarta();
    if (cartaInicial is CartaUno cartaUnoInicial)
    {
      mesaDeJuego.Add(cartaUnoInicial);
    }
  }
public void JugarRonda()
{
  JugadorUno jugadorActual = (JugadorUno)jugadores[indiceJugadorActual];
  Jugador siguienteJugador = jugadores[(indiceJugadorActual + 1) % jugadores.Count];
  CartaUno cartaSuperior = mesaDeJuego.Last();
  LoggearAccion($"{jugadorActual.Nombre} está jugando... Carta superior: {cartaSuperior}");

  CartaUno? cartaJugada = jugadorActual.SeleccionarCarta(cartaSuperior, siguienteJugador);

  if (cartaJugada != null)
  {
    jugadorActual.JugarCarta(cartaJugada);
    mesaDeJuego.Add(cartaJugada);
    LoggearAccion($"{jugadorActual.Nombre} juega {cartaJugada}");
    AplicarEfectoCarta(cartaJugada);
  }
  else
  {
    Carta? nueva = baraja.RepartirCarta();
    jugadorActual.TomarCarta(nueva);
    LoggearAccion($"{jugadorActual.Nombre} toma una carta del mazo");
  }

  if (jugadorActual.Mano.Count == 1)
  {
    LoggearAccion($"¡{jugadorActual.Nombre} grita UNO!");
  }
  
  VerificarCondicionFinJuego();
  AvanzarJugador();
}


  private void AplicarEfectoCarta(CartaUno carta)
  {
    switch (carta.Valor)
    {
      case ValorUno.TomaDos:
        EfectoTomaDos();
        break;

      case ValorUno.Reversa:
        EfectoReversa();
        break;

      case ValorUno.Salta:
        EfectoSalta();
        break;

      case ValorUno.ComodinTomaCuatro:
        EfectoComodinTomaCuatro();
        break;
    }
  }
  private void EfectoTomaDos()
  {
    AvanzarJugador();
    Carta? carta1 = baraja.RepartirCarta();
    Carta? carta2 = baraja.RepartirCarta();
    jugadores[indiceJugadorActual].TomarCarta(carta1);
    jugadores[indiceJugadorActual].TomarCarta(carta2);
  }

  private void EfectoReversa()
  {
    sentidoHorario = !sentidoHorario;
  }

  private void EfectoSalta()
  {
    AvanzarJugador(); // Salta al siguiente jugador
  }

  private void EfectoComodinTomaCuatro()
  {
    AvanzarJugador();
    for (int i = 0; i < 4; i++)
    {
      Carta carta = baraja.RepartirCarta();
      jugadores[indiceJugadorActual].TomarCarta(carta);
    }
  }
  private void AvanzarJugador()
  {
    if (sentidoHorario)
    {
      indiceJugadorActual = (indiceJugadorActual + 1) % jugadores.Count;
    }
    else
    {
      indiceJugadorActual = (indiceJugadorActual - 1 + jugadores.Count) % jugadores.Count;
    }
  }

  public bool VerificarGanador()
  {
    return jugadores[indiceJugadorActual].Mano.Count == 0;
  }
  public Jugador? ObtenerGanador()
  {
    return VerificarGanador() ? jugadores[indiceJugadorActual] : null;
  }
  
  protected override bool VerificarCondicionFinJuego()
  {
    return VerificarGanador();
  }
  public override void ConfigurarJuego(List<IEstrategiaJuego> estrategias)
  {
    // Configurar estrategias de acuerdo a los jugadores
    NotImplementedException notImplementedException = new NotImplementedException();
    throw notImplementedException;
  }

  public override int CalcularPuntos(Jugador jugador)
  {
    return 0;
  }
  protected override void EjecutarLogicaJuego()
  {
    InicializarJuego(); 
      while (!VerificarCondicionFinJuego())
      {
        JugarRonda();
      }
  }
  protected override void AnunciarGanador()
  {
    Jugador? ganador = ObtenerGanador();
    if (ganador != null)
    {
      LoggearAccion($"¡{ganador.Nombre} ha ganado!");
    }
    else
      {
        LoggearAccion("No hay ganador");
      }
  }
}
