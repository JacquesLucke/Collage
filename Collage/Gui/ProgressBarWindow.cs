using Gtk;
using System.Threading;

namespace Collage
{
    public class ProgressBarWindow
    {
        Window window;
        ProgressBar progressBar;
        int totalSteps;
        int currentStep;
        string name = "";

        public ProgressBarWindow() { }

        public void Start()
        {
            currentStep = 0;

            window = new Window(WindowType.Toplevel);
            window.Move(10, 10);
            window.KeepAbove = true;
            window.Resize(400, 30);
            window.AcceptFocus = false;

            progressBar = new ProgressBar();
            progressBar.Fraction = 0;

            window.Add(progressBar);
            window.ShowAll();
        }
        public void Start(int totalSteps)
        {
            Start();
            this.totalSteps = totalSteps;
        }

        public void Destroy()
        {
            if(progressBar == null)
            {
                for(int i = 0; i < 30; i++)
                {
                    if (progressBar != null) break;
                    Thread.Sleep(1);
                }
            }

            if(progressBar != null) progressBar.Destroy();
            if(progressBar != null) window.Destroy();
        }

        public void StepUp(string text)
        {
            StepUp();
            progressBar.Text = text;
        }
        public void StepUp()
        {
            currentStep++;
            if (progressBar != null)
            {
                // change the progress fraction
                progressBar.Fraction = (double)currentStep / (double)totalSteps;
                // update the text
                if (name != "") progressBar.Text = name + " : " + currentStep + " of " + totalSteps;
                else progressBar.Text = currentStep + " of " + totalSteps;
            }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public float Fraction
        {
            get { return (float)progressBar.Fraction; }
            set { progressBar.Fraction = (float)value; }
        }
        public int TotalSteps
        {
            get { return totalSteps; }
            set { totalSteps = value; }
        }
        public int CurrentStep
        {
            get { return currentStep; }
            set { currentStep = value; }
        }
    }
}
