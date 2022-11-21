using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.AR;

public class TreasureSpawner : MonoBehaviour
{
    public GameObject treasure;
    public GameObject[] seats;

    private GameObject spawnedTreasure;
    private int spawnedIndex = -1;

    // Start is called before the first frame update
    void Start() {
        RespwanTreasure(null);
    }

    public void RespwanTreasure(SelectEnterEventArgs arg0) {
        Debug.Log("Spwan");

        if (spawnedTreasure != null) {
            Destroy(spawnedTreasure);
        }

        int index = Random.Range(0, seats.Length);
        while (spawnedIndex == index) {
            index = Random.Range(0, seats.Length);
        }
        GameObject seat = seats[index];
        spawnedIndex = index;
        Vector3 position = seat.transform.position;
        position.y += 0.1f;
        Quaternion quaternion = Quaternion.Euler(new Vector3(-90, 0, 0));
        spawnedTreasure = Instantiate(treasure, position, quaternion);
        spawnedTreasure.GetComponent<ARSelectionInteractable>().selectEntered.AddListener(RespwanTreasure);
    }
}