using System;

namespace SmartMonitoring.MemoryDatabase.Entities
{
    public class Label
    {
        public Label(string value)
        {
            Value = value;
        }

        public Guid Id { get; set; }
        public Guid ServiceId { get; set; }
        public string Value { get; set; }
    }
}
