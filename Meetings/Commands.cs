using System;
using System.Globalization;
using System.Collections.Generic;
using System.Text;

namespace Meetings
{
    public class Commands
    {
        /// <summary>
        /// Using user dialogue, create new meeting
        /// </summary>
        /// <returns>New meeting</returns>
        public static Meeting createMeeting()
        {
            DateTime startDate = new DateTime();
            DateTime endDate = new DateTime();

            CultureInfo ltLT = new CultureInfo("lt-LT");
            Console.WriteLine("New meeting creation:");
            Console.WriteLine("\n");
            
            Console.WriteLine("Name of this meeting: ");
            string name = Console.ReadLine();

            Console.WriteLine("Responsible persons name: ");
            string responsiblePerson = Console.ReadLine();

            Console.WriteLine("Describtion of this meet: ");
            string describtion = Console.ReadLine();

            //Ensure category is a fixed value
            string category = "";
            Console.WriteLine("Supported values: CodeMonkey, Hub, Short, TeamBuilding");
            while (InOutUtils.EnsureCategory(category) != true)
            {
                Console.WriteLine("Category of this meeting: ");
                category = Console.ReadLine();
            }

            //Ensure type is a fixed value
            string type = "";
            Console.WriteLine("Supported type values: Live, InPerson");
            while (InOutUtils.EnsureType(type) != true)
            {
                Console.WriteLine("Type of meeting: ");
                type = Console.ReadLine();
            }

            bool endFormat = false;
            bool startFormat = false;
            while (startFormat != true)
            {
                Console.WriteLine("Meeting starts(Format: yyyy-MM-dd HH:mm) ");
                startFormat = DateTime.TryParseExact(Console.ReadLine(), "yyyy-MM-dd HH:mm", ltLT, DateTimeStyles.None, out startDate);
                if (startFormat == false)
                {
                    Console.WriteLine("Error: Date format is invalid");
                }
            }
            
            while (endFormat != true)
            {
                Console.WriteLine("Meeting ends(Format: yyyy-MM-dd HH:mm) ");
                endFormat = DateTime.TryParseExact(Console.ReadLine(), "yyyy-MM-dd HH:mm", ltLT, DateTimeStyles.None, out endDate);
                if(endFormat == false)
                {
                    Console.WriteLine("Error: Date format is invalid");
                }
            }
            
            Meeting newMeeting = new Meeting(name, responsiblePerson, describtion, category, type, startDate, endDate); ;
            Console.WriteLine("Meeting `{0}` has been successfully added", name);
            return newMeeting;
        }

        /// <summary>
        /// Delete meeting from the list of meetings
        /// </summary>
        /// <param name="meetings">All saved meetings</param>
        /// <param name="currentUser">Current logged in user</param>
        /// <param name="toDelete">Name of the meeting to be deleted</param>
        public static void deleteMeeting(List<Meeting> meetings, string currentUser, string toDelete)
        {
            
            for (int i = 0; i < meetings.Count; i++)
            {
                if(meetings[i].Name == toDelete)
                {
                    if (meetings[i].ResponsiblePerson == currentUser)
                    {
                        Console.WriteLine("Removed meeting {0}", meetings[i].Name);
                        meetings.Remove(meetings[i]);
                        return;
                    }
                    else
                    {
                        Console.WriteLine("Only the person responsible for the meeting can delete it!");
                        return;
                    }
                }
            }
            Console.WriteLine("Such meeting does not exist!");
            
        }

