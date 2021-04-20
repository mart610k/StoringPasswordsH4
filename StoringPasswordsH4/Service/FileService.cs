using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using StoringPasswordsH4.DTO;

namespace StoringPasswordsH4.Service
{

    class FileService
    {

        /// <summary>
        /// Pulls the database config from the file system to be used for the authentication towards the database
        /// </summary>
        /// <returns>the object with infomation the required data values in a connection string</returns>
        public static MysqlConfigurationObject GetDatabaseConfig()
        {
            string workingDirectory = Directory.GetCurrentDirectory();
            //TODO: Check if the file actually exists.... (Can throw exception)
            return JsonConvert.DeserializeObject<MysqlConfigurationObject>(File.ReadAllText(workingDirectory + @"\config.json"));

        }
    }
}
