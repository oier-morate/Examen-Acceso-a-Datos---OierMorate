using System;
using System.IO;

class Program
{
    static string[] lineas = File.ReadAllLines("acb2526.csv");

    static void Main(string[] args)
    {
        int opcion = 0;
        // Menu
        while (opcion != 6)
        {
            Console.WriteLine();
            Console.WriteLine("<-----| ACB 25-26 |----->");
            Console.WriteLine("1. Resultados de un equipo");
            Console.WriteLine("2. Resultados de una jornada");
            Console.WriteLine("3. Top 3 MVPs");
            Console.WriteLine("4. Clasificación");
            Console.WriteLine("5. Crear un fichero por cada equipo");
            Console.WriteLine("6. Salir");
            Console.Write("Elige una opción: ");
            int.TryParse(Console.ReadLine(), out opcion);

            switch (opcion)
            {
                case 1:
                    Equipo();
                    break;
                case 2:
                    Jornada();
                    break;
                case 3:
                    Mvps();
                    break;
                case 4:
                    Clasificacion();
                    break;
                case 5:
                    FicherosEquipos();
                    break;
                case 6:
                    Console.WriteLine("Saliendo...");
                    break;
                default:
                    Console.WriteLine("Opción no válida");
                    break;
            }
        }
    }

    // Resultados de un equipo
    static void Equipo()
    {
        // Como juegan todos los equipos solo hay 9 enfrentamientos por jornada
        string[] equipos = new string[18];
        for (int i = 0; i < 9; i++)
        {
            string[] datos = lineas[i + 1].Split(',');
            // datos[1] es el local y datos[2] el visitante
            equipos[i * 2] = datos[1];
            equipos[i * 2 + 1] = datos[2];
        }

        // Muestra la lista de equipos y elegimos uno por su número
        for (int i = 0; i < 18; i++)
        {
            Console.WriteLine((i + 1) + ". " + equipos[i]);
        }
        Console.Write("Elige un equipo (1-18): ");
        int numero = int.Parse(Console.ReadLine());
        if (numero < 1 || numero > 18)
        {
            Console.WriteLine("Equipo no válido");
            return;
        }
        string equipo = equipos[numero - 1];

        string texto = "";
        for (int i = 1; i < lineas.Length; i++)
        {
            string[] datos = lineas[i].Split(',');
            //Formato de la línea: Jornada,Local,Visitante,Marcador,MVP
            if (datos[1] == equipo || datos[2] == equipo)
            {
                texto = texto + lineas[i].Replace(",", " | ") + "\n";
            }
        }
        Console.WriteLine(texto);

        // Opcion de escribir el resultado en un fichero
        Console.Write("¿Guardar en un fichero? (s/n): ");
        if (Console.ReadLine() == "s")
        {
            Console.Write("Nombre del fichero: ");
            File.WriteAllText(Console.ReadLine() + ".txt", texto);
        }
    }

    // Resultados de una jornada
    static void Jornada()
    {
        Console.Write("Número de jornada: ");
        string numero = Console.ReadLine();

        string texto = "";
        for (int i = 1; i < lineas.Length; i++)
        {
            string[] datos = lineas[i].Split(',');
            //datos[0] es la jornada
            if (datos[0] == "Jornada " + numero)
            {
                texto = texto + lineas[i].Replace(",", " | ") + "\n";
            }
        }
        Console.WriteLine(texto);

        // Opcion de escribir el resultado en un fichero
        Console.Write("¿Guardar en un fichero? (s/n): ");
        if (Console.ReadLine() == "s")
        {
            Console.Write("Nombre del fichero: ");
            File.WriteAllText(Console.ReadLine() + ".txt", texto);
        }
    }

    // Top 3 de jugadores que más veces han sido MVP
    static void Mvps()
    {
        // Guarda el mvp de cada partido
        string[] mvps = new string[lineas.Length];
        for (int i = 1; i < lineas.Length; i++)
        {
            // datos[4] es el MVP
            mvps[i] = lineas[i].Split(',')[4];
        }

        // Por partido contamos cuántas veces aparece ese jugador como mvp
        int[] veces = new int[lineas.Length];
        for (int i = 1; i < lineas.Length; i++)
        {
            for (int j = 1; j < lineas.Length; j++)
            {
                if (mvps[i] == mvps[j])
                {
                    veces[i]++;
                }
            }
        }

        // Saca el que más veces sale, tres veces seguidas
        string texto = "TOP 3 MVPs\n";
        for (int puesto = 1; puesto <= 3; puesto++)
        {
            int mejor = 1;
            //combrueba cuál es el jugador que más veces ha sido mvp
            for (int i = 1; i < lineas.Length; i++)
            {
                if (veces[i] > veces[mejor])
                {
                    mejor = i;
                }
            }
            string nombre = mvps[mejor];
            //imprime el puesto, el nombre y las veces que ha sido mvp
            texto = texto + puesto + ". " + nombre + " - " + veces[mejor] + " veces\n";

            // Si ya se ha imprimido un jugador se  pone a 0 para que no vuelva a salir
            for (int i = 1; i < lineas.Length; i++)
            {
                if (mvps[i] == nombre)
                {
                    veces[i] = 0;
                }
            }
        }
        Console.WriteLine(texto);

        // Opcion de escribir el resultado en un fichero
        Console.Write("¿Guardar en un fichero? (s/n): ");
        if (Console.ReadLine() == "s")
        {
            Console.Write("Nombre del fichero: ");
            File.WriteAllText(Console.ReadLine() + ".txt", texto);
        }
    }

    // Clasificación por victorias y derrotas
    static void Clasificacion()
    {
        string[] equipos = new string[18];
        for (int i = 0; i < 9; i++)
        {
            string[] datos = lineas[i + 1].Split(',');
            equipos[i * 2] = datos[1];
            equipos[i * 2 + 1] = datos[2];
        }

        int[] victorias = new int[18];
        int[] derrotas = new int[18];
        int[] diferencia = new int[18]; // diferencia de puntos, para desempatar

        // Para cada equipo recorre todos los partidos
        for (int j = 0; j < 18; j++)
        {
            for (int i = 1; i < lineas.Length; i++)
            {
                string[] datos = lineas[i].Split(',');
                // datos[3] es el marcador con el formato: 80-75
                string[] marcador = datos[3].Split('-');
                int puntosLocal = int.Parse(marcador[0]);
                int puntosVisitante = int.Parse(marcador[1]);

                // Compreuba si el equipo juega de local o de visitante
                if (datos[1] == equipos[j])
                {
                    // comprueba si ha ganado o perdido y suma la diferencia de puntos
                    if (puntosLocal > puntosVisitante)
                    {
                        victorias[j]++;
                    }
                    else
                    {
                        derrotas[j]++;
                    }
                    diferencia[j] = diferencia[j] + puntosLocal - puntosVisitante;
                }
                // Si juega de visitante hace lo mismo pero al reves
                else if (datos[2] == equipos[j])
                {
                    if (puntosVisitante > puntosLocal)
                    {
                        victorias[j]++;
                    }
                    else
                    {
                        derrotas[j]++;
                    }
                    diferencia[j] = diferencia[j] + puntosVisitante - puntosLocal;
                }
            }
        }

        // Puntuación para ordenar: lo mas importante son las victorias y luego la diferencia de puntos
        int[] puntuacion = new int[18];
        for (int j = 0; j < 18; j++)
        {
            puntuacion[j] = victorias[j] * 1000 + diferencia[j];
        }

        // imprime el mejor equipo, 18 veces seguidas
        string texto = "CLASIFICACIÓN (desempate por diferencia de puntos)\n";
        for (int puesto = 1; puesto <= 18; puesto++)
        {
            int mejor = 0;
            for (int j = 0; j < 18; j++)
            {
                if (puntuacion[j] > puntuacion[mejor])
                {
                    mejor = j;
                }
            }
            texto = texto + puesto + ". " + equipos[mejor] + " - V: " + victorias[mejor] + " D: " + derrotas[mejor] + "\n";

            // Si un equipo ya ha salido se le pone una puntuación muy baja para que no vuelva a salir
            puntuacion[mejor] = -100000;
        }
        Console.WriteLine(texto);

        // Opcion de escribir el resultado en un fichero
        Console.Write("¿Guardar en un fichero? (s/n): ");
        if (Console.ReadLine() == "s")
        {
            Console.Write("Nombre del fichero: ");
            File.WriteAllText(Console.ReadLine() + ".txt", texto);
        }
    }

    // Crea un fichero .txt por cada equipo
    static void FicherosEquipos()
    {
        string[] equipos = new string[18];
        for (int i = 0; i < 9; i++)
        {
            string[] datos = lineas[i + 1].Split(',');
            equipos[i * 2] = datos[1];
            equipos[i * 2 + 1] = datos[2];
        }

        // Para cada equipo junta sus partidos y los guarda en su fichero
        for (int j = 0; j < 18; j++)
        {
            string texto = "";
            for (int i = 1; i < lineas.Length; i++)
            {
                string[] datos = lineas[i].Split(',');
                if (datos[1] == equipos[j] || datos[2] == equipos[j])
                {
                    texto = texto + lineas[i].Replace(",", " | ") + "\n";
                }
            }
            // Crea el fichero con el nombre del equipo
            File.WriteAllText(equipos[j] + ".txt", texto);
        }
        Console.WriteLine("Se han creado 18 ficheros");
    }
}