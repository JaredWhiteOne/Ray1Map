﻿using System;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json;

namespace R1Engine
{
    /// <summary>
    /// Serializes a byte array as a hex string
    /// </summary>
    public class ByteArrayHexConverter : JsonConverter<byte[]>
    {
        public override bool CanRead => true;

        public override bool CanWrite => true;

        public override byte[] ReadJson(JsonReader reader, System.Type objectType, byte[] existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            return reader.Value.ToString().Split(' ').Select(x => Byte.Parse(x, NumberStyles.HexNumber)).ToArray();
        }

        public override void WriteJson(JsonWriter writer, byte[] value, JsonSerializer serializer)
        {
            writer.WriteValue(String.Join(" ", value.Select(p => p.ToString("X2"))));
        }
    }
}