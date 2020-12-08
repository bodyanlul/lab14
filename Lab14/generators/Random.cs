using System;
using System.Text.Json.Serialization;
using LabaCSharp.Generators.Types;

namespace LabaCSharp.Generators
{
    class RandomGenerator : BaseGenerator
    {

        private Random rnd = new Random();

        public RandomGenerator(string name, int N) : base(name, N) { }

        [JsonIgnore]
        public override double LastNumber
        {
            get => base.LastNumber;
            set
            {
                base.LastNumber = value;
                rnd = new Random(Convert.ToInt32(value));
            }
        }

        public override GeneratorType Type => GeneratorType.RAND;

        public override double Generate()
        {
            double res = rnd.NextDouble();

            try
            {
                res = (res + base.Generate()) / (Generators.Count + 1);
            }
            catch (IndexOutOfRangeException) { }

            base.Push(res);
            return res;
        }
    }
}

