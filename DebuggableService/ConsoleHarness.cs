using System;
using $safeprojectname$.Types;

namespace $safeprojectname$.Framework
{
	internal static class ConsoleHarness
	{
        private static bool shutdown = false;

        // Run a service from the console given a service implementation
        internal static void Run(string[] args, IWindowsService service)
		{
            string serviceName = service.GetType().Name;
            bool isRunning = true;

            // simulate starting the windows service
            // bypass the system event call via ServiceBase and go straight to our handler
            if (service.OnStart(args, new EventLog(true)))
            {
                // let it run as long as Q is not pressed
                while (isRunning)
                {
                    WriteToConsole(ConsoleColor.Yellow, "\nEnter either [P]ause, [R]esume, [Q]uit (stop), [S]hutdown: ");
                    isRunning = HandleConsoleInput(service, WaitForKey(false));
                }

                if (shutdown)
                    service.OnShutdown();
                else
                    service.OnStop();
            }

            WaitForKey(true);
        }

		// Private input handler for console commands.
		private static bool HandleConsoleInput(IWindowsService service, ConsoleKeyInfo key)
		{
			bool canContinue = true;

            // check input
            switch (key.Key.ToString())
            {
                case "Q":
                    canContinue = false;
                    break;

                case "P":
                    service.OnPause();
                    break;

                case "R":
                    service.OnContinue();
                    break;

                case "S":
                    canContinue = false;
                    shutdown = true;
                    break;

                default:
                    WriteToConsole(ConsoleColor.Red, "\nDid not understand that input, try again.");
                    break;
            }

            return canContinue;
		}

		// Helper method to write a message to the console at the given foreground color.
		internal static void WriteToConsole(ConsoleColor foregroundColor, string format, params object[] formatArguments)
		{
			ConsoleColor originalColor = Console.ForegroundColor;
			Console.ForegroundColor = foregroundColor;

			Console.WriteLine(format, formatArguments);
			Console.Out.Flush();

			Console.ForegroundColor = originalColor;
		}

        internal static ConsoleKeyInfo WaitForKey(bool promptExit)
        {
            if (promptExit) WriteToConsole(ConsoleColor.Gray, "\n\nPress any key to exit\n");
            return Console.ReadKey(true);
        }
    }
}
