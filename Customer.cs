namespace pizzeria
{
    public class Customer // you can alter the signature and contents of this class as needed DANI: maybe this means injecting the mutex? (into start)
                          // it is not allowed to remove code.
    {
        public int _id { get; private set; }
        public Customer(int id) { this._id = id; }
        public void start() { life(); }
        public void life() // customer: feel free to add instructions to make it thread safe.
        {
            Thread.Sleep(new Random().Next(50, 200));
            //wait to order a pizza slice
            //order pizza slice
            PizzaOrder p = new();

            Program.orderMutex.WaitOne();

            Console.WriteLine($"Customer {_id} about to order a pizza slice");

            Program.order.AddFirst(p);

            Console.WriteLine($"Customer {_id} waits for a pizza slice");

            Program.orderMutex.ReleaseMutex();
            Program.orderSemaphore.Release();

            // wait a bit
            Thread.Sleep(new Random().Next(100, 500));
            // only up to 4 people can get a slice from the same pizza
            // pick up pizza slice when possible
            // no more than n_slices slices per pizza so no more than n_slices customers time over the order.
            PizzaDish pizza = new PizzaDish(0, "");
            var temp = false;

            Program.pickupSemaphore.WaitOne(); // wait until (notified) that pizza is ready (happens 4 at a time)
            Program.pickupMutex.WaitOne(); // claim 1 slice
            pizza = Program.pickUp.First(); //remove one slice

            System.Console.WriteLine($"Customer {_id} picked up his order | {pizza.Slices} : {Program.pickUp.Count}");

            pizza.RemoveSlice();

            if (pizza.Slices == 0) //the dish is empty take it out.
            {
                //if it is the last slice remove the pizza from the pick up
                Program.pickUp.RemoveFirst();
                temp = true;
            }

            Program.pickupMutex.ReleaseMutex();

            // ?????????????????? VVVVVV
            if (temp) { }//  Console.WriteLine($"Customer {_id} has eaten a pizza the final slice total slices: {pizza.Slices} {Program.pickUp.Count}");
            //Console.WriteLine($"Customer {_id} has eaten a slice a pizza total slices: {pizza.Slices} {Program.pickUp.Count}");
        }
    }
}