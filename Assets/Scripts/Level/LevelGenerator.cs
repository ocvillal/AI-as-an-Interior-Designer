
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;
using Feature = FurnitureFeature; // name of the furniture, x, y
using System;

public class LevelGenerator : MonoBehaviour
{
    GameObject player;
    int currPlot;

    public int SUCCESSOR_ITERATIONS = 1;
    public int MAX_LEVELS_PER_ROW = 5;
    public float PLOT_SIZE = 14;
    public List<Color> COLORS = new List<Color>();

    public List<LevelGene> population;
    public List<GameObject> renderedObjects;


    [SerializeField] private Vector2Int _dimensions = new Vector2Int(10, 10);
    public Vector2Int Dimensions {
        get { return _dimensions; }
        set {
            _dimensions = value;
        }
    }

    [SerializeField] private int _numLevels = 3;
    public int NumLevels {
        get { return _numLevels; }
        set {
            _numLevels = value;
            TopLeftCenter = Vector3.zero;
            TopLeftCenter.y = -1;
            TopLeftCenter.x =
                (_numLevels <= MAX_LEVELS_PER_ROW) ? - ((_numLevels - 1) * PLOT_SIZE)/2.0f : -((MAX_LEVELS_PER_ROW - 1) * PLOT_SIZE) / 2.0f;
            // Debug.Log(TopLeftCenter);
        }
    }

    // [Serialized]


    public GameObject Level;
    public Vector3 TopLeftCenter;

    public void CreateLevel(Vector2Int dim, Vector3 pos){
        GameObject l = Instantiate(Level, pos, Quaternion.identity);
        renderedObjects.Add(l);
        Level gen_level = l.GetComponent<Level>();

        gen_level.Dimensions = dim;
        gen_level.GetComponent<Level>().Position = pos;

        gen_level.PlaceItemAtTile(0,0);
        // gen_level.PlaceItemAtTile(4,4);
        gen_level.PlaceItemAtTile(9,9);
        gen_level.PlaceItemAtTile(0,9);
        gen_level.PlaceItemAtTile(9,0);

        // Generation gets called here

        gen_level.GetComponent<Level>().Render();

    }

    public void CreateLevel(LevelGene gene, Vector3 pos){
        GameObject l = Instantiate(Level, pos, Quaternion.identity);
        renderedObjects.Add(l);
        Level gen_level = l.GetComponent<Level>();
        // gen_level.Dimensions = _dimensions;

        gen_level.Gene = gene;
        gen_level.GetComponent<Level>().Position = pos;

        // gen_level.PlaceItemAtTile(0,0);
        // gen_level.PlaceItemAtTile(4,4);
        // gen_level.PlaceItemAtTile(9,9);
        // gen_level.PlaceItemAtTile(0,9);
        // gen_level.PlaceItemAtTile(9,0);

        // // Generation gets called here

        gen_level.GetComponent<Level>().RenderLevel();
    }


    // public void DestroyLevel(){
    //     while (renderedObjects.Count > 0){
    //         Level l = renderedObjects[renderedObjects.Count - 1].GetComponent<Level>();
    //         // l.DestroyLevel();
    //         // Destroy(renderedObjects[renderedObjects.Count - 1]);
    //         // renderedObjects.RemoveAt(renderedObjects.Count - 1);
    //     }
    // }

    public void OnSwitchLevel(InputAction.CallbackContext context)
    {
        // Vector2 curr_val = context.ReadValue<Vector2>();
        switch (context.phase)
        {
            case InputActionPhase.Canceled:
                break;

            case InputActionPhase.Started:
                List<LevelGene> pop = generateSuccessors();
                // Call generate successors however many times
                // Replace population with output of generate successors
                for (int i =  0; i < SUCCESSOR_ITERATIONS - 1; i++) {
                    pop = generateSuccessors();
                }
                population.Clear();
                population.AddRange(pop);

                // Debug.Log(population.Count);
                // Debug.Log("Hello");
                // // Disable player movement


                // // Delete some objects (?)
                // // Attach it all to a gameobject and delete THAT gameobject to recursively delete (maybe)
                GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();

                foreach (GameObject obj in allObjects)
                {
                    if (obj.CompareTag("Player"))
                    {
                        continue;
                    }
                UnityEngine.Object.Destroy(obj);
                }
                renderedObjects.Clear();

                // // Then call RenderPopulation()
                RenderPopulation();

                // // Re-enable player movement (PlayerMovement MovementEnabled/LookEnabled)
                // movement.isEnabled = true;


                break;
        }
    }


