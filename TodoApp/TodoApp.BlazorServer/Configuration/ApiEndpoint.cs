namespace TodoApp.BlazorServer.Configuration
{
    public static class ApiEndpoint
    {
        public static string AutRegisterApiEndpoint = $"api/Account/UserRegister";
        public static string AutLoginApiEndpoint = $"api/Account/UserLogin";
        public static string GetTodosEndpoint = $"api/Todo/GetTodos/";
        public static string GetSingleTodoEndpoint = $"api/Todo/";
        public static string AddTodoEndpoint = $"api/Todo/AddTodo";
        public static string EditTodoEndpoint = $"api/Todo/";
        public static string DeleteTodoEndpoint = $"api/Todo/";
    }
}
