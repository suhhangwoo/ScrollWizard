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
	private string m_atk;
	public string Atk { get { return m_atk; } set { m_atk = value; } }
	[SerializeField]
	private string m_hp;
	public string Hp { get { return m_hp; } set { m_hp = value; } }
	[SerializeField]
	private string m_spd;
	public string Spd { get { return m_spd; } set { m_spd = value; } }
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
	private string m_skill;
	public string Skill { get { return m_skill; } set { m_skill = value; } }
	[SerializeField]
	private string m_property;
	public string Property { get { return m_property; } set { m_property = value; } }

	public void InitData()
	{
		Code = "";
		Name = "";
		Target = ETarget.None;
		Atk = "";
		Hp = "";
		Spd = "";
		Def = 0;
		Avd = 0;
		Sprite = new Sprite[7];
		Skill = "";
		Property = "";
	}
	public void ShowDebug()
	{
		Debug.Log("Code : " + Code);
		Debug.Log("Name : " + Name);
		Debug.Log("Target : " + Target);
		Debug.Log("Atk : " + Atk);
		Debug.Log("Hp : " + Hp);
		Debug.Log("Spd : " + Spd);
		Debug.Log("Def : " + Def);
		Debug.Log("Avd : " + Avd);
		for (int i = 0; i < Sprite.Length; i++)
		{
			Debug.Log("Sprite[" + i + "] : " + Sprite[i]);
		}
		Debug.Log("Skill : " + Skill);
		Debug.Log("Property : " + Property);
	}

	public void Copy(CharacterData other)
	{
		this.Code = other.Code;
		this.Name = other.Name;
		this.Target = other.Target;
		this.Atk = other.Atk;
		this.Hp = other.Hp;
		this.Spd = other.Spd;
		this.Def = other.Def;
		this.Avd = other.Avd;
		this.Sprite = (Sprite[])other.Sprite.Clone();
		this.Skill = other.Skill;
		this.Property = other.Property;
	}
}
