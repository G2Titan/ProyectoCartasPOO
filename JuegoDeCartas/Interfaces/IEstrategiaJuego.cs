namespace JuegoDeCartas.Interfaces;
public interface IEstrategiaJuego
{
  //void AccionJuego();
  AccionJuego decidirAccion(Mano mano, Juego juego);
  ICarta SeleccionarCarta();
  //Carta SeleccionarCarta(Mano mano, Juego juego);
  public enum AccionJuego { 
    //Blackjack
    PedirCarta,
    Quedarse,
    //Uno
    JugarCarta,
    RobarCarta }
}

//--- IGNORE ---
public class Juego
{
}

public class Mano
{
}