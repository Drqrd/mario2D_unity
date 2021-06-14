using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AudioController : MonoBehaviour
{
    DirectoryInfo soundDirectory;
    FileInfo[] soundInfo;

    void Start()
    {
        // Load sound directory
        soundDirectory = new DirectoryInfo("Assets/Resources/Sounds");
        // get information on each item in the directory (specifically names)
        soundInfo = soundDirectory.GetFiles("*.wav", SearchOption.AllDirectories);

        // Loop through each sound in the sounds folder
        foreach (FileInfo name in soundInfo)
        {
            GenerateSound(Path.GetFileNameWithoutExtension(name.Name));
        }
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

    void PlaySound(string name)
    {
        GameObject.Find(name).GetComponent<AudioSource>().Play();
    }
}
