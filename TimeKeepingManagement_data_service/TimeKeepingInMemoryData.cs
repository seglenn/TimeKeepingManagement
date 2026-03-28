using System;
using System.Collections.Generic;

namespace TimeKeepingManagement_data_service
{
    public class TimeKeepingInMemoryData : ITimeKeepingDataService
    {
        private List<string> employeeNames = new List<string>();
        private List<string> employeeIds = new List<string>();
        private List<string> employeeShifts = new List<string>();

        private List<DateTime> timeInRecords = new List<DateTime>();
        private List<DateTime> timeOutRecords = new List<DateTime>();
        private List<string> attendanceId = new List<string>();

        public List<string> GetEmployeeNames() => employeeNames;
        public List<string> GetEmployeeIds() => employeeIds;
        public List<string> GetEmployeeShifts() => employeeShifts;

        public void AddEmployee(string name, string id, string shift)
        {
            employeeNames.Add(name);
            employeeIds.Add(id);
            employeeShifts.Add(shift);
        }

        public void UpdateEmployeeShift(int index, string newShift)
        {
            if (index >= 0 && index < employeeShifts.Count)
                employeeShifts[index] = newShift;
        }

        public int FindEmployeeIndex(string employeeId)
        {
            return employeeIds.IndexOf(employeeId);
        }

        public int GetEmployeeCount() => employeeIds.Count;

        public List<DateTime> GetTimeInRecords() => timeInRecords;
        public List<DateTime> GetTimeOutRecords() => timeOutRecords;
        public List<string> GetAttendanceIds() => attendanceId;

        public void AddAttendance(string employeeId, DateTime timeIn, DateTime timeOut)
        {
            attendanceId.Add(employeeId);
            timeInRecords.Add(timeIn);
            timeOutRecords.Add(timeOut);
        }

        public int GetAttendanceCount() => attendanceId.Count;
    }
}