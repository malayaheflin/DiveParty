using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSoundMgr : MonoBehaviour
{
    public void PlaySound(string SoundName){
        SoundMgr.Instance.PlaySound(SoundName);
    }
}
