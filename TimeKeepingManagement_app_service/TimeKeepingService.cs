using TimeKeepingManagement_data_service;
using System;
using System.Collections.Generic;

namespace TimeKeepingManagement_app_service
{
    public class TimeKeepingService
    {
        private TimeKeepingDataService _dataService;

        public TimeKeepingService(ITimeKeepingDataService dataService)
        {
            _dataService = new TimeKeepingDataService(dataService);
        }

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

        public List<string> GetEmployeeNames() => _dataService.GetEmployeeNames();
        public List<string> GetEmployeeIds() => _dataService.GetEmployeeIds();
        public List<string> GetEmployeeShifts() => _dataService.GetEmployeeShifts();

        public bool RecordAttendance(string employeeId, DateTime timeIn, DateTime timeOut)
        {
            int employeeIndex = _dataService.FindEmployeeIndex(employeeId);

            if (employeeIndex == -1)
            {
                return false;
            }

            if (timeOut <= timeIn)
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

        public List<AttendanceInfo> GetLateRecords()
        {
            var lateRecords = new List<AttendanceInfo>();

            TimeSpan morningGraceEnd = new TimeSpan(9, 15, 0);
            TimeSpan nightGraceEnd = new TimeSpan(23, 15, 0);

            for (int i = 0; i < _dataService.GetAttendanceCount(); i++)
            {
                string employeeId = _dataService.GetAttendanceIds()[i];
                DateTime timeIn = _dataService.GetTimeInRecords()[i];
                DateTime timeOut = _dataService.GetTimeOutRecords()[i];

                int employeeIndex = _dataService.FindEmployeeIndex(employeeId);
                string shift = _dataService.GetEmployeeShifts()[employeeIndex];
                string employeeName = _dataService.GetEmployeeNames()[employeeIndex];

                if (shift == "morning")
                {
                    if (timeIn.TimeOfDay > morningGraceEnd)
                    {
                        lateRecords.Add(new AttendanceInfo
                        {
                            EmployeeId = employeeId,
                            EmployeeName = employeeName,
                            Shift = shift,
                            TimeIn = timeIn,
                            TimeOut = timeOut
                        });
                    }
                }
                else if (shift == "night")
                {
                    if (timeIn.TimeOfDay > nightGraceEnd)
                    {
                        lateRecords.Add(new AttendanceInfo
                        {
                            EmployeeId = employeeId,
                            EmployeeName = employeeName,
                            Shift = shift,
                            TimeIn = timeIn,
                            TimeOut = timeOut
                        });
                    }
                }
            }

            return lateRecords;
        }

        public List<AttendanceInfo> GetAllAttendanceRecords()
        {
            var records = new List<AttendanceInfo>();

            for (int i = 0; i < _dataService.GetAttendanceCount(); i++)
            {
                string employeeId = _dataService.GetAttendanceIds()[i];
                int employeeIndex = _dataService.FindEmployeeIndex(employeeId);

                records.Add(new AttendanceInfo
                {
                    EmployeeId = employeeId,
                    EmployeeName = _dataService.GetEmployeeNames()[employeeIndex],
                    Shift = _dataService.GetEmployeeShifts()[employeeIndex],
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

    public class AttendanceInfo
    {
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string Shift { get; set; }
        public DateTime TimeIn { get; set; }
        public DateTime TimeOut { get; set; }
    }
}