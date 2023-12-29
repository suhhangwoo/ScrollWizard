using System.Collections;
using System.Collections.Generic;
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
	public void LoadAddressableAsset(string address, System.Action<ScriptableObject> onComplete)
	{
		Addressables.LoadAssetAsync<ScriptableObject>(defaultAddress + address + ".asset").Completed += LoadAssetComplete;

		void LoadAssetComplete(AsyncOperationHandle<ScriptableObject> obj)
		{
			if (obj.Status == AsyncOperationStatus.Succeeded)
			{
				onComplete?.Invoke(obj.Result);
			}
			else
			{
				Debug.LogError("��巹���� ������ �ε��ϴ� �� �����߽��ϴ�. �ּ�: " + defaultAddress + address + ".asset");
			}
		}
	}
}
