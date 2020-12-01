using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioSystem : MonoBehaviour
{
    public static int CHANEL_AMOUNT = 5;

    public static AudioSystem Instance;

    public List<string> registeredClips;

    private Dictionary<string, AudioClip> clips;
    private AudioSource[] chanels;

    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);

        clips = new Dictionary<string, AudioClip>();
        chanels = new AudioSource[CHANEL_AMOUNT];

        for (int i = 0; i < CHANEL_AMOUNT; i++) {
            chanels[i] = new GameObject().AddComponent<AudioSource>();
            chanels[i].transform.SetParent(transform);
        }

        LoadSound();
    }

    public void PlaySound(string path, int chanel_id = 0) {
        if (!clips.ContainsKey(path)) {
            AudioClip clip = Resources.Load<AudioClip>(path);
            clips.Add(path, clip);
        }

        chanels[chanel_id].clip = clips[path];
        chanels[chanel_id].Play();
    }

    public void SetLooping(bool looping, int chanel_id = 0) {
        chanels[chanel_id].loop = looping;
    }

    void LoadSound() {
        for (int i = 0; i < registeredClips.Count; i++) {
            AudioClip clip = Resources.Load<AudioClip>(registeredClips[i]);
            clips.Add(registeredClips[i], clip);
        }
    }


}
