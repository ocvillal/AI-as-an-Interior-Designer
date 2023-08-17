using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Finish by Thursday


using Feature = System.Tuple<string, float, float>; // name of the furniture, x, y

// Represents an individual inside the population
public class LevelGene {

    //static // What would the json load in?

    List<Feature> features;

    bool isValid;

    LevelGene(){
        features = new List<Feature>();
    }

    static void LoadAllFurniture(){ // Arrian

    }

    static LevelGene GenerateRandomLevelGene(){ // Arrian
        return new LevelGene();
    }

    static LevelGene GenerateEmptyLevelGene(){ // Angela
        return new LevelGene();
    }


    float Fitness(){ // THE MEAT (HOw would we define this???? Someone research) Angela
        return 0.0f;
    }

    bool ValidateSelf(){ // n^2 complexity
        return true;
    }

    bool ValidateFeatureAddition(Feature feat, bool allowDuplicates){ // Can I add this item into myself? O(n) complexity // THE MEAT // Arrian
        // go through every piece of furniture in the features
        // validate with the rest of the
        return false;
    }

    LevelGene Mutate(){ // A mutate function // Angela
        // Either

        // Modifies the placement of an item

        // Adds a new item

        // Removes an item
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