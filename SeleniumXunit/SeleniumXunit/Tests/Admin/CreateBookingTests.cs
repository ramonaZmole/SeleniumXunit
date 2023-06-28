using SeleniumXunit.Helpers;
using SeleniumXunit.Helpers.Models;
using SeleniumXunit.Helpers.Models.ApiModels;
using SeleniumXunit.Helpers.Models.Enum;
using Room = SeleniumXunit.Helpers.Models.Room;

namespace SeleniumXunit.Tests.Admin
{
    public class CreateBookingTests : IClassFixture<BaseTest>, IDisposable
    {
        private readonly CreateRoomOutput _createRoomOutput;
        private readonly User _user = new();
        private readonly Room _room;
        private readonly BaseTest _baseTest;

        public CreateBookingTests(BaseTest baseTest)
        {
            _baseTest = baseTest;
            _createRoomOutput = _baseTest.Client.CreateRoom();
            _room = new Room
            {
                RoomName = _createRoomOutput.roomName.ToString()
            };
        }

        [Fact]
        public void WhenBookingARoom_BookingShouldBeDisplayedTest()
        {
            Browser.GoTo(Constants.AdminUrl);

            Pages.LoginPage.Login();
            Pages.AdminHeaderPage.GoToMenu(Menu.Report);

            Pages.ReportPage.SelectDates();
            Pages.ReportPage.Book();
            Pages.ReportPage.IsErrorMessageDisplayed().Should().BeTrue();

            Pages.ReportPage.InsertBookingDetails(_user, _room);
            Pages.ReportPage.Book();

            var bookingName = $"{_user.FirstName} {_user.LastName}";
            Pages.ReportPage.IsBookingDisplayed(bookingName, _createRoomOutput.roomName).Should().BeTrue();
        }

        public void Dispose()
        {
            _baseTest.Client.DeleteRoom(_createRoomOutput.roomid);
        }
    }
}
