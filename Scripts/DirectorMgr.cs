using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[RequireComponent(typeof(PlayableDirector))]
public class DirectorMgr : IActorMgrInterface
{
    public PlayableDirector playDir;

    [Header("=== Timeline assets ===")]
    public TimelineAsset frontStab;
    public TimelineAsset openBox;
    public TimelineAsset leverUp;

    [Header("=== Assets Settings ===")]
    public ActorMgr attacker;
    public ActorMgr victim;
    // Start is called before the first frame update
    void Start()
    {
        playDir = GetComponent<PlayableDirector>();
        playDir.playOnAwake = false;
        //playDir.playableAsset = Instantiate<TimelineAsset>(frontStab);
    }

    public bool IsPlaying()
    {
        return true;
    }

    public void PlayFrontStab(string timelineName,ActorMgr attacker,ActorMgr Victim)
    {
        //if(playDir.state == PlayState.Playing)
        //{
        //    return;
        //}

        if (timelineName == "frontStab")
        {
            playDir.playableAsset = Instantiate(frontStab);

            TimelineAsset timeline = (TimelineAsset)playDir.playableAsset;

            foreach (var track in timeline.GetOutputTracks())
            {
                if(track.name == "Attacker Script")
                {
                    playDir.SetGenericBinding(track, attacker);
                    foreach (var clip in track.GetClips())
                    {
                        MySuperPlayableClip myClips = (MySuperPlayableClip)clip.asset;
                        MySuperPlayableBehaviour myBehav = myClips.template;
                        myBehav.myFloat = 666;
                        //myClips._actMgr.exposedName = GetInstanceID().ToString();//18年版本官方没有初始化，也暂时忘记跟Operator讲述
                        myClips._actMgr.exposedName = System.Guid.NewGuid().ToString();//这种不会同一获得同一东西
                        playDir.SetReferenceValue(myClips._actMgr.exposedName, attacker);
                        Debug.Log(myClips._actMgr.exposedName);
                        //不支持加载固件，只能加载资源
                    }
                }
                else if(track.name == "Victim Script")
                {
                    playDir.SetGenericBinding(track, victim);
                    foreach (var clip in track.GetClips())
                    { 
                        MySuperPlayableClip myClips = (MySuperPlayableClip)clip.asset;
                        MySuperPlayableBehaviour myBehav = myClips.template;
                        myBehav.myFloat = 777;
                        myClips._actMgr.exposedName = System.Guid.NewGuid().ToString();//这种不会同一获得同一东西
                        playDir.SetReferenceValue(myClips._actMgr.exposedName, victim);
                        Debug.Log(myClips._actMgr.exposedName);
                        //不支持加载固件，只能加载资源
                    }
                }
                else if (track.name == "Attacker Animation")
                {
                    playDir.SetGenericBinding(track, attacker.actCtrl.anim);
                }
                else if (track.name == "Victim Animation")
                {
                    playDir.SetGenericBinding(track, victim.actCtrl.anim);
                }
            }
            playDir.Evaluate();//在MySuperPlayableClip的类里，要加入物体数值的初始化，先前官方没有添加
            playDir.Play();
        }

        else if(timelineName == "openBox")
        {
            playDir.playableAsset = Instantiate(openBox);

            TimelineAsset timeline = (TimelineAsset)playDir.playableAsset;

            foreach (var track in timeline.GetOutputTracks())
            {
                if (track.name == "Player Script")
                {
                    playDir.SetGenericBinding(track, attacker);
                    foreach (var clip in track.GetClips())
                    {
                        MySuperPlayableClip myClips = (MySuperPlayableClip)clip.asset;
                        MySuperPlayableBehaviour myBehav = myClips.template;
                        myClips._actMgr.exposedName = System.Guid.NewGuid().ToString();//这种不会同一获得同一东西
                        playDir.SetReferenceValue(myClips._actMgr.exposedName, attacker);
                        Debug.Log(myClips._actMgr.exposedName);
                    }
                }
                else if (track.name == "Box Script")
                {
                    playDir.SetGenericBinding(track, victim);
                    foreach (var clip in track.GetClips())
                    {
                        MySuperPlayableClip myClips = (MySuperPlayableClip)clip.asset;
                        MySuperPlayableBehaviour myBehav = myClips.template;
                        myClips._actMgr.exposedName = System.Guid.NewGuid().ToString();
                        playDir.SetReferenceValue(myClips._actMgr.exposedName, victim);
                        Debug.Log(myClips._actMgr.exposedName);
                    }
                }
                else if (track.name == "Player Animation")
                {
                    playDir.SetGenericBinding(track, attacker.actCtrl.anim);
                }
                else if (track.name == "Box Animation")
                {
                    Debug.Log("this is 123pre on DirectorMgr :" + victim.actCtrl.anim.name); 
                    playDir.SetGenericBinding(track, victim.actCtrl.anim);
                }
            }
            playDir.Evaluate();//在MySuperPlayableClip的类里，要加入物体数值的初始化，先前官方没有添加
            playDir.Play();
        }

        else if(timelineName == "leverUp")
        {
            playDir.playableAsset = Instantiate(leverUp);

            TimelineAsset timeline = (TimelineAsset)playDir.playableAsset;

            foreach (var track in timeline.GetOutputTracks())
            {
                if (track.name == "Player Script")
                {
                    playDir.SetGenericBinding(track, attacker);
                    foreach (var clip in track.GetClips())
                    {
                        MySuperPlayableClip myClips = (MySuperPlayableClip)clip.asset;
                        MySuperPlayableBehaviour myBehav = myClips.template;
                        myClips._actMgr.exposedName = System.Guid.NewGuid().ToString();//这种不会同一获得同一东西
                        playDir.SetReferenceValue(myClips._actMgr.exposedName, attacker);
                        //Debug.Log(myClips._actMgr.exposedName);
                    }
                }
                else if (track.name == "Lever Script")
                {
                    playDir.SetGenericBinding(track, victim);
                    foreach (var clip in track.GetClips())
                    {
                        MySuperPlayableClip myClips = (MySuperPlayableClip)clip.asset;
                        MySuperPlayableBehaviour myBehav = myClips.template;
                        myClips._actMgr.exposedName = System.Guid.NewGuid().ToString();
                        playDir.SetReferenceValue(myClips._actMgr.exposedName, victim);
                    }
                }
                else if (track.name == "Player Animation")
                {
                    playDir.SetGenericBinding(track, attacker.actCtrl.anim);
                    //Debug.Log(attacker.actCtrl.anim);
                }
                else if (track.name == "Lever Animation")
                {
                    playDir.SetGenericBinding(track, victim.actCtrl.anim);
                }
            }
            playDir.Evaluate();//在MySuperPlayableClip的类里，要加入物体数值的初始化，先前官方没有添加
            playDir.Play();
        }
    }
}
