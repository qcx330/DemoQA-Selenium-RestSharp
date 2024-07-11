using MongoDB.Bson.IO;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Authenticators.OAuth2;
using RestSharp.Serializers.NewtonsoftJson;

namespace Core.API
{
    public class APIClient
    {
        private readonly RestClient _client;
        public RestRequest Request;
        private RestClientOptions requestOptions;
        public APIClient(RestClient client){
            _client = client;
            Request = new RestRequest(); 
        }
        public APIClient(string url){
            var options = new RestClientOptions(url);
            _client = new RestClient(options, configureSerialization: s => s.UseNewtonsoftJson());
            Request = new RestRequest();
        }
        public APIClient(RestClientOptions options){
            _client = new RestClient(options, configureSerialization: s=> s.UseNewtonsoftJson());
            Request = new RestRequest();
        }
        public APIClient SetBasicAuthentication(string username, string password){
            requestOptions.Authenticator = new HttpBasicAuthenticator(username, password);
            return new APIClient(requestOptions);
        }
        public APIClient SetRequestAuthentication(string consumerKey, string consumerSecret ){
            requestOptions.Authenticator = OAuth1Authenticator.ForRequestToken(consumerKey, consumerSecret);
            return new APIClient(requestOptions);
        }
        public APIClient SetAccessTokenAuthentication(string consumerKey, string consumerSecret, string oauthToken, string oauthTokenSecret){
            requestOptions.Authenticator = OAuth1Authenticator.ForAccessToken(consumerKey,consumerSecret, oauthToken, oauthTokenSecret);
            return new APIClient(requestOptions);
        }
        public APIClient SetRequestHeaderAuthentication(string token, string authType = "Bearer"){
            requestOptions.Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(token, authType);
            return new APIClient(requestOptions);
        }
        public APIClient SetJwtAuthenticator(string token){
            requestOptions.Authenticator= new JwtAuthenticator(token);
            return new APIClient(requestOptions);
        }
        public APIClient ClearAuthenticator(){
            requestOptions.Authenticator = null;
            return new APIClient(requestOptions);
        }
        public APIClient AddDefaultHeaders(Dictionary<string, string> headers){
            _client.AddDefaultHeaders(headers);
            return this;
        }
        public APIClient CreateRequest(string source=""){
            Request = new RestRequest(source);
            return this;
        }
        public APIClient AddAuthorizationHeader(string value)
        {
            return AddHeader("Authorization", value);
        }
        public APIClient AddHeader(string name, string value){
            Request.AddHeader(name, value);
            return this;
        }
        public APIClient AddHeaderBearerToken(string token)
        {
            return AddHeader("Authorization", $"Bearer {token}");
        }
        public APIClient AddContentTypeHeader(string value){
            return AddHeader("Content-Type", value);
        }
        public APIClient AddParameter(string name,string value){
            Request.AddParameter(name, value);
            return this;
        }
        public APIClient AddBody(object obj,string contentType = null){
            Request.AddJsonBody(obj, contentType);
            return this;
        }
        public async Task<RestResponse> ExecuteGetAsync(){
            return await _client.ExecuteGetAsync(Request);
        }
        public async Task<RestResponse<T>> ExecuteGetAsync<T>(){
            return await _client.ExecuteGetAsync<T>(Request);
        }
        public async Task<RestResponse> ExecutePostAsync(){
            return await _client.ExecutePostAsync(Request);
        }
        public async Task<RestResponse<T>> ExecutePostAsync<T>(){
            return await _client.ExecutePostAsync<T>(Request);
        }
        public  RestResponse<T> ExecutePost<T>(){
            return  _client.ExecutePost<T>(Request);
        }
        public async Task<RestResponse> ExecutePutAsync(){
            return await _client.ExecutePutAsync(Request);
        }
        public async Task<RestResponse<T>> ExecutePutAsync<T>(){
            return await _client.ExecutePutAsync<T>(Request);
        }
        public async Task<RestResponse> ExecuteDeleteAsync(){
            return await _client.ExecuteDeleteAsync(Request);
        }
        public async Task<RestResponse<T>> ExecuteDeleteAsync<T>(){
            return await _client.ExecuteDeleteAsync<T>(Request);
        }
    }
}