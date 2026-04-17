using System;
using System.Collections.Generic;
using TimeKeepingManagement_data_service;
using TimeKeepingManagement_models;


namespace TimeKeepingManagement_app_service
{
    public class TimeKeepingService
    {
        TimeKeepingDataService _dataService = new TimeKeepingDataService(new TimeDBData());

        public bool RegisterEmployee(string employeeName, string employeeId, string shift)
        {
            foreach (string id in _dataService.GetEmployeeIds())
            {
                if (id == employeeId)
                {
                    return false;
                }
            }

            if (shift != "morning" && shift != "night")
            {
                return false;
            }

            _dataService.AddEmployee(employeeName, employeeId, shift);
            return true;
        }

        public bool ValidateTimeShift(DateTime timeIn, DateTime timeOut, string shift)
        {
            TimeSpan morningStart = new TimeSpan(9, 0, 0);   // 9:00 AM
            TimeSpan morningEnd = new TimeSpan(17, 0, 0);    // 5:00 PM
            TimeSpan nightStart = new TimeSpan(23, 0, 0);    // 11:00 PM
            TimeSpan nightEnd = new TimeSpan(7, 0, 0);       // 7:00 AM

            if (shift == "morning")
            {
                // Allow time-in from 9:00 AM up to (but not at) 5:00 PM
                if (timeIn.TimeOfDay < morningStart || timeIn.TimeOfDay >= morningEnd)
                {
                    return false;
                }

                // Time-out must not exceed 5:00 PM
                if (timeOut.TimeOfDay > morningEnd)
                {
                    return false;
                }

                // Time-out must be after time-in
                if (timeOut <= timeIn)
                {
                    return false;
                }
            }
            else if (shift == "night")
            {
                bool validTimeIn = timeIn.TimeOfDay >= nightStart || timeIn.TimeOfDay < nightEnd;

                bool validTimeOut = timeOut.TimeOfDay <= nightEnd || timeOut.TimeOfDay >= nightStart;

                if (!validTimeIn)
                {
                    return false;
                }

                if (!validTimeOut)
                {
                    return false;
                }

                DateTime adjustedTimeOut = timeOut;
                if (timeIn.TimeOfDay >= nightStart && timeOut.TimeOfDay <= nightEnd)
                {
                    adjustedTimeOut = timeOut.AddDays(1);
                }

                if (adjustedTimeOut <= timeIn)
                {
                    return false;
                }
            }
            else
            {
                return false; 
            }

            return true;
        }
        

        public string WorkDuration(DateTime timeIn, DateTime timeOut)
        {
            TimeSpan duration = timeOut - timeIn;
            
            if (duration < TimeSpan.Zero)
            {
                duration = duration.Add(TimeSpan.FromHours(24));
            }

            return $"{duration.Hours} hours and {duration.Minutes} minutes";
        }

        public List<string> GetEmployeeNames()
        {
            return _dataService.GetEmployeeNames();
        }

        public List<string> GetEmployeeIds()
        {
            return _dataService.GetEmployeeIds();
        }

        public List<string> GetEmployeeShifts()
        {
            return _dataService.GetEmployeeShifts();
        }

        public bool RecordAttendance(string employeeId, DateTime timeIn, DateTime timeOut)
        {
            int employeeIndex = _dataService.FindEmployeeIndex(employeeId);

            if (employeeIndex == -1)
            {
                return false;
            }

            string shift = _dataService.GetEmployeeShifts()[employeeIndex];

            if (!ValidateTimeShift(timeIn, timeOut, shift))
            {
                return false;
            }

            _dataService.AddAttendance(employeeId, timeIn, timeOut);
            return true;
        }

        public string GetEmployeeName(string employeeId)
        {
            int index = _dataService.FindEmployeeIndex(employeeId);
            if (index != -1)
                return _dataService.GetEmployeeNames()[index];
            return null;
        }

        public string GetEmployeeShift(string employeeId)
        {
            int index = _dataService.FindEmployeeIndex(employeeId);
            
            if (index != -1)
                return _dataService.GetEmployeeShifts()[index];
            return null;
        }

        public bool UpdateEmployeeShift(string employeeId, string newShift)
        {
            int employeeIndex = _dataService.FindEmployeeIndex(employeeId);

            if (employeeIndex == -1)
            {
                return false;
            }

            if (newShift != "morning" && newShift != "night")
            {
                return false;
            }

            string currentShift = _dataService.GetEmployeeShifts()[employeeIndex];

            if (currentShift == newShift)
            {
                return false;
            }

            _dataService.UpdateEmployeeShift(employeeIndex, newShift);
            return true;
        }

        public List<Attendance> GetLateRecords()
        {
            var lateRecords = new List<Attendance>();

            var employeeNames = _dataService.GetEmployeeNames();
            var employeeShifts = _dataService.GetEmployeeShifts();

            int attendanceCount = _dataService.GetAttendanceCount();

            for (int i = 0; i < attendanceCount; i++)
            {
                string employeeId = _dataService.GetAttendanceIds()[i];
                DateTime timeIn = _dataService.GetTimeInRecords()[i];
                DateTime timeOut = _dataService.GetTimeOutRecords()[i];

                int employeeIndex = _dataService.FindEmployeeIndex(employeeId);
                string shift = employeeShifts[employeeIndex].Trim();
                string employeeName = employeeNames[employeeIndex].Trim();

                int timeInHour = timeIn.Hour;
                int timeInMinute = timeIn.Minute;

                bool isLate = false;

                if (shift == "morning")
                {
                    if (timeInHour > 9 || (timeInHour == 9 && timeInMinute > 15))
                    {
                        isLate = true;
                    }
                }
                else if (shift == "night")
                {
                    if (timeInHour == 23 && timeInMinute > 15)
                    {
                        isLate = true;
                    }
                }

                if (isLate)
                {
                    lateRecords.Add(new Attendance
                    {
                        EmployeeId = employeeId,
                        EmployeeName = employeeName,
                        Shift = shift,
                        TimeIn = timeIn,
                        TimeOut = timeOut
                    });
                }
            }

            return lateRecords;
        }

        public List<Attendance> GetAllAttendanceRecords()
        {
            var records = new List<Attendance>();
            var employeeIds = _dataService.GetEmployeeIds();
            var employeeNames = _dataService.GetEmployeeNames();
            var employeeShifts = _dataService.GetEmployeeShifts();

            for (int i = 0; i < _dataService.GetAttendanceCount(); i++)
            {
                string employeeId = _dataService.GetAttendanceIds()[i];
                int employeeIndex = _dataService.FindEmployeeIndex(employeeId);

                records.Add(new Attendance
                {
                    EmployeeId = employeeId,
                    EmployeeName = employeeIndex != -1 ? employeeNames[employeeIndex] : "Unknown",
                    Shift = employeeIndex != -1 ? employeeShifts[employeeIndex] : "Unknown",
                    TimeIn = _dataService.GetTimeInRecords()[i],
                    TimeOut = _dataService.GetTimeOutRecords()[i]
                });
            }

            return records;
        }

        public bool HasAttendanceRecords()
        {
            return _dataService.GetAttendanceCount() > 0;
        }

        public int GetAttendanceCount() => _dataService.GetAttendanceCount();
    }

}