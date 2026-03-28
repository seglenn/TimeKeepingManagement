using System;
using System.Collections.Generic;

namespace TimeKeepingManagement_data_service
{
    public interface ITimeKeepingDataService
    {
        List<string> GetEmployeeNames();
        List<string> GetEmployeeIds();
        List<string> GetEmployeeShifts();

        void AddEmployee(string name, string id, string shift);
        void UpdateEmployeeShift(int index, string newShift);
        int FindEmployeeIndex(string employeeId);
        int GetEmployeeCount();

        List<DateTime> GetTimeInRecords();
        List<DateTime> GetTimeOutRecords();
        List<string> GetAttendanceIds();

        void AddAttendance(string employeeId, DateTime timeIn, DateTime timeOut);
        int GetAttendanceCount();
    }
}