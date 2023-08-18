
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
    Vector2Int dimensions;

    int[,] grid;

    bool isValid;

    public LevelGene(Vector2Int dimensions){
        if (furnitureLibrary == null){
            LevelGene.LoadAllFurniture();
        }
        this.dimensions = dimensions;


        features = new List<Feature>();
        Feature feat1 = new Feature("bed_single", 0, 2);
        Feature feat2 = new Feature("door", 0, 2);
        Feature feat3 = new Feature("night_stand", 3, 3);
        Feature feat4 = new Feature("drawer_small", 4, 0);

        Debug.Log(feat1);
        Debug.Log(feat2);
        Debug.Log(feat3);
        Debug.Log(feat4);

        // Debug.Log(feat1.OverlapsWith(feat1));
        // Debug.Log(feat1.OverlapsWith(feat2));
        // Debug.Log(feat1.OverlapsWith(feat3));
        // Debug.Log(feat1.OverlapsWith(feat4));

        grid = new int[(int) dimensions.x, (int) dimensions.y]; // Top left (0,0) Bottom right (n, n)


        Debug.Log(ToString());

        LevelGene try1 = TryPlaceObject(feat1);
        Debug.Log(try1.ToString());

        LevelGene try2 = try1.TryPlaceObject(feat2);
        Debug.Log(try2.ToString());

        LevelGene try3 = try2.TryPlaceObject(feat3);
        Debug.Log(try3.ToString());

        LevelGene try4 = try3.TryPlaceObject(feat4);
        Debug.Log(try4.ToString());
    }

    public LevelGene(LevelGene gene){
        this.features = gene.features.ConvertAll(feat => new Feature(feat));
        this.dimensions = new Vector2Int(gene.dimensions.x, gene.dimensions.y);
        this.grid = new int[(int) dimensions.x, (int) dimensions.y];
        for (int y = 0; y < dimensions.y; y++)
            for (int x = 0; x < dimensions.x; x++)
                this.grid[y, x] = gene.grid[y, x];
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

    // static LevelGene GenerateRandomLevelGene(){ // Arrian
    //     return new LevelGene();
    // }

    // static LevelGene GenerateEmptyLevelData(){ // Angela
    //     LevelGene emptyLevel = new LevelGene();
    //     for (int i = 0; i < 10; i++)
    //     {
    //         for (int j = 0; j < 10; j++)
    //         {
    //             emptyLevel.features.Add(new Feature("empty", i, j));
    //         }
    //     }
    //     return emptyLevel;
    // }

    bool isCategory(Feature feature, string category){
        // Get furniture category from JSON

        return false;
    }

    Dictionary<string, float> Metrics()
    { // THE MEAT (HOw would we define this???? Someone research) Angela
        float balance   = 0.0f;
        float harmony   = 0.0f;
        float emphasis  = 0.0f;
        float contrast  = 0.0f;
        float scale     = 0.0f;
        float details   = 0.0f;
        float rhythm    = 0.0f;

        int emphasisCount = 0;

        bool hasSmall = false;
        bool hasLarge = false;

        var rhythmList = new List<string>();

        foreach (var feature in features)
        {
            float area = feature.GetArea();

            // Balance
            // May check if furniture of the same size are on opposite sides of the room
            if (area > 8.0f){
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
            if (isCategory(feature, "general")){
                emphasisCount++;
            }

            // Contrast
            if (!hasSmall || !hasLarge){
                if (area < 4.0f){
                    hasSmall = true;
                }
                else if (area > 8.0f){
                    hasLarge = true;
                }
            } else {
                contrast = 1.0f;
            }

            // Scale and Proportion
            scale += area;

            // Deatils - Decorative furniture
            if (feature.HasTag("decorative")){
                details += 1.0f;
            }

            // Rhythm - Small and duplicate non-essential furniture
            if (!feature.HasTag("essential")){
                if (!rhythmList.Contains(feature.name)){
                    rhythmList.Add(feature.name);
                }
                else {
                    rhythm += 1.0f;
                }
            }
        }

        // Parabola of fitness -(x - 2)^2 + 4
        emphasis = -((emphasisCount - 2) * (emphasisCount - 2)) + 4;

        var metrics = new Dictionary<string, float>(){
            {"balance", balance},
            {"harmony", harmony},
            {"emphasis", emphasis},
            {"contrast", contrast},
            {"scale", scale},
            {"details", details},
            {"rhythm", rhythm}
        };

        return metrics;
    }

    public float Fitness(){
        var tileMetrics = Metrics();
        float fitness = 0.0f;

        // How heavily each category should affect overall fitness
        float balance   = 0.0f;
        float harmony   = 0.0f;
        float emphasis  = 0.0f;
        float contrast  = 0.0f;
        float scale     = 0.0f;
        float details   = 0.0f;
        float rhythm    = 0.0f;

        fitness += tileMetrics["Balance"]   * balance;
        fitness += tileMetrics["harmony"]   * harmony;
        fitness += tileMetrics["emphasis"]  * emphasis;
        fitness += tileMetrics["contrast"]  * contrast;
        fitness += tileMetrics["scale"]     * scale;
        fitness += tileMetrics["details"]   * details;
        fitness += tileMetrics["rhythm"]    * rhythm;

        return fitness;
    }

    bool ValidateSelf(){ // n^2 complexity
        return true;
    }


    public bool FeatureIsWithinBounds(Feature feat){

        return false;
    }

    bool CheckNoOverlaps(Feature feat){
        bool ret = true;
        foreach (Feature f in features){
            if (f == feat) continue;
            ret &= !feat.OverlapsWith(f);
            if (!ret) break;
        }
        return ret;
    }

    bool ValidateFeatureAddition(Feature feat, bool allowDuplicates){ // Can I add this item into myself? O(n) complexity // THE MEAT // Arrian
        bool ret =  true;

        ret &= CheckNoOverlaps(feat);
        // go through every piece of furniture in the features
        // validate with the rest of the
        Debug.Log(ret);
        return ret;
    }

    LevelGene Mutate(){ // A mutate function // Angela
        // Either
        int actions = Random.Range(0, 4);
        int[,] occupiedSpace = new int[10, 10];
        foreach (var feature in features){
                // Set to 1 if the space is occupied
            }
        switch (actions)
        {
        // Modifies the placement of an item
            case 0:


                // Rotate or move?
                break;

        // Adds a new item
            case 1:
                string randomItem = "";
                break;

        // Removes an item
            case 2:
                var removableFeatureList = new List<string>();
                foreach(var feature in features) {
                    if (!feature.HasTag("essential")) {
                        removableFeatureList.Add(feature.name);
                    }
                }
                if (removableFeatureList.Count > 0) {
                    int removeIndex = Random.Range(0, removableFeatureList.Count);
                    features.RemoveAt(removeIndex);
                }
                break;

            // Do nothing
            default:
                break;
        }
        return null;
    }

    List<(int, int)> GetAvailableTiles(){
        List<(int, int)> availableTiles = new List<(int,int)>();
        for(int x = 0; x < 10; x++){
            for(int y = 0; y < 10; y++){
                if(grid[x,y] == 0){
                    availableTiles.Add( new (x, y));
                }
            }
        }
        return availableTiles;
    }

    // Returns where the top left corner an object can be  in
    List<(int, int)> GetValidTiles(string name, float orientation=0){
        List<int> furn_dims = furnitureLibrary.GetFurnitureDimensions(name);

        List<(int, int)> availableTiles = GetAvailableTiles();

        List<(int, int)> validTiles = new List<(int, int)>();

        int dim_x = (orientation == 0 || orientation == 180) ? furn_dims[0] : furn_dims[1];
        int dim_y = (orientation == 90 || orientation == 270) ? furn_dims[1] : furn_dims[0];
        foreach((int, int) tile in availableTiles){
            bool isValid = true;
            for (int y = tile.Item2; y < tile.Item2 + dim_y; y++){
                for (int x = tile.Item1; x < tile.Item1 + dim_x; x++){
                    isValid &= grid[y, x] == 0;
                    if (!isValid) break;
                }
            }
            if (isValid) validTiles.Add(tile);
        }

        return validTiles;
    }

    public override string ToString() {
        string [,] ret_grid = new string[dimensions[0], dimensions[1]];
        string ret = "";

        foreach (Feature feat in features){
            List<int> furn_dims = feat.dimensions;
            for (int y = feat.position.y; y < feat.position.y + furn_dims[1]; y++){
                for (int x = feat.position.x; x < feat.position.x + furn_dims[0]; x++){
                    ret_grid[y, x] = new string(feat.name[0], 1);
                }
            }
        }

        for (int j = 0; j < dimensions.y; j++){
            for (int i = 0; i < dimensions.x; i++){
                ret += (ret_grid[j, i] != null) ? string.Format(" [{0}] ", ret_grid[j, i]) : " [-] ";
            }
            ret += "\n";
        }

        return ret;
    }
    LevelGene TryPlaceObject(Feature feat){
        return (ValidateFeatureAddition(feat, false)) ? PlaceObject(feat) : this;
    }


    LevelGene PlaceObject(Feature feature){ //
        LevelGene ret = new LevelGene(this);
        List<int> furn_dims = feature.dimensions;

        // Rectangular features
        List<int> dims = feature.dimensions;
        int dim_x = dims[0];
        int dim_y = dims[1];
        if (dims[0] != dims[1]) {
            dim_x = (feature.orientation == 0 || feature.orientation == 180) ? furn_dims[0] : furn_dims[1];
            dim_y = (feature.orientation == 90 || feature.orientation == 270) ? furn_dims[1] : furn_dims[0];
        }

        for (int y = feature.position.y; y < feature.position.y + dim_y; y++){
            for (int x = feature.position.x; x < feature.position.x + dim_x; x++){
                ret.grid[y, x] = 1;
            }
        }

        ret.features.Add(feature);
        return ret;

    }

    LevelGene RemoveObject(Feature feature){ // Octavio
        LevelGene ret = new LevelGene(this);
        List<int> furn_dims = feature.dimensions;

        // What if there's more than one of a particular feature
        // A: features are always unique by comparison

        // Update grid
        if(!(ret.features.Contains(feature))) {
            return null;
        }

        // Rectangular features
        List<int> dims = feature.dimensions;
        int dim_x = dims[0];
        int dim_y = dims[1];
        if (dims[0] != dims[1]) {
            dim_x = (feature.orientation == 0 || feature.orientation == 180) ? furn_dims[0] : furn_dims[1];
            dim_y = (feature.orientation == 90 || feature.orientation == 270) ? furn_dims[1] : furn_dims[0];
        }

        for (int y = feature.position.y; y < feature.position.y + dim_y; y++){
            for (int x = feature.position.x; x < feature.position.x + dim_x; x++){
                ret.grid[y, x] = 0;
            }
        }

        ret.features.Remove(feature);
        return ret;
    }

    public List<LevelGene> GenerateChildren(LevelGene other) { // Alan
        // crossover


        // validation is needed
        // Consider feasible and infeasible population?
        return new List<LevelGene>();

    }

}