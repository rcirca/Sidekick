﻿using Sidekick.Helpers;
using Sidekick.Helpers.POEPriceInfoAPI;
using Sidekick.Helpers.POETradeAPI;
using Sidekick.Windows.Overlay;
using System;
using System.Threading;
using System.Windows.Forms;

namespace Sidekick
{
    class Program
    {
        static readonly string APPLICATION_PROCESS_GUID = "93c46709-7db2-4334-8aa3-28d473e66041";

        [STAThread]
        static void Main()
        {
            // Only have one instance running.
            var mutex = new Mutex(true, APPLICATION_PROCESS_GUID, out bool instanceResult);
            if (!instanceResult) return;
            GC.KeepAlive(mutex);

            Logger.Log("Starting Sidekick.");

            // System tray icon.
            TrayIcon.Initialize();

            // Load POE Trade information.
            _ = TradeClient.Initialize();

            // Keyboard hooks.
            EventsHandler.Initialize();

            // Overlay.
            OverlayController.Initialize();

            // Run window.
            Application.ThreadExit += ThreadExit;
            Application.Run();
        }

        private static void ThreadExit(object sender, EventArgs e)
        {
            // check that the main thread is about to exit
            if (SynchronizationContext.Current != null)
            {
                TrayIcon.Dispose();
                TradeClient.Dispose();
                EventsHandler.Dispose();
                OverlayController.Dispose();
            }
        }
    }
}