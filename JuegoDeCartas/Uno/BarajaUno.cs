using System;
using System.Collections.Generic;
using System.Linq;

public class BarajaUno : Baraja
{
  public override void InicializarBaraja()
  {
    cartas.Clear();

    var colores = new List<ColorUno> { ColorUno.Rojo, ColorUno.Verde, ColorUno.Azul, ColorUno.Amarillo };

    var valoresNormales = new List<ValorUno>
    {
      ValorUno.Cero,
      ValorUno.Uno,
      ValorUno.Dos,
      ValorUno.Tres,
      ValorUno.Cuatro,
      ValorUno.Cinco,
      ValorUno.Seis,
      ValorUno.Siete,
      ValorUno.Ocho,
      ValorUno.Nueve,
      ValorUno.Salta,
      ValorUno.Reversa,
      ValorUno.TomaDos
    };

    foreach (var color in colores)
    {
      cartas.Add(new CartaUno(color, ValorUno.Cero));
      foreach (var valor in valoresNormales.Where(v => v != ValorUno.Cero))
      {
        cartas.Add(new CartaUno(color, valor));
        cartas.Add(new CartaUno(color, valor));
      }
    }

    for (int i = 0; i < 4; i++)
    {
      cartas.Add(new CartaUno(ColorUno.Comodin, ValorUno.Comodin));
      cartas.Add(new CartaUno(ColorUno.Comodin, ValorUno.ComodinTomaCuatro));
    }

    Barajear();
  }
}
