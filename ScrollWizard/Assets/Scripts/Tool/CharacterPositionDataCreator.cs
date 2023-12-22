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
		characterPosData.Code = EditorGUILayout.TextField("캐릭터 배치 코드", characterPosData.Code);
		GUILayout.Label("캐릭터 코드");
		EditorGUILayout.BeginHorizontal();
		for (int i = 0; i < characterPosData.CharacterCode.Length; i++)
		{
			characterPosData.CharacterCode[i] = EditorGUILayout.TextField(characterPosData.CharacterCode[i], GUILayout.MinWidth(75), GUILayout.MaxWidth(75));
		}
		EditorGUILayout.EndHorizontal();
		characterPosData.Probability = EditorGUILayout.IntField("등장 확률", characterPosData.Probability);

		if (GUILayout.Button("캐릭터 배치 데이터 생성"))
		{
			int ChapterNum = GetChapterNum(characterPosData.Code);
			if (ChapterNum == -1)
			{
				Debug.LogError("배치 코드 오류");
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
		// 정규식을 사용하여 "CH" 다음에 "_"가 나오는 숫자를 추출
		string pattern = @"CH(\d+)_";
		Match match = Regex.Match(input, pattern);
		if (match.Success)
		{
			// 정규식에 매칭된 숫자를 int로 변환하여 반환
			if (int.TryParse(match.Groups[1].Value, out int result))
			{
				return result;
			}
		}
		return -1;
	}
}
