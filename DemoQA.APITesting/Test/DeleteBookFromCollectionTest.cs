using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Reports;
using Core.Utilities;
using DemoQA.Service.Service;
using DemoQA.APITesting.DataObject;
using FluentAssertions;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;
using NUnit.Framework;

namespace DemoQA.APITesting.Test
{
    [TestFixture("user", "Book"), Category("DeleteBookFromCollection")]
    [Parallelizable(ParallelScope.None)]
    public class DeleteBookFromCollectionTest:BaseTest
    {

        private BookService _bookServices;
        private UserService _userServices;
        private string userDataKey, bookDataKey, token;
        UserDto user;
        BookDto book;
        public DeleteBookFromCollectionTest(string userKey, string bookKey)
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
        public async Task DeleteBookToCollectionSuccessfully()
        {
            ReportLog.Info("1. Add book to user's collection");
            await _bookServices.AddBookToCollectionAsync(user.UserId, book.Isbn, token);
            ReportLog.Info("2. Delete book from user's collection");
            var response = await _bookServices.DeleteBookFromCollectionAsync(user.UserId, book.Isbn, token);
            Console.WriteLine(response.Content);
            ReportLog.Info("3. Verify delete book response");
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        }
        [Test]
        public async Task DeleteBookFromCollectionWithoutAuthotized()
        {
            ReportLog.Info("1. Delete book from user's collection");
            var response = await _bookServices.DeleteBookFromCollectionAsync(user.UserId, book.Isbn, null);
            Console.WriteLine(response.Content);

            ReportLog.Info("2. Verify delete book response");
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
            var result = (dynamic)JsonConvert.DeserializeObject(response.Content);
            ((int)result["code"]).Should().Be(1200);
            ((string)result["message"]).Should().Be("User not authorized!");
        }
        [Test]
        public async Task DeleteBookFromCollectionWithWrongUserId()
        {
            ReportLog.Info("1. Add book to user's collection");
            await _bookServices.AddBookToCollectionAsync(user.UserId, book.Isbn, token);
            ReportLog.Info("2. Delete book from user's collection");
            var response = await _bookServices.DeleteBookFromCollectionAsync(user.UserId+"4444", book.Isbn, token);
            ReportLog.Info("3. Verify add book response");
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
            var result = (dynamic)JsonConvert.DeserializeObject(response.Content);
            Console.WriteLine(result);
            ((int)result["code"]).Should().Be(1207);
            ((string)result["message"]).Should().Be("User Id not correct!");

            await _bookServices.DeleteBookFromCollectionAsync(user.UserId, book.Isbn, token);
        }
        [Test]
        public async Task DeleteBookFromCollectionWithWrongBookId()
        {
            ReportLog.Info("1. Add book to user's collection");
            await _bookServices.AddBookToCollectionAsync(user.UserId, book.Isbn, token);
            ReportLog.Info("2. Delete book from user's collection");
            var response = await _bookServices.DeleteBookFromCollectionAsync(user.UserId, book.Isbn+"444", token);
            ReportLog.Info("3. Verify add book response");
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            var result = (dynamic)JsonConvert.DeserializeObject(response.Content);
            Console.WriteLine(result);
            ((int)result["code"]).Should().Be(1206);
            ((string)result["message"]).Should().Be("ISBN supplied is not available in User's Collection!");

            await _bookServices.DeleteBookFromCollectionAsync(user.UserId, book.Isbn, token);
        }
    }
}