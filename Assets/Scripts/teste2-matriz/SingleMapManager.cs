using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleMapManager : MonoBehaviour {
    // public MapGenMatriz mapGenMatrizInstance;
    [SerializeField]
    private int[] selfArrayPos = new int[2];

    [SerializeField]
    private float[] positionInWorld;

    private MapGenMatriz mapGenMatrizObj = MapGenMatriz.instance;

    public void SetStuff (int posCol, int posRow, float[] position) {
        this.selfArrayPos[0] = posCol;
        this.selfArrayPos[1] = posRow;
        positionInWorld = position;
    }

    // Start is called before the first frame update
    void Start () {

        // mapGenMatrizObj.MakeSides (selfArrayPos, false);
        // mapGenMatrizObj.TryToSpawnSide (selfArrayPos, 2, false);

        print ("I have spawned! " + selfArrayPos[0] + "/" + selfArrayPos[1]);
        SetPositionInWorld ();
    }

    public void SpawnOther () {
        print ("Spawning?");
        for (var i = 0; i < 4; i++) {
            mapGenMatrizObj.MakeSides (selfArrayPos, false);
        }
    }

    private void SetPositionInWorld () {
        transform.position = new Vector3 (positionInWorld[0], 0, positionInWorld[1]);
    }

    // Update is called once per frame
    void Update () {

    }
}