using StoringPasswordsH4.DTO;
using StoringPasswordsH4.Service;
using System;
using System.Threading;

namespace StoringPasswordsH4
{
    class Program
    {
        static PasswordService passwordService = new PasswordService();

        static void Main(string[] args)
        {
            

            while (true)
            {
                int option = GetRegisterAndLoginOption();
                switch (option)
                {
                    case 1:
                        //Register User
                        RegisterUserGUI();
                        break;
                    case 2:
                        //Login User
                        LoginUserGUI();
                        break;
                    default:
                        break;
                }


            }
        }
        static int GetRegisterAndLoginOption()
        {
            bool inputAccepted = false;
            int chosen = -1;
            while (!inputAccepted)
            {
                Console.WriteLine("1. Register");
                Console.WriteLine("2. Login");
                Console.WriteLine("3. Back");
                Console.Write("Type in an option:");
                string optionChosen = Console.ReadLine();
                try
                {
                    int optionChosenInt = int.Parse(optionChosen);

                    if(optionChosenInt > 0 && optionChosenInt < 4)
                    {
                        chosen = optionChosenInt;
                        inputAccepted = true;
                    }
                }
                catch
                {

                }
            }

            return chosen;
            
        }

        static void RegisterUserGUI()
        {
            Console.Write("Type in your UserName:");
            string userName = Console.ReadLine();

            Console.Write("Type in your password:");
            string password = Console.ReadLine();

            Console.Write("Type in your password again:");
            string passwordSecondtime = Console.ReadLine();

            if(password == passwordSecondtime)
            {
                if(passwordService.RegisterUser(userName, password))
                {
                    Console.WriteLine("Sucessfully registered your user.");
                }
                else
                {
                    Console.WriteLine("your user was not registered");
                }
            }
            else
            {
                Console.WriteLine("Passwords did not match try again");
            }

        }

        static void LoginUserGUI()
        {
            int maxTries = 5;
            int currentTries = 0;
            while (currentTries < maxTries)
            {
                Console.Write("Type in your username:");
                string username= Console.ReadLine();

                Console.Write("password:");
                string password = Console.ReadLine();
                bool result = passwordService.CheckUserCredentials(username, password);
                if (!result)
                {
                    currentTries++;
                }
                else
                {
                    Console.WriteLine("you have now logged in");
                    break;
                }

                
            }
            if(maxTries == currentTries)
            {
                Console.WriteLine("To many tries wait a bit to try again");
                Thread.Sleep(5000);
            }
        }
    }
}
