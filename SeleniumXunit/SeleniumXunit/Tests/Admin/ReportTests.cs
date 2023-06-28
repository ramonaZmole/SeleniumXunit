using SeleniumXunit.Helpers;
using SeleniumXunit.Helpers.Models.ApiModels;
using SeleniumXunit.Helpers.Models.Enum;

namespace SeleniumXunit.Tests.Admin;

public class ReportTests : IClassFixture<BaseTest>, IDisposable
{
    private readonly CreateRoomOutput _createRoomOutput;
    private readonly CreateBookingInput _bookingInput;
    private readonly BaseTest _baseTest;

    public ReportTests(BaseTest baseTest)
    {
        _baseTest = baseTest;
        _createRoomOutput = _baseTest.Client.CreateRoom();

        _bookingInput = new CreateBookingInput
        {
            roomid = _createRoomOutput.roomid
        };
        _baseTest.Client.CreateBooking(_bookingInput);
    }

    [Fact]
    public void WhenViewingReports_BookedRoomsShouldBeDisplayedTest()
    {
        Browser.GoTo(Constants.AdminUrl);

        Pages.LoginPage.Login();
        Pages.AdminHeaderPage.GoToMenu(Menu.Report);

        var bookingName = $"{_bookingInput.firstname} {_bookingInput.lastname}";
        Pages.ReportPage.IsBookingDisplayed(bookingName, _createRoomOutput.roomName).Should().BeTrue();
    }

    public void Dispose()
    {
        _baseTest.Client.DeleteRoom(_createRoomOutput.roomid);
    }
}