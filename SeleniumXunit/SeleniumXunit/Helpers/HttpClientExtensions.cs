using Newtonsoft.Json;
using NsTestFrameworkApi.RestSharp;
using RestSharp;
using SeleniumXunit.Helpers.Models.ApiModels;

namespace SeleniumXunit.Helpers;

public static class HttpClientExtensions
{
    public static string GetLoginToken(this RestClient client)
    {
        var output = client.CreateRequest(ApiResource.Login, new LoginInput(), Method.POST);
        return output.Cookies[0].Value;
    }

    public static CreateRoomOutput CreateRoom(this RestClient client)
    {
        var roomResponse = client.CreateRequest(ApiResource.Room, new CreateRoomInput(), Method.POST);
        return JsonConvert.DeserializeObject<CreateRoomOutput>(roomResponse.Content);
    }

    public static void CreateBooking(this RestClient client, CreateBookingInput createBookingInput)
    {
        createBookingInput.bookingdates.checkin = createBookingInput.bookingdates.checkin.Remove(8, 2).Insert(8, Constants.BookingStartDay);
        createBookingInput.bookingdates.checkout = createBookingInput.bookingdates.checkout.Remove(8, 2).Insert(8, Constants.BookingEndDay);
        client.CreateRequest(ApiResource.Booking, createBookingInput, Method.POST);
    }
}