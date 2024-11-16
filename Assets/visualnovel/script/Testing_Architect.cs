using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dialogue;

namespace TESTING
{
    public class Testing : MonoBehaviour
    {
        DialogueSystem ds;
        TextArchitect architect;

        public TextArchitect.BuildMethod bm = TextArchitect.BuildMethod.instant;

        string[] lines = new string[10]
        {
            "zGpjxNT5QvGIpwVbyqcu",
            "tLpb0EH7k8WR15OVWyYM",
            "YucMeEOQGsL0acTQyIYJ",
            "ZORR5fi0wrIQSVN7Ccf2",
            "ns6Es6FeujlnUB6hdUQz",
            "t9GKJEMopVNU1lWDQueB",
            "LdHuaVl9PhtFlU2tABVe",
            "cmQn4Cy9H4qdDRnJGLoM",
            "Umpvzy5cjl5Jbl4fBpMc",
            "90BWg1WlK4L1BqWoUXeQ",
        };

        // Start is called before the first frame update
        void Start()
        {
            ds = DialogueSystem.instance;
            architect = new TextArchitect(ds.dialogueContainer.dialogueText);
            architect.buildMethod = TextArchitect.BuildMethod.typewriter;
            architect.speed = 0.5f;
        }

        // Update is called once per frame
        void Update()
        {
            if (bm != architect.buildMethod)
            {
                architect.buildMethod = bm;
                architect.Stop();
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                architect.Stop();
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (architect.isBuilding)
                {
                    if (!architect.hurryUp)
                    {
                        architect.hurryUp = true;
                    }
                    else
                        architect.ForceComplete();
                }
                architect.Build(lines[Random.Range(0, lines.Length)]);
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                architect.Append(lines[Random.Range(0, lines.Length)]);
            }
        }
    }

}


