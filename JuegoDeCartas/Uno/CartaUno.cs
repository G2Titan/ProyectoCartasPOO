using System;
using JuegoDeCartas.Clases_Abstractas;

public class CartaUno : Carta
{
    public ColorUno Color { get; private set; }
    public ValorUno Valor { get; private set; }

    public CartaUno(ColorUno color, ValorUno valor)
    {
        Color = color;
        Valor = valor;
    }

    public override string GetRepresentacion()
    {
        return $"{Valor} de {Color}";
    }
}