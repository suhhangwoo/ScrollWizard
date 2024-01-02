using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEditor.AddressableAssets;
using UnityEngine.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;

public static class FileHandler
{
	public static void CreateSO(string createPath, string fileName, ScriptableObject scriptableObject)
	{
		// 생성될 에셋의 경로
		string directoryPath = $"Assets/GameData/{createPath}";
		Directory.CreateDirectory(directoryPath); //폴더가 없으면 생성
		string path = $"{directoryPath}/{fileName}.asset";
		Debug.Log(path);

		// 주어진 경로가 올바르고 .asset 확장자를 갖는지 확인
		if (!string.IsNullOrEmpty(path) && path.StartsWith("Assets/") && path.EndsWith(".asset"))
		{
			UnityEditor.AssetDatabase.CreateAsset(scriptableObject, path); // 에셋을 생성하고 등록
			AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;
            string guid = AssetDatabase.AssetPathToGUID(path); // 생성된 에셋의 GUID 획득
			AddressableAssetEntry assetEntry = null;

            if (createPath != "SkillData" && createPath != "CharacterData")
			{
				settings = AddressableAssetSettingsDefaultObject.Settings;
                string group = "PositionData_Ch" + createPath[createPath.Length - 1];
				AddressableAssetGroup assetGroup = settings.FindGroup(group);

                if (assetGroup == null)
				{
                    assetGroup = settings.CreateGroup(group, false, false, false, null, typeof(BundledAssetGroupSchema));
                }

				assetEntry = settings.CreateOrMoveEntry(guid, assetGroup);
			}
			else
			{
				assetEntry = settings.CreateOrMoveEntry(guid, settings.DefaultGroup); // Addressable로 에셋 등록
			}

			assetEntry.SetAddress(path); // 에셋 주소 설정
			UnityEditor.EditorUtility.SetDirty(scriptableObject);   // 에디터에게 해당 에셋이 변경되었다고 알림
			UnityEditor.AssetDatabase.SaveAssets(); // 에셋 데이터 저장
			UnityEditor.AssetDatabase.Refresh(); // 에셋 데이터 새로고침
		}
		else
		{
			// 주어진 경로가 올바르지 않으면 에러 출력
			Debug.LogError("Invalid asset file path: " + path);
		}
	}

	public static T LoadSO<T>(string loadPath, string fileName) where T : ScriptableObject
	{
		string path = $"Assets/GameData/{loadPath}/{fileName}.asset";
		return UnityEditor.AssetDatabase.LoadAssetAtPath<T>(path);
	}

	public static T[] LoadAllSO<T>(string folderPath) where T : ScriptableObject
	{
		string[] guids = UnityEditor.AssetDatabase.FindAssets("t:" + typeof(T).Name, new string[] { $"Assets/GameData/{folderPath}", });
		T[] objects = new T[guids.Length];

		for (int i = 0; i < guids.Length; i++)
		{
			string assetPath = UnityEditor.AssetDatabase.GUIDToAssetPath(guids[i]);
			objects[i] = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(assetPath);
			if (objects[i] == null)
			{
				Debug.LogError($"Failed to load asset at path: {assetPath}");
			}
		}

		return objects;
	}
}
