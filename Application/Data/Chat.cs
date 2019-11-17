using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace Application.Data
{
    public class Chat
    {
        public Guid ChatId { get; } = Guid.NewGuid();

        public string Title { get; } = Util.NameGenerator.NewChatTitle();
        public List<User> Users { get; } = new List<User>(); 

        private SortedList<DateTime, Message> _messages = new SortedList<DateTime, Message>();
        private List<Action> _callbacks = new List<Action>();
        public IList<Message> Messages => _messages.Values;

        public event EventHandler<EventArgs> NewMessage; 
        
        public void AddMessage(Message message)
        {
            _messages.Add(message.createdOn, message);
            NewMessage?.Invoke(this, EventArgs.Empty);
        }
    }
}