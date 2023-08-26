using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace SaveSystem
{
    public class BinarySaveSystem : ISaveSystem
    {
        private readonly string _filePath;

        public BinarySaveSystem(string data)
        {
            _filePath = Application.persistentDataPath + $"/{data}.dat";
        }

        public void Save(ISaveble data)
        {
            using FileStream file = File.Create(_filePath);
            new BinaryFormatter().Serialize(file, data);
        }

        public ISaveble Load()
        {
            ISaveble saveData = null;
            using FileStream file = File.Open(_filePath, FileMode.OpenOrCreate);
            if (file.Length != 0)
            {
                file.Position = 0;
                object loadedData = new BinaryFormatter().Deserialize(file);
                saveData = (ISaveble)loadedData;
            }

            return saveData;
        }

        public static void DeleteAllSaves()
        {
            DirectoryInfo dirInfo = new DirectoryInfo(Application.persistentDataPath);

            foreach (FileInfo file in dirInfo.GetFiles())
            {

                if (file.Name.EndsWith("dat"))
                {
                    Debug.Log("DELETE FILE " + file.Name);
                    file.Delete();
                }

            }
        }
    }
}