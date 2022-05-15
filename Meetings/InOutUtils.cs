using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;

namespace Meetings
{
    public class InOutUtils
    {
        /// <summary>
        /// Read all meetings from JSON file
        /// </summary>
        /// <param name="fileName">JSON file name</param>
        /// <returns>List of meetings</returns>
        public static List<Meeting> ReadMeetings(string fileName)
        {
            List<Meeting> meetings = new List<Meeting>();
            string jsonData = File.ReadAllText(fileName);
            meetings = JsonConvert.DeserializeObject<List<Meeting>>(jsonData);

            return meetings;
        }
        /// <summary>
        /// Write 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="meeting"></param>
        public static void WriteMeetings(string fileName, List<Meeting> meeting)
        {
            string json = JsonConvert.SerializeObject(meeting);
            File.WriteAllText(fileName, json);
        }

        /// <summary>
        /// Get meeting by its name
        /// </summary>
        /// <param name="meetings">All saved meetings</param>
        /// <param name="name">Name of the meeting to be selected</param>
        /// <returns>Found meeting by the name</returns>
        public static Meeting GetMeeting(List<Meeting> meetings, string name)
        {
            for (int i = 0; i < meetings.Count; i++)
            {
                if(meetings[i].Name == name)
                {
                    return meetings[i];
                }
            }
            return null;
        }
        /// <summary>
        /// Check if meetings are overlapping with eachother
        /// </summary>
        /// <param name="first">First meeting</param>
        /// <param name="second">Second meeting</param>
        /// <returns>Overlaps = true / false</returns>
        public static bool areMeetingsOverlapping(Meeting first, Meeting second)
        {
            return first.StartDate < second.EndDate && second.StartDate < first.EndDate;
        }
        /// <summary>
        /// Ensure input category is one of the fixed enum values: CodeMonkey / Hub / Short / TeamBuilding
        /// </summary>
        /// <param name="category">Category input made by user</param>
        /// <returns>Input exists in fixed enum values? true/false</returns>
        public static bool EnsureCategory(string category)
        {
            foreach(string key in Enum.GetNames(typeof(Categories)))
            {
                if(category == key)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Ensure input type is one of the fixed enum values: Live / InPerson
        /// </summary>
        /// <param name="type">Type input made by user</param>
        /// <returns>Input ecists in fixed enum values? true/false</returns>
        public static bool EnsureType(string type)
        {
            foreach (string key in Enum.GetNames(typeof(Types)))
            {
                if (type == key)
                {
                    return true;
                }
            }
            return false;
        }

    }
}
