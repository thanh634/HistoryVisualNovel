using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;
using UnityEditor.Profiling.Memory.Experimental;
using TMPro;

namespace CHARACTER
{
    public abstract class VisualNovelCharater
    {
        public const bool ENABLE_ON_START = false;
        private const float UNHIGHLIGHTED_DARKEN_STRENGTH = 0.65f;
        public const bool DEFAULT_ORIENTATION_IS_LEFT = false;
        public const string ANIMATION_REFRESH_TRIGGER = "Refresh";

        public string name = "";
        public string displayName = "";
        public RectTransform root = null;
        public CharacterConfigData config;
        public Animator animator;
        protected CharacterManager charManager => CharacterManager.instance;
        public DialogueSystem dialogueSystem => DialogueSystem.instance;

        public Color color { get; protected set; } = Color.white;
        protected Color displayColor => isHighlighted ? highlightedColor : unhighlightedColor;
        protected Color highlightedColor => color;
        protected Color unhighlightedColor => new Color(color.r * UNHIGHLIGHTED_DARKEN_STRENGTH, color.g * UNHIGHLIGHTED_DARKEN_STRENGTH, color.b * UNHIGHLIGHTED_DARKEN_STRENGTH, color.a);
        public bool isHighlighted { get; protected set; } = true;
        protected bool facingLeft = DEFAULT_ORIENTATION_IS_LEFT;
        public int priority { get; protected set; }

        protected Coroutine co_revealing, co_hiding;
        protected Coroutine co_moving;
        protected Coroutine co_changingColor;
        protected Coroutine co_highlighting;
        protected Coroutine co_fliping;

        public bool isRevealing => co_revealing != null;
        public bool isHiding => co_hiding != null;
        public bool isMoving => co_moving != null;
        public bool isChangingColor => co_changingColor != null;
        public bool isHighlighting => (isHighlighted && co_highlighting != null);
        public bool isUnHighlighting => (!isHighlighted && co_highlighting != null);
        public virtual bool isVisible { get; set; }
        public bool isFacingLeft => facingLeft;
        public bool isFacingRight => !facingLeft;
        public bool isFliping => co_fliping != null;

        public VisualNovelCharater(string name, CharacterConfigData config, GameObject prefab)
        { 
            this.name = name;
            displayName = name;
            this.config = config;

            if(prefab != null)
            {
                GameObject ob = Object.Instantiate(prefab, charManager.characterPanel);
                ob.name = charManager.FormatCharacterPath(charManager.characterPrefabNameFormat, name);
                ob.SetActive(true);
                root = ob.GetComponent<RectTransform>();
                animator = root.GetComponentInChildren<Animator>();
            }
            else
            {
                Debug.LogError($"Character prefab for '{name}' is null!");
            }
        }

        public Coroutine Say(string dialogue) => Say(new List<string> { dialogue });

        public Coroutine Say(List<string> dialogue)
        {
            dialogueSystem.ShowSpeakerName(displayName);
            UpdateTextCustomizationOnScreen();
            return dialogueSystem.Say(dialogue);
        }

        public void SetNameColor(Color color) => config.nameColor = color;
        public void SetDialogueColor(Color color) => config.dialogueColor = color;
        public void SetNameFont(TMP_FontAsset font) => config.nameFont = font;
        public void SetDialogueFont(TMP_FontAsset font) => config.dialogueFont = font;
        public void ResetConfigurationData() => config = CharacterManager.instance.GetCharacterConfig(name, getOriginal : true);
        public void UpdateTextCustomizationOnScreen() => dialogueSystem.ApplySpeakerDataToDialogueContainer(config);

        public virtual Coroutine Show(float speedMultiplier = 1f)
        {
            if (isRevealing)
                charManager.StopCoroutine(co_revealing);
            
            if(isHiding)
                charManager.StopCoroutine(co_hiding);

            co_revealing = charManager.StartCoroutine(ShowingOrHiding(true, speedMultiplier));

            return co_revealing;

        }

        public virtual Coroutine Hide(float speedMultiplier = 1f)
        {
            if (isHiding)
                charManager.StopCoroutine(co_hiding);

            if (isRevealing)
                charManager.StopCoroutine(co_revealing);

            co_hiding = charManager.StartCoroutine(ShowingOrHiding(false, speedMultiplier));

            return co_hiding;
        }

        public virtual IEnumerator ShowingOrHiding(bool show, float speedMultiplier = 1f)
        {
            Debug.Log("Show/Hide cannot be called from a base character type.");
            yield return null;
        }

        public virtual void SetPosition(Vector2 position)
        {
            if (root == null)
                return;

            (Vector2 minAnchorTarget, Vector2 maxAnchorTarget) = ConvertUITargetPositionToRelativeCharacterAnchorTargets(position);

            root.anchorMin = minAnchorTarget;
            root.anchorMax = maxAnchorTarget;
        }

        public virtual Coroutine MoveToPosition(Vector2 position, float speed = 2f, bool smooth = false)
        {
            if (root == null)
                return null;

            if (isMoving)
                charManager.StopCoroutine(co_moving);

            co_moving = charManager.StartCoroutine(MovingToPosition(position, speed, smooth));

            return co_moving;
        }

        private IEnumerator MovingToPosition(Vector2 position, float speed, bool smooth)
        {
            (Vector2 minAnchorTarget, Vector2 maxAnchorTarget) = ConvertUITargetPositionToRelativeCharacterAnchorTargets(position);
            Vector2 padding= root.anchorMax - root.anchorMin;

            while (root.anchorMin != minAnchorTarget || root.anchorMax != maxAnchorTarget)
            {
                root.anchorMin = smooth ?
                    Vector2.Lerp(root.anchorMin, minAnchorTarget, speed*Time.deltaTime) 
                    : Vector2.MoveTowards(root.anchorMin, minAnchorTarget, speed*Time.deltaTime * 0.35f);

                root.anchorMax = root.anchorMin + padding;

                if (smooth && Vector2.Distance(root.anchorMin, minAnchorTarget) <= 0.001f)
                {
                    root.anchorMin = minAnchorTarget;
                    root.anchorMax = maxAnchorTarget;
                    break;
                }

                yield return null;
            }

            Debug.Log("Finished Moving");
            co_moving = null;
        }

        protected (Vector2, Vector2) ConvertUITargetPositionToRelativeCharacterAnchorTargets(Vector2 position)
        {
            Vector2 padding = root.anchorMax - root.anchorMin;

            float maxX = 1f - padding.x;
            float maxY = 1f - padding.y;

            Vector2 minAnchorTarget = new Vector2(maxX * position.x, maxY * position.y);
            Vector2 maxAnchorTarget = minAnchorTarget + padding;

            return (minAnchorTarget, maxAnchorTarget);
        }

        public virtual void SetColor(Color color)
        {
            this.color = color;
        }

        public Coroutine TransitionColor(Color color,float speed = 1f)
        {
            this.color = color;

            if (isChangingColor)
                charManager.StopCoroutine(co_changingColor);

            co_changingColor = charManager.StartCoroutine(ChangingColor(displayColor, speed));

            return co_changingColor;
        }

        public virtual IEnumerator ChangingColor(Color color, float speed)
        {
            Debug.Log("Color changing is not applicable on this character type!");
            yield return null;
        }

        public Coroutine Highlight(float speed = 1f, bool immediate = false)
        {
            if (isHighlighting || isUnHighlighting)
                charManager.StopCoroutine(co_highlighting);

            isHighlighted = true;
            co_highlighting = charManager.StartCoroutine(Highlighting(speed, immediate));

            return co_highlighting;
        }

        public Coroutine UnHighlight(float speed = 1f, bool immediate = false)
        {
            if (isUnHighlighting || isHighlighting)
                charManager.StopCoroutine(co_highlighting);

            isHighlighted = false;
            co_highlighting = charManager.StartCoroutine(Highlighting(speed, immediate));

            return co_highlighting;
        }

        public virtual IEnumerator Highlighting(float speedMultiplier, bool immediate)
        {
            Debug.Log("Highlighting is not available on this character type!");
            yield return null;
        }

        public Coroutine Flip(float speed = 1, bool immediate = true)
        {
            if (isFliping)
                charManager.StopCoroutine(co_fliping);

            facingLeft = !facingLeft;
            co_fliping = charManager.StartCoroutine(FaceDirection(facingLeft, speed, immediate));

            return co_fliping;
        }

        public virtual IEnumerator FaceDirection(bool faceLeft, float speedMultiplier, bool immediate)
        {
            Debug.Log("Cannot flip a character of this type!");
            yield return null;
        }

        public void SetPriority(int priority, bool autoSortCharacterOnUI = true)
        { 
            this.priority = priority;

            if (autoSortCharacterOnUI)
                charManager.SortCharacters();
        }

        public void Animate(string animation)
        {
            animator.SetTrigger(animation);
        }
        public void Animate(string animation, bool state)
        {
            animator.SetBool(animation, state);
            animator.SetTrigger(ANIMATION_REFRESH_TRIGGER);
        }

        public virtual void OnSort(int sortingIndex)
        {
            return;
        }

        public virtual void OnReceiveCastingExpression(int layer, string expression)
        {
            return;
        }

        public enum CharacterType
        {
            Text,
            Sprite,
            SpriteSheet,
            Live2D,
            Model3D
        }

    }
}