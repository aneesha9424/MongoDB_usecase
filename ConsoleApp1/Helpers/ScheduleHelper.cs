using ConsoleApp1.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1.Helpers
{
   public static class ScheduleHelper
    {
        public static List<Schedule> CreateSchedules(int count, int ScenarioID)
        {
            List<Schedule> scheduleList = new List<Schedule>();

            for (int i = 0; i < count; i++)
            {
                Schedule schedule = new Schedule
                {
                    Name = "Scenario",
                    Id = Guid.NewGuid(),
                    Origin = "TLO5",
                    DepartureTime = DateTime.Now,
                    Destination = "CD5",
                    ScenarioID = ScenarioID,
                    Movements = CreateMovements()

                };
                scheduleList.Add(schedule);
            }
            return scheduleList;
        }

        public static List<Movement> CreateMovements()
        {
            List<Movement> movementList = new List<Movement>();

            for (int i = 0; i < 500; i++)
            {
                Movement movement = new Movement
                {
                    TrackId = Guid.NewGuid(),
                    TrackName = $"CA01:KFS:KDS-{i}",
                    Time = DateTime.Now,
                    Offset = i,
                    Speed = 45 + i,
                    MovementType = $"movement-{i}",
                    Col1 = "col1",
                    Col2 = "col2",
                    Col3 = "col3",
                    Col4 = "col4",
                    Col5 = "col5"


                };
                movementList.Add(movement);
            }
            return movementList;
        }
    }
}
