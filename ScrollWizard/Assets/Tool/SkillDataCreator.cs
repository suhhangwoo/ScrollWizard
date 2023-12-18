using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SkillDataCreator : EditorWindow
{
	private  SkillData skillData;
	private static int keywordCnt = 0;
	private static string[] keywordArr = new string[10];
	[MenuItem("Window/SkillDataCreator")]
	public static void ShowWindow()
	{
		EditorWindow.GetWindow(typeof(SkillDataCreator));
	}
	private void Awake()
	{
		skillData = CreateInstance<SkillData>();
		skillData.InitData();
	}
	private void OnGUI()
	{
		skillData.Code = EditorGUILayout.TextField("스킬 코드", skillData.Code);
		skillData.Name = EditorGUILayout.TextField("스킬 이름", skillData.Name);
		skillData.Accuracy = EditorGUILayout.IntField("명중률", skillData.Accuracy);
		skillData.Property = (EProperty)EditorGUILayout.EnumPopup("속성", skillData.Property);
		GUILayout.Label("레벨 별 데미지", EditorStyles.boldLabel);
		for (int i = 0; i < skillData.Damage.Length; i++)
		{
			skillData.Damage[i] = EditorGUILayout.IntField(string.Format("데미지[{0}]", i), skillData.Damage[i]);
		}
		GUILayout.Label("사거리", EditorStyles.boldLabel);
		EditorGUILayout.BeginHorizontal();
		for (int i = 0; i < skillData.Range.Length; i++)
		{
			skillData.Range[i] = EditorGUILayout.Toggle(skillData.Range[i], GUILayout.MinWidth(20), GUILayout.MaxWidth(20));
		}
		EditorGUILayout.EndHorizontal();

		skillData.Count = EditorGUILayout.IntField("사용 가능 횟수", skillData.Count);
		GUILayout.Label("키워드", EditorStyles.boldLabel);
		keywordCnt = EditorGUILayout.IntSlider("키워드 개수", keywordCnt, 0, 3);
		for (int i = 0; i < keywordCnt; i++)
		{
			keywordArr[i] = EditorGUILayout.TextField(string.Format("키워드[{0}]", i), keywordArr[i]);
		}

		if (GUILayout.Button("스킬 생성"))
		{
			for (int i = 0; i < keywordCnt; i++)
			{
				skillData.KeywordList.Add(keywordArr[i]);
			}

			SkillData newSkillData = ScriptableObject.CreateInstance<SkillData>();

			// 데이터 복사
			newSkillData.Copy(skillData);

			FileHandler.CreateSO("SkillData", newSkillData.Code, newSkillData);
			skillData.InitData();
			keywordCnt = 0;
			keywordArr = new string[10];
		}
	}
}
