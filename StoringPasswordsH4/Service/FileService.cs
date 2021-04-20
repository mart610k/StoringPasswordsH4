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

        public static MysqlConfigurationObject GetDatabaseConfig()
        {
            string workingDirectory = Directory.GetCurrentDirectory();
            //TODO: Check if the file actually exists.... (Can throw exception)
            return JsonConvert.DeserializeObject<MysqlConfigurationObject>(File.ReadAllText(workingDirectory + @"\config.json"));

        }
    }
}
