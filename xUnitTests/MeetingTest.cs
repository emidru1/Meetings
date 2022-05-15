using System;
using Xunit;
using Meetings;

namespace xUnitTests
{
    public class MeetingTest
    {
        [Fact]
        public void Ensure_Category()
        {
            string category = "";
            string category1 = "CodeMonkey";
            string category2 = "Hub";
            string category3 = "Short";
            string category4 = "TeamBuilding";
            string category5 = "InvalidCategory";

            bool cat1 = InOutUtils.EnsureCategory(category);
            bool cat2 = InOutUtils.EnsureCategory(category1);
            bool cat3 = InOutUtils.EnsureCategory(category2);
            bool cat4 = InOutUtils.EnsureCategory(category3);
            bool cat5 = InOutUtils.EnsureCategory(category4);
            bool cat6 = InOutUtils.EnsureCategory(category5);

            Assert.False(cat1);
            Assert.True(cat2);
            Assert.True(cat3);
            Assert.True(cat4);
            Assert.True(cat5);
            Assert.False(cat6);
        }

        [Fact]
        public void Ensure_Type()
        {
            string type = "";
            string type1 = "Live";
            string type2 = "InPerson";
            string type3 = "InvalidType";

            bool typ1 = InOutUtils.EnsureType(type);
            bool typ2 = InOutUtils.EnsureType(type1);
            bool typ3 = InOutUtils.EnsureType(type2);
            bool typ4 = InOutUtils.EnsureType(type3);


            Assert.False(typ1);
            Assert.True(typ2);
            Assert.True(typ3);
            Assert.False(typ4);
        }

        [Fact]
        public void Date_Overlap_Test()
        {
            var con1 = new Meeting("Meeting1", "Responsible Person", "Meeting for unit test", "CodeMonkey", "Live", new DateTime(2022,05,15,15,00,00), new DateTime(2022, 05, 15, 16, 00, 00));
            var con2 = new Meeting("Meeting with missing inputs", "", "", "CodeMonkey", "Live", new DateTime(2022, 05, 15, 15, 10, 00), new DateTime(2022, 05, 15, 16, 10, 00));
            var con3 = new Meeting("Meeting", "Responsible Person", "Meeting for unit test", "CodeMonkey", "Live", new DateTime(2022, 05, 15, 17, 00, 00), new DateTime(2022, 05, 15, 18, 00, 00));

            Assert.True(InOutUtils.areMeetingsOverlapping(con1, con2));
            Assert.False(InOutUtils.areMeetingsOverlapping(con2, con3));
            Assert.False(InOutUtils.areMeetingsOverlapping(con1, con3));
            
            


        }
    }
}
