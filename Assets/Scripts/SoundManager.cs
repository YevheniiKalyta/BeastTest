using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class SoundManager : MonoBehaviour
{
    private ObjectPool<AudioSource> soundsPool;
    [SerializeField] private AudioSource soundObjPrefab;
    [SerializeField] List<AudioContainer> containerList;
    [SerializeField] List<AudioSource> activeSources;
    public static SoundManager Instance;

    [System.Serializable]
    public struct AudioContainer
    {
        public string name;
        public AudioClip clip;
        public float volume;
    }

    private void Awake()
    {
        soundsPool = new ObjectPool<AudioSource>(OnCreate, (x) => x.gameObject.SetActive(true), (x) => x.gameObject.SetActive(false));
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;
    }

    private AudioSource OnCreate()
    {
        return Instantiate(soundObjPrefab);
    }

    public void PlaySFXOneShot(string name)
    {
        AudioSource audioSource = soundsPool.Get();
        AudioContainer ac = GetSFXByName(name);
        audioSource.volume = ac.volume;
        audioSource.PlayOneShot(ac.clip);
        StartCoroutine(DelayedRelease(audioSource, ac.clip.length));
    }
    public void PlaySFXLoop(string name)
    {
        AudioSource audioSource = soundsPool.Get();
        AudioContainer ac = GetSFXByName(name);
        audioSource.loop = true;
        audioSource.volume = ac.volume;
        audioSource.clip = ac.clip;
        audioSource.name = ac.name;
        activeSources.Add(audioSource);
        audioSource.Play();
    }

    public void StopSXFLoop(string name)
    {
        for (int i = 0; i < activeSources.Count; i++)
        {
            if (activeSources[i].name == name)
            {
                activeSources[i].DOFade(0f, 0.25f).OnComplete(() =>
                {
                    soundsPool.Release(activeSources[i]);
                    activeSources.RemoveAt(i);
                });
                break;
            }
        }
    }

    public IEnumerator DelayedRelease(AudioSource audioSource, float time)
    {
        yield return new WaitForSeconds(time);
        soundsPool.Release(audioSource);
    }
    private AudioContainer GetSFXByName(string name)
    {
        for (int i = 0; i < containerList.Count; i++)
        {
            if (containerList[i].name == name)
            {
                return containerList[i];
            }
        }
        Debug.LogError($"Can't find SFX called {name}");
        return new AudioContainer();
    }
}
