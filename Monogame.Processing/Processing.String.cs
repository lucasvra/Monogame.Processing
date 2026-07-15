using System;
using System.Linq;
using static System.Math;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Monogame.Processing
{
    partial class Processing
    {
        #region String Functions

        public string binary(int value) => Convert.ToString(value, 2);

        public string binary(byte value) => Convert.ToString(value, 2).PadLeft(8, '0');

        public string binary(char value) => Convert.ToString(value, 2).PadLeft(16, '0');

        public string binary(color value) => Convert.ToString((long)ToArgb(value), 2).PadLeft(32, '0');

        public string hex(int value) => Convert.ToString(value, 16);

        public string hex(byte value) => value.ToString("X2", CultureInfo.InvariantCulture);

        public string hex(char value) => ((int)value).ToString("X4", CultureInfo.InvariantCulture);

        public string hex(color value) => ToArgb(value).ToString("X8", CultureInfo.InvariantCulture);

        public bool boolean(string txt) => bool.Parse(txt);

        public bool boolean(int value) => value != 0;

        public byte parseByte(bool value) => value ? (byte)1 : (byte)0;

        public byte parseByte(byte value) => value;

        public byte parseByte(sbyte value) => unchecked((byte)value);

        public byte parseByte(char value) => unchecked((byte)value);

        public byte parseByte(short value) => unchecked((byte)value);

        public byte parseByte(ushort value) => unchecked((byte)value);

        public byte parseByte(int value) => unchecked((byte)value);

        public byte parseByte(uint value) => unchecked((byte)value);

        public byte parseByte(long value) => unchecked((byte)value);

        public byte parseByte(ulong value) => unchecked((byte)value);

        public byte parseByte(float value) => unchecked((byte)value);

        public byte parseByte(double value) => unchecked((byte)value);

        public byte parseByte(decimal value) => unchecked((byte)value);

        public byte parseByte(string value) => byte.Parse(value, CultureInfo.InvariantCulture);

        public byte @byte(bool value) => parseByte(value);

        public byte @byte(byte value) => parseByte(value);

        public byte @byte(sbyte value) => parseByte(value);

        public byte @byte(char value) => parseByte(value);

        public byte @byte(short value) => parseByte(value);

        public byte @byte(ushort value) => parseByte(value);

        public byte @byte(int value) => parseByte(value);

        public byte @byte(uint value) => parseByte(value);

        public byte @byte(long value) => parseByte(value);

        public byte @byte(ulong value) => parseByte(value);

        public byte @byte(float value) => parseByte(value);

        public byte @byte(double value) => parseByte(value);

        public byte @byte(decimal value) => parseByte(value);

        public byte @byte(string value) => parseByte(value);

        public char parseChar(byte value) => (char)value;

        public char parseChar(char value) => value;

        public char parseChar(int value) => (char)value;

        public char parseChar(string value) => string.IsNullOrEmpty(value) ? '\0' : value[0];

        public char @char(byte value) => parseChar(value);

        public char @char(char value) => parseChar(value);

        public char @char(int value) => parseChar(value);

        public char @char(string value) => parseChar(value);

        public int parseInt(bool value) => value ? 1 : 0;

        public int parseInt(byte value) => value;

        public int parseInt(sbyte value) => value;

        public int parseInt(char value) => value;

        public int parseInt(short value) => value;

        public int parseInt(ushort value) => value;

        public int parseInt(long value) => unchecked((int)value);

        public int parseInt(float value) => (int)value;

        public int parseInt(double value) => (int)value;

        public int parseInt(decimal value) => (int)value;

        public int parseInt(string value) => int.Parse(value, CultureInfo.InvariantCulture);

        public int @int(bool value) => parseInt(value);

        public int @int(byte value) => parseInt(value);

        public int @int(sbyte value) => parseInt(value);

        public int @int(char value) => parseInt(value);

        public int @int(short value) => parseInt(value);

        public int @int(ushort value) => parseInt(value);

        public int @int(long value) => parseInt(value);

        public int @int(float value) => parseInt(value);

        public int @int(double value) => parseInt(value);

        public int @int(decimal value) => parseInt(value);

        public int @int(string value) => parseInt(value);

        public float parseFloat(bool value) => value ? 1f : 0f;

        public float parseFloat(byte value) => value;

        public float parseFloat(sbyte value) => value;

        public float parseFloat(char value) => value;

        public float parseFloat(short value) => value;

        public float parseFloat(ushort value) => value;

        public float parseFloat(int value) => value;

        public float parseFloat(uint value) => value;

        public float parseFloat(long value) => value;

        public float parseFloat(ulong value) => value;

        public float parseFloat(double value) => (float)value;

        public float parseFloat(decimal value) => (float)value;

        public float parseFloat(string value) => float.Parse(value, CultureInfo.InvariantCulture);

        public float @float(bool value) => parseFloat(value);

        public float @float(byte value) => parseFloat(value);

        public float @float(sbyte value) => parseFloat(value);

        public float @float(char value) => parseFloat(value);

        public float @float(short value) => parseFloat(value);

        public float @float(ushort value) => parseFloat(value);

        public float @float(int value) => parseFloat(value);

        public float @float(uint value) => parseFloat(value);

        public float @float(long value) => parseFloat(value);

        public float @float(ulong value) => parseFloat(value);

        public float @float(double value) => parseFloat(value);

        public float @float(decimal value) => parseFloat(value);

        public float @float(string value) => parseFloat(value);

        private static uint ToArgb(color value) => (uint)(value.A << 24 | value.R << 16 | value.G << 8 | value.B);

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
