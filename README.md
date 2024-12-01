A Slice 4 My Life!
This repository contains the solution to the Concurrency Exam assignment INFCOCx1-A for the academic year 2024-2025. The goal is to implement a multithreaded simulation of a fast-pickup pizza restaurant using .NET 6.

The application features:

N pizzaioli (pizza makers) collaboratively creating pizzas one slice at a time.
N customers independently ordering and picking up a single slice of pizza.
A thread-safe implementation ensuring fairness and correctness.
Synchronization mechanisms such as mutexes, locks, or semaphores to manage shared resources like orders, the kitchen space, and pickup areas.
Key Functional Requirements
Fairness & Correctness: Every thread must complete its tasks without interference, ensuring order and pickup consistency.
Efficient Concurrency: Minimize locking while avoiding spinning and overprotection.
Separation of Responsibilities: Pizzaioli prepare the slices collaboratively; customers take turns picking up slices.
Thread Safety: No random interleaving or race conditions.
Project Constraints
No user interaction: Fully autonomous simulation with predefined input parameters.
Cross-platform support: The solution runs on any system supporting .NET 6.
Standard libraries only: Avoid external libraries, async/await, or parallel-for constructs.
No spinning: Ensure synchronization without unnecessary CPU load.
Evaluation Criteria
The repository follows strict guidelines for thread-safe design, logical correctness, and adherence to the project’s constraints. Testing includes a variety of configurations for thread counts, pizza slices, and workload.

Deadline: January 24, 2025
Submission Link: Microsoft Forms

Getting Started
Clone the repository, review the provided code structure, and implement the required changes to enable concurrency. Use the .dotnet run command to test the application and ensure it meets all requirements.

Enjoy coding, and buon appetito! 🍕
