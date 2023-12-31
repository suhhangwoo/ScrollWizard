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
			characterData.Code = EditorGUILayout.TextField("캐릭터 코드", characterData.Code);
			characterData.Name = EditorGUILayout.TextField("캐릭터 이름", characterData.Name);
			characterData.Target = (ETarget)EditorGUILayout.EnumPopup("공격 성향", characterData.Target);
			characterData.Hp = EditorGUILayout.TextField("챕터별 체력", characterData.Hp);
			characterData.Spd = EditorGUILayout.IntField("속도", characterData.Spd);
			characterData.Def = EditorGUILayout.IntField("방어", characterData.Def);
			characterData.Avd = EditorGUILayout.IntField("회피", characterData.Avd);
			characterData.Cri = EditorGUILayout.IntField("치명타율", characterData.Cri);
			characterData.Size = EditorGUILayout.IntField("크기", characterData.Size);
			characterData.Chapter = EditorGUILayout.TextField("등장 챕터", characterData.Chapter);
			GUILayout.Label("스프라이트 (대기:0, 단일:1, 광역:2, 버프:3, 소환:4, 이동:5, 피격:6)", EditorStyles.boldLabel);
			GUILayout.BeginHorizontal();
			for (int i = 0; i < characterData.Sprite.Length; i++)
			{
				characterData.Sprite[i] =
					EditorGUILayout.ObjectField(characterData.Sprite[i],  typeof(Sprite), true, 
					GUILayout.MinWidth(70), GUILayout.MaxWidth(70), GUILayout.MinHeight(70), GUILayout.MaxHeight(70)) as Sprite;
			}
			GUILayout.EndHorizontal();
			characterData.Skill = EditorGUILayout.TextField("스킬", characterData.Skill);
			characterData.Property = EditorGUILayout.TextField("특성", characterData.Property);

			if (GUILayout.Button("캐릭터 생성"))
			{
				CharacterData newCharacterData = ScriptableObject.CreateInstance<CharacterData>();
				// 데이터 복사
				newCharacterData.Copy(characterData);
				FileHandler.CreateSO("CharacterData", newCharacterData.Code, newCharacterData);
				characterData.InitData();
			}
			scrollview.handleScrollWheel = true;
			scrollPosition.Set(scrollview.scrollPosition.x, scrollview.scrollPosition.y);
		}
	}
}
