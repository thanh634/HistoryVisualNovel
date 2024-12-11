using DIALOGUE;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace CHARACTER
{
    public class CharacterManager : MonoBehaviour
    {
        public static CharacterManager instance { get; private set; }
<<<<<<< HEAD
        public VisualNovelCharater[] allCharacters => characters.Values.ToArray();
        private Dictionary<string, VisualNovelCharater> characters = new Dictionary<string, VisualNovelCharater>();
=======
        public VNCharacter[] allCharacters => characters.Values.ToArray();
        private Dictionary<string, VNCharacter> characters = new Dictionary<string, VNCharacter>();
>>>>>>> mergeCombat

        private CharacterConfigSO config => DialogueSystem.instance.config.CharacterConfigurationAsset;

        private const string CHARACTER_CASTING_ID = " as ";
        private const string CHARACTER_NAME_ID = "<charname>";
        public string characterRootPathFormat => $"Characters/{CHARACTER_NAME_ID}";
        public string characterPrefabNameFormat => $"VisualNovelCharater - [{CHARACTER_NAME_ID}]";
        public string characterPrefabPathFormat => $"{characterRootPathFormat}/{characterPrefabNameFormat}";

        [SerializeField] private RectTransform _characterPanel = null;
        public RectTransform characterPanel => _characterPanel;


        private void Awake()
        {
            instance = this;
        }

        public CharacterConfigData GetCharacterConfig(string name, bool getOriginal = false)
        {
            if(!getOriginal)
            {
<<<<<<< HEAD
                VisualNovelCharater character = GetCharacter(name);
=======
                VNCharacter character = GetCharacter(name);
>>>>>>> mergeCombat
                if (character != null)
                    return character.config;

            }

            return config.GetConfig(name);
        }

<<<<<<< HEAD
        public VisualNovelCharater GetCharacter(string name, bool createIfDoesNotExist = false)
=======
        public VNCharacter GetCharacter(string name, bool createIfDoesNotExist = false)
>>>>>>> mergeCombat
        {
            if(characters.ContainsKey(name.ToLower()))
                return characters[name.ToLower()];
            else if(createIfDoesNotExist)
                return CreateCharacter(name);

            return null;

        }

        public bool HasCharacter(string name) => characters.ContainsKey(name.ToLower());

<<<<<<< HEAD
        public VisualNovelCharater CreateCharacter(string characterName, bool revealAfterCreated = false)
=======
        public VNCharacter CreateCharacter(string characterName, bool revealAfterCreated = false)
>>>>>>> mergeCombat
        {
            if (characters.ContainsKey(characterName.ToLower()))
            {
                Debug.LogWarning($"A VisualNovelCharater called '{characterName}' already exists. Did not create the character.");
                return null;
            }

            CHARACTER_INFO info = GetCharInfo(characterName);

<<<<<<< HEAD
            VisualNovelCharater character = CreateCharacterFromInfo(info);
=======
            VNCharacter character = CreateCharacterFromInfo(info);
>>>>>>> mergeCombat

            characters.Add(info.characterName.ToLower(), character);
            
            if(revealAfterCreated )
                character.Show();

            return character;
        }

        private CHARACTER_INFO GetCharInfo(string characterName)
        { 
            CHARACTER_INFO result = new CHARACTER_INFO();

            string[] nameData = characterName.Split(CHARACTER_CASTING_ID, System.StringSplitOptions.RemoveEmptyEntries);

            result.characterName = nameData[0];
            result.castingName = nameData.Length > 1 ? nameData[1] : result.characterName;

            result.config = config.GetConfig(result.castingName);

            result.prefab = GetPrefabForCharacter(result.castingName);

            result.rootCharacterFolder = FormatCharacterPath(characterRootPathFormat, result.castingName);

            return result;
        }

        private GameObject GetPrefabForCharacter(string characterName)
        {
            string prefabPath = FormatCharacterPath(characterPrefabPathFormat, characterName);
            return Resources.Load<GameObject>(prefabPath);
        }

        public string FormatCharacterPath(string path, string characterName) => path.Replace(CHARACTER_NAME_ID, characterName);

<<<<<<< HEAD
        private VisualNovelCharater CreateCharacterFromInfo(CHARACTER_INFO info)
=======
        private VNCharacter CreateCharacterFromInfo(CHARACTER_INFO info)
>>>>>>> mergeCombat
        { 
            CharacterConfigData config = info.config;

            switch(config.characterType)
            {
<<<<<<< HEAD
                case VisualNovelCharater.CharacterType.Text:
                    return new Character_Text(info.characterName, config);

                case VisualNovelCharater.CharacterType.Sprite:
                case VisualNovelCharater.CharacterType.SpriteSheet:
                    return new Character_Sprite(info.characterName, config, info.prefab, info.rootCharacterFolder);

                case VisualNovelCharater.CharacterType.Live2D:
=======
                case VNCharacter.CharacterType.Text:
                    return new Character_Text(info.characterName, config);

                case VNCharacter.CharacterType.Sprite:
                case VNCharacter.CharacterType.SpriteSheet:
                    return new Character_Sprite(info.characterName, config, info.prefab, info.rootCharacterFolder);

                case VNCharacter.CharacterType.Live2D:
>>>>>>> mergeCombat
                    return new Character_Live2D(info.characterName, config, info.prefab, info.rootCharacterFolder);

                default:
                    return null;

            }
        }

        public void SortCharacters()
        {
<<<<<<< HEAD
            List<VisualNovelCharater> activeChars = characters.Values.Where(c => c.root.gameObject.activeInHierarchy && c.isVisible).ToList();
            List<VisualNovelCharater> inactiveChars = characters.Values.Except(activeChars).ToList();
=======
            List<VNCharacter> activeChars = characters.Values.Where(c => c.root.gameObject.activeInHierarchy && c.isVisible).ToList();
            List<VNCharacter> inactiveChars = characters.Values.Except(activeChars).ToList();
>>>>>>> mergeCombat

            activeChars.Sort((a, b) => a.priority.CompareTo(b.priority));
            activeChars.Concat(inactiveChars);

            SortCharacter(activeChars);
        }

        public void SortCharacters(string[] characterNames)
        {
<<<<<<< HEAD
            List<VisualNovelCharater> sortedCharacter = new List<VisualNovelCharater>();
=======
            List<VNCharacter> sortedCharacter = new List<VNCharacter>();
>>>>>>> mergeCombat

            sortedCharacter = characterNames
                .Select(name => GetCharacter(name))
                .Where(character => character != null)
                .ToList();

<<<<<<< HEAD
            List<VisualNovelCharater> remainingCharacters = characters.Values
=======
            List<VNCharacter> remainingCharacters = characters.Values
>>>>>>> mergeCombat
                .Except(sortedCharacter)
                .OrderBy(character => character.priority)
                .ToList();

            sortedCharacter.Reverse();

            int startingPriority = remainingCharacters.Count > 0 ? remainingCharacters.Max(c => c.priority) : 0;
            for (int i = 0; i < sortedCharacter.Count; i++)
            {
<<<<<<< HEAD
                VisualNovelCharater character = sortedCharacter[i];
=======
                VNCharacter character = sortedCharacter[i];
>>>>>>> mergeCombat
                character.SetPriority(startingPriority + i + 1, autoSortCharacterOnUI: false);
            }

            sortedCharacter.Reverse();
<<<<<<< HEAD
            List<VisualNovelCharater> allChars = remainingCharacters.Concat(sortedCharacter).ToList();
            SortCharacter(allChars);
        }

        private void SortCharacter(List<VisualNovelCharater> charactersSortOrder)
        {
            int i = 0;
            foreach (VisualNovelCharater character in charactersSortOrder)
=======
            List<VNCharacter> allChars = remainingCharacters.Concat(sortedCharacter).ToList();
            SortCharacter(allChars);
        }

        private void SortCharacter(List<VNCharacter> charactersSortOrder)
        {
            int i = 0;
            foreach (VNCharacter character in charactersSortOrder)
>>>>>>> mergeCombat
            {
                Debug.Log($"{character.name} priority is: {character.priority}");
                character.root.SetSiblingIndex(i++);
            }
        }

        private class CHARACTER_INFO
        { 
            public string characterName = "";
            public string castingName = "";

            public string rootCharacterFolder = "";

            public CharacterConfigData config = null;

            public GameObject prefab = null;
        }
    }
}