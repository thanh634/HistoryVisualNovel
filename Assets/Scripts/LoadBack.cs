using UnityEngine;

public class LoadBack : MonoBehaviour
{
    public void LoadBackToMainMenu()
    {
        // Debug.Log("Button clicked!");
        Loader.Load(Loader.Scene.MainMenu);
    }

    public void LoadBackToSetupScene()
    {
        // Debug.Log("Button clicked!");
        Loader.Load(Loader.Scene.SetupScene);
    }

    public void LoadBackToBattleScene()
    {
        // Debug.Log("Button clicked!");
        Loader.Load(Loader.Scene.BattleScene);
    }
}
