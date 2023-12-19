using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPos : MonoBehaviour
{
    public GameObject linkedObj;
    public Character character;

    void Awake()
    {
        linkedObj = null;
        character = null;
    }

    public void CreateCharacter(GameObject obj)
    {
        linkedObj = Instantiate(obj);
        linkedObj.transform.position = transform.position;
        character = linkedObj.GetComponent<Character>();
    }
}
