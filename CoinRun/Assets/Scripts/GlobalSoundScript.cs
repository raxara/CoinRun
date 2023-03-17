using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalSoundScript : MonoBehaviour
{

    [SerializeField]
    AudioSource audioSource;

    [SerializeField]
    List<AudioClip> soundsList;

    [SerializeField]
    float minTimer = 10;

    [SerializeField]
    float maxTimer = 20;

    [SerializeField]
    List<SoundPosition> soundPosList;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(timerCorout());
    }

    IEnumerator timerCorout()
    {
        float delay;
        while (true)
        {
            delay = setNextSound();
            while (audioSource.isPlaying) yield return null;
            float t = 0;
            while (t < delay)
            {
                yield return null;
                t += Time.deltaTime; 
            }
            Debug.Log("playing " + audioSource.clip.name);
        }
    }

    float setNextSound()
    {
        SoundPosition nextSound = soundPosList[Random.Range(0, soundPosList.Count)];
        audioSource.clip = nextSound.clip;
        transform.position = nextSound.getPosition;
        audioSource.Play();
        return Random.Range(minTimer, maxTimer);
    }

}

[System.Serializable]
public class SoundPosition
{

    public AudioClip clip;

    [SerializeField]
    List<Transform> positionList;

    public Vector3 getPosition { get
        {
            int index = Random.Range(0, positionList.Count);
            return positionList[index].position;
        } }
}
