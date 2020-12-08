using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using LabaCSharp.Generators.Types;

namespace LabaCSharp.Generators
{
    class BaseGenerator : IComparable<BaseGenerator>, IComparer<BaseGenerator>, ICloneable
    {
        public virtual GeneratorType Type => GeneratorType.BASE;

        public string Name { get; set; }
        public double[] Sequence
        {
            get
            {
                double[] tmp;

                if (this.full)
                {
                    tmp = new double[this.N];
                    int ndx = 0;

                    for (int i = 0; i < this.N; i++)
                    {
                        tmp[ndx] = this._sequence[(i + this.counter) % this.N];
                        ndx++;
                    }
                }
                else
                {
                    tmp = new double[this.counter];
                    for (int i = 0; i < this.counter; i++)
                    {
                        tmp[i] = this._sequence[i];
                    }
                }
                return tmp;
            }
        }
        [JsonPropertyName("count")]
        public int N { get; }

        private double[] _sequence;
        private int counter = 0;
        private bool full = false;
        public List<object> Generators => _generators;
        protected List<object> _generators = new List<object>();

        public BaseGenerator(string name, int N)
        {
            if (name.Length == 0 || N <= 0)
            {
                throw new ArgumentException("invalid arguments for generator");
            }

            this.Name = name;
            this.N = N;
            this._sequence = new double[N];
        }

        public void Add(BaseGenerator generator)
        {
            _generators.Add(generator);
        }

        public virtual double Generate()
        {
            if (_generators.Count == 0)
            {
                throw new IndexOutOfRangeException("not enough generators");
            }

            double sum = 0;

            foreach (BaseGenerator generator in _generators)
            {
                sum += generator.Generate();
            }


            double res = sum / _generators.Count;
            this.Push(res);

            return res;
        }

        [JsonIgnore]
        public virtual double LastNumber
        {
            get
            {
                if (_sequence.Length == 0)
                {
                    throw new IndexOutOfRangeException("sequence is empty");
                }

                int ndx = counter - 1;
                return _sequence[ndx == -1 ? N - 1 : ndx];
            }
            set
            {
                _sequence[counter != 0 ? counter - 1 : N - 1] = value;
            }
        }

        public double Average()
        {
            double sum = 0;

            if (!full)
            {
                throw new IndexOutOfRangeException("invalid variable value");
            }

            foreach (var number in Sequence)
            {
                sum += number;
            }

            return sum / N;
        }

        protected void Push(double number)
        {
            _sequence[counter] = number;
            counter++;

            if (counter == N)
            {
                counter = 0;
                if (!full)
                {
                    full = true;
                }
            }
        }

        public void Save(Stream stream)
        {
            if (!stream.CanWrite)
            {
                throw new ArgumentException("invalid stream");
            }

            var serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            Utf8JsonWriter writer = new Utf8JsonWriter(stream);
            JsonSerializer.Serialize<object>(writer, this, serializerOptions);
        }

        public static BaseGenerator Load(JsonElement root)
        {
            BaseGenerator generator;

            JsonElement typeGenerator = root.GetProperty("type");

            string name = root.GetProperty("name").GetString();
            JsonElement sequenceElement = root.GetProperty("sequence");
            int n = root.GetProperty("count").GetInt32();
            double[] sequence = new double[n];
            int ndx = 0;

            foreach (var number in sequenceElement.EnumerateArray())
            {
                sequence[ndx] = number.GetDouble();
                ndx++;
            }


            switch ((GeneratorType)typeGenerator.GetInt32())
            {
                case GeneratorType.BASE:
                    generator = new BaseGenerator(name, n);
                    break;
                case GeneratorType.RAND:
                    generator = new RandomGenerator(name, n);
                    break;
                case GeneratorType.STEP:
                    double step = root.GetProperty("step").GetDouble();
                    generator = new GeneratorWithStep(name, n, 0, step);
                    break;
                default:
                    throw new ArgumentException("invalid json");
            }

            for (int i = 0; i < ndx; i++)
            {
                generator.Push(sequence[i]);
            }

            JsonElement generatorsElement = root.GetProperty("generators");
            foreach (var generatorElement in generatorsElement.EnumerateArray())
            {
                BaseGenerator children = BaseGenerator.Load(generatorElement);
                generator.Add(children);
            }

            return generator;
        }

        public static BaseGenerator Load(Stream stream)
        {
            if (!stream.CanRead)
            {
                throw new ArgumentException("invalid stream");
            }

            UTF8Encoding utf8 = new UTF8Encoding(true);
            byte[] array = new byte[stream.Length];
            stream.Read(array, 0, array.Length);
            string json = utf8.GetString(array);
            BaseGenerator generator;

            using (JsonDocument document = JsonDocument.Parse(json))
            {
                JsonElement root = document.RootElement;
                generator = BaseGenerator.Load(root);
            }

            return generator;
        }

        public int CompareTo(BaseGenerator generator)
        {
            if (generator is null) return 1;

            var nComparison = this.N.CompareTo(generator.N);
            if (nComparison != 0) return nComparison;
            return string.Compare(this.Name, generator.Name, StringComparison.Ordinal);
        }

        public int Compare(BaseGenerator x, BaseGenerator y)
        {
            if (ReferenceEquals(x, y)) return 0;
            if (x is null) return -1;
            return x.CompareTo(y);
        }

        public object Clone()
        {
            var clone =  new BaseGenerator(this.Name, this.N)
            {
                _sequence = this._sequence,
                _generators = new List<object>(),
                full = this.full,
                counter = this.counter
            };

            foreach (var _gen in this._generators)
            {
                BaseGenerator gen = _gen as BaseGenerator;
                clone._generators.Add(gen.Clone());
            }

            return clone;
        }
    }
}

