using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class FurnitureLibrary{
    // public
    public Dictionary<string, List<string>> categories;
    public List<string> tags;
    public List<string> constraints;
    public Dictionary<string, FurnitureData> furniture_pack_1;
    public Dictionary<string, FurnitureData> furniture_pack_2;

    public Dictionary<string, FurnitureData> library;

    // public static FurnitureLibrary CreateFromJSON(string jsonString)
    // {
    //     Debug.Log("LOL");
    //     return JsonUtility.FromJson<FurnitureLibrary>(jsonString);
    // }

    public List<int> GetFurnitureDimensions(string name){
        return furniture_pack_1.TryGetValue(name, out var ret) ? ret.dimensions : furniture_pack_2.TryGetValue(name, out ret) ? ret.dimensions : new List<int>();
    }


}