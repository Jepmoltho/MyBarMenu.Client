namespace MyBarMenu.Client.Providers.Interfaces;

public interface ITokenProvider
{
    string GetToken();

    void SetToken(string token);

    void ClearToken();
}
