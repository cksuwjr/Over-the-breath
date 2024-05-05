using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TimelineControl : MonoBehaviour
{

    PlayableDirector playableDirector;
    public TimelineAsset timeline;

    [SerializeField]
    public List<float> PauseTime;
    private void Start()
    {
        playableDirector = GetComponent<PlayableDirector>();
    }
    public void Play()
    {
        playableDirector.Play();
        
    }

    public void PlayFromTimeline()
    {
        playableDirector.Play(timeline);
    }
    public void Play(float time)
    {
        Invoke("Play", time);
     
    }
    public double GetPlaytime()
    {
        return playableDirector.time;
    }
    public void Pause()
    {
        playableDirector.Pause();
    }
    public void Resume()
    {
        playableDirector.Resume();
    }

}
              