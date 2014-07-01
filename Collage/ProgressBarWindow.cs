using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gtk;

namespace Collage
{
    public class ProgressBarWindow
    {
        DataAccess dataAccess;
        Window window;
        ProgressBar progressBar;
        int totalSteps;
        int currentStep;

        public ProgressBarWindow(DataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
        }

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
            progressBar.Destroy();
            window.Destroy();
        }

        public void StepUp(string text)
        {
            StepUp();
            progressBar.Text = text;
        }
        public void StepUp()
        {
            currentStep++;
            progressBar.Fraction = (double)currentStep / (double)totalSteps;
            progressBar.Text = currentStep + " of " + totalSteps;
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
