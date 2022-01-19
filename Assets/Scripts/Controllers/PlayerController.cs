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
        //�̵����� Ȯ��
        if (Dir != MoveDir.None)
        {
            State = CreatureState.Walking;
            return;
        }

        //�������� Ȯ��
        if (Input.GetKey(KeyCode.Space))
        {
            State = CreatureState.Action;
            //_coAction = StartCoroutine(CoStartAttack());
            _coAction = StartCoroutine("CoStartShootArrow");
        }
    }

    void GetDirInput() //Ű�Է�
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
        //�ǰ�
        GameObject go = Managers.Object.Find(GetFrontCellPos());
        if (go != null)
        {
            CreatureController cc = go.GetComponent<CreatureController>();
            if (cc != null)
                cc.OnDamaged();
        }

        //���ð�
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

        //���ð�
        yield return new WaitForSeconds(0.5f);
        State = CreatureState.Idle;
        _coAction = null;
    }

    protected override void SetSorting()
    {
        SpriteRenderer sp = gameObject.GetComponent<SpriteRenderer>();
        sp.sortingLayerName = "Creature";
        sp.sortingOrder = ((int)transform.position.y * -1) + 1;
    }

    public override void OnDamaged()
    {
        Debug.Log("player Hit!");
    }
}
