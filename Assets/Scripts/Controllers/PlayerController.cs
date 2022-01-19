using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class PlayerController : CreatureController
{
    Coroutine _coAction;

    private void LateUpdate()
    {
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
    }

    protected override void Init()
    {
        base.Init();
    }

    //protected override void UpdateAnimation()
    //{
    //    if (_state == CreatureState.Idle)
    //    {
    //        anim.SetBool("Walking", false);
    //        anim.SetBool("Attack", false);
    //    }
    //    else if (_state == CreatureState.Walking)
    //    {
    //        anim.SetBool("Walking", true);
    //        switch (_dir)
    //        {

    //            case MoveDir.Up:
    //                anim.SetFloat("DirX", 0);
    //                anim.SetFloat("DirY", 1);
    //                _lastDir = MoveDir.Up;
    //                break;
    //            case MoveDir.Down:
    //                anim.SetFloat("DirX", 0);
    //                anim.SetFloat("DirY", -1);
    //                _lastDir = MoveDir.Down;
    //                break;
    //            case MoveDir.Left:
    //                anim.SetFloat("DirX", -1);
    //                anim.SetFloat("DirY", 0);
    //                _lastDir = MoveDir.Left;
    //                break;
    //            case MoveDir.Right:
    //                anim.SetFloat("DirX", 1);
    //                anim.SetFloat("DirY", 0);
    //                _lastDir = MoveDir.Right;
    //                break;
    //        }
    //    }
    //    else if (_state == CreatureState.Action)
    //    {
    //        anim.SetBool("Attack", true);
    //    }
    //    else if (_state == CreatureState.Dead)
    //    {

    //    }
    //    else
    //    {

    //    }
    //}

    protected override void UpdateController()
    {
        switch (State)
        {
            case CreatureState.Idle:
                GetDirInput();
                break;
            case CreatureState.Walking:
                GetDirInput();
                break;
        }

        base.UpdateController();
    }

    protected override void UpdateIdle()
    {
        //이동할지 확인
        if (Dir != MoveDir.None)
        {
            State = CreatureState.Walking;
            return;
        }

        //공격할지 확인
        if (Input.GetKey(KeyCode.Space))
        {
            State = CreatureState.Action;
            //_coAction = StartCoroutine(CoStartAttack());
            _coAction = StartCoroutine(CoStartShootArrow());
        }
    }

    void GetDirInput() //키입력
    {
        if (Input.GetKey(KeyCode.W))
        {
            Dir = MoveDir.Up;
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

    IEnumerator CoStartAttack()
    {
        //피격
        GameObject go = Managers.Object.Find(GetFrontCellPos());
        if (go != null)
        {
            CreatureController cc = go.GetComponent<CreatureController>();
            if (cc != null)
                cc.OnDamaged();
        }

        //대기시간
        yield return new WaitForSeconds(0.5f);
        State = CreatureState.Idle;
        _coAction = null;
    }

    IEnumerator CoStartShootArrow()
    {
        GameObject go = Managers.Resource.Instantiate("Creature/Arrow");
        ArrowController ac = go.GetComponent<ArrowController>();
        ac.Dir = _lastDir;
        ac.CellPos = CellPos;

        //대기시간
        yield return new WaitForSeconds(0.3f);
        State = CreatureState.Idle;
        _coAction = null;
    }

    protected override void SetSorting()
    {
        SpriteRenderer sp = gameObject.GetComponent<SpriteRenderer>();
        sp.sortingLayerName = "Creature";
        sp.sortingOrder = ((int)transform.position.y * -1) + 1;
    }
}
