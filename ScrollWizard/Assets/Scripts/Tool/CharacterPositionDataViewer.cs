using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class CharacterPositionDataViewer : EditorWindow
{
	Vector2 scrollPosition;

	static private CharacterPositionData[][] Data;
	static private string[] Folder;
	[MenuItem("Window/CharacterPositionDataViewer")]
	public static void ShowWindow()
	{
		EditorWindow.GetWindow(typeof(CharacterPositionDataViewer));
		Folder = GetFolderNames("Assets/GameData/CharacterPositionData");
		Data = new CharacterPositionData[Folder.Length][];
		for (int i = 0; i < Folder.Length; i++)
		{
			//Debug.Log(Folder[i]);
			Data[i] = FileHandler.LoadAllSO<CharacterPositionData>("CharacterPositionData/" + Folder[i]);
		}
	}

	private void OnGUI()
	{
		using (var scrollview = new EditorGUILayout.ScrollViewScope(scrollPosition))
		{
			scrollview.handleScrollWheel = true;
			scrollview.scrollPosition.Set(scrollPosition.x, scrollPosition.y);

			GUILayout.Label("��ġ�ڵ�\t\t\t\t\t\t\tĳ�����ڵ�\t\t\t\t\t\t\t\t\t\t����Ȯ��");
			for (int i = 0; i < Data.Length; i++) 
			{
				GUILayout.Label(Folder[i]);
				for(int j = 0; j < Data[i].Length; j++) 
				{
					GUILayout.BeginHorizontal();
					GUILayout.Label(Data[i][j].Code);
					for (int k = 0; k < Data[i][j].CharacterCode.Length; k++)
					{
						Data[i][j].CharacterCode[k] = EditorGUILayout.TextField(Data[i][j].CharacterCode[k], GUILayout.MinWidth(70), GUILayout.MaxWidth(70));
						CharacterData[] CharacterData = FileHandler.LoadAllSO<CharacterData>("CharacterData");
						string name = "NULL";
						for(int h = 0; h < CharacterData.Length; h++)
						{
							if (CharacterData[h].Code == Data[i][j].CharacterCode[k])
							{
								name = CharacterData[h].Name;
							}
						}
						GUILayout.Label(": " + name, GUILayout.MinWidth(100), GUILayout.MaxWidth(100));
					}
					EditorGUILayout.Space(10);
					Data[i][j].Probability = EditorGUILayout.IntField(Data[i][j].Probability, GUILayout.MinWidth(75), GUILayout.MaxWidth(75));
					GUILayout.EndHorizontal();
				}
			}
			
			scrollview.handleScrollWheel = true;
			scrollPosition.Set(scrollview.scrollPosition.x, scrollview.scrollPosition.y);
		}
	}

	// ������ ��ο� �ִ� ��� ������ �̸��� �迭�� ��ȯ
	private static string[] GetFolderNames(string path)
	{
		try
		{
			// �ش� ����� ��� ������ ������
			string[] folderPaths = Directory.GetDirectories(path);

			// �� ������ �̸��� �����Ͽ� ��ȯ
			for (int i = 0; i < folderPaths.Length; i++)
			{
				folderPaths[i] = Path.GetFileName(folderPaths[i]);
			}

			return folderPaths;
		}
		catch (System.Exception e)
		{
			Debug.LogError("Error getting folder names: " + e.Message);
			return new string[0]; // ������ �߻��ϸ� �� �迭 ��ȯ
		}
	}
}
