using System;
using System.Collections.Generic;
using System.Linq;

namespace Criaath.Extensions
{
    public static class ArrayExtensions
    {
        public static bool IsNullOrEmpty<T>(this T[] array)
        {
            if (array == null) return true;
            if (array.Length == 0) return true;

            return false;
        }

        public static T[] Expand<T>(this T[] array, T[] values)
        {
            if (values.IsNullOrEmpty()) return array;

            if (array == null)
            {
                return values;
            }

            int newArrayCount = array.Length + values.Length;

            T[] newArray = new T[newArrayCount];
            int index = 0;

            for (int i = 0; i < array.Length; i++)
            {
                newArray[index] = array[i];
                index++;
            }

            for (int i = 0; i < values.Length; i++)
            {
                newArray[index] = values[i];
                index++;
            }

            return newArray;
        }

        public static T[] Remove<T>(this T[] array, T value)
        {
            if (array.IsNullOrEmpty()) return array;
            if (value == null) return array;
            if (!array.Contains(value)) return array;

            if (array.Length == 1) return new T[0];

            int newArrayLength = array.Length - 1;
            int index = 0;
            T[] newArray = new T[newArrayLength];

            for (int i = 0; i < array.Length; i++)
            {
                if (array[i].Equals(value)) continue;

                newArray[index] = array[i];
                index++;
            }

            return newArray;
        }

        public static T[] Shuffle<T>(this T[] list)
        {
            int count = list.Length;
            T[] result = new T[count];
            List<int> indexList = new List<int>();

            for (int i = 0; i < count; i++)
            {
                indexList.Add(i);
            }

            while (count > 0)
            {
                count--;

                int index = indexList[UnityEngine.Random.Range(0, indexList.Count)];

                result[count] = list[index];

                indexList.Remove(index);

            }

            return result;
        }

        public static T[] Add<T>(this T[] array, T value)
        {
            if (array == null)
            {
                return array;
            }

            if (value == null)
            {
                return array;
            }

            int newArrayLength = array.Length + 1;
            int baseArrayIndex = 0;
            T[] newArray = new T[newArrayLength];

            for (int i = 0; i < newArrayLength; i++)
            {
                if (i != newArrayLength)
                {
                    newArray[i] = array[baseArrayIndex];
                    baseArrayIndex++;
                }
                else
                {
                    newArray[i] = value;
                }
            }

            return newArray;
        }
    }
}