        /// <summary>
        /// Add a selected person to the meeting
        /// </summary>
        /// <param name="personDictionary">People and their personal meetings</param>
        /// <param name="meetings">All saved meetings</param>
        /// <param name="person">Person to add to the meeting</param>
        /// <param name="nameOfMeeting"></param>
        /// <returns>Updated dictionary of people and their personal meetings</returns>
        public static Dictionary<string,List<Meeting>> addPerson(Dictionary<string,List<Meeting>> personDictionary, List<Meeting> meetings,
            string person, string nameOfMeeting)
        {
            List<Meeting> personsMeetings = new List<Meeting>();
            //Check if dictionary already has the person as key
            if (!personDictionary.ContainsKey(person))
            {
                personDictionary.Add(person, personsMeetings);
            }
            personDictionary.TryGetValue(person, out personsMeetings);
            Meeting toAdd = InOutUtils.GetMeeting(meetings, nameOfMeeting);
            //Check if the meeting we are trying to add the person to exists at all
            if(toAdd == null)
            {
                Console.WriteLine("Meeting you are trying to add a person to does not exist");
                return personDictionary;
            }
            bool overlap = false;
            string answer;
            if (personsMeetings.Count > 0)
            {
                for (int i = 0; i < personsMeetings.Count; i++)
                {
                    //Iterate through all meetings the person is in, check if he is already in the meeting we are trying to add him to.
                    if (personsMeetings[i].Equals(toAdd))
                    {
                        Console.WriteLine("Person already is in this meeting");
                        return personDictionary;
                    }
                    //If not, check if any of his personal meetings are overlapping with the one we are trying to add him to
                    overlap = InOutUtils.areMeetingsOverlapping(personsMeetings[i], toAdd);
                    if (overlap == true)
                    {
                        //If any of the meetings overlap, ask if person should be added anyways.
                        Console.WriteLine("Meeting you are trying to add `{0}` to intersects with meeting `{1}`", person, personsMeetings[i].Name);
                        Console.WriteLine("Would you like to add the person to the meeting anyways? (Y/N): ");
                        answer = Console.ReadLine();
                        if (answer.Equals("Y") || answer.Equals("y"))
                        {
                            personsMeetings.Add(toAdd);
                            personDictionary[person] = personsMeetings;
                            Console.WriteLine("`{0}` has been added to the meeting `{1}` which starts at: `{2}`", person, toAdd.Name, toAdd.StartDate);
                            return personDictionary;
                        }
                        else
                        {
                            Console.WriteLine("Person has not been added to the meeting");
                            return personDictionary;

                        }
                    }
                }
            }
            //If none of the conditions above apply, add the person to the meeting, update his personal meetings list
            personsMeetings.Add(toAdd);
            personDictionary[person] = personsMeetings;
            Console.WriteLine("`{0}` has been added to the meeting `{1}` which starts at: `{2}`", person, toAdd.Name, toAdd.StartDate);
            
            return personDictionary;
        }
        /// <summary>
        /// Remove person from selected meeting
        /// </summary>
        /// <param name="person">Full name of the person that needs to be removed from the meeting</param>
        /// <param name="meeting">Name of meeting</param>
        /// <param name="personsMeetings">Persons personal meeting list</param>
        /// <param name="allMeetings">All saved meetings</param>
        /// <returns>Dictionary of updated persons personal meetings</returns>
        public static Dictionary<string, List<Meeting>> deletePerson(string person, string meeting, Dictionary<string,List<Meeting>> personsMeetings, List<Meeting> allMeetings)
        {
            List<Meeting> personalMeetings = new List<Meeting>();
            Meeting toRemoveFrom = InOutUtils.GetMeeting(allMeetings, meeting);
            if(person == toRemoveFrom.ResponsiblePerson)
            {
                Console.WriteLine("Person is responsible for this meeting and cannot be removed");
                return personsMeetings;
            }
            personsMeetings.TryGetValue(person, out personalMeetings);
            for (int i = 0; i < personalMeetings.Count; i++)
            {
                if(personalMeetings[i].Equals(toRemoveFrom))
                {
                    personalMeetings.Remove(toRemoveFrom);
                    personsMeetings[person] = personalMeetings;
                    Console.WriteLine("Removed {0} from meeting `{1}`", person, toRemoveFrom.Name);
                    return personsMeetings;
                }
            }
            Console.WriteLine("{0} was not found in the specified meeting", person);
            return personsMeetings;

            

        }
        /// <summary>
        /// Print out all meetings provided in the list
        /// </summary>
        /// <param name="meetings">List of meetings to print</param>
        public static void listAllMeetings(List<Meeting> meetings)
        {
            Console.WriteLine(string.Format("| {0,20} | {1,20} | {2,20} | {3,10} | {4,10} | {5,-22} | {6,-22} |",
                "Name", "Responsible person", "Describtion", "Category", "Type", "Start date", "End date"));
            foreach(Meeting meeting in meetings)
            {
                Console.WriteLine(meeting.ToString());
            }
        }
        /// <summary>
        /// Filter meetings by describtion
        /// </summary>
        /// <param name="allMeetings">All saved meetings</param>
        /// <param name="input">Describtion to filter by</param>
        /// <returns>List of filtered meetings</returns>
        public static List<Meeting> filterByDescribtion(List<Meeting> allMeetings, string input)
        {
            List<Meeting> filtered = new List<Meeting>();
            for (int i = 0; i < allMeetings.Count; i++)
            {
                if(allMeetings[i].Describtion.Contains(input))
                {
                    filtered.Add(allMeetings[i]);
                }
            }
            return filtered;
        }
        /// <summary>
        /// Filter meetings by responsible person
        /// </summary>
        /// <param name="allMeetings">All saved meetings</param>
        /// <param name="input">Responsible person to filter by</param>
        /// <returns>List of filtered meetings</returns>
        public static List<Meeting> filterByResponsiblePerson(List<Meeting> allMeetings, string input)
        {
            List<Meeting> filtered = new List<Meeting>();
            for (int i = 0; i < allMeetings.Count; i++)
            {
                if (allMeetings[i].ResponsiblePerson.Equals(input))
                {
                    filtered.Add(allMeetings[i]);
                }
            }
            return filtered;
        }
        /// <summary>
        /// Filter meetings by category
        /// </summary>
        /// <param name="allMeetings">All saved meetings</param>
        /// <param name="input">Category to filter by</param>
        /// <returns>List of filtered meetings</returns>
        public static List<Meeting> filterByCategory(List<Meeting> allMeetings, string input)
        {
            List<Meeting> filtered = new List<Meeting>();
            for (int i = 0; i < allMeetings.Count; i++)
            {
                if (allMeetings[i].Category.Equals(input))
                {
                    filtered.Add(allMeetings[i]);
                }
            }
            return filtered;
        }
        /// <summary>
        /// Filter meetings by Type
        /// </summary>
        /// <param name="allMeetings">All saved meetings</param>
        /// <param name="input">Type to filter by</param>
        /// <returns>List of filtered meetings</returns>
        public static List<Meeting> filterByType(List<Meeting> allMeetings, string input)
        {
            List<Meeting> filtered = new List<Meeting>();
            for (int i = 0; i < allMeetings.Count; i++)
            {
                if (allMeetings[i].Type.Equals(input))
                {
                    filtered.Add(allMeetings[i]);
                }
            }
            return filtered;
        }
        /// <summary>
        /// Filter meetings by dates
        /// </summary>
        /// <param name="allMeetings">All saved meetings</param>
        /// <param name="date1">First date (starting from)</param>
        /// <param name="date2">Second date (until)</param>
        /// <returns>List of filtered meetings</returns>
        public static List<Meeting> filterByDates(List<Meeting> allMeetings, DateTime date1, DateTime date2)
        {
            List<Meeting> filtered = new List<Meeting>();
            for (int i = 0; i < allMeetings.Count; i++)
            {
                if (date2.ToString() == "0001-01-01 00:00:00") // Default unassigned datetime value
                {
                    if (allMeetings[i].StartDate.CompareTo(date1) > 0)
                    {
                        filtered.Add(allMeetings[i]);
                    }
                }
                else
                {
                    if (allMeetings[i].StartDate.CompareTo(date1) > 0 && allMeetings[i].StartDate.CompareTo(date2) < 0)
                    {
                        filtered.Add(allMeetings[i]);
                    }
                }
            }
            return filtered;
        }
        /// <summary>
        /// Filter by specified number of attendees
        /// </summary>
        /// <param name="people">Peoples personal meetings list</param>
        /// <param name="attendees">Number of attendees to filter by</param>
        /// <returns>List of filtered meetings</returns>
        public static List<Meeting> filterByAttendees(Dictionary<string,List<Meeting>> people, int attendees)
        {
            List<Meeting> filtered = new List<Meeting>();
            Dictionary<Meeting, int> occurances = new Dictionary<Meeting, int>();
            foreach (var meetings in people)
            {
                for (int i = 0; i < meetings.Value.Count; i++)
                {
                    if (!occurances.ContainsKey(meetings.Value[i]))
                    {
                        occurances.Add(meetings.Value[i], 0);
                        occurances[meetings.Value[i]]++;
                    }
                    else
                    {
                        occurances[meetings.Value[i]]++; 
                    }
                }
            }
            foreach (var item in occurances)
            {
                if(item.Value >= attendees)
                {
                    filtered.Add(item.Key);
                }
            }
            
            return filtered;
        }


    }
}
