using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Core.Extentions;
using Core.Reports;
using Core.Utilities;
using DemoQA.Service.Service;
using DemoQA.APITesting.DataObject;
using FluentAssertions;
using Newtonsoft.Json;
using NUnit.Framework;

namespace DemoQA.APITesting.Test
{
    [TestFixture("user", "Book", "ReplaceBook"), Category("ReplaceBook")]
    [Parallelizable(ParallelScope.None)]
    public class ReplaceBookInCollectionTest :BaseTest
    {
        private UserService _userServices;
        private BookService _bookServices;
        private string userDataKey, bookDataKey, replaceBookDataKey, token;
        UserDto user;
        BookDto book, replaceBook;
        public ReplaceBookInCollectionTest(string userKey, string book1Key, string book2Key)
        {
            _bookServices = new BookService(ApiClient);
            _userServices = new UserService(ApiClient);
            this.userDataKey = userKey;
            this.bookDataKey = book1Key;
            this.replaceBookDataKey = book2Key;
            user = Hook.Users[userDataKey];
            book = Hook.Books[bookDataKey];
            replaceBook = Hook.Books[replaceBookDataKey];
        }
        [SetUp]
        public new void Setup(){
            ReportLog.Info("Generate token for account");
            _userServices.StoreUserToken(userDataKey, user.Username, user.Password);
            token = _userServices.GetUserToken(userDataKey);
        }
        [Test]
        public async Task RepleaceBookInCollectionSuccessfully()
        {
            ReportLog.Info("1. Add book to User collection");
            await _bookServices.AddBookToCollectionAsync(user.UserId, book.Isbn, token);

            ReportLog.Info($"2. Replace {book.Title} by {replaceBook.Title} User collection");
            var response = await _bookServices.RepleaceBookInCollectionAsync(user.UserId,book.Isbn, replaceBook.Isbn ,token);
            
            ReportLog.Info("3. Verify replace books response");
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            response.Data.UserId.Should().Be(user.UserId);
            response.Data.Username.Should().BeEquivalentTo(user.Username);
            response.Data.Books.Should().ContainSingle(book => book.Isbn == replaceBook.Isbn);

            ReportLog.Info("4. Verify json schema of the response body");
            response.VerifySchema(SchemaConstant.ReplaceBookSchema);

            ReportLog.Info("5. Delete replaced book in collection");
            await _bookServices.DeleteBookFromCollectionAsync(user.UserId, replaceBook.Isbn, token);
        }
        [Test]
        public async Task RepleaceBookInCollectionWithoutAuthorized()
        {
            ReportLog.Info("1. Add book to User collection");
            await _bookServices.AddBookToCollectionAsync(user.UserId, book.Isbn, token);

            ReportLog.Info($"2. Replace {book.Title} by {replaceBook.Title} User collection");
            var response = await _bookServices.RepleaceBookInCollectionAsync(user.UserId,book.Isbn, replaceBook.Isbn ,null);
            
            ReportLog.Info("3. Verify replace books response");
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
            var result = (dynamic)JsonConvert.DeserializeObject(response.Content);
            ((int)result["code"]).Should().Be(1200);
            ((string)result["message"]).Should().Be("User not authorized!");

            ReportLog.Info("4. Delete replaced book in collection");
            await _bookServices.DeleteBookFromCollectionAsync(user.UserId, replaceBook.Isbn, token);
        }
        [Test]
        public async Task RepleaceBookInCollectionWithWrongUserId()
        {
            ReportLog.Info("1. Add book to User collection");
            await _bookServices.AddBookToCollectionAsync(user.UserId, book.Isbn, token);

            ReportLog.Info($"2. Replace {book.Title} by {replaceBook.Title} User collection");
            var response = await _bookServices.RepleaceBookInCollectionAsync(user.UserId+"4444",book.Isbn, replaceBook.Isbn ,token);
            
            ReportLog.Info("3. Verify replace books response");
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
            var result = (dynamic)JsonConvert.DeserializeObject(response.Content);
            ((int)result["code"]).Should().Be(1207);
            ((string)result["message"]).Should().Be("User Id not correct!");

            ReportLog.Info("4. Delete replaced book in collection");
            await _bookServices.DeleteBookFromCollectionAsync(user.UserId, replaceBook.Isbn, token);
        }
        [Test]
        public async Task RepleaceBookInCollectionWithWrongBookId()
        {
            ReportLog.Info("1. Add book to User collection");
            await _bookServices.AddBookToCollectionAsync(user.UserId, book.Isbn, token);

            ReportLog.Info($"2. Replace {book.Title} by {replaceBook.Title} User collection");
            var response = await _bookServices.RepleaceBookInCollectionAsync(user.UserId,book.Isbn+"444", replaceBook.Isbn ,token);
            
            ReportLog.Info("3. Verify replace books response");
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            var result = (dynamic)JsonConvert.DeserializeObject(response.Content);
            ((int)result["code"]).Should().Be(1206);
            ((string)result["message"]).Should().Be("ISBN supplied is not available in User's Collection!");

            ReportLog.Info("4. Delete replaced book in collection");
            await _bookServices.DeleteBookFromCollectionAsync(user.UserId, replaceBook.Isbn, token);
        }
        [Test]
        public async Task RepleaceBookInCollectionWithTheSameBookId()
        {
            ReportLog.Info("1. Add book to User collection");
            await _bookServices.AddBookToCollectionAsync(user.UserId, book.Isbn, token);

            ReportLog.Info($"2. Replace {book.Title} by {book.Title} User collection");
            var response = await _bookServices.RepleaceBookInCollectionAsync(user.UserId,book.Isbn, book.Isbn ,token);
            
            ReportLog.Info("3. Verify replace books response");
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            var result = (dynamic)JsonConvert.DeserializeObject(response.Content);
            ((int)result["code"]).Should().Be(1206);
            ((string)result["message"]).Should().Be("ISBN supplied is not available in User's Collection!");

            ReportLog.Info("4. Delete replaced book in collection");
            await _bookServices.DeleteBookFromCollectionAsync(user.UserId, book.Isbn, token);
        }
        [Test]
        [TestCase("9781449325862")]
        public async Task RepleaceBookInCollectionWithExistentBookId(string isbn)
        {
            ReportLog.Info("1. Add book to User collection");
            await _bookServices.AddBookToCollectionAsync(user.UserId, book.Isbn, token);
            
            ReportLog.Info($"2. Replace {book.Title} by existent book in User collection");
            var response = await _bookServices.RepleaceBookInCollectionAsync(user.UserId,book.Isbn, isbn ,token);
            
            ReportLog.Info("3. Verify replace books response");
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            var result = (dynamic)JsonConvert.DeserializeObject(response.Content);
            ((int)result["code"]).Should().Be(1206);
            ((string)result["message"]).Should().Be("ISBN supplied is not available in User's Collection!");
            Console.WriteLine(result);

            ReportLog.Info("4. Delete replaced book in collection");
            await _bookServices.DeleteBookFromCollectionAsync(user.UserId, book.Isbn, token);
        }
    }
}