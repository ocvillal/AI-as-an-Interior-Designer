using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class FurnitureLibrary{
    // public
    public Dictionary<string, List<string>> categories;
    public List<string> tags;
    public List<string> constraints;
    public List<string> models;
    public Dictionary<string, FurnitureData> Basic;
    public Dictionary<string, FurnitureData> Minimalist;
    public Dictionary<string, FurnitureData> Modern;

    public Dictionary<string, FurnitureData> library;
    public Dictionary<string, GameObject> furniturePrefabs;

    // public static FurnitureLibrary CreateFromJSON(string jsonString)
    // {
    //     Debug.Log("LOL");
    //     return JsonUtility.FromJson<FurnitureLibrary>(jsonString);
    // }

    public FurnitureData GetFurniture(string name){
        return Basic.TryGetValue(name, out var ret) ? ret : Minimalist.TryGetValue(name, out ret) ? ret: (Modern.TryGetValue(name, out ret) ? ret: null);
        // return Basic.TryGetValue(name, out var ret) ? ret : (Minimalist.TryGetValue(name, out ret) ? ret: null);
    }

    public FurnitureData GetRandomFurnitureByType(string type){
        categories.TryGetValue(type, out var catalog);
        return (catalog != null) ? GetFurniture(catalog[Random.Range(0, catalog.Count)]) : null;
    }

    public FurnitureData GetRandomFurnitureByMultipleType(params string[] types){
        List<string> catalog = new List<string>();
        foreach (string type in types){

            if (categories.TryGetValue(type, out var val)){
                // Debug.Log(string.Join(", ", val));
                catalog.AddRange(val);
            }

        }
        string key = catalog[Random.Range(0, catalog.Count)];
        // Debug.Log(key);
        return (catalog.Count > 0) ? GetFurniture(key) : null;
    }

    public List<int> GetFurnitureDimensions(string name){
        var ret = GetFurniture(name);
        return (ret != null) ? ret.dimensions : new List<int>();
    }

    public List<string> GetFurnitureTags(string name){
        return Basic.TryGetValue(name, out var ret) ? ret.tags : Minimalist.TryGetValue(name, out ret) ? ret.tags: (Modern.TryGetValue(name, out ret) ? ret.tags: new List<string>());
        // return Basic.TryGetValue(name, out var ret) ? ret.tags : (Minimalist.TryGetValue(name, out ret) ? ret.tags: new List<string>());
    }

    public List<string> GetFurnitureConstraints(string name){
        return Basic.TryGetValue(name, out var ret) ? ret.constraints : Minimalist.TryGetValue(name, out ret) ? ret.constraints : (Modern.TryGetValue(name, out ret) ? ret.constraints : new List<string>());
        // return Basic.TryGetValue(name, out var ret) ? ret.constraints : (Minimalist.TryGetValue(name, out ret) ? ret.constraints : new List<string>());
    }
    
    public List<string> GetFurnitureModels(string name){
        return Basic.TryGetValue(name, out var ret) ? ret.models : Minimalist.TryGetValue(name, out ret) ? ret.models : (Modern.TryGetValue(name, out ret) ? ret.models : new List<string>());
        // return Basic.TryGetValue(name, out var ret) ? ret.models : (Minimalist.TryGetValue(name, out ret) ? ret.models : new List<string>());
    }


    public GameObject GetPrefab(string name){
        return furniturePrefabs[name];
    }

    public void LoadPrefabs(){
        furniturePrefabs = new Dictionary<string, GameObject>();
        foreach (KeyValuePair<string, List<string>> category in categories){
            foreach (string name in category.Value){
                // Debug.Log(string.Format("Livingroom/{0}/{1}", category.Key, name));
                GameObject Hello = (GameObject) Resources.Load(string.Format("Livingroom/{0}/{1}", category.Key, name));
                // Debug.Log(string.Format("{0} {1}", name, Hello.transform.position));
                // Debug.Log(name);
                if (Hello.Equals(null))
                    Debug.Log(string.Format("Livingroom/{0}/{1}", category.Key, name));
                furniturePrefabs[name] = Hello;
            }
        }
    }

    public override string ToString(){
        string ret = "";
        foreach (KeyValuePair<string, FurnitureData> entry in Basic){
            ret += string.Format("{0} : {1}\n", entry.Key, !entry.Value.Equals(null));
        }
        foreach (KeyValuePair<string, FurnitureData> entry in Minimalist){
            ret += string.Format("{0} : {1}\n", entry.Key, !entry.Value.Equals(null));
        }
        foreach (KeyValuePair<string, FurnitureData> entry in Modern){
            ret += string.Format("{0} : {1}\n", entry.Key, !entry.Value.Equals(null));
        }
        return ret;
    }


}