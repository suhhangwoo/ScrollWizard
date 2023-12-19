using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ChangeEffect
{
    public string name;
    public int turn;

    public ChangeEffect(string name, int turn)
    {
        this.name = name;
        this.turn = turn;
    }
}

public struct Data
{
    public int hp;  // hp
    public int def;  // ���
    public int spd;  // ���ǵ�
    public int eva;  // ȸ�� 

    public void CopyData(Data origin)
    {
        hp = origin.hp;
        def = origin.def;
        spd = origin.spd;
        eva = origin.eva;
    }
}

public class Character : MonoBehaviour
{
    Transform targetpos;
    [SerializeField]
    float movespeed; // ĳ���� �̵��ӵ�

    public List<string> skills; // ��ų
    public List<ChangeEffect> state; // ����

    public Data curData; // ���� ��ġ
    public Data startData; // �ʱ� ��ġ
    public string code; // ĳ������ �ڵ�
    public int priority; // �켱 ����

    public bool is_arrive = true;

    void Awake()
    {
        targetpos = transform;
        movespeed = 5.0f;

        skills = new List<string>();
        state = new List<ChangeEffect>();
    }

    void Update()
    {
        if (!is_arrive)
        {
            float step = movespeed * Time.deltaTime;
            transform.position = Vector2.Lerp(transform.position, targetpos.position, step); 

            if (targetpos.position == transform.position)
            {
                is_arrive = true;
            }
        }
    }

    public void Init(string code)
    {
        // DB���� �� ã�� ����
        this.code = code;
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = GameManager.instance.s;

        if (code == "Player")
        {
            // �÷��̾� ���� �߰� ���� (������, PlayerPref ���� ��)
            priority = 9;
        }
    }

    public bool TakeDamage(int dmg)
    {
        curData.hp -= dmg;

        if (curData.hp <= 0)
            return true;

        return false;
    }

    public void Heal(int amount)
    {
        curData.hp += amount;

        if (curData.hp > startData.hp)
            curData.hp = startData.hp;
    }

    public void Move(Transform pos)
    {
        targetpos = pos;
        is_arrive = false;
    }

    public void Translucence() // �׸��� �������ϰ�
    {
        SpriteRenderer s = GetComponent<SpriteRenderer>();
        Color color;
        color = s.color;
        color.a = 0.5f;
        s.color = color;
    }

    public void AddState(string name, int turn)
    {
        state.Add(new ChangeEffect(name, turn));
    }
    public string GetState(int index)
    {
        return state[index].name;
    }
    public void AddSkill(string skillcode)
    {
        skills.Add(skillcode);
    }
    public string GetSkill(int index)
    {
        return skills[index];
    }
}