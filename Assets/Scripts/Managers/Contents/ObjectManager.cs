using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager
{
    //Dictionary<int, GameObject> _objects = new Dictionary<int, GameObject>();

    List<GameObject> _object = new List<GameObject>();

    public void Add(GameObject obj)
    {
        _object.Add(obj);
    }

    public void Remove(GameObject obj)
    {
        _object.Remove(obj);
    }

    public GameObject Find(Vector3 cellPos)
    {
        foreach(GameObject obj in _object)
        {
            CreatureController cc = obj.GetComponent<CreatureController>();
            if (cc == null)
                continue;

            if (cc.CellPos == cellPos)
                return obj;
        }

        return null;
    }

    public void Clear()
    {
        _object.Clear();
    }
}
