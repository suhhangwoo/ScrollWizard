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
    public int def;  // 방어
    public int spd;  // 스피드
    public int eva;  // 회피 

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
    float movespeed; // 캐릭터 이동속도

    public List<string> skills; // 스킬
    public List<ChangeEffect> state; // 상태

    public Data curData; // 현재 수치
    public Data startData; // 초기 수치
    public string code; // 캐릭터의 코드
    public int priority; // 우선 순위

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
        // DB에서 값 찾아 저장
        this.code = code;
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = GameManager.instance.s;

        if (code == "Player")
        {
            // 플레이어 스탯 추가 저장 (아이템, PlayerPref 정보 등)
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

    public void Translucence() // 그림을 반투명하게
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