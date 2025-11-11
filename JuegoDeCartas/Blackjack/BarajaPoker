using System;
using JuegoDeCartas.Clases_Abstractas;

public class BarajaPoker : Baraja
{
    public override void InicializarBaraja()
    {
        cartas.Clear();

        foreach (Palo palo in Enum.GetValues(typeof(Palo)))
        {
            foreach (ValorPoker valor in Enum.GetValues(typeof(ValorPoker)))
            {
                cartas.Add(new CartaPoker(palo, valor));
            }
        }
    }
}
 
 