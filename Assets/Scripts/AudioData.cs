using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public class AudioData : MonoBehaviour
{
    private string audioFileName, currentDate, currentTime, audioFileNameFormat;
    AudioRecorder audioRecorder;
    public List<string> audioFilesinRootDirectory;
    int fileCount = 0;
    string audioFileNameInDirectory;
    string[] splitFileNames;
    private void Start()
    {
        fileCount = Directory.GetFiles(Application.persistentDataPath + "/Clips/").Length;
        audioRecorder = GetComponent<AudioRecorder>();
        GetAllRecordedSound();
    }
    private void Update()
    {
        if (fileCount != Directory.GetFiles(Application.persistentDataPath + "/Clips/").Length)
        {
            fileCount = Directory.GetFiles(Application.persistentDataPath + "/Clips/").Length;
            GetAllRecordedSound();
        }
    }
    public AudioFile createAudioFile()
    {
        audioRecorder = GetComponent<AudioRecorder>();
        AudioFile audioFile = new AudioFile();
        audioFile.sample = new float[audioRecorder.recorderAudio.samples * audioRecorder.recorderAudio.channels];
        audioFile.frequency = audioRecorder.recorderAudio.frequency;
        audioFile.samples = audioRecorder.recorderAudio.samples;
        audioFile.channels = audioRecorder.recorderAudio.channels;
        audioRecorder.recorderAudio.GetData(audioFile.sample, 0);
        //audioFile.audioData = _recorderAudio.LoadAudioData();
        return audioFile;
    }

    public void SaveAsAudioFile()
    {
        audioRecorder = GetComponent<AudioRecorder>();
        audioFileName = audioRecorder.audioUI.userName.text;
        currentDate = System.DateTime.Now.ToString("yyyy-MM-dd");
        currentTime = DateTime.Now.ToString("HH-mm-ss");
        audioFileNameFormat = audioFileName + "_" + currentDate + "_" + currentTime + ".mp3";
        AudioFile audioFile = createAudioFile();
        BinaryFormatter bf = new BinaryFormatter();
        if (Directory.Exists(Application.persistentDataPath + "/Clips"))
        {
            if (!File.Exists(Application.persistentDataPath + "/" + audioFileNameFormat))
            {
                FileStream fileStream = File.Create(Application.persistentDataPath + "/Clips/" + audioFileNameFormat);
                bf.Serialize(fileStream, audioFile);
                fileStream.Close();
            }
        }
        else
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Clips");
            if (!File.Exists(Application.persistentDataPath + "/Clips/" + audioFileNameFormat))
            {
                FileStream fileStream = File.Create(Application.persistentDataPath + "/Clips/" + audioFileNameFormat);
                bf.Serialize(fileStream, audioFile);
                fileStream.Close();
            }
        }
    }
    public void LoadAudioFile(AudioSource audioSource, string filename)
    {
        if (File.Exists(Application.persistentDataPath + "/Clips/" + filename))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/Clips/" + filename, FileMode.Open);
            AudioFile audioFile = (AudioFile)bf.Deserialize(file);
            file.Close();
            AudioClip newClip = AudioClip.Create(filename, audioFile.samples, audioFile.channels, audioFile.frequency, false);
            newClip.SetData(audioFile.sample, 0);
            audioSource.clip = newClip;
            audioSource.Play();
        }
        else
        {
            Debug.Log("File Not Found!");
        }
    }

    public void PlayLastRecording()
    {
        audioRecorder = GetComponent<AudioRecorder>();
        LoadAudioFile(audioRecorder.audioSource, audioFileNameFormat);
    }

    private void GetAllRecordedSound()
    {
        string seprator = "/";
        audioFilesinRootDirectory.Clear();
        for (int files = 0; files < fileCount; files++)
        {
            audioFileNameInDirectory = Directory.GetFiles(Application.persistentDataPath + "/Clips/")[files];
            splitFileNames = audioFileNameInDirectory.Split(seprator.ToCharArray()[0]);
            audioFilesinRootDirectory.Add(splitFileNames[splitFileNames.Length - 1]);
        }
    }
}
