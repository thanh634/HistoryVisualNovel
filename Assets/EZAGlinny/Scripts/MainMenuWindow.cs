/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;

public class MainMenuWindow : MonoBehaviour {

    private enum Sub {
        Recipe,
    }

    private Transform subMain;
    private Transform subRecipe;
    private Transform subControls;
    private Transform subRecipePlay;

    private void Awake() {
        foreach (Sub sub in System.Enum.GetValues(typeof(Sub))) {
            transform.Find("sub" + sub).GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            transform.Find("sub" + sub).GetComponent<RectTransform>().sizeDelta = Vector2.zero;
            transform.Find("sub" + sub).gameObject.SetActive(false);
        }
        
        SoundManager.Initialize();

        subRecipe = transform.Find("subRecipe");

        ShowSub(Sub.Recipe);
    }

    private void Update() {
        // press space to skip
        if (Input.GetKeyDown(KeyCode.Space)) {
            subRecipe.gameObject.SetActive(false);
        }
    }

    private void ShowSub(Sub sub) {
        subRecipe.gameObject.SetActive(false);

        switch (sub) {
        case Sub.Recipe:
            subRecipe.gameObject.SetActive(true);
            break;
        // case Sub.RecipePlay:
        //     subRecipePlay.gameObject.SetActive(true);

        //     Text recipeText = subRecipePlay.Find("recipeText").GetComponent<Text>();
        //     recipeText.text = "";
            
        //     recipeText.text += "<color=#00FF00>-</color> You have a extremely annoying small sidekick. Who will in particular randomly trip you over giggle and say don't mind me.\n    Despite having no likeable characteristics everyone in the game your character included loves this piece of trash. Suggested name Sleezer";

        //     FunctionTimer.Create(() => { 
        //         recipeText.text += "\n\n<color=#00FF00>-</color> You can only input up to 4 lowercase letters for you and your party's names, but the default presets are all clearly over that limit while also being horrible names.";
        //     }, 1.0f);
        //     FunctionTimer.Create(() => { 
        //         recipeText.text += "\n\n<color=#00FF00>-</color> Redeeming codes found under the cap of participating Mountain Dew beverages unlocks powerful armor covered in Mountain Dew branding.\n    If you abstain from the promotion, your party will occasionally comment about how good a Mountain Dew would taste about now.";
        //     }, 2.0f);
        //     FunctionTimer.Create(() => { 
        //         recipeText.text += "\n\n<color=#00FF00>-</color> There is a minor NPC early on in the game that shares the exact character model as the player.\n    This is never explained or acknowledged by anyone in the game.";
        //     }, 3.0f);
        //     FunctionTimer.Create(() => { 
        //         recipeText.text += "\n\n<color=#00FF00>-</color> Deep bass \"ooohhh nooooo!\" every time you die";
        //     }, 4.0f);
        //     FunctionTimer.Create(() => { 
        //         recipeText.text += "\n\n<color=#00FF00>-</color> The 'Interact' button to talk to an NPC is the same button as 'Use Consumable Item', which is a precious resource.";
        //     }, 5.0f);
        //     FunctionTimer.Create(() => { 
        //         recipeText.text += "\n\n<color=#00FF00>-</color> The player character sprite in the overworld doesn't look like the portrait";
        //     }, 6.0f);
        //     FunctionTimer.Create(() => { 
        //         recipeText.text += "\n\n<color=#00FF00>-</color> Not having a way to tell you how much time you've played.";
        //     }, 7.0f);
        //     FunctionTimer.Create(() => { 
        //         recipeText.text += "\n\n<color=#00FF00>-</color> After you get the main villain's HP close to zero, a cutscene plays out in which the villain overpowers your character and takes them captive.";
        //     }, 8.0f);
        //     FunctionTimer.Create(() => { 
        //         recipeText.text += "\n\n\nSLOGAN: \"Hurt me Daddy\"";
        //     }, 9.0f);
        //     FunctionTimer.Create(() => { 
        //         recipeText.text += "\nCELEBRITY LIKENESS: Ian McKellen as The Randy Old Man";
        //     }, 10.0f);
        //     break;
        // }
    }
    }
}
