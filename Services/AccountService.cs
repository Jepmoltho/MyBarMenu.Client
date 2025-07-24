//using MyBarMenu.Client.DTOs;
//using MyBarMenu.Client.Services.Interfaces;
//using Blazored.LocalStorage;

//namespace MyBarMenu.Client.Services;

//public class AccountService : IAccountService
//{
//    private readonly HttpClient _httpClient;
//    private readonly ILocalStorageService _localStorageService;

//    public AccountService(HttpClient httpClient, ILocalStorageService localStorageService)
//    {
//        _httpClient = httpClient;
//        _localStorageService = localStorageService;
//    }

//    public async Task<IEnumerable<UserDTO>> GetUsers(string token)
//    {
//        //var token = await _localStorageService.GetItemAsync<string>("authToken");

//        if (string.IsNullOrEmpty(token))
//        {
//            return new List<UserDTO>();
//        }

//        var request = new HttpRequestMessage(HttpMethod.Get, "users");
//        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
//        var response = await _httpClient.SendAsync(request);

//        //var response = await _httpClient.GetAsync("users");

//        if (response.IsSuccessStatusCode)
//        {
//            var users = await response.Content.ReadFromJsonAsync<List<UserDTO>>();

//            return users ?? new List<UserDTO>();
//        }

//        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
//        {
//            throw new UnauthorizedAccessException();
//            //return new List<UserDTO>(); //ENDS IN HERE.
//        }

//        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
//        {
//            return new List<UserDTO>();
//        }

//        return new List<UserDTO>();
//        //throw new Exception($"Failed to fetch users. Status code: {response.StatusCode}");
//    }

//    //public async Task<UserResult> SignInWithGoogle()
//    //{
//    //    var result = await _httpClient.GetFromJsonAsync<UserResult>("user/auth");

//    //    if (result is not null && result.Success)
//    //    {
//    //        return result;
//    //    }

//    //    return new UserResult
//    //    {
//    //        Success = false,
//    //        Message = "No user returned from API",
//    //        authToken = string.Empty,
//    //        Id = Guid.Empty,
//    //    };
//    //}
//}
