using SeleniumXunit.Helpers;
using SeleniumXunit.Helpers.Models.Enum;

namespace SeleniumXunit.Tests.Admin;

public class CreateRoomTests : IDisposable
{
    private Helpers.Models.Room _roomModel = new();
    private readonly BaseTest _baseTest;

    public CreateRoomTests()
    {
        _baseTest = new BaseTest();
    }

    [InlineData(RoomType.Double)]
    [InlineData(RoomType.Family)]
    [InlineData(RoomType.Single)]
    [InlineData(RoomType.Suite)]
    [InlineData(RoomType.Twin)]
    [Theory]
    public void WhenCreatingARoom_ThenItShouldBeCreatedTest(RoomType roomType)
    {
        Browser.GoTo(Constants.AdminUrl);

        Pages.LoginPage.Login();

        Pages.RoomPage.CreateRoom();
        Pages.RoomPage.IsErrorMessageDisplayed().Should().BeTrue();
        var errorMessages = Pages.RoomPage.GetErrorMessages();
        errorMessages.Should().Contain("must be greater than or equal to 1");
        errorMessages.Should().Contain("Room name must be set");

        _roomModel = new Helpers.Models.Room
        {
            Type = roomType.ToString()
        };

        Pages.RoomPage.InsertRoomDetails(_roomModel);
        Pages.RoomPage.CreateRoom();
        Pages.RoomPage.GetLastRoomDetails().Should().BeEquivalentTo(_roomModel);
    }

    [Fact]
    public void WhenCreatingRoomWithNoRoomDetails_NoFeaturesShouldBeDisplayedTest()
    {
        _roomModel.RoomDetails = string.Empty;

        Browser.GoTo(Constants.AdminUrl);
        Pages.LoginPage.Login();

        Pages.RoomPage.InsertRoomDetails(_roomModel);
        Pages.RoomPage.CreateRoom();
        Pages.RoomPage.GetLastRoomDetails().RoomDetails.Should().Be("No features added to the room");
    }

    public void Dispose()
    {
        _baseTest.Dispose();

        _baseTest.Client.DeleteRoom(_roomModel.RoomName);
    }
}