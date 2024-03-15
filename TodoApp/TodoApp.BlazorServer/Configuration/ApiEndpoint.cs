namespace TodoApp.BlazorServer.Configuration
{
    public static class ApiEndpoint
    {
        public static string BaseUrl = "https://localhost:7132/";
        public static string AutRegisterApiEndpoint = $"{BaseUrl}api/Account/UserRegister";
        public static string AutLoginApiEndpoint = $"{BaseUrl}api/Account/UserLogin";
        public static string GetTodosEndpoint = $"{BaseUrl}api/Todo/GetTodos/";
        public static string GetSingleTodoEndpoint = $"{BaseUrl}api/Todo/";
        public static string AddTodoEndpoint = $"{BaseUrl}api/Todo/AddTodo";
        public static string EditTodoEndpoint = $"{BaseUrl}api/Todo/";
        public static string DeleteTodoEndpoint = $"{BaseUrl}api/Todo/";
    }
}
