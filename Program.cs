using System;
using  System.Threading;
using System.Collections.Concurrent;

namespace hw2
{
    class Program
    {
        static int SIZE = 16;
        static DateTime t = DateTime.Now;
        static long saveticks = 0;
        static BlockingCollection<double> buffer1= new BlockingCollection<double>();
        static BlockingCollection<double> buffer2= new BlockingCollection<double>();
        static BlockingCollection<double> buffer3= new BlockingCollection<double>();
        static void Main(string[] args)
        {        
            saveticks = t.Ticks;
            new Thread(fact).Start();
            new Thread(power).Start();
            new Thread(sum).Start();
            
            new Thread(addUp).Start();
            Console.WriteLine("hello!");
        }
        static void addUp(){
            double counter = 0;
            for(int i=0; i <= SIZE; i++){
                counter += buffer3.Take();
            }
            Console.WriteLine(counter);
            done();
        }
        static void sum(){
            buffer3.Add(0);
            for(int i=0; i <=SIZE; i++){
                double x = buffer1.Take();
                double y = buffer2.Take();
                Console.WriteLine("buffer1: " + x + "  Buffer2:" +y + " SUM: " + x*y);
                buffer3.Add(x*y);
            }
        }
        static void power(){
            double power = 0;
            buffer2.Add(0);
            for(int i=2; i<=SIZE; i++){
                power = i * i;
                buffer2.Add(power);
            }
        }
        static void fact(){
            double fact1 = .9999999;
            buffer1.Add(0);
            for(int i=1; i<=SIZE;i++){
                fact1 *= 0.9999999;
                //Console.WriteLine(fact1);
                buffer1.Add(fact1);
            }
        }

        static void done(){
            t = DateTime.Now;
            Console.WriteLine((t.Ticks - saveticks) / 10000000.0);
        }
    }

}

/*
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace _412_Assignment2
{
    class Program
    {

        static BlockingCollection<double> factCol = new BlockingCollection<double>(100000);
        static BlockingCollection<long> expCol = new BlockingCollection<long>(100000);
        static BlockingCollection<double> addendCol = new BlockingCollection<double>();

        static double sum = 0.0;
        static double factor = 0.9999999;
        const int size = 100000000;
        static DateTime pt;
        static long pticks;

        static void Main(string[] args)
        {
            
            DateTime t = DateTime.Now;
            long sticks = t.Ticks;
            //sequential
            double ssum = 0.0;
            double sfact1 = factor;
            for (int i = 1; i <= size; i++)
            {
                ssum += (sfact1 * i * i);
                sfact1 *= factor;
            }
            //report sequential
            Console.WriteLine("Sequential result: " + ssum);
            t = DateTime.Now;
            //report sequential time
            Console.WriteLine("sequential time: " + ((t.Ticks - sticks) / 10000000.0) + " seconds");

            pt = DateTime.Now;
            pticks = t.Ticks;

            new Thread(FactorThread).Start();
            new Thread(ExponentThread).Start();
            new Thread(MultiplyThread).Start();
            new Thread(SumThread).Start();
        }

        static void FactorThread()
        {
            double fact1 = factor;
            factCol.Add(fact1);
            for (int i = 2; i <= size; i++)
            {
                fact1 *= factor;
                factCol.Add(fact1);
            }
        }

        static void ExponentThread()
        {
            for (long i = 1; i <= size; i++)
            {
                long exp = i*i;
                expCol.Add(exp);
            }
               
        }

        static void MultiplyThread()
        {
            for (int i = 1; i <= size; i++)
            {
                double addend = 0.0;
                long exp = expCol.Take();
                double fact = factCol.Take();
                addend = exp * fact;
                addendCol.Add(addend);
            }
        }

        static void SumThread()
        {
            for (int i = 1; i <= size; i++)
            {
                double add = addendCol.Take();
                sum += add;
               
            }
            pt = DateTime.Now;
            //report parallel
            Console.WriteLine("parallel result: " + sum);
            Console.WriteLine("parallel time : " + ((pt.Ticks - pticks) / 10000000.0) + " seconds");
            
            
        }
     
    }
}
 */