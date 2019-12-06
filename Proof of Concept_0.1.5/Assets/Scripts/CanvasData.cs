﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasData : MonoBehaviour
{
    public static GameObject throwText;

    private LocalCanvasData localData;

    public static GameObject ladderDropText;

    public static GameObject platformSpawnText;

    public static LevelLoader levelLoader;

    // Start is called before the first frame update
    void Start()
    {
        localData = GetComponent<LocalCanvasData>();

        throwText = localData.throwText;

        ladderDropText = localData.ladderDropText;

        platformSpawnText = localData.platformSpawnText;

        levelLoader = localData.levelLoader;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
