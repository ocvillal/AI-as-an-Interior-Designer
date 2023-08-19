
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

    public List<Feature> features;
    public Vector2Int dimensions;

    private int[,] grid;
    public int[,] Grid {
        get { return grid; }
        set {
            grid = value;
        }
    }


    bool isValid;

    public LevelGene(Vector2Int dimensions){
        if (furnitureLibrary == null){
            LevelGene.LoadAllFurniture();
        }
        this.dimensions = dimensions;
        features = new List<Feature>();
        grid = new int[(int) dimensions.x, (int) dimensions.y]; // Top left (0,0) Bottom right (n, n)
    }

    public LevelGene(LevelGene gene){
        if (furnitureLibrary == null){
            LevelGene.LoadAllFurniture();
        }

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

    public static LevelGene GenerateRandomLevelGene(Vector2Int dims, int num_feat){
        LevelGene randomLevel = new LevelGene(dims);

        for (int i = 0; i < num_feat; i++){
            FurnitureData furnitureData = LevelGene.furnitureLibrary.GetRandomFurnitureByMultipleType("livingroom", "general");
            // Debug.Log(furnitureData.ToString());
            Feature feat = null;
            if (furnitureData != null)
                feat = randomLevel.GenerateValidFeature(furnitureData);
            // Debug.Log(feat);
            // Debug.Log(feat != null);

            if (feat != null){
                randomLevel = randomLevel.TryPlaceObject(feat);
            }
        }

        return randomLevel;
    }

    public static LevelGene GenerateEmptyLevelData(Vector2Int dims){
        LevelGene emptyLevel = new LevelGene(dims);
        // for (int i = 0; i < 10; i++)
        // {
        //     for (int j = 0; j < 10; j++)
        //     {
        //         emptyLevel.features.Add(new Feature("empty", i, j));
        //     }
        // }
        return emptyLevel;
    }

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
        // Debug.Log(ret);
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


                // Rotate or move? A; Both
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
        for(int y = 0; y < dimensions.y; y++){
            for(int x = 0; x < dimensions.x; x++){
                if(grid[y, x] == 0){
                    availableTiles.Add( new (x, y));
                }
            }
        }
        return availableTiles;
    }

    // Returns where the top left corner an object can be  in
    List<(int, int)> GetValidTiles(FurnitureData feat, float orientation=0){
        List<int> furn_dims = feat.dimensions;


        List<(int, int)> availableTiles = GetAvailableTiles();

        List<(int, int)> validTiles = new List<(int, int)>();

        int dim_x = (orientation == 0 || orientation == 180) ? furn_dims[0] : furn_dims[1];
        int dim_y = (orientation == 90 || orientation == 270) ? furn_dims[1] : furn_dims[0];
        foreach((int, int) tile in availableTiles){
            bool isValid = true;
            for (int y = tile.Item2; y < tile.Item2 + dim_y; y++){
                for (int x = tile.Item1; x < tile.Item1 + dim_x; x++){
                    if (x >= dimensions.x || y >= dimensions.y){
                        isValid = false;
                        break;
                    }

                    // Debug.Log(string.Format("{0} {1}", x, y));
                    isValid &= grid[y, x] == 0;
                    // Some extra validation needs to be done here... or maybe not...

                    // Constraints

                    if (!isValid) break;
                }
                if (!isValid) break;
            }
            if (isValid) validTiles.Add(tile);
        }

        return validTiles;
    }

    public Feature GenerateValidFeature(FurnitureData featureData){
        // pick random orientation

        List<(int, int)> validTiles = GetValidTiles(featureData);

        Feature feature = null;
        if (validTiles.Count > 0){
            (int, int) randomTile = validTiles[Random.Range(0, validTiles.Count)];
            feature = new Feature(featureData, randomTile.Item1, randomTile.Item2);
        }

        return feature;
    }

    public override string ToString() {
        string [,] ret_grid = new string[dimensions[0], dimensions[1]];
        string ret = "";


        ret += string.Join(", ", features)  + "\n";

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
        List<LevelGene> children = new List<LevelGene>();

        int crossoverPoint = Random.Range(0, features.Count); // Choose a random crossover point

        // Create the first child by combining the features from the two parents
        LevelGene child1 = new LevelGene(this);
        for (int i = 0; i < crossoverPoint; i++)
        {
            Feature featureToAdd = other.features[i]; // Take feature from the other parent
            if (child1.ValidateFeatureAddition(featureToAdd, true))
            {
                child1 = child1.PlaceObject(featureToAdd); // Add the feature to the child if valid
            }
        }
        children.Add(child1);

        // Create the second child by combining the features from the two parents
        LevelGene child2 = new LevelGene(other);
        for (int i = crossoverPoint; i < other.features.Count; i++)
        {
            Feature featureToAdd = features[i]; // Take feature from this parent
            if (child2.ValidateFeatureAddition(featureToAdd, true))
            {
                child2 = child2.PlaceObject(featureToAdd); // Add the feature to the child if valid
            }
        }
        children.Add(child2);

        return children;

        // validation is needed
        // Consider feasible and infeasible population?

    }

}
