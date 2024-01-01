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
    public int def;  // ���
    public int spd;  // ���ǵ�
    public int avd;  // ȸ��
    public int cri;  // ġ��Ÿ��

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
    private float movespeed; // ĳ���� �̵��ӵ�

    public List<string> skills; // ��ų
    public List<ChangeEffect> state; // ����

    public Data curData; // ���� ��ġ
    public Data startData; // �ʱ� ��ġ

    public string code; // ĳ������ �ڵ�
    public int priority; // �켱 ����
    public bool isArrive; // ��ġ�� �����ߴ���

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
        // DB���� �� ã�� ����
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
            // �÷��̾� ���� �߰� ���� (������, PlayerPref ���� ��)
            priority = 9;
        }
        else if (code.Contains("SU"))
        {
            // ��ȯ������ ���� ����
        }
    }

    public void UpdateState()
    {
        for (int i = 0; i < state.Count; i++)
        {
            if (state[i].name.Contains("�ߵ�"))
            {
                TakeDamage(int.Parse(state[i].name.Replace("�ߵ�", string.Empty)));
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
        // �� ĭ ĳ���� ����ó��
        targetpos = pos;
        isArrive = false;
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
    public void SetPriority(int priority)
    {
        if (priority > this.priority)
            this.priority = priority;
    }
}