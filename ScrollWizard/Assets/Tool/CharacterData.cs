using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using UnityEditor.U2D.Animation;
using UnityEngine;

public enum ETarget
{
    None,
    User,
	Creature
}


public class CharacterData : ScriptableObject
{
	[SerializeField]
	private string m_code;
	public string Code { get { return m_code; } set { m_code = value; } }
	[SerializeField]
	private string m_name;
	public string Name { get { return m_name; } set { m_name = value; } }
	[SerializeField]
	private ETarget m_target;
	public ETarget Target { get { return m_target; } set { m_target = value; } }
	[SerializeField]
	private int[] m_atk;
	public int[] Atk { get { return m_atk; } set { m_atk = value; } }
	[SerializeField]
	private int[] m_hp;
	public int[] Hp { get { return m_hp; } set { m_hp = value; } }
	[SerializeField]
	private int[] m_spd;
	public int[] Spd { get { return m_spd; } set { m_spd = value; } }
	[SerializeField]
	private int m_def;
	public int Def { get { return m_def; } set { m_def = value; } }
	[SerializeField]
	private int m_avd;
	public int Avd { get { return m_avd; } set { m_avd = value; } }
	[SerializeField]
	private Sprite[] m_sprite;
	public Sprite[] Sprite { get { return m_sprite; } set { m_sprite = value; } }
	[SerializeField]
	private List<string> m_skillList;
	public List<string> SkillList { get { return m_skillList; } set { m_skillList = value; } }
	[SerializeField]
	private List<string> m_propertyList;
	public List<string> PropertyList { get { return m_propertyList; } set { m_propertyList = value; } }

	public void InitData()
	{
		Code = "";
		Name = "";
		Target = ETarget.None;
		Atk = new int[3] { 0, 0, 0 };
		Hp = new int[3] { 0, 0, 0 };
		Spd = new int[3] { 0, 0, 0 };
		Def = 0;
		Avd = 0;
		Sprite = new Sprite[7];
		SkillList = new List<string>();
		PropertyList = new List<string>();
	}
	public void ShowDebug()
	{
		Debug.Log("Code : " + Code);
		Debug.Log("Name : " + Name);
		Debug.Log("Target : " + Target);
		for(int i = 0; i < 3; i++)
		{
			Debug.Log("ATK[" + i + "] : " + Atk[i] );
		}
		for (int i = 0; i < 3; i++)
		{
			Debug.Log("Hp[" + i + "] : " + Hp[i]);
		}
		for (int i = 0; i < 3; i++)
		{
			Debug.Log("Spd[" + i + "] : " + Spd[i]);
		}
		Debug.Log("Def : " + Def);
		Debug.Log("Avd : " + Avd);
		for (int i = 0; i < 7; i++)
		{
			Debug.Log("Sprite[" + i + "] : " + Sprite[i]);
		}
		for (int i = 0; i < SkillList.Count; i++)
		{
			Debug.Log("SkillList[" + i + "] : " + SkillList[i]);
		}
		for (int i = 0; i < PropertyList.Count; i++) 
		{
			Debug.Log("PropertyList[" + i + "] : " + PropertyList[i]);
		}
	}

	public void Copy(CharacterData other)
	{
		this.Code = other.Code;
		this.Name = other.Name;
		this.Target = other.Target;
		this.Atk = (int[])other.Atk.Clone();
		this.Hp = (int[])other.Hp.Clone();
		this.Spd = (int[])other.Spd.Clone();
		this.Def = other.Def;
		this.Avd = other.Avd;
		this.Sprite = (Sprite[])other.Sprite.Clone();
		this.SkillList = new List<string>(other.SkillList);
		this.PropertyList = new List<string>(other.PropertyList);
	}
}
