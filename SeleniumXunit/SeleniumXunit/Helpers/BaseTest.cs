using System.Reflection;
using RestSharp;

namespace SeleniumXunit.Helpers;

public class BaseTest : IDisposable
{
    public readonly RestClient Client = RequestHelper.GetRestClient(Constants.Url);

    public BaseTest()
    {
        SetClientToken();
        Browser.InitializeDriver(new DriverOptions
        {
            IsHeadless = true,
            ChromeDriverPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
        });
    }

    private void SetClientToken()
    {
        var token = Client.GetLoginToken();
        Client.AddDefaultHeader("cookie", $"token={token}");
    }

    public void Dispose()
    {
        Browser.Cleanup();
    }
}