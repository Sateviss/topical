using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using Application.Util;

namespace Application.Data
{
    public class User
    {
        public delegate void UserChatPairCreated();
        public event UserChatPairCreated Notify;
        public Guid Id { get; } = Guid.NewGuid();
        public string Name { get; }
        public string Code { get; }
        
        public IList<Guid> Chats { get; }
        
        public bool LoggedIn { get; } 

        public void Updated()
        {
            Notify.Invoke();
        }
        
        public User(string name, string password, bool loggedIn)
        {
            LoggedIn = loggedIn;
            if (loggedIn)
            {
                var arr = new byte[16]; 
                Array.Copy(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(name)), arr, 16);
                Id = new Guid(arr);
            }

            Name = (name == null || name != "") ? name : NameGenerator.NewUserName();
            if (password == null || password != "")
            {
                var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(password));
                hmac.Initialize();
                var hmacHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(name));
                Code = string.Concat(Array.ConvertAll(hmacHash, x => x.ToString("X2"))).Substring(0, 8);
            }
            else
            {
                Code = "";
            }
        }
    }
}
