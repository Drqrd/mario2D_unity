using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AudioController : MonoBehaviour
{
    DirectoryInfo soundDirectory, musicDirectory;
    FileInfo[] soundInfo, musicInfo;

    void Start()
    {
        // Load sound and music directories
        soundDirectory = new DirectoryInfo("Assets/Resources/Sounds");
        musicDirectory = new DirectoryInfo("Assets/Resources/Music");
        // get information on each item in the directories (specifically names)
        soundInfo = soundDirectory.GetFiles("*.wav", SearchOption.AllDirectories);
        musicInfo = musicDirectory.GetFiles("*.mp3", SearchOption.AllDirectories);

        // Loop through each sound in the sounds folder
        foreach (FileInfo name in soundInfo)
        {
            GenerateSound(Path.GetFileNameWithoutExtension(name.Name));
        }
        foreach (FileInfo name in musicInfo)
        {
            GenerateMusic(Path.GetFileNameWithoutExtension(name.Name));
        }

        // Play the soundtrack for the corresponding level
        string tag = GameObject.Find("World").tag;
        PlaySound(tag);
    }

    // So you don't have to call it every time
    void GenerateSound(string name)
    {
        GameObject obj = new GameObject(name);
        obj.transform.parent = transform;
        AudioSource aud = obj.AddComponent<AudioSource>();
        aud.clip = (AudioClip)Resources.Load("Sounds/" + name, typeof(AudioClip));
        aud.loop = false;
        aud.playOnAwake = false;
    }

    void GenerateMusic(string name)
    {
        GameObject obj = new GameObject(name);
        obj.transform.parent = transform;
        AudioSource aud = obj.AddComponent<AudioSource>();
        aud.clip = (AudioClip)Resources.Load("Music/" + name, typeof(AudioClip));
        aud.volume = .6f;
        aud.loop = true;
        aud.playOnAwake = false;
    }

    // Accessible method to play a sound
    public static void PlaySound(string name)
    {
        GameObject.Find("AudioController").transform.Find(name).GetComponent<AudioSource>().Play();
    }

    // Accessible method to stop a sound
    public static void StopSound(string name)
    {
        GameObject.Find("AudioController").transform.Find(name).GetComponent<AudioSource>().Stop();
    }
}
