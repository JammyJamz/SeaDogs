using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasData : MonoBehaviour
{
    public static GameObject throwText;

    private LocalCanvasData localData;

    public static GameObject ladderDropText;

    public static GameObject platformSpawnText;

    public static LevelLoader levelLoader;

    public static Animator saltyAnim;
    public static Animator rustyAnim;

    public static GameObject missingJungleText;
    public static GameObject missingTempleText;

    // Start is called before the first frame update
    void Start()
    {
        localData = GetComponent<LocalCanvasData>();

        throwText = localData.throwText;

        ladderDropText = localData.ladderDropText;

        platformSpawnText = localData.platformSpawnText;

        levelLoader = localData.levelLoader;

        saltyAnim = localData.saltyAnim;
        rustyAnim = localData.rustyAnim;

        missingJungleText = localData.missingJungleText;
        missingTempleText = localData.missingTempleText;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
