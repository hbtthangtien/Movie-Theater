using System.Text.RegularExpressions;

namespace WebAPI.Services.Impl
{
    public class ValidServiceImpl : IValidService
    {
        // user's age must be larger than 13
        public bool IsAgeValid(DateOnly age)
        {
            var now = DateOnly.FromDateTime(DateTime.Now);
            return now.Year - age.Year > 13;
        }

        // email matches one or more alphanumeric characters before the @ symbol. and have at least 1 domain
        public bool IsEmailValid(string email)
        {
            var pattern = "^[A-Za-z0-9]+@[A-Za-z0-9]+\\.[A-Za-z0-9]+(\\.[A-Za-z0-9]+)*$";
            return Regex.IsMatch(email,pattern);
        }
        // to check password must be between 8 and 31 character and must be alphanumeric and contain at least 1 special character
        public bool IsPasswordValid(string password)
        {
            var pattern = "^(?=.*[a-zA-Z])(?=.*\\d)(?=.*[@$!%*?&])[a-zA-Z\\d@$!%*?&]{8,31}$";
            return Regex.IsMatch(password,pattern);
        }
        // username must be alphanumeric and doesn't contain a speacial character
        public bool IsUsernameValid(string username)
        {
            var pattern = "^[a-zA-Z][a-zA-Z0-9]*$";
            return Regex.IsMatch(username,pattern);
        }
    }
}
