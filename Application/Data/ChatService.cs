using System;
using System.Collections.Generic;
using System.Linq;
using Application.Util;

namespace Application.Data
{
    public class ChatService
    {
        private readonly Dictionary<Guid, Chat> _chats = new Dictionary<Guid, Chat>();

        private Chat GenerateChat(IEnumerable<string> topics)
        {
            var chat = new Chat(topics);
            _chats.Add(chat.ChatId, chat);
            return chat;
        }

        public IList<(string, Guid)> RequestChats(IEnumerable<string> topics)
        {
            var matchedChats = ChatMatcher.MatchChats(_chats.Values, topics);
            if (matchedChats.Count == 0)
            {
                var chat = GenerateChat(topics);
                matchedChats.Add((chat.Title, chat.ChatId));
            }

            return matchedChats;
        }

        public Chat GetChat(Guid chatId) => _chats[chatId];

        public ChatService()
        {
            var chat1 = new Chat();
            _chats.Add(chat1.ChatId, chat1);
        }
    }
}