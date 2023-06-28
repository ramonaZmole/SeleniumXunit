using SeleniumXunit.Helpers;
using SeleniumXunit.Helpers.Models;
using SeleniumXunit.Helpers.Models.ApiModels;

namespace SeleniumXunit.Tests.Book;

public class BookingTests : IDisposable
{
    private readonly CreateRoomOutput _createRoomResponse;
    private readonly BaseTest _baseTest;

    public BookingTests()
    {
        _baseTest = new BaseTest();
        _createRoomResponse = _baseTest.Client.CreateRoom();
    }

    [Fact]
    public void WhenBookingRoom_SuccessMessageShouldBeDisplayedTest()
    {
        Browser.GoTo(Constants.Url);

        Pages.Homepage.BookThisRoom(_createRoomResponse.description);
        Pages.Homepage.InsertBookingDetails(new User());
        Pages.Homepage.BookRoom();
        Pages.Homepage.IsSuccessMessageDisplayed().Should().BeTrue();
    }

    [Fact]
    public void WhenCancellingBooking_FormShouldNotBeDisplayedTest()
    {
        Browser.GoTo(Constants.Url);

        Pages.Homepage.BookThisRoom(_createRoomResponse.description);
        Pages.Homepage.InsertBookingDetails(new User());
        Pages.Homepage.CancelBooking();
        Pages.Homepage.IsBookingFormDisplayed().Should().BeFalse();
        Pages.Homepage.IsCalendarDisplayed().Should().BeFalse();
    }

    public void Dispose()
    {
        _baseTest.Dispose();
        _baseTest.Client.DeleteRoom(_createRoomResponse.roomid);
    }
}