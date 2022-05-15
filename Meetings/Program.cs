using System;
using System.Globalization;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
namespace Meetings
{
    class Program
    {
        /* Command to create a new meeting
         * Command to delete a meeting
         * Command to add a person to the meeting
         * Command to remove a person from the meeting
         * Command to list all the meetings
         */
        static void Main(string[] args)
        {
            string user = "";
            while (user == "")
            {
                Console.WriteLine("Please enter your Fullname(Format: Firstname Lastname): ");
                user = Console.ReadLine();
            }
            Console.WriteLine("Current user: {0}", user);
            bool abort = false;
            List<Meeting> allMeetings = InOutUtils.ReadMeetings("..\\..\\..\\meetings.json");
            Dictionary<string, List<Meeting>> people = new Dictionary<string, List<Meeting>>();
            List<Meeting> personalMeetings = new List<Meeting>();
            while (!abort)
            {
                List<Meeting> filteredMeetings = new List<Meeting>();
                Console.WriteLine("Choose an option (1-5):");
                Console.WriteLine("1) Create a new meeting");
                Console.WriteLine("2) Delete a meeting");
                Console.WriteLine("3) Add a person to the meeting");
                Console.WriteLine("4) Remove a person from the meeting");
                Console.WriteLine("5) List all meetings");
                int option = int.Parse(Console.ReadLine());
                Console.WriteLine();
                switch (option)
                {
                    case (1):
                        Meeting newMeeting = Commands.createMeeting();
                        if (!allMeetings.Contains(newMeeting))
                        {
                            allMeetings.Add(newMeeting);
                            InOutUtils.WriteMeetings(@"..\\..\\..\\meetings.json", allMeetings);
                        }
                        break;
                    case (2):
                        Console.WriteLine("Name of the meeting you want to delete: ");
                        string toDelete = Console.ReadLine();
                        Commands.deleteMeeting(allMeetings, user, toDelete);
                        InOutUtils.WriteMeetings(@"..\\..\\..\\meetings.json", allMeetings);
                        break;
                    case (3):
                        Console.WriteLine("Name of the person you want to add to the meeting: ");
                        string personToAdd = Console.ReadLine();
                        Console.WriteLine("Enter the name of the meeting you want the person to be added to: ");
                        string addToMeeting = Console.ReadLine();
                        people = Commands.addPerson(people, allMeetings, personToAdd, addToMeeting);
                        break;
                    case (4):
                        Console.WriteLine("Enter the full name of a person you want to remove from a meeting: ");
                        string nameOfPerson = Console.ReadLine();
                        Console.WriteLine("Enter the name of the meeting: ");
                        string meetingName = Console.ReadLine();
                        Commands.deletePerson(nameOfPerson, meetingName, people, allMeetings);
                        break;
                    case (5):
                        Console.WriteLine("All meetings: ");
                        Commands.listAllMeetings(allMeetings);
                        Console.WriteLine();
                        Console.WriteLine("Choose an option (1-6):");
                        Console.WriteLine("1) Filter by describtion");
                        Console.WriteLine("2) Filter by responsible person");
                        Console.WriteLine("3) Filter by category");
                        Console.WriteLine("4) Filter by type");
                        Console.WriteLine("5) Filter by dates");
                        Console.WriteLine("6) Filter by number of attendees");
                        int selection = int.Parse(Console.ReadLine());
                        Console.WriteLine();
                        switch(selection)
                        {
                            case (1):
                                Console.WriteLine("Enter describtion to filter by");
                                string describtion = Console.ReadLine();
                                filteredMeetings = Commands.filterByDescribtion(allMeetings, describtion);
                                Commands.listAllMeetings(filteredMeetings);
                                break;
                            case (2):
                                Console.WriteLine("Enter responsible person to filter by");
                                string respPerson = Console.ReadLine();
                                filteredMeetings = Commands.filterByResponsiblePerson(allMeetings, respPerson);
                                Commands.listAllMeetings(filteredMeetings);
                                break;
                            case (3):
                                Console.WriteLine("Enter category to filter by");
                                string category = Console.ReadLine();
                                filteredMeetings = Commands.filterByCategory(allMeetings, category);
                                Commands.listAllMeetings(filteredMeetings);
                                break;
                            case (4):
                                Console.WriteLine("Enter type to filter by");
                                string type = Console.ReadLine();
                                filteredMeetings = Commands.filterByType(allMeetings, type);
                                Commands.listAllMeetings(filteredMeetings);
                                break;
                            case (5):
                                CultureInfo ltLT = new CultureInfo("lt-LT");
                                Console.WriteLine("Enter 2 dates to filter by (Format: yyyy-MM-dd): ");
                                Console.WriteLine("Enter the first date: ");
                                DateTime date1 = DateTime.ParseExact(Console.ReadLine(), "yyyy-MM-dd", ltLT, DateTimeStyles.None);
                                Console.WriteLine("Enter the second date: ");
                                DateTime date2 = new DateTime();
                                DateTime.TryParseExact(Console.ReadLine(), "yyyy-MM-dd", ltLT, DateTimeStyles.None, out date2);
                                filteredMeetings = Commands.filterByDates(allMeetings, date1, date2);
                                Commands.listAllMeetings(filteredMeetings);
                                break;
                            case (6):
                                Console.WriteLine("Enter number of attendees to filter by");
                                int attendees = int.Parse(Console.ReadLine());
                                filteredMeetings = Commands.filterByAttendees(people,attendees);
                                Commands.listAllMeetings(filteredMeetings);
                                break;
                        }
                        break;

                }
                Console.WriteLine();
                Console.WriteLine("Stop the program?(Y/N) ");
                string answer = Console.ReadLine();
                if (answer == "Y" || answer == "y")
                {
                    abort = true;
                }
                Console.WriteLine();
            }
                InOutUtils.WriteMeetings(@"..\\..\\..\\meetings.json", allMeetings);
            
        }
        
    }
}
