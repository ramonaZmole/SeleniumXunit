using SeleniumXunit.Helpers;
using SeleniumXunit.Helpers.Models;

namespace SeleniumXunit.Tests;

public class ContactFormTests : BaseTest
{
    private static readonly ContactForm EmptyData = new()
    {
        Email = string.Empty,
        Name = string.Empty,
        Phone = string.Empty,
        Message = string.Empty,
        Subject = string.Empty
    };

    private static readonly ContactForm InvalidData = new()
    {
        Email = RandomNumber.Next(50).ToString(),
        Name = RandomNumber.Next(50).ToString(),
        Phone = RandomNumber.Next(50).ToString(),
        Message = RandomNumber.Next(50).ToString(),
        Subject = RandomNumber.Next(50).ToString()
    };

    public static IEnumerable<object[]> ContactForm => new List<object[]>
    {
        new object[] { EmptyData, Messages.ContactFormEmptyFieldsErrorMessages },
        new object[] { InvalidData, Messages.ContactFormInvalidDataErrorMessages}
    };

    [MemberData(nameof(ContactForm))]
    [Theory]
    public void WhenSendingMessageWithInvalidData_ErrorShouldBeReturned(ContactForm formData, List<string> errorMessages)
    {
        Browser.GoTo(Constants.Url);

        Pages.Homepage.SendMessage(formData);

        Pages.Homepage.GetErrorMessages().Should().BeEquivalentTo(errorMessages);
    }
}