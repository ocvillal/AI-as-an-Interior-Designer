using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FurnitureFeature{

    public static FurnitureLibrary furnitureLibrary = null;

    public string name;
    public Vector2Int position; // this is the top left tile position (0,0) is top left and (n, n) bottom right
    public List<int> dimensions;
    public float orientation;

    static public bool operator ==(FurnitureFeature item1, FurnitureFeature item2){
        return item1 != null && item2 != null &&
                item1.name == item2.name &&
                item1.position == item2.position &&
                item1.dimensions == item2.dimensions &&
                 item1.orientation == item2.orientation;
    }


    static public bool operator !=(FurnitureFeature item1, FurnitureFeature item2){
        return item1 == null || item2 == null ||
                item1.name != item2.name ||
                item1.position != item2.position ||
                item1.dimensions != item2.dimensions ||
                 item1.orientation != item2.orientation;
    }

    // Position is the top left tile of furniture

    public FurnitureFeature(string name, int x,  int z){
        this.name = name;
        this.position = new Vector2Int(x, z);
        this.orientation = 0;

        List<int> dims = furnitureLibrary.GetFurnitureDimensions(name);
        this.dimensions = new List<int>() {
                dims[0],
                dims[1]
            };
    }

    public FurnitureFeature(FurnitureFeature feat){
        this.name = feat.name;
        this.position = new Vector2Int(feat.position.x, feat.position.y);
        this.orientation = feat.orientation;
        this.dimensions = new List<int>() {
            feat.dimensions[0],
            feat.dimensions[1]
        };
    }


   public override bool Equals(System.Object obj)
   {
      //Check for null and compare run-time types.
      if ((obj == null) || ! this.GetType().Equals(obj.GetType()))
      {
         return false;
      }
      else {
         FurnitureFeature other = (FurnitureFeature) obj;
         return this == other;
      }
   }

   public override int GetHashCode() // Change this
   {
      return ((int )position.x << 2) ^ (int)position.y;
   }

    // Define everything based off top left corner
    public bool OverlapsWith(FurnitureFeature other){
        // Simple AABB check
        Vector2Int my_tl = this.position;
        Vector2Int my_br = this.position + new Vector2Int(this.dimensions[0], this.dimensions[1]);
        Vector2Int other_tl = other.position;
        Vector2Int other_br = other.position + new Vector2Int(other.dimensions[0], other.dimensions[1]);

        Debug.Log(string.Format("{0} vs {1}", name, other.name));
        Debug.Log(string.Format("{0} < {1}", my_tl.x, other_br.x));
        Debug.Log(string.Format("{0} > {1}", my_br.x, other_tl.x));
        Debug.Log(string.Format("{0} < {1}", my_tl.y, other_br.y));
        Debug.Log(string.Format("{0} > {1}", my_br.y, other_tl.y));



        return my_tl.x  < other_br.x &&
                my_br.x > other_tl.x &&
                my_tl.y  < other_br.y &&
                my_br.y > other_tl.y
                ;
    }

    public float GetArea(){
        return dimensions[0] * dimensions[1];
    }

    public bool HasTag(string tag){
        return furnitureLibrary.GetFurnitureTags(name).Contains(tag);
    }


    public override string ToString(){
        return string.Format("{0} TL: {1} - BR: {2} ", name,
                 this.position - new Vector2Int(this.dimensions[0], this.dimensions[1])/ 2,
                 this.position + new Vector2Int(this.dimensions[0], this.dimensions[1])/ 2);
    }
}