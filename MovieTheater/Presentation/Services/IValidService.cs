namespace WebAPI.Services
{
    public interface IValidService
    {
        public bool IsPasswordValid(string password);

        public bool IsUsernameValid(string username);

        public bool IsEmailValid(string email);

        public bool IsAgeValid(DateOnly age);
    }
}
