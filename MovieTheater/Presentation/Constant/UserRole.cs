namespace WebAPI.Constant
{
    public static class UserRole
    {
        public const string ADMIN = "admin";
        public const string MANAGER = "manager";
        public const string EMPLOYEE = "Employee";
        public const string MEMBER = "Member";
        public static readonly string[] ALL = { ADMIN, MANAGER, EMPLOYEE, MEMBER };

    }
}
