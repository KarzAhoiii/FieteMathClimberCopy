using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class FMC_IntroAnimation : MonoBehaviour
{

    public SkeletonAnimation fieteBig;
    public SkeletonAnimation fieteSmall;

    // Big
    public AudioClip fiete;
    public AudioClip atmosphere;

    // Small
    public AudioClip swoosh;
    public AudioClip coinHit;
    public AudioClip fieteHit;
    public AudioClip coinPoof;

    private void Awake ()
    {
        LeanTween.delayedCall(0.35f, startAnimation);
    }

    private void startAnimation ()
    {
        fieteBig.gameObject.SetActive(true);
        fieteSmall.gameObject.SetActive(true);
        fieteSmall.state.Event += getSpineEvent;

        LeanTween.delayedCall(0.45f, playSwoosh);
        LeanTween.delayedCall(0.7f, playFiete);
        LeanTween.delayedCall(0.7f, playAtmo);
        LeanTween.delayedCall(2.8f, playSwoosh);
    }

    private void playSwoosh()
    {
        LeanAudio.play(swoosh, 0.45f);
    }

    private void playFiete()
    {
        LeanAudio.play(fiete, 0.8f);
    }

    private void playAtmo()
    {
        LeanAudio.play(atmosphere, 0.8f);
    }

    private void getSpineEvent(Spine.TrackEntry entry, Spine.Event e)
    {

        if (e.Data.Name == "snd_swoosh")
            LeanAudio.play(swoosh);
        else if (e.Data.Name == "coin_hit")
            LeanAudio.play(coinHit);
        else if (e.Data.Name == "snd_hit_fiete")
            LeanAudio.play(fieteHit);
        else if (e.Data.Name == "coin_poof")
            LeanAudio.play(coinPoof, 1.0f);

    }
}
