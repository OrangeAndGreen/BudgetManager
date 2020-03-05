using BudgetManager.Data;
using System.Collections.Generic;
using System.IO;

namespace BudgetManager.Logic
{
    public static class TypeManager
    {
        public static Dictionary<int, TransactionType> Types;

        public static void Load()
        {
            Types = new Dictionary<int, TransactionType>();

            if(File.Exists(Constants.TypesFilename))
            {
                using (TextReader reader = File.OpenText(Constants.TypesFilename))
                {
                    string[] lines = reader.ReadToEnd().Split('\n');

                    foreach (string line in lines)
                    {
                        TransactionType loaded = LoadFromString(line);

                        if(loaded != null)
                        {
                            Types[loaded.Id] = loaded;
                        }
                    }
                }
            }
        }

        public static TransactionType Identify(string description)
        {
            foreach (int key in Types.Keys)
            {
                if (Types[key].Matches(description))
                {
                    return Types[key];
                }
            }

            return null;
        }

        public static void Save()
        {
            if(File.Exists(Constants.TypesFilename))
            {
                File.Delete(Constants.TypesFilename);
            }

            using (TextWriter writer = File.CreateText(Constants.TypesFilename))
            {
                foreach(int key in Types.Keys)
                {
                    writer.WriteLine(TypeToString(Types[key]));
                }
            }
        }

        public static TransactionType Get(int id)
        {
            if(Types.ContainsKey(id))
            {
                return Types[id];
            }

            return null;
        }

        public static int Add(TransactionType toAdd)
        {
            if (toAdd.Id <= 0)
            {
                //Find an unused Id
                int maxId = 0;
                foreach (int key in Types.Keys)
                {
                    if (key > maxId)
                    {
                        maxId = key;
                    }
                }

                toAdd.Id = maxId + 1;
            }

            Types[toAdd.Id] = toAdd;

            Save();

            return toAdd.Id;
        }

        static TransactionType LoadFromString(string line)
        {
            List<string> parts = FileHelpers.ParseCSVLine(line);

            if(parts.Count < 4)
            {
                return null;
            }

            return new TransactionType
            {
                Id = int.Parse(parts[0]),
                Vendor = parts[1],
                Category = parts[2],
                Identifier = parts[3].Trim(),
                IdentifierMustStart = parts.Count > 4 && bool.Parse(parts[4]),
                IdentifierCaseSensitive = parts.Count <= 5 || bool.Parse(parts[5]),
            };
        }

        static string TypeToString(TransactionType transactionType)
        {
            string[] parts = {
                $"\"{transactionType.Id}\"",
                $"\"{transactionType.Vendor}\"",
                $"\"{transactionType.Category}\"",
                $"\"{transactionType.Identifier}\"",
                $"\"{transactionType.IdentifierMustStart}\"",
                $"\"{transactionType.IdentifierCaseSensitive}\""
            };

            return string.Join(",", parts);
        }
    }
}
