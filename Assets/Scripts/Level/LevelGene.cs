
using Newtonsoft.Json;
using System.Linq;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Finish by Thursday


using Feature = FurnitureFeature; // name of the furniture, x, y

// Represents an individual inside the population
public class LevelGene {

    //static // What would the json load in?
    public static FurnitureLibrary furnitureLibrary = null;

    public List<Feature> features;
    public Vector2Int dimensions;

    public float fitness = float.NaN;

    private int[,] grid;
    public int[,] Grid {
        get { return grid; }
        set {
            grid = value;
        }
    }


    // bool isValid = true;
    int fails = 0;
    bool isFull = false;

    public LevelGene(Vector2Int dimensions){
        if (furnitureLibrary == null){
            LevelGene.LoadAllFurniture();
        }
        this.dimensions = dimensions;
        features = new List<Feature>();
        grid = new int[(int) dimensions.x, (int) dimensions.y]; // Top left (0,0) Bottom right (n, n)
        this.fitness = float.NaN;
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
        this.fitness = float.NaN;
    }

    static void LoadAllFurniture(){ // Arrian
        LevelGene.furnitureLibrary = JsonConvert.DeserializeObject<FurnitureLibrary>(File.ReadAllText("./TileData/FutureData(Octavio).json"));
        LevelGene.furnitureLibrary.LoadPrefabs();
        Feature.furnitureLibrary = LevelGene.furnitureLibrary;
        Debug.Log(LevelGene.furnitureLibrary.ToString());
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
        int fail = 0;
        for (int i = 0; i < num_feat; i++){
            // FurnitureData furnitureData = LevelGene.furnitureLibrary.GetRandomFurnitureByMultipleType("Basic", "Minimalist");
            FurnitureData furnitureData = LevelGene.furnitureLibrary.GetFurniture("ottoman_Modern");
            // Debug.Log(furnitureData);
            // Debug.Log(furnitureData.ToString());
            Feature feat = null;
            if (furnitureData != null)
                feat = randomLevel.GenerateValidFeature(furnitureData);
                // Debug.Log(feat);
                // Debug.Log(feat != null);

            if (feat != null){
                randomLevel = randomLevel.TryPlaceObject(feat);
            } else {
                fail += 1;
            }
        }
        // Debug.Log(string.Format("{0} fails out of {1}", fail, num_feat));
        randomLevel.Fitness();

        return randomLevel;
    }

