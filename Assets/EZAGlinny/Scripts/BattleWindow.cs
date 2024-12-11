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

public class BattleWindow : MonoBehaviour {

    private GameObject healthPotionBlocker;
    private Text healthPotionAmountText;
    private GameObject specialBlocker;
    private Text specialAmountText;

    private void Awake() {
<<<<<<< HEAD
        // healthPotionAmountText = transform.Find("healthPotionAmountText").GetComponent<Text>();
        // healthPotionBlocker = transform.Find("healthPotionBlocker").gameObject;
=======
        healthPotionAmountText = transform.Find("healthPotionAmountText").GetComponent<Text>();
        healthPotionBlocker = transform.Find("healthPotionBlocker").gameObject;
>>>>>>> mergeCombat
        
        specialAmountText = transform.Find("specialAmountText").GetComponent<Text>();
        specialBlocker = transform.Find("specialBlocker").gameObject;
    }

    private void Update() {
<<<<<<< HEAD
        // healthPotionAmountText.text = GameData.healthPotionCount.ToString();
        // healthPotionBlocker.gameObject.SetActive(GameData.healthPotionCount <= 0);
=======
        healthPotionAmountText.text = GameData.healthPotionCount.ToString();
        healthPotionBlocker.gameObject.SetActive(GameData.healthPotionCount <= 0);
>>>>>>> mergeCombat

        CharacterBattle characterBattle = BattleHandler.GetInstance().GetActiveCharacterBattle();
        specialAmountText.text = characterBattle.GetSpecial().ToString();
        specialAmountText.gameObject.SetActive(characterBattle.GetSpecial() > 0);
        specialBlocker.gameObject.SetActive(characterBattle.GetSpecial() > 0);
    }

}
