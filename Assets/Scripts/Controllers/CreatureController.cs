using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class CreatureController : MonoBehaviour
{
    public float speed = 5.0f;
    
    public Vector3Int CellPos { get; set; } = Vector3Int.zero;
    protected  Animator anim;

    public CreatureState _state = CreatureState.idle;
    public CreatureState State
    {
        get { return _state; }
        set 
        {
            if (_state == value)
                return;

            _state = value;
            UpdateAnimation();
        }
    }

    public MoveDir _dir = MoveDir.None;
    public MoveDir Dir
    {
        get { return _dir; }
        set
        {
            if (_dir == value)
                return;
           
            _dir = value;

            UpdateAnimation();
        }
    }

    protected virtual void UpdateAnimation()
    {
        if(_state == CreatureState.idle)
        {
            anim.SetBool("Walking", false);
        }
        else if(_state == CreatureState.Walking)
        {
            anim.SetBool("Walking", true);
            switch (_dir)
            {
                case MoveDir.Up:
                    anim.SetFloat("DirX", 0);
                    anim.SetFloat("DirY", 1);
                    break;
                case MoveDir.Down:
                    anim.SetFloat("DirX", 0);
                    anim.SetFloat("DirY", -1);
                    break;
                case MoveDir.Left:
                    anim.SetFloat("DirX", -1);
                    anim.SetFloat("DirY", 0);
                    break;
                case MoveDir.Right:
                    anim.SetFloat("DirX", 1);
                    anim.SetFloat("DirY", 0);
                    break;
            }
        }
        else if(_state == CreatureState.Action)
        {

        }
        else if(_state == CreatureState.Dead)
        {

        }
        else
        {

        }
    }

    void Start()
    {
        Init();
    }

    void Update()
    {
        UpdateController();
    }

    protected virtual void Init()
    {
        anim = GetComponent<Animator>();

        Vector3 pos = Managers.Map.CurrentGrid.CellToWorld(CellPos) + new Vector3(0.5f, 0);
        transform.position = pos;
    }

    protected virtual void UpdateController()
    {
        UpdatePosition();
        Walking();
    }

    void UpdatePosition()
    {
        if (State != CreatureState.Walking)
            return;

        Vector3 destPos = Managers.Map.CurrentGrid.CellToWorld(CellPos) + new Vector3(0.5f, 0);
        Vector3 moveDir = destPos - transform.position;

        //도착여부 체크
        float dist = moveDir.magnitude;
        if (dist < speed * Time.deltaTime)
        {
            transform.position = destPos;
            State = CreatureState.idle;
        }
        else
        {
            transform.position += moveDir.normalized * speed * Time.deltaTime;
            State = CreatureState.Walking;
        }
    }

    void Walking()
    {
        if (State == CreatureState.idle && _dir != MoveDir.None)
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

            State = CreatureState.Walking;
            if (Managers.Map.CanGo(destPos))
            {
                if(Managers.Object.Find(destPos) == null)
                {
                    CellPos = destPos;
                }
            }
        }
    }
}
