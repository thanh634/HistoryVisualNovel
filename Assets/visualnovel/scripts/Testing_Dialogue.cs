using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dialogue;

namespace TESTING
{
    public class TestingDialogue : MonoBehaviour
    {
        [SerializeField] private TextAsset file = null;
        // Start is called before the first frame update
        void Start()
        {
            StartConversation();
        }

        void StartConversation()
        {
            List<string> lines = FileManager.ReadTextAsset(file,false);
            
            /*
            foreach (string line in lines)
            {
                DialogueLine dl = DialogueParser.Parse(line);

                if(dl.hasCommands)
                    for (int i = 0; i < dl.commandData.commands.Count; i++)
                    {

                        DL_CommandData.Command command = dl.commandData.commands[i];

                        Debug.Log($"Command [{i}] '{command.name}' has arguments [{string.Join(", ", command.arguments)}]");
                    }
            }
            */

            DialogueSystem.instance.Say(lines);
        }
    }
}



