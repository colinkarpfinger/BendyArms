using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class MusicManager : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] EventReference musicRef;
    private FMOD.Studio.EventInstance musicInstance;

    void Awake()
    {
        DontDestroyOnLoad(this);
        musicInstance = FMODUnity.RuntimeManager.CreateInstance(musicRef);
        musicInstance.start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StopMusic() {
        musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        musicInstance.release();
    }
}
