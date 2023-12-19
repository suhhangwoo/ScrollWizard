using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class FileHandler
{
	public static void CreateSO(string createPath, string fileName, ScriptableObject scriptableObject)
	{
		string path = $"Assets/Resources/{createPath}/{fileName}.asset";
		Debug.Log(path);
		if (!string.IsNullOrEmpty(path) && path.StartsWith("Assets/Resources/") && path.EndsWith(".asset"))
		{
			UnityEditor.AssetDatabase.CreateAsset(scriptableObject, path);
			UnityEditor.EditorUtility.SetDirty(scriptableObject);
			UnityEditor.AssetDatabase.SaveAssets();
			UnityEditor.AssetDatabase.Refresh();
		}
		else
		{
			Debug.LogError("Invalid asset file path: " + path);
		}
	}

	public static T LoadSO<T>(string loadPath, string fileName) where T : ScriptableObject
	{
		string path = $"Assets/Resources/{loadPath}/{fileName}.asset";
		return UnityEditor.AssetDatabase.LoadAssetAtPath<T>(path);
	}

	public static T[] LoadAllSO<T>(string folderPath) where T : ScriptableObject
	{
		string[] guids = UnityEditor.AssetDatabase.FindAssets("t:" + typeof(T).Name, new string[] { $"Assets/Resources/{folderPath}", });
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
