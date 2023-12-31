using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressableManager : Singleton<AddressableManager>
{
	private static readonly string defaultAddress = "Assets/GameData/"; //에셋들의 기본 주소

	//기본 초기화
	public void Initialize() 
	{
		Addressables.InitializeAsync();
	}

	//어드레서블 에셋을 로드하는 함수
	//어드레서블 주소와 대리자를 입력하면 해당 주소의 에셋을 로드한 후 대리자를 콜백으로 실행한다.
	//사용 예시)  AddressableManager.Instance.LoadAddressableAsset("SkillData/FI_0001", action);
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