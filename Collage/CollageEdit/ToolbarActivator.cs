﻿using System.Collections.Generic;

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
            window = new ToolbarWindow(dataAccess.Keymap);
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
                        if (action == "auto position") startableOperators.Add(operators[11]);
                        if (action == "save collage") startableOperators.Add(operators[10]);
                        if (action == "delete images") startableOperators.Add(operators[8]);
                        if (action == "change aspect ratio") startableOperators.Add(operators[12]);
                        if (action == "select all") startableOperators.Add(operators[9]);
                        if (action == "set backward") startableOperators.Add(operators[16]);
                        if (action == "set forward") startableOperators.Add(operators[15]);
                        if (action == "set as background") startableOperators.Add(operators[14]);
                        if (action == "set to front") startableOperators.Add(operators[13]);
                        if (action == "clear collage") startableOperators.Add(operators[17]);
                        if (action == "change background color") startableOperators.Add(operators[2]);
                        if (action == "undo") startableOperators.Add(operators[18]);
                        if (action == "redo") startableOperators.Add(operators[19]);
                    }
                }
                interactions.Clear();
            }

            return startableOperators;
        }
    }
}
