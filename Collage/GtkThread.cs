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
        Thread thread;
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
            // add the method to the list -> the gtk thread call every method inside
            invokeMethods.Add(method);
        }

        public void RunLoop()
        {
            while (true)
            {
                // call the new methods and clear the list
                foreach(Invoke method in invokeMethods)
                    method();
                invokeMethods.Clear();

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
