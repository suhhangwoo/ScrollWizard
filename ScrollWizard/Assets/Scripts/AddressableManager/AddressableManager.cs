using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressableManager : Singleton<AddressableManager>
{
	private static readonly string defaultAddress = "Assets/GameData/"; //���µ��� �⺻ �ּ�

	//�⺻ �ʱ�ȭ
	public void Initialize() 
	{
		Addressables.InitializeAsync();
	}

	//��巹���� ������ �ε��ϴ� �Լ�
	//��巹���� �ּҿ� �븮�ڸ� �Է��ϸ� �ش� �ּ��� ������ �ε��� �� �븮�ڸ� �ݹ����� �����Ѵ�.
	//��� ����)  AddressableManager.Instance.LoadAddressableAsset("SkillData/FI_0001", action);
	public async void LoadAddressableAsset(string address)
	{
        StringBuilder sb = new StringBuilder();
        sb.Append(defaultAddress);
        sb.Append(address);
        sb.Append(".asset");

        AsyncOperationHandle<ScriptableObject> handle = Addressables.LoadAssetAsync<ScriptableObject>(sb.ToString());

        await handle.Task;

		DataManager.Instance.Obj = handle.Result;
	}
}