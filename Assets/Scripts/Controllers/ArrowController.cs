using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class ArrowController : CreatureController
{
    public int maxrange;
    public int range;

    protected override void Init()
    {
        switch (_dir)
        {
            case MoveDir.Up:
                transform.rotation = Quaternion.Euler(0, 0, -90);
                break;
            case MoveDir.Down:
                transform.rotation = Quaternion.Euler(0, 0, 90);
                break;
            case MoveDir.Left:
                break;
            case MoveDir.Right:
                gameObject.GetComponent<SpriteRenderer>().flipX = true;
                break;
        }

        State = CreatureState.Walking;
        speed = 10;
        //base.Init();

        Vector3 pos = Managers.Map.CurrentGrid.CellToWorld(CellPos) + new Vector3(0.5f, 0.5f);
        transform.position = pos;

    }

    protected override void UpdateAnimation()
    {

    }

    protected override void WalkToNextPos()
    {
        Vector3Int destPos = CellPos;

        switch (_dir)
        {
            case MoveDir.Up:
                destPos += Vector3Int.up;
                break;
            case MoveDir.Down:
                destPos += Vector3Int.down;
                break;
            case MoveDir.Left:
                destPos += Vector3Int.left;
                break;
            case MoveDir.Right:
                destPos += Vector3Int.right;
                break;
        }

        if (Managers.Map.CanGo(destPos))
        {
            GameObject go = Managers.Object.Find(destPos);
            if (go == null)
            {
                CellPos = destPos;
            }
            else
            {
                CreatureController cc = go.GetComponent<CreatureController>();
                if (cc != null)
                    cc.OnDamaged();
                Managers.Resource.Destroy(gameObject);
            }
        }
        else
        {
            Managers.Resource.Destroy(gameObject);
        }
    }

    protected override void UpdateWalking()
    {
        Vector3 destPos = Managers.Map.CurrentGrid.CellToWorld(CellPos) + new Vector3(0.5f, 0.5f);
        Vector3 moveDir = destPos - transform.position;

        //도착여부 체크
        float dist = moveDir.magnitude;
        if (dist < speed * Time.deltaTime)
        {
            transform.position = destPos;
            WalkToNextPos();

            range++;
            if (range >= maxrange) //공격 사거리
                Managers.Resource.Destroy(gameObject);
        }
        else
        {
            transform.position += moveDir.normalized * speed * Time.deltaTime;
            State = CreatureState.Walking;
        }
    }

    protected override void SetSorting()
    {
        SpriteRenderer sp = gameObject.GetComponent<SpriteRenderer>();
        sp.sortingLayerName = "Creature";
        sp.sortingOrder = ((int)transform.position.y * -1) + 10;
    }
}
