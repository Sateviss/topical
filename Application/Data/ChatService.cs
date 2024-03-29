using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Util;
using Microsoft.Extensions.Logging;

namespace Application.Data
{
    public class ChatService
    {
        private readonly Dictionary<Guid, Chat> _chats = new Dictionary<Guid, Chat>();
        private readonly long _timeAfterUserOld = 10000;

        private readonly ConcurrentDictionary<Guid, (User, IList<string>)> _usersHaveNoPair =
            new ConcurrentDictionary<Guid, (User, IList<string>)>();

        private readonly Dictionary<Guid, List<Chat>> _userChats = new Dictionary<Guid, List<Chat>>();
        private ILogger<ChatService> _logger;

        private Chat GenerateChat(IEnumerable<string> topics)
        {
            var chat = new Chat(topics);
            _chats.Add(chat.ChatId, chat);
            return chat;
        }

        private IList<string> CrossTopics(IList<string> first, IList<string> second)
        {
            HashSet<string> topics = new HashSet<string>(first);
            List<string> result = new List<string>();
            
            foreach (var element in second)
            {
                if (topics.Contains(element))
                {
                    result.Add(element);
                }
            }

            return result;
        }

        private void MatchUserWithSomeone(User user, IList<string> topics, long created)
        {
            while (true)
            {
                while (!_usersHaveNoPair.TryAdd(user.Id, (user, topics)))
                {
                }

                var category = (DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - created > _timeAfterUserOld);

                IList<string> maxMatch = new List<string>();
                User second = null;

                foreach (var element in _usersHaveNoPair.Values)
                {
                    if (element.Item1.Id.Equals(user.Id))
                    {
                        continue;
                    }

                    var countMatch = CrossTopics(topics, element.Item2);
                    if (countMatch.Count > maxMatch.Count)
                    {
                        second = element.Item1;
                        maxMatch = countMatch;
                    }
                }

                if (!_usersHaveNoPair.ContainsKey(user.Id))
                {
                    break;
                }

                _usersHaveNoPair.Remove(user.Id, out _);
                if (second != null && (category || topics.Count == 1 || (!category && maxMatch.Count > 1)) &&
                    _usersHaveNoPair.ContainsKey(second.Id))
                {
                    _usersHaveNoPair.Remove(second.Id, out _);
                    var chat = GenerateChat(maxMatch);
                    _userChats[user.Id].Add(chat);
                    _userChats[second.Id].Add(chat);
                    chat.Users.Add(user);
                    chat.Users.Add(second);
                    user.Updated();
                    second.Updated();
                    _logger.LogInformation("Created chat for " + user.Name + " and " + second.Name);
                    break;
                }
            }
        }

        public ChatService(ILogger<ChatService> logger)
        {
            _logger = logger;
        }

        public async void AnnounceTopics(User sender, IList<string> topics)
        {
            if (!_userChats.ContainsKey(sender.Id))
            {
                _userChats.Add(sender.Id, new List<Chat>());
            }
            await Task.Run(() => MatchUserWithSomeone(sender, topics, DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()));
        }


        public IList<Chat> RequestChats(User sender)
        {
            return _userChats[sender.Id];
        }

        public Chat GetChat(User sender, Guid chatId)
        {
            return _chats[chatId].Users.Any(u => u.Id == sender.Id) ? _chats[chatId] : null;
        }

        public ChatService()
        {
            var chat1 = new Chat();
            _chats.Add(chat1.ChatId, chat1);
        }
    }
}