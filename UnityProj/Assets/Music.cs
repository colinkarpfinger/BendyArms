using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using FMODUnity;

public class Music : MonoBehaviour
{
    public Canvas containerCanvas;
    private Text containerText;
    private BeatSystem bS;

    [SerializeField] EventReference bgmRef;
    private FMOD.Studio.EventInstance bgmSound;

    void Start()
    {
        bS = GetComponent<BeatSystem>();
        //containerText = containerCanvas.GetComponent<Text>();
        bgmSound = FMODUnity.RuntimeManager.CreateInstance(bgmRef);
        bgmSound.start();
        bS.AssignBeatEvent(bgmSound);
        
        bgmSound.setParameterByName("Containers", 0);
        TentacleController.containerCount = 0;
    }

    void Update() 
    {

        if(BeatSystem.marker == "Switch") {
            bgmSound.setParameterByName("Containers", TentacleController.containerCount);
        }
        //Debug.Log(BeatSystem.beat);
        //Debug.Log(BeatSystem.marker);
        //Debug.Log(containerCount);
    }
}