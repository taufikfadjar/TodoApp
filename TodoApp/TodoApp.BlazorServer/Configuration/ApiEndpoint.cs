namespace TodoApp.BlazorServer.Configuration
{
    public static class ApiEndpoint
    {
        public static string BaseUrl = "https://localhost:7132";
        public static string AutRegisterApiEndpoint = $"{BaseUrl}/api/Account/UserRegister";
        public static string AutLoginApiEndpoint = $"{BaseUrl}/api/Account/UserLogin";
    }
}
