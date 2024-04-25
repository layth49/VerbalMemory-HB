using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using HtmlAgilityPack;
using OpenQA.Selenium.Support.UI;

class VerbalMemoryTest
{
    static void Main()
    {
        // Set up the driver
        IWebDriver driver = new ChromeDriver();

        // Create a list to store seen words
        List<string> seenWords = new List<string>();

        // Navigate to word memory website
        driver.Navigate().GoToUrl("https://humanbenchmark.com/tests/verbal-memory");

        // Give some time for the page to load and maximize the window to full screen
        driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(10);
        driver.Manage().Window.Maximize();

        #region
        IWebElement loginButton = driver.FindElement(By.XPath("//*[@id=\"root\"]/div/div[3]/div/div[2]/a[2]"));
        loginButton.Click();

        IWebElement inputUser = WaitForElement(driver, By.CssSelector("#root > div > div.css-1f554t4.e19owgy74 > div > div > form > p:nth-child(1) > input[type=text]"), TimeSpan.FromSeconds(10));
        inputUser.SendKeys("BOT49");

        IWebElement inputKey = WaitForElement(driver, By.CssSelector("#root > div > div.css-1f554t4.e19owgy74 > div > div > form > p:nth-child(2) > input[type=password]"), TimeSpan.FromSeconds(10));
        inputKey.SendKeys("Layth12+");

        IWebElement confirmButton = driver.FindElement(By.XPath("//*[@id=\"root\"]/div/div[4]/div/div/form/p[3]/input"));
        confirmButton.Click();

        IWebElement verbalButton = WaitForElement(driver, By.XPath("//*[@id=\"root\"]/div/div[4]/div/div/div[1]/div/div[4]"), TimeSpan.FromSeconds(10));
        verbalButton.Click();

        IWebElement startButton = WaitForElement(driver, By.XPath("//*[@id=\"root\"]/div/div[4]/div/div/div[2]/div[2]/div/div[4]/a"), TimeSpan.FromSeconds(10));
        startButton.Click();

        IWebElement startTest = driver.FindElement(By.XPath("//*[@id=\"root\"]/div/div[4]/div[1]/div/div/div"));
        Thread.Sleep(1000);
        startTest.Click();
        #endregion

        IWebElement start2Button = driver.FindElement(By.XPath("//*[@id=\"root\"]/div/div[4]/div[1]/div/div/div/div[4]/button"));
        start2Button.Click();

        Thread.Sleep(1000);


        // Get the page source
        string pageSource = driver.PageSource;

        // Use HtmlAgilityPack to parse the page source
        HtmlDocument htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(pageSource);
        while (true)
        {
            // Get the word shown using XPath
            string wordXPath = "//*[@id=\"root\"]/div/div[4]/div[1]/div/div/div/div[2]/div";
            IWebElement wordElement = WaitForElement(driver, By.XPath(wordXPath), TimeSpan.FromSeconds(10));
            string word = wordElement.Text;
            

            // Check if the word is not in the seenWords list
            if (!seenWords.Contains(word))
            {
                // Add the word to the seenWords list
                seenWords.Add(word);

                // Print how many words are in the list
                Console.WriteLine($"{seenWords.Count} stored words");

                // Print the word (for fun)
                Console.WriteLine(word);

                // Select new word using XPath
                string newWordXPath = "//*[@id=\"root\"]/div/div[4]/div[1]/div/div/div/div[3]/button[2]";
                IWebElement newWordButton = WaitForElement(driver, By.XPath(newWordXPath), TimeSpan.FromSeconds(10));
                newWordButton.Click();
            }
            else
            {
                // Select seen word button using XPath
                string seenWordXPath = "//*[@id=\"root\"]/div/div[4]/div[1]/div/div/div/div[3]/button[1]";
                IWebElement seenWordButton = WaitForElement(driver, By.XPath(seenWordXPath), TimeSpan.FromSeconds(10));
                seenWordButton.Click();
            }
        }
    }

    // Helper method to wait for an element to be present
    static IWebElement WaitForElement(IWebDriver driver, By by, TimeSpan timeout)
    {
        WebDriverWait wait = new WebDriverWait(driver, timeout);
        return wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(by));
    }
}
