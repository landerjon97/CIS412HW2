/*
Author: Jonathan Lander
Date: 10/15/2019
Class: CIS 412

Answer: The reason why this parallel process takes so much more time is because that the amount of 
        sharing data and waiting for other threads to catch up causes slowing. Some cores are wasting
        time waiting on a buffer to fill backup and others cores are stressing. This is also a load
        balancing issue. Not only that, but at the end it is possible that the only thread that is
        working is the thread that has to finalize buffering the last buffer3. 
 */
using System;
using  System.Threading;
using System.Collections.Concurrent;

namespace hw2
{
    class Program
    {
        static int SIZE = 100000000;
        static DateTime t = DateTime.Now;
        static long saveticks = 0;
        static BlockingCollection<double> buffer1= new BlockingCollection<double>();
        static BlockingCollection<long> buffer2= new BlockingCollection<long>();
        static BlockingCollection<double> buffer3= new BlockingCollection<double>();
        static void Main(string[] args)
        {   
            double summ = 0.0;
            double fact1 = 0.9999999;
            //save current time
            DateTime t = DateTime.Now;
            saveticks = t.Ticks;
           	//start sequential summation 
            for (int i = 1; i <= SIZE; i++)
            {
                summ += (fact1 * i * i);
                //Console.WriteLine("sum1: " + sum);
                fact1 *= 0.9999999;
                //Console.WriteLine("buffer1: " + fact1 + "Buffer2: " + i*i + "sum: " + fact1 * i * i);
            }
            //get current time and get the difference to give sequential speed
            t = DateTime.Now;
            Console.WriteLine("sequential: " + ((t.Ticks - saveticks) / 10000000.0) + " seconds");
            Console.WriteLine("Sequential SUM: " + summ);

            saveticks = t.Ticks;
            new Thread(fact).Start();
            new Thread(power).Start();
            new Thread(sum).Start();
            new Thread(addUp).Start();
        }
        static void addUp(){
            double counter = 0;
            for(int i=1; i <= SIZE; i++){
                counter += buffer3.Take();
            }
            Console.WriteLine("Parallel Sum: " + counter);
            done();
        }
        static void sum(){
            for(int i=1; i <=SIZE; i++){
                double x = buffer1.Take();
                double y = buffer2.Take();
                //Console.WriteLine("buffer1: " + x + "  Buffer2:" +y + " SUM: " + x*y);
                buffer3.Add(x*y);
            }
        }
        static void power(){
            for(long i=1; i<=SIZE; i++){
                buffer2.Add(i*i);
            }
        }
        static void fact(){
            double fact1 = .9999999;
            buffer1.Add(fact1);
            for(int i=2; i<=SIZE;i++){
                fact1 *= 0.9999999;
                buffer1.Add(fact1);
                //Console.WriteLine(fact1);
            }
        }

        static void done(){
            t = DateTime.Now;
            Console.WriteLine("Time that parallel took: " + (t.Ticks - saveticks) / 10000000.0 + "seconds");
        }
    }

}