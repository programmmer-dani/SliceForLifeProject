using System;
using System.Threading;
//no need to add any other using directives

namespace pizzeria //this is useless, if you remove it your assignment will be NVL
{
    internal class Program // feel free to add methods/variables to this class
    {
        public static int n_slices = 1; // Number of slices per pizza, 
                                        // maximum amount of customers per pizza default: 4
                                        // IF n_slices INCREASED, NEW BUGS OCCUR
        public static int n_customers = 12; // must be a multiple of n_slices, default: 1000
        public static int n_pizzaioli = n_customers; // must be the same as n_customers
        public static Mutex orderMutex = new Mutex();
        public static Mutex pickupMutex = new Mutex();
        public static Mutex workingsurfaceMutex = new Mutex();
        public static Thread[] pizzaioliThreads = new Thread[n_customers];
        public static Thread[] customerThreads = new Thread[n_customers];
        //do not change any class variable under this line
        public static LinkedList<PizzaOrder> order = new(); // DANI: mutex
        public static LinkedList<PizzaDish> pickUp = new(); // DANI: mutex - every customer picks 1 slices + (?semaphore? allowing 4 clients to wait inside....?)
        public static LinkedList<PizzaSlice> workingsurface = new();// DANI: mutex (list containing n_slices (4) )
        public static Pizzaiolo[] pizzaioli = new Pizzaiolo[n_pizzaioli];
        public static Customer[] customers = new Customer[n_customers];
        static void Main(string[] args)
        {

            if (n_customers % n_slices != 0) //check if n_customers is a multiple of n_slices DO NOT ALTER THIS CODE.
            {
                throw new Exception("n_customers must be a multiple of n_slices");
            }
            //init environment variables here if needed

            //do not change any code of the following 3 function call. You can add code before and after them
            //init pizzaioli and customers
            InitPeople();
            Console.WriteLine("people initiated");
            //activate pizzaioli
            ActivatePizzaioli();
            System.Console.WriteLine("pizzaioli active");
            //activate customers
            ActivateCustomers();
            System.Console.WriteLine("customers active");
            // insert code here if necessary

            // DANI: join all the threads

            // DO NOT ADD OR MODIFY CODE AFTER THIS LINE, if you do, your assignment will be NVL
            Console.WriteLine("All should customers have eaten a pizza slice.");
            Console.WriteLine($"Pickup location: There are {pickUp.Count} pizzas left.");
            Console.WriteLine($"Working location: There are {workingsurface.Count} slices left.");
            Console.WriteLine($"Order location: There are {order.Count} orders left.");
        }

        private static void ActivateCustomers() // todo: implement this method
        {
            for (int i = 0; i < customers.Length - 1; i++)
            {
                customerThreads[i].Start();
            }
            //joinAllThreads();
        }

        private static void ActivatePizzaioli() //todo: implement this method
        {
            for (int i = 0; i < pizzaioli.Length - 1; i++)
            {
                // here start threads from the list
                pizzaioliThreads[i].Start();
            }
        }

        private static void InitPeople()
        {
            for (int i = 0; i < n_customers; i++)
            {
                //customers[i] = new Customer(i + 1);
                //pizzaioli[i] = new Pizzaiolo(i + 1);
                Pizzaiolo pizziolo = new Pizzaiolo(i + 1);
                Customer customer = new Customer(i + 1);
                pizzaioliThreads[i] = new Thread(() => pizziolo.start());
                customerThreads[i] = new Thread(() => customer.start());
            }
        }
    }

    public enum OrderState //DO NOT TOUCH THIS ENUM
    {
        Ordered,
        Working,
        Ready
    }
    public class PizzaDish //DO NOT TOUCH THIS CLASS
    {
        public int Slices { get; private set; }
        private string _id;
        public PizzaDish(int slices, string id)
        {
            Slices = slices;
            _id = id;
        }
        public int RemoveSlice()
        {
            if (Slices > 0)
            {
                Slices--;
            }
            else
            {
                throw new Exception($"{_id} No more slices");
            }
            return Slices;
        }
    }
    public class PizzaOrder //DO NOT TOUCH THIS CLASS
    {
        private int sliceprepared = 0;
        public OrderState State { get; private set; }
        public PizzaOrder()
        {
            State = OrderState.Ordered;
        }

        public void StartWorking()
        {
            State = OrderState.Working;

        }

        public PizzaSlice FinishWorking(string sliceId)
        {
            State = OrderState.Ready;
            return new PizzaSlice(sliceId);
        }
    }

    public class PizzaSlice //DO NOT TOUCH THIS CLASS
    {
        private string _id;
        public PizzaSlice(string id)
        {
            //constructor
            _id = id;
        }

    }
}