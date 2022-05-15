using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Meetings
{
    public enum Categories
    {
        CodeMonkey,
        Hub,
        Short,
        TeamBuilding
    }
    public enum Types
    { 
        Live,
        InPerson
    }

    public class Meeting
    {
        public string Name { get; set; }
        public string ResponsiblePerson { get; set; }
        public string Describtion { get; set; }
        public string Category { get; set; }
        public string Type { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public Meeting(string name, string responsiblePerson, string describtion, string category, string type, DateTime start, DateTime end)
        {
                this.Name = name;
                this.ResponsiblePerson = responsiblePerson;
                this.Describtion = describtion;
                this.Category = category;
                this.Type = type;
                this.StartDate = start;
                this.EndDate = end;
        }
        public Meeting()
        {

        }

        public override string ToString()
        {
            return string.Format("| {0,20} | {1,20} | {2,20} | {3,10} | {4,10} | {5,-22} | {6,-22} |", this.Name, this.ResponsiblePerson,
                this.Describtion, this.Category, this.Type, this.StartDate, this.EndDate);
        }
    }
}
