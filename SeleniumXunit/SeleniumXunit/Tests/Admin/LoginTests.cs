using System.Globalization;
using System.Reflection;
using CsvHelper;
using CsvHelper.Configuration;
using SeleniumXunit.Helpers;
using SeleniumXunit.Helpers.Models;

namespace SeleniumXunit.Tests.Admin;

public class LoginTests : BaseTest
{
    [InlineData("admin", "password", true)]
    [InlineData("invalidUser", "invalidPassword", false)]
    [Theory]
    public void LoginAsAdmin(string username, string password, bool isLoggedIn)
    {
        Browser.GoTo(Constants.AdminUrl);

        Pages.LoginPage.Login(username, password);

        Pages.AdminHeaderPage.IsLogoutButtonDisplayed().Should().Be(isLoggedIn);
    }

    [MemberData(nameof(GetLoginScenarios))]
    [Theory]
    public void LoginAsAdmin2(LoginData loginData)
    {
        Browser.GoTo(Constants.AdminUrl);

        Pages.LoginPage.Login(loginData.Username, loginData.Password);

        Pages.AdminHeaderPage.IsLogoutButtonDisplayed().Should().Be(loginData.IsLoggedIn);
    }

    public static IEnumerable<object[]> GetLoginScenarios()
    {
        var dataFilePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\Helpers\Data\LoginData.csv";

        using var stream = new StreamReader(dataFilePath);
        using var reader = new CsvReader(stream, new CsvConfiguration(CultureInfo.CurrentCulture));
        var rows = reader.GetRecords<LoginData>();

        return rows.Select(row => new object[] { row }).ToList();
    }
}