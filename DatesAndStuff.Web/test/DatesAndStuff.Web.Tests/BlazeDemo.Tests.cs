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


      /////////////////////////////// BONUSZ
        double priceLimit = 300.0;

        var rows = driver.FindElements(By.CssSelector("table tbody tr"));
        foreach (var row in rows)
        {
            // utolso oszlopban vna az ar
            var priceText = row.FindElements(By.TagName("td"))[5].Text;

            // parsol
            var price = double.Parse(priceText.Replace("$", "").Trim(), System.Globalization.CultureInfo.InvariantCulture);

            if (price < priceLimit)
            {
                // keszul a screenshot
                var screenshot = ((ITakesScreenshot)row).GetScreenshot();
                var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                screenshot.SaveAsFile(Path.Combine(desktopPath, "cheap_flight_screenshot.png"));
                break; // elso olcso jaratrol keszit kepet
            }
        }
            }
}