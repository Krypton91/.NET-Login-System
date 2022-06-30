using Login_Backend.classes.objects;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Login_Backend.classes
{
    internal class Database
    {
        private Dictionary<string, UserObject> Users = new Dictionary<string, UserObject>();

        internal Database() 
        {
            RegisterUser("MarioRauch", ComputeHash("Test123"));
            RegisterUser("Administrator", ComputeHash("Admin0815"));
        }

        internal bool RegisterUser(string username, string password) 
        {
            if (string.IsNullOrEmpty(username) && string.IsNullOrEmpty(password))
                return false;

            string hashedPassword = ComputeHash(password);

            if (!string.IsNullOrEmpty(hashedPassword)) 
            {
                Users.Add(username, new UserObject(username, password));
                
            }

            return Users.ContainsKey(username);
        }

        internal bool CanLoginToAccount(string username, string password) 
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return false;

            if (Users.ContainsKey(username)) 
            {
                var user = Users[username];
                if(user != null) 
                {
                    return user.Name == username && user.Password == password;
                }
            }

            return false;
        }

        private string ComputeHash(string input)
        {  
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input)); 
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private static Database g_LocalDatabase;
        public static Database GetInstance() 
        {
            if(g_LocalDatabase == null)
                g_LocalDatabase = new Database();
            return g_LocalDatabase;
        }
    }
}