    public static LevelGene GenerateEmptyLevelGene(Vector2Int dims){
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
        float diversity = 0.0f;

        int emphasisCount = 0;

        bool hasSmall = false;
        bool hasLarge = false;

        var colorList = new List<string>();
        var rhythmList = new List<string>();


        // Balance
        var top = new List<Feature>();
        var bot = new List<Feature>();
        var lef = new List<Feature>();
        var rig = new List<Feature>();

        foreach (var feature in features)
        {
            if (feature.position.x + feature.dimensions[0] <= 3) lef.Add(feature);
            if (feature.position.y + feature.dimensions[1] <= 3) top.Add(feature);
            if (feature.position.x >= 5) rig.Add(feature);
            if (feature.position.y >= 5) bot.Add(feature);

        }

        float topArea = top.Sum(x => x.GetArea());
        float botArea = bot.Sum(x => x.GetArea());
        float lefArea = lef.Sum(x => x.GetArea());
        float rigArea = rig.Sum(x => x.GetArea());


        float topbot = (botArea > topArea) ? topArea / botArea : botArea / topArea;
        float lefrig = (rigArea > lefArea) ? lefArea / rigArea : rigArea / lefArea;


        if (float.IsNaN(topbot)){
            topbot = 0;
        }
        if (float.IsNaN(lefrig)){
            lefrig = 0;
        }

        balance += topbot;
        balance += lefrig;

        // Debug.Log(string.Format("{0} ? {1} = {2}; {3} ? {4} = {5}, {2} + {5} = {6}", topArea, botArea, topbot, lefArea, rigArea, lefrig, balance));


        // foreach (Feature topfeat in top){
        //     foreach(Feature botfeat in bot){
        //         balance += (Mathf.Abs(topfeat.GetArea() - botfeat.GetArea()) < 2) ? 1 : 0;
        //     }
        // }

        // foreach (Feature leffeat in lef){
        //     foreach(Feature rigfeat in rig){
        //         balance += (Mathf.Abs(leffeat.GetArea() - rigfeat.GetArea()) < 2) ? 1 : 0;

        //     }
        // }

        float totArea = 0;
        foreach (var feature in features)
        {
            float area = feature.GetArea();

            // Balance
            // May check if furniture of the same size are on opposite sides of the room


            // For every feature, organize it into quadrants

            // if (area > 8.0f){
            //     if (hasLarge) {
            //         balance = -1.0f;
            //     } else {
            //         hasLarge = true;
            //     }
            // }

            // Harmony
            // Common colors in furniture
            // if (!colorList.Contains(feature.color)) {
            //     colorList.Add(feature.color);
            // } else {
            //     harmony += 1.0f;
            // }

            // Emphasis
            if (feature.HasTag("essential")){
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

            totArea += area;

            // Deatils - Decorative furniture
            if (feature.HasTag("decorative")){
                details += 1.0f;
            }

            // Rhythm - Small and duplicate non-essential furniture
            if (!feature.HasTag("essential")){
                if (feature.GetArea() < 2f){
                    rhythm += 1.0f;
                }
                // if (!rhythmList.Contains(feature.name)){
                //     rhythmList.Add(feature.name);
                // }
                // else {
                //     rhythm += 1.0f;
                // }
            }

            foreach (var other in features){
                if (feature == other)
                    continue;
                if (other.name != feature.name)
                    diversity += 1;
            }

            diversity /=2;
        }

        // Parabola of fitness -(x - 2)^2 + 4
        emphasis = -((emphasisCount - 2) * (emphasisCount - 2)) + 4;

        // Total “footprint” of furniture pieces should not exceed half of the available space
        scale = (totArea > dimensions.x*dimensions.y * 0.5) ? -1 : 0;

        var metrics = new Dictionary<string, float>(){
            {"balance", balance},
            {"harmony", harmony},
            {"emphasis", emphasis},
            {"contrast", contrast},
            {"scale", scale},
            {"details", details},
            {"rhythm", rhythm},
            {"diversity", diversity}
        };

        return metrics;
    }

    public float Fitness(){
        // if (!float.IsNaN(fitness)){
        //     return fitness;
        // }
        var tileMetrics = Metrics();
        fitness = 0.0f;

        // How heavily each category should affect overall fitness
        float balance   = 1.0f;
        // float harmony   = 2.0f;
        float emphasis  = 2.0f;
        float contrast  = 1.0f;
        float scale     = 0.5f;
        float details  = 0.5f;
        float rhythm    = 0.5f;
        float diversity = 0.1f;

        fitness += tileMetrics["balance"]   * balance;
        // fitness += tileMetrics["harmony"]   * harmony;
        // fitness += tileMetrics["emphasis"]  * emphasis;
        fitness += tileMetrics["contrast"]  * contrast;
        fitness += tileMetrics["scale"]     * scale;
        // fitness += tileMetrics["details"]   * details;
        // fitness += tileMetrics["rhythm"]    * rhythm;
        fitness += tileMetrics["diversity"]    * diversity;


        // fitness += (features.Any(f => f.HasTag("seat"))) ? 3f : 0f;

        // Connectability
        float TOTAL_EDGES = 2* dimensions.x * dimensions.y - dimensions.y - dimensions.x;
        float edges = 0f;
        float emptyArea = 0f;

        for (int j = 0; j < dimensions.y; j++){
            for (int i = 0; i < dimensions.x; i++){
                emptyArea += (grid[j, i] == 1) ? 0f : 1f;
                // Check right and down
                if (i + 1 < dimensions.x){
                    edges += (grid[j, i+1] == 1) ? 0f : 1f;
                }
                if (j + 1 < dimensions.y){
                    edges += (grid[j+1, i] == 1) ? 0f : 1f;
                }

            }
        }

        fitness += 0.2f * edges / TOTAL_EDGES;
        fitness += 0.5f*(1.0f - emptyArea /(dimensions.x * dimensions.y));

        // Debug.Log(string.Format("{0} {1} {2} {3} {4}", fitness, emptyArea,(dimensions.x * dimensions.y), emptyArea /(dimensions.x * dimensions.y), ToString()));
        return fitness;
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

        FurnitureData featData = furnitureLibrary.GetFurniture(feat.name);

        if (feat.HasTag("unique")){
            ret &= features.All(f => f.name != feat.name);
        }

        // Checks if it is of similar model stylistic purpose or checks if there is already a furniture piece of the same style just different size
        // if(feat.HasModel("first") || feat.HasModel("second") || feat.HasModel("third")){
        //     foreach (Feature f in features){
        //         // model 1
        //         if((!f.HasModel("first")) && (feat.HasModel("first"))){
        //             ret = false;
        //         }
        //         else if((!f.HasModel("second")) && (feat.HasModel("second"))){
        //             ret = false;
        //         }
        //         else if((!f.HasModel("third")) && (feat.HasModel("third"))){
        //             ret = false;
        //         }

        //     }
        // }

        // Checks if there is already a different type of furniture (Ex if we have a coffee table no need for an ottoman)
        if (feat.HasModel("middle_table")) {
            foreach (var f in features){
                if(f.HasModel("middle_table")){
                    ret = false;
                }
            }
        }

        // More intensive computation
        ret &= GetValidTiles(featData, feat.orientation).Contains(new (feat.position.x, feat.position.y));

        // ret &= CheckNoOverlaps(feat);
        // go through every piece of furniture in the features
        // validate with the rest of the
        // Debug.Log(ret);
        return ret;
    }

    public LevelGene Mutate(){ // A mutate function
        // Either
        LevelGene mutatedGene = new LevelGene(this);

        // Annealing based off feature count
        // int start = 0;
        // int end = 3;

        float v = UnityEngine.Random.Range(0f, 1f);

        float removeProb = 0.025f * features.Count;
        float addProb = removeProb + 0.05f * Mathf.Max(10f - features.Count, 0f) + 0.1f;
        int actions = 2; //Random.Range(start, end);

        if (v < removeProb){
            actions = 0;
        } else if (v < addProb){
            actions = 1;
        }


        // if (features.Count == 0) start = 3;
        // if (features.Count >= 10) end = 2;
        // if (features.Count == 0) end = 1;
        // if (features.Count == 0) end = 1;


        // if (features.Count > 9) actions = actions % 1;

        // if (features.Count > 8) actions = actions % 2;

        switch (actions)
        {
        // Removes an item
            case 0:
                var removableFeatureList = new List<int>();
                for(int i = 0; i < mutatedGene.features.Count; i++) {
                    Feature feature = mutatedGene.features[i];
                    // if (!feature.HasTag("essential")) {
                        removableFeatureList.Add(i);
                    // }
                }
                // Debug.Log(removableFeatureList.Count);
                if (removableFeatureList.Count > 0) {
                    // Debug.Log("Before " + mutatedGene.features.Count.ToString());
                    int removeIndex = removableFeatureList[Random.Range(0, removableFeatureList.Count)];
                    mutatedGene = mutatedGene.RemoveObject(mutatedGene.features[removeIndex]);
                    // Debug.Log("After " + mutatedGene.features.Count.ToString());
                }
                break;

        // Adds a new item
            case 1:
                // FurnitureData furnitureData = LevelGene.furnitureLibrary.GetRandomFurnitureByMultipleType("Basic", "Minimalist");
                FurnitureData furnitureData = LevelGene.furnitureLibrary.GetFurniture("ottoman_Modern");
                // Debug.Log(furnitureData.ToString());
                Feature feat = null;
                if (furnitureData != null)
                    feat = mutatedGene.GenerateValidFeature(furnitureData);
                    // Debug.Log(feat);
                    // Debug.Log(feat != null);
                if (feat != null)
                    mutatedGene = mutatedGene.TryPlaceObject(feat);
                else {
                    // Debug.Log("Failure to add");
                    fails += 1;
                    // if (fails >= )
                }

                break;


        // Modifies the placement of an item
            case 2:
                if (mutatedGene.features.Count > 0){
                    // Select a random feature
                    int randomIndex = Random.Range(0, mutatedGene.features.Count);

                    Feature selected = mutatedGene.features[randomIndex];
                    FurnitureData data = LevelGene.furnitureLibrary.GetFurniture(selected.name);

                    // Remove it
                    LevelGene removed = mutatedGene.RemoveObject(selected);

                    // Generate a new feature?
                    int MOVE = 0;
                    int ROTATE = 1;
                    int moveOrRotate = Random.Range(0, 2);

                    // Mutate and add it back
                    if (moveOrRotate == MOVE){ // Move
                        List<(int, int)> validTiles = removed.GetValidTiles(data, selected.orientation);
                        List<(int, int)> destinations = new List<(int, int)>();

                        (int, int) val = new (selected.position.x + 1, selected.position.y + 1);
                        if (validTiles.Contains(val)) destinations.Add(val);

                        val = new (selected.position.x - 1, selected.position.y + 1);
                        if (validTiles.Contains(val)) destinations.Add(val);

                        val = new (selected.position.x + 1, selected.position.y - 1);
                        if (validTiles.Contains(val)) destinations.Add(val);

                        val = new (selected.position.x - 1, selected.position.y - 1);
                        if (validTiles.Contains(val)) destinations.Add(val);


                        if (destinations.Count > 0){
                            int randomDest = Random.Range(0, destinations.Count);
                            (int, int) randomTile = destinations[randomDest];
                            Feature newFeature = new Feature(data, randomTile.Item1, randomTile.Item2);

                            removed = removed.TryPlaceObject(newFeature);

                            if (removed.features.Count == mutatedGene.features.Count)
                                mutatedGene = removed;
                        }

                    } else if (moveOrRotate == ROTATE){ // Move
                        List<float> destinations = new List<float>();

                        // We check if we may place the feature in the same location, but rotated
                        (int, int) val = new (selected.position.x, selected.position.y);

                        // // Rotate 90 degrees counter
                        List<(int, int)> validTiles = removed.GetValidTiles(data, (selected.orientation + 90) % 360);
                        if (validTiles.Contains(val)) destinations.Add((selected.orientation + 90) % 360);

                        // // Rotate 180 degrees
                        validTiles = removed.GetValidTiles(data, (selected.orientation + 180) % 360);
                        if (validTiles.Contains(val)) destinations.Add((selected.orientation + 180) % 360);

                        // // Rotate 270 degrees
                        validTiles = removed.GetValidTiles(data, (selected.orientation + 270) % 360);
                        if (validTiles.Contains(val)) destinations.Add((selected.orientation + 270) % 360);
                        // Debug.Log(destinations.Count);


                        if (destinations.Count > 0){
                            int randomDest = Random.Range(0, destinations.Count);
                            float randomOri = destinations[randomDest];
                            Feature newFeature = new Feature(data, val.Item1, val.Item2, randomOri);
                            removed = removed.TryPlaceObject(newFeature);

                            if (removed.features.Count == mutatedGene.features.Count)
                                mutatedGene = removed;
                        }
                    }
                }

                break;




            // Do nothing
            default:
                break;
        }

        string rets = "";
        for (int j = 0; j < dimensions.y; j++){
            for (int i = 0; i < dimensions.x; i++){
                rets += string.Format(" [{0}] ", (mutatedGene.grid[j, i] == 1)? "A" : (mutatedGene.grid[j, i] == 0) ? "0" : "H");
            }
            rets += "\n";
        }
        // Debug.Log(availableTiles.Count);
        // Debug.Log(rets);
        return mutatedGene;
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
        string [,] ret_grid = new string[dimensions[0], dimensions[1]];
        for (int j = 0; j < dimensions[1]; j++){
            for(int i = 0; i < dimensions[0]; i++){
                ret_grid[j, i] = "0";
            }
        }

        List<(int, int)> availableTiles = GetAvailableTiles();

        List<(int, int)> validTiles = new List<(int, int)>();

        int dim_x = (orientation == 0 || orientation == 180) ? furn_dims[0] : furn_dims[1];
        int dim_y = (orientation == 0 || orientation == 180) ? furn_dims[1] : furn_dims[0];

        foreach((int, int) tile in availableTiles){
            bool isValid = true;
            int tlx = tile.Item1;
            int tly = tile.Item2;


            // Prechecks

            // Check if against wall
            if (feat.HasConstraint("against_wall")){
                isValid &= ((tlx == 0 || tlx + dim_x == dimensions.x) || (tly == 0 || tly + dim_y == dimensions.y));
            }
            if((feat.HasConstraint("back_against_wall"))){
                isValid &= (orientation == 0 && tly + dim_y == dimensions.y) ||
                            (orientation == 90 && tlx == 0) ||
                            (orientation == 180 && tly == 0) ||
                            (orientation == 270 && tlx + dim_x == dimensions.x);
            }
            if (feat.HasConstraint("face_far_from_wall")){
                isValid &= (orientation == 0 && tly >= 4) ||
                            (orientation == 90 && tlx + dim_x - 1 <= 4) ||
                            (orientation == 180 && tly + dim_y  - 1 <= 4) ||
                            (orientation == 270 && tlx >= 4);
            }
            if (feat.HasConstraint("not_against_wall")){
                isValid &= ((tlx > 0 && tlx + dim_x < dimensions.x) && (tly > 0 && tly + dim_y < dimensions.y));
            }
            // if (feat.)
            // Check if within bounds
            isValid &= (tlx + dim_x <= dimensions.x && tly + dim_y <= dimensions.y);



            // Checking against every tile of furniture

            int y_start = tile.Item2;
            int y_end = tile.Item2 + dim_y;

            int x_start = tile.Item1;
            int x_end = tile.Item1 + dim_x;

            int yfront = -1;
            int xfront = -1;

            // space in front constraint # Can I place it here?
            if (feat.HasConstraint("space_in_front")){
                switch (orientation){
                    case 0:
                        y_start -= 1;
                        yfront = y_start;
                        break;

                    case 180:
                        y_end += 1;
                        yfront = y_end - 1;
                        break;

                    case 90:
                        x_end += 1;
                        xfront = x_end - 1;
                        break;

                    case 270:
                        x_start -= 1;
                        xfront = x_start;
                        break;
                }

                isValid &= (y_start >= 0 && x_start >= 0 && y_end <= dimensions.y && x_end <= dimensions.x);
                // Debug.Log(isValid);
            }

            // Check if there is a need to do tilebased checks
            if (!isValid) {
                // ret_grid[tly, tlx] = "0";
                continue;
            }

            for (int y = y_start; y < y_end; y++){
                if (y >= dimensions.y){
                    isValid = false;
                    break;
                }
                for (int x = x_start; x < x_end; x++){
                    if (x >= dimensions.x){
                        // Debug.Log(string.Format("IM HERE ({0}, {1}) Tile: ({2}, {3})", x,y, tile.Item1, tile.Item2));
                        isValid = false;
                        break;
                    }

                    // Debug.Log(string.Format("{0} {1}", x, y));
                    isValid &= (grid[y, x] == 0) ||
                                (grid[y, x] == 2 && (x == xfront || y == yfront));

                    // Some extra validation needs to be done here... or maybe not...

                    // Constraints
                    if (!isValid) break;
                }
                // Debug.Log(isValid == false);
                if (!isValid) break;
            }

            if (isValid) {
                ret_grid[tly, tlx] = "A";
                // Debug.Log(string.Format("IM sss Tile: ({0}, {1})", tly, tlx));
                validTiles.Add(tile);
            }
        }


        string ret = "";
        for (int j = 0; j < dimensions.y; j++){
            for (int i = 0; i < dimensions.x; i++){
                ret += string.Format(" [{0}] ", ret_grid[j, i]);
            }
            ret += "\n";
        }
        // Debug.Log(availableTiles.Count);
        // Debug.Log(feat.ToString() + "\n" + ret);

        return validTiles;
    }

    public Feature GenerateValidFeature(FurnitureData featureData){
        // pick random orientation
        float orientation = Random.Range(0,4) * 90;

        List<(int, int)> validTiles = GetValidTiles(featureData, orientation);

        Feature feature = null;
        if (validTiles.Count > 0){
            (int, int) randomTile = validTiles[Random.Range(0, validTiles.Count)];
            feature = new Feature(featureData, randomTile.Item1, randomTile.Item2, orientation);
        }

        return feature;
    }

    public override string ToString() {
        string [,] ret_grid = new string[dimensions[0], dimensions[1]];
        string ret = "";

        ret += features.Count.ToString() + " features: ";

        ret += string.Join(", ", features)  + "\n";

        foreach (Feature feat in features){
            List<int> furn_dims = feat.dimensions;
            int dim_x = (feat.orientation == 0 || feat.orientation == 180) ? furn_dims[0] : furn_dims[1];
            int dim_y = (feat.orientation == 0 || feat.orientation == 180) ? furn_dims[1] : furn_dims[0];

            for (int y = feat.position.y; y < feat.position.y + dim_y; y++){
                for (int x = feat.position.x; x < feat.position.x + dim_x; x++){
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
        int dim_x = (feature.orientation == 0 || feature.orientation == 180) ? furn_dims[0] : furn_dims[1];
        int dim_y = (feature.orientation == 0 || feature.orientation == 180) ? furn_dims[1] : furn_dims[0];


        int y_start = feature.position.y;
        int y_end = feature.position.y + dim_y;

        int x_start = feature.position.x;
        int x_end = feature.position.x + dim_x;





        // space in front constraint # Can I be placed here?
        if (feature.HasConstraint("space_in_front")){
            if (feature.orientation == 0 || feature.orientation == 180){
                int front = (feature.orientation == 0) ? y_start - 1 : y_end;
                for (int i = x_start; i < x_end; i++)
                    ret.grid[front, i] = 2;
            }

            if (feature.orientation == 90 || feature.orientation == 270){
                int front = (feature.orientation == 270) ? x_start - 1 : x_end;
                for (int i = y_start; i < y_end; i++)
                    ret.grid[i, front] = 2;
            }
        }




        // Debug.Log(string.Format("{0} {1}", dim_x, dim_y));

        for (int y = y_start; y < y_end; y++){
            for (int x = x_start; x < x_end; x++){
                try{
                    ret.grid[y, x] = 1;

                } catch (System.IndexOutOfRangeException e){
                    // Debug.Log(feature);
                    // Debug.Log(ret);
                    // Debug.Log(y);
                    // Debug.Log(x);
                    // Debug.Log(ret.dimensions[0]);
                    // Debug.Log(ret.dimensions[1]);
                }
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
            Debug.Log(string.Format("{0} does not contain {1}", ToString(), feature.ToString()));
            // Debug.Log("Came here");
            return ret;
        }

        // Rectangular features

        int dim_x = (feature.orientation == 0 || feature.orientation == 180) ? furn_dims[0] : furn_dims[1];
        int dim_y = (feature.orientation == 0 || feature.orientation == 180) ? furn_dims[1] : furn_dims[0];

        int y_start = feature.position.y;
        int y_end = feature.position.y + dim_y;

        int x_start = feature.position.x;
        int x_end = feature.position.x + dim_x;

        if (feature.HasConstraint("space_in_front")){
                switch (feature.orientation){
                    case 0:
                        y_start = Mathf.Max(y_start - 1, 0);
                        break;

                    case 180:
                        y_end = Mathf.Min(y_end + 1, dimensions.y);
                        break;

                    case 90:
                        x_end = Mathf.Min(x_end + 1, dimensions.x);
                        break;

                    case 270:
                        x_start = Mathf.Max(x_start - 1, 0);
                        break;
                }
        }


        for (int y = y_start; y < y_end; y++){
            for (int x = x_start; x < x_end; x++){
                ret.grid[y, x] = 0;
            }
        }


        // // space in front constraint # Can I be placed here?


        // for (int j = y_start; j < y_end; j++)

        ret.features.Remove(feature);

        return ret;
    }

    public List<LevelGene> GenerateChildren(LevelGene other) { // Alan
        // crossover
        List<LevelGene> children = new List<LevelGene>();
        // Debug.Log("parent 1: " + ToString());
        // // Debug.Log("parent 2: " + other.ToString());

        // // Create the first child by combining the features from the two parents
        LevelGene child1 = LevelGene.GenerateEmptyLevelGene(dimensions);
        LevelGene child2 = LevelGene.GenerateEmptyLevelGene(dimensions);

        int crossoverPoint = Random.Range(0, Mathf.Min(features.Count / 2, other.features.Count / 2)); // Choose a random crossover point
        // Debug.Log(crossoverPoint);

        // child1 = child1.TryPlaceObject(features[0]);
        // child2 = child2.TryPlaceObject(features[1]);


        for (int i = 0; i < crossoverPoint; i++)
        {
            child1 = child1.TryPlaceObject(new Feature(features[i]));
            child2 = child2.TryPlaceObject(new Feature(other.features[i]));
        }

        for (int i = crossoverPoint; i < features.Count; i++)
        {
            child2 = child2.TryPlaceObject(features[i]);
        }

        for (int i = crossoverPoint; i < other.features.Count; i++)
        {
            child1 = child1.TryPlaceObject(other.features[i]);
        }

        // LevelGene child1 = new LevelGene(this);
        // LevelGene child2 = new LevelGene(other);

        for (int i = 0; i < 2; i++){
            child1 = child1.Mutate();
            child2 = child2.Mutate();
        }

        children.Add(child1);
        children.Add(child2);

        // Debug.Log(string.Format("C Count: {0}", children.Count));
        // for(int i = 0; i < children.Count; i++){
        //     Debug.Log(children[i].ToString());
        // }




        return children;

        // validation is needed
        // Consider feasible and infeasible population?

    }

}
