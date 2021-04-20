using System;
using System.Collections.Generic;
using System.Text;

namespace StoringPasswordsH4.DTO
{
    class UserPasswordObject
    {
        public byte[] HashedPassword { get; private set; }
        public byte[] Salt { get; private set; }

        public UserPasswordObject(byte[] hashedPassword, byte[] salt)
        {
            HashedPassword = hashedPassword;

            Salt = salt;
        }

    }
}
