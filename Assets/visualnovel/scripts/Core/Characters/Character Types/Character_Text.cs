using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CHARACTER
{
<<<<<<< HEAD
    public class Character_Text : VisualNovelCharater
=======
    public class Character_Text : VNCharacter
>>>>>>> mergeCombat
    {
        public Character_Text(string name, CharacterConfigData config) : base(name, config, prefab : null) 
        {
            Debug.Log($"Created Text VisualNovelCharater: '{name}'");
        }
    }

}

