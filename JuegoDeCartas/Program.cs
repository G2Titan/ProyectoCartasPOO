using System;
using System.Collections.Generic;

using JuegoDeCartas.Clases_Abstractas;
using JuegoDeCartas.Interfaces;
using JuegoDeCartas.Blackjack;

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("--- Bienvenido al Simulador de Juegos de Cartas ---");
        
        bool salir = false;
        while (!salir)
        {
            Console.WriteLine("\n--- MENÚ PRINCIPAL ---");
            Console.WriteLine("¿Qué juego deseas simular?");
            Console.WriteLine("1. 21 Blackjack");
            Console.WriteLine("2. Uno (No implementado aún)");
            Console.WriteLine("3. Salir");
            Console.Write("Selecciona una opción: ");
            
            string opcion = Console.ReadLine();
            
            Juego juegoASimular = null;

            switch (opcion)
            {
                case "1":
                    juegoASimular = ConfigurarBlackjack();
                    break;
                
                case "2":
                    // juegoASimular = ConfigurarUno();
                    Console.WriteLine("Juego 'Uno' aún no está implementado.");
                    break;
                
                case "3":
                    salir = true;
                    break;
                
                default:
                    Console.WriteLine("Opción no válida. Inténtalo de nuevo.");
                    break;
            }

            if (juegoASimular != null)
            {
                Console.Clear();
                juegoASimular.IniciarSimulacion();
                
                Console.WriteLine("\n--- Simulación Finalizada ---");
                Console.WriteLine("Presiona Enter para volver al menú...");
                Console.ReadLine();
                Console.Clear();
            }
        }
        
        Console.WriteLine("Gracias por usar el simulador. ¡Adiós!");
    }

    private static Juego ConfigurarBlackjack()
    {
        Console.Clear();
        Console.WriteLine("--- Configurando 21 Blackjack ---");
        
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

        Console.WriteLine("Jugadores IA configurados:");
        Console.WriteLine(" - Jugador 1: Temerario (siempre pide si < 21)");
        Console.WriteLine(" - Jugador 2: Cauteloso (se queda en 15+)");
        Console.WriteLine(" - Jugador 3: Cauteloso (se queda en 18+)");

        Juego blackjack = new JuegoBlackjack(rondas);
        blackjack.ConfigurarJuego(estrategias);

        Console.WriteLine("\n¡Juego listo! Presiona Enter para comenzar...");
        Console.ReadLine();
        
        return blackjack;
    }
}