using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TESTING
{

    public class Testing_Text : MonoBehaviour
    {

        [SerializeField] private TextAsset filename;

        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(Run());
        }

        IEnumerator Run()
        {
            List<string> lines = FileManager.ReadTextAsset(filename, false);

            foreach (string line in lines)
            {
                Debug.Log(line);
            }

            yield return null;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}