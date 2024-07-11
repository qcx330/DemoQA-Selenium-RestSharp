using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Reports;
using Core.Utilities;
using DemoQA.Service.Service;
using DemoQA.APITesting.DataObject;
using FluentAssertions;
using MongoDB.Bson;
using Newtonsoft.Json;
using NUnit.Framework;
using OpenQA.Selenium;

namespace DemoQA.APITesting.Test
{
    [TestFixture("user", "Book"), Category("AddBookToCollection")]
    [Parallelizable(ParallelScope.None)]
    public class AddBookToColectionTest : BaseTest
    {
        private BookService _bookServices;
        private UserService _userServices;
        private string userDataKey, bookDataKey, token;
        UserDto user;
        BookDto book;
        public AddBookToColectionTest(string userKey, string bookKey)
        {
            _bookServices = new BookService(ApiClient);
            _userServices = new UserService(ApiClient);
            userDataKey = userKey;
            bookDataKey = bookKey;
            user = Hook.Users[userDataKey];
            book = Hook.Books[bookDataKey];
        }
        [SetUp]
        public new void Setup(){
            ReportLog.Info("Generate token for account");
            _userServices.StoreUserToken(userDataKey, user.Username, user.Password);
            token = _userServices.GetUserToken(userDataKey);
        }
        [Test]
        public async Task AddBookToCollectionSuccessfully()
        {
            var responseUser = await _userServices.GetUserAsync(user.UserId, token);
            if (responseUser.Data.Books.Any(book => book.Isbn == book.Isbn))
                await _bookServices.DeleteBookFromCollectionAsync(user.UserId, book.Isbn, token);

            ReportLog.Info("1. Add book to user's collection");
            var response = await _bookServices.AddBookToCollectionAsync(user.UserId, book.Isbn, token);
            
            ReportLog.Info("2. Verify add book response");
            Console.WriteLine(response.Content);
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
            response.Data.Books.FirstOrDefault().Isbn.Should().Be(book.Isbn);

            ReportLog.Info("3. Delete added book from user's collection");
            await _bookServices.DeleteBookFromCollectionAsync(user.UserId, book.Isbn, token);
        }
        [Test]
        public async Task AddBookToCollectionWithoutAuthotized()
        {
            ReportLog.Info("1. Add book to user's collection");
            var response = await _bookServices.AddBookToCollectionAsync(user.UserId, book.Isbn, null);
            ReportLog.Info("2. Verify add book response");
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
            var result = (dynamic)JsonConvert.DeserializeObject(response.Content);
            ((int)result["code"]).Should().Be(1200);
            ((string)result["message"]).Should().Be("User not authorized!");

            
        }
        [Test]
        public async Task AddBookToCollectionWithWrongUserId()
        {
            ReportLog.Info("1. Add book to user's collection");
            var response = await _bookServices.AddBookToCollectionAsync(user.UserId + "444", book.Isbn, token);
            ReportLog.Info("2. Verify add book response");
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
            var result = (dynamic)JsonConvert.DeserializeObject(response.Content);
            Console.WriteLine(result);
            ((int)result["code"]).Should().Be(1207);
            ((string)result["message"]).Should().Be("User Id not correct!");
        }
        [Test]
        public async Task AddBookToCollectionWithWrongBookId()
        {
            ReportLog.Info("1. Add book to user's collection");
            var response = await _bookServices.AddBookToCollectionAsync(user.UserId, book.Isbn+"444", token);
            ReportLog.Info("2. Verify add book response");
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            var result = (dynamic)JsonConvert.DeserializeObject(response.Content);
            Console.WriteLine(result);
            ((int)result["code"]).Should().Be(1205);
            ((string)result["message"]).Should().Be("ISBN supplied is not available in Books Collection!");
        }
        [Test]
        [TestCase("9781449325862")]
        public async Task AddBookToCollectionWithAddedBookId(string isbn)
        {

            ReportLog.Info("1. Add book to user's collection");
            var response = await _bookServices.AddBookToCollectionAsync(user.UserId, isbn, token);
            ReportLog.Info("2. Verify add book response");
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            var result = (dynamic)JsonConvert.DeserializeObject(response.Content);
            Console.WriteLine(result);
            ((int)result["code"]).Should().Be(1210);
            ((string)result["message"]).Should().Be("ISBN already present in the User's Collection!");
        }
    }
}