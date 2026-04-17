using System;
using System.Collections.Generic;
using TimeKeepingManagement_app_service;
using TimeKeepingManagement_models;

namespace TimeKeepingManagement
{
    class Program
    {
        static TimeKeepingService _service = new TimeKeepingService();

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
        }

        static bool ShowOptions()
        {
            Console.Write("\nDo you want to continue? (Y/N): ");
            string userInput = Console.ReadLine().Trim();

            switch (userInput.ToLower())
            {
                case "y":
                    return true;
                case "n":
                    Console.WriteLine("\nExiting the program.");
                    return false;
                default:
                    Console.WriteLine("\nInvalid input. Please enter 'Y' or 'N'.");
                    Environment.Exit(0);
                    return false;
            }
        }

        static void EmployeeRegistration()
        {
            string employeeName, employeeId, shift;

            Console.WriteLine("\n=== EMPLOYEE REGISTRATION: === \n");

            Console.Write("Enter your name: ");
            employeeName = Console.ReadLine().Trim();

            Console.Write("Enter your Employee ID number: ");
            employeeId = Console.ReadLine().Trim();

            if (_service.GetEmployeeIds().Contains(employeeId))
            {
                Console.WriteLine("\nEmployee ID already exists. Please enter a unique Employee ID.");
                return;
            }

            Console.WriteLine("\nSELECT YOUR SHIFT:");
            Console.WriteLine("--------------------------------");
            Console.WriteLine("Morning Shift: 9:00 AM - 5:00 PM");
            Console.WriteLine("Night Shift: 11:00 PM - 7:00 AM");
            Console.WriteLine("--------------------------------\n");

            Console.Write("Enter your shift (Morning/Night): ");
            shift = Console.ReadLine().Trim().ToLower();

            bool success = _service.RegisterEmployee(employeeName, employeeId, shift);

            if (success)
            {
                if (shift == "morning")
                {
                    Console.WriteLine($"\nEmployee Name: {employeeName}");
                    Console.WriteLine($"Employee ID: {employeeId}");
                    Console.WriteLine($"Shift: Morning Shift (9:00 AM - 5:00 PM)");
                    Console.WriteLine("\n[== Registration successful. ==]");
                }
                else if (shift == "night")
                {
                    Console.WriteLine($"\nEmployee Name: {employeeName}");
                    Console.WriteLine($"Employee ID: {employeeId}");
                    Console.WriteLine($"Shift: Night Shift (11:00 PM - 7:00 AM)");
                    Console.WriteLine("\n[== Registration successful. ==]");
                }
            }
            else
            {
                Console.WriteLine("\nInvalid shift selection. Please enter 'Morning' or 'Night'.");
                return;
            }
        }

        static void TimeInAndTimeOut()
        {
            Console.WriteLine("\n=== TIME-IN AND TIME-OUT: === \n");

            Console.Write("Enter your Employee ID number: ");
            string employeeId = Console.ReadLine().Trim();

            int employeeIndex = _service.GetEmployeeIds().IndexOf(employeeId);

            if (employeeIndex == -1)
            {
                Console.WriteLine("Employee ID not found. Please register first.");
                return;
            }

            Console.WriteLine($"\nWelcome, {_service.GetEmployeeNames()[employeeIndex]}");
            Console.WriteLine($"Shift: {_service.GetEmployeeShifts()[employeeIndex]}");

            Console.WriteLine("\n--------------------------------");
            Console.WriteLine("          Time Schedule:        ");
            Console.WriteLine("--------------------------------");
            Console.WriteLine("Morning Shift: 9:00 AM - 5:00 PM");
            Console.WriteLine("Night Shift: 11:00 PM - 7:00 AM");
            Console.WriteLine("--------------------------------");

            Console.Write("\nTIME IN (Format: 9:00 AM or 17:45): ");
            string timeIn = Console.ReadLine();
            Console.Write("TIME OUT (Format: 5:00 PM or 17:00): ");
            string timeOut = Console.ReadLine();     

            if (DateTime.TryParse(timeIn, out DateTime employeeTimeIn) && DateTime.TryParse(timeOut, out DateTime employeeTimeOut))
            {           

                bool success = _service.RecordAttendance(employeeId, employeeTimeIn, employeeTimeOut);

                if (success)
                {
                    string duration = _service.WorkDuration(employeeTimeIn, employeeTimeOut);

                    Console.WriteLine($"\nTime in: {employeeTimeIn:hh:mm tt}");
                    Console.WriteLine($"Time Out: {employeeTimeOut:hh:mm tt}");
                    Console.WriteLine($"Total work duration: {duration}");

                    Console.WriteLine("\n[== Time in recorded successfully. ==]");
                }
                else
                {
                    Console.WriteLine("\nPlease enter valid times. Refer to the shift schedule.");
                }
            }
            else
            {
                Console.WriteLine("\nInvalid time format. Please enter a valid time.");
                return;
            }
        }

