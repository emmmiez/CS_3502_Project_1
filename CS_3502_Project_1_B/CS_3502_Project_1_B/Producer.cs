using System;
using System.Diagnostics;
using System.IO.Pipes;

class Producer
{
    public static void Main(string[] args)
    {
        //define pipe name
        string pipe = "BinaryPipe";

        //create server object 
        using(NamedPipeServerStream pipeServer = new NamedPipeServerStream(pipe))
        {
            Console.WriteLine("Producer: Waiting for consumer to connect...");
            pipeServer.WaitForConnection(); //wait until consumer connects to the pipe
            Console.WriteLine("Producer: Connected! ");

            //use a streamwriter to write to the pipe
            using(StreamWriter writer = new StreamWriter(pipeServer))
            {
                writer.AutoFlush = true; //flush after each write
                
                //random number generator for simulating data
                Random randNumb = new Random();
                //stopwatch for performance testing 
                Stopwatch stopwatch = new Stopwatch();

                //generate 5 rand<om numbers for conversion
                for(int i = 0; i < 5; i++)
                {
                    try
                    {
                        int numb = randNumb.Next(1, 100); //generate a number and assign a variable
                        long sentTimeStamp = Stopwatch.GetTimestamp(); //for testing
                        Console.WriteLine($"Producer: Sending decimal number {numb} at timestamp {sentTimeStamp}");

                        writer.WriteLine($"{numb}, {sentTimeStamp}"); //send the number and timestamp to the consumer through pipe
                    
                        //simulate small delay before sending next number 
                        Thread.Sleep(1000); 
                    }
                    catch(IOException ex)
                    {
                        Console.WriteLine("Producer: Error in writing data. Terminating process: " +ex.Message);
                        break;
                    }
                }
            }
        }
    }
}