using FluentAssertions;
using Newtonsoft.Json;
using NsTestFrameworkApi.RestSharp;
using NsTestFrameworkUI.Helpers;
using SeleniumXunit.Helpers;
using SeleniumXunit.Helpers.Models.ApiModels;

namespace SeleniumXunit.Tests.Admin;

public class CreateRoomTests : IDisposable
{
    private readonly Helpers.Models.Room _roomModel = new();
    private readonly BaseTest _baseTest;

    public CreateRoomTests()
    {
        _baseTest = new BaseTest();
    }

    [Fact]
    public void WhenCreatingARoom_ThenItShouldBeCreatedTest()
    {
        Browser.GoTo(Constants.AdminUrl);

        Pages.LoginPage.Login();

        Pages.RoomPage.CreateRoom();
        Pages.RoomPage.IsErrorMessageDisplayed().Should().BeTrue();
        var errorMessages = Pages.RoomPage.GetErrorMessages();
        errorMessages.Should().Contain("must be greater than or equal to 1");
        errorMessages.Should().Contain("Room name must be set");

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
        var response = _baseTest.Client.CreateRequest(ApiResource.Room);
        var roomsList = JsonConvert.DeserializeObject<GetRoomsOutput>(response.Content);
        if (roomsList == null) return;
        var id = roomsList.rooms.First(x => x.roomName == int.Parse(_roomModel.RoomName)).roomid;
        _baseTest.Client.CreateRequest($"{ApiResource.Room}{id}", RestSharp.Method.DELETE);
    }
}