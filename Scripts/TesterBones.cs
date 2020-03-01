using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TesterBones : MonoBehaviour
{
    public SkinnedMeshRenderer srcMeshRenderer;
    public List<SkinnedMeshRenderer> TgtMeshRenderer;
    // Start is called before the first frame update
    void Start()
    {
        foreach (var t in TgtMeshRenderer)
        {
            t.bones = srcMeshRenderer.bones;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
