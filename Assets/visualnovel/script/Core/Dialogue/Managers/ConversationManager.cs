using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Dialogue 
{
    public class ConversationManager
    {
        private DialogueSystem dialogueSystem => DialogueSystem.instance;

        private Coroutine process = null;
        public bool isRunning => process != null;

        private TextArchitect architect = null;
        private bool userPrompt = false;

        public ConversationManager(TextArchitect architect) 
        {
            this.architect = architect;
            dialogueSystem.onUserPrompt_Next += OnUserPrompt_Next;
        }

        private void OnUserPrompt_Next()
        {
            userPrompt = true;
        }

        public void StartConversation(List<string> conversation)
        {
            StopConversation();

            process = dialogueSystem.StartCoroutine(RunningConversation(conversation));
        }
        
        public void StopConversation()
        {
            if (!isRunning)
                return;

            dialogueSystem.StopCoroutine(process);
            process = null;
        }

        IEnumerator RunningConversation(List<string> conversation)
        {
            for (int i = 0; i < conversation.Count; i++)
            {
                //Ignore blank lines
                if (string.IsNullOrWhiteSpace(conversation[i])) continue;

                DialogueLine line  = DialogueParser.Parse(conversation[i]);

                //Show dialogue
                if(line.hasDialogue)
                    yield return Line_RunDialogue(line);

                if (line.hasCommands)
                    yield return Line_RunCommands(line);

                yield return new WaitForSeconds(1);
            }
        }

        IEnumerator Line_RunDialogue(DialogueLine line)
        {
            if (line.hasSpeaker)
                dialogueSystem.ShowSpeakerName(line.speaker);
            else
                dialogueSystem.HideSpeakerName();

            yield return BuildDialogue(line.dialogue);

            //wait for user input
            yield return WaitForUserInput();

        }

        IEnumerator Line_RunCommands(DialogueLine line)
        {
            Debug.Log(line.commands);
            yield return null;
        }

        IEnumerator BuildDialogue(string dialogue)
        {
            architect.Build(dialogue);

            while (architect.isBuilding)
            {
                if (userPrompt)
                {
                    if (!architect.hurryUp)
                        architect.hurryUp = true;
                    else
                        architect.ForceComplete();


                    userPrompt = false;
                }
                yield return null;
            }
        }

        IEnumerator WaitForUserInput()
        {
            while(!userPrompt)
                yield return null;

            userPrompt = false;
        }
    }
}
