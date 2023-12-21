using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEditor.AddressableAssets;
using UnityEngine.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;

public static class FileHandler
{
	public static void CreateSO(string createPath, string fileName, ScriptableObject scriptableObject)
	{
		// ������ ������ ���
		string path = $"Assets/GameData/{createPath}/{fileName}.asset";
		Debug.Log(path);

		// �־��� ��ΰ� �ùٸ��� .asset Ȯ���ڸ� ������ Ȯ��
		if (!string.IsNullOrEmpty(path) && path.StartsWith("Assets/") && path.EndsWith(".asset"))
		{
			UnityEditor.AssetDatabase.CreateAsset(scriptableObject, path); // ������ �����ϰ� ���
			AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;
			string guid = AssetDatabase.AssetPathToGUID(path); // ������ ������ GUID ȹ��
			AddressableAssetEntry assetEntry = settings.CreateOrMoveEntry(guid, settings.DefaultGroup); // Addressable�� ���� ���
			assetEntry.SetAddress(path); // ���� �ּ� ����
			UnityEditor.EditorUtility.SetDirty(scriptableObject);   // �����Ϳ��� �ش� ������ ����Ǿ��ٰ� �˸�
			UnityEditor.AssetDatabase.SaveAssets(); // ���� ������ ����
			UnityEditor.AssetDatabase.Refresh(); // ���� ������ ���ΰ�ħ
		}
		else
		{
			// �־��� ��ΰ� �ùٸ��� ������ ���� ���
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
