using System;
using System.Collections.Generic;

class Program
{
    static List<string> employeeNames = new List<string>();
    static List<string> employeeIds = new List<string>();
    static List<string> employeeShifts = new List<string>();

    static List<DateTime> timeInRecords = new List<DateTime>();
    static List<DateTime> timeOutRecords = new List<DateTime>();
    static List<string> attendanceId =  new List<string>();


    static void Main(string[] args)
    {
        Console.WriteLine("=================================");
        Console.WriteLine("     TIME KEEPING MANAGEMENT     ");
        Console.WriteLine("=================================\n");

        bool isContinue = true;
        int choice = 0;

        while (isContinue)
        {
            Console.WriteLine("---------------------------------");
            Console.WriteLine("Please select an operation below:");
            Console.WriteLine("---------------------------------\n");

            Console.WriteLine("Operations: ");
            Console.WriteLine("[ 1.] Employee Registration");
            Console.WriteLine("[ 2.] Time-in & Time-out");
            Console.WriteLine("[ 3.] Late Records");
            Console.WriteLine("[ 4.] Shifting Schedule");
            Console.WriteLine("[ 5.] View Records");
            Console.WriteLine("[ 6.] Exit \n");

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
                    LateRecords();
                    break;
                case 4:
                    Console.WriteLine("\nYou selected: Shifting Schedule");
                    ShiftSched();
                    break;
                case 5:
                    Console.WriteLine("\nYou selected: View All Records");
                    ViewAllRecords();
                    break;
                case 6:
                    Console.WriteLine("\nExiting the program.");
                    return;
                default:
                    Console.WriteLine("\nInvalid choice. Please select a valid operation.");
                    break;
            }


            if (choice != 6)
            {
                isContinue = ShowOptions();

                Console.WriteLine();
            }

        }


        static bool ShowOptions()
        {
            Console.Write("\nDo you want to continue? (Y/N): ");
            bool isContinue = false;
            string userInput = Console.ReadLine();

            switch (userInput.ToLower())
            {
                case "y":
                    isContinue = true;
                    break;
                case "n":
                    isContinue = false;
                    Console.WriteLine("\nExiting the program.");
                    return false;
                default:
                    Console.WriteLine("\nInvalid input. Please enter 'Y' or 'N'.");
                    Environment.Exit(0);
                    return false;
            }

            return isContinue;
        }


        static void EmployeeRegistration() {

            string employeeName, employeeId, shift;

            Console.WriteLine("\n=== EMPLOYEE REGISTRATION: === \n");

            Console.Write("Enter your name: ");
            employeeName = Console.ReadLine();

            Console.Write("Enter your Employee ID number: ");
            employeeId = Console.ReadLine();

            foreach (string id in employeeIds)
            {
                if (id == employeeId)
                {
                    Console.WriteLine("Employee ID already exists. Please enter a unique Employee ID.");
                    return;
                }

            }

            Console.WriteLine("\nSELECT YOUR SHIFT:");
            Console.WriteLine("--------------------------------");
            Console.WriteLine("Morning Shift: 9:00 AM - 5:00 PM");
            Console.WriteLine("Night Shift: 11:00 PM - 7:00 AM");
            Console.WriteLine("--------------------------------\n");

            Console.Write("Enter your shift (Morning/Night): ");
            shift = Console.ReadLine().ToLower();

            if (shift == "morning")
            {
                Console.WriteLine($"\nEmployee Name: {employeeName}");
                Console.WriteLine($"Employee ID: {employeeId}");
                Console.WriteLine($"Shift: Morning Shift (9:00 AM - 5:00 PM)");
                Console.WriteLine("Registration successful.");

                employeeNames.Add(employeeName);
                employeeIds.Add(employeeId);
                employeeShifts.Add(shift);

            }
            else if (shift == "night")
            {
                Console.WriteLine($"\nEmployee Name: {employeeName}");
                Console.WriteLine($"Employee ID: {employeeId}");
                Console.WriteLine($"Shift: Night Shift (11:00 PM - 7:00 AM)");
                Console.WriteLine("Registration successful. ");

                employeeNames.Add(employeeName);
                employeeIds.Add(employeeId);
                employeeShifts.Add(shift);
            }
            else
            {
                Console.WriteLine("Invalid shift selection. Please enter 'Morning' or 'Night'.");
                return;
            }
        }


        static void TimeInAndTimeOut() {

            Console.WriteLine("\n=== TIME-IN AND TIME-OUT: === \n");

            Console.Write("Enter your Employee ID number: ");
            string employeeId = Console.ReadLine();

            int employeeIndex = -1;
            for (int i = 0; i < employeeIds.Count; i++)
            {
                if (employeeIds[i] == employeeId)
                {
                    employeeIndex = i;
                    break;
                }
            }

            if (employeeIndex == -1)
            {
                Console.WriteLine("Employee ID not found. Please register first.");
                return;
            }

            Console.WriteLine($"\nWelcome, {employeeNames[employeeIndex]}! Your shift is: {employeeShifts[employeeIndex]}\n");

            Console.Write("TIME IN (Format: 9:00 AM or 17:45): ");
            string timeIn = Console.ReadLine();
            Console.Write("TIME OUT (Format: 5:00 PM or 17:00): ");
            string timeOut = Console.ReadLine();

            DateTime employeeTimeIn, employeeTimeOut;

            if (DateTime.TryParse(timeIn, out employeeTimeIn) && DateTime.TryParse(timeOut, out employeeTimeOut))
            {

                if (employeeTimeOut <= employeeTimeIn)
                {
                    Console.WriteLine("Time out cannot be earlier than time in. Please enter valid times.");
                    return;
                }

                TimeSpan workDuration = employeeTimeOut - employeeTimeIn;

                timeInRecords.Add(employeeTimeIn);
                timeOutRecords.Add(employeeTimeOut);
                attendanceId.Add(employeeId);

                Console.WriteLine($"Time in:{employeeTimeIn:hh:mm tt}");
                Console.WriteLine($"Time Out:{employeeTimeOut:hh:mm tt}");
                Console.WriteLine($"Total work duration: {workDuration.Hours} hours and {workDuration.Minutes} minutes.");
                Console.WriteLine("Time in recorded successfully.");

            }
            else
            {
                Console.WriteLine("Invalid time format. Please enter a valid time.");
                return;
            }
        }



