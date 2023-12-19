using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CharacterDataViewer : EditorWindow
{
	private static CharacterData[] arrCharacterData;
	private static string[] textData;
	private static string[] textData2;
	private static string[] textData3;
	private static string[] textData4;

	private readonly static float WIDTH = 80.0f;
	private readonly static float WIDTH2 = 90.0f;
	private readonly static float SPACE = 5.0f;

	Vector2 scrollPosition;

	[MenuItem("Window/CharacterDataViewer")]
	public static void ShowWindow()
	{
		EditorWindow.GetWindow(typeof(CharacterDataViewer));
		arrCharacterData = FileHandler.LoadAllSO<CharacterData>("CharacterData");
		textData = new string[3] { "코드", "이름", "타겟"};
		textData2 = new string[3] { "공격력", "체력", "속도" };
		textData3 = new string[2] { "방어력", "회피" };
		textData4 = new string[2] { "스킬", "특성" };
	}
	private void OnGUI()
	{
		using (var scrollview = new EditorGUILayout.ScrollViewScope(scrollPosition))
		{
			scrollview.handleScrollWheel = true;
			scrollview.scrollPosition.Set(scrollPosition.x, scrollPosition.y);

			GUILayout.BeginHorizontal();
			for (int j = 0; j < textData.Length; j++)
			{
				GUILayout.Label(string.Format("{0}", textData[j]), EditorStyles.boldLabel, GUILayout.MinWidth(WIDTH), GUILayout.MaxWidth(WIDTH));
				GUILayout.Space(SPACE);
			}
			for (int j = 0; j < textData2.Length; j++)
			{
				GUILayout.Label(string.Format("{0}", textData2[j]), EditorStyles.boldLabel, GUILayout.MinWidth(WIDTH), GUILayout.MaxWidth(WIDTH));
				GUILayout.Space(SPACE);
			}
			for (int j = 0; j < textData3.Length; j++)
			{
				GUILayout.Label(string.Format("{0}", textData3[j]), EditorStyles.boldLabel, GUILayout.MinWidth(WIDTH), GUILayout.MaxWidth(WIDTH));
				GUILayout.Space(SPACE);
			}
			for (int j = 0; j < textData4.Length; j++)
			{
				GUILayout.Label(string.Format("{0}", textData4[j]), EditorStyles.boldLabel, GUILayout.MinWidth(WIDTH2), GUILayout.MaxWidth(WIDTH2));
				GUILayout.Space(SPACE);
			}
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();

			for (int i = 0; i < arrCharacterData.Length; i++)
			{
				GUILayout.BeginHorizontal();
				arrCharacterData[i].Code = EditorGUILayout.TextField(arrCharacterData[i].Code, GUILayout.MinWidth(WIDTH), GUILayout.MaxWidth(WIDTH)); GUILayout.Space(SPACE);
				arrCharacterData[i].Name = EditorGUILayout.TextField(arrCharacterData[i].Name, GUILayout.MinWidth(WIDTH), GUILayout.MaxWidth(WIDTH)); GUILayout.Space(SPACE);
				arrCharacterData[i].Target = (ETarget)EditorGUILayout.EnumPopup(arrCharacterData[i].Target, GUILayout.MinWidth(WIDTH), GUILayout.MaxWidth(WIDTH)); GUILayout.Space(SPACE);
				arrCharacterData[i].Atk = EditorGUILayout.TextField(arrCharacterData[i].Atk, GUILayout.MinWidth(WIDTH), GUILayout.MaxWidth(WIDTH)); GUILayout.Space(SPACE);
				arrCharacterData[i].Hp = EditorGUILayout.TextField(arrCharacterData[i].Hp, GUILayout.MinWidth(WIDTH), GUILayout.MaxWidth(WIDTH)); GUILayout.Space(SPACE);
				arrCharacterData[i].Spd = EditorGUILayout.TextField(arrCharacterData[i].Spd, GUILayout.MinWidth(WIDTH), GUILayout.MaxWidth(WIDTH)); GUILayout.Space(SPACE);
				arrCharacterData[i].Def = EditorGUILayout.IntField(arrCharacterData[i].Def, GUILayout.MinWidth(WIDTH), GUILayout.MaxWidth(WIDTH));
				arrCharacterData[i].Avd = EditorGUILayout.IntField(arrCharacterData[i].Avd, GUILayout.MinWidth(WIDTH), GUILayout.MaxWidth(WIDTH));
				arrCharacterData[i].Skill = EditorGUILayout.TextField(arrCharacterData[i].Skill, GUILayout.MinWidth(WIDTH2), GUILayout.MaxWidth(WIDTH2));
				arrCharacterData[i].Property = EditorGUILayout.TextField(arrCharacterData[i].Property, GUILayout.MinWidth(WIDTH2), GUILayout.MaxWidth(WIDTH2));

				GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();
			}
			scrollview.handleScrollWheel = true;
			scrollPosition.Set(scrollview.scrollPosition.x, scrollview.scrollPosition.y);
		}

	}
}
