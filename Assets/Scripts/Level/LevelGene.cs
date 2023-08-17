
using Newtonsoft.Json;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Finish by Thursday


using Feature = FurnitureFeature; // name of the furniture, x, y

// Represents an individual inside the population
public class LevelGene {

    //static // What would the json load in?
    static FurnitureLibrary furnitureLibrary = null;

    List<Feature> features;

    bool isValid;

    public LevelGene(){
        if (furnitureLibrary == null){
            LevelGene.LoadAllFurniture();
        }
        features = new List<Feature>();
        Feature feat1 = new Feature("bed_single", 0, 0);
        Feature feat2 = new Feature("door", 0, 0);
        Feature feat3 = new Feature("night_stand", 2.5f, 0.5f);
        Feature feat4 = new Feature("drawer_small", 3.5f, 3f);

        Debug.Log(feat1);
        Debug.Log(feat2);
        Debug.Log(feat3);
        Debug.Log(feat4);

        Debug.Log(feat1.OverlapsWith(feat1));
        Debug.Log(feat1.OverlapsWith(feat2));
        Debug.Log(feat1.OverlapsWith(feat3));
        Debug.Log(feat1.OverlapsWith(feat4));


    }

    static void LoadAllFurniture(){ // Arrian
        LevelGene.furnitureLibrary = JsonConvert.DeserializeObject<FurnitureLibrary>(File.ReadAllText("./TileData/FurnitureData.json"));
        Feature.furnitureLibrary = LevelGene.furnitureLibrary;
        // Debug.Log(string.Join(", ", LevelGene.furnitureLibrary.categories));
        // string val = "";
        // foreach (KeyValuePair<string, List<string>> kvp in LevelGene.furnitureLibrary.categories)
        // {
        //     val += string.Format("Key = {0}, Value = {1}", kvp.Key, string.Join(", ", kvp.Value));
        // }
        // Debug.Log(val);
        // Debug.Log(string.Join(", ", LevelGene.furnitureLibrary.tags));
        // Debug.Log(string.Join(", ", LevelGene.furnitureLibrary.constraints));

        // string dval = "";
        // foreach (KeyValuePair<string, FurnitureData> kvp in LevelGene.furnitureLibrary.furniture_pack_2)
        // {
        //     dval += string.Format("Key = {0}, Value = {1}", kvp.Key, kvp.Value.ToString());
        // }
        // Debug.Log(dval);


    }

    static LevelGene GenerateRandomLevelGene(){ // Arrian
        return new LevelGene();
    }

    static LevelGene GenerateEmptyLevelData(){ // Angela
        LevelGene emptyLevel = new LevelGene();
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                emptyLevel.features.Add(new Feature("empty", i, j));
            }
        }
        return emptyLevel;
    }
    bool isCategory(Feature feature, string category)
    {
        // Get furniture category from JSON

        return false;
    }

    float furnitureArea(Feature feature)
    {
        // Get furniture dimensions from JSON

        return 1.0f;
    }

    bool hasTag(Feature feature, string tag)
    {
        // Check tags from JSON

        return false;
    }

    Dictionary<string, float> Metrics()
    { // THE MEAT (HOw would we define this???? Someone research) Angela
        float balance = 0.0f;

        float harmony = 0.0f;

        float emphasis = 0.0f;
        int emphasisCount = 0;

        float contrast = 0.0f;
        bool hasSmall = false;
        bool hasLarge = false;

        float scale = 0.0f;

        float details = 0.0f;

        float rhythm = 0.0f;
        var rhythmList = new List<string>();

        foreach (var item in features)
        {
            // Balance
            // May check if furniture of the same size are on opposite sides of the room
            if furnitureArea(item > 8.0f) {
                if (hasLarge) {
                    balance = -1.0f;
                } else {
                    hasLarge = true;
                }
            }

            // Harmony
            // This uses the color of the furniture

            // Emphasis
            // This may also use the color of the furniture
            if (isCategory(item, "general"){
                emphasisCount++;
            }

            // Contrast
            if (!hasSmall || !hasLarge)
            {
                float area = furnitureArea(item);
                if (area < 4.0f)
                {
                    hasSmall = true;
                }
                else if (area > 8.0f)
                {
                    hasLarge = true;
                }
            } else {
                contrast = 1.0f;
            }

            // Scale and Proportion
            scale += furnitureArea(item);

            // Deatils
            if (hasTag(item, "decorative"))
            {
                details += 1.0f;
            }

            // Rhythm
            if (!hasTag(item, "essential"))
            {
                if (!rhythmList.Contains(item[0]))
                {
                    rhythmList.Add(item[0]);
                }
                else
                {
                    rhythm += 1.0f;
                }
            }
        }

        // Parabola of fitness -(x - 2)^2 + 4
        emphasis = -((emphasisCount - 2) * (emphasisCount - 2)) + 4;

        var metrics = new Dictionary<string, float>(){
            {"Balance", balance},
            {"Harmony", harmony},
            {"Emphasis", emphasis},
            {"Contrast", contrast},
            {"Scale", scale},
            {"Details", details},
            {"Rhythm", rhythm}
        };

        return metrics;
    }

    float Fitness(){
        var tileMetrics = Metrics();
        float fitness = 0.0f;

        // How heavily each category should affect overall fitness
        float balance = 0.0f;
        float harmony = 0.0f;
        float emphasis = 0.0f;
        float contrast = 0.0f;
        float scale = 0.0f;
        float details = 0.0f;
        float rhythm = 0.0f;

        return fitness;
    }

    bool ValidateSelf(){ // n^2 complexity
        return true;
    }


    public bool FeatureIsWithinBounds(Feature feat){

        return false;
    }

    bool ValidateFeatureAddition(Feature feat, bool allowDuplicates){ // Can I add this item into myself? O(n) complexity // THE MEAT // Arrian
        // go through every piece of furniture in the features
        // validate with the rest of the
        return false;
    }

    LevelGene Mutate(){ // A mutate function // Angela
        // Either
        int actions = Random.Range(0, 4);

        switch (actions)
        {
        // Modifies the placement of an item
            case 0:
                int[,] occupiedSpace = new int[10, 10];
                foreach (var item in features)
                {
                    // Set to 1 if the space is occupied
                }
                // Rotate or move?
                break;

        // Adds a new item
            case 1:
                int[,] occupiedSpace = new int[10, 10];
                foreach(var item in features){
                    // Set to 1 if the space is occupied
                }
                string randomItem = "";
                
                break;

        // Removes an item
            case 2:
                var removableFeatureList = new List<string>();
                foreach(var item in features) {
                    if (!hasTag(item, "essential") {
                        removableFeatureList.Add(item[0]);
                    }
                }
                if (removableFeatureList.Count > 0) {
                    int removeIndex = Random.Range(0, removableFeatureList.Count);
                    features.Remove(removableFeatureList[removeIndex]);
                }
                break;

            // Do nothing
            default:
                break;
        }
        return new LevelGene();
    }

    LevelGene PlaceObject(){ // Alan
        return new LevelGene();
    }

    LevelGene RemoveObject(){ // Octavio
        return new LevelGene();
    }

    List<LevelGene> GenerateChildren(LevelGene other) { // Alan
        // crossover


        // validation is needed
        // Consider feasible and infeasible population?
        return new List<LevelGene>();

    }

}