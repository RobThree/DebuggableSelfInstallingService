using $safeprojectname$.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace $safeprojectname$.Processing
{
    internal static class WorkerProcess
    {
        private static bool _shutdown;

        private static EventLog _eventLog;

        private static BackgroundWorker backgroundWorker;

        internal static void Initialise(EventLog eventLog)
        {
            if (eventLog != null) _eventLog = eventLog;

            // initialisation code..

            // start working
            DoWork();
        }

        private static void DoWork()
        {
            try
            {
                backgroundWorker = new BackgroundWorker();
                backgroundWorker.WorkerReportsProgress = true;
                backgroundWorker.WorkerSupportsCancellation = true;

                // hook-up the event handlers
                backgroundWorker.DoWork += new DoWorkEventHandler(backgroundWorker_DoWork);
                backgroundWorker.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker_ProgressChanged);
                backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_RunWorkerCompleted);

                if (backgroundWorker.IsBusy != true)
                {
                    // Start the asynchronous operation.
                    backgroundWorker.RunWorkerAsync();
                    return;
                }
                throw new InvalidOperationException("Worker process already running");
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to create worker process: " + ex.Message);
            }
        }

        // This method is called in order to indicate that the work should be canclled/stopped
        internal static void CancelWork(bool shutdown)
        {
            _shutdown = shutdown;

            if (backgroundWorker != null
                &&
                backgroundWorker.WorkerSupportsCancellation == true
               )
            {
                // Cancel the asynchronous operation.
                backgroundWorker.CancelAsync();
            }
        }

        // This event handler deals with the progress of the background operation.
        internal static void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
        }

        // This event handler deals with the results of the background operation.
        internal static void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // un-hook the event handlers and dispose
            backgroundWorker.DoWork -= new DoWorkEventHandler(backgroundWorker_DoWork);
            backgroundWorker.ProgressChanged -= new ProgressChangedEventHandler(backgroundWorker_ProgressChanged);
            backgroundWorker.RunWorkerCompleted -= new RunWorkerCompletedEventHandler(backgroundWorker_RunWorkerCompleted);
            backgroundWorker = null;

            if (e.Cancelled == true)
            {
                // cancelled by user/system action
            }
            else if (e.Error != null)
            {
                // an error occurred
            }
            else
            {
                // normal finish
                // it is acceptable to call DoWork() from here to re-start the background process, if required...
            }
        }

        // This event handler is where the time-consuming work is done.
        internal static void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            // during processing, regular checks should be made to see if a Cancel is pending.
            // If so, and shutdown=true, stop immediately.
            // If so, and shutdown=false, stop when convenient to do so.
            // To stop, set e.Cancel = true, then return
        }
    }
}