using System;
using System.Configuration;
using Sb.Interfaces;

namespace Sb.Services
{
    public class AppSettingsReader : ISettingsReader
    {
        public string this[string key] => ConfigurationManager.AppSettings[key];

        public int ReadInt(string key, int defaultIfAbsent = 0)
        {
            return Read(key, int.Parse, defaultIfAbsent);
        }

        public bool ReadBool(string key, bool defaultIfAbsent = false)
        {
            return Read(key, bool.Parse, defaultIfAbsent);
        }

        private T Read<T>(string key, Func<string, T> parseMethod, T defaultIfAbsent)
        {
            var stringVal = this[key];

            if (string.IsNullOrWhiteSpace(stringVal))
            {
                return defaultIfAbsent;
            }

            return parseMethod(stringVal);
        }
    }
}
