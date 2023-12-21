using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.U2D.Animation;
using UnityEngine;

public class CharacterDataCreator : EditorWindow
{
	private CharacterData characterData;
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
			characterData.Target = (ETarget)EditorGUILayout.EnumPopup("���� ����", characterData.Target);
			characterData.Hp = EditorGUILayout.TextField("é�ͺ� ü��", characterData.Hp);
			characterData.Spd = EditorGUILayout.IntField("�ӵ�", characterData.Spd);
			characterData.Def = EditorGUILayout.IntField("���", characterData.Def);
			characterData.Avd = EditorGUILayout.IntField("ȸ��", characterData.Avd);
			characterData.Cri = EditorGUILayout.IntField("ġ��Ÿ��", characterData.Cri);
			characterData.Size = EditorGUILayout.IntField("ũ��", characterData.Size);
			characterData.Chapter = EditorGUILayout.TextField("���� é��", characterData.Chapter);
			GUILayout.Label("��������Ʈ (���:0, ����:1, ����:2, ����:3, ��ȯ:4, �̵�:5, �ǰ�:6)", EditorStyles.boldLabel);
			GUILayout.BeginHorizontal();
			for (int i = 0; i < characterData.Sprite.Length; i++)
			{
				characterData.Sprite[i] =
					EditorGUILayout.ObjectField(characterData.Sprite[i],  typeof(Sprite), true, 
					GUILayout.MinWidth(70), GUILayout.MaxWidth(70), GUILayout.MinHeight(70), GUILayout.MaxHeight(70)) as Sprite;
			}
			GUILayout.EndHorizontal();
			characterData.Skill = EditorGUILayout.TextField("��ų", characterData.Skill);
			characterData.Property = EditorGUILayout.TextField("Ư��", characterData.Property);

			if (GUILayout.Button("ĳ���� ����"))
			{
				CharacterData newCharacterData = ScriptableObject.CreateInstance<CharacterData>();
				// ������ ����
				newCharacterData.Copy(characterData);
				FileHandler.CreateSO("CharacterData", newCharacterData.Code, newCharacterData);
				characterData.InitData();
			}
			scrollview.handleScrollWheel = true;
			scrollPosition.Set(scrollview.scrollPosition.x, scrollview.scrollPosition.y);
		}
	}
}