using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour {

    private RoomTemplates templates;
    private int randomNumber;
    private bool spawned = false;

    void Start () {
        templates = GameObject.FindGameObjectWithTag ("GameController").GetComponent<RoomTemplates> ();
        Invoke ("SpawnRoom", 0.1f);
    }

    // Update is called once per frame
    void SpawnRoom () {
        if (!spawned && templates.amountOfRooms < templates.maxOfRooms) {
            templates.amountOfRooms++;
            randomNumber = Random.Range (0, templates.RoomTemplateList.Length);
            Instantiate (templates.RoomTemplateList[randomNumber], transform.position, Quaternion.identity);
            spawned = true;
        }
    }

    private void OnTriggerEnter (Collider other) {
        if (other.CompareTag ("SpawnPoint")) {
            if (other.GetComponent<RoomSpawner> ().spawned == false && spawned == false) {

            }
            spawned = true;
        }
    }
}