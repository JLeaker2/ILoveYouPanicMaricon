using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

class Program
{
    // Importar funciones de la API de Windows para detectar pulsaciones de teclas
    [DllImport("User32.dll")]
    private static extern short GetAsyncKeyState(int vKey);

    // Método para cerrar un proceso por su nombre
    static void CloseProcess(string processName)
    {
        Console.WriteLine($"Buscando el proceso '{processName}'...");
        Process[] processes = Process.GetProcessesByName(processName);

        if (processes.Length > 0)
        {
            foreach (var process in processes)
            {
                try
                {
                    process.Kill(); // Finaliza el proceso
                    Console.WriteLine($"El proceso '{processName}' ha sido finalizado con éxito.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al intentar finalizar el proceso: {ex.Message}");
                }
            }
        }
        else
        {
            Console.WriteLine($"El proceso '{processName}' no se encontró en ejecución.");
        }
    }

    // Método principal
    static void Main(string[] args)
    {
        Console.WriteLine(@"
  ____  _____ _____ ___  _   _    _  _____ ___  ____  
 |  _ \| ____|_   _/ _ \| \ | |  / \|_   _/ _ \|  _ \ 
 | | | |  _|   | || | | |  \| | / _ \ | || | | | |_) |
 | |_| | |___  | || |_| | |\  |/ ___ \| || |_| |  _ < 
 |____/|_____| |_| \___/|_| \_/_/   \_\_| \___/|_| \_\ 
       By Alafant & ILoveYouPanic
");

        Console.WriteLine("Bienvenido a Detonator, este programa te servirá para crashear el juego al instante.");
        Console.WriteLine("Por defecto, se cerrará el proceso 'GTA5.exe'.\n");

        Console.Write("Introduce el nombre del proceso a finalizar (presiona Enter para usar 'GTA5'): ");
        string processName = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(processName))
        {
            processName = "GTA5"; // Valor predeterminado
        }

        Console.Write("Introduce una tecla para cerrar el proceso (por defecto 'F5'): ");
        string keyInput = Console.ReadLine();
        ConsoleKey key = ConsoleKey.F5;
        if (!string.IsNullOrWhiteSpace(keyInput))
        {
            if (Enum.TryParse(keyInput, true, out ConsoleKey parsedKey))
            {
                key = parsedKey;
            }
            else
            {
                Console.WriteLine("Tecla no válida, usando 'F5' por defecto.");
            }
        }

        Console.WriteLine($"\nKeybind configurada: Presiona '{key}' para cerrar el proceso '{processName}'.");
        Console.WriteLine("Presiona 'Ctrl+C' para salir del programa en cualquier momento.\n");

        // Hilo para detectar pulsaciones de teclas
        new Thread(() =>
        {
            while (true)
            {
                Thread.Sleep(100);

                // Detectar la pulsación de la tecla configurada
                if ((GetAsyncKeyState((int)key) & 0x8000) != 0)
                {
                    CloseProcess(processName);
                }
            }
        }).Start();

        // Mantener el programa en ejecución hasta que el usuario lo detenga
        Console.CancelKeyPress += (sender, eventArgs) =>
        {
            Console.WriteLine("\nCtrl+C detectado. Cerrando el programa.");
            Environment.Exit(0);
        };

        while (true)
        {
            Thread.Sleep(1000); // Mantener la aplicación activa
        }
    }
}
