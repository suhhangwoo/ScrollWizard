using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCutScene : MonoBehaviour
{
    public Sprite[] Left;
	public Sprite[] Right;

	public Sprite[] LeftHit;
	public Sprite[] RightHit;

    public void StartCutScene()
    {
		CutSceneController.Instance.StartCutScene(Left, Right, LeftHit, RightHit);
	}

	// Start is called before the first frame update
	void Start()
    {
        CutSceneController.Instance.Initialize();
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
