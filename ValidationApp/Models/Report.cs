using System;

namespace CreationApp.Models
{
    public class Report
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public virtual User User { get; set; }

        public DateTime Date { get; set; }

        public int Result { get; set; }
    }
}
