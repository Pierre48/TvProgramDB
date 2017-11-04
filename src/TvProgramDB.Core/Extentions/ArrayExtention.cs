using System;
using System.Collections.Generic;
using System.Text;

namespace TvProgramDB.Core.Extensions
{
    public static class ArrayExtention
    {
        public static void ForEach<T>(this T[] array, Action<T> action)
        {
            foreach (var a in array)
            {
                action((T)a);
            }
        }
    }
}
