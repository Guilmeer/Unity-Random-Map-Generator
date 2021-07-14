using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapGenMatriz : MonoBehaviour {

    private static MapGenMatriz Instance;
    public static MapGenMatriz instance { get { return Instance; } }

    public TileScriptable tiles;

    [SerializeField]
    private int rows = 5;

    [SerializeField]
    private int cols = 5;

    [SerializeField]
    private float tileSize;
    public GameObject mainTileMap;
    // public GameObject tileMap;

    [SerializeField]
    private GameObject[, ] mapMatrix;

    [SerializeField]
    private Stack<GameObject> SpawnedRooms;

    [SerializeField]
    private float chanceToSpawn = 100;
    [SerializeField]
    private float chanceToMainSpawn = 100;
    [SerializeField]
    private float chanceToSpawnCut = 2;
    public int numberOfRooms = 0;
    public int maximumNumberOfRooms = 10;
    // Start is called before the first frame update    
    private void Awake () {
        Instance = this;
    }
    void Start () {
        mapMatrix = new GameObject[rows, cols];
        SpawnedRooms = new Stack<GameObject> ();
        int[] mainTilePosition = GetInitialTilePosition ();
        StartMapGen (mainTilePosition);
        // GenerateGrid ();
    }

    private int[] GetInitialTilePosition () {
        int mapM0 = (int) Math.Ceiling ((float) mapMatrix.GetLength (0) / 2);
        int mapM1 = (int) Math.Ceiling ((float) mapMatrix.GetLength (1) / 2);
        return new int[] { mapM0, mapM1 };
    }

    private void StartMapGen (int[] positionInArray) {
        print (positionInArray[0] + " " + positionInArray[1]);
        GameObject mainTile = Instantiate (mainTileMap);
        mapMatrix[positionInArray[0], positionInArray[1]] = mainTile;
        mainTile.transform.position = new Vector3 (positionInArray[0] * tileSize, 0, positionInArray[1] * tileSize);

        MakeSides (positionInArray, true);
        SpawnRoomsOnSecondaryTiles ();
    }

    private void SpawnRoomsOnSecondaryTiles () {
        for (var i = 0; i < 10; i++) {
            int index = Random.Range (0, SpawnedRooms.Count);
            SpawnedRooms.ToArray () [index].GetComponent<SingleMapManager> ().SpawnOther ();
        }

    }

    public void MakeSides (int[] positionInArray, bool isMainTileSpawn) {
        int amount = Random.Range (1, 4);
        bool clockwise = Random.Range (1, 2) == 1 ? true : false;
        for (var i = 0; i < amount; i++) {
            if (clockwise) {
                for (var j = 0; j < 4; j++) {
                    TryToSpawnSide (positionInArray, j, isMainTileSpawn);
                }
            } else {
                for (var j = 3; j > 0; j--) {
                    TryToSpawnSide (positionInArray, j, isMainTileSpawn);
                }
            }

        }
    }

    public bool TryToSpawnSide (int[] positionInArray, int side, bool isMainTileSpawn) {

        if (side > 3 && side < 0) { return false; }
        int[] newPositionInArray = CheckIfSideIsAvailable (positionInArray, side);

        if (newPositionInArray != null && numberOfRooms < maximumNumberOfRooms) {
            CreateTile (newPositionInArray, isMainTileSpawn);
            numberOfRooms++;
            return true;
        }
        return false;
        // for (int i = 0; i < Random.Range (2, 5); i++) {
        //     int numberOfTimes = Random.Range (0, 3);
        //     int side = Random.Range (0, numberOfTimes);
        //     int[] arrayPos = new int[2];
        //     int[] worldSidePos = GetSidePosition (side);
        //     arrayPos[0] = positionInArray[0] + worldSidePos[0];
        //     arrayPos[1] = positionInArray[1] + worldSidePos[1];

        //     if (IsSlotAvailable (arrayPos)) {
        //         CreateTile (arrayPos, worldSidePos, isMainTileSpawn);
        //         numberOfRooms++;
        //     }
        // }

    }

    private int[] CheckIfSideIsAvailable (int[] positionInArray, int side) {
        int[] newPositionInArray = new int[2];
        int[] worldSidePos = GetSidePosition (side);
        newPositionInArray[0] = positionInArray[0] + worldSidePos[0];
        newPositionInArray[1] = positionInArray[1] + worldSidePos[1];
        if (IsSlotAvailable (newPositionInArray)) { return newPositionInArray; } else { return null; }
    }

    private bool IsSlotAvailable (int[] positionInArray) {

        if (positionInArray[0] < mapMatrix.GetLength (0) &&
            positionInArray[1] < mapMatrix.GetLength (1) &&
            positionInArray[0] > 0 &&
            positionInArray[1] > 0 &&
            mapMatrix[positionInArray[0], positionInArray[1]] == null) {
            return true;
        }
        return false;
    }

    private void CreateTile (int[] arrayPos, bool isMainTileSpawn) {
        int canSpawnChance = Random.Range (0, 100);

        if (isMainTileSpawn) {
            InstantiateTile (arrayPos, false);
            chanceToMainSpawn /= (chanceToSpawnCut / 1.4f);
        } else if (!isMainTileSpawn) {
            InstantiateTile (arrayPos, true);
            // newTile.transform.position = new Vector3 (posx, 0, posy);
            chanceToSpawn /= chanceToSpawnCut;
        }
    }

    private void InstantiateTile (int[] arrayPos, bool getToTileList) {
        GameObject newTile = Instantiate (GetATile ());
        mapMatrix[arrayPos[0], arrayPos[1]] = newTile;
        SpawnedRooms.Push (newTile);
        float[] positionWorld = GetWorldPosition (arrayPos);
        newTile.GetComponent<SingleMapManager> ().SetStuff (arrayPos[0], arrayPos[1], new float[] {
            positionWorld[0],
                positionWorld[1]
        });
    }

    private GameObject GetATile () {
        int randIndex = Random.Range (0, tiles.tilesArray.Length);
        GameObject tile = tiles.tilesArray[randIndex];
        return tile;
    }

    private float[] GetWorldPosition (int[] arrayPos) {
        float posx = tileSize * arrayPos[0];
        float posy = tileSize * arrayPos[1];
        return new float[] { posx, posy };
    }

    public int[] GetSidePosition (int side) {
        int nextValueCol = 0;
        int nextValueRow = 0;
        switch (side) {
            case 0:
                nextValueCol = 0;
                nextValueRow = -1;
                break;
            case 1:
                nextValueCol = 1;
                nextValueRow = 0;
                break;
            case 2:
                nextValueCol = 0;
                nextValueRow = 1;
                break;
            case 3:
                nextValueCol = -1;
                nextValueRow = 0;
                break;
        }

        return new int[] { nextValueCol, nextValueRow };
    }
    // Update is called once per frame
    void Update () {

    }
}