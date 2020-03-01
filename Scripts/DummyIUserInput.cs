using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 伪操作输出
/// </summary>
public class DummyIUserInput : IUserInput
{
    // Start is called before the first frame update
    IEnumerator Start()
    {
        while (true)
        {
            //Dup = 1.0f;
            //Dright = 0;
            //JUp = 0;
            //JRight = 1.0f;
            //run = true;
            //yield return new WaitForSeconds(3.0f);
            //Dup = 0f;
            //Dright = 0;
            //JUp = 0;
            //JRight = 0f;
            rAttack = true;
            yield return 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDmagDvec(Dup, Dright);
    }
}
