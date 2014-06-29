using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Gtk;

namespace Collage
{
    public class GtkThread
    {
        private Thread thread;

        public bool IsInitialized { get; private set; }
        bool wantsToStop = false;
        public GtkWindow w;
        public string newOpen = "";

        public GtkThread()
        {
            IsInitialized = false;
            //this.window = window;
        }

        public void Start()
        {
            thread = new Thread(new ThreadStart(RunLoop));
            thread.Start();
        }

        public void Stop()
        {
            wantsToStop = true;
        }

        public void RunLoop()
        {
            while (true)
            {
                if (newOpen != "") { newOpen = ""; w.Open(); }
                if (!wantsToStop)
                {
                    if (!IsInitialized)
                    {
                        Gtk.Application.Init();
                        w = new GtkWindow();
                        w.Show();
                        IsInitialized = true;
                    }

                    Gtk.Application.RunIteration(false);
                }
                else
                {
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
