using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using TimeKeepingManagement_models;

namespace TimeKeepingManagement_data_service
{
    public class TimeKeepingJsonData : ITimeKeepingDataService
    {
        private List<Employee> employees = new List<Employee>();
        private List<Attendance> attendances = new List<Attendance>();

        private string _jsonFileName;

        public TimeKeepingJsonData()
        {
            _jsonFileName = $"{AppDomain.CurrentDomain.BaseDirectory}/TimeKeepingData.json";
            RetrieveDataFromJsonFile();
        }

        private void SaveDataToJsonFile()
        {
            var data = new DataContainer
            {
                Employees = employees,
                Attendances = attendances
            };

            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            string jsonString = JsonSerializer.Serialize(data, options);
            File.WriteAllText(_jsonFileName, jsonString);
        }

        private void RetrieveDataFromJsonFile()
        {
            if (!File.Exists(_jsonFileName))
            {
                employees = new List<Employee>();
                attendances = new List<Attendance>();
                return;
            }

            string jsonContent = File.ReadAllText(_jsonFileName);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var data = JsonSerializer.Deserialize<DataContainer>(jsonContent, options);

            if (data != null)
            {
                employees = data.Employees ?? new List<Employee>();
                attendances = data.Attendances ?? new List<Attendance>();
            }
            else
            {
                employees = new List<Employee>();
                attendances = new List<Attendance>();
            }
        }

        public List<string> GetEmployeeNames()
        {
            return employees.Select(e => e.Name).ToList();
        }

        public List<string> GetEmployeeIds()
        {
            return employees.Select(e => e.EmployeeId).ToList();
        }

        public List<string> GetEmployeeShifts()
        {
            return employees.Select(e => e.Shift).ToList();
        }

        public void AddEmployee(string name, string id, string shift)
        {
            employees.Add(new Employee
            {
                Name = name,
                EmployeeId = id,
                Shift = shift
            });
            SaveDataToJsonFile();
        }

        public void UpdateEmployeeShift(int index, string newShift)
        {
            if (index >= 0 && index < employees.Count)
            {
                employees[index].Shift = newShift;
                SaveDataToJsonFile();
            }
        }

        public int FindEmployeeIndex(string employeeId)
        {
            for (int i = 0; i < employees.Count; i++)
            {
                if (employees[i].EmployeeId == employeeId)
                    return i;
            }
            return -1;
        }

        public int GetEmployeeCount()
        {
            return employees.Count;
        }

        public List<DateTime> GetTimeInRecords()
        {
            return attendances.Select(a => a.TimeIn).ToList();
        }

        public List<DateTime> GetTimeOutRecords()
        {
            return attendances.Select(a => a.TimeOut).ToList();
        }

        public List<string> GetAttendanceIds()
        {
            return attendances.Select(a => a.EmployeeId).ToList();
        }

        public void AddAttendance(string employeeId, DateTime timeIn, DateTime timeOut)
        {
            attendances.Add(new Attendance
            {
                EmployeeId = employeeId,
                TimeIn = timeIn,
                TimeOut = timeOut
            });
            SaveDataToJsonFile();
        }

        public int GetAttendanceCount()
        {
            return attendances.Count;
        }

        private class DataContainer
        {
            public List<Employee> Employees { get; set; } = new List<Employee>();
            public List<Attendance> Attendances { get; set; } = new List<Attendance>();
        }
    }
}