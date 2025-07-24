/*
1.  User signs in with google on @page "/login"
2.  User is redirected to @page "/login-success with a token parameter in the URL
3.  The token is stored in local storage by AuthService.LoginAsync 
3.1 AuthService.LoginAsync calls CustomAuthStateProvider.NotifyUserAuthenticationAsync
4.  On a page level, the user is authenticated by calling CustomAuthStateProvider.GetAuthenticationStateAsync 
5.  On request level, the users request are authenticated by attatching the token to the request headers
*/