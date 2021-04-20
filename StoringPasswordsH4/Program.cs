using StoringPasswordsH4.DTO;
using StoringPasswordsH4.Service;
using System;

namespace StoringPasswordsH4
{
    class Program
    {
        static void Main(string[] args)
        {
            PasswordService password = new PasswordService();
            UserPasswordObject passwordObject = password.HashPassword("Test");
            Console.WriteLine(Convert.ToBase64String(passwordObject.HashedPassword));

            Console.WriteLine(password.CheckUserCredentials("User", "Test"));

            Console.WriteLine("Hello World!");
        }
    }
}
