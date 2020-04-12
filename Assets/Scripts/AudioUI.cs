using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioUI : MonoBehaviour
{
    public Dropdown audioDeviceList;
    public InputField userName;
    public List<string> audioDevices;
    int audioDevicesLength = 0;

    private void Start()
    {
        audioDevicesLength = Microphone.devices.Length;
        GetAudioDeviceNames();
    }

    private void Update()
    {
        if (Microphone.devices.Length > audioDevicesLength)
        {
            audioDevicesLength = Microphone.devices.Length;
            GetAudioDeviceNames();
        }
        if (Microphone.devices.Length < audioDevicesLength)
        {
            audioDevicesLength = Microphone.devices.Length;
            GetAudioDeviceNames();
        }
    }
    private void GetAudioDeviceNames()
    {
        audioDevices.Clear();
        audioDeviceList.ClearOptions();
        for (int audioDevice = 0; audioDevice < audioDevicesLength; audioDevice++)
        {
            audioDevices.Add(Microphone.devices[audioDevice]);

        }
        audioDeviceList.AddOptions(audioDevices);
    }
}
