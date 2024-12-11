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

    public void LoadToVS2(){
        Loader.Load(Loader.Scene.VisualNovel2);
    }

    public void LoadToVS3(){
        Loader.Load(Loader.Scene.VisualNovel3);
    }

    public void LoadToVS4(){
        Loader.Load(Loader.Scene.VisualNovel4);
    }

    public void LoadToVS5(){
        Loader.Load(Loader.Scene.VisualNovel5);
    }

    public void LoadToEnding(){
        Loader.Load(Loader.Scene.Ending);
    }
}
