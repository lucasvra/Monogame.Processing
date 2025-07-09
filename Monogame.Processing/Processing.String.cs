using System;
using System.Linq;
using static System.Math;
using System.Text.RegularExpressions;

namespace Monogame.Processing
{
    partial class Processing
    {
        #region String Functions

        public string binary(int value) => Convert.ToString(value, 2);

        public string hex(int value) => Convert.ToString(value, 16);

        public bool boolean(string txt) => bool.Parse(txt);

        public string str(object obj) => obj.ToString();

        public int unhex(string hex) => int.Parse(hex, System.Globalization.NumberStyles.HexNumber);

        public int unbinary(string bin) => Convert.ToInt32(bin, 2);

        public string join(string[] list, string separator) => list.Skip(1).Aggregate(list[0], (str, acc) => acc + separator + str);

        public string[] split(string value, string delim) => value.Split(new[] { delim }, StringSplitOptions.RemoveEmptyEntries);

        public string[] splitTokens(string value, string[] delim) => value.Split(delim, StringSplitOptions.RemoveEmptyEntries);

        public string[] splitTokens(string value) => value.Split(' ', '\n', '\r', '\t', '\f');

        public string[] trim(string[] array) => array.Select(str => str.Trim()).ToArray();

        public string trim(string str) => str.Trim();

        public string nf(double num, int left, int right = 0) => ((int)num).ToString($"D{left}") + ((right == 0) ? "" : "." + (int)(Pow(10, right) * (num - (int)num)));

        public string[] nf(double[] nums, int left, int right = 0) => nums.Select(num => nf(num, left, right)).ToArray();

        public string nfc(double num, int right) => num.ToString($"N{right}");

        public string[] nfc(double[] nums, int right) => nums.Select(num => nfc(num, right)).ToArray();

        public string nfp(double num, int left, int right = 0) => (num >= 0 ? "+" : "-") + nf(Math.Abs(num), left, right);

        public string[] nfp(double[] nums, int left, int right = 0) => nums.Select(num => nfp(num, left, right)).ToArray();

        public string nfs(double num, int left, int right = 0) => (num >= 0 ? " " : "-") + nf(Math.Abs(num), left, right);

        public string[] nfs(double[] nums, int left, int right = 0) => nums.Select(num => nfs(num, left, right)).ToArray();

        public string[] match(string str, string regexp)
        {
            var matches = Regex.Matches(str, regexp);
            if (matches.Count == 0) return null;

            return Enumerable.Range(0, matches[0].Groups.Count).Select(i => matches[0].Groups[i].Value).ToArray();
        }

        public string[][] matchAll(string str, string regexp)
        {
            var matches = Regex.Matches(str, regexp);
            if (matches.Count == 0) return null;

            return Enumerable.Range(0, matches.Count).Select(i => matches[i]).Select(match =>
                Enumerable.Range(0, match.Groups.Count).Select(i => match.Groups[i].Value).ToArray()).ToArray();
        }

        #endregion
    }
}
