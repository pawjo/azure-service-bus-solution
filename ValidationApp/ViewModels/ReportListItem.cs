using System;

namespace ValidationApp.ViewModels
{
    public class ReportListItem
    {
        public int Id { get; set; }

        public string UserEmail { get; set; }

        public DateTime Date { get; set; }

        public int Result { get; set; }
    }
}
