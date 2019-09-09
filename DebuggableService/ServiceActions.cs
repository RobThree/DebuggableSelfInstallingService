using System;
using System.Collections.Generic;
using System.Linq;
using $safeprojectname$.ProcessingTasks;
using $safeprojectname$.Types;

namespace $safeprojectname$.ServiceActions
{
    static internal class Action
    {
        static internal EventLog EventLog { private get; set; }

        internal static bool Start(string[] args)
        {
            EventLog.WriteEntry("Service Starting", System.Diagnostics.EventLogEntryType.Information);
            try
            {
                Task.Initialise(EventLog);
                return true;
            }
            catch
            {
                EventLog.WriteEntry("Service failed to start", System.Diagnostics.EventLogEntryType.Error);
                return false;
            }
        }

        internal static void Stop(bool shutdown = false)
        {
            if (shutdown)
                EventLog.WriteEntry("Service Stopping due to system shutdown", System.Diagnostics.EventLogEntryType.Information);
            else
                EventLog.WriteEntry("Service Stopping normally", System.Diagnostics.EventLogEntryType.Information);

            Task.Shutdown(shutdown);
        }

        internal static void Pause()
        {
            Task.Pause();
        }

        internal static void Continue()
        {
            Task.Resume();
        }

        internal static void Shutdown()
        {
            // firstly Stop the service, indicating urgency
            Stop(true);

            // then perform any clean-up required

        }

        internal static void CustomCommand(int command)
        {
            Task.CustomCommand(command);
        }
    }
}
