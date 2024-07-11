using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using Core.Driver;

namespace Core
{
    public class Element
    {
        public Element(By by)
        {
            this.by = by;
        }

        public By? by { get; set; }

        public IWebElement WaitForElementToBeVisible()
        {

            var wait = new WebDriverWait(DriverManager.WebDriver, TimeSpan.FromSeconds(5));
            return wait.Until(ExpectedConditions.ElementIsVisible(this.by));
        }
        public IWebElement WaitForElementToBeClickable()
        {
            var wait = new WebDriverWait(DriverManager.WebDriver, TimeSpan.FromSeconds(5));
            return wait.Until(ExpectedConditions.ElementToBeClickable(this.by));
        }
        public void ClickOnElement()
        {
            IWebElement element = WaitForElementToBeClickable();
            element.Click();
        }

        public void InputText(string value)
        {
            IWebElement element = WaitForElementToBeVisible();
            element.SendKeys(value);
        }
        public String GetTextFromElement()
        {
            return WaitForElementToBeVisible().Text;
        }
        public void SelectByText(string text)
        {
            IWebElement element = WaitForElementToBeVisible();
            var selectElement = new SelectElement(element);
            selectElement.SelectByText(text);
        }
        public IList<IWebElement> FindElements(By row)
        {
            IWebElement element = this.WaitForElementToBeVisible();
            var webElements = element.FindElements(row);
            IList<IWebElement> elements = new List<IWebElement>();
            foreach (var webElement in webElements)
            {
                elements.Add(webElement);
            }
            return elements;
        }
        public bool IsExpanded()
        {
            IWebElement element = WaitForElementToBeVisible();
            return element.GetAttribute("expanded") == "true";
        }
        public bool IsEnabled()
        {
            IWebElement element = WaitForElementToBeVisible();
            return element.Enabled;
        }
        public bool IsDisplayed()
        {
            try
            {
                IWebElement element = WaitForElementToBeVisible();
                return element.Displayed;
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }
        public void ClearText()
        {
            IWebElement element = WaitForElementToBeVisible();
            element.Clear();
        }
        public void ScrollToElement()
        {
            IWebElement element = WaitForElementToBeVisible();
            IJavaScriptExecutor js = (IJavaScriptExecutor)DriverManager.WebDriver;
            js.ExecuteScript("arguments[0].scrollIntoView(true);", element);
        }
    }
}