
using JuegoDeCartas.Clases_Abstractas;

public class CartaPoker : Carta
{
    public Palo Palo { get; private set; }
    public ValorPoker Valor { get; private set; }

    public CartaPoker(Palo palo, ValorPoker valor)
    {
        this.Palo = palo;
        this.Valor = valor;
    }

    public int GetPuntosBlackjack()
    {
        switch (Valor)
        {
            case ValorPoker.As:
                return 11;
            case ValorPoker.K:
            case ValorPoker.Q:
            case ValorPoker.J:
                return 10;
            default:
                return (int)Valor + 1;
        }
    }

    public override string GetRepresentacion()
    {
        return $"{Valor} de {Palo}";
    }
}