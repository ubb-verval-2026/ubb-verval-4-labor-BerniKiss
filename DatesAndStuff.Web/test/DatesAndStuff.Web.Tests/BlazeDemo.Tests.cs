using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using FluentAssertions;
using NUnit.Framework;

namespace DatesAndStuff.Web.Tests;

[TestFixture]
public class BlazeDemoTests
{
    private IWebDriver driver;

    [SetUp]
    public void SetupTest()
    {
        driver = new ChromeDriver();
    }

    [TearDown]
    public void TeardownTest()
    {
        driver.Quit();
        driver.Dispose();
    }

    [Test]
    public void BlazDemo_MexicoCity_Dublin_AtLeastThreeFlights()
    {
        // Arrange
        driver.Navigate().GoToUrl("https://blazedemo.com");

        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

        // kivalasztja mexico cityt
        var fromDropdown = new SelectElement(driver.FindElement(By.Name("fromPort")));
        fromDropdown.SelectByText("Mexico City");

        // kivalasztja dublint
        var toDropdown = new SelectElement(driver.FindElement(By.Name("toPort")));
        toDropdown.SelectByText("Dublin");

        // Act
        driver.FindElement(By.CssSelector("input[type='submit']")).Click();

        //  legalabb 3 jarat
        wait.Until(d => d.FindElements(By.CssSelector("table tbody tr")).Count >= 3);

        // megszamolnja ohgy legalabb 3 legyen
        var flights = driver.FindElements(By.CssSelector("table tbody tr"));
        flights.Count.Should().BeGreaterThanOrEqualTo(3);
    }
}