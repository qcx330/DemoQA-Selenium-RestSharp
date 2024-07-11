using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.API;
using DemoQA.Service.Model.Request;
using DemoQA.Service.Model.Response;
using RestSharp;

namespace DemoQA.Service.Service
{
    public class BookService
    {
        private readonly APIClient _client;
        public BookService(APIClient client){
            _client = client;
        }
        public async Task<RestResponse<AddBookToCollectionResponseDto>> AddBookToCollectionAsync(string userId,string bookId, string token)
        {
            var requestBody = new AddBookToCollectionRequestDto(){
                UserId = userId,
                CollectionOfIsbns = new List<BookRequestDto>
                {
                    new BookRequestDto { Isbn = bookId }
                }
            };
            return await _client.CreateRequest(String.Format(APIConstant.AddBookToCollectionEndPoint, userId, bookId))
                .AddContentTypeHeader("application/json")
                .AddHeader("accept","application/json")
                .AddHeaderBearerToken(token)
                .AddBody(requestBody)
                .ExecutePostAsync<AddBookToCollectionResponseDto>();
        }
        public async Task<RestResponse> DeleteBookFromCollectionAsync(string userId,string bookId, string token)
        {
            var requestBody = new DeleteBookFromCollectionRequestDto(){
                UserId = userId,
                Isbn = bookId
            };
            return await _client.CreateRequest(String.Format(APIConstant.DeleteBookFromCollectionEndPoint))
                .AddContentTypeHeader("application/json")
                .AddHeader("accept","application/json")
                .AddHeaderBearerToken(token)
                .AddBody(requestBody)
                .ExecuteDeleteAsync();
        }
        public async Task<RestResponse<ReplaceBookInCollectionResponseDto>> RepleaceBookInCollectionAsync(string userId,string bookId, string replaceBookId, string token)
        {
            var requestBody = new ReplaceBookInCollectionRequestDto(){
                UserId = userId,
                Isbn = replaceBookId
            };
            return await _client.CreateRequest(String.Format(APIConstant.ReplaceBookInCollectionEndPoint, bookId))
                .AddContentTypeHeader("application/json")
                .AddHeader("accept","application/json")
                .AddHeaderBearerToken(token)
                .AddBody(requestBody)
                .ExecutePutAsync<ReplaceBookInCollectionResponseDto>();
        }
    }
}