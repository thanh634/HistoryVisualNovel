using UnityEngine;
using CHARACTER;
using TMPro;

namespace Dialogue
{
    [CreateAssetMenu(fileName = "Dialogue System Configuration", menuName = "Dialogue System/Dialogue Configuration Asset")]

    public class DialogueSystemConfigSO : ScriptableObject
    {
        public CharacterConfigSO CharacterConfigurationAsset;

        public Color defaultColor = Color.black;
        public TMP_FontAsset defaultFont; 
    }
}