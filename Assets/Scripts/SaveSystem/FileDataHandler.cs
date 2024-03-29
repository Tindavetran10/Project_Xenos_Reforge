using System;
using System.IO;
using UnityEngine;

namespace SaveSystem
{
    public class FileDataHandler
    {
        private readonly string _dataDirPath;
        private readonly string _dataFileName;

        private readonly bool _encryptData;
        private const string CodeWord = "Project:L";

        public FileDataHandler(string dataDirPath, string dataFileName, bool encryptData)
        {
            _dataDirPath = dataDirPath;
            _dataFileName = dataFileName;
            _encryptData = encryptData;
        }

        public void Save(GameData data)
        {
            var fullPath = Path.Combine(_dataDirPath, _dataFileName);

            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath) ?? throw new InvalidOperationException());
                
                var dataToStore = JsonUtility.ToJson(data, true);

                if (_encryptData)
                    dataToStore = EncryptDecrypt(dataToStore);

                using var stream = new FileStream(fullPath, FileMode.Create);
                using var writer = new StreamWriter(stream);
                writer.Write(dataToStore);
            }
            catch (Exception e) {
                Debug.LogError("Error on trying to save data to file" + fullPath + "\n" + e);
            }
        }

        public GameData Load()
        {
            var fullPath = Path.Combine(_dataDirPath, _dataFileName);
            GameData loadData = null;

            if (File.Exists(fullPath))
            {
                try
                {
                    string dataToLoad;
                    using (var stream = new FileStream(fullPath, FileMode.Open))
                    {
                        using (var  reader =new StreamReader(stream)) 
                            dataToLoad = reader.ReadToEnd();
                    }

                    if (_encryptData)
                        dataToLoad = EncryptDecrypt(dataToLoad);
                    
                    loadData = JsonUtility.FromJson<GameData>(dataToLoad);
                }
                catch (Exception e) {
                    Debug.LogError("Error on trying to load data from file:" + fullPath + "\n" + e);
                }
            }
            return loadData;
        }

        public void Delete()
        {
            var fullPath = Path.Combine(_dataDirPath, _dataFileName);
            
            if(File.Exists(fullPath))
                File.Delete(fullPath);
        }

        private static string EncryptDecrypt(string data)
        {
            var modifiedData = "";

            for (var i = 0; i < data.Length; i++) 
                modifiedData += (char)(data[i] ^ CodeWord[i % CodeWord.Length]);
            return modifiedData;
        }
    }
}