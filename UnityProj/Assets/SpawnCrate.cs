using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCrate : MonoBehaviour
{

    public GameObject[] cargoCrates;
    public GameObject spawnArea;
    public int spawnCount = 100;
    public int spawnAreaSize = 100;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < spawnCount; i++) {
            int cargoIndex = Random.Range(0, cargoCrates.Length);
            Vector3 randomArea = Random.insideUnitSphere * spawnAreaSize;
            Vector3 spawnPosition = new Vector3(spawnArea.transform.position.x + randomArea.x, -1.0f, spawnArea.transform.position.z + randomArea.z); 
            Instantiate(cargoCrates[cargoIndex], spawnPosition, Quaternion.identity);

        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
