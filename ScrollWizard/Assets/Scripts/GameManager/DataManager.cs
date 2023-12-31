using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using static UnityEditor.PlayerSettings;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;

public class DataManager : Singleton<DataManager>
{
    private ScriptableObject obj;
    public ScriptableObject Obj { get { return obj; } set { obj = value; } }

    private void Start()
    {
        AddressableManager.Instance.Initialize();
    }

    public void LoadSkillData(string code)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("SkillData/");
        sb.Append(code);

        AddressableManager.Instance.LoadAddressableAsset(sb.ToString());
    }

    public void LoadCharacterData(string code)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("CharacterData/");
        sb.Append(code);

        AddressableManager.Instance.LoadAddressableAsset(sb.ToString());
    }

    public void LoadCharacterPositionData(int chapter, string code)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("CharacterPositionData/Chapter");
        sb.Append(chapter.ToString());
        sb.Append("/");
        sb.Append(code);

        AddressableManager.Instance.LoadAddressableAsset(sb.ToString());
    }

    public int[] ConvertIntArray(string input, char op)
    {
        string[] parts = input.Split(op);
        int[] numbers = new int[parts.Length];

        for (int i = 0; i < parts.Length; i++)
        {
            if (int.TryParse(parts[i], out int number))
            {
                numbers[i] = number;
            }
        }

        return numbers;
    }
}
