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
        ProgressBarWindow progressBar;

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
            progressBar.TotalSteps = imageSources.Count;
            foreach(ImageSource source in imageSources)
            {
                source.Load();
                progressBar.StepUp();
            }
            progressBar.Destroy();
        }

        private void StartProgressBar()
        {
            progressBar = new ProgressBarWindow(dataAccess);
            progressBar.Start();
        }
    }
}
