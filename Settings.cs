using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SciChain
{
    public static class Settings
    {
        static Dictionary<string, string> Default = new Dictionary<string, string>();

        public static string GetSettings(string name)
        {
            if (Default.Keys.Count == 0)
            {
                Load();
            }
            if (Default.ContainsKey(name)) return Default[name];
            return "";
        }

        public static void AddSettings(string name, string val)
        {
            if (Default.ContainsKey(name))
                Default[name] = val;
            else
                Default.Add(name, val);
        }

        static string path = System.IO.Path.GetDirectoryName(Environment.ProcessPath);

        public static void Save()
        {
            string val = "";
            foreach (var item in Default)
            {
                val += item.Key + "=" + item.Value + Environment.NewLine;
            }
            File.WriteAllText(path + "/Settings.txt", val);
        }

        // FIX #14: Use Split with limit of 2 to preserve '=' in values
        public static void Load()
        {
            if (!File.Exists(path + "/Settings.txt"))
                return;
            string[] sts = File.ReadAllLines(path + "/Settings.txt");
            foreach (string item in sts)
            {
                if (string.IsNullOrWhiteSpace(item))
                    continue;
                string[] st = item.Split(new[] { '=' }, 2);
                if (st.Length == 2 && !Default.ContainsKey(st[0]))
                    Default.Add(st[0], st[1]);
            }
        }
    }
}
