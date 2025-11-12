using System;
using System.Collections.Generic;
using System.Linq;

using JuegoDeCartas.Clases_Abstractas;
using JuegoDeCartas.Interfaces;
using JuegoDeCartas.Blackjack;
using JuegoDeCartas.Uno;

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("--- Bienvenido al Simulador de Juegos de Cartas ---");
        
        bool salir = false;
        while (!salir)
        {
            Console.WriteLine("\n--- MENÚ PRINCIPAL ---");
            Console.WriteLine("1. Pruebas de Baraja (Crear, Mostrar, Barajear)");
            Console.WriteLine("2. Simular 21 Blackjack");
            Console.WriteLine("3. Simular Uno");
            Console.WriteLine("4. Salir");
            Console.Write("Selecciona una opción: ");
            
            string? opcion = Console.ReadLine();
            if (string.IsNullOrEmpty(opcion)) continue;
            
            switch (opcion)
            {
                case "1":
                    PruebasDeBaraja();
                    break;
                
                case "2":
                    SimularBlackjack();
                    break;
                
                case "3":
                    SimularUno();
                    break;
                
                case "4":
                    salir = true;
                    break;
                
                default:
                    Console.WriteLine("Opción no válida. Inténtalo de nuevo.");
                    break;
            }
        }
        
        Console.WriteLine("\nGracias por usar el simulador. ¡Adiós!");
    }

    private static void PruebasDeBaraja()
    {
        Console.Clear();
        Console.WriteLine("--- PRUEBAS DE BARAJA ---");
        Console.WriteLine("¿Qué baraja deseas probar?");
        Console.WriteLine("1. Baraja Poker (Blackjack)");
        Console.WriteLine("2. Baraja Uno");
        Console.Write("Selecciona: ");
        
        string? opcion = Console.ReadLine();
        if (string.IsNullOrEmpty(opcion)) return;
        
        switch (opcion)
        {
            case "1":
                PruebaBarajaPoker();
                break;
            case "2":
                PruebaBarajaUno();
                break;
            default:
                Console.WriteLine("Opción no válida.");
                break;
        }
    }

    private static void PruebaBarajaPoker()
    {
        Console.Clear();
        Console.WriteLine("--- PRUEBA BARAJA POKER ---\n");
        
        BarajaPoker baraja = new BarajaPoker();
        baraja.InicializarBaraja();
        baraja.Barajear();
        
        Console.WriteLine($"Total de cartas en la baraja: {baraja.CartasRestantes()}\n");
        
        // Paso 1: Mostrar primeras 10 cartas
        Console.WriteLine("--- Primeras 10 cartas del deck (inicial) ---");
        var primerasCartas = new List<Carta>();
        for (int i = 0; i < 10 && !baraja.EstaVacia(); i++)
        {
            Carta carta = baraja.RepartirCarta();
            primerasCartas.Add(carta);
            Console.WriteLine($"{i + 1}. {carta}");
        }
        
        // Paso 2: Reintroducir, barajear y mostrar nuevamente
        Console.WriteLine("\n--- Reintroduciendo las 10 cartas, barajeando... ---");
        baraja.ReintroducirCartas(primerasCartas);
        baraja.Barajear();
        
        Console.WriteLine("\n--- Primeras 10 cartas después de barajear ---");
        int contador = 1;
        for (int i = 0; i < 10 && !baraja.EstaVacia(); i++)
        {
            Carta carta = baraja.RepartirCarta();
            Console.WriteLine($"{contador++}. {carta}");
        }
        
        Console.WriteLine("\n\nPresiona Enter para volver al menú...");
        Console.ReadLine();
    }

    private static void PruebaBarajaUno()
    {
        Console.Clear();
        Console.WriteLine("--- PRUEBA BARAJA UNO ---\n");
        
        BarajaUno baraja = new BarajaUno();
        baraja.InicializarBaraja();
        baraja.Barajear();
        
        Console.WriteLine($"Total de cartas en la baraja: {baraja.CartasRestantes()}\n");
        
        // Paso 1: Mostrar primeras 10 cartas
        Console.WriteLine("--- Primeras 10 cartas del deck (inicial) ---");
        var primerasCartas = new List<Carta>();
        for (int i = 0; i < 10 && !baraja.EstaVacia(); i++)
        {
            Carta carta = baraja.RepartirCarta();
            primerasCartas.Add(carta);
            Console.WriteLine($"{i + 1}. {carta}");
        }
        
        // Paso 2: Reintroducir, barajear y mostrar nuevamente
        Console.WriteLine("\n--- Reintroduciendo las 10 cartas, barajeando... ---");
        baraja.ReintroducirCartas(primerasCartas);
        baraja.Barajear();
        
        Console.WriteLine("\n--- Primeras 10 cartas después de barajear ---");
        int contador = 1;
        for (int i = 0; i < 10 && !baraja.EstaVacia(); i++)
        {
            Carta carta = baraja.RepartirCarta();
            Console.WriteLine($"{contador++}. {carta}");
        }
        
        Console.WriteLine("\n\nPresiona Enter para volver al menú...");
        Console.ReadLine();
    }

    private static void SimularBlackjack()
    {
        Console.Clear();
        Console.WriteLine("--- SIMULACIÓN BLACKJACK ---");
        
        Console.Write("¿Cuántas rondas deseas simular? ");
        int rondas;
        while (!int.TryParse(Console.ReadLine(), out rondas) || rondas <= 0)
        {
            Console.WriteLine("Por favor, ingresa un número válido (mayor a 0).");
            Console.Write("¿Cuántas rondas deseas simular? ");
        }

        List<IEstrategiaJuego> estrategias = new List<IEstrategiaJuego>();
        
        estrategias.Add(new EstrategiaTemeraria());
        estrategias.Add(new EstrategiaCautelosa(15));
        estrategias.Add(new EstrategiaCautelosa(18));

        Console.WriteLine("\n--- Jugadores configurados ---");
        Console.WriteLine(" - Jugador 1: Temerario (siempre pide si < 21)");
        Console.WriteLine(" - Jugador 2: Cauteloso (se queda en 15+)");
        Console.WriteLine(" - Jugador 3: Cauteloso (se queda en 18+)");

        Juego blackjack = new JuegoBlackjack(rondas);
        blackjack.ConfigurarJuego(estrategias);

        Console.WriteLine("\n¡Juego listo! Presiona Enter para comenzar...");
        Console.ReadLine();
        Console.Clear();
        
        blackjack.IniciarSimulacion();
        
        Console.WriteLine("\n--- Simulación Finalizada ---");
        Console.WriteLine("Presiona Enter para volver al menú...");
        Console.ReadLine();
    }

    private static void SimularUno()
    {
        Console.Clear();
        Console.WriteLine("--- SIMULACIÓN UNO ---");
        
        // Crear jugadores de Uno directamente
        // La estrategia está implícita en el tipo de jugador (Aleatorio, Calculador)
        // Crear estrategias específicas para Uno
        var estrategiaAleatoria = new EstrategiaAleatoriaUno();
        var estrategiaCalculadora = new EstrategiaCalculadoraUno();

        var jugadores = new List<Jugador>
        {
            new JugadorAleatorio("Jugador Aleatorio 1", estrategiaAleatoria),
            new JugadorCalculador("Jugador Calculador", estrategiaCalculadora),
            new JugadorAleatorio("Jugador Aleatorio 2", estrategiaAleatoria)
        };

        Console.WriteLine("--- Jugadores configurados ---");
        foreach (var jugador in jugadores)
        {
            Console.WriteLine($" - {jugador.Nombre} ({jugador.GetType().Name})");
        }

        Juego uno = new JuegoUno(jugadores);
        
        // Configurar con estrategias (aunque en Uno no se usan realmente)
        uno.ConfigurarJuego(new List<IEstrategiaJuego> 
        { 
            new EstrategiaAleatoriaUno(),
            new EstrategiaCalculadoraUno(),
            new EstrategiaAleatoriaUno()
        });

        Console.WriteLine("\n¡Juego listo! Presiona Enter para comenzar...");
        Console.ReadLine();
        Console.Clear();
        
        uno.IniciarSimulacion();
        
        Console.WriteLine("\n--- Simulación Finalizada ---");
        Console.WriteLine("Presiona Enter para volver al menú...");
        Console.ReadLine();
    }
}