using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.Linq;
using System;

namespace DIALOGUE.LogicalLines
{
    public class LogicalLineManager
    {
        ConversationManager conversationManager;
        private DialogueSystem dialogueSystem => DialogueSystem.instance;
        public List<ILogicalLine> logicalLines = new List<ILogicalLine>();

        public LogicalLineManager() => LoadLogicalLines();

        private void LoadLogicalLines()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Type[] lineTypes = assembly.GetTypes().Where(t=>typeof(ILogicalLine).IsAssignableFrom(t) && !t.IsInterface).ToArray();
            foreach (Type lineType in lineTypes)
            { 
                ILogicalLine line = (ILogicalLine)Activator.CreateInstance(lineType);
                logicalLines.Add(line);
            }
        }
        public bool TryGetLogic(DialogueLine line, out Coroutine logic)
        {
            foreach(var logicalLine in logicalLines)
            {
                if(logicalLine.Matches(line))
                {
                    logic = dialogueSystem.StartCoroutine(logicalLine.Execute(line));
                    return true;
                }
            }

            logic = null;
            return false;
        }
    }
}