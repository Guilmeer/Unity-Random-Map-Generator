using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBlockDestroyer : MonoBehaviour {
    private void OnTriggerEnter (Collider other) {
        if (other.CompareTag ("SpawnPoint")) Destroy (other.gameObject);
    }
}