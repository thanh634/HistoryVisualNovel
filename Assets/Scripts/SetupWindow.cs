using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetupWindow : MonoBehaviour {

    private GameObject healthPotionBlocker;
    private Text healthPotionAmountText;
    private GameObject specialBlocker;
    private Text specialAmountText;

    private void Awake() {
        // healthPotionAmountText = transform.Find("healthPotionAmountText").GetComponent<Text>();
        // healthPotionBlocker = transform.Find("healthPotionBlocker").gameObject;
        
        // specialAmountText = transform.Find("specialAmountText").GetComponent<Text>();
        // specialBlocker = transform.Find("specialBlocker").gameObject;
    }

    private void Update() {
        // healthPotionAmountText.text = GameData.healthPotionCount.ToString();
        // healthPotionBlocker.gameObject.SetActive(GameData.healthPotionCount <= 0);

        CharacterBattle characterBattle = MoveHandler.GetInstance().GetActiveCharacterBattle();
        // specialAmountText.text = characterBattle.GetSpecial().ToString();
        // specialAmountText.gameObject.SetActive(characterBattle.GetSpecial() > 0);
        // specialBlocker.gameObject.SetActive(characterBattle.GetSpecial() > 0);
    }

}
