using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DIALOGUE;

namespace CHARACTER
{
    [System.Serializable]
    public class CharacterConfigData
    {
        public string name;
        public string alias;
        public Character.CharacterType characterType;

        public Color nameColor;
        public Color dialogueColor;

        public TMP_FontAsset nameFont;
        public TMP_FontAsset dialogueFont;

        public float nameFontSize;
        public float dialogueFontSize;

        public CharacterConfigData Copy()
        {
            CharacterConfigData result = new CharacterConfigData();

            result.name = name;
            result.alias = alias;
            result.characterType = characterType;

            result.nameColor = new Color(nameColor.r, nameColor.g, nameColor.b, nameColor.a);
            result.dialogueColor = new Color(dialogueColor.r, dialogueColor.g, dialogueColor.b, dialogueColor.a);

            result.nameFont = nameFont;
            result.dialogueFont = dialogueFont;

            result.nameFontSize = nameFontSize;
            result.dialogueFontSize = dialogueFontSize;

            return result;
        }

        private static Color defaultColor => DialogueSystem.instance.config.defaultColor;
        private static TMP_FontAsset defaultFont => DialogueSystem.instance.config.defaultFont;
        public static CharacterConfigData Default
        {
            get
            {
                CharacterConfigData result = new CharacterConfigData();

                result.name = "";
                result.alias = "";
                result.characterType = Character.CharacterType.Text;

                result.nameColor = new Color(defaultColor.r, defaultColor.g, defaultColor.b, defaultColor.a);
                result.dialogueColor = new Color(defaultColor.r, defaultColor.g, defaultColor.b, defaultColor.a);

                result.nameFont = defaultFont;
                result.dialogueFont = defaultFont;

                result.nameFontSize = DialogueSystem.instance.config.defaultNameFontSize;
                result.dialogueFontSize = DialogueSystem.instance.config.defaultDialogueFontSize;

                return result;
            }

        }
    }
}