using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

public class CharacterPositionDataCreator : EditorWindow
{
	private CharacterPositionData characterPosData;

	[MenuItem("Window/CharacterPositionDataCreator")]
	public static void ShowWindow()
	{
		EditorWindow.GetWindow(typeof(CharacterPositionDataCreator));
	}
	private void Awake()
	{
		characterPosData = CreateInstance<CharacterPositionData>();
		characterPosData.InitData();
	}

	private void OnGUI()
	{
		characterPosData.Code = EditorGUILayout.TextField("ĳ���� ��ġ �ڵ�", characterPosData.Code);
		GUILayout.Label("ĳ���� �ڵ�");
		EditorGUILayout.BeginHorizontal();
		for (int i = 0; i < characterPosData.CharacterCode.Length; i++)
		{
			characterPosData.CharacterCode[i] = EditorGUILayout.TextField(characterPosData.CharacterCode[i], GUILayout.MinWidth(75), GUILayout.MaxWidth(75));
		}
		EditorGUILayout.EndHorizontal();
		characterPosData.Probability = EditorGUILayout.IntField("���� Ȯ��", characterPosData.Probability);

		if (GUILayout.Button("ĳ���� ��ġ ������ ����"))
		{
			int ChapterNum = GetChapterNum(characterPosData.Code);
			if (ChapterNum == -1)
			{
				Debug.LogError("��ġ �ڵ� ����");
				return;
			}
			string path = string.Format("CharacterPositionData/Chapter{0}", ChapterNum);
			CharacterPositionData newData = ScriptableObject.CreateInstance<CharacterPositionData>();
			newData.Copy(characterPosData);
			FileHandler.CreateSO(path, newData.Code, newData);
			characterPosData.InitData();
		}
	}
	private int GetChapterNum(string input)
	{
		// ���Խ��� ����Ͽ� "CH" ������ "_"�� ������ ���ڸ� ����
		string pattern = @"CH(\d+)_";
		Match match = Regex.Match(input, pattern);
		if (match.Success)
		{
			// ���ԽĿ� ��Ī�� ���ڸ� int�� ��ȯ�Ͽ� ��ȯ
			if (int.TryParse(match.Groups[1].Value, out int result))
			{
				return result;
			}
		}
		return -1;
	}
}
