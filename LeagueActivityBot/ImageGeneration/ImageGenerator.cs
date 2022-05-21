using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Drawing;
using System.IO;

namespace LeagueActivityBot.ImageGeneration
{
    public static class ImageGenerator
    {
        public static byte[] ToBytes(this Image img)
        {
            using var stream = new MemoryStream();

            img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);

            return stream.ToArray();
        }

        public static Bitmap GetScreenshot(this IWebDriver driver, IWebElement element, double dpi = 1.0)
        {
            var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
            var img = Image.FromStream(new MemoryStream(screenshot.AsByteArray)) as Bitmap;

            // Если dpi нестандартный, то у элемента Locatiton и Size будут без учета dpi, надо скейлить вручную.
            var location = element.Location;
            var size = element.Size;
            
            location.X = (int)(location.X * dpi);
            location.Y = (int)(location.Y * dpi);
            size.Width = (int)(size.Width * dpi);
            size.Height = (int)(size.Height * dpi);

            return img.Clone(new Rectangle(location, size), img.PixelFormat);
        }

        public static void WaitUntilVisible(this WebDriver webDriver, IWebElement element)
        {
            var driverWait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(1));
            driverWait.Until(driver => ((IJavaScriptExecutor)driver).ExecuteScript("return arguments[0].complete", element));
        }

        public static byte[] Gen()
        {
            var chromeOptions = new ChromeOptions();
            double dpi = 3.0;
            chromeOptions.AddArguments("headless");
            chromeOptions.AddArguments($"--force-device-scale-factor={dpi}");

            using var driver = new ChromeDriver(chromeOptions);

            driver.Navigate().GoToUrl("https://localhost:44339/");

            var summaryElement = driver.FindElement(By.Id("match-summary"));
            var avatar = summaryElement.FindElement(By.ClassName("avatar"));

            driver.WaitUntilVisible(avatar);

            return driver.GetScreenshot(summaryElement, dpi).ToBytes();
        }
    }
}
