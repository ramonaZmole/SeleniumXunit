using SeleniumXunit.Helpers;
using SeleniumXunit.Helpers.Models;
using SeleniumXunit.Helpers.Models.ApiModels;

namespace SeleniumXunit.Tests.Book;

public class BookingFormTests : IClassFixture<BaseTest>, IDisposable
{
    private readonly CreateRoomOutput _createRoomOutput;
    private readonly BaseTest _baseTest;

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

        Pages.Homepage.BookThisRoom(_createRoomOutput.description);
        Pages.Homepage.BookRoom();
        Pages.Homepage.GetErrorMessages().Should().BeEquivalentTo(Messages.FormErrorMessages);

        Pages.Homepage.InsertBookingDetails(new User());
        Pages.Homepage.BookRoom();
        Pages.Homepage.GetErrorMessages()[0].Should().Be(Messages.AlreadyBookedErrorMessage);
    }

    public void Dispose()
    {
        _baseTest.Client.DeleteRoom(_createRoomOutput.roomid);
    }
}