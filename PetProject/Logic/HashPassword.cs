namespace Project.DBExchange
{
    internal static class HashPassword
    {
        internal static String Get_Hash(String Password)
        {
            return BCrypt.Net.BCrypt.HashPassword(Password);
        }

        internal static Boolean Verify_Password(String Password, String PasswordHash)
        {
            return BCrypt.Net.BCrypt.Verify(Password, PasswordHash);

        }
    }
}
