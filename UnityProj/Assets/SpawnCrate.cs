using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCrate : MonoBehaviour
{

    public GameObject[] cargoCrates;

    public static GameObject[] staticCargoCrates;
    public GameObject spawnArea;
    public int spawnCount = 10;
    public int spawnAreaSize = 100;
    public float spawnHeight = 1.0f;

    // Start is called before the first frame update
    // Crates are "respawned" aka moved in DeliveryZone.cs
    public static void SpawnNewCrate(Vector3 spawnPosition) {
        GameObject crate = Instantiate(staticCargoCrates[(int)(Random.value * staticCargoCrates.Length)], spawnPosition, Quaternion.identity);
        crate.GetComponent<FMODUnity.StudioEventEmitter>().PlayEvent = FMODUnity.EmitterGameEvent.TriggerEnter;
    }

    void Start()
    {
        Debug.Log(spawnCount);
        GameObject[] crates = new GameObject[spawnCount];
        for(int i = 0; i < spawnCount; i++) {
            int cargoIndex = Random.Range(0, cargoCrates.Length);
            Vector3 randomArea = Random.insideUnitSphere * spawnAreaSize;
            Vector3 spawnPosition = new Vector3(spawnArea.transform.position.x + randomArea.x, spawnHeight, spawnArea.transform.position.z + randomArea.z); 
            crates[i] = Instantiate(cargoCrates[cargoIndex], spawnPosition, Quaternion.identity);
            Debug.Log("spawwn");
        }
        IEnumerator coroutine = ActivateAudio(crates);
        StartCoroutine(coroutine);
        staticCargoCrates = cargoCrates;
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
