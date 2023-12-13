using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator Animin;
    public Collider2D Coll;
    public Rigidbody2D rigid;
    public SpriteRenderer Spriter;
    public Transform targetpos;
    //����Ʈ
    public List<string> Skills; //��ų
    public List<string> Hediffs;//�����

    public float movespeed = 2.0f; //ĳ���� �̵��ӵ�
    //�Ӽ�

    public int Hp;  //hp
    public int MaxHp; //�ִ�hp
    public int Def;  //���
    public int Spd;  //���ǵ�
    public int Eva;  //ȸ�� 

    public bool is_arrive = true;

    // Start is called before the first frame update
    private void Awake()
    {
        Animin = GetComponent<Animator>();
        Coll = GetComponent<Collider2D>();
        Spriter = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        targetpos = transform;
        Skills = new List<string>();
        Hediffs = new List<string>();
    }

    void Update()
    {
        if (!is_arrive)
        {
            float step = movespeed * Time.deltaTime;
            transform.position = Vector2.Lerp(transform.position, targetpos.position, step); //õõ�� �����̴� ��  
            if (targetpos.position == transform.position)
            {
                is_arrive = true;
            }
        }
    }
    public void Addhediff(string hediffcode)
    {
        Skills.Add(hediffcode);
    }
    public void AddSkill(string skillcode)
    {
        Skills.Add(skillcode);
    }
    public string GetSkillcode(int index)
    {
        return Skills[index];//���ʹ� �����ִ¿뵵��.
    }
    public string GetHediffcode(int index)
    {
        return Hediffs[index];
    }
    public void Init(Data data)
    {
        Hp = data.Hp;
        MaxHp = data.MaxHp;
        Def = data.Def;
        Spd = data.Spd;
        Eva = data.Eva;
    }
    public void Move(Transform pos)
    {
        targetpos = pos;
        is_arrive = false;
    }
}
public class Data
{
    public int Hp;
    public int MaxHp;
    public int Def;
    public int Spd;
    public int Eva;
}

