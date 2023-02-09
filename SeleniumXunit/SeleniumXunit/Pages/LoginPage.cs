using NsTestFrameworkUI.Helpers;
using NsTestFrameworkUI.Pages;
using OpenQA.Selenium;
using SeleniumXunit.Helpers;

namespace SeleniumXunit.Pages;

public class LoginPage
{
    #region Selectors

    private readonly By _usernameInput = By.CssSelector("#username");
    private readonly By _passwordInput = By.CssSelector("#password");
    private readonly By _loginButton = By.CssSelector("#doLogin");

    #endregion

    public void Login()
    {
        _usernameInput.ActionSendKeys(Constants.Username);
        _passwordInput.ActionSendKeys(Constants.Password);
        _loginButton.ActionClick();
        WaitHelpers.ExplicitWait();
    }

}