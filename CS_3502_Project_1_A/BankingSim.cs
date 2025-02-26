using System;
using System.Threading;


class BankAccount
{
public int ID {get;} //unique read-only account id variable 
private Mutex mutex = new Mutex(); //mutex to ensure proper synchronization (phase 2)
private double AccountBalance; //store account balance

//constructor to initialize bank account 
public BankAccount(int ID, double initialAccountBal)
{
    this.ID = ID;
    AccountBalance = initialAccountBal;
}

//method for depositing money into the account 
public void DepositMoney(double amount)
{
    mutex.WaitOne(); //acquire mutex lock before modifying account bal
    try
        {
            AccountBalance += amount; //adjust account bal
            Console.WriteLine($"Successfully deposited ${amount} to Account {ID}. Balance: ${AccountBalance}");
        }
        finally
        {
            mutex.ReleaseMutex(); //release lock
        }
}

//method to withdraw money from the account 
public bool WithdrawalMoney(double amount)
{
    mutex.WaitOne(); //acquire lock before modifying account bal
        try
        {
            if (AccountBalance >= amount)
            {
                AccountBalance -= amount; //if amount does not exceed account bal, perform withdrawal
                Console.WriteLine($"Successfully withdrew ${amount} from Account {ID}. Balance: ${AccountBalance}");
                return true; 
            }
            else //amount exceeded account balance 
            {
                Console.WriteLine($"Withdrawal failed. Insufficient funds in Account {ID}. Balance: ${AccountBalance}");
                return false;
            }
        }
        finally
        {
            mutex.ReleaseMutex(); //release lock
        }
}

//method for transferring money between accounts (phase 3)
public static void TransferMoney(BankAccount sourceAccnt, BankAccount destinationAccnt, double amount)
{
    bool sourceLocked = false, destinationLocked = false; //track mutex status with these boolean variables
    int failedAttempts = 0; //use counter for lock attempts 
    while(failedAttempts < 3)
    {
        try
        {
            //attempt to lock source account
            Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} attempting to lock Account {sourceAccnt.ID}");
            sourceLocked = sourceAccnt.mutex.WaitOne(500);
            if(!sourceLocked) //if lock attempt fails
            {
                Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} failed to lock Account {sourceAccnt.ID}, retrying...");
                failedAttempts++; //increase attempt counter 
                continue;
            }

            //attempt to lock destination account 
            destinationLocked = destinationAccnt.mutex.WaitOne(500); // Try to acquire lock for destination account
            if(!destinationLocked) //if lock attempt fails 
            {
                Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} failed to lock Account {destinationAccnt.ID}, retrying...");
                    failedAttempts++; //increase attempt counter 
                    continue;
            }
            
            //if both accounts are locked, perform the transfer
                if (sourceAccnt.WithdrawalMoney(amount))
                {
                    destinationAccnt.DepositMoney(amount);
                }
                return; //successfully completed transfer
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Error in transaction: {ex.Message}"); //display error message to user
            failedAttempts++;
            Thread.Sleep(100); //delay before retrying 
        }
        finally
        {
            //release destination lock if acquired 
            if(destinationLocked)
            {
                destinationAccnt.mutex.ReleaseMutex();
            }

            //release source lock if acquired 
            if(sourceLocked)
            {
                sourceAccnt.mutex.ReleaseMutex();
            }
        }
    }
    Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} reached maximum attempts. Aborting transfer to prevent deadlock.");
  }
}

//main program
class BankingSim
{
    public static void Main(string[] args)
    {
        //create a list of bank accounts for individual transactions and multi-account transfers 
        List<BankAccount> accounts = new List<BankAccount>
        {
            new BankAccount(1, 500),
            new BankAccount(2, 1000),
            new BankAccount(3, 1500),
        };

        Thread[] threads = new Thread[20]; //create 20 threads for stress testing 
        Random random = new Random(); //random object to randomly select accounts 

        //phase 1: first 10 threads for individual transactions (deposit and withdrawal)
        for(int i = 0; i < 10; i++)
        {
            //select random account 
            int index = random.Next(accounts.Count);

            //define the method that each thread will execute 
            threads[i] = new Thread(() =>
            {
                accounts[index].DepositMoney(150);
                accounts[index].WithdrawalMoney(75);
            }) 
            {
                Name = $"Thread {i+1}" //name each thread (phase 1)
            }; 
        }


        //phase 3/4: next 10 threads for multi-account transfers
        for(int i = 10; i < 20; i++)
        {
            int sourceIndex, destinationIndex;
            
            //make sure the source and destination indexes are different
            do
            {
                sourceIndex = random.Next(accounts.Count);
                destinationIndex = random.Next(accounts.Count);
            }
            while(sourceIndex == destinationIndex);

            threads[i] = new Thread(() =>
            {
                double amount = random.Next(50, 200); //generate transfer amount between 50-199
                BankAccount.TransferMoney(accounts[sourceIndex], accounts[destinationIndex], amount);
            })
            {
                Name = $"Transfer Thread {i - 9}" //name transfer threads for debugging
            };
        }

        //start each thread
        foreach(Thread thread in threads)
        {
            thread.Start();
        }

        //wait for each thread to finish 
        foreach(Thread thread in threads)
        {
            thread.Join();
        }
    
        Console.WriteLine("\nAll transfers and transactions are complete! ");
    }
}