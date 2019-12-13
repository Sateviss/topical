using System;
using System.Collections;
using System.Collections.Generic;
using Application.Data;

namespace Application.Util
{
    public static class ChatMatcher
    {
        private const double MatchBorder = 0.5;

        public static IList<(string, Guid)> MatchChats(IEnumerable<Chat> chats, IEnumerable<string> topics)
        {
            List<(string, Guid)> result = new List<(string, Guid)>();
            var listTopics = new List<string>(topics);
            
            foreach (var chat in chats)
            {
                var matched = 0;
                foreach (var topic in listTopics)
                {
                    if (chat.Topics.Contains(topic))
                    {
                        matched++;
                    }
                }

                if ((double) matched / listTopics.Count >= 0.5)
                {
                    result.Add((chat.Title, chat.ChatId));
                }
            }

            return result;
        }
    }
}