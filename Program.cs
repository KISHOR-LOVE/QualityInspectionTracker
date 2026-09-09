using System;
using System.Collections.Generic;
using System.IO;

namespace QualityInspectionTracker
{   
    
    
    static void Main()
    {
        Console.Write("Enter Product ID: ");
        string productId = Console.ReadLine();

        Console.Write("Enter Product Length: ");
        double length = Convert.ToDouble(Console.ReadLine());

        Console.Write("Enter Product Width: ");
        double width = Convert.ToDouble(Console.ReadLine());

        // Quality limits
        double minLength = 10;
        double maxLength = 12;

        double minWidth = 5;
        double maxWidth = 7;

        // Inspection
        if (length >= minLength && length <= maxLength &&
            width >= minWidth && width <= maxWidth)
        {
            Console.WriteLine("Product: " + productId);
            Console.WriteLine("Result: PASS");
        }
        else
        {
            Console.WriteLine("Product: " + productId);
            Console.WriteLine("Result: FAIL");
        }
    }
}
















