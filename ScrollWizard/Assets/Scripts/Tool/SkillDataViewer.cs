using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SkillDataViewer : EditorWindow
{
	private static SkillData[] arrSkillData;
	private static string[] textData;
	private static string[] textData2;

	private readonly static float WIDTH = 100.0f;
	private readonly static float WIDTH2 = 150.0f;
	private readonly static float SPACE = 5.0f;

	Vector2 scrollPosition;

	[MenuItem("Window/SkillDataViewer")]
	public static void ShowWindow()
	{
		EditorWindow.GetWindow(typeof(SkillDataViewer));
		arrSkillData = FileHandler.LoadAllSO<SkillData>("SkillData");
		textData = new string[6] { "�ڵ�", "�̸�", "���߷�", "�Ӽ�", "Ƚ��", "������" };
		textData2 = new string[2] { "��Ÿ�", "Ű����" };
	}
	private void OnGUI()
	{
		using (var scrollview = new EditorGUILayout.ScrollViewScope(scrollPosition))
		{
			scrollview.handleScrollWheel = true;
			scrollview.scrollPosition.Set(scrollPosition.x, scrollPosition.y);

			GUILayout.BeginHorizontal();
			for (int i = 0; i < textData.Length; i++)
			{
				GUILayout.Label(string.Format("{0}", textData[i]), EditorStyles.boldLabel,GUILayout.MinWidth(WIDTH),GUILayout.MaxWidth(WIDTH));
				GUILayout.Space(SPACE);
			}
			for (int i = 0; i < textData2.Length; i++)
			{
				GUILayout.Label(string.Format("{0}", textData2[i]), EditorStyles.boldLabel, GUILayout.MinWidth(WIDTH2), GUILayout.MaxWidth(WIDTH2));
				GUILayout.Space(SPACE);
			}
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			for (int i = 0; i < arrSkillData.Length; i++)
			{
				GUILayout.BeginHorizontal();
				arrSkillData[i].Code = EditorGUILayout.TextField(arrSkillData[i].Code, GUILayout.MinWidth(WIDTH), GUILayout.MaxWidth(WIDTH));  GUILayout.Space(SPACE);
				arrSkillData[i].Name = EditorGUILayout.TextField(arrSkillData[i].Name, GUILayout.MinWidth(WIDTH), GUILayout.MaxWidth(WIDTH)); GUILayout.Space(SPACE);
				arrSkillData[i].Accuracy = EditorGUILayout.IntField(arrSkillData[i].Accuracy, GUILayout.MinWidth(WIDTH), GUILayout.MaxWidth(WIDTH)); GUILayout.Space(SPACE);
				arrSkillData[i].Property = (EProperty)EditorGUILayout.EnumPopup(arrSkillData[i].Property, GUILayout.MinWidth(WIDTH), GUILayout.MaxWidth(WIDTH)); GUILayout.Space(SPACE);
				arrSkillData[i].Count = EditorGUILayout.IntField(arrSkillData[i].Count, GUILayout.MinWidth(WIDTH), GUILayout.MaxWidth(WIDTH)); GUILayout.Space(SPACE);
                arrSkillData[i].Damage = EditorGUILayout.TextField(arrSkillData[i].Damage, GUILayout.MinWidth(WIDTH), GUILayout.MaxWidth(WIDTH)); GUILayout.Space(SPACE);
				
				for (int j = 0; j < arrSkillData[i].Range.Length;j++)
				{
					arrSkillData[i].Range[j] = EditorGUILayout.Toggle(arrSkillData[i].Range[j], GUILayout.MinWidth(15), GUILayout.MaxWidth(15));
				}
                GUILayout.Space(SPACE);
                string keywords = "";
				for (int j = 0; j < arrSkillData[i].KeywordList.Count; j++)
				{
					keywords += ("[" + arrSkillData[i].KeywordList[j] + "] ");
				}
				GUILayout.Label(keywords, GUILayout.MinWidth(WIDTH2), GUILayout.MaxWidth(WIDTH2));
				GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();

				if (GUI.changed)
				{
					EditorUtility.SetDirty(arrSkillData[i]);
					AssetDatabase.SaveAssets();
					AssetDatabase.Refresh();
				}
			}
			scrollview.handleScrollWheel = true;
			scrollPosition.Set(scrollview.scrollPosition.x, scrollview.scrollPosition.y);
		}	
	}
}
