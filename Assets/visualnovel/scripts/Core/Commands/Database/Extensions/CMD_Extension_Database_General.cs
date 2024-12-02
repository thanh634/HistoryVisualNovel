using COMMANDS;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

namespace TESTING
{
    public class CMD_Extension_Database_General : CMD_Database_Extension
    {
        new public static void Extend(CommandDatabase database)
        {
            //add command with no parameters
            database.AddCommand("print", new Action(PrintDefaultMessage));
            database.AddCommand("print_1p", new Action<string>(PrintUserMessage));
            database.AddCommand("print_mp", new Action<string[]>(PrintLines));

            //add lambda command with no parameters
            database.AddCommand("lambda", new Action(() =>
            {
                Debug.Log("Printing a default message to console from lambda command.");
            }));
            database.AddCommand("lambda_1p", new Action<string>((arg) =>
            {
                Debug.Log($"User Lambda Message '{arg}'");
            }));
            database.AddCommand("lambda_mp", new Action<string[]>((args) =>
            {
                int i = 1;
                foreach (string line in args)
                {
                    Debug.Log($"lambda {i++}. '{line}'");
                }
            }));

            //add coroutine command with no parameters
            database.AddCommand("process", new Func<IEnumerator>(SimpleProcess));
            database.AddCommand("process_1p", new Func<string, IEnumerator>(LineProcess));
            database.AddCommand("process_mp", new Func<string[], IEnumerator>(ParaProcess));

            //demo
            database.AddCommand("moveCharDemo", new Func<string, IEnumerator>(MoveCharacter));

            //general
            database.AddCommand("wait", new Func<string, IEnumerator>(Wait));
        }

        private static IEnumerator Wait(string data)
        {
            if(float.TryParse(data, out float time))
            {
                yield return new WaitForSeconds(time);
            }
        }

        private static void PrintDefaultMessage()
        {
            Debug.Log("Printing a default message to console.");
        }

        private static void PrintUserMessage(string message)
        {
            Debug.Log($"User Message '{message}'");
        }

        private static void PrintLines(string[] lines)
        {
            int i = 1;
            foreach (string line in lines)
            {
                Debug.Log($"{i++}. '{line}'");
            }
        }

        private static IEnumerator SimpleProcess()
        {
            for (int i = 0; i < 5; i++)
            {
                Debug.Log($"Process running... [{i++}]");
                yield return new WaitForSeconds(1);
            }
        }

        private static IEnumerator LineProcess(string data)
        {
            if (int.TryParse(data, out int num))
                for (int i = 0; i < num; i++)
                {
                    Debug.Log($"Process running... [{i}]");
                    yield return new WaitForSeconds(1);
                }
        }

        private static IEnumerator ParaProcess(string[] data)
        {
            foreach (string line in data)
            {
                Debug.Log($"Process: '{line}'");
                yield return new WaitForSeconds(1);
            }
        }

        private static IEnumerator MoveCharacter(string direction)
        {
            bool left = direction.ToLower() == "left";

            //Get the varriable needed that defined somewhere else

            Transform character = GameObject.Find("Vua").transform;
            float moveSpeed = 15;

            //Calculate the target position for the image
            float targetX = left ? -12 : 10.5f;

            //Calculate the current position for the image
            float currentX = character.position.x;

            //Move the image gradually toward target position
            while (Mathf.Abs(targetX - currentX) > 0.1f)
            {
                //Debug.Log($"Moving character to {(left ? "left" : "right")} [{currentX / targetX}]");
                currentX = Mathf.MoveTowards(currentX, targetX, moveSpeed * Time.deltaTime);
                character.position = new Vector3(currentX, character.position.y, character.position.z);
                yield return null;
            }
        }
    }
}