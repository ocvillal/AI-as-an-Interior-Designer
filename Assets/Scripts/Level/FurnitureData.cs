using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class FurnitureData{
    public string name;
    public List<int> dimensions;
    public List<string> constraints;
    public List<string> tags;
    // public static FurnitureData CreateFromJSON(string jsonString)
    // {
    //     return JsonUtility.FromJson<FurnitureData>(jsonString);
    // }
    public override string ToString(){
        return name + string.Format(" ({0}, {1})", dimensions[0], dimensions[1]);
    }
}