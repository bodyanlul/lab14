using System;
using System.Collections;
using  System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using LabaCSharp.Generators;

namespace Lab14
{
    class Program
    {
        static void FirstTask()
        {
            Console.WriteLine(" --- Первое задание --- ");
            ArrayList list = new ArrayList();
            Random rnd = new Random();

            for (int i = 0; i < 5; i++)
            {
                list.Add(rnd.Next());
            }

            int ndx;
            
            list.Add("test string");
            Console.WriteLine($"Количество - {list.Count}");

            while (true)
            {
                try
                {
                    Console.WriteLine("Какой элемент по счету нужно удалить?");
                    ndx = Convert.ToInt32(Console.ReadLine());

                    if (ndx <= list.Count && ndx >= 1)
                    {
                        ndx -= 1;
                        break;
                    }
                    
                    Console.WriteLine($"Введенное число находится за границами. Границы : [1; {list.Count}]");
                }
                catch (Exception)
                {
                    Console.WriteLine("Введите корректно целое число");
                }
            }
            
            list.RemoveAt(ndx);
            
            Console.WriteLine($"Количество - {list.Count}");
            foreach (var element in list)
            {
                Console.WriteLine($" - {element}");
            }
            
            Console.WriteLine("Введите что нужно искать:");
            var searchLine = Console.ReadLine();
            object toSearch;

            if (int.TryParse(searchLine, out var numberToFind))
            {
                toSearch = numberToFind;
            }
            else
            {
                toSearch = searchLine;
            }

            int ndxFound = list.IndexOf(toSearch);

            Console.WriteLine(ndxFound == -1
                ? "Элемент не найден!"
                : $"Найденный элемент находится на {ndxFound + 1} позиции");

        }

        static void SecondTask()
        {
            Console.WriteLine(" --- Второе задание ---");
            Stack<double> stack = new Stack<double>();
            Random rnd = new Random();
            
            for (int i = 0; i < 10; i++)
            {
                stack.Push(rnd.NextDouble());
            }
            
            Console.WriteLine($"Количество - {stack.Count}");
            foreach (var element in stack)
            {
                Console.WriteLine($" * {element}");
            }

            int n;
            
            while (true)
            {
                Console.WriteLine("Сколько элементов необходимо удалить?");
                string stringIn = Console.ReadLine();

                if (int.TryParse(stringIn, out var nn))
                {
                    if (nn > stack.Count || nn < 0)
                    {
                        Console.WriteLine("В коллекции нет столько элементов!");
                        continue;
                    }

                    n = nn;
                    break;
                }
                else
                {
                    Console.WriteLine("Некорректно введено число!");
                }
            }

            for (int i = 0; i < n; i++)
            {
                stack.Pop();
            }

            for (int i = 0; i < 5; i++)
            {
                stack.Push(rnd.NextDouble());
            }

            var linkedList = new LinkedList<double>();
            int counter = 0;
            foreach (var element in stack)
            {
                _ = counter % 2 == 0 ? linkedList.AddLast(element) : linkedList.AddAfter(linkedList.Last, element);
                counter++;
            }
            
            Console.WriteLine($"Количество - {linkedList.Count}");
            foreach (var element in linkedList)
            {
                Console.WriteLine($" * {element}");
            }
            
            double toSearch;

            while (true)
            {
                Console.WriteLine("Введите что нужно искать:");
                string stringIn = Console.ReadLine();

                if (double.TryParse(stringIn, out toSearch))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Некорректно введено число!");
                }
            }

            var found = linkedList.Find(toSearch);

            Console.WriteLine(found == null
                ? "Элемент не найден!"
                : "Элемент есть в этом списке");
        }

        static void ThirdTask()
        {
            Console.WriteLine(" --- Третье задание --- ");
            Stack<BaseGenerator> stack = new Stack<BaseGenerator>();
            
            for (int i = 0; i < 10; i++)
            {
                stack.Push(new BaseGenerator(i.ToString(), i + 1));
            }

            Console.WriteLine($"Количество - {stack.Count}");
            foreach (var element in stack)
            {
                Console.WriteLine($" * {element.Name} - {element.N}");
            }

            int n;
            
            while (true)
            {
                Console.WriteLine("Сколько элементов необходимо удалить?");
                string stringIn = Console.ReadLine();

                if (int.TryParse(stringIn, out var nn))
                {
                    if (nn > stack.Count || nn < 0)
                    {
                        Console.WriteLine("В коллекции нет столько элементов!");
                        continue;
                    }

                    n = nn;
                    break;
                }
                else
                {
                    Console.WriteLine("Некорректно введено число!");
                }
            }

            for (int i = 0; i < n; i++)
            {
                stack.Pop();
            }

            for (int i = 0; i < 5; i++)
            {
                stack.Push(new BaseGenerator(i.ToString(), i + 1));
            }

            var linkedList = new LinkedList<BaseGenerator>();
            int counter = 0;
            foreach (var element in stack)
            {
                _ = counter % 2 == 0 ? linkedList.AddLast(element) : linkedList.AddAfter(linkedList.Last, element);
                counter++;
            }

            Console.WriteLine($"Количество - {linkedList.Count}");
            foreach (var element in linkedList)
            {
                Console.WriteLine($" * {element.Name} - {element.N}");
            }
            
            BaseGenerator gen1 = new BaseGenerator("1", 5), gen2 = new BaseGenerator("2", 4);
            int res = gen1.CompareTo(gen2);

            if (res < 0)
            {
                Console.WriteLine("gen1 меньше gen2");
            }
            else if (res > 0)
            {
                Console.WriteLine("gen1 больше gen2");
            }
            else
            {
                Console.WriteLine("gen1 равен gen2");
            }

            var gen1Clone = gen1.Clone() as BaseGenerator;
            res = gen1.CompareTo(gen1Clone);
            if (res < 0)
            {
                Console.WriteLine("gen1 меньше gen1Clone");
            }
            else if (res > 0)
            {
                Console.WriteLine("gen1 больше gen1Clone");
            }
            else
            {
                Console.WriteLine("gen1 равен gen1Clone");
            }
        }
        
        static void LastTask()
        {
            Console.WriteLine(" --- Четвертое задание --- ");

            ObservableCollection<BaseGenerator> generators = new ObservableCollection<BaseGenerator>();
            generators.CollectionChanged += OnCollectionChanged;

            for (int i = 0; i < 10; i++)
            {
                generators.Add(new BaseGenerator(i.ToString(), i + 1));
            }

            for (int i = 0; i < 10; i++)
            {
                generators.RemoveAt(0);
            }
        }

        static void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Console.WriteLine("Были произведены изменения в коллекции!");
            if (e.NewItems != null)
            {
                Console.WriteLine("Изменение в списке:");

                foreach (BaseGenerator newItem in e.NewItems)
                {
                    Console.WriteLine($" + \"{newItem.Name}\" - {newItem.N}");
                }
            }
            else if (e.OldItems != null && e.Action == NotifyCollectionChangedAction.Remove)
            {
                Console.WriteLine("Были удалены следующие элементы:");

                foreach (BaseGenerator oldItem in e.OldItems)
                {
                    Console.WriteLine($" - \"{oldItem.Name}\" - {oldItem.N}");
                }
            }
            else if (e.OldItems != null)
            {
                Console.WriteLine("Были изменены следующие элементы:");

                foreach (BaseGenerator oldItem in e.OldItems)
                {
                    Console.WriteLine($" * \"{oldItem.Name}\" - {oldItem.N}");
                }
            }
        }

        static void Main(string[] args)
        {
            FirstTask();
            SecondTask();
            ThirdTask();
            LastTask();
        }
    }
}