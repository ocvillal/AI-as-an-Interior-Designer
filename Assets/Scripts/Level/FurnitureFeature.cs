using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FurnitureFeature{
    public string name;
    public Vector3 position;
    public float orientation;

    public FurnitureFeature(string name, float x,  float z){
        this.name = name;
        this.position = new Vector3(x, 0, z);
        this.orientation = 0;
    }
}