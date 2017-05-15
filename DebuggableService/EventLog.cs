using $safeprojectname$.Framework;
using System;
using System.Diagnostics;

namespace $safeprojectname$.Types
{
    // local copy of system class which allows us to have our own version of a logging method instead of the inbuilt one
    public class EventLog : System.Diagnostics.EventLog
    {
        bool useConsole = false;

        internal EventLog(bool local)
        {
            useConsole = local;
        }

        internal new void WriteEntry(string message, EventLogEntryType type)
        {
            if (useConsole)
            {
                ConsoleColor color = ConsoleColor.Green;
                switch (type)
                {
                    case EventLogEntryType.Error:
                        color = ConsoleColor.Red; break;
                    case EventLogEntryType.Information:
                        color = ConsoleColor.Blue; break;
                    case EventLogEntryType.Warning:
                        color = ConsoleColor.White; break;
                }

                ConsoleHarness.WriteToConsole(color, "\n" + message);
            }
            else
                base.WriteEntry(message, type);
        }
    }

    // local copy of system class which allows us to expose our own property instead of the inbuilt one
    public class ServiceBase : System.ServiceProcess.ServiceBase
    {
        public new virtual EventLog EventLog
        {
            get;
        }
    }
}
