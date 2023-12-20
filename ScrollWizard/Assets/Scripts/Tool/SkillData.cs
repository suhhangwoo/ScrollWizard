using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EProperty
{
	None,
	Fire,
	Ice,
	Nature
}

public class SkillData : ScriptableObject
{
	[SerializeField]
	private string m_code;
	public string Code { get { return m_code; } set { m_code = value; } }
	[SerializeField]
	private string m_name;
	public string Name { get { return m_name; } set { m_name = value; } }
	[SerializeField]
	private int m_accuracy;
	public int Accuracy { get { return m_accuracy; } set { m_accuracy = value; } }
	[SerializeField]
	private EProperty m_property;
	public EProperty Property { get { return m_property; } set { m_property = value; } }
	[SerializeField]
	private string m_damage;
	public string Damage { get { return m_damage; } set { m_damage = value; } }
	[SerializeField]
	private bool[] m_range;
	public bool[] Range { get { return m_range; } set { m_range = value; } }
	[SerializeField]
	private int m_count;
	public int Count { get { return m_count; } set { m_count = value; } }
	[SerializeField]
	private List<string> m_keywordList;
	public List<string> KeywordList { get { return m_keywordList; } set { m_keywordList = value; } }

	public void InitData()
	{
		Code = "";
		Name = "";
		Accuracy = 100;
		Property = EProperty.None;
		Damage = "";
		Range = new bool[8];
		Count = 0;
		KeywordList = new List<string>();
	}
	public void ShowDebug()
	{
		Debug.Log("Code : " + Code);
		Debug.Log("Name : " + Name);
		Debug.Log("Accuracy : " + Accuracy);
		Debug.Log("Property : " + Property);
		Debug.Log("Damage : " + Damage);

		for (int i = 0; i < 8; i++)
		{
			Debug.Log("Range[" + i + "] : " + Range[i]);
		}
		Debug.Log("Count : " + Count);
		for (int i = 0; i < KeywordList.Count; i++)
		{
			Debug.Log("KeywordList[" + i + "] : " + KeywordList[i]);
		}
	}
	public void Copy(SkillData other)
	{
		this.Code = other.Code;
		this.Name = other.Name;
		this.Accuracy = other.Accuracy;
		this.Property = other.Property;
		this.Damage = other.Damage;
		this.Range = (bool[])other.Range.Clone();
		this.KeywordList = new List<string>(other.KeywordList);
		this.Count = other.Count;
	}
}
