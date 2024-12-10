using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DIALOGUE.LogicalLines
{
    public class LL_Input : ILogicalLine
    {
        public string keyword => "input";
        public IEnumerator Execute(DialogueLine line)
        {
            string title = line.dialogueData.rawData;

            InputPanel panel = InputPanel.instance;
            panel.Show(title);
            
            while(panel.isWaitingForUserInput)
                yield return null;
        }

        public bool Matches(DialogueLine line)
        {
            return (line.hasSpeaker && line.speakerData.name.ToLower() == keyword);
        }
    }
}