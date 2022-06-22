using System;
using System.Linq;

namespace EasyMacro.Common
{
    public static class HashGen
    {
        private static Random random = new Random();

        public static string RandomHashGen(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

    }
}
