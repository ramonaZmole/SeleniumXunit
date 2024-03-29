﻿using NsTestFrameworkUI.Pages;
using OpenQA.Selenium;
using SeleniumXunit.Helpers.Models;

namespace SeleniumXunit.Pages;

public class ReportPage : CalendarPage
{
    private readonly By _firstNameInput = By.CssSelector("[name='firstname']");
    private readonly By _lastNameInput = By.CssSelector("[name='lastname']");
    private readonly By _roomDropdown = By.CssSelector("#roomid");
    private readonly By _depositPaidDropdown = By.CssSelector("#depositpaid");

    private readonly By _bookButton = By.CssSelector(".col-sm-12.text-right button:last-child");


    public bool IsBookingDisplayed(string name, int roomName)
    {
        PageHelpers.ScrollDownToView(1000);
        try
        {
            Browser.WebDriver.FindElement(By.XPath($"//*[contains(text(), '{name} - Room: {roomName}')]"));
            return true;
        }
        catch (NoSuchElementException)
        {
            return false;
        }
    }

    public void InsertBookingDetails(User user, Room room)
    {
        _firstNameInput.ActionSendKeys(user.FirstName);
        _lastNameInput.ActionSendKeys(user.LastName);
        _roomDropdown.SelectFromDropdownByText(room.RoomName);
        _depositPaidDropdown.SelectFromDropdownByText("true");
    }
    public void Book() => _bookButton.ActionClick();

}