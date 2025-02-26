using System;
using System.IO;
using System.Diagnostics;
using System.IO.Pipes;

class Consumer
{
    public static void Main(string[] args)
    {
        //create same pipe name that consumer will connect to 
        string pipe = "BinaryPipe"; 
        
        //create client pipe object
        using(NamedPipeClientStream pipeClient = new NamedPipeClientStream(".", pipe, PipeDirection.In))
        {
            Console.WriteLine("Consumer: Connecting to producer... ");
            //connect to named pipe 
            pipeClient.Connect();
            Console.WriteLine("Consumer: Connected! ");

            //create streamreader for reading data from pipe 
            using(StreamReader reader = new StreamReader(pipeClient))
            {
                string line; 
                while((line = reader.ReadLine()) != null)
                {
                    long receivedTimeStamp = Stopwatch.GetTimestamp();
                    string[] parts = line.Split(',');
                    if(parts.Length != 2)
                    {
                        Console.WriteLine("Consumer: Received invalid data format. ");
                        continue;
                    }
                    int numb = int.Parse(parts[0]);
                    long sentTimeStamp = long.Parse(parts[1]);

                    //convert decimal number to binary
                    string binary = Convert.ToString(numb, 2);

                    //calculate latency
                   double latencyMs = (receivedTimeStamp - sentTimeStamp) * (1000.0 / Stopwatch.Frequency);


                    //print results to screen
                    Console.WriteLine($"Consumer: Decimal {numb} in binary is {binary} ");
                    Console.WriteLine($"Consumer: Latency: {latencyMs} ms");
                
                }
            }
        }
    }
}