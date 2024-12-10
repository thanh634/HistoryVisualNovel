using CHARACTER;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class Testing_InputPanel : MonoBehaviour
{
    public InputPanel inputPanel;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Running());
    }

    // Update is called once per frame
    IEnumerator Running()
    {
        Character_Sprite linh1 = CharacterManager.instance.CreateCharacter("Linh 1 as Linh Dai Viet", revealAfterCreated: true) as Character_Sprite;
        yield return linh1.Say("Ten cua nha nguoi la gi ?");

        inputPanel.Show("ten ban la gi ?");

        while(inputPanel.isWaitingForUserInput)
            yield return null;

        string characterName = inputPanel.lastInput;

        yield return linh1.Say($"{characterName}, Cam vu khi len hay chuan bi ra tran!!");
    }
}
