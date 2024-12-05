using System.Security.Claims;

namespace pizzeria
{
    public class Pizzaiolo // you can alter the signature and contents of this class as needed
                           // it is not allowed to remove code.
    {
        public int _id { get; private set; }
        //constructor
        public Pizzaiolo(int id) { this._id = id; }
        public void start()
        {
            Program.orderSemaphore.WaitOne();
            life();
        }
        public void life() // pizzaiolo: feel free to add instructions to make it thread safe.
        {
            // wait for a customer to order a pizza slice
            Thread.Sleep(new Random().Next(50, 200));
            PizzaOrder p;
            Console.WriteLine($"Pizzaiolo {_id} is about to take the pizza order");

            Program.orderMutex.WaitOne();
            p = Program.order.First();
            Program.order.RemoveFirst();
            Program.orderMutex.ReleaseMutex();

            //work on pizza
            Thread.Sleep(new Random().Next(50, 200));
            p.StartWorking();
            Thread.Sleep(new Random().Next(50, 200));

            //finish pizza slice
            Console.WriteLine($"Pizzaiolo {_id} about to finish the pizza slice");
            PizzaSlice s = p.FinishWorking(_id.ToString()); //feel free to change the init of the ID
                                                            // with anything that can help you debug
                                                            // go to woking surface
                                                            // wait for the working surface to be available
                                                            // deposit the pizza slice
                                                            // if the working surface contains less than n_slices slices
                                                            //          Add the pizza slice to the working surface
            Console.WriteLine($"Pizzaiolo {_id} about to deposit the pizza slice");
            //if the pizza is not full
            //      add the pizza slice to the working surface
            //if the pizza is full
            //      clear the working surface
            //      add the pizza to the pick up

            Program.workingsurfaceMutex.WaitOne();
            if (Program.workingsurface.Count < Program.n_slices)
            {
                Program.workingsurface.AddFirst(s);
                //Console.WriteLine($"Pizzaiolo {_id} deposited a slice."); //this is for debug purposes
                if (Program.workingsurface.Count == Program.n_slices)
                {
                    Program.workingsurface.Clear();
                    Program.workingsurfaceMutex.ReleaseMutex();

                    Program.pickupMutex.WaitOne(); // claim pickup counter
                    Program.pickUp.AddFirst(new PizzaDish(Program.n_slices, s.ToString()));
                    // Console.WriteLine($"Pizzaiolo {_id} deposited the pizza {s.ToString()}."); //this is for debug purposes
                    Program.pickupMutex.ReleaseMutex(); // release pickup counter

                    for (int i = 0; i < Program.n_slices; i++)
                    {
                        Program.pickupSemaphore.Release();
                    }
                }
                else { Program.workingsurfaceMutex.ReleaseMutex(); }
            }
            else
            {
                Program.workingsurfaceMutex.ReleaseMutex();
                throw new Exception($"Pizzaiolo {_id} [MISTAKE!]");
                //Console.WriteLine($"Pizzaiolo{_id} [MISTAKE!]"); //this is for debug purposes
            }
            Console.WriteLine($"Pizzaiolo{_id} finished and goes to sleep.");
            // if the working surface contains exactly n_slices slices
            //          add the pizza to the pick up
        }
    }
}