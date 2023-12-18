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
		skillData.Code = EditorGUILayout.TextField("��ų �ڵ�", skillData.Code);
		skillData.Name = EditorGUILayout.TextField("��ų �̸�", skillData.Name);
		skillData.Accuracy = EditorGUILayout.IntField("���߷�", skillData.Accuracy);
		skillData.Property = (EProperty)EditorGUILayout.EnumPopup("�Ӽ�", skillData.Property);
		GUILayout.Label("���� �� ������", EditorStyles.boldLabel);
		for (int i = 0; i < skillData.Damage.Length; i++)
		{
			skillData.Damage[i] = EditorGUILayout.IntField(string.Format("������[{0}]", i), skillData.Damage[i]);
		}
		GUILayout.Label("��Ÿ�", EditorStyles.boldLabel);
		EditorGUILayout.BeginHorizontal();
		for (int i = 0; i < skillData.Range.Length; i++)
		{
			skillData.Range[i] = EditorGUILayout.Toggle(skillData.Range[i], GUILayout.MinWidth(20), GUILayout.MaxWidth(20));
		}
		EditorGUILayout.EndHorizontal();

		skillData.Count = EditorGUILayout.IntField("��� ���� Ƚ��", skillData.Count);
		GUILayout.Label("Ű����", EditorStyles.boldLabel);
		keywordCnt = EditorGUILayout.IntSlider("Ű���� ����", keywordCnt, 0, 3);
		for (int i = 0; i < keywordCnt; i++)
		{
			keywordArr[i] = EditorGUILayout.TextField(string.Format("Ű����[{0}]", i), keywordArr[i]);
		}

		if (GUILayout.Button("��ų ����"))
		{
			for (int i = 0; i < keywordCnt; i++)
			{
				skillData.KeywordList.Add(keywordArr[i]);
			}

			SkillData newSkillData = ScriptableObject.CreateInstance<SkillData>();

			// ������ ����
			newSkillData.Copy(skillData);

			FileHandler.CreateSO("SkillData", newSkillData.Code, newSkillData);
			skillData.InitData();
			keywordCnt = 0;
			keywordArr = new string[10];
		}
	}
}
