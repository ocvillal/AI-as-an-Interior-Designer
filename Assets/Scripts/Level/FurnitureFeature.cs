using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FurnitureFeature{

    public static FurnitureLibrary furnitureLibrary = null;

    public string name;
    public Vector3 position;
    public Vector3 dimensions;
    public float orientation;



    public FurnitureFeature(string name, float x,  float z){
        this.name = name;
        this.position = new Vector3(x, 0, z);
        this.orientation = 0;

        List<int> dims = furnitureLibrary.GetFurnitureDimensions(name);
        this.dimensions = new Vector3(dims[0], 0, dims[1]);
    }

    public bool OverlapsWith(FurnitureFeature other){
        // Simple AABB check
        Vector3 my_tl = this.position - this.dimensions / 2;
        Vector3 my_br = this.position + this.dimensions / 2;
        Vector3 other_tl = other.position - other.dimensions / 2;
        Vector3 other_br = other.position + other.dimensions / 2;

        Debug.Log(string.Format("{0} vs {1}", name, other.name));
        Debug.Log(string.Format("{0} < {1}", my_tl.x, other_br.x));
        Debug.Log(string.Format("{0} > {1}", my_br.x, other_tl.x));
        Debug.Log(string.Format("{0} < {1}", my_tl.z, other_br.z));
        Debug.Log(string.Format("{0} > {1}", my_br.z, other_tl.z));



        return my_tl.x  < other_br.x &&
                my_br.x > other_tl.x &&
                my_tl.z  < other_br.z &&
                my_br.z > other_tl.z
                ;
    }

    public override string ToString(){
        return string.Format("{0} TL: {1} - BR: {2} ", name, this.position - this.dimensions / 2, this.position + this.dimensions / 2);
    }
}