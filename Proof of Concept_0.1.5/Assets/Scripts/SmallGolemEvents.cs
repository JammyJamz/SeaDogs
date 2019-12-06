using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallGolemEvents : MonoBehaviour
{
    public bool inAttackWindow;
    public bool hitInThisWindow;
    // Start is called before the first frame update
    void Start()
    {
        inAttackWindow = false;
        hitInThisWindow = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartAttackWindow()
    {
        inAttackWindow = true;
    }

    public void EndAttackWindow()
    {
        inAttackWindow = false;
        hitInThisWindow = false;
    }
}
