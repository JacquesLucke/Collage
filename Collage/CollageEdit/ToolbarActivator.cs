using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Collage
{
    public class ToolbarActivator : IParallelOperatorActivator
    {
        DataAccess dataAccess;
        ToolbarWindow window;
        List<ICollageOperator> operators;

        public ToolbarActivator(DataAccess dataAccess, List<ICollageOperator> operators)
        {
            this.dataAccess = dataAccess;
            this.operators = operators;

            dataAccess.GuiThread.Invoke(OpenWindow);
        }

        private void OpenWindow()
        {
            window = new ToolbarWindow();
            window.Start();
        }

        public void SetSensitivity(bool sensitivity)
        {
            window.SetSensitivity(sensitivity);
        }

        public List<ICollageOperator> GetActivatedOperators()
        {
            List<ICollageOperator> startableOperators = new List<ICollageOperator>();

            if (window != null)
            {
                List<object> interactions = window.GetInteractions();
                foreach(object ob in interactions)
                {
                    if(ob is string)
                    {
                        string action = (string)ob;
                        if (action == "open images") startableOperators.Add(operators[3]);
                    }
                }
                interactions.Clear();
            }

            return startableOperators;
        }
    }
}
