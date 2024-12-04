namespace pizzeria
{
    public class Pizzaiolo // you can alter the signature and contents of this class as needed
                           // it is not allowed to remove code.
    {
        public int _id { get; private set; }
        //constructor
        public Pizzaiolo(int id)
        {
            this._id = id;
        }
        public void start()
        {
            Program.orderMutex.WaitOne();   // wait untill first order is placed
                                            // maybe use flag for this instead of mutex
            Program.orderMutex.ReleaseMutex();
            life();
        }
        public void life() // pizzaiolo: feel free to add instructions to make it thread safe.
        {
            // wait for a customer to order a pizza slice

            Thread.Sleep(new Random().Next(50, 200));

            PizzaOrder p = new();

            Console.WriteLine($"Pizzaiolo {_id} is about to take the pizza order");

            Program.orderMutex.WaitOne(); // lock
            try
            {
                p = Program.order.First(); // ERROR: is empty list
                Program.order.RemoveFirst();
                Program.orderMutex.ReleaseMutex(); // unlock
            }
            catch
            {
                Program.orderMutex.ReleaseMutex(); // unlock
                life();
            };

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

            Program.workingsurfaceMutex.WaitOne();// lock
            if (Program.workingsurface.Count < Program.n_slices)
            {
                Program.workingsurface.AddFirst(s);
                //Console.WriteLine($"Pizzaiolo {_id} deposited a slice."); //this is for debug purposes

                if (Program.workingsurface.Count == Program.n_slices)
                {
                    Program.pickupMutex.WaitOne(); // lock
                    Program.pickUp.AddFirst(new PizzaDish(Program.n_slices, s.ToString()));
                    //Console.WriteLine($"Pizzaiolo {_id} deposited a pizza {s.ToString()}."); //this is for debug purposes
                    Program.pickupMutex.ReleaseMutex(); // unlock
                    Program.workingsurface.Clear();
                    Program.workingsurfaceMutex.ReleaseMutex();// unlock
                }
            }
            else
            {
                throw new Exception($"Pizzaiolo {_id} [MISTAKE!]");
                //Console.WriteLine($"Pizzaiolo{_id} [MISTAKE!]"); //this is for debug purposes
            }


            Console.WriteLine($"Pizzaiolo{_id} finished and goes to sleep.");
            // if the working surface contains exactly n_slices slices
            //          add the pizza to the pick up
        }
    }
}