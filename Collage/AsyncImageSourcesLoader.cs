using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public AsyncImageSourcesLoader(DataAccess dataAccess, List<ImageSource> sourcesToLoad)
        {
            this.dataAccess = dataAccess;
            imageSources = sourcesToLoad;
        }

        public void LoadAll()
        {
            Thread thread = new Thread(LoadingThread);
            thread.Start();
        }

        private void LoadingThread()
        {
            foreach(ImageSource source in imageSources)
            {
                source.Load();
            }
        }
    }
}
