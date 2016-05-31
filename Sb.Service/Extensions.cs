using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Sb.Services
{
    public static class Extensions
    {
        public static Dictionary<string, string> ToDictionary(this NameValueCollection nameValueCollection)
        {
            return nameValueCollection.AllKeys.ToDictionary(k => k, k => nameValueCollection[k]);
        }
    }
}