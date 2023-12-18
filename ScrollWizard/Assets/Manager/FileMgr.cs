using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileMgr : MonoBehaviour
{
	public static SkillData GetSkillData(string Code)
	{
		SkillData skill = Resources.Load<SkillData>("SkillData/" + Code);
		return skill;
	}
	public static CharacterData GetCharacterData(string Code)
	{
		CharacterData character = Resources.Load<CharacterData>("CharacterData/" + Code);
		return character;
	}
}