        static void LateRecords()
        {
            Console.WriteLine("\n=== LATE RECORDS: === \n");

            var lateRecords = _service.GetLateRecords();

            if (lateRecords.Count == 0)
            {
                Console.WriteLine("No late records found.");
                return;
            }

            Console.WriteLine("Late Records:");
            Console.WriteLine("-------------");

            string currentId = "";

            for (int i = 0; i < lateRecords.Count; i++)
            {
                if (lateRecords[i].EmployeeId != currentId)
                {
                    currentId = lateRecords[i].EmployeeId;
                    Console.WriteLine($"\nName: {lateRecords[i].EmployeeName}, ({lateRecords[i].EmployeeId})");
                }

                Console.WriteLine($"    Time In: {lateRecords[i].TimeIn:hh:mm tt} - LATE");
            }

            Console.WriteLine($"\nTotal late records: {lateRecords.Count}");
        }

        static void ShiftSched()
        {
            Console.WriteLine("\n=== SHIFT SCHEDULE: === \n");

            Console.Write("Enter your Employee ID number: ");
            string employeeId = Console.ReadLine();

            int employeeIndex = _service.GetEmployeeIds().IndexOf(employeeId);

            if (employeeIndex == -1)
            {
                Console.WriteLine("\nEmployee ID not found. Please register first.");
                return;
            }

            Console.WriteLine($"\nWelcome, {_service.GetEmployeeNames()[employeeIndex]}!");
            Console.WriteLine($"\nYour Current shift is: {_service.GetEmployeeShifts()[employeeIndex]}");

            Console.WriteLine("\nShift Schedule:");
            Console.WriteLine("--------------------------------");
            Console.WriteLine("Morning Shift: 9:00 AM - 5:00 PM");
            Console.WriteLine("Night Shift: 11:00 PM - 7:00 AM");
            Console.WriteLine("--------------------------------");

            Console.Write("\nEnter your new shift (Morning/Night): ");
            string newShift = Console.ReadLine().Trim().ToLower();

            bool success = _service.UpdateEmployeeShift(employeeId, newShift);

            if (success)
            {
                Console.WriteLine($"\nShift updated successfully to {newShift} shift.");
            }
            else if (_service.GetEmployeeShift(employeeId) == newShift)
            {
                Console.WriteLine($"\nYou are already assigned to the {newShift} shift.");
            }
            else
            {
                Console.WriteLine("\nInvalid shift selection. Please enter 'Morning' or 'Night'.");
            }
        }

        static void ViewAllRecords()
        {
            Console.WriteLine("\n=== ALL ATTENDANCE RECORDS: === \n");

            Console.WriteLine("\nREGISTERED EMPLOYEES:");
            Console.WriteLine("---------------------");

            var employeeIds = _service.GetEmployeeIds();
            var employeeNames = _service.GetEmployeeNames();
            var employeeShifts = _service.GetEmployeeShifts();

            if (employeeIds.Count == 0)
            {
                Console.WriteLine("\nNo registered employees found.");
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

            var records = _service.GetAllAttendanceRecords();

            if (records.Count == 0)
            {
                Console.WriteLine("\nNo attendance records found.");
            }

            else
            {
                string currentId = " ";

                for (int i = 0; i < records.Count; i++)
                {
                    if (records[i].EmployeeId != currentId)
                    {
                        currentId = records[i].EmployeeId;
                        Console.WriteLine($"\nName: {records[i].EmployeeName}, ({records[i].EmployeeId})");
                    }

                    Console.WriteLine($"    Time In: {records[i].TimeIn:hh:mm tt}, Time Out: {records[i].TimeOut:hh:mm tt}");
                }

            }
            
            Console.WriteLine("\nLATE RECORDS:");
            Console.WriteLine("-----------------");

            var lateRecords = _service.GetLateRecords();

            if (lateRecords.Count == 0)
            {
                Console.WriteLine("\nNo late records found.");
            }
            else
            {
                string currentIdLate = " ";

                for (int i = 0; i < lateRecords.Count; i++)
                {
                    if (lateRecords[i].EmployeeId != currentIdLate)
                    {
                        currentIdLate = lateRecords[i].EmployeeId;
                        Console.WriteLine($"\nName: {lateRecords[i].EmployeeName}, ({lateRecords[i].EmployeeId})");
                    }

                    Console.WriteLine($"    Time In: {lateRecords[i].TimeIn:hh:mm tt} - LATE");
                }

            }

            
           
        }
    }

}
