using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class Music : MonoBehaviour
{
    public Canvas containerCanvas;
    private Text containerText;
    private FMOD.Studio.EventInstance instance;
    private BeatSystem bS;

    void Start()
    {
        bS = GetComponent<BeatSystem>();
        //containerText = containerCanvas.GetComponent<Text>();
        instance = FMODUnity.RuntimeManager.CreateInstance("event:/Music/Gameplay/BGM_layered");
        instance.start();
        bS.AssignBeatEvent(instance); 
    }

    void Update() 
    {

        if(BeatSystem.marker == "Switch") {
            Debug.Log(TentacleController.containerCount);
            instance.setParameterByName("Containers", TentacleController.containerCount);
        }
        //Debug.Log(BeatSystem.beat);
        //Debug.Log(BeatSystem.marker);
        //Debug.Log(containerCount);
    }
}