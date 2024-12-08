using CHARACTER;
using COMMANDS;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace DIALOGUE
{
    public class ConversationManager
    {
        private DialogueSystem dialogueSystem => DialogueSystem.instance;

        private Coroutine process = null;
        public bool isRunning => process != null;

        public TextArchitect architect = null;
        private bool userPrompt = false;

        private TagManager tagManager;

        public ConversationManager(TextArchitect architect) 
        {
            this.architect = architect;
            dialogueSystem.onUserPrompt_Next += OnUserPrompt_Next;

            tagManager = new TagManager();
            //logicalLineManager = new LogicalLineManager();
            //conversationQueue = new ConversationQueue();
        }

        //public void Enqueue(Conversation conversation) => conversationQueue.Enqueue(conversation);
        //public void EnqueuePriority(Conversation conversation) => conversationQueue.EnqueuePriority(conversation);

        private void OnUserPrompt_Next()
        {
            userPrompt = true;
        }

        //public Coroutine StartConversation(Conversation conversation)
        //{
        //    StopConversation();
        //    conversationQueue.Clear();

        //    Enqueue(conversation);

        //    process = dialogueSystem.StartCoroutine(RunningConversation());

        //    return process;
        //}

        public Coroutine StartConversation(List<string> conversation)
        {
            StopConversation();

            process = dialogueSystem.StartCoroutine(RunningConversation(conversation));

            return process;
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

                //wait for user input
                if(line.hasDialogue)
                {
                    yield return WaitForUserInput();

                    CommandManager.instance.stopAllProcesses();
                }
            }
        }

        IEnumerator Line_RunDialogue(DialogueLine line)
        {
            if (line.hasSpeaker)
                HandleSpeakerLogic(line.speakerData);
            //else
            //    dialogueSystem.HideSpeakerName();

            if (!dialogueSystem.dialogueContainer.isVisible)
                dialogueSystem.dialogueContainer.Show();

            //Build Dialogue
            yield return BuildLineSegments(line.dialogueData);
        }

        private void HandleSpeakerLogic(DL_SpeakerData speakerData)
        {

            bool charMustBeCreated = (speakerData.makeCharacterEnter || speakerData.isCastingPosition || speakerData.isCastingExpressions);
            Character character = CharacterManager.instance.GetCharacter(speakerData.name, createIfDoesNotExist: charMustBeCreated);

            if (speakerData.makeCharacterEnter && (!character.isVisible && !character.isRevealing))
            {
                character.Show();
            }


            dialogueSystem.ShowSpeakerName(tagManager.Inject(speakerData.displayName));

            DialogueSystem.instance.ApplySpeakerDataToDialogueContainer(speakerData.name);
            Debug.Log($"{speakerData.displayName}, {speakerData.castPosition}, {speakerData.CastExpression}");
            //Cast Position
            if (speakerData.isCastingPosition)
                character.MoveToPosition(speakerData.castPosition);

            //Cast Expression
            if(speakerData.isCastingExpressions)
            {
                foreach(var ce in speakerData.CastExpression)
                {
                    character.OnReceiveCastingExpression(ce.layer,ce.expression);
                }
            }
        }

        IEnumerator BuildLineSegments(DL_DialogueData line)
        {
            for (int i = 0; i < line.segments.Count; i++)
            { 
                DL_DialogueData.DIALOGUE_SEGMENT segment = line.segments[i];

                yield return WaitForDialogueSegmentSignalToBeTriggered(segment);

                yield return BuildDialogue(segment.dialogue, segment.appendText);
            }

        }

        public bool isWaitingOnAutoTimer { get; private set; } = false;
        IEnumerator WaitForDialogueSegmentSignalToBeTriggered(DL_DialogueData.DIALOGUE_SEGMENT segment)
        {
            switch(segment.startSignal)
            {
                case DL_DialogueData.DIALOGUE_SEGMENT.StartSignal.C:
                case DL_DialogueData.DIALOGUE_SEGMENT.StartSignal.A:
                    yield return WaitForUserInput();
                    break;
                case DL_DialogueData.DIALOGUE_SEGMENT.StartSignal.WC:
                case DL_DialogueData.DIALOGUE_SEGMENT.StartSignal.WA:
                    isWaitingOnAutoTimer = true;
                    yield return new WaitForSeconds(segment.signalDelay);
                    isWaitingOnAutoTimer = false;
                    break;
                default:
                    break;
            }
        }
        IEnumerator Line_RunCommands(DialogueLine line)
        {
            List<DL_CommandData.Command> commands = line.commandData.commands;

            foreach (DL_CommandData.Command command in commands)
            { 
                if(command.waitForCompletion || command.name == "wait")
                {
                    CoroutineWrapper cw = CommandManager.instance.Execute(command.name, command.arguments);
                    while (!cw.isDone)
                    {
                        if(userPrompt)
                        {
                            CommandManager.instance.stopCurrentProcess();
                            userPrompt = false;
                        }
                        yield return null;
                    }
                }
                else
                    CommandManager.instance.Execute(command.name,  command.arguments);
            }

            yield return null;
        }

        IEnumerator BuildDialogue(string dialogue, bool append = false)
        {
            dialogue = tagManager.Inject(dialogue);
            //Build Dialogue
            if(!append)
                architect.Build(dialogue);
            else
                architect.Append(dialogue);

            //Wait For Dialogue to complete
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
            dialogueSystem.prompt.Show();
            while(!userPrompt)
                yield return null;

            dialogueSystem.prompt.Hide();
            userPrompt = false;
        }


    }
}
