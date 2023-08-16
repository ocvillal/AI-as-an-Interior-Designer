using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Finish by Thursday

using Feature = (string, float, float); // name of the furniture, x, y

// Represents an individual inside the population
class LevelGene {

    //static // What would the json load in?

    Array<Feature> features;

    bool isValid;

    void LevelGene(){
        features = new Array<Feature>();
    }

    static void LoadAllFurniture(){ // Arrian

    }

    static LevelData GenerateRandomLevelData(){ // Arrian

    }

    static LevelData GenerateEmptyLevelData(){ // Angela

    }


    float Fitness(){ // THE MEAT (HOw would we define this???? Someone research) Angela

    }

    bool ValidateSelf(){ // n^2 complexity

    }

    bool ValidateFeatureAddition(Feature feat, bool allowDuplicates){ // Can I add this item into myself? O(n) complexity // THE MEAT // Arrian
        // go through every piece of furniture in the features
        // validate with the rest of the
    }

    LevelData Mutate(){ // A mutate function // Angela
        // Either

        // Modifies the placement of an item

        // Adds a new item

        // Removes an item
    }

    LevelData PlaceObject(){ // Alan

    }

    LevelData RemoveObject(){ // Octavio

    }

    Array<LevelData> GenerateChildren(LevelData other) { // Alan
        // crossover


        // validation is needed
        // Consider feasible and infeasible population?
    }

}