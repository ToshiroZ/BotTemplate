using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BotTemplate
{
    public static class Extensions
    {
        public static Random rnd = new Random();

        /// <summary>
        /// returns an IEnumerable with randomized element order
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="items"></param>
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> items)
        {
            using var provider = RandomNumberGenerator.Create();
            var list = items.ToList();
            var n = list.Count;
            while (n > 1)
            {
                var box = new byte[(n / Byte.MaxValue) + 1];
                int boxSum;
                do
                {
                    provider.GetBytes(box);
                    boxSum = box.Sum(b => b);
                }
                while (!(boxSum < n * ((Byte.MaxValue * box.Length) / n)));
                var k = (boxSum % n);
                n--;
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
            return list;
        }
        public static float Next(this Random random, float minimum, float maximum)
        {
            return (float)random.NextDouble() * (maximum - minimum) + minimum;
        }
        public static string ToHms(this TimeSpan time)
        {
            string converted = null;
            converted += time.Days switch
            {
                0 => "",
                1 => $"{time.Days} day ",
                _ => $"{time.Days} days "
            };
            converted += time.Hours switch
            {
                0 => "",
                1 => $"{time.Hours} hour ",
                _ => $"{time.Hours} hours "
            };
            converted += (time.Minutes == 0 ? "" : time.Seconds == 0 ? time.Minutes == 1 ? $"{time.Minutes} minute " : $"{time.Minutes} minutes " : time.Minutes == 1 ? $"{time.Minutes} minute and " : $"{time.Minutes} minutes and ");
            converted += time.Seconds switch
            {
                0 => "",
                1 => $"{time.Seconds} second",
                _ => $"{time.Seconds} seconds"
            };
            return converted;
        }
        public static string RemoveSpecialCharacters(this string str)
        {
            return Regex.Replace(str, "[^a-zA-Z0-9_]+", "", RegexOptions.Compiled);
        }
        public static bool ContainsSpecialCharacters(this string str)
        {
            Regex namechecker = new Regex("^[a-zA-Z0-9_ ]*$", RegexOptions.Compiled);
            return !namechecker.IsMatch(str);
        }
        public static bool IsHexString(this string str)
        {
            Regex stringchecker = new Regex("^[a-fA-F0-9]*$", RegexOptions.Compiled);
            return stringchecker.IsMatch(str);
        }
        public static bool IsUrl(this string str)
        {
            Regex stringchecker = new Regex(@"^(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#_]*)?$", RegexOptions.Compiled);
            return stringchecker.IsMatch(str);
        }
        public static string GetUntilOrEmpty(this string text, char stopAt)
        {
            if (string.IsNullOrWhiteSpace(text)) 
                return string.Empty;
            var charLocation = text.IndexOf(stopAt, StringComparison.Ordinal);

            return charLocation > 0 ? text.Substring(0, charLocation) : string.Empty;
        }
        public static EmbedBuilder WithRandomColor(this EmbedBuilder embed)
        {
            embed.Color = new Color(rnd.Next(0, 255), rnd.Next(0, 255), rnd.Next(0, 255));
            return embed;
        }
        public static string ReverseString(this string content)
        {

            var textElementEnumerator = StringInfo.GetTextElementEnumerator(content);

            var SbBuilder = new StringBuilder(content.Length);

            while (textElementEnumerator.MoveNext())
            {
                SbBuilder.Insert(0, textElementEnumerator.GetTextElement());
            }

            return SbBuilder.ToString();
        }
        public static string TrimTo(this string str, int maxLength, bool hideDots = false)
        {
            return maxLength switch
            {
                < 0 => throw new ArgumentOutOfRangeException(nameof(maxLength),
                    $"Argument {nameof(maxLength)} can't be negative."),
                0 => string.Empty,
                <= 3 => string.Concat(str.Select(c => '.')),
                _ => str.Length < maxLength ? str :
                    hideDots ? string.Concat(str.Take(maxLength)) : string.Concat(str.Take(maxLength - 3)) + "..."
            };
        }
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }
        public static bool IsNullOrWhiteSpace(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        } public static string GetSubstring(this string str, string startAt, string stopAt)
        {
            if (string.IsNullOrWhiteSpace(str)) 
                return string.Empty;
            var charLocation = str.IndexOf(startAt, StringComparison.Ordinal);
            var charLocation2 = str.IndexOf(stopAt, StringComparison.Ordinal) - charLocation;

            if (charLocation <= 0 || charLocation2 <= 0) return string.Empty;
            var ret = str.Substring(charLocation, charLocation2);
            return ret;
        }
        public static string GetSubstring(this string str, string startAt)
        {
            if (string.IsNullOrWhiteSpace(str)) 
                return string.Empty;
            var charLocation = str.IndexOf(startAt, StringComparison.Ordinal);

            return charLocation > 0 ? str[charLocation..] : string.Empty;
        }
    }
}