        static void LateRecords() {

            Console.WriteLine("\n=== LATE RECORDS: === \n");

            if (attendanceId.Count == 0)
            {
                Console.WriteLine("No attendance records found.");
                return;
            }

            TimeSpan morningShiftStart = new TimeSpan(9, 0, 0);
            TimeSpan morningGraceEnd = new TimeSpan(9, 15, 0);

            TimeSpan nightShiftStart = new TimeSpan(23, 0, 0);
            TimeSpan nightGraceEnd = new TimeSpan(23, 15, 0);

            Console.WriteLine("Late Records:");

            int lateCount = 0;

            for (int i = 0; i < attendanceId.Count; i++)
            {
                string employeeId = attendanceId[i];
                DateTime timeIn = timeInRecords[i];
                DateTime timeOut = timeOutRecords[i];

                int employeeIndex = employeeIds.IndexOf(employeeId);
                string shift = employeeShifts[employeeIndex];

                if (shift == "morning")
                {
                    if (timeIn.TimeOfDay > morningGraceEnd)
                    {
                        Console.WriteLine($"Employee ID: {employeeId}, Name: {employeeNames[employeeIndex]}, Shift: Morning, Time In: {timeIn.ToShortTimeString()} - LATE");
                        lateCount++;
                    }
                }
                else if (shift == "night")
                {
                    if (timeIn.TimeOfDay > nightGraceEnd)
                    {
                        Console.WriteLine($"Employee ID: {employeeId}, Name: {employeeNames[employeeIndex]}, Shift: Night, Time In: {timeIn.ToShortTimeString()} - LATE");
                        lateCount++;
                    }
                }
            }

            if (lateCount == 0) {
                Console.WriteLine("No late records found.");
            } else
            {
                Console.WriteLine($"Total late records: {lateCount}");
            }


        }

        static void ShiftSched() {
            Console.WriteLine("\n=== SHIFT SCHEDULE: === \n");

            Console.Write("Enter your Employee ID number: ");
            string employeeId = Console.ReadLine();

            int employeeIndex = -1;
            for (int i = 0; i < employeeIds.Count; i++)
            {
                if (employeeIds[i] == employeeId)
                {
                    employeeIndex = i;
                    break;
                }
            }

            if (employeeIndex == -1)
            {
                Console.WriteLine("Employee ID not found. Please register first.");
                return;
            }

            Console.WriteLine($"\nWelcome, {employeeNames[employeeIndex]}!");
            Console.WriteLine($"\nYour Current shift is: {employeeShifts[employeeIndex]}");

            Console.WriteLine("\nShift Schedule:");
            Console.WriteLine("--------------------------------");
            Console.WriteLine("Morning Shift: 9:00 AM - 5:00 PM");
            Console.WriteLine("Night Shift: 11:00 PM - 7:00 AM");
            Console.WriteLine("--------------------------------\n");

            Console.Write("Enter your new shift (Morning/Night): ");
            string newShift = Console.ReadLine().ToLower();

            if (newShift != "morning" && newShift != "night")
            {
                Console.WriteLine("Invalid shift selection. Please enter 'Morning' or 'Night'.");
                return;
            }

            if (employeeShifts[employeeIndex] == newShift)
            {
                Console.WriteLine($"You are already assigned to the {newShift} shift.");
                return;
            }

            employeeShifts[employeeIndex] = newShift;
            Console.WriteLine($"Shift updated successfully to {newShift} shift.");
        }



        static void ViewAllRecords()
        {
            Console.WriteLine("\n=== ALL ATTENDANCE RECORDS: === \n");          
            

            Console.WriteLine("\nREGISTERED EMPLOYEES:");
            Console.WriteLine("---------------------");

            if (employeeIds.Count == 0)
            {
                Console.WriteLine("No registered employees found.");

            }
            else
            {
                for (int i = 0; i < employeeIds.Count; i++)
                {
                    Console.WriteLine($"Employee ID: {employeeIds[i]}, Name: {employeeNames[i]}, Shift: {employeeShifts[i]}");

                }

            }


            Console.WriteLine("\nATTENDANCE RECORDS:");
            Console.WriteLine("---------------------");

            if (attendanceId.Count == 0)
            {
                Console.WriteLine("No attendance records found.");
            }
            else
            {
                for (int i = 0; i < attendanceId.Count; i++)
                {
                    string employeeId = attendanceId[i];
                                  

                    Console.WriteLine($"Employee ID: {attendanceId[i]}, Name: {employeeNames[employeeIds.IndexOf(attendanceId[i])]}, Time In: {timeInRecords[i]:hh:mm tt}, Time Out: {timeOutRecords[i]:hh:mm tt}");
                }


            }


        }
    }
}
