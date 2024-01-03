using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressableManager : Singleton<AddressableManager>
{
	private static readonly string defaultAddress = "Assets/GameData/"; //���µ��� �⺻ �ּ�
    private string[] groupName = { "PositionData_Ch1", "PositionData_Ch2", "PositionData_Ch3" };

	//�⺻ �ʱ�ȭ
	public void Initialize() 
	{
		Addressables.InitializeAsync();
	}

	//��巹���� ������ �ε��ϴ� �Լ�
	public async void LoadAddressableAsset(string address)
	{
        StringBuilder sb = new StringBuilder();
        sb.Append(defaultAddress);
        sb.Append(address);
        sb.Append(".asset");

        AsyncOperationHandle<ScriptableObject> handle = Addressables.LoadAssetAsync<ScriptableObject>(sb.ToString());

        await handle.Task;

		DataManager.Instance.obj = handle.Result;
        Addressables.Release(handle);
    }

    public async void LoadGroupAsset(int chapter)
	{
        AsyncOperationHandle<IList<ScriptableObject>> handle = Addressables.LoadAssetsAsync<ScriptableObject>(groupName[chapter - 1], null, Addressables.MergeMode.Union);
        
		await handle.Task;

        foreach (ScriptableObject go in handle.Result)
        {
            DataManager.Instance.objGroup.Add(go);
        }

        Addressables.Release(handle);
    }
}