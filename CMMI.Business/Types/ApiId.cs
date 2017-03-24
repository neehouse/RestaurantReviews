using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
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
    /// <summary>
    /// A Type to allow for the automatic conversion of numeric longs into a hashed string
    /// </summary>
    [TypeConverter(typeof(ApiIdTypeConverter))]
    [JsonConverter(typeof(ApiIdJsonConverter))]
    public struct ApiId : IXmlSerializable
    {
        private readonly object _instance;

        private static readonly Base10Converter Encoder = new Base10Converter(
            salt: "CALIBER",
            minLength: 5,
            alphabet: "ABCDEFGHIJKLMNOPQRSTUVWXYZ",
            separators: "AEIOU" // helps to avoid spelling real words.
        );

        private static readonly bool Disabled = string.Equals(ConfigurationManager.AppSettings["ApiId.Disabled"], "true",
            StringComparison.InvariantCultureIgnoreCase);


        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiId"/> struct with a <see cref="long" /> value.
        /// </summary>
        /// <param name="value">The <see cref="long" /> value.</param>
        public ApiId(long value)
        {
            _instance = new object();
            Value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiId"/> struct with a <see cref="string" /> hash.
        /// </summary>
        /// <param name="value">The <see cref="string" /> hash value.</param>
        public ApiId(string value)
        {
            _instance = new object();
            Value = 0;
            Hash = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiId"/> struct from another <see cref="ApiId" />.
        /// </summary>
        /// <param name="value">The <see cref="ApiId" /> value.</param>
        public ApiId(ApiId value)
        {
            _instance = new object();
            Value = value.Value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiId"/> struct from an object.
        /// </summary>
        /// <param name="value">The object value.</param>
        public ApiId(object value)
        {
            _instance = new object();
            long lng;

            // long
            if (value is long) Value = (long)value;
            // ApiId
            else if (value is ApiId) Value = ((ApiId)value).Value;
            // string
            else Value = long.TryParse(value.ToString(), out lng) ? lng : Encoder.Decode(value.ToString())[0];
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the <see cref="long" /> value.
        /// </summary>
        /// <value>
        /// The <see cref="long" /> value.
        /// </value>
        public long Value { get; private set; }

        /// <summary>
        /// Gets the <see cref="string" /> hash.
        /// </summary>
        /// <value>
        /// The <see cref="string" /> hash.
        /// </value>
        public string Hash
        {
            get { return Encoder.Encode(Value); }
            private set { Value = Encoder.Decode(value)[0]; }
        }

        #endregion

        #region Comparison Methods

        /// <summary>
        /// Determines whether an object of type <see cref="long" />, <see cref="string" />, or <see cref="ApiId" /> is assignable to a ApiId object.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        ///   <c>true</c> if the object is of type <see cref="long" />, <see cref="string" />, or <see cref="ApiId" />; otherwise, <c>false</c>.
        /// </returns>
        public bool IsAssignableFrom(Type type)
        {
            return type == typeof(long) || type == typeof(string) || type == typeof(ApiId);
        }

        /// <summary>
        /// Equals the specified identifier.
        /// </summary>
        /// <param name="value">The <see cref="ApiId" /> value.</param>
        /// <returns></returns>
        public bool Equals(ApiId value)
        {
            return Value == value.Value;
        }

        /// <summary>
        /// Determines whether the specified <see cref="object" />, is equal to this instance.
        /// </summary>
        /// <param name="value">The <see cref="object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object value)
        {
            if (ReferenceEquals(null, value)) return false;
            var hashId = new ApiId(value);
            return Value == hashId.Value;
        }

        /// <summary>
        /// Returns a <see cref="string" /> hash that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> hash that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return Hash;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return _instance.GetHashCode();
        }

        #endregion

        #region Conversion Operators

        /// <summary>
        /// Performs an implicit conversion from <see cref="ApiId"/> to <see cref="long"/>.
        /// </summary>
        /// <param name="value">The <see cref="ApiId" /> value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator long(ApiId value)
        {
            return value.Value;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="ApiId"/> to <see cref="string"/>.
        /// </summary>
        /// <param name="value">The <see cref="ApiId"/> value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator string(ApiId value)
        {
            return value.Hash;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="string"/> to <see cref="ApiId"/>.
        /// </summary>
        /// <param name="value">The <see cref="ApiId"/>  value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator ApiId(string value)
        {
            return new ApiId(value);
        }

        /// <summary>
        /// Converts an ApiId from the object.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static ApiId FromObject(object value)
        {
            return new ApiId(value);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="long"/> to <see cref="ApiId"/>.
        /// </summary>
        /// <param name="value">The <see cref="long"/> value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator ApiId(long value)
        {
            return new ApiId(value);
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="id1">The <see cref="ApiId"/> value 1.</param>
        /// <param name="id2">The <see cref="ApiId"/> value 2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(ApiId id1, ApiId id2)
        {
            return id1.Value == id2.Value;
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="id1">The <see cref="ApiId"/> value 1.</param>
        /// <param name="id2">The <see cref="ApiId"/> value 2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(ApiId id1, ApiId id2)
        {
            return id1.Value != id2.Value;
        }

        #endregion

        #region XML Serialization
        /// <summary>
        /// This method is reserved and should not be used. When implementing the IXmlSerializable interface, you should return null (Nothing in Visual Basic) from this method, and instead, if specifying a custom schema is required, apply the <see cref="T:System.Xml.Serialization.XmlSchemaProviderAttribute" /> to the class.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Xml.Schema.XmlSchema" /> that describes the XML representation of the object that is produced by the <see cref="M:System.Xml.Serialization.IXmlSerializable.WriteXml(System.Xml.XmlWriter)" /> method and consumed by the <see cref="M:System.Xml.Serialization.IXmlSerializable.ReadXml(System.Xml.XmlReader)" /> method.
        /// </returns>
        public XmlSchema GetSchema()
        {
            return null;
        }

        /// <summary>
        /// Generates an object from its XML representation.
        /// </summary>
        /// <param name="reader">The <see cref="T:System.Xml.XmlReader" /> stream from which the object is de-serialized.</param>
        public void ReadXml(XmlReader reader)
        {
            var str = reader.ReadString();
            reader.ReadEndElement();

            if (str == null) return;

            long lng;
            var isLong = long.TryParse(str, out lng);

            if (isLong && !Disabled)
                throw new ArgumentException("The value is in the incorrect format.");

            if (isLong)
                Value = lng;
            else
                Hash = str;
        }

        /// <summary>
        /// Converts an object into its XML representation.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Xml.XmlWriter" /> stream to which the object is serialized.</param>
        public void WriteXml(XmlWriter writer)
        {
            writer.WriteValue(Disabled ? Value.ToString() : Hash);
        }

        #endregion

        /// <summary>
        /// JsonConverter for ApiId Serialization
        /// </summary>
        /// <seealso cref="Newtonsoft.Json.JsonConverter" />
        private class ApiIdJsonConverter : JsonConverter
        {
            /// <summary>
            /// Reads the JSON representation of the object.
            /// </summary>
            /// <param name="reader">The <see cref="T:Newtonsoft.Json.JsonReader" /> to read from.</param>
            /// <param name="objectType">Type of the object.</param>
            /// <param name="existingValue">The existing value of object being read.</param>
            /// <param name="serializer">The calling serializer.</param>
            /// <returns>
            /// The object value.
            /// </returns>
            public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
                JsonSerializer serializer)
            {
                var str = JToken.ReadFrom(reader).Value<string>();
                if (str == null) return null;

                long lng;
                var isLong = long.TryParse(str, out lng);

                if (isLong && !Disabled)
                    throw new ArgumentException("The value is in the incorrect format.");

                return isLong ? new ApiId(lng) : new ApiId(str);
            }

            /// <summary>
            /// Determines whether this instance can convert the specified object type.
            /// </summary>
            /// <param name="type">Type of the object.</param>
            /// <returns>
            /// <c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.
            /// </returns>
            public override bool CanConvert(Type type)
            {
                return typeof(long) == type || typeof(string) == type;
            }

            /// <summary>
            /// Writes the JSON representation of the object.
            /// </summary>
            /// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter" /> to write to.</param>
            /// <param name="value">The value.</param>
            /// <param name="serializer">The calling serializer.</param>
            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                var id = (ApiId)value;
                serializer.Serialize(writer, Disabled ? (object)id.Value : id.Hash);
            }
        }

        /// <summary>
        /// Provides a way to convert to <see cref="ApiId" /> from <see cref="long" /> and  <see cref="string" />.
        /// </summary>
        /// <seealso cref="System.ComponentModel.TypeConverter" />
        private class ApiIdTypeConverter : TypeConverter
        {
            /// <summary>
            /// Returns whether this converter can convert an object of the given type to the type of this converter, using the specified context.
            /// </summary>
            /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context.</param>
            /// <param name="sourceType">A <see cref="T:System.Type" /> that represents the type you want to convert from.</param>
            /// <returns>
            /// true if this converter can perform the conversion; otherwise, false.
            /// </returns>
            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                return sourceType == typeof(long) || sourceType == typeof(string);
            }

            /// <summary>
            /// Converts the given object to the type of this converter, using the specified context and culture information.
            /// </summary>
            /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context.</param>
            /// <param name="culture">The <see cref="T:System.Globalization.CultureInfo" /> to use as the current culture.</param>
            /// <param name="value">The <see cref="T:System.Object" /> to convert.</param>
            /// <returns>
            /// An <see cref="T:System.Object" /> that represents the converted value.
            /// </returns>
            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                if (value == null) return null;

                long lng;
                var isLong = long.TryParse(value.ToString(), out lng);

                if (isLong && !Disabled)
                    throw new ArgumentException("The value is in the incorrect format.");

                return isLong ? new ApiId(lng) : new ApiId(value.ToString());
            }

            /// <summary>
            /// Returns whether this converter can convert the object to the specified type, using the specified context.
            /// </summary>
            /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context.</param>
            /// <param name="destinationType">A <see cref="T:System.Type" /> that represents the type you want to convert to.</param>
            /// <returns>
            /// true if this converter can perform the conversion; otherwise, false.
            /// </returns>
            public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
            {
                return destinationType == typeof(long) || destinationType == typeof(string);
            }

            /// <summary>
            /// Converts the given value object to the specified type, using the specified context and culture information.
            /// </summary>
            /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context.</param>
            /// <param name="culture">A <see cref="T:System.Globalization.CultureInfo" />. If null is passed, the current culture is assumed.</param>
            /// <param name="value">The <see cref="T:System.Object" /> to convert.</param>
            /// <param name="destinationType">The <see cref="T:System.Type" /> to convert the <paramref name="value" /> parameter to.</param>
            /// <returns>
            /// An <see cref="T:System.Object" /> that represents the converted value.
            /// </returns>
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value,
                Type destinationType)
            {
                return new ApiId(value);
            }
        }

        /// <summary>
        /// A numeric encoder that can encode a list of numbers to string and back.
        /// Logic derived from http://hashids.org/net/
        /// </summary>        
        private class Base10Converter
        {

            private const string DefaultAlphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            private const string DefaultSeps = "cfhistuCFHISTU";

            private const int MinAlphabetLength = 16;
            private const double SeparatorDiv = 3.5;
            private const double GuardDiv = 12.0;

            private string _alphabet;
            private readonly string _salt;
            private string _separators;
            private string _guards;
            private readonly int _minHashLength;

            private Regex _guardsRegex;
            private Regex _separatorsRegex;

            /// <summary>
            /// Instantiates a new Base10Converter encoder/decoder.
            /// </summary>
            /// <param name="salt"></param>
            /// <param name="minLength"></param>
            /// <param name="alphabet"></param>
            /// <param name="separators"></param>
            public Base10Converter(string salt = "", int minLength = 0, string alphabet = DefaultAlphabet,
                string separators = DefaultSeps)
            {
                if (string.IsNullOrWhiteSpace(alphabet))
                    throw new ArgumentNullException(nameof(alphabet));
                if (alphabet.Length < MinAlphabetLength)
                    throw new ArgumentException(
                        $"alphabet must contain at least {MinAlphabetLength} unique characters.", nameof(alphabet));

                _salt = salt;
                _alphabet = new string(alphabet.ToCharArray().Distinct().ToArray());
                _separators = separators;
                _minHashLength = minLength;

                SetupSeparators();
                SetupGuards();
            }

            /// <summary>
            /// Decodes the provided hashed string into an array of longs 
            /// </summary>
            /// <param name="hash">the hashed string</param>
            /// <returns>the numbers</returns>
            public long[] Decode(string hash)
            {
                return GetNumbers(hash);
            }

            /// <summary>
            /// Encodes the provided longs to a hashed string
            /// </summary>
            /// <param name="numbers">the numbers</param>
            /// <returns>the hashed string</returns>
            public string Encode(params long[] numbers)
            {
                return numbers.Any(n => n < 0) ? string.Empty : GetHash(numbers);
            }

            /// <summary>
            /// Configures the list of characters used as separators between numbers
            /// </summary>
            private void SetupSeparators()
            {
                // separators should contain only characters present in alphabet; 
                _separators = new string(_separators.ToCharArray().Intersect(_alphabet.ToCharArray()).ToArray());

                // alphabet should not contain separators.
                _alphabet = new string(_alphabet.ToCharArray().Except(_separators.ToCharArray()).ToArray());
                _separators = ConsistentShuffle(_separators, _salt);

                // ReSharper disable once PossibleLossOfFraction
                if (_separators.Length == 0 || (_alphabet.Length / _separators.Length) > SeparatorDiv)
                {
                    var separatorsLength = (int)Math.Ceiling(_alphabet.Length / SeparatorDiv);
                    if (separatorsLength == 1)
                        separatorsLength = 2;

                    if (separatorsLength > _separators.Length)
                    {
                        var diff = separatorsLength - _separators.Length;
                        _separators += _alphabet.Substring(0, diff);
                        _alphabet = _alphabet.Substring(diff);
                    }

                    else _separators = _separators.Substring(0, separatorsLength);
                }

                _separatorsRegex = new Regex(string.Concat("[", _separators, "]"), RegexOptions.Compiled);
                _alphabet = ConsistentShuffle(_alphabet, _salt);
            }

            /// <summary>
            /// Configures the list of characters used as filler characters
            /// </summary>
            private void SetupGuards()
            {
                var guardCount = (int)Math.Ceiling(_alphabet.Length / GuardDiv);

                if (_alphabet.Length < 3)
                {
                    _guards = _separators.Substring(0, guardCount);
                    _separators = _separators.Substring(guardCount);
                }
                else
                {
                    _guards = _alphabet.Substring(0, guardCount);
                    _alphabet = _alphabet.Substring(guardCount);
                }

                _guardsRegex = new Regex(string.Concat("[", _guards, "]"), RegexOptions.Compiled);
            }

            /// <summary>
            /// Internal function that does the work of creating the hash
            /// </summary>
            /// <param name="numbers"></param>
            /// <returns></returns>
            private string GetHash(IReadOnlyList<long> numbers)
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
                    var sepsIndex = ((int)number % _separators.Length);

                    ret.Append(_separators[sepsIndex]);
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

            /// <summary>
            /// Gets the <see cref="long" /> values from the <see cref="string" /> hash.
            /// </summary>
            /// <param name="hash">The <see cref="string" /> hash.</param>
            /// <returns>A list of <see cref="long" /> values</returns>
            private long[] GetNumbers(string hash)
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

                hashBreakdown = _separatorsRegex.Replace(hashBreakdown, " ");
                hashArray = hashBreakdown.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var subHash in hashArray)
                {
                    var buffer = lottery + _salt + alphabet;
                    alphabet = ConsistentShuffle(alphabet, buffer.Substring(0, alphabet.Length));
                    ret.Add(Unhash(subHash, alphabet));
                }

                if (Encode(ret.ToArray()) != hash)
                {
                    ret.Clear();
                }

                return ret.ToArray();
            }


            /// <summary>
            /// Encodes the specified input against the supplied alphabet.
            /// </summary>
            /// <param name="value">The <see cref="long" /> value.</param>
            /// <param name="alphabet">The <see cref="string" /> alphabet.</param>
            /// <returns>The <see cref="string" /> hash of the value.</returns>
            /// <exception cref="System.ArgumentNullException">alphabet</exception>
            /// <exception cref="System.ArgumentOutOfRangeException">input</exception>
            private static string Hash(long value, string alphabet)
            {
                if (alphabet == null) throw new ArgumentNullException(nameof(alphabet));
                if (value < 0) throw new ArgumentOutOfRangeException(nameof(value));

                var hash = new StringBuilder();

                do
                {
                    hash.Insert(0, alphabet[(int)(value % alphabet.Length)]);
                    value = (value / alphabet.Length);
                } while (value > 0);

                return hash.ToString();
            }

            /// <summary>
            /// Decodes the specified value.
            /// </summary>
            /// <param name="value">The <see cref="string" /> value.</param>
            /// <param name="alphabet">The <see cref="string" /> alphabet.</param>
            /// <returns>The <see cref="long" /> value.</returns>
            private static long Unhash(string value, string alphabet)
            {
                return value
                    .Select(t => alphabet.IndexOf(t))
                    .Select((pos, i) => (long)(pos * Math.Pow(alphabet.Length, value.Length - i - 1)))
                    .Sum();
            }

            /// <summary>
            /// Consistently re-sequences characters in a <see cref="string" /> based upon another <see cref="string" />.
            /// </summary>
            /// <param name="alphabet"></param>
            /// <param name="salt"></param>
            /// <returns>The alphabet in a new sequence.</returns>
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
    }
}