using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LANPartyAPI_Utils
{
    public static class Utils
    {
        public static class Roles
        {
            public const string Admin = "Admin";
            public const string Joueur = "Joueur";
        }
        public static string GetAllWritableCharacters(Encoding encoding)
        {
            encoding = Encoding.GetEncoding(encoding.WebName, new EncoderExceptionFallback(), new DecoderExceptionFallback());
            var sb = new StringBuilder();

            char[] chars = new char[1];
            byte[] bytes = new byte[16];


            for (int i = 20; i <= char.MaxValue; i++)
            {
                chars[0] = (char)i;
                try
                {
                    int count = encoding.GetBytes(chars, 0, 1, bytes, 0);

                    if (count != 0)
                    {
                        sb.Append(chars[0]);
                    }
                }
                catch
                {
                    break;
                }
            }
            return sb.ToString();
        }
    }
}
