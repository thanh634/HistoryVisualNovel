using Dialogue;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TESTING
{


    public class TestingParser : MonoBehaviour
    {
        [SerializeField] private TextAsset file;
        // Start is called before the first frame update
        void Start()
        {
            sendFileToParse(file);
        }

        // Update is called once per frame
        void sendFileToParse(TextAsset filename)
        {
            List<string> lines = FileManager.ReadTextAsset(filename, false);

            foreach (string line in lines)
            {
                DialogueLine dl = DialogueParser.Parse(line);
            }
        }
    }


}