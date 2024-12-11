using UnityEngine;

public class LoadBack : MonoBehaviour
{
<<<<<<< HEAD
    public void LoadBackToMainMenu()
    {
        // Debug.Log("Button clicked!");
        Loader.Load(Loader.Scene.MainMenu);
=======
    public void LoadBackToMainScene()
    {
        Debug.Log("Button clicked!");
        Loader.Load(Loader.Scene.BattleScene);
>>>>>>> mergeCombat
    }

    public void LoadBackToSetupScene()
    {
<<<<<<< HEAD
        // Debug.Log("Button clicked!");
        Loader.Load(Loader.Scene.SetupScene);
    }

    public void LoadBackToBattleScene()
    {
        // Debug.Log("Button clicked!");
        Loader.Load(Loader.Scene.BattleScene);
    }
=======
        Debug.Log("Button clicked!");
        Loader.Load(Loader.Scene.SetupScene);
    }
>>>>>>> mergeCombat
}
