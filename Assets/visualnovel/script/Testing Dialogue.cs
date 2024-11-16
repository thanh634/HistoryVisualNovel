using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dialogue;

namespace TESTING
{
    public class TestingDialogue : MonoBehaviour
    {
        [SerializeField] private TextAsset file;
        // Start is called before the first frame update
        void Start()
        {
            StartConversation();
        }

        // Update is called once per frame
        void StartConversation()
        {
            List<string> lines = FileManager.ReadTextAsset(file,false);

            Debug.Log(lines);

            DialogueSystem.instance.Say(lines);
        }
    }
}



