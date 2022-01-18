using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class MonsterController : CreatureController
{
    protected override void Init()
    {
        base.Init();
    }

    protected override void UpdateController()
    {
        //GetDir();
        base.UpdateController();
    }

    void GetDir() //Å°ÀÔ·Â
    {
        if (Input.GetKey(KeyCode.W))
        {
            Dir = MoveDir.Up;
            //anim.SetFloat("DirY", 1);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            Dir = MoveDir.Down;

        }
        else if (Input.GetKey(KeyCode.A))
        {
            Dir = MoveDir.Left;

        }
        else if (Input.GetKey(KeyCode.D))
        {
            Dir = MoveDir.Right;
        }
        else
        {
            Dir = MoveDir.None;
        }
    }
}
