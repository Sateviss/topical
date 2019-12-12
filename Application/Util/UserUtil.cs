using System.Security.Cryptography;
using System.Text;
using Application.Data;

namespace Application.Util
{
    public class UserUtil
    {
        private static readonly SHA256 _sha = SHA256.Create();
        
        public static int UserToHue(User user)
        {
            var hash = _sha.ComputeHash(Encoding.UTF8.GetBytes(user.Code + user.Name));
            return (int)((hash[0]*256+hash[1])*356f/65536f);
        }
    }
}