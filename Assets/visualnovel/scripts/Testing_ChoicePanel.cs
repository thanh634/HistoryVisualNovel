using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TESTING
{
    public class Testing_ChoicePanel : MonoBehaviour
    {
        public ChoicePanel panel;
        void Start()
        {
            panel = ChoicePanel.instance;
            StartCoroutine(Running());
        }

        IEnumerator Running()
        { 

            string[] choices = new string[]
            {
            "mot cong mot la hai",
            "hai them hai la bon",
            "bon voi mot la nam",
            "nam ngon tay sach deu"
            };

            panel.Show("Did you hear that ?", choices);

            while(panel.isWaitingOnUserChoice)
                yield return null;

            var decision = panel.lastDecision;

            Debug.Log($"Make choice {decision.answerIndex}: '{decision.choices[decision.answerIndex]}'");
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
