using System.Collections.Generic;
using System.Dynamic;

namespace ITOps.ViewModelComposition
{
    public static class Dynamic
    {
        public static bool HasOwnProperty(dynamic settings, string name)
        {
            if (settings is ExpandoObject)
            {
                return ((IDictionary<string, object>)settings).ContainsKey(name);
            }

            return settings.GetType().GetProperty(name) != null;
        }
    }
}
