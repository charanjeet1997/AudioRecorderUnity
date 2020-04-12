using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class AudioRecorder : MonoBehaviour
{
    [HideInInspector]
    public AudioClip recorderAudio;
    public GameObject audioSystem;
    [HideInInspector]
    public AudioSource audioSource;
    AudioData audioData;
    [HideInInspector]
    public AudioUI audioUI;   
    private void Start()
    {
        audioSource = audioSystem.GetComponent<AudioSource>();
        audioUI = GetComponent<AudioUI>();
        audioData = GetComponent<AudioData>();
        
    }
   
    private void SetMicrophoneDevice()
    {
        recorderAudio = Microphone.Start(audioUI.audioDeviceList.options[audioUI.audioDeviceList.value].text, false, 2400, 44100);
    }
    public void StartRecording()
    {
        SetMicrophoneDevice();
    }
    public void StopRecording()
    {
        Microphone.End(audioUI.audioDeviceList.options[audioUI.audioDeviceList.value].text);
        AudioFile audioFile = audioData.createAudioFile();
        audioSource.clip = recorderAudio;
        //audioSource.Play();
        audioData.SaveAsAudioFile();
        
    }
}
