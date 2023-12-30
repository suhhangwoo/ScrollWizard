using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    private void Awake()
    {
        instance = this;
    }

    public SkillData GetSkillData(string code)
    {
        SkillData skill = FileHandler.LoadSO<SkillData>("SkillData", code);
        return skill;
    }

    public CharacterData GetCharacterData(string code)
    {
        CharacterData character = FileHandler.LoadSO<CharacterData>("CharacterData", code);
        return character;
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
