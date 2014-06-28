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
        Window window;

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
                if (!wantsToStop)
                {
                    if (!IsInitialized)
                    {
                        Gtk.Application.Init();
                        GtkWindow window = new GtkWindow();
                        ColorSelectionDialog w = new ColorSelectionDialog("hallo");
                        w.ShowAll();
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
