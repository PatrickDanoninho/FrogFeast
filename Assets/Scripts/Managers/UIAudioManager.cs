using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class UIAudioManager : MonoBehaviour
{
    static public UIAudioManager Instance;

    [SerializeField]
    private EventReference uiClickEvent;

    [SerializeField]
    private EventReference uiStartClickEvent;

    [SerializeField]
    private EventReference uiCloseEvent;

    [SerializeField]
    private EventReference uiBuyEvent;

    [SerializeField]
    private EventReference uiPauseEvent;

    [SerializeField]
    private EventReference uiResumeEvent;

    private void Awake()
    {
        Instance = this;
    }

    public void PlayUIClickEvent() 
    {
        RuntimeManager.PlayOneShot(uiClickEvent);
    }
    public void PlayUIStartClickEvent() 
    {
        RuntimeManager.PlayOneShot(uiStartClickEvent);
    }
    public void PlayUICloseEvent() 
    {
        RuntimeManager.PlayOneShot(uiCloseEvent);
    }
    public void PlayUIBuyEvent() 
    {
        RuntimeManager.PlayOneShot(uiBuyEvent);
    }
    public void PlayUIPauseEvent() 
    {
        RuntimeManager.PlayOneShot(uiPauseEvent);
    }
    public void PlayUIResumeEvent() 
    {
        RuntimeManager.PlayOneShot(uiResumeEvent);
    }
}
