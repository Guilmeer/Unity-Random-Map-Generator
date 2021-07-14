using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTemplates : MonoBehaviour {
    public GameObject[] RoomTemplateList;

    public int minimumNumberOfRooms;
    public int maximumNumberOfRooms;
    public int maxOfRooms;
    public int amountOfRooms;

    private void Awake () {
        maxOfRooms = Random.Range (minimumNumberOfRooms, maximumNumberOfRooms);
    }
}