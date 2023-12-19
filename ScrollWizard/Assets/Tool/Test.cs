using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SkillData s = FileMgr.GetSkillData("001");
        s.ShowDebug();

        CharacterData c = FileMgr.GetCharacterData("001");
        c.ShowDebug();
		//SkillData s2 = FileHandler.LoadSO<SkillData>("SkillData","004");
		//s2.ShowDebug();
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
