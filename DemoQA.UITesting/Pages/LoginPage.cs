using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;
using OpenQA.Selenium;

namespace DemoQA.UITesting.Pages
{
    public class LoginPage:BasePage
    {
        private Element _txtUserName = new(By.Id("userName"));
        private Element _txtPassword = new(By.Id("password"));
        private Element _btnLogin = new(By.Id("login"));
        public void Login(string username, string password){
            _txtUserName.ScrollToElement();
            _txtUserName.InputText(username);
            _txtPassword.InputText(password);
            _btnLogin.ClickOnElement();
        }
    }
}