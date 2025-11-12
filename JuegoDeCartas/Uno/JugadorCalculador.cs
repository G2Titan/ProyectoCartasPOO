using JuegoDeCartas.Clases_Abstractas;
using JuegoDeCartas.Interfaces;
using System;
using System.Linq;

namespace JuegoDeCartas.Uno
{
  public class JugadorCalculador : JugadorUno
  {
    public JugadorCalculador(string nombre, IEstrategiaUno estrategiaUno) : base(nombre, estrategiaUno) { }
  }
}
