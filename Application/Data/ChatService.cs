using System;
using System.Collections.Generic;
using System.Linq;

namespace Application.Data
{
    public class ChatService
    {
        private readonly Dictionary<Guid, Chat> _chats = new Dictionary<Guid, Chat>();
        
        public IList<(string, Guid)> RequestChats(IEnumerable<string> topics)
        {
            var returnList = new List<(string, Guid)>();
            _chats.ToList().ForEach(pair => returnList.Add((pair.Value.Title, pair.Key)));
            return returnList;
        }

        public Chat GetChat(Guid chatId) => _chats[chatId];

        public ChatService()
        {
            var chat1 = new Chat();
            _chats.Add(chat1.ChatId, chat1);
        }
    }
}