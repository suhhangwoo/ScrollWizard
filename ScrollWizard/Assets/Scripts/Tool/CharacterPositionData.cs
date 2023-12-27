using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.Properties;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterPositionData : ScriptableObject
{
	[SerializeField]
	private string m_code;
	public string Code { get { return m_code; } set { m_code = value; } }
	[SerializeField]
	private string[] m_characterCode;
	public string[] CharacterCode { get { return m_characterCode; } set { m_characterCode = value; } }
	[SerializeField]
	private int m_probability;
	public int Probability { get { return m_probability; } set { m_probability = value; } }

	public void InitData()
	{
		Code = "";
		CharacterCode = new string[4];
		Probability = 0;
	}
	public void ShowDebug()
	{
		Debug.Log("Code : " + Code);
		for(int i = 0; i < CharacterCode.Length; i++) 
		{
			Debug.Log("CharacterCode[" + i.ToString() + "] : " + CharacterCode[i]);
		}
		Debug.Log("Probability : " + Probability.ToString());
	}
	public void Copy(CharacterPositionData other)
	{
		Code = other.Code;
		CharacterCode = (string[])other.CharacterCode.Clone();
		Probability = other.Probability;
	}
}
