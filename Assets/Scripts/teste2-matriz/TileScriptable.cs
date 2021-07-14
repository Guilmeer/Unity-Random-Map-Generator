using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "TileScriptable", menuName = "Map/TileScriptable", order = 0)]
public class TileScriptable : ScriptableObject {
    public GameObject[] tilesArray;
}