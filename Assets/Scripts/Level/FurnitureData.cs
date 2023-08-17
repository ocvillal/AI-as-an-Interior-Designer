using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class FurnitureData{
    string name;
    Tuple<int, int> dimensions;
    List<string> constraints;
    List<string> tags;
}