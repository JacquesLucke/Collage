using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Collage
{
    public class KeymapActivator : IOperatorActivator
    {
        DataAccess dataAccess;
        List<ICollageOperator> operators;

        public KeymapActivator(DataAccess dataAccess, List<ICollageOperator> operators)
        {
            this.dataAccess = dataAccess;

            this.operators = new List<ICollageOperator>(operators);
        }

        public List<ICollageOperator> GetActivatedOperators()
        {
            List<ICollageOperator> startableOperators = new List<ICollageOperator>();
            Keymap keymap = dataAccess.Keymap;

            // check if key combinations are pressed
            if (keymap["change background color"].IsPressed(dataAccess.Input)) startableOperators.Add(operators[2]);
            if (keymap["open images"].IsPressed(dataAccess.Input)) startableOperators.Add(operators[3]);
            if (keymap["delete images"].IsPressed(dataAccess.Input)) startableOperators.Add(operators[8]);
            if (keymap["select all"].IsPressed(dataAccess.Input)) startableOperators.Add(operators[9]);
            if (keymap["save collage"].IsPressed(dataAccess.Input)) startableOperators.Add(operators[10]);
            if (keymap["auto position"].IsPressed(dataAccess.Input)) startableOperators.Add(operators[11]);
            if (keymap["change aspect ratio"].IsPressed(dataAccess.Input)) startableOperators.Add(operators[12]);
            if (keymap["set to front"].IsPressed(dataAccess.Input)) startableOperators.Add(operators[13]);
            if (keymap["set as background"].IsPressed(dataAccess.Input)) startableOperators.Add(operators[14]);
            if (keymap["set forward"].IsPressed(dataAccess.Input)) startableOperators.Add(operators[15]);
            if (keymap["set backward"].IsPressed(dataAccess.Input)) startableOperators.Add(operators[16]);
            if (keymap["clear collage"].IsPressed(dataAccess.Input)) startableOperators.Add(operators[17]);
            if (keymap["undo"].IsPressed(dataAccess.Input)) startableOperators.Add(operators[18]);
            if (keymap["redo"].IsPressed(dataAccess.Input)) startableOperators.Add(operators[19]);

            return startableOperators;
        }
    }
}
