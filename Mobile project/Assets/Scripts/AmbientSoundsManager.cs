using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientSoundsManager : MonoBehaviour
{
    [SerializeField] private float minDelay, maxDelay;
    float timePassed;
    float delay;

    private void Start()
    {
        delay = Random.Range(minDelay,maxDelay);
    }

    void FixedUpdate()
    {
        timePassed += Time.deltaTime;
        if(timePassed >= delay)
        {
            PlayRandomCrystalSound();
            delay = Random.Range(minDelay, maxDelay);
        }
    }

    void PlayRandomCrystalSound()
    {
        AudioManager.instance.PlayRandomSound("AMBIENT_Crystals");
    }
}
