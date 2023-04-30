using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class Music : MonoBehaviour
{
    private FMOD.Studio.EventInstance instance;
    private BeatSystem bS;
    private int containerCount;

    void Start()
    {
        bS = GetComponent<BeatSystem>();
        instance = FMODUnity.RuntimeManager.CreateInstance("event:/Music/Gameplay/BGM_layered");
        instance.start();
        bS.AssignBeatEvent(instance); 
        containerCount = 0;
    }

    void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            containerCount += 1;
        }
        if(BeatSystem.marker == "Switch") {
            instance.setParameterByName("Containers", containerCount);
        }
        Debug.Log(BeatSystem.beat);
        Debug.Log(BeatSystem.marker);
        Debug.Log(containerCount);
    }
}