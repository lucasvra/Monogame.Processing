using System;
using System.Linq;

namespace Monogame.Processing
{
    partial class Processing
    {
        #region Array Functions

        public T[] append<T>(T[] array, T value) => array.Append(value).ToArray();

        public void arrayCopy<T>(T[] src, int srcPosition, T[] dst, int dstPosition, int length) =>
            Array.ConstrainedCopy(src, srcPosition, dst, dstPosition, length);

        public void arrayCopy<T>(T[] src, T[] dst, int length) => arrayCopy(src, 0, dst, 0, length);
        public void arrayCopy<T>(T[] src, T[] dst) => arrayCopy(src, 0, dst, 0, src.Length);

        public T[] concat<T>(T[] a, T[] b) => a.Concat(b).ToArray();

        public T[] expand<T>(T[] list, int newSize = -1)
        {
            newSize = newSize < 0 ? list.Length * 2 : newSize;

            var newList = new T[newSize];
            list.CopyTo(newList, 0);
            return newList;
        }

        public T[] reverse<T>(T[] list) => list.Reverse().ToArray();
        public T[] shorten<T>(T[] list) => list.Take(list.Length - 1).ToArray();
        public T[] sort<T>(T[] list, int count) => list.Take(count).OrderBy(e => e).Concat(list.Skip(count)).ToArray();
        public T[] sort<T>(T[] list) => list.OrderBy(e => e).ToArray();

        public T[] splice<T>(T[] list, T[] value, int index) => list.Take(index).Concat(value).Concat(list.Skip(index)).ToArray();

        public T[] subset<T>(T[] list, int start, int count) => list.Skip(start).Take(count).ToArray();
        public T[] subset<T>(T[] list, int start) => list.Skip(start).ToArray();

        #endregion
    }
}
