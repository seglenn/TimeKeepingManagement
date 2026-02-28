using System;

class Program
{
    static void Main(string[] args)
    {
        int choice;

        Console.WriteLine("=================================");
        Console.WriteLine("     TIME KEEPING MANAGEMENT     ");
        Console.WriteLine("=================================\n");

        //Employee Registration - DONE
        //Employee shift (Dynamic shift) - DONE
        //Employee Time-in
        //Employee Overtime (Compute Overtime)
        //Include late policies 
        //Late policies (5 mins late)
        //Time out - Out time (Morning or Night shift)
        //Salary computation (Compute salary based on time in and time out) (Pending)
        //if employeeTimeOut > outNightTime = overtime 


        Console.WriteLine("---------------------------------");
        Console.WriteLine("Please select an operation below:");
        Console.WriteLine("--------------------------------\n");

        Console.WriteLine("Operations: ");
        Console.WriteLine("[ 1.] Employee Registration");
        Console.WriteLine("[ 2.] Time-in & Time-out");
        Console.WriteLine("[ 3.] Late Records");
        Console.WriteLine("[ 4.] View Records");
        Console.WriteLine("[ 5.] Exit \n");

        Console.Write("Enter a number: ");
        choice = Convert.ToInt32(Console.ReadLine());

        switch (choice)
        {
            case 1:
                Console.WriteLine("\nYou selected: Employee Registration");
                EmployeeRegistration();
                break;
            case 2:
                Console.WriteLine("\nYou selected: Time-in and Time-out");
                TimeInAndTimeOut();
                break;
            case 3:
                Console.WriteLine("\nYou selected: Late Records");

                break;
            case 4:
                Console.WriteLine("\nYou selected: View All Records");
                break; 
            case 5:
                Console.WriteLine("\nExiting the program.");
                return;
            default:
                Console.WriteLine("\nInvalid choice. Please select a valid operation.");
                return;
        }

        
        static void EmployeeRegistration() {

            string employeeName, employeeId, shift;

            Console.WriteLine("=== EMPLOYEE REGISTRATION: === \n");

            Console.Write("Enter your name: ");
            employeeName = Console.ReadLine();
            Console.Write("Enter your Employee ID number: ");
            employeeId = Console.ReadLine();

            Console.WriteLine("\nSELECT YOUR SHIFT:");
            Console.WriteLine("\n--------------------------------");
            Console.WriteLine("Morning Shift: 9:00 AM - 5:00 PM");
            Console.WriteLine("Night Shift: 11:00 PM - 7:00 AM");
            Console.WriteLine("--------------------------------\n");

            Console.Write("Enter your shift (Morning/Night): ");
            shift = Console.ReadLine();

            if (shift == "Morning" || shift == "morning")
            {
                Console.WriteLine($"\nEmployee Name: {employeeName}");
                Console.WriteLine($"Employee ID: {employeeId}");
                Console.WriteLine($"Shift: Morning Shift (9:00 AM - 5:00 PM)");
                Console.WriteLine("Registration successful.");
            }
            else if (shift == "Night" || shift == "night")
            {
                Console.WriteLine($"\nEmployee Name: {employeeName}");
                Console.WriteLine($"Employee ID: {employeeId}");
                Console.WriteLine($"Shift: Night Shift (11:00 PM - 7:00 AM)");
                Console.WriteLine("Registration successful.");
            }
            else
            {
                Console.WriteLine("Invalid shift selection. Please enter 'Morning' or 'Night'.");
                return;
            }
        }




        static void TimeInAndTimeOut() {

            Console.Write("TIME IN: ");
            string timeIn = Console.ReadLine();
            Console.Write("TIME OUT: ");
            string timeOut = Console.ReadLine();

            DateTime employeeTimeIn, employeeTimeOut;

            if (DateTime.TryParse(timeIn, out employeeTimeIn) && DateTime.TryParse(timeOut, out employeeTimeOut))
            {
                Console.WriteLine($"Time in: Hour: {employeeTimeIn.Hour}, Minute: {employeeTimeIn.Minute}");
                Console.WriteLine($"Time out: Hour: {employeeTimeOut.Hour}, Minute: {employeeTimeOut.Minute}"); 
                Console.WriteLine("Time in recorded successfully.");
            }
            else
            {
                Console.WriteLine("Invalid time format. Please enter a valid time.");
                return;
            }
        }



        static void LateRecords() {

            //method to methods is alright, just need to call the method in the switch case and it will work, no need to create an instance of the class since we are using static methods

            if ({ employeeTimeIn.Hour}){


            }

            else{

            }

        }



        static void ViewAllRecords()
        {



        }

        












    }
}
