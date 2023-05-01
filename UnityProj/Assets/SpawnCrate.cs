using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCrate : MonoBehaviour
{

    public GameObject[] cargoCrates;
    public GameObject spawnArea;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float spawnProbability = Random.Range(0.0f, 1.0f);
        if(spawnProbability <= 0.001f) {
            int cargoIndex = Random.Range(0, cargoCrates.Length);
            Debug.Log(spawnProbability);
            Vector3 randomArea = Random.insideUnitSphere * 5;
            Vector3 spawnPosition = new Vector3(spawnArea.transform.position.x + randomArea.x, -1.0f, spawnArea.transform.position.z + randomArea.z); 
            Instantiate(cargoCrates[cargoIndex], spawnPosition, Quaternion.identity);

        }
    }
}
