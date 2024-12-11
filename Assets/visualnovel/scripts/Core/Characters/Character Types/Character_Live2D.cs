using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CHARACTER
{
    public class Character_Live2D : VisualNovelCharater
    {
        public Character_Live2D(string name, CharacterConfigData config, GameObject prefab, string rootAssetFolder) : base(name, config, prefab)
        {
            Debug.Log($"Created Live2D VisualNovelCharater: '{name}'");
        }
    }

}

