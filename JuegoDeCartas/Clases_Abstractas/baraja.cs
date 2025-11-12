using System;
using System.Collections.Generic;
using System.Linq;

public abstract class Baraja
{
    protected List<Carta> cartas;

    public Baraja()
    {
        this.cartas = new List<Carta>();
    }

    public abstract void InicializarBaraja();

    public void Barajear()
    {
        Random rng = new Random();
        this.cartas = this.cartas.OrderBy(c => rng.Next()).ToList();
    }

    public Carta RepartirCarta()
    {
        if (EstaVacia())
        {
            return null; 
        }

        Carta carta = cartas[0];
        cartas.RemoveAt(0);
        return carta;
    }

    public bool EstaVacia()
    {
        return cartas.Count == 0;
    }

    public int CartasRestantes()
    {
        return cartas.Count;
    }
    public void ReintroducirCarta(Carta carta)
    {
        if (carta != null)
        {
            cartas.Add(carta);
        }
    }
    public void ReintroducirCartas(List<Carta> cartasAReintroducir)
    {
        if (cartasAReintroducir != null)
        {
            cartas.AddRange(cartasAReintroducir);
        }
    }
}