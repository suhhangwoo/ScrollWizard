using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockInfo : MonoBehaviour
{
    public GameObject linkedObj;
    public Character character;
    public SpriteRenderer spriteRenderer;
    [SerializeField]
    private int priority;

    private void Awake()
    {
        linkedObj = null;
        character = null;
    }

    public void CreateCharacter(GameObject obj)
    {
        linkedObj = Instantiate(obj);
        linkedObj.transform.position = transform.position;
        character = linkedObj.GetComponent<Character>();
        character.SetPriority(priority);
        spriteRenderer = linkedObj.GetComponent<SpriteRenderer>();
    }
}
