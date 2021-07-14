using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBlockSpawner : MonoBehaviour {
    public List<GameObject> spawnPoint;
    private RoomTemplates templates;

    public int randomNumber;
    private int randomRoom;

    private void Start () {
        templates = GameObject.FindGameObjectWithTag ("GameController").GetComponent<RoomTemplates> ();
        randomNumber = Random.Range (0, spawnPoint.Count);
        randomRoom = Random.Range (0, templates.RoomTemplateList.Length);
        Invoke ("MapBlockSpawn", 0.1f);

    }

    private void MapBlockSpawn () {
        GameObject mB = Instantiate (templates.RoomTemplateList[randomRoom], spawnPoint[randomNumber].transform.position, Quaternion.identity);
        int index = 0;
        switch (randomNumber) {
            case 0:
                index = 1;
                break;
            case 1:
                index = 0;
                break;
            case 2:
                index = 3;
                break;
            case 3:
                index = 2;
                break;
        }
        Destroy (mB.GetComponentInChildren<MapBlockSpawner> ().spawnPoint[index].gameObject);
        mB.GetComponentInChildren<MapBlockSpawner> ().spawnPoint.RemoveAt (index);
    }
}