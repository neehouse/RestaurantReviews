using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMMI.Business.Types
{
    internal class BaseX
    {
        private const string DefaultCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        private const string DefaultGuards = "aeiouAEIOU";
        private const string DefaultSalt = "Pepper";
        private char[] _saltArray;
        private char[] _charArray;
        private char[] _guardArray;
        private readonly string _prefix;
        private readonly int _minLength;

        public BaseX(string characters = DefaultCharacters, int minLength = 1, string prefix = "", string guards = DefaultGuards, string salt = DefaultSalt)
        {
            _charArray = ValidateString(characters.ToCharArray());
            _guardArray = ValidateString(guards.ToCharArray());
            _saltArray = salt.ToCharArray();

            ConsistentShuffle(_charArray);
            SetupGuards();
            ConsistentShuffle(_guardArray);


            if (_charArray.Length == 0)
            {
                throw new ArgumentException("The character sequence does not contain enough characters for the minimal length");
            }

            _prefix = prefix;
            _minLength = minLength;

        }

        private char[] ValidateString(char[] charsArray)
        {
            if (!charsArray.Distinct().SequenceEqual(charsArray))
            {
                throw new ArgumentException("There are duplicate characters in the sequence.", nameof(charsArray));
            }
            return charsArray;
        }

        private void SetupGuards()
        {
            // remove guards from character array
            _charArray = _charArray.Except(_guardArray).ToArray();
            // we need a minimal set of guards.
            var neededGuards = _minLength - _guardArray.Length;

            if (neededGuards <= 0) return;

            _guardArray = _guardArray.Concat(_charArray.Take(neededGuards)).ToArray();
            _charArray = _charArray.Skip(neededGuards).ToArray();
        }

        private void ConsistentShuffle(char[] array)
        {
            if (_saltArray.Length == 0) return;

            for (int i = array.Length - 1, v = 0, p = 0; i > 0; i--, v++)
            {
                v %= _saltArray.Length;
                int n;
                p += (n = _saltArray[v]);
                var j = (n + v + p) % i;
                // swap characters at positions i and j
                var temp = array[j];
                array[j] = array[i];
                array[i] = temp;
            }
        }

        /// <summary>
        /// Encode the given number into a Base36 string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string Encode(long input)
        {

            if (input < 0) throw new ArgumentOutOfRangeException(nameof(input), input, "Input cannot be negative");

            var result = new List<char>();

            while (input != 0)
            {
                result.Add(_charArray[input % _charArray.Length]);
                input /= _charArray.Length;
            }
            // pad with guard characters.
            var guardsNeeded = _minLength - result.Count;
            if (guardsNeeded > 0)
            {
                var split = guardsNeeded / 2;
                var preGaurds = _guardArray.Take(split);
                var postGuards = _guardArray.Skip(split).Take(guardsNeeded - split);
                result = (List<char>) preGaurds.Concat(result).Concat(postGuards);
            }

            return $"{_prefix}{new string(result.ToArray())}";
        }

        public string Convert(long Long)
        {
            var number = Long >= 0 ? long.MaxValue + (ulong)Long : (ulong)(long.MaxValue + Long);

            var buffer = new StringBuilder();
            var quotient = number;
            while (quotient != 0)
            {
                var remainder = quotient % (ulong)_charArray.LongLength;
                quotient = quotient / (ulong)_charArray.LongLength;
                buffer.Insert(0, _charArray[remainder].ToString());
            }
            return buffer.ToString();
        }

        /// <summary>
        /// Decode the Base36 Encoded string into a number
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public long Decode(string input)
        {
            if (!string.IsNullOrWhiteSpace(_prefix) && !input.StartsWith(_prefix))
            {
                throw new ArgumentException("Invalid prefix", nameof(input));
            }

            var encoded = input.Substring(_prefix.Length);
            var reversed = encoded.Reverse();
            long result = 0;
            int pos = 0;
            foreach (char c in reversed)
            {
                var index = Array.IndexOf(_charArray, c);
                if (index < 0)
                {
                    throw new ArgumentException("Invalid character sequence", nameof(input));
                }
                result += index * (long)Math.Pow(_charArray.Length, pos);
                pos++;
            }
            return result;
        }
    }
}