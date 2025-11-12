using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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