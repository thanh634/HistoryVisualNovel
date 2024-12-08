using UnityEngine;
using CHARACTER;
using TMPro;

namespace DIALOGUE
{
    [CreateAssetMenu(fileName = "Dialogue System Configuration", menuName = "Dialogue System/Dialogue Configuration Asset")]

    public class DialogueSystemConfigSO : ScriptableObject
    {
        public CharacterConfigSO CharacterConfigurationAsset;

        public Color defaultColor = Color.black;
        public TMP_FontAsset defaultFont;

        public float dialogueFontScale = 1f;
        public float defaultNameFontSize = 50f;
        public float defaultDialogueFontSize = 35f;
    }
}