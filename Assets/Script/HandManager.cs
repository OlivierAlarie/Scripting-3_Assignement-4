using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource _chooseHandClip;

    public void PlayHand(string handname)
    {
        if(handname == "Rock" || handname == "Paper" || handname == "Scissors" && _chooseHandClip != null && !_chooseHandClip.isPlaying)
        {
            _chooseHandClip.Play();
        }
    }
}
