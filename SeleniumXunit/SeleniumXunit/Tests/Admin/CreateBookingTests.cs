using AngleSharp.Dom;
using FluentAssertions;
using NsTestFrameworkApi.RestSharp;
using NsTestFrameworkUI.Helpers;
using RestSharp;
using SeleniumXunit.Helpers;
using SeleniumXunit.Helpers.Models;
using SeleniumXunit.Helpers.Models.ApiModels;
using Room = SeleniumXunit.Helpers.Models.Room;

namespace SeleniumXunit.Tests.Admin
{
    public class CreateBookingTests : IClassFixture<BaseTest>, IDisposable
    {
        private CreateRoomOutput _createRoomOutput;
        private User user = new();
        private Room room;
        private BaseTest _baseTest;

        public CreateBookingTests(BaseTest baseTest)
        {
            _baseTest = baseTest;
            _createRoomOutput = _baseTest.Client.CreateRoom();
            room = new Room
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

            Pages.ReportPage.InsertBookingDetails(user, room);
            Pages.ReportPage.Book();

            var bookingName = $"{user.FirstName} {user.LastName}";
            Pages.ReportPage.IsBookingDisplayed(bookingName, _createRoomOutput.roomName).Should().BeTrue();
        }

        public void Dispose()
        {
            var t = _baseTest.Client.CreateRequest($"{ApiResource.Room}{_createRoomOutput.roomid}", Method.DELETE);

        }
    }
}
