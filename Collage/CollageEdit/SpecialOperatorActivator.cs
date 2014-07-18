using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Collage
{
    public class SpecialOperatorActivator : IOperatorActivator
    {
        DataAccess dataAccess;
        List<ISpecialOperatorStart> operators;

        public SpecialOperatorActivator(DataAccess dataAccess, List<ICollageOperator> operators)
        {
            this.dataAccess = dataAccess;

            this.operators = new List<ISpecialOperatorStart>();
            foreach(ICollageOperator op in operators)
            {
                if (op is  ISpecialOperatorStart) this.operators.Add((ISpecialOperatorStart)op);
            }
        }

        public List<ICollageOperator> GetActivatedOperators()
        {
            List<ICollageOperator> startableOperators = new List<ICollageOperator>();
            foreach (ISpecialOperatorStart op in operators)
                if (op.CanStart()) startableOperators.Add(op);

            return startableOperators;
        }
    }
}
