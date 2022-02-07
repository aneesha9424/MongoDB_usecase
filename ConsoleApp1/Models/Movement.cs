using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1.Models
{
  public  class Movement
    {    public Guid TrackId { get; set; }
        public string TrackName { get; set; }
        public DateTime Time { get; set; }
        public int Offset { get; set; }
        public int Speed { get; set; }
        public string MovementType  { get; set; }
        public string Col1 { get; set; }
        public string Col2 { get; set; }
        public string Col3 { get; set; }
        public string Col4 { get; set; }
        public string Col5 { get; set; }


    }
}
