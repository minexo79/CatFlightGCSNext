using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace CatFlightGCSNext.Core.Utils
{
    internal class YamlReader
    {
        public static string[]? ReadArray(string filename, string key)
        {
            var serializer = new SharpYaml.Serialization.Serializer();
            Dictionary<object, object> yamlTree;

            // open file
            using (FileStream fs = File.Open(filename, FileMode.Open))
            {
                // read file
                using (StreamReader sr = new StreamReader(fs))
                {
                    // using sharpyaml to deserialize
                    yamlTree = (Dictionary<object, object>)serializer.Deserialize(sr);
                }
            }

            // search key
            var _value = yamlTree.FirstOrDefault(x => x.Key.ToString() == key);

            // if found items, return list, otherwise return null
            if (_value.Key != null)
                return ((List<object>)_value.Value).Select(x => x.ToString()).ToArray();

            return null;
        }
    }
}
