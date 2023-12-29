using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Test : MonoBehaviour
{

    private Action<ScriptableObject> action;

	private void TestFunc(ScriptableObject obj)
    {
		CharacterPositionData data = obj as CharacterPositionData;
        for(int i = 0; i < data.CharacterCode.Length; i++) 
        {
            Debug.Log(data.CharacterCode[i]);
		}
    }

    void Start()
    {
        action = TestFunc;

		AddressableManager.Instance.Initialize();
        AddressableManager.Instance.LoadAddressableAsset("CharacterPositionData/Chapter1/CH1_0001_Test", action);
	}

    void Update()
    {
        
    }
}
