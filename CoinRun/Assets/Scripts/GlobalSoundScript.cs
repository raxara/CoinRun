using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//script qui gere les sons environnementaux
public class GlobalSoundScript : MonoBehaviour
{

    [SerializeField]
    AudioSource audioSource;

    [SerializeField]
    List<AudioClip> soundsList;

    //temps minimum et maximum entre 2 sons
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

    //la coroutine qui joue un son avec la fonction SetNextSound puis attend le delai aleatoire que al fonction lui retourne
    IEnumerator timerCorout()
    {
        float delay;
        while (true)
        {
            delay = SetNextSound();
            while (audioSource.isPlaying) yield return null;
            float t = 0;
            while (t < delay)
            {
                yield return null;
                t += Time.deltaTime; 
            }
        }
    }

    //fonction qui choisis un son au hasard, le joue, puis renvoie un delai aleatoire 
    float SetNextSound()
    {
        SoundPosition nextSound = soundPosList[Random.Range(0, soundPosList.Count)];
        audioSource.clip = nextSound.clip;
        transform.position = nextSound.getRandomPosition;
        audioSource.Play();
        return Random.Range(minTimer, maxTimer);
    }

}

//classe permettant de lier un clip audio a une liste de positions d'ou ce son pourrait etre joué
[System.Serializable]
public class SoundPosition
{

    public AudioClip clip;

    [SerializeField]
    List<Transform> positionList;

    //propriété renvoyant une position aléatoire d'ou le son sera joué
    public Vector3 getRandomPosition { get
        {
            int index = Random.Range(0, positionList.Count);
            return positionList[index].position;
        } }
}