    public void OnMoveToLevel(InputAction.CallbackContext context){

        Vector2 curr_val = context.ReadValue<Vector2>();
        Debug.Log(curr_val);
        switch (context.phase)
        {
            case InputActionPhase.Started:
                if (curr_val.x > 0){
                    int currRow = currPlot / MAX_LEVELS_PER_ROW;
                    currPlot = (((currPlot + 1)% MAX_LEVELS_PER_ROW) + currRow * MAX_LEVELS_PER_ROW) % NumLevels;

                }
                if (curr_val.x < 0){
                    int currRow = currPlot / MAX_LEVELS_PER_ROW;
                    currPlot = (((currPlot - 1 + MAX_LEVELS_PER_ROW) % MAX_LEVELS_PER_ROW) + currRow * MAX_LEVELS_PER_ROW) % NumLevels;
                }
                if (curr_val.y > 0){
                    currPlot = (currPlot + MAX_LEVELS_PER_ROW) % NumLevels;
                }
                if (curr_val.y < 0){
                    currPlot = (currPlot - MAX_LEVELS_PER_ROW + NumLevels) % NumLevels;
                }


                player.transform.position = renderedObjects[currPlot].transform.position + Vector3.up * 2.0f;

                break;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        // Debug.Log(_numLevels);
        NumLevels = _numLevels;  // Population Size
        population = new List<LevelGene>();

        player = GameObject.Find("Player");

        PlayerMovement movement = player.GetComponent<PlayerMovement>();
        movement.isEnabled = false;



        // Feature feat1 = new Feature("bed_single", 0, 2);
        // Feature feat2 = new Feature("door", 0, 2);
        // Feature feat3 = new Feature("night_stand", 2, 2);
        // Feature feat4 = new Feature("drawer_small", 4, 0);

        // Debug.Log(feat1);
        // Debug.Log(feat2);
        // Debug.Log(feat3);
        // Debug.Log(feat4);

        // Debug.Log(feat1.OverlapsWith(feat1));
        // Debug.Log(feat1.OverlapsWith(feat2));
        // Debug.Log(feat1.OverlapsWith(feat3));
        // Debug.Log(feat1.OverlapsWith(feat4));

        // Debug.Log(ToString());

        // LevelGene try1 = TryPlaceObject(feat1);
        // Debug.Log(try1.ToString());

        // LevelGene try2 = try1.TryPlaceObject(feat2);
        // Debug.Log(try2.ToString());

        // LevelGene try3 = try2.TryPlaceObject(feat3);
        // Debug.Log(try3.ToString());

        // LevelGene try4 = try3.TryPlaceObject(feat4);
        // Debug.Log(try4.ToString());

        // Debug.Log(randlevel.ToString());
        for (int i = 0; i < _numLevels; i++){
            population.Add(LevelGene.GenerateRandomLevelGene(Dimensions, 6));
        }

        // // Teleport player upwards

        RenderPopulation();


        player.transform.position = renderedObjects[0].transform.position + Vector3.up * 2.0f;

        currPlot = 0;
        // for (int i =  0; i < SUCCESSOR_ITERATIONS; i++) {
        //     population = new List<LevelGene>(generateSuccessors());
        // }
    }

    // Update is called once per frame
    void Update()
    {

    }


    List<LevelGene> elitestSelection(){
        // Debug.Log("CALLED");
        List<LevelGene> results = new List<LevelGene>();

        // Sort from Biggest to smallest
        List<LevelGene> randPop = new List<LevelGene>();

        randPop = population.OrderBy(x => -x.Fitness()).ToList();
        int i = 0;
        while(results.Count < population.Count/8){
            results.Add(randPop[i]);
            i++;
        }

        return results;
    }

    List<LevelGene> tourneySelection(){
        List<LevelGene> results = new List<LevelGene>();
        List<LevelGene> randPop = new List<LevelGene>();
        randPop = population.OrderBy(x=> UnityEngine.Random.Range(0, population.Count - 1)).ToList();



        while (results.Count < 7*population.Count/8){
            // choose k individuals lets say 4
            randPop.Clear();
            for (int i = 0; i < 4; i++){
                randPop.Add(population[UnityEngine.Random.Range(population.Count/8, population.Count)]);
            }

            // from those 4 sort em
            randPop.Sort((x, y) => -x.Fitness().CompareTo(y.Fitness()));

            // choose with tournament

            float randProb = UnityEngine.Random.Range(0f, 1f);
            int randIndex = randPop.Count - 1;
            float p = 0.5f;
            for (int i = 0; i < randPop.Count; i++){
                if (randProb < p*Mathf.Pow(1 - p, i)){
                    randIndex = i;
                    break;
                }
            }
            // Debug.Log(randProb);
            // Debug.Log(randIndex);


            results.Add(randPop[randIndex]);
        }

        // LevelGene contestantA = randPop[i];
        // LevelGene contestantB = randPop[population.Count - i-1];
        // if (contestantB.Fitness() > contestantA.Fitness()){
        //     results.Add(contestantB);
        // }
        // else{
        //     results.Add(contestantA);
        // }
        return results;
    }



    List<LevelGene> generateSuccessors(){ // Octavio
        List<LevelGene> results = new List<LevelGene>();


        List<LevelGene> selectList = new List<LevelGene>();
        selectList.AddRange(elitestSelection());
        selectList.AddRange(tourneySelection());

        // elitestSelection(ref selectList);
        // tourneySelection(ref selectList);


        // Debug.Log(string.Format("PARENTS: {0}", selectList.Count));

        // Note... I think we may not want our generator to choose the same parents


        for(int i = 0; i < selectList.Count / 2; i++){
            LevelGene parentFirst = selectList[UnityEngine.Random.Range(0, selectList.Count)];
            LevelGene parentSecond = selectList[UnityEngine.Random.Range(0, selectList.Count)];
            results.AddRange(parentFirst.GenerateChildren(parentSecond));
        }

        return results;
    }


    void Render(){
        int count = 0;
        Vector3 plot_pos = TopLeftCenter;
        for (int k = 0; k < _numLevels; k++){
            if (count == MAX_LEVELS_PER_ROW){
                Debug.Log("HELLO");
                Debug.Log(_numLevels - k );
                plot_pos.x = (_numLevels - k  < MAX_LEVELS_PER_ROW) ? -(((_numLevels - k - 1) * PLOT_SIZE)/2.0f)  : TopLeftCenter.x;
                plot_pos.z += PLOT_SIZE;
                count = 0;
            }

            CreateLevel(Dimensions, plot_pos);
            plot_pos.x += PLOT_SIZE;
            count += 1;
        }
    }

    void RenderPopulation(){
        // Copying population in case it would drastically affect outcomes
        population.Sort((x, y) => -x.Fitness().CompareTo(y.Fitness()));
        Debug.Log(string.Format("BEST FITNESS: {0}\nBest individual: {1}", population[0].Fitness(), population[0].ToString()));
        // Debug.Log(string.Format("WORST FITNESS: {0}\nBest individual: {1}", population[NumLevels - 1].Fitness(), population[NumLevels - 1].ToString()));


        int count = 0;
        Vector3 plot_pos = TopLeftCenter;
        for (int k = 0; k < _numLevels; k++){
            if (count == MAX_LEVELS_PER_ROW){
                // Debug.Log(_numLevels - k );
                plot_pos.x = (_numLevels - k  < MAX_LEVELS_PER_ROW) ? -(((_numLevels - k - 1) * PLOT_SIZE)/2.0f)  : TopLeftCenter.x;
                plot_pos.z += PLOT_SIZE;
                count = 0;
            }

            CreateLevel(population[k], plot_pos);
            plot_pos.x += PLOT_SIZE;
            count += 1;
        }
        // for(int i = 0; i < population.Count; i++){
        //     Debug.Log(population[i].ToString());
        // }
    }


}

