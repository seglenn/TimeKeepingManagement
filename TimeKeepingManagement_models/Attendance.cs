using System;

namespace TimeKeepingManagement_models
{  
        public class Attendance
        {
            public string EmployeeId { get; set; }
            public DateTime TimeIn { get; set; }
            public DateTime TimeOut { get; set; }
            public string EmployeeName { get; set; } 
            public string Shift { get; set; }
    }

}
