using System;
using System.Collections.Generic;
using System.Text;
using ConsoleApp1.Models;
using System.Text.Json;

namespace ConsoleApp1.Helpers
{
           public static class SerializerDeserializerExtensions
        {
        public static byte[] Serializer(this object _object)
        {
            byte[] bytes = JsonSerializer.SerializeToUtf8Bytes(_object);
            return bytes;
        }



        public static string SerializInString(this object _object)
        {
            var sData = JsonSerializer.Serialize(_object);
            return sData;
        }



        public static T Deserializer<T>(this byte[] _byteArray)
        {
            T ReturnValue = JsonSerializer.Deserialize<T>(_byteArray);
            return ReturnValue;
        }
    }
    }

