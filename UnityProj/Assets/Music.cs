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
    private int containerCount;

    void Start()
    {
        bS = GetComponent<BeatSystem>();
        containerText = containerCanvas.GetComponent<Text>();
        instance = FMODUnity.RuntimeManager.CreateInstance("event:/Music/Gameplay/BGM_layered");
        instance.start();
        bS.AssignBeatEvent(instance); 
        containerCount = 0;
    }

    void Update() 
    {
        containerText.text = "Containers: " + containerCount.ToString();
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