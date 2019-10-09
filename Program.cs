using System;
using  System.Threading;
using System.Collections.Concurrent;

namespace hw2
{
    class Program
    {
        static int SIZE = 10;

        static BlockingCollection<double> buffer1= new BlockingCollection<double>();
        static BlockingCollection<double> buffer2= new BlockingCollection<double>();
        static BlockingCollection<double> buffer3= new BlockingCollection<double>();
        static void Main(string[] args)
        {
            
            DateTime t = DateTime.Now;
            long saveticks = t.Ticks;
            new Thread(fact).Start();
            new Thread(power).Start();
            new Thread(sum).Start();
            new Thread(addUp).Start();
            t = DateTime.Now;
            Console.WriteLine((t.Ticks - saveticks) / 10000000.0);
            
        }
        static void addUp(){
            double counter = 0;
            for(int i=1; i < SIZE; i++){
                counter += buffer3.Take();
            }
            Console.WriteLine(counter);
        }
        static void sum(){
            double sum = 0;
            buffer3.Add(sum);
            for(int i=1; i <SIZE; i++){
                double x = buffer1.Take();
                double y = buffer2.Take();
                buffer3.Add(x*y);
            }
        }
        static void power(){
            double power = 0;
            buffer2.Add(power);
            for(int i=1; i<SIZE; i++){
                power += i * i;
                buffer2.Add(power);
            }
        }
        static void fact(){
            double fact1 = .9999999;
            buffer1.Add(0);
            for(int i=1; i<SIZE;i++){
                fact1 *= 0.9999999;
                buffer1.Add(fact1);
            }
        }
    }

}
