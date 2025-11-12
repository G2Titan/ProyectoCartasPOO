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
    // Repartir 4 cartas a cada jugador
    foreach (Jugador jugador in jugadores)
    {
      for (int i = 0; i < 4; i++)
      {
        Carta carta = baraja.RepartirCarta();
        if (carta != null)
        {
          jugador.TomarCarta(carta);
          LoggearAccion($"{jugador.Nombre} recibe una carta");
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
  // Use the generic Jugador reference and prefer the IEstrategiaUno when available.
  Jugador jugadorActual = jugadores[indiceJugadorActual];
  Jugador siguienteJugador = jugadores[(indiceJugadorActual + 1) % jugadores.Count];
  CartaUno cartaSuperior = mesaDeJuego.Last();
  LoggearAccion($"{jugadorActual.Nombre} está jugando... Carta superior: {cartaSuperior}");

  CartaUno cartaJugada = null;

  // Primero intenta usar la estrategia expuesta en el jugador (si implementa IEstrategiaUno)
  if (jugadorActual.Estrategia is JuegoDeCartas.Interfaces.IEstrategiaUno estrategiaUno)
  {
    cartaJugada = estrategiaUno.SeleccionarCarta(jugadorActual.Mano.AsReadOnly(), cartaSuperior, siguienteJugador);
  }
  else if (jugadorActual is JugadorUno jugadorUno)
  {
    // si el jugador es un JugadorUno que tiene su propia estrategiaUno interna
    cartaJugada = jugadorUno.SeleccionarCarta(cartaSuperior, siguienteJugador);
  }

  if (cartaJugada != null)
  {
    // pqrq prevenir bug wilds
    bool esComodinTomaCuatro = cartaJugada.Valor == ValorUno.ComodinTomaCuatro;
    if (esComodinTomaCuatro)
    {
      // Si el jugador tenía otra carta válida que no sea comodin, no puede jugar ComodinTomaCuatro
      bool tieneAlternativa = jugadorActual.Mano.OfType<CartaUno>()
        .Any(c => c != cartaJugada && (c.Color == cartaSuperior.Color || c.Valor == cartaSuperior.Valor));
      if (tieneAlternativa)
      {
        LoggearAccion($"Juego ilegal: {jugadorActual.Nombre} intentó jugar Comodin Toma 4 aunque tenía otra carta válida. Fuerza a robar en su lugar.");
        Carta? nueva = baraja.RepartirCarta();
        if (nueva != null) jugadorActual.TomarCarta(nueva);
        LoggearAccion($"{jugadorActual.Nombre} toma una carta del mazo (penalizado por jugar Comodin Toma 4 ilegal)");
        // No aplicar efectos, terminamos la ronda
        VerificarCondicionFinJuego();
        AvanzarJugador();
        return;
      }
    }

    if (cartaSuperior.Valor == ValorUno.ComodinTomaCuatro && cartaJugada.Valor == ValorUno.ComodinTomaCuatro)
    {
      LoggearAccion($"Juego prohibido: No se permite 'Comodin Toma 4' encima de otro 'Comodin Toma 4'. {jugadorActual.Nombre} debe robar en su lugar.");
      Carta? nueva = baraja.RepartirCarta();
      if (nueva != null) jugadorActual.TomarCarta(nueva);
      VerificarCondicionFinJuego();
      AvanzarJugador();
      return;
    }

    if (cartaJugada.Color == ColorUno.Comodin)
    {
      bool tieneAlternativaNoWild = jugadorActual.Mano.OfType<CartaUno>()
        .Any(c => c.Color != ColorUno.Comodin && (c.Color == cartaSuperior.Color || c.Valor == cartaSuperior.Valor));
      if (tieneAlternativaNoWild)
      {
        LoggearAccion($"{jugadorActual.Nombre} intentó jugar un comodin aunque tenía alternativa no-comodin. Se fuerza a robar en su lugar para evitar loops.");
        Carta? nueva = baraja.RepartirCarta();
        if (nueva != null) jugadorActual.TomarCarta(nueva);
        VerificarCondicionFinJuego();
        AvanzarJugador();
        return;
      }
    }

    // Si la carta es un comodin, el jugador debe elegir un color — si no lo hace, elegimos uno por él
    if (cartaJugada.Color == ColorUno.Comodin)
    {
      // Remover la carta original de la mano
      jugadorActual.JugarCarta(cartaJugada);

      // Determinar color preferido
      ColorUno nuevoColor = ElegirColorPreferido(jugadorActual);

      // Colocar en mesa una representación de la carta con el color elegido
      CartaUno cartaEnMesa = new CartaUno(nuevoColor, cartaJugada.Valor);
      mesaDeJuego.Add(cartaEnMesa);
      LoggearAccion($"{jugadorActual.Nombre} juega {cartaJugada} y elige el color {nuevoColor}");

      // Aplicar efecto usando la carta en mesa (que conserva el tipo/valor)
      AplicarEfectoCarta(cartaEnMesa);
    }
    else
    {
      jugadorActual.JugarCarta(cartaJugada);
      mesaDeJuego.Add(cartaJugada);
      LoggearAccion($"{jugadorActual.Nombre} juega {cartaJugada}");
      AplicarEfectoCarta(cartaJugada);
    }
  }
  else
  {
    // Robar una carta
    Carta? nueva = baraja.RepartirCarta();
    if (nueva != null) jugadorActual.TomarCarta(nueva);
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
    LoggearAccion($"{jugadores[indiceJugadorActual].Nombre} toma 2 cartas por comodin: 'Toma Dos'");
  }

  private void EfectoReversa()
  {
    sentidoHorario = !sentidoHorario;
    LoggearAccion("Comodin: Ahora el sentido del juego ha sido revertido");
  }

  private void EfectoSalta()
  {
    AvanzarJugador(); // Salta al siguiente jugador
    LoggearAccion($"{jugadores[indiceJugadorActual].Nombre} es saltado por comodin: 'No juega'");
  }

  private void EfectoComodinTomaCuatro()
  {
    AvanzarJugador();
    for (int i = 0; i < 4; i++)
    {
      Carta carta = baraja.RepartirCarta();
      jugadores[indiceJugadorActual].TomarCarta(carta);
    }
    LoggearAccion($"{jugadores[indiceJugadorActual].Nombre} toma 4 cartas por comodin: 'Comodin Toma Cuatro'");
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

  // Elige el color preferido del jugador (el color que más cartas tiene en su mano), o un color aleatorio si no hay preferencia
  private ColorUno ElegirColorPreferido(Jugador jugador)
  {
    var contador = new Dictionary<ColorUno, int>();
    foreach (ColorUno c in Enum.GetValues(typeof(ColorUno)))
    {
      if (c == ColorUno.Comodin) continue;
      contador[c] = 0;
    }

    foreach (var c in jugador.Mano.OfType<CartaUno>())
    {
      if (c.Color == ColorUno.Comodin) continue;
      contador[c.Color]++;
    }

    var max = contador.OrderByDescending(kv => kv.Value).FirstOrDefault();
    if (max.Value > 0) return max.Key;

    // Si no tiene cartas de colores, devolvemos rojo por defecto
    return ColorUno.Rojo;
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
public override void ConfigurarJuego(List<IEstrategiaJuego> estrategiasJugadores)
{
  // Si los jugadores ya fueron provistos (por el constructor), no sobrescribimos la lista.
  if (jugadores != null && jugadores.Count > 0)
  {
    LoggearAccion("Jugadores ya provistos, omitiendo reconfiguración de jugadores.");
    return;
  }

  baraja = new BarajaUno();

  LoggearAccion("Configurando el juego de Uno (creando jugadores desde estrategias)...");

  int i = 1;
  foreach (var estrategia in estrategiasJugadores)
  {
    string nombre = $"Jugador {i++}";
    // Intentamos crear jugadores concretos de Uno si la estrategia es compatible
    if (estrategia is JuegoDeCartas.Interfaces.IEstrategiaUno estrategiaUno)
    {
      // Por defecto creamos un jugador aleatorio que delega en la estrategia
      Jugador nuevoJugador = new JugadorUno(nombre, estrategiaUno);
      jugadores.Add(nuevoJugador);
      LoggearAccion($"Jugador '{nombre}' (Uno) configurado con estrategia: {estrategia.GetType().Name}");
    }
    else
    {
      // Si no es una estrategia de Uno, creamos un jugador genérico
      Jugador nuevoJugador = new JugadorConcreto(nombre, estrategia);
      jugadores.Add(nuevoJugador);
      LoggearAccion($"Jugador '{nombre}' configurado con estrategia genérica: {estrategia.GetType().Name}");
    }
  }

  LoggearAccion($"Juego configurado con {jugadores.Count} jugadores.");
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
