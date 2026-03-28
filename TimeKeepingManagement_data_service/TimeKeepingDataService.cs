using System;
using System.Collections.Generic;

namespace TimeKeepingManagement_data_service
{
    public class TimeKeepingDataService
    {
        private ITimeKeepingDataService _dataService;

        public TimeKeepingDataService(ITimeKeepingDataService dataService)
        {
            _dataService = dataService;
        }

        public List<string> GetEmployeeNames() => _dataService.GetEmployeeNames();
        public List<string> GetEmployeeIds() => _dataService.GetEmployeeIds();
        public List<string> GetEmployeeShifts() => _dataService.GetEmployeeShifts();

        public void AddEmployee(string name, string id, string shift)
        {
            _dataService.AddEmployee(name, id, shift);
        }

        public void UpdateEmployeeShift(int index, string newShift)
        {
            _dataService.UpdateEmployeeShift(index, newShift);
        }

        public int FindEmployeeIndex(string employeeId)
        {
            return _dataService.FindEmployeeIndex(employeeId);
        }

        public int GetEmployeeCount() => _dataService.GetEmployeeCount();

        public List<DateTime> GetTimeInRecords() => _dataService.GetTimeInRecords();
        public List<DateTime> GetTimeOutRecords() => _dataService.GetTimeOutRecords();
        public List<string> GetAttendanceIds() => _dataService.GetAttendanceIds();

        public void AddAttendance(string employeeId, DateTime timeIn, DateTime timeOut)
        {
            _dataService.AddAttendance(employeeId, timeIn, timeOut);
        }

        public int GetAttendanceCount() => _dataService.GetAttendanceCount();
    }
}