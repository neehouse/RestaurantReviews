using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CMMI.Business.Types
{
    [TypeConverter(typeof(HashIdTypeConverter))]
    [JsonConverter(typeof(HashIdJsonConverter))]
    public struct ApiId : IXmlSerializable
    {
        private readonly object _instance;
        private static readonly HashIds HashIds = new HashIds(
            salt: "CALIBER", 
            minHashLength: 5, 
            alphabet: "THEQUICKBROWNFXJMPSVLAZYDG",
            separators: "AEIOU"
            );

        #region constructors
        public ApiId(long value)
        {
            _instance = new object();
            Value = value;
        }

        public ApiId(string value)
        {
            _instance = new object();
            Value = 0;
            Hash = value;
        }

        //public ApiId()
        //{
        //    Value = default(long);
        //}

        public ApiId(object value)
        {
            _instance = new object();
            long lng;
            Value = long.TryParse(value.ToString(), out lng) ? lng : HashIds.DecodeLong(value.ToString())[0];
        }
        #endregion

        #region Comparison Methods
        public bool IsAssignableFrom(Type type)
        {
            return type == typeof(long) || type == typeof(string);
        }

        public bool Equals(ApiId id)
        {
            return Value == id.Value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            var hashId = (ApiId) obj;
            return Equals(hashId);
        }

        public override string ToString()
        {
            return Hash;
        }

        //this is ugly.  
        public override int GetHashCode()
        {
            return _instance.GetHashCode();
        }
        #endregion

        public long Value { get; private set; }

        public string Hash
        {
            get
            {
                return HashIds.EncodeLong(Value);
            }
            private set
            {
                Value = HashIds.DecodeLong(value)[0];
            }
        }

        #region conversion operations
        public static implicit operator long(ApiId id)
        {
            return id.Value;
        }

        public static implicit operator string(ApiId id)
        {
            return id.Hash;
        }

        public static implicit operator ApiId(string id)
        {
            return new ApiId(id);
        }

        public static implicit operator ApiId(long id)
        {
            return new ApiId(id);
        }

        public static bool operator ==(ApiId id1, ApiId id2)
        {
            return id1.Value == id2.Value;
        }

        public static bool operator !=(ApiId id1, ApiId id2)
        {
            return id1.Value != id2.Value;
        }
        #endregion

        #region serialization
        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            string str = reader.ReadString();
            reader.ReadEndElement();
            Hash = str;
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteValue(Hash);
        }

        #endregion
    }

    class HashIds
    {
        public const string DefaultAlphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        public const string DefaultSeps = "cfhistuCFHISTU";

        private const int MinAlphabetLength = 16;
        private const double SepDiv = 3.5;
        private const double GuardDiv = 12.0;

        private string _alphabet;
        private readonly string _salt;
        private string _seps;
        private string _guards;
        private readonly int _minHashLength;

        private Regex _guardsRegex;
        private Regex _sepsRegex;

        //  Creates the Regex in the first usage, speed up first use of non hex methods
#if CORE
        private static readonly Lazy<Regex> HexValidator = new Lazy<Regex>(() => new Regex("^[0-9a-fA-F]+$"));
        private static readonly Lazy<Regex> HexSplitter = new Lazy<Regex>(() => new Regex(@"[\w\W]{1,12}"));
#else
        private static readonly Lazy<Regex> HexValidator = new Lazy<Regex>(() => new Regex("^[0-9a-fA-F]+$", RegexOptions.Compiled));
        private static readonly Lazy<Regex> HexSplitter = new Lazy<Regex>(() => new Regex(@"[\w\W]{1,12}", RegexOptions.Compiled));
#endif

        /// <summary>
        /// Instantiates a new HashIds with the default setup.
        /// </summary>
        public HashIds() : this(string.Empty)
        { }

        /// <summary>
        /// Instantiates a new HashIds encoder/decoder.
        /// </summary>
        /// <param name="salt"></param>
        /// <param name="minHashLength"></param>
        /// <param name="alphabet"></param>
        /// <param name="separators"></param>
        public HashIds(string salt = "", int minHashLength = 0, string alphabet = DefaultAlphabet, string separators = DefaultSeps)
        {
            if (string.IsNullOrWhiteSpace(alphabet))
                throw new ArgumentNullException(nameof(alphabet));
            if (alphabet.Length < MinAlphabetLength)
                throw new ArgumentException($"alphabet must contain at least {MinAlphabetLength} unique characters.", nameof(alphabet));

            _salt = salt;
            _alphabet = new string(alphabet.ToCharArray().Distinct().ToArray());
            _seps = separators;
            _minHashLength = minHashLength;


            SetupSeps();
            SetupGuards();
        }

        /// <summary>
        /// Encodes the provided numbers into a hashed string
        /// </summary>
        /// <param name="numbers">the numbers to encode</param>
        /// <returns>the hashed string</returns>
        public virtual string EncodeInt(params int[] numbers)
        {
            if (numbers.Any(n => n < 0)) return string.Empty;
            return GenerateHashFrom(numbers.Select(n => (long)n).ToArray());
        }

        /// <summary>
        /// Encodes the provided numbers into a hashed string
        /// </summary>
        /// <param name="numbers">the numbers to encode</param>
        /// <returns>the hashed string</returns>
        public virtual string EncodeInt(IEnumerable<int> numbers)
        {
            return EncodeInt(numbers.ToArray());
        }

        /// <summary>
        /// Decodes the provided hash into
        /// </summary>
        /// <param name="hash">the hash</param>
        /// <exception cref="T:System.OverflowException">if the decoded number overflows integer</exception>
        /// <returns>the numbers</returns>
        public virtual int[] DecodeInt(string hash)
        {
            return GetNumbersFrom(hash).Select(n => (int)n).ToArray();
        }

        /// <summary>
        /// Encodes the provided hex string to a HashIds hash.
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public virtual string EncodeHex(string hex)
        {
            if (!HexValidator.Value.IsMatch(hex))
                return string.Empty;

            var numbers = new List<long>();
            var matches = HexSplitter.Value.Matches(hex);

            foreach (Match match in matches)
            {
                var number = Convert.ToInt64(string.Concat("1", match.Value), 16);
                numbers.Add(number);
            }

            return EncodeLong(numbers.ToArray());
        }

        /// <summary>
        /// Decodes the provided hash into a hex-string
        /// </summary>
        /// <param name="hash"></param>
        /// <returns></returns>
        public virtual string DecodeHex(string hash)
        {
            var ret = new StringBuilder();
            var numbers = DecodeLong(hash);

            foreach (var number in numbers)
                ret.Append($"{number:X}".Substring(1));

            return ret.ToString();
        }

        /// <summary>
        /// Decodes the provided hashed string into an array of longs 
        /// </summary>
        /// <param name="hash">the hashed string</param>
        /// <returns>the numbers</returns>
        public long[] DecodeLong(string hash)
        {
            return GetNumbersFrom(hash);
        }

        /// <summary>
        /// Encodes the provided longs to a hashed string
        /// </summary>
        /// <param name="numbers">the numbers</param>
        /// <returns>the hashed string</returns>
        public string EncodeLong(params long[] numbers)
        {
            if (numbers.Any(n => n < 0)) return string.Empty;
            return GenerateHashFrom(numbers);
        }

        /// <summary>
        /// Encodes the provided longs to a hashed string
        /// </summary>
        /// <param name="numbers">the numbers</param>
        /// <returns>the hashed string</returns>
        public string EncodeLong(IEnumerable<long> numbers)
        {
            return EncodeLong(numbers.ToArray());
        }

        /// <summary>
        /// 
        /// </summary>
        private void SetupSeps()
        {
            // seps should contain only characters present in alphabet; 
            _seps = new string(_seps.ToCharArray().Intersect(_alphabet.ToCharArray()).ToArray());

            // alphabet should not contain seps.
            _alphabet = new string(_alphabet.ToCharArray().Except(_seps.ToCharArray()).ToArray());

            _seps = ConsistentShuffle(_seps, _salt);

            // ReSharper disable once PossibleLossOfFraction
            if (_seps.Length == 0 || (_alphabet.Length / _seps.Length) > SepDiv)
            {
                var sepsLength = (int)Math.Ceiling(_alphabet.Length / SepDiv);
                if (sepsLength == 1)
                    sepsLength = 2;

                if (sepsLength > _seps.Length)
                {
                    var diff = sepsLength - _seps.Length;
                    _seps += _alphabet.Substring(0, diff);
                    _alphabet = _alphabet.Substring(diff);
                }

                else _seps = _seps.Substring(0, sepsLength);
            }

#if CORE
            sepsRegex = new Regex(string.Concat("[", seps, "]"));
#else
            _sepsRegex = new Regex(string.Concat("[", _seps, "]"), RegexOptions.Compiled);
#endif
            _alphabet = ConsistentShuffle(_alphabet, _salt);
        }

        /// <summary>
        /// 
        /// </summary>
        private void SetupGuards()
        {
            var guardCount = (int)Math.Ceiling(_alphabet.Length / GuardDiv);

            if (_alphabet.Length < 3)
            {
                _guards = _seps.Substring(0, guardCount);
                _seps = _seps.Substring(guardCount);
            }

            else
            {
                _guards = _alphabet.Substring(0, guardCount);
                _alphabet = _alphabet.Substring(guardCount);
            }

#if CORE
            guardsRegex = new Regex(string.Concat("[", guards, "]"));
#else
            _guardsRegex = new Regex(string.Concat("[", _guards, "]"), RegexOptions.Compiled);
#endif
        }

        /// <summary>
        /// Internal function that does the work of creating the hash
        /// </summary>
        /// <param name="numbers"></param>
        /// <returns></returns>
        private string GenerateHashFrom(IReadOnlyList<long> numbers)
        {
            if (numbers == null) throw new ArgumentNullException(nameof(numbers));
            if (numbers.Count == 0) throw new ArgumentException("List cannot be empty", nameof(numbers));

            var ret = new StringBuilder();
            var alphabet = _alphabet;

            long numbersHashInt = 0;
            for (var i = 0; i < numbers.Count; i++)
                numbersHashInt += (int)(numbers[i] % (i + 100));

            var lottery = alphabet[(int)(numbersHashInt % alphabet.Length)];
            ret.Append(lottery.ToString());

            for (var i = 0; i < numbers.Count; i++)
            {
                var number = numbers[i];
                var buffer = lottery + _salt + alphabet;

                alphabet = ConsistentShuffle(alphabet, buffer.Substring(0, alphabet.Length));
                var last = Hash(number, alphabet);

                ret.Append(last);

                if (i + 1 >= numbers.Count) continue;

                number %= last[0] + i;
                var sepsIndex = ((int)number % _seps.Length);

                ret.Append(_seps[sepsIndex]);
            }

            if (ret.Length < _minHashLength)
            {
                var guardIndex = (int)(numbersHashInt + ret[0]) % _guards.Length;
                var guard = _guards[guardIndex];

                ret.Insert(0, guard);

                if (ret.Length < _minHashLength)
                {
                    guardIndex = (int)(numbersHashInt + ret[2]) % _guards.Length;
                    guard = _guards[guardIndex];

                    ret.Append(guard);
                }
            }

            var halfLength = alphabet.Length / 2;
            while (ret.Length < _minHashLength)
            {
                alphabet = ConsistentShuffle(alphabet, alphabet);
                ret.Insert(0, alphabet.Substring(halfLength));
                ret.Append(alphabet.Substring(0, halfLength));
                var excess = ret.Length - _minHashLength;

                if (excess <= 0) continue;

                ret.Remove(0, excess / 2);
                ret.Remove(_minHashLength, ret.Length - _minHashLength);
            }

            return ret.ToString();
        }

        private static string Hash(long input, string alphabet)
        {
            if (alphabet == null) throw new ArgumentNullException(nameof(alphabet));
            if (input < 0) throw new ArgumentOutOfRangeException(nameof(input));

            var hash = new StringBuilder();

            do
            {
                hash.Insert(0, alphabet[(int)(input % alphabet.Length)]);
                input = (input / alphabet.Length);
            } while (input > 0);

            return hash.ToString();
        }

        private static long Unhash(string input, string alphabet)
        {
            return input
                .Select(t => alphabet.IndexOf(t))
                .Select((pos, i) => (long) (pos * Math.Pow(alphabet.Length, input.Length - i - 1)))
                .Sum();
        }

        private long[] GetNumbersFrom(string hash)
        {
            if (string.IsNullOrWhiteSpace(hash)) return new long[0];

            var alphabet = new string(_alphabet.ToCharArray());
            var ret = new List<long>();
            var i = 0;

            var hashBreakdown = _guardsRegex.Replace(hash, " ");
            var hashArray = hashBreakdown.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (hashArray.Length == 3 || hashArray.Length == 2)
            {
                i = 1;
            }

            hashBreakdown = hashArray[i];

            if (hashBreakdown[0] == default(char)) return ret.ToArray();

            var lottery = hashBreakdown[0];
            hashBreakdown = hashBreakdown.Substring(1);

            hashBreakdown = _sepsRegex.Replace(hashBreakdown, " ");
            hashArray = hashBreakdown.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var subHash in hashArray)
            {
                var buffer = lottery + _salt + alphabet;
                alphabet = ConsistentShuffle(alphabet, buffer.Substring(0, alphabet.Length));
                ret.Add(Unhash(subHash, alphabet));
            }

            if (EncodeLong(ret.ToArray()) != hash)
            {
                ret.Clear();
            }

            return ret.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="alphabet"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        private static string ConsistentShuffle(string alphabet, string salt)
        {
            if (string.IsNullOrWhiteSpace(salt)) return alphabet;

            var letters = alphabet.ToCharArray();

            for (int i = letters.Length - 1, v = 0, p = 0; i > 0; i--, v++)
            {
                v %= salt.Length;
                int n;
                p += (n = salt[v]);
                var j = (n + v + p) % i;
                // swap characters at positions i and j
                var temp = letters[j];
                letters[j] = letters[i];
                letters[i] = temp;
            }

            return new string(letters);
        }
    }

    class BaseX
    {
        private readonly char[] _charArray;
        private readonly int _charArrayLength;
        private readonly string _prefix;

        public BaseX(string prefix = "", string characters = "0123456789abcdefghijklmnopqrstuvwxyz")
        {
            _charArray = ValidateString(characters.ToCharArray());
            _charArrayLength = _charArray.Length;
            _prefix = prefix;
        }

        private char[] ValidateString(char[] charsArray)
        {
            if (!charsArray.Distinct().SequenceEqual(charsArray))
            {
                throw new ArgumentException("There are duplicate characters in the sequence.", nameof(charsArray));
            }
            return charsArray;
        }

        /// <summary>
        /// Encode the given number into a Base36 string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public String Encode(long input)
        {
            if (input < 0) throw new ArgumentOutOfRangeException(nameof(input), input, "Input cannot be negative");

            var result = new Stack<char>();

            while (input != 0)
            {
                result.Push(_charArray[input % _charArrayLength]);
                input /= _charArrayLength;
            }
            return $"{_prefix}{new string(result.ToArray())}";
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
                result += index * (long)Math.Pow(_charArrayLength, pos);
                pos++;
            }
            return result;
        }
    }

    class HashIdJsonConverter : JsonConverter
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JToken jt = JToken.ReadFrom(reader);

            return new ApiId(jt.Value<string>());
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(long) == objectType;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value.ToString());
        }
    }

    class HashIdTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(long) || sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context,
            CultureInfo culture, object value)
        {
            return new ApiId(value);
        }

        //public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        //{
        //    return destinationType == typeof(long) || destinationType == typeof(string);
        //}

        //public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        //{
        //    return new ApiId(value);
        //}
    }
}