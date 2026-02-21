using System;

class Program
{
    static void Main(string[] args)
    {

        string employeeName;
        int employeeId;

        Console.WriteLine("=======================");
        Console.WriteLine("TIME KEEPING MANAGEMENT");
        Console.WriteLine("=======================\n");

        //Employee Registration
        //Employee shift (Dynamic shift)
        //Employee Time-in
        

        //Employee Overtime (Compute Overtime)
        //Include late policies 


        Console.WriteLine("Employee Registration: \n");

        Console.WriteLine("Enter your name: ");
        employeeName = Console.ReadLine();
        Console.WriteLine("Enter your Employee ID number: \n");
        employeeId = int.Parse(Console.ReadLine());


        Console.WriteLine("\n--------------------------------");
        Console.WriteLine("Morning Shift: 9:00 AM - 5:00 PM");
        Console.WriteLine("Night Shift: 11:00 PM - 7:00 AM");
        Console.WriteLine("--------------------------------\n");

        Console.WriteLine("TIME IN: ");
        string timeIn = Console.ReadLine();

        DateTime employeeTimeIn;

        if (DateTime.TryParse(timeIn, out employeeTimeIn))
        {
            Console.WriteLine($"Time in: Hour: {employeeTimeIn.Hour}, Minute: {employeeTimeIn.Minute}");
            Console.WriteLine("Time in recorded successfully.");
        }
        else
        {
            Console.WriteLine("Invalid time format. Please enter a valid time.");
            return;
        }

        //Late policies (5 mins late)
        //Time out - Out time (Morning or Night shift)

        //if employeeTimeOut > outNightTime = overtime 



    }
}
