using UnityEngine;

namespace CHARACTER
{
    [CreateAssetMenu(fileName = "VisualNovelCharater Configuration Asset", menuName = "Dialogue System/VisualNovelCharater Configuration Asset") ]
    public class CharacterConfigSO : ScriptableObject
    {
        public CharacterConfigData[] characters;

        public CharacterConfigData GetConfig(string charName)
        {
            charName = charName.ToLower();

            for (int i = 0; i < characters.Length; i++)
            {
                CharacterConfigData data = characters[i];

                if (string.Equals(charName, data.name.ToLower()) || string.Equals(charName, data.alias.ToLower()))
                    return data.Copy();
            }

            return CharacterConfigData.Default;

        }
    }
}