using UnityEngine;

public class LoadBack : MonoBehaviour
{
    public void LoadBackToMainScene()
    {
        Debug.Log("Button clicked!");
        Loader.Load(Loader.Scene.BattleScene);
    }

    public void LoadBackToSetupScene()
    {
        Debug.Log("Button clicked!");
        Loader.Load(Loader.Scene.SetupScene);
    }
}
