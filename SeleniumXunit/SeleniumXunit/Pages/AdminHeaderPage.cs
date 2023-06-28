using NsTestFrameworkUI.Pages;
using OpenQA.Selenium;
using SeleniumXunit.Helpers.Models.Enum;

namespace SeleniumXunit.Pages;

public class AdminHeaderPage
{
    private readonly By _menuItems = By.CssSelector(".mr-auto li a");
    private readonly By _logoutButton = By.CssSelector("[href='#/admin']");


    public void GoToMenu(Menu menu)
    {
        _menuItems.GetElements().First(x => x.Text.Equals(menu.ToString())).Click();
    }

    public bool IsLogoutButtonDisplayed() => _logoutButton.IsElementPresent();

}