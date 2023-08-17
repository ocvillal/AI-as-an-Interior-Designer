using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public int MAX_LEVELS_PER_ROW = 5;
    public float PLOT_SIZE = 14;

    public LevelGene populations;


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

    public void CreateLevel(Vector2 dim, Vector3 pos){
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

        LevelGene g = new LevelGene();

    }

    // Update is called once per frame
    void Update()
    {

    }

    
    void elitestSelection(){
        List<> results = new List<>();
        // Sort from Biggest to smallest
        List<> randPop = new List<>();
        // randPop = populations.OrderBy(x => x.count).ToList();
        for(int i = 0; i < populations.Length % 2; i++){
            results.Add(population[i]);
        }
        return results;
    }

    void tourneySelection(){
        List<> results = new List<>();
        List<> randPop = new List<>();
        // randPop = populations.OrderBy(x=> Random.Shared.Next()).ToList();
        for(int i = 0; population.Length < results.Lenght % 2; i++){
            contestantA = randPop[i];
            contestantB = randPop[populations.Length - i];
            if contestantB > contestantA{
                results.Add(cotestantA);
            }
            else{
                results.Add(cotestantB);
            }
        }
        return results;
    }

    void generateSuccessors(){ // Octavio
        List<> results = new List<>();
        List<> selectList = elitestSelection();
        selectList.AddRange(tourneySelection);
        for(int i = 0; i < selectList.Lenght % 2; i++){
            parentFirst = selectList[i];
            parentSecond = selectList[-(i+1)];
            results.Add(generateChildren);
            results.Add(generateChildren)
        }
        return results
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

            CreateLevel(new Vector2(10, 10), plot_pos);
            plot_pos.x += PLOT_SIZE;
            count += 1;
        }
    }
}
