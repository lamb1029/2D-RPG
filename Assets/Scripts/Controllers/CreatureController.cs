using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class CreatureController : MonoBehaviour
{
    public float speed = 5.0f;

    public Vector3Int CellPos { get; set; } = Vector3Int.zero;
    protected Animator _animator;

    [SerializeField] protected bool _range;

    [SerializeField] protected CreatureState _state = CreatureState.Idle;
    public virtual CreatureState State
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

    [SerializeField] protected MoveDir _lastDir;
    [SerializeField] protected MoveDir _dir = MoveDir.None;
    public MoveDir Dir
    {
        get { return _dir; }
        set
        {
            if (_dir == value)
                return;

            _dir = value;
            if (value != MoveDir.None)
                _lastDir = value;

            UpdateAnimation();
        }
    }

    public MoveDir GetDirFromVec(Vector3Int dir)
    {
        if (dir.x > 0)
            return MoveDir.Right;
        else if (dir.x < 0)
            return MoveDir.Left;
        else if (dir.y > 0)
            return MoveDir.Up;
        else if (dir.y < 0)
            return MoveDir.Down;
        else
            return MoveDir.None;
    }

    public Vector3Int GetFrontCellPos()
    {
        Vector3Int cellPos = CellPos;

        switch (_lastDir)
        {
            case MoveDir.Up:
                cellPos += Vector3Int.up;
                break;
            case MoveDir.Down:
                cellPos += Vector3Int.down;
                break;
            case MoveDir.Left:
                cellPos += Vector3Int.left;
                break;
            case MoveDir.Right:
                cellPos += Vector3Int.right;
                break;
        }

        return cellPos;
    }

    protected virtual void UpdateAnimation()
    {
        if (_state == CreatureState.Idle)
        {
            switch (_lastDir)
            {
                case MoveDir.Up:
                    _animator.Play("Fighter_Idle_Up");
                    break;
                case MoveDir.Down:
                    _animator.Play("Fighter_Idle_Down");
                    break;
                case MoveDir.Left:
                    _animator.Play("Fighter_Idle_Left");
                    break;
                case MoveDir.Right:
                    _animator.Play("Fighter_Idle_Right");
                    break;
            }
        }
        else if (_state == CreatureState.Walking)
        {
            switch (_dir)
            {
                case MoveDir.Up:
                    _animator.Play("Fighter_Walking_Up");
                    break;
                case MoveDir.Down:
                    _animator.Play("Fighter_Walking_Down");
                    break;
                case MoveDir.Left:
                    _animator.Play("Fighter_Walking_Left");
                    break;
                case MoveDir.Right:
                    _animator.Play("Fighter_Walking_Right");
                    break;
            }
        }
        else if (_state == CreatureState.Action)
        {
            switch (_lastDir)
            {
                case MoveDir.Up:
                    _animator.Play(_range ? "Fighter_Range_Left" : "Fighter_Attack_Left");
                    break;
                case MoveDir.Down:
                    _animator.Play(_range ? "Fighter_Range_Right" : "Fighter_Attack_Right");
                    break;
                case MoveDir.Left:
                    _animator.Play(_range ? "Fighter_Range_Left" : "Fighter_Attack_Left");
                    break;
                case MoveDir.Right:
                    _animator.Play(_range ? "Fighter_Range_Right" : "Fighter_Attack_Right");
                    break;
            }
        }
        else if (_state == CreatureState.Dead)
        {
            switch (_lastDir)
            {
                case MoveDir.Up:
                    _animator.Play("Fighter_Dead_Left");
                    break;
                case MoveDir.Down:
                    _animator.Play("Fighter_Dead_Right");
                    break;
                case MoveDir.Left:
                    _animator.Play("Fighter_Dead_Left");
                    break;
                case MoveDir.Right:
                    _animator.Play("Fighter_Dead_Right");
                    break;
            }
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
        SetSorting();
    }


    protected virtual void Init()
    {
        _animator = GetComponent<Animator>();

        Vector3 pos = Managers.Map.CurrentGrid.CellToWorld(CellPos) + new Vector3(0.5f, 0);
        transform.position = pos;
    }

    protected virtual void UpdateController()
    {
        switch (State)
        {
            case CreatureState.Idle:
                UpdateIdle();
                break;
            case CreatureState.Walking:
                UpdateWalking();
                break;
            case CreatureState.Action:
                UpdateAction();
                break;
            case CreatureState.Dead:
                UpdateDead();
                break;
        }
    }

    protected virtual void UpdateIdle()
    {

    }

    protected virtual void UpdateWalking() 
    {
        Vector3 destPos = Managers.Map.CurrentGrid.CellToWorld(CellPos) + new Vector3(0.5f, 0);
        Vector3 moveDir = destPos - transform.position;

        //도착여부 체크
        float dist = moveDir.magnitude;
        if (dist < speed * Time.deltaTime)
        {
            transform.position = destPos;
            WalkToNextPos();
            //State = CreatureState.Idle;
        }
        else
        {
            transform.position += moveDir.normalized * speed * Time.deltaTime;
            State = CreatureState.Walking;
        }
    }

    protected virtual void WalkToNextPos()
    {
        if (_dir == MoveDir.None)
        {
            State = CreatureState.Idle;
            return;
        }

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
            if (Managers.Object.Find(destPos) == null)
            {
                CellPos = destPos;
            }
        }

    }

    protected virtual void UpdateAction()
    {

    }

    protected virtual void UpdateDead()
    {

    }

    public virtual void OnDamaged()
    {

    }

    protected virtual void SetSorting()
    {
        SpriteRenderer sp = gameObject.GetComponent<SpriteRenderer>();
        sp.sortingLayerName = "Creature";
        sp.sortingOrder = (int)transform.position.y * -1;
    }
}
