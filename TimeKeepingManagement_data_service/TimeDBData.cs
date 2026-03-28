using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using TimeKeepingManagement_models;

namespace TimeKeepingManagement_data_service
{
    public class TimeDBData : ITimeKeepingDataService
    {
        private string connectionString = "Data Source=localhost\\SQLEXPRESS; Initial Catalog=TimeKpngMngmnt; Integrated Security=True; TrustServerCertificate=True;";
        private SqlConnection sqlConnection;

        public TimeDBData()
        {
            sqlConnection = new SqlConnection(connectionString);
            CreateTablesIfNotExist();
            AddSeeds();
        }

        private void CreateTablesIfNotExist()
        {
            sqlConnection.Open();

            string createEmployeesTable = @"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Employees' AND xtype='U')
                CREATE TABLE Employees (
                    Id INT IDENTITY(1,1) PRIMARY KEY,
                    EmployeeId NVARCHAR(50) NOT NULL UNIQUE,
                    Name NVARCHAR(100) NOT NULL,
                    Shift NVARCHAR(20) NOT NULL
                )";

            string createAttendanceTable = @"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Attendance' AND xtype='U')
                CREATE TABLE Attendance (
                    Id INT IDENTITY(1,1) PRIMARY KEY,
                    EmployeeId NVARCHAR(50) NOT NULL,
                    TimeIn DATETIME NOT NULL,
                    TimeOut DATETIME NOT NULL,
                    FOREIGN KEY (EmployeeId) REFERENCES Employees(EmployeeId)
                )";

            using (var cmd = new SqlCommand(createEmployeesTable, sqlConnection))
                cmd.ExecuteNonQuery();

            using (var cmd = new SqlCommand(createAttendanceTable, sqlConnection))
                cmd.ExecuteNonQuery();

            sqlConnection.Close();
        }

        private void AddSeeds()
        {
            var existing = GetEmployeeIds();

            if (existing.Count == 0)
            {
                AddEmployee("Glenn", "001", "morning");
                AddEmployee("Lambert", "002", "night");
                AddEmployee("Cordial", "003", "morning");

                AddAttendance("001", DateTime.Now.AddHours(-8), DateTime.Now);
                AddAttendance("002", DateTime.Now.AddHours(-7), DateTime.Now);
            }
        }

        public List<string> GetEmployeeNames()
        {
            string selectStatement = "SELECT Name FROM Employees";
            SqlCommand selectCommand = new SqlCommand(selectStatement, sqlConnection);

            sqlConnection.Open();
            SqlDataReader reader = selectCommand.ExecuteReader();

            var names = new List<string>();

            while (reader.Read())
            {
                names.Add(reader["Name"].ToString());
            }

            sqlConnection.Close();
            return names;
        }

        public List<string> GetEmployeeIds()
        {
            string selectStatement = "SELECT EmployeeId FROM Employees";
            SqlCommand selectCommand = new SqlCommand(selectStatement, sqlConnection);

            sqlConnection.Open();
            SqlDataReader reader = selectCommand.ExecuteReader();

            var ids = new List<string>();

            while (reader.Read())
            {
                ids.Add(reader["EmployeeId"].ToString());
            }

            sqlConnection.Close();
            return ids;
        }

        public List<string> GetEmployeeShifts()
        {
            string selectStatement = "SELECT Shift FROM Employees";
            SqlCommand selectCommand = new SqlCommand(selectStatement, sqlConnection);

            sqlConnection.Open();
            SqlDataReader reader = selectCommand.ExecuteReader();

            var shifts = new List<string>();

            while (reader.Read())
            {
                shifts.Add(reader["Shift"].ToString());
            }

            sqlConnection.Close();
            return shifts;
        }

        public void AddEmployee(string name, string id, string shift)
        {
            var insertStatement = "INSERT INTO Employees (EmployeeId, Name, Shift) VALUES (@EmployeeId, @Name, @Shift)";

            SqlCommand insertCommand = new SqlCommand(insertStatement, sqlConnection);

            insertCommand.Parameters.AddWithValue("@EmployeeId", id);
            insertCommand.Parameters.AddWithValue("@Name", name);
            insertCommand.Parameters.AddWithValue("@Shift", shift);

            sqlConnection.Open();
            insertCommand.ExecuteNonQuery();
            sqlConnection.Close();
        }

        public void UpdateEmployeeShift(int index, string newShift)
        {
            var employeeIds = GetEmployeeIds();

            if (index >= 0 && index < employeeIds.Count)
            {
                string employeeId = employeeIds[index];
                var updateStatement = "UPDATE Employees SET Shift = @Shift WHERE EmployeeId = @EmployeeId";

                SqlCommand updateCommand = new SqlCommand(updateStatement, sqlConnection);

                updateCommand.Parameters.AddWithValue("@Shift", newShift);
                updateCommand.Parameters.AddWithValue("@EmployeeId", employeeId);

                sqlConnection.Open();
                updateCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }
        }

        public int FindEmployeeIndex(string employeeId)
        {
            var ids = GetEmployeeIds();

            for (int i = 0; i < ids.Count; i++)
            {
                if (ids[i] == employeeId)
                    return i;
            }
            return -1;
        }

        public int GetEmployeeCount()
        {
            string selectStatement = "SELECT COUNT(*) FROM Employees";
            SqlCommand selectCommand = new SqlCommand(selectStatement, sqlConnection);

            sqlConnection.Open();
            int count = (int)selectCommand.ExecuteScalar();
            sqlConnection.Close();

            return count;
        }

        public List<DateTime> GetTimeInRecords()
        {
            string selectStatement = "SELECT TimeIn FROM Attendance ORDER BY Id";
            SqlCommand selectCommand = new SqlCommand(selectStatement, sqlConnection);

            sqlConnection.Open();
            SqlDataReader reader = selectCommand.ExecuteReader();

            var times = new List<DateTime>();

            while (reader.Read())
            {
                times.Add(Convert.ToDateTime(reader["TimeIn"]));
            }

            sqlConnection.Close();
            return times;
        }

        public List<DateTime> GetTimeOutRecords()
        {
            string selectStatement = "SELECT TimeOut FROM Attendance ORDER BY Id";
            SqlCommand selectCommand = new SqlCommand(selectStatement, sqlConnection);

            sqlConnection.Open();
            SqlDataReader reader = selectCommand.ExecuteReader();

            var times = new List<DateTime>();

            while (reader.Read())
            {
                times.Add(Convert.ToDateTime(reader["TimeOut"]));
            }

            sqlConnection.Close();
            return times;
        }

        public List<string> GetAttendanceIds()
        {
            string selectStatement = "SELECT EmployeeId FROM Attendance ORDER BY Id";
            SqlCommand selectCommand = new SqlCommand(selectStatement, sqlConnection);

            sqlConnection.Open();
            SqlDataReader reader = selectCommand.ExecuteReader();

            var ids = new List<string>();

            while (reader.Read())
            {
                ids.Add(reader["EmployeeId"].ToString());
            }

            sqlConnection.Close();
            return ids;
        }

        public void AddAttendance(string employeeId, DateTime timeIn, DateTime timeOut)
        {
            var insertStatement = "INSERT INTO Attendance (EmployeeId, TimeIn, TimeOut) VALUES (@EmployeeId, @TimeIn, @TimeOut)";

            SqlCommand insertCommand = new SqlCommand(insertStatement, sqlConnection);

            insertCommand.Parameters.AddWithValue("@EmployeeId", employeeId);
            insertCommand.Parameters.AddWithValue("@TimeIn", timeIn);
            insertCommand.Parameters.AddWithValue("@TimeOut", timeOut);

            sqlConnection.Open();
            insertCommand.ExecuteNonQuery();
            sqlConnection.Close();
        }

        public int GetAttendanceCount()
        {
            string selectStatement = "SELECT COUNT(*) FROM Attendance";
            SqlCommand selectCommand = new SqlCommand(selectStatement, sqlConnection);

            sqlConnection.Open();
            int count = (int)selectCommand.ExecuteScalar();
            sqlConnection.Close();

            return count;
        }
    }
}