using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AventStack.ExtentReports.Utils;
using Core;
using DemoQA.UITesting.Model;
using DemoQA.UITesting.Pages;
using MongoDB.Bson.IO;
using OpenQA.Selenium;
using OpenQA.Selenium.DevTools.V123.Network;

namespace final.Pages
{
    public class RegisterPage : BasePage
    {
        private Element _txtFirstName = new(By.Id("firstName"));
        private Element _txtLastName = new(By.Id("lastName"));
        private Element _txtEmail = new(By.Id("userEmail"));
        private Element _chkGender(string gender) => new(By.XPath($"//label[text()='{gender}']"));
        private Element _txtMobile = new(By.Id("userNumber"));
        private Element _txtBoD = new(By.Id("dateOfBirthInput"));
        private Element _ddlMonth = new(By.CssSelector("select[class*='month-select']"));
        private Element _ddlYear = new(By.CssSelector("select[class*='year-select']"));
        private Element _lblDay(string day) => new(By.XPath($"//div[contains(@class,'datepicker') and .='{day}']"));
        private Element _txtSubjects = new(By.Id("subjectsInput"));
        private Element _optSubject(string subject) => new(By.XPath($"//div[contains(@class,'option') and contains(text(),'{subject}')]"));
        private Element _chkHobby(string hobby) => new(By.XPath($"//label[text()='{hobby}']"));
        private Element _btnUploadPicture = new(By.Id("uploadPicture"));
        private Element _txaCurrentAddress = new(By.Id("currentAddress"));
        private Element _ddlState = new(By.CssSelector("div#state"));
        private Element _optState(string state) => new(By.XPath($"//div[contains(text(),'{state}')]"));
        private Element _ddlCity = new(By.CssSelector("div#city"));
        private Element _optCity(string city) => new(By.XPath($"//div[contains(text(),'{city}')]"));
        private Element _btnSubmit = new(By.Id("submit"));
        private Element _lblConfirm = new(By.XPath("//div[contains(text(),'Thanks for submitting the form')]"));
        private Element _popupRegister = new(By.ClassName("modal-content"));
        private Element _rowData(string row) => new(By.XPath($"//td[text()='{row}']/following-sibling::td"));
        public void Register(Student student)
        {
            _txtFirstName.InputText(student.FirstName);
            _txtLastName.InputText(student.LastName);
            InputEmail(student.Email);
            SelectGender(student.Gender);
            _txtMobile.InputText(student.Mobile);
            InputDateOfBirth(student.DateOfBirth);
            InputSubjects(student.Subjects);
            SelectHobby(student.Hobbies);
            UploadPicture(student.Picture);
            InputCurrentAddress(student.CurrentAddress);
            SelectState(student.State);
            SelectCity(student.City);
            _btnSubmit.ScrollToElement();
            _btnSubmit.ClickOnElement();
        }
        public void VerifyRegister(Student expectedStudent)
        {
            Assert.That(_popupRegister, Is.Not.Null);
            VerifyPopupData(expectedStudent);
        }
        public void VerifyPopupData(Student expectedStudent)
        {
            Assert.That($"{expectedStudent.FirstName} {expectedStudent.LastName}", Is.EqualTo(_rowData("Student Name").GetTextFromElement()));
            VerifyEmail(expectedStudent.Email);
            Assert.That(expectedStudent.Gender, Is.EqualTo(_rowData("Gender").GetTextFromElement()));
            Assert.That(expectedStudent.Mobile, Is.EqualTo(_rowData("Mobile").GetTextFromElement()));
            VerifyDateOfBirth(expectedStudent.DateOfBirth);
            VerifySubjects(expectedStudent.Subjects);
            VerifyHobbies(expectedStudent.Hobbies);
            VerifyPicture(expectedStudent.Picture);
            VerifyAddress(expectedStudent.CurrentAddress);
            VerifyStateCity(expectedStudent.State, expectedStudent.City);
        }
        public void VerifyStateCity(string state, string city)
        {
            if (state.IsNullOrEmpty() || city.IsNullOrEmpty())
            {
                return;
            }
            Assert.That($"{state} {city}", Is.EqualTo(_rowData("State and City").GetTextFromElement()));
        }
        public void VerifyAddress(string address)
        {
            if (address.IsNullOrEmpty())
            {
                return;
            }
            Assert.That(address, Is.EqualTo(_rowData("Address").GetTextFromElement()));
        }
        public void VerifyPicture(string picture)
        {
            if (picture.IsNullOrEmpty())
            {
                return;
            }
            Assert.That(Path.GetFileName(picture), Is.EqualTo(_rowData("Picture").GetTextFromElement()));
        }
        public void VerifyHobbies(List<string> hobbies)
        {
            if (hobbies.IsNullOrEmpty())
            {
                return;
            }
            var hobbiesText = string.Join(", ", hobbies);
            Assert.That(hobbiesText, Is.EqualTo(_rowData("Hobbies").GetTextFromElement()));
        }
        public void VerifySubjects(List<Subject> subjects)
        {
            if (subjects.IsNullOrEmpty())
            {
                return;
            }
            var actualSubjects = string.Join(", ", subjects.Select(s => s.Name));
            Assert.That(actualSubjects, Is.EqualTo(_rowData("Subjects").GetTextFromElement()));
        }
        public void VerifyDateOfBirth(string dob)
        {
            if (dob.IsNullOrEmpty())
            {
                return;
            }
            Assert.That(ConvertDateFormat(dob), Is.EqualTo(_rowData("Date of Birth").GetTextFromElement()));
        }
        public static string ConvertDateFormat(string inputDate)
        {
            DateTime dateTime = DateTime.ParseExact(inputDate, "d-MMMM-yyyy", CultureInfo.InvariantCulture);
            return dateTime.ToString("dd MMMM,yyyy");
        }
        public void VerifyEmail(string email)
        {
            if (email.IsNullOrEmpty())
            {
                return;
            }
            Assert.That(email, Is.EqualTo(_rowData("Student Email").GetTextFromElement()));
        }
        public void InputCurrentAddress(string address)
        {
            if (address.IsNullOrEmpty())
                return;
            _txaCurrentAddress.InputText(address);
        }
        public void InputEmail(string email)
        {
            if (email.IsNullOrEmpty())
                return;
            _txtEmail.InputText(email);
        }
        public void UploadPicture(string picturePath)
        {
            if (picturePath.IsNullOrEmpty())
                return;
            try
            {
                string baseDirectory = Directory.GetCurrentDirectory();
                string combinedPath = Path.Combine(baseDirectory, picturePath);
                string fullPath = Path.GetFullPath(combinedPath);

                _btnUploadPicture.InputText(combinedPath);
            }
            catch (FileNotFoundException)
            {
                throw new FileNotFoundException("Picture file does not exist.", picturePath);
            }
        }
        public void SelectCity(string city)
        {
            if (city.IsNullOrEmpty())
                return;
            _ddlCity.ClickOnElement();
            _optCity(city).ClickOnElement();
        }
        public void SelectState(string state)
        {
            if (state.IsNullOrEmpty())
                return;
            _ddlState.ClickOnElement();
            _optState(state).ClickOnElement();
        }
        public void SelectGender(string gender)
        {
            _chkGender(gender).ScrollToElement();
            _chkGender(gender).ClickOnElement();
        }
        public void InputSubjects(List<Subject> subjects)
        {
            if (subjects.IsNullOrEmpty())
                return;
            foreach (Subject subject in subjects)
            {
                _txtSubjects.InputText(subject.Name);
                _optSubject(subject.Name).ScrollToElement();
                _optSubject(subject.Name).ClickOnElement();
            }
        }
        public void InputDateOfBirth(string date)
        {
            if (date.IsNullOrEmpty())
                return;
            string[] dateParts = date.Split('-', ' ');
            string targetDay = dateParts[0];
            string targetMonth = dateParts[1];
            string targetYear = dateParts[2];

            _txtBoD.ScrollToElement();
            _txtBoD.ClickOnElement();
            _ddlYear.SelectByText(targetYear);
            _ddlMonth.SelectByText(targetMonth);
            _lblDay(targetDay).ClickOnElement();
        }
        public void SelectHobby(List<string> hobbies)
        {
            if (hobbies.IsNullOrEmpty())
                return;
            foreach (string hobby in hobbies)
            {
                _chkHobby(hobby).ClickOnElement();
            }
        }
    }
}