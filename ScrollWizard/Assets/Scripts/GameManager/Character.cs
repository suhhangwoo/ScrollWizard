using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static GameManager;

public struct ChangeEffect
{
    public string name;
    public int turn;

    public ChangeEffect(string name, int turn)
    {
        this.name = name;
        this.turn = turn;
    }

    public bool UpdateEffect()
    {
        turn--;

        if (turn == 0)
            return true;

        return false;
    }
}

public struct Data
{
    public int hp;  // hp
    public int def;  // 방어
    public int spd;  // 스피드
    public int avd;  // 회피
    public int cri;  // 치명타율

    public void Init(int hp, int def, int spd, int avd, int cri)
    {
        this.hp = hp;
        this.def = def;
        this.spd = spd;
        this.avd = avd;
        this.cri = cri;
    }

    public void CopyData(Data origin)
    {
        hp = origin.hp;
        def = origin.def;
        spd = origin.spd;
        avd = origin.avd;
        cri = origin.cri;
    }
}

public class Character : MonoBehaviour
{
    private Transform targetpos;
    [SerializeField]
    private float movespeed; // 캐릭터 이동속도

    public List<string> skills; // 스킬
    public List<ChangeEffect> state; // 상태

    public Data curData; // 현재 수치
    public Data startData; // 초기 수치

    public string code; // 캐릭터의 코드
    public int priority; // 우선 순위
    public bool isArrive; // 위치에 도달했는지

    private void Awake()
    {
        targetpos = transform;
        movespeed = 5.0f;
        priority = 0;
        isArrive = true;

        skills = new List<string>();
        state = new List<ChangeEffect>();
    }

    private void Update()
    {
        if (!isArrive)
        {
            float step = movespeed * Time.deltaTime;
            transform.position = Vector2.Lerp(transform.position, targetpos.position, step); 

            if (targetpos.position == transform.position)
            {
                isArrive = true;
            }
        }
    }

    public void Init(CharacterData characterData)
    {
        // DB에서 값 찾아 저장
        code = characterData.Code;
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = characterData.Sprite[(int)SpriteKind.IDLE];

        int[] hp = DataManager.Instance.ConvertIntArray(characterData.Hp, '/');
        startData.Init(hp[PlayerPrefs.GetInt("chapter") - 1], characterData.Def, characterData.Spd, characterData.Avd, characterData.Cri);
        curData.CopyData(startData);
        string[] sk = characterData.Skill.Split('/');

        for (int i = 0; i < sk.Length; i++)
        {
            skills.Add(sk[i]);
        }

        if (code.Equals("Player"))
        {
            // 플레이어 스탯 추가 저장 (아이템, PlayerPref 정보 등)
            priority = 9;
        }
        else if (code.Contains("SU"))
        {
            // 소환수만의 스탯 저장
        }
    }

    public void UpdateState()
    {
        for (int i = 0; i < state.Count; i++)
        {
            if (state[i].name.Contains("중독"))
            {
                TakeDamage(int.Parse(state[i].name.Replace("중독", string.Empty)));
            }

            if (state[i].UpdateEffect())
            {
                state.Remove(state[i]);
                i--;
            }
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
        // 두 칸 캐릭터 예외처리
        targetpos = pos;
        isArrive = false;
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
    public void SetPriority(int priority)
    {
        if (priority > this.priority)
            this.priority = priority;
    }
}