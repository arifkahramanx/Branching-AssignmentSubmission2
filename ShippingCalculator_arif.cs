// Express Shipping Rate Calculator
// Developer: Kevin O'Brien
// Last Updated: March 2024
using System;

namespace ShippingExpress.States
{
    // State interface
    public interface IShippingState
    {
        void ProcessInput(ShippingContext context);
        bool IsComplete { get; }
    }

    // Context class
    public class ShippingContext
    {
        public IShippingState CurrentState { get; set; }
        public double Weight { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double Length { get; set; }

        public ShippingContext()
        {
            CurrentState = new WeightInputState();
        }

        public void ProcessInput()
        {
            CurrentState.ProcessInput(this);
        }
    }

    // Weight input state
    public class WeightInputState : IShippingState
    {
        public bool IsComplete { get; private set; }

        public void ProcessInput(ShippingContext context)
        {
            Console.WriteLine("Please enter the package weight:");
            if (!double.TryParse(Console.ReadLine(), out double weight))
            {
                Console.WriteLine("Invalid weight input.");
                return;
            }

            if (weight > 50)
            {
                Console.WriteLine("Package too heavy to be shipped via Package Express. Have a good day.");
                return;
            }

            context.Weight = weight;
            context.CurrentState = new DimensionsInputState();
            IsComplete = true;
        }
    }

    // Dimensions input state
    public class DimensionsInputState : IShippingState
    {
        public bool IsComplete { get; private set; }

        public void ProcessInput(ShippingContext context)
        {
            Console.WriteLine("Please enter the package width:");
            if (!double.TryParse(Console.ReadLine(), out double width))
            {
                Console.WriteLine("Invalid width input.");
                return;
            }

            Console.WriteLine("Please enter the package height:");
            if (!double.TryParse(Console.ReadLine(), out double height))
            {
                Console.WriteLine("Invalid height input.");
                return;
            }

            Console.WriteLine("Please enter the package length:");
            if (!double.TryParse(Console.ReadLine(), out double length))
            {
                Console.WriteLine("Invalid length input.");
                return;
            }

            double totalSize = width + height + length;
            if (totalSize > 50)
            {
                Console.WriteLine("Package too big to be shipped via Package Express.");
                return;
            }

            context.Width = width;
            context.Height = height;
            context.Length = length;
            context.CurrentState = new CalculationState();
            IsComplete = true;
        }
    }

    // Calculation state
    public class CalculationState : IShippingState
    {
        public bool IsComplete { get; private set; }

        public void ProcessInput(ShippingContext context)
        {
            double shippingCost = (context.Width * context.Height * context.Length * context.Weight) / 100;
            Console.WriteLine($"Your estimated total for shipping this package is: ${shippingCost:F2}");
            Console.WriteLine("Thank you!");
            IsComplete = true;
        }
    }

    // Main program
    class Program
    {
        static void Main(string[] args)
        {
            // Show program header
            Console.WriteLine("Welcome to Package Express. Please follow the instructions below.");

            try
            {
                var context = new ShippingContext();
                while (!context.CurrentState.IsComplete)
                {
                    context.ProcessInput();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
} 