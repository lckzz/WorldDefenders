using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager
{
    AudioSource[] _audioSources = new AudioSource[(int)Define.Sound.MaxCount];

    Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();


    public void Init()
    {
        GameObject root = GameObject.Find("@Sound");
        if(root == null)
        {
            root = new GameObject { name = "@Sound" };
            Object.DontDestroyOnLoad(root);

            string[] soundNames = System.Enum.GetNames(typeof(Define.Sound));
            for(int i = 0; i < soundNames.Length - 1; i++)
            {
                GameObject go = new GameObject { name = soundNames[i] };
                _audioSources[i] = go.AddComponent<AudioSource>();
                go.transform.parent = root.transform;
            }

            _audioSources[(int)Define.Sound.BGM].loop = true;
        }
    }


    public void Clear()
    {
        foreach(AudioSource audioSource in _audioSources)
        {
            if(audioSource != null)
            {
                audioSource.clip = null;
                audioSource.Stop();
            }

        }
        _audioClips.Clear();
    }

    public void Play(string path, Define.Sound type = Define.Sound.Effect, float pitch = 1.0f)
    {

        if (path.Contains("Sounds/") == false)
            path = $"Sounds/{path}";

        if(type == Define.Sound.BGM)
        {
            AudioClip audioClip = Managers.Resource.Load<AudioClip>(path);

            if(audioClip == null)
            {
                Debug.Log($"AudioClip Missing! {path}");
                return;
            }

            AudioSource audioSource = _audioSources[(int)Define.Sound.BGM];

            if (audioSource.isPlaying)
                audioSource.Stop();

            audioSource.pitch = pitch;
            audioSource.clip = audioClip;
            audioSource.volume = Managers.Game.BgmValue;
            audioSource.mute = Managers.Game.BgmisOn;
            audioSource.Play();

        }
        else
        {
            AudioClip audioClip = GetOrAddAudioClip(path);

            if (audioClip == null)
            {
                Debug.Log($"AudioClip Missing! {path}");
                return;
            }


            AudioSource audioSource = _audioSources[(int)Define.Sound.Effect];
            audioSource.pitch = pitch;
            audioSource.volume = Managers.Game.EffValue;
            audioSource.mute = Managers.Game.EffisOn;
            audioSource.PlayOneShot(audioClip);
        }


    }


    AudioClip GetOrAddAudioClip(string path)
    {
        AudioClip audioClip = null;
        if(_audioClips.TryGetValue(path,out audioClip) == false)
        {

            audioClip = Managers.Resource.Load<AudioClip>(path);
            _audioClips.Add(path, audioClip);
        }

        return audioClip;
    }


    public void SoundMute(Define.Sound type,bool toggle)
    {
        if (_audioSources.Length <= 0)
            return;

        _audioSources[(int)type].mute = toggle;
    }

    public void SoundValue(Define.Sound type, float value)
    {
        if (_audioSources.Length <= 0)
            return;

        _audioSources[(int)type].volume = value;
    }
}
