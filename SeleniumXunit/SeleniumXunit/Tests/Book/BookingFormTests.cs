using FluentAssertions;
using NsTestFrameworkApi.RestSharp;
using NsTestFrameworkUI.Helpers;
using RestSharp;
using SeleniumXunit.Helpers;
using SeleniumXunit.Helpers.Models;
using SeleniumXunit.Helpers.Models.ApiModels;

namespace SeleniumXunit.Tests.Book;

public class BookingFormTests : IClassFixture<BaseTest>, IDisposable
{
    private CreateRoomOutput _createRoomOutput;
    private BaseTest _baseTest;

    public BookingFormTests(BaseTest baseTest)
    {
        _baseTest = baseTest;
        _createRoomOutput = _baseTest.Client.CreateRoom();

        var bookingInput = new CreateBookingInput
        {
            roomid = _createRoomOutput.roomid
        };
        _baseTest.Client.CreateBooking(bookingInput);
    }


    [Fact]
    public void WhenBookingRoom_ErrorMessageShouldBeDisplayedTest()
    {
        Browser.GoTo(Constants.Url);

        Pages.HomePage.BookThisRoom(_createRoomOutput.description);
        Pages.HomePage.BookRoom();
        Pages.HomePage.GetErrorMessages().Should().BeEquivalentTo(Constants.FormErrorMessages);

        Pages.HomePage.InsertBookingDetails(new User());
        Pages.HomePage.BookRoom();
        Pages.HomePage.GetErrorMessages()[0].Should().Be(Constants.AlreadyBookedErrorMessage);
    }

    public void Dispose()
    {
        _baseTest.Client.CreateRequest($"{ApiResource.Room}{_createRoomOutput.roomid}", Method.DELETE);
    }
}