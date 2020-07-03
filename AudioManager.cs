using UnityEngine;
using UnityEngine.Audio;
using System;
using System.Collections;
using System.Collections.Generic;
public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager instance;

    void Awake(){

        if (instance == null){
            instance = this;
        }else{
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        foreach (Sound s in sounds){
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.pitch = s.pitch;
            s.source.volume = s.volume;
            s.source.loop = s.loop;
        }
        
    }

    void Start(){
        Play("Theme", 1, .4f);
    }

    public void Play(string name, float pitch, float volume){
        Sound s = Array.Find(sounds, sound => sound.name == name );
        if (s == null){
            Debug.LogWarning("AUDIO" + name + "NAO ENCONTRADO BURRO ANIMAL");
            return;
        }
        s.source.Play();
        s.source.pitch = pitch;
        s.source.volume = volume;
    }

    public void Stop(string name){
        Sound s = Array.Find(sounds, sound => sound.name == name );
        s.source.Stop();
    }  
}   
