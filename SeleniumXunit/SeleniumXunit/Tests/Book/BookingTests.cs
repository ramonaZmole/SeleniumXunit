using FluentAssertions;
using NsTestFrameworkApi.RestSharp;
using NsTestFrameworkUI.Helpers;
using RestSharp;
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

        Pages.HomePage.BookThisRoom(_createRoomResponse.description);
        Pages.HomePage.InsertBookingDetails(new User());
        Pages.HomePage.BookRoom();
        Pages.HomePage.IsSuccessMessageDisplayed().Should().BeTrue();
    }

    [Fact]
    public void WhenCancellingBooking_FormShouldNotBeDisplayedTest()
    {
        Browser.GoTo(Constants.Url);

        Pages.HomePage.BookThisRoom(_createRoomResponse.description);
        Pages.HomePage.InsertBookingDetails(new User());
        Pages.HomePage.CancelBooking();
        Pages.HomePage.IsBookingFormDisplayed().Should().BeFalse();
        Pages.HomePage.IsCalendarDisplayed().Should().BeFalse();
    }

    public void Dispose()
    {
        _baseTest.Dispose();
        _baseTest.Client.CreateRequest($"{ApiResource.Room}{_createRoomResponse.roomid}", Method.DELETE);
    }
}