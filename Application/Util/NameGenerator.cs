using System;
using System.Collections.Generic;

namespace Application.Util
{
    public static class NameGenerator
    {
        private static string[] _namePrefixes = {"User", "Anon", "BobSmith", "Interlocutor"};
        private static string[] _chatPrefixes = {"Chat", "Conversation", "Discussion"};
        private static Random _random = new Random();
        
        public static string NewUserName()
        {
            return _namePrefixes[_random.Next(_namePrefixes.Length)] + NewSalt();
        }

        public static string NewChatTitle()
        {
            return _chatPrefixes[_random.Next(_chatPrefixes.Length)] + NewSalt();
        }

        public static string NewSalt()
        {
            return _random.Next(999, 10000).ToString();
        }
    }
}