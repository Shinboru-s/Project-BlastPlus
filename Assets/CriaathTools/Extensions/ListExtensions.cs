using System.Collections.Generic;
using UnityEngine;

namespace Criaath.Extensions
{
    public static class ListExtensions
    {
        public static List<T> Shuffle<T>(this List<T> list)
        {
            int count = list.Count;
            List<T> result = new List<T>();
            List<int> indexList = new List<int>();

            for (int i = 0; i < count; i++)
            {
                indexList.Add(i);
            }

            while (count > 0)
            {
                int index = indexList[Random.Range(0, indexList.Count)];

                result.Add(list[index]);

                indexList.Remove(index);

                count--;
            }

            return result;
        }

        public static bool IsNullOrEmpty<T>(this List<T> list)
        {
            if (list == null) return true;
            if (list.Count == 0) return true;

            return false;
        }
    }
}
