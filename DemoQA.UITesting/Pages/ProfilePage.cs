using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;
using Core.Driver;
using DemoQA.UITesting.Pages;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace final.Pages
{
    public class ProfilePage : BasePage
    {
        private Element _txtSearch = new(By.Id("searchBox"));
        private Element _tblBook = new(By.CssSelector("[class*='table']"));
        private Element _btnOK = new(By.XPath("//button[.='OK']"));
        public void RemoveBook(string title)
        {
            _txtSearch.InputText(title);
            var book = GetBook(title);
            if (book == null)
                Assert.That(GetBook(title), Is.Null, "The book was not found");
            var _btnRemove = book.FindElement(By.Id("delete-record-undefined"));
            _btnRemove.Click();

            _btnOK.ClickOnElement();
            HandleAlert();

            WebDriverWait wait = new WebDriverWait(DriverManager.WebDriver, TimeSpan.FromSeconds(10));
            wait.Until(d => GetBook(title) == null);
        }
        public IWebElement GetBook(string title)
        {
            try
            {
                var firstRow = _tblBook.FindElements(By.CssSelector("div[role='rowgroup'] > div"));
                return firstRow.FirstOrDefault(row => row.FindElement(By.CssSelector("a")).Text == title);
            }
            catch (NoSuchElementException)
            {
                return null;
            }
            catch (WebDriverTimeoutException){
                return null;
            }
            catch(NullReferenceException){
                return null;
            }
        }
        public void AssertBookRemoved(string title)
        {
            var row = GetBook(title);
            Assert.That(row, Is.Null, "The book have not removed yet");
        }
        public void HandleAlert()
        {
            WebDriverWait wait = new WebDriverWait(DriverManager.WebDriver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.AlertIsPresent());
            IAlert alert = DriverManager.WebDriver.SwitchTo().Alert();
            alert.Accept(); // Click the "OK" button
        }

    }
}