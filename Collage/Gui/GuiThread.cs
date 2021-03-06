﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Gtk;

namespace Collage
{
    public delegate void Invoke();
    public class GuiThread
    {
        Thread thread;
        public bool IsInitialized { get; private set; }
        bool wantsToStop = false;
        bool waitsToInvoke = false;

        List<Invoke> invokeMethods = new List<Invoke>();
        bool isBlockedByDialog = false;

        public GuiThread()
        {
            IsInitialized = false;
        }

        public void Start()
        {
            thread = new Thread(new ThreadStart(RunLoop));
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }
        public void Stop()
        {
            wantsToStop = true;
        }
        public void Invoke(Invoke method)
        {
            // add the method to the list -> the gtk thread call every method inside
            isBlockedByDialog = true;
            waitsToInvoke = true;
            invokeMethods.Add(method);
        }

        public bool IsBlockedByDialog { get { return isBlockedByDialog; } }
        public bool WaitsToInvoke { get { return waitsToInvoke; } }

        private void RunLoop()
        {
            while (true)
            {
                // call the new methods and clear the list
                for (int i = 0; i < invokeMethods.Count; i++ )
                {
                    isBlockedByDialog = true;
                    invokeMethods[i]();
                    isBlockedByDialog = false;
                }
                invokeMethods.Clear();
                waitsToInvoke = false;

                if (!wantsToStop)
                {
                    if (!IsInitialized)
                    {
                        // initialize Gtk
                        Gtk.Application.Init();
                        IsInitialized = true;
                    }

                    Gtk.Application.RunIteration(false);
                }
                else
                {
                    // stop the Gtk thread
                    if (IsInitialized)
                    {
                        Gtk.Application.Quit();
                        IsInitialized = false;
                    }
                    break;
                }
                System.Threading.Thread.Sleep(20);
            }
        }
    }
}
