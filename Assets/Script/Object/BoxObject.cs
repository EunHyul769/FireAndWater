using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxObject : InteractObject
{
    [SerializeField] SpriteRenderer mySprite;

    void Start()
    {
        StageInfo stgInfo = GameObject.Find("StageInfos").GetComponent<StageInfo>();
        mySprite.sprite = stgInfo.stageBoxsprite;
    }
}
