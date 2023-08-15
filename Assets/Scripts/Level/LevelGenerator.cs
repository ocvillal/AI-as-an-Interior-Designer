using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public int numLevels = 3;
    public Level level;

    public void CreateLevel(Vector2 dim, Vector3 pos){
        
        Instantiate(level, pos, Quaternion.identity);
        level.Dimensions = dim;
        level.Position = pos;
        level.Grid = new int[(int) dim.y, (int) dim.x];
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
