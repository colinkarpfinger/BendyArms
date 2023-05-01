using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCrate : MonoBehaviour
{

    public GameObject[] cargoCrates;
    public GameObject spawnArea;
    public int spawnCount = 100;
    public int spawnAreaSize = 100;
    public float spawnHeight = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] crates = new GameObject[spawnCount];
        for(int i = 0; i < spawnCount; i++) {
            int cargoIndex = Random.Range(0, cargoCrates.Length);
            Vector3 randomArea = Random.insideUnitSphere * spawnAreaSize;
            Vector3 spawnPosition = new Vector3(spawnArea.transform.position.x + randomArea.x, spawnHeight, spawnArea.transform.position.z + randomArea.z); 
            crates[i] = Instantiate(cargoCrates[cargoIndex], spawnPosition, Quaternion.identity);
        }
        IEnumerator coroutine = ActivateAudio(crates);
        StartCoroutine(coroutine);
    }

    private IEnumerator ActivateAudio(GameObject[] crates)
    {
        yield return new WaitForSeconds(1);
        for(int i = 0; i < spawnCount; i++) {
            crates[i].GetComponent<FMODUnity.StudioEventEmitter>().PlayEvent = FMODUnity.EmitterGameEvent.TriggerEnter;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
