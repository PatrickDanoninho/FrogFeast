using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    public EventReference music;

    private FMOD.Studio.EventInstance musicInstance;
    private FMOD.Studio.EVENT_CALLBACK beatCallback;

    public TimelineInfo timeLineInfo;
    private GCHandle timeLineHandle;

    private int pendingBeat = -1;
    private string pendingMarker = null;

    //todo
    public static event Action<int> OnBeat;
    public static event Action<string> OnMarker;

    [StructLayout(LayoutKind.Sequential)]
    public class TimelineInfo
    {
        public int currentBeat = 0;
        public FMOD.StringWrapper lastMarker = new FMOD.StringWrapper();
    }

    public void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        musicInstance = RuntimeManager.CreateInstance(music);
        SetUpTimeLineInstance();
    }

    public void PlayMusic()
    {
        if (musicInstance.isValid())
            musicInstance.start();

    }
    public void StopMusic()
    {
        musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    public void Update()
    {
        if (pendingBeat != -1)
        {
            OnBeat?.Invoke(pendingBeat);
            pendingBeat = -1;
        }

        if (!string.IsNullOrEmpty(pendingMarker))
        {
            OnMarker?.Invoke(pendingMarker);
            pendingMarker = null;
        }
    }

    //To be called when GM comes back into the main menu, to clean up
    public void OnResetEverything()
    {
        musicInstance.setUserData(IntPtr.Zero);
        musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        musicInstance.release();
        timeLineHandle.Free();
    }


#if UNITY_EDITOR
    private void OnGUI()
    {
        GUILayout.Box($"Current Beat = {timeLineInfo.currentBeat}, Last Marker = {(string)timeLineInfo.lastMarker}");
    }
#endif

    [AOT.MonoPInvokeCallback(typeof(FMOD.Studio.EVENT_CALLBACK))]
    private static FMOD.RESULT BeatCallback(
        FMOD.Studio.EVENT_CALLBACK_TYPE type,
        IntPtr instancePtr,
        IntPtr parameterPtr)
    {
        var instance = new FMOD.Studio.EventInstance(instancePtr);
        instance.getUserData(out IntPtr timelinePtr);

        if (timelinePtr == IntPtr.Zero)
            return FMOD.RESULT.OK;

        var handle = GCHandle.FromIntPtr(timelinePtr);
        var info = (TimelineInfo)handle.Target;

        switch (type)
        {
            case FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_BEAT:
                var beat = (FMOD.Studio.TIMELINE_BEAT_PROPERTIES)
                    Marshal.PtrToStructure(parameterPtr, typeof(FMOD.Studio.TIMELINE_BEAT_PROPERTIES));
                info.currentBeat = beat.beat;
                Instance.pendingBeat = beat.beat;
                break;

            case FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_MARKER:
                var marker = (FMOD.Studio.TIMELINE_MARKER_PROPERTIES)
                    Marshal.PtrToStructure(parameterPtr, typeof(FMOD.Studio.TIMELINE_MARKER_PROPERTIES));
                info.lastMarker = marker.name;
                Instance.pendingMarker = marker.name;
                break;
        }

        return FMOD.RESULT.OK;
    }

    public void SetUpTimeLineInstance()
    {
        if (music.IsNull)
        {
            Debug.LogError("Music string is null");
            return;
        }

        timeLineInfo = new TimelineInfo();
        beatCallback = new FMOD.Studio.EVENT_CALLBACK(BeatCallback);
        timeLineHandle = GCHandle.Alloc(timeLineInfo, GCHandleType.Pinned);
        musicInstance.setUserData(GCHandle.ToIntPtr(timeLineHandle));
        musicInstance.setCallback(beatCallback, FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_BEAT | FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_MARKER);

    }
}
