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

}