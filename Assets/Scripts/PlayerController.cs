using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    Vector3Int cellpos = Vector3Int.zero;
    Animator anim;

    bool isWalking = false;

    MoveDir _dir = MoveDir.None;
    public MoveDir Dir
    {
        get { return _dir; }
        set
        {
            if (_dir == value)
                return;

            switch(value)
            {
                case MoveDir.Up:
                    anim.SetFloat("DirX", 0);
                    anim.SetFloat("DirY", 1);
                    anim.SetBool("Walking", true);
                    break;
                case MoveDir.Down:
                    anim.SetFloat("DirX", 0);
                    anim.SetFloat("DirY", -1);
                    anim.SetBool("Walking", true);
                    break;
                case MoveDir.Left:
                    anim.SetFloat("DirX", -1);
                    anim.SetFloat("DirY", 0);
                    anim.SetBool("Walking", true);
                    break;
                case MoveDir.Right:
                    anim.SetFloat("DirX", 1);
                    anim.SetFloat("DirY", 0);
                    anim.SetBool("Walking", true);
                    break;
                case MoveDir.None:
                    anim.SetBool("Walking", false);
                    break;
            }
            _dir = value;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();

        Vector3 pos = Managers.Map.CurrentGrid.CellToWorld(cellpos) + new Vector3(0.5f, 0);
        transform.position = pos;
    }

    // Update is called once per frame
    void Update()
    {
        GetDir();
        UpdatePosition();
        Walking();
    }

    private void LateUpdate()
    {
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
    }

    void GetDir()
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

    void UpdatePosition()
    {
        if (isWalking == false)
            return;

        Vector3 destPos = Managers.Map.CurrentGrid.CellToWorld(cellpos) + new Vector3(0.5f, 0);
        Vector3 moveDir = destPos - transform.position;

        //도착여부 체크
        float dist = moveDir.magnitude;
        if(dist < speed * Time.deltaTime)
        {
            transform.position = destPos;
            isWalking = false;
        }
        else
        {
            transform.position += moveDir.normalized * speed * Time.deltaTime;
            isWalking = true;
        }
    }

    void Walking()
    {
        if (!isWalking && _dir != MoveDir.None)
        {
            Vector3Int destPos = cellpos;
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

            if(Managers.Map.CanGo(destPos))
            {
                cellpos = destPos;
                isWalking = true;
            }
        }
    }
}
