using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockInfo : MonoBehaviour
{
    public GameObject linkedObj;
    public Character character;
    public SpriteRenderer spriteRenderer;
    public int index;
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

    public void CreateCharacter(GameObject obj, GameObject block)
    {
        linkedObj = Instantiate(obj);
        Vector2 pos = transform.position;
        pos.x = (pos.x + block.transform.position.x) / 2;
        linkedObj.transform.position = pos;
        character = linkedObj.GetComponent<Character>();
        character.SetPriority(priority);
        spriteRenderer = linkedObj.GetComponent<SpriteRenderer>();
        BlockInfo blockInfo = block.GetComponent<BlockInfo>();
        blockInfo.Init(linkedObj, character, spriteRenderer);
    }

    public void Init(GameObject o, Character c, SpriteRenderer r)
    {
        linkedObj = o;
        character = c;
        spriteRenderer = r;
    }
}
