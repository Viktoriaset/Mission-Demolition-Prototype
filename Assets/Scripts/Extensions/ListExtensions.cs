using System;
using System.Collections.Generic;

namespace Assets.Scripts.Extensions
{
    public static class ListExtensions
    {
        private static Random rnd = new Random();
        public static T GetRandom<T>(this List<T> list)
        {
            return list[rnd.Next(list.Count)];
        }
    }
}
