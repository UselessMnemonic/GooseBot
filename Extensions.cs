using System;
using System.Collections.Generic;

namespace GooseBot
{
    static class Extensions
    {
        private static readonly Random random = new Random();
        
        public static T GetRandomElement<T>(this IList<T> list)
        {
            return list[random.Next(list.Count)];
        }
    }
}
