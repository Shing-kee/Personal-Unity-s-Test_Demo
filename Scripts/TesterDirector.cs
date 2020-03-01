using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TesterDirector : MonoBehaviour
{
    public PlayableDirector playDir;

    public Animator attacker;
    public Animator victim;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.H)){
            foreach (var item in playDir.playableAsset.outputs)
            {
                if(item.streamName == "Attacker Animation")
                {
                    playDir.SetGenericBinding(item.sourceObject, attacker); 
                }
                else if (item.streamName == "Victim Animation") 
                {
                    playDir.SetGenericBinding(item.sourceObject,victim);
                }
            }

            //playDir.time = 0;
            //playDir.Stop();
            playDir.Play();
            //playDir.Evaluate();
        }
    }
}
