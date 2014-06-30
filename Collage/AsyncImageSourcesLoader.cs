using Gtk;
using System.Collections.Generic;
using System.Threading;

namespace Collage
{
    /// <summary>
    /// this should be used when you need to load more than one image
    /// </summary>
    public class AsyncImageSourcesLoader
    {
        List<ImageSource> imageSources = new List<ImageSource>();
        DataAccess dataAccess;
        Window window;
        ProgressBar progressBar;

        public AsyncImageSourcesLoader(DataAccess dataAccess, List<ImageSource> sourcesToLoad)
        {
            this.dataAccess = dataAccess;
            imageSources = sourcesToLoad;
        }

        public void LoadAll()
        {
            Thread thread = new Thread(LoadingThread);
            thread.Start();
            dataAccess.GtkThread.Invoke(StartProgressBar);
        }

        private void LoadingThread()
        {
            while (progressBar == null) ;
            int counter = 0;
            foreach(ImageSource source in imageSources)
            {
                counter++;
                source.Load();
                progressBar.Fraction = (double)counter / (double)imageSources.Count;
                progressBar.Text = "Load images: " + counter + " of " + imageSources.Count;
            }
            window.Destroy();
        }

        private void StartProgressBar()
        {
            window = new Window(WindowType.Toplevel);
            window.Move(10, 10);
            window.KeepAbove = true;
            window.Resize(400, 30);
            progressBar = new ProgressBar();
            progressBar.Fraction = 0;
            progressBar.BarStyle = ProgressBarStyle.Continuous;
            window.Add(progressBar);

            window.ShowAll();
        }
    }
}
