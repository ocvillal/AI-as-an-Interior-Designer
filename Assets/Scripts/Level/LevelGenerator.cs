using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public int MAX_LEVELS_PER_ROW = 5;
    public float PLOT_SIZE = 14;

    public List<LevelGene> population;


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
            Debug.Log(TopLeftCenter);
        }
    }

    // [Serialized]


    public GameObject Level;
    public Vector3 TopLeftCenter;

    public void CreateLevel(Vector2Int dim, Vector3 pos){
        GameObject l = Instantiate(Level, pos, Quaternion.identity);
        Debug.Log(l);
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

    public void CreateLevel(LevelGene gene){
        GameObject l = Instantiate(Level, pos, Quaternion.identity);
        Debug.Log(l);
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



    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(_numLevels);
        NumLevels = _numLevels;  // Population Size

        LevelGene g = new LevelGene(Dimensions);

    }

    // Update is called once per frame
    void Update()
    {

    }


    List<LevelGene> elitestSelection(){
        List<LevelGene> results = new List<LevelGene>();
        // Sort from Biggest to smallest
        List<LevelGene> randPop = new List<LevelGene>();
        // randPop = populations.OrderBy(x => x.count).ToList();
        for(int i = 0; i < population.Count % 2; i++){
            results.Add(population[i]);
        }
        return results;
    }

    List<LevelGene> tourneySelection(){
        List<LevelGene> results = new List<LevelGene>();
        List<LevelGene> randPop = new List<LevelGene>();
        // randPop = populations.OrderBy(x=> Random.Shared.Next()).ToList();
        for(int i = 0; population.Count < results.Count % 2; i++){
            LevelGene contestantA = randPop[i];
            LevelGene contestantB = randPop[population.Count - i];
            if (contestantB.Fitness() > contestantA.Fitness()){
                results.Add(contestantB);
            }
            else{
                results.Add(contestantA);
            }
        }
        return results;
    }

    List<LevelGene> generateSuccessors(){ // Octavio
        List<LevelGene> results = new List<LevelGene>();
        List<LevelGene> selectList = elitestSelection();
        selectList.AddRange(tourneySelection());
        for(int i = 0; i < selectList.Count % 2; i++){
            LevelGene parentFirst = selectList[i];
            LevelGene parentSecond = selectList[-(i+1)];
            results.AddRange(parentFirst.GenerateChildren(parentSecond));
            // results.Add(parentSecond.generateChildren);
        }
        return results;
    }


    void Render(){
        int count = 0;
        Vector3 plot_pos = TopLeftCenter;
        for (int k = 0; k < _numLevels; k++){
            if (count == 5){
                Debug.Log("HELLO");
                Debug.Log(_numLevels - k );
                plot_pos.x = (_numLevels - k  < MAX_LEVELS_PER_ROW) ? -(((_numLevels - k - 1) * PLOT_SIZE)/2.0f)  : TopLeftCenter.x;
                plot_pos.z += PLOT_SIZE;
                count = 0;
            }

            CreateLevel(new Vector2Int(10, 10), plot_pos);
            plot_pos.x += PLOT_SIZE;
            count += 1;
        }
    }
}
