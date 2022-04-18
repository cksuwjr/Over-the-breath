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
    private void Update()
    {
        if (playableDirector.time > 0)
        {
            for (int i = 0; i < PauseTime.Count; i++)
            {
                if (PauseTime[i] == playableDirector.time)
                    Pause();
            }
        }
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
        Debug.Log(" ½ÇÇà " + time);
    }
    public double GetPlaytime()
    {
        return timeline.duration;
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
