using $safeprojectname$.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace $safeprojectname$.ProcessingTasks
{
    static internal class Task
    {
        static internal void Initialise(EventLog eventLog)
        {
            StartProcessing(eventLog);
        }

        static internal void Shutdown(bool shutdown)
        {
            Processing.WorkerProcess.CancelWork(shutdown);
        }

        internal static void Pause()
        {
            Processing.WorkerProcess.CancelWork(false);
        }

        internal static void Resume()
        {
            StartProcessing(null);
        }

        internal static void CustomCommand(int command)
        {
            throw new NotImplementedException();
        }

        private static void StartProcessing(EventLog eventLog)
        {
            try
            {
                Processing.WorkerProcess.Initialise(eventLog); // start it working
            }
            catch (Exception ex)
            {
                eventLog.WriteEntry(ex.Message + " : " + ex.InnerException.Message, System.Diagnostics.EventLogEntryType.Error);
                throw;
            }
        }
    }
}
