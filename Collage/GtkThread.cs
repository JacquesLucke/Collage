using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Gtk;

namespace Collage
{
    public delegate void Invoke();
    public class GtkThread
    {
        private Thread thread;

        public bool IsInitialized { get; private set; }
        bool wantsToStop = false;

        List<Invoke> invokeMethods = new List<Invoke>();

        public GtkThread()
        {
            IsInitialized = false;
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

        public void Invoke(Invoke method)
        {
            invokeMethods.Add(method);
        }

        public void RunLoop()
        {
            while (true)
            {
                foreach(Invoke method in invokeMethods)
                    method();
                invokeMethods.Clear();
                if (!wantsToStop)
                {
                    if (!IsInitialized)
                    {
                        Gtk.Application.Init();
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
