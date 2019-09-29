using System;

namespace Application.Models
{
    public class Chat
    {
        public long Id { get; set; }
        
        public Tuple<User, User> Users { get; set; }

        public String Topic { get; set; }
    }
}