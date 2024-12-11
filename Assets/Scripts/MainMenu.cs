using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    private Button exit;
    private Button start;
    void Awake()
    {
        exit = GameObject.Find("btnExit").GetComponent<Button>();
        start = GameObject.Find("btnStart").GetComponent<Button>();
        exit.onClick.AddListener(ExitGame);
        start.onClick.AddListener(StartGame);
    }

    void ExitGame()
    {
        Application.Quit();
    }

    void StartGame()
    {
        Loader.Load(Loader.Scene.VisualNovel);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
