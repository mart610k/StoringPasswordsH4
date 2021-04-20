using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using StoringPasswordsH4.DTO;
using System.Linq;
using MySql.Data.MySqlClient;



namespace StoringPasswordsH4.Service
{
    class PasswordService
    {
        RNGCryptoServiceProvider rngSecure = new RNGCryptoServiceProvider();
        SHA256 encrypter = SHA256.Create();
        MysqlConfigurationObject mysql;
        int WorkCount { get; set; }

        public PasswordService()
        {
            mysql = FileService.GetDatabaseConfig();

            WorkCount = 1000;
        }

        public bool CreateUserAndPassword(string userId, UserPasswordObject userPassword)
        {
            MySqlConnection conn = new MySqlConnection(mysql.ConnectionString);

            MySqlCommand comm = conn.CreateCommand();

            bool success = false;

            try
            {
                conn.Open();
                comm.CommandText = "INSERT INTO userpassword(ID,Password,Salt) value (@ID,@Password,@Salt);";


                comm.Parameters.AddWithValue("@ID", userId);
                comm.Parameters.AddWithValue("@Password", Convert.ToBase64String(userPassword.HashedPassword));
                comm.Parameters.AddWithValue("@Salt", Convert.ToBase64String(userPassword.Salt));


                comm.ExecuteNonQuery();
                success = true;
            }
            catch
            {

            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return success;
        }


        public UserPasswordObject HashPassword(string password, byte[] salt = null)
        {
            int saltSize = 16;
            if(salt == null)
            {
                salt = GenerateSalt(saltSize);
            }

            byte[] hashedBytes;

            using (Rfc2898DeriveBytes hashGenerator = new Rfc2898DeriveBytes(password, salt))
            {
                hashGenerator.IterationCount = WorkCount;
                hashedBytes = hashGenerator.GetBytes(64);
            }

            return new UserPasswordObject(hashedBytes, salt);
        }

        private byte[] GenerateSalt(int saltSize)
        {
            byte[] saltBytes = new byte[saltSize];

            rngSecure.GetBytes(saltBytes);

            return saltBytes;
        }


        public bool VerifyPassword(string plainPassword,UserPasswordObject passwordObject)
        {
            if(passwordObject.HashedPassword.Length == HashPassword(plainPassword, passwordObject.Salt).HashedPassword.Length &&
                passwordObject.HashedPassword.SequenceEqual(HashPassword(plainPassword, passwordObject.Salt).HashedPassword))
            {
                return true;
            }
            return false;
        }

        public bool CheckUserCredentials(string userID, string plainPass)
        {
            UserPasswordObject userPasswordObject = GetUserPassword(userID);

            if (userPasswordObject == null)
            {
                return false;
            }
            bool verifyPasswordResult = VerifyPassword(plainPass, userPasswordObject);

            if (verifyPasswordResult)
            {
                return true;
            }
            else
            {
                return false;
            }
                   
        }


        public UserPasswordObject GetUserPassword(string userId)
        {
            MySqlConnection conn = new MySqlConnection(mysql.ConnectionString);

            MySqlCommand comm = conn.CreateCommand();

            UserPasswordObject userPasswordObject = null;

            try
            {
                conn.Open();

                comm.CommandText = "SELECT Password,Salt FROM userpassword WHERE ID = @ID LIMIT 1;";


                comm.Parameters.AddWithValue("@ID", userId);

                MySqlDataReader reader = comm.ExecuteReader();

                while(reader.Read()){
                    userPasswordObject = new UserPasswordObject(
                        Convert.FromBase64String(reader.GetString("Password")),
                        Convert.FromBase64String(reader.GetString("Salt"))
                        );
                }
                

            }
            catch
            {

            }
            finally
            {
                if(conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return userPasswordObject;
        }



    }
}
