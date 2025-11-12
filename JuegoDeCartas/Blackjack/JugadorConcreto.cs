using JuegoDeCartas.Clases_Abstractas;
using JuegoDeCartas.Interfaces;
public class JugadorConcreto : Jugador
{
    public JugadorConcreto(string nombre, IEstrategiaJuego estrategia)
        : base(nombre, estrategia)
    {


    }
}
