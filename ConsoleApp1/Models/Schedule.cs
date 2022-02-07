using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1.Models
{
    public class Schedule
    {

        public string Name { get; set; }
        public Guid Id { get; set; }
        public DateTime DepartureTime { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public int ScenarioID { get; set; }

        public List<Movement> Movements { get; set; }

    }
}
