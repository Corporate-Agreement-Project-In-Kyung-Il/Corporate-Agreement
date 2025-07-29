using System.Collections.Generic;
using System.IO;
using _03.Script.엄시형.Stage.DTO;
using UnityEngine;

namespace _03.Script.엄시형.Util
{
    public static class AreaPatternPersistenceManager
    {
        public static string fullPath 
            = Path.Combine(Application.dataPath, "05.DataTable", "AreaPattern.json");
            
        public static void WriteAsJSON(List<AreaPatternDTO> patterns)
        {
            AllAreaPatternDTO allPatterns = new AllAreaPatternDTO(patterns);
            
            string json = JsonUtility.ToJson(allPatterns);

            Debug.Log(fullPath);
            File.WriteAllText(fullPath, json);
        }

        public static bool TryReadFromJson(out List<AreaPatternDTO> areaPatternList)
        {
            areaPatternList = new List<AreaPatternDTO>();

            if (File.Exists(fullPath) == false) return false;
            
            areaPatternList = JsonUtility.FromJson<AllAreaPatternDTO>(
                File.ReadAllText(fullPath)).AreaPatternList;
            
            return true;
        }
    }
}

namespace _03.Script.엄시형.Util.V2
{
    public static class PersistManager
    {
        // public static string fullPath 
        //     = Path.Combine(Application.dataPath, "05.DataTable", "AreaPattern.json");
            
        public static void WriteAsJson<T>(T data, string fullPath)
        {
            string json = JsonUtility.ToJson(data);

            Debug.Log(fullPath);
            File.WriteAllText(fullPath, json);
        }

        public static bool TryReadFromJson<T>(out T data, string fullPath)
        {
            data = default;

            if (File.Exists(fullPath) == false) return false;
            
            string json = File.ReadAllText(fullPath);
            data = JsonUtility.FromJson<T>(json);
            
            return true;
        }
    }
}