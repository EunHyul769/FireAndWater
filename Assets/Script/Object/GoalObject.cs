using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalObject : InteractObject
{
    public override void Interact()
    {
        base.Interact();
        // 플레이어 충돌 시 속성에 맞는 문이 열리도록
    }

    public override void InteractOut()
    {
        base.InteractOut();
        //플레이어 충돌 끝 문 닫힘 
    }
}
