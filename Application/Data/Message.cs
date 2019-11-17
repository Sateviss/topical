using System;

namespace Application.Data
{
    public class Message
    {
        public User sender { get; }
        
        public string message { get; }
        
        public DateTime createdOn { get; }

        public Message(string message, User sender)
        {
            createdOn = DateTime.Now;
            this.message = message;
            this.sender = sender;
        }
    }
}