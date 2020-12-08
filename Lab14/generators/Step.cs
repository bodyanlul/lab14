using System;
using LabaCSharp.Generators.Types;

namespace LabaCSharp.Generators
{
    class GeneratorWithStep : BaseGenerator
    {

        public double Step { get; set; }

        public override GeneratorType Type => GeneratorType.STEP;
        public GeneratorWithStep(string name, int N, double first, double step) : base(name, N)
        {
            this.Step = step;
            this.Push(first);
        }

        public override double Generate()
        {
            double res = this.LastNumber + this.Step;
            try
            {
                res = (res + base.Generate()) / (Generators.Count + 1);
            }
            catch (IndexOutOfRangeException) { }

            this.Push(res);
            return res;
        }
    }
}

