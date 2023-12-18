using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.U2D.Animation;
using UnityEngine;

public class CharacterDataCreator : EditorWindow
{
	private CharacterData characterData;
	private static int skillCnt = 0;
	private static string[] skillArr = new string[10];
	private static int propertyCnt = 0;
	private static string[] propertyArr = new string[10];
	Vector2 scrollPosition;

	[MenuItem("Window/CharacterDataCreator")]
	public static void ShowWindow()
	{
		EditorWindow.GetWindow(typeof(CharacterDataCreator));
	}
	private void Awake()
	{
		characterData = CreateInstance<CharacterData>();
		characterData.InitData();
	}
	private void OnGUI()
	{
		using (var scrollview = new EditorGUILayout.ScrollViewScope(scrollPosition))
		{
			scrollview.handleScrollWheel = true;
			scrollview.scrollPosition.Set(scrollPosition.x, scrollPosition.y);
			characterData.Code = EditorGUILayout.TextField("ĳ���� �ڵ�", characterData.Code);
			characterData.Name = EditorGUILayout.TextField("ĳ���� �̸�", characterData.Name);
			characterData.Target = (ETarget)EditorGUILayout.EnumPopup("Ÿ��AI", characterData.Target);
			GUILayout.Label("ATK", EditorStyles.boldLabel);
			for (int i = 0; i < characterData.Atk.Length; i++)
			{
				characterData.Atk[i] = EditorGUILayout.IntField(string.Format("é�ͺ� ATK[{0}]", i), characterData.Atk[i]);
			}
			GUILayout.Label("HP", EditorStyles.boldLabel);
			for (int i = 0; i < characterData.Hp.Length; i++)
			{
				characterData.Hp[i] = EditorGUILayout.IntField(string.Format("é�ͺ� HP[{0}]", i), characterData.Hp[i]);
			}
			GUILayout.Label("SPD", EditorStyles.boldLabel);
			for (int i = 0; i < characterData.Spd.Length; i++)
			{
				characterData.Spd[i] = EditorGUILayout.IntField(string.Format("é�ͺ� SPD[{0}]", i), characterData.Spd[i]);
			}
			characterData.Def = EditorGUILayout.IntField("DEF", characterData.Def);
			characterData.Avd = EditorGUILayout.IntField("AVD", characterData.Avd);
			GUILayout.Label("SPRITE", EditorStyles.boldLabel);
			GUILayout.BeginHorizontal();
			for (int i = 0; i < characterData.Sprite.Length; i++)
			{
				characterData.Sprite[i] =
					EditorGUILayout.ObjectField(characterData.Sprite[i],  typeof(Sprite), true, 
					GUILayout.MinWidth(70), GUILayout.MaxWidth(70), GUILayout.MinHeight(70), GUILayout.MaxHeight(70)) as Sprite;
			}
			GUILayout.EndHorizontal();
			GUILayout.Label("��ų", EditorStyles.boldLabel);
			skillCnt = EditorGUILayout.IntSlider("��ų ����", skillCnt, 0, 10);
			for (int i = 0; i < skillCnt; i++)
			{
				skillArr[i] = EditorGUILayout.TextField(string.Format("��ų[{0}]", i), skillArr[i]);
			}
			GUILayout.Label("Ư��", EditorStyles.boldLabel);
			propertyCnt = EditorGUILayout.IntSlider("Ư�� ����", propertyCnt, 0, 10);
			for (int i = 0; i < propertyCnt; i++)
			{
				propertyArr[i] = EditorGUILayout.TextField(string.Format("Ư��[{0}]", i), propertyArr[i]);
			}

			if (GUILayout.Button("ĳ���� ����"))
			{
				for (int i = 0; i < skillCnt; i++)
				{
					characterData.SkillList.Add(skillArr[i]);
				}
				for (int i = 0; i < propertyCnt; i++)
				{
					characterData.PropertyList.Add(propertyArr[i]);
				}

				CharacterData newCharacterData = ScriptableObject.CreateInstance<CharacterData>();

				// ������ ����
				newCharacterData.Copy(characterData);

				FileHandler.CreateSO("CharacterData", newCharacterData.Code, newCharacterData);
				characterData.InitData();
				skillCnt = 0;
				skillArr = new string[10];
				propertyCnt = 0;
				propertyArr = new string[10];
			}
			scrollview.handleScrollWheel = true;
			scrollPosition.Set(scrollview.scrollPosition.x, scrollview.scrollPosition.y);
		}
	}
}
