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
using V_AnimationSystem;
using CodeMonkey.Utils;
using CodeMonkey.MonoBehaviours;

public class BattleHandler {

    private static BattleHandler instance;
    public static BattleHandler GetInstance() => instance;

    public static Character character;
    public static GameData.EnemyEncounter enemyEncounter;

    public static void LoadEnemyEncounter(Character character, GameData.EnemyEncounter enemyEncounter) {
        OvermapHandler.SaveAllCharacterPositions();
        BattleHandler.character = character;
        BattleHandler.enemyEncounter = enemyEncounter;
        Loader.Load(Loader.Scene.BattleScene);
    }


    private int turn;
    private CameraFollow cameraFollow;
    private List<CharacterBattle> characterBattleList;
    private CharacterBattle activeCharacterBattle;
    private CharacterBattle selectedTargetCharacterBattle;

    private LanePosition lastPlayerActiveLanePosition;
    private LanePosition lastEnemyActiveLanePosition;

    private State state;

    private int currentCharacter = 0;
    public enum LanePosition {
        Square_0_0, Square_0_1, Square_0_2, Square_0_3, Square_0_4, Square_0_5, Square_0_6, Square_0_7,
        Square_1_0, Square_1_1, Square_1_2, Square_1_3, Square_1_4, Square_1_5, Square_1_6, Square_1_7,
        Square_2_0, Square_2_1, Square_2_2, Square_2_3, Square_2_4, Square_2_5, Square_2_6, Square_2_7,
        Square_3_0, Square_3_1, Square_3_2, Square_3_3, Square_3_4, Square_3_5, Square_3_6, Square_3_7,
        Square_4_0, Square_4_1, Square_4_2, Square_4_3, Square_4_4, Square_4_5, Square_4_6, Square_4_7,
        Square_5_0, Square_5_1, Square_5_2, Square_5_3, Square_5_4, Square_5_5, Square_5_6, Square_5_7,
        Square_6_0, Square_6_1, Square_6_2, Square_6_3, Square_6_4, Square_6_5, Square_6_6, Square_6_7,
        Square_7_0, Square_7_1, Square_7_2, Square_7_3, Square_7_4, Square_7_5, Square_7_6, Square_7_7,
    }

    private enum State {
        WaitingForPlayer,
        Busy,
    }

    public enum MoveDirection
    {
        Up,
        Down,
        Left,
        Right
    }

    public BattleHandler() {
        instance = this;
        turn = 0;
        characterBattleList = new List<CharacterBattle>();
        state = State.Busy;
    }

    public static void SpawnCharacter(Character.Type characterType, BattleHandler.LanePosition lanePosition, bool isPlayerTeam, Character.Stats stats) {
        Transform characterTransform = Object.Instantiate(GameAssets.i.pfCharacterBattle, GetPosition(lanePosition, isPlayerTeam), Quaternion.identity);
        CharacterBattle characterBattle = characterTransform.GetComponent<CharacterBattle>();
        characterBattle.Setup(characterType, lanePosition, GetPosition(lanePosition, isPlayerTeam), isPlayerTeam, stats);
        instance.characterBattleList.Add(characterBattle);
    }

    public static void SpawnTexture(Sprite sprite, BattleHandler.LanePosition lanePosition, bool isPlayerTeam)
    {
        // Create a new GameObject
        GameObject textureObject = new GameObject("SpawnedTexture");

        // Add a SpriteRenderer component
        SpriteRenderer spriteRenderer = textureObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite;


        // Set the position of the GameObject
        textureObject.transform.position = GetPosition(lanePosition, isPlayerTeam);
    }

    public static Vector3 GetPosition(LanePosition lanePosition, bool isPlayerTeam)
    {
        // Extract row and column indices from the enum name
        string[] parts = lanePosition.ToString().Split('_');
        int row = int.Parse(parts[1]);
        int col = int.Parse(parts[2]);

        // Base coordinates with the multiplier for player team direction
        float playerTeamMultiplier = isPlayerTeam ? -1f : 1f;

        // Calculate the position dynamically
        float x = (col - 3.5f) * 20f;  // Columns are spaced by 20 units, centered around 0
        float y = (3.5f - row) * 10f + 5f;  // Rows are spaced by 20 units

        return new Vector3(x, y);
    }


    public static LanePosition GetNextLanePosition(LanePosition lanePosition, MoveDirection moveDirection)
    {
        int row = (int)lanePosition / 8; 
        int col = (int)lanePosition % 8;

        switch (moveDirection)
        {
            case MoveDirection.Up:
                row = (row - 1 + 8) % 8; // Wrap around using modulo
                break;

            case MoveDirection.Down:
                row = (row + 1) % 8;
                break;

            case MoveDirection.Left:
                col = (col - 1 + 8) % 8; // Wrap around using modulo
                break;

            case MoveDirection.Right:
                col = (col + 1) % 8;
                break;
        }

        return (LanePosition)(row * 8 + col); // Convert back to LanePosition
    }

    public void Start(CameraFollow cameraFollow) {
        this.cameraFollow = cameraFollow;
        cameraFollow.Setup(() => Vector3.zero, () => 50f, true, true);
        cameraFollow.SetCameraMoveSpeed(10f);
        cameraFollow.SetCameraZoomSpeed(10f);
        ResetCamera();

        for (int i = 0; i < 64; i++) {
            LanePosition lanePosition = (LanePosition)i;
            SpawnTexture(GameAssets.i.s_TankPortrait, lanePosition, true);
        }

        // Set up Battle Scene
<<<<<<< HEAD
        //heal all character
        GameData.HealAllCharacter();
=======
>>>>>>> mergeCombat
        foreach (Character character in GameData.characterList) {
            if (character.isInPlayerTeam) {
                LanePosition lanePosition;
                switch (character.type) {
                default:
                case Character.Type.Player: lanePosition = GameData.our_general_position1; break;
                case Character.Type.Tank:   lanePosition = GameData.our_general_position2;   break;
                case Character.Type.Healer: lanePosition = GameData.our_general_position3;     break;
                }
                SpawnCharacter(character.type, lanePosition, true, character.stats);
            }
        }

        GameData.EnemyEncounter enemyEncounter = BattleHandler.enemyEncounter;
        for (int i = 0; i < enemyEncounter.enemyBattleArray.Length; i++) {
            GameData.EnemyEncounter.EnemyBattle enemyBattle = enemyEncounter.enemyBattleArray[i];
            SpawnCharacter(enemyBattle.characterType, enemyBattle.lanePosition, false, new Character(enemyBattle.characterType).stats);
        }

        SetActiveCharacterBattle(GetAliveTeamCharacterBattleList(true)[0]);
        SetSelectedTargetCharacterBattle(GetAliveTeamCharacterBattleList(false)[0]);
        
        lastPlayerActiveLanePosition = GetAliveTeamCharacterBattleList(true)[0].GetLanePosition();

        LanePosition RightNearestEnemyLanePosition = GetNextLanePosition(GetAliveTeamCharacterBattleList(false)[0].GetLanePosition(), MoveDirection.Right);
        LanePosition DownNearestEnemyLanePosition = GetNextLanePosition(GetAliveTeamCharacterBattleList(false)[0].GetLanePosition(), MoveDirection.Down);

        float distanceRight = GetDistance(GetAliveTeamCharacterBattleList(true)[0].GetLanePosition(), RightNearestEnemyLanePosition);
        float distanceDown = GetDistance(GetAliveTeamCharacterBattleList(true)[0].GetLanePosition(), DownNearestEnemyLanePosition);
        
        if (distanceRight < distanceDown) {
            lastEnemyActiveLanePosition = RightNearestEnemyLanePosition;
        } else {
            lastEnemyActiveLanePosition = DownNearestEnemyLanePosition;
        }
    
        state = State.WaitingForPlayer;
    }
    private void ResetCamera() {
        cameraFollow.SetCameraFollowPosition(Vector3.zero);
        cameraFollow.SetCameraZoom(50f);
    }

    private void SetCamera(Vector3 position, float zoom) {
        cameraFollow.SetCameraFollowPosition(position);
        cameraFollow.SetCameraZoom(zoom);
    }

    private void SetActiveCharacterBattle(CharacterBattle characterBattle) {
        Debug.Log("SetActiveCharacterBattle: " + characterBattle.GetLanePosition());
        if (activeCharacterBattle != null) {
            activeCharacterBattle.HideSelectionCircle();
        }
        if (!characterBattle.IsPlayerTeam()) {
            // Enemy Active
            selectedTargetCharacterBattle.HideSelectionCircle();
        }
        activeCharacterBattle = characterBattle;
        activeCharacterBattle.ShowSelectionCircle(new Color(0, 1, 0, 1));

    }

    private void SetSelectedTargetCharacterBattle(CharacterBattle characterBattle) {
        if (selectedTargetCharacterBattle != null) {
            selectedTargetCharacterBattle.HideSelectionCircle();
        }
        selectedTargetCharacterBattle = characterBattle;
        selectedTargetCharacterBattle.ShowSelectionCircle(new Color(1, 0, 0, 1));
    }

    private float GetDistance(LanePosition src, LanePosition des) {
        // Extract row and column indices from the enum name
        string[] partsSrc = src.ToString().Split('_');
        int rowSrc = int.Parse(partsSrc[1]);
        int colSrc = int.Parse(partsSrc[2]);

        string[] partsDes = des.ToString().Split('_');
        int rowDes = int.Parse(partsDes[1]);
        int colDes = int.Parse(partsDes[2]);

        return Mathf.Sqrt((rowSrc - rowDes) * (rowSrc - rowDes) + (colSrc - colDes) * (colSrc - colDes));
    }

    private CharacterBattle GetClosestAliveInSameColumn(int row, int col, bool isPlayerTeam) {
        Debug.Log("GetClosestAliveInSameColumn: " + row + " " + col + " " + isPlayerTeam);
        if (col < 0 || col > 7) return null;
        CharacterBattle closestCharacterBattle = null;
        int closestDistance = 10;
        for (int i = 0; i < 8; i++) {
            LanePosition lanePosition = (LanePosition)(i * 8 + col);
            CharacterBattle characterBattle = GetCharacterBattleAt(lanePosition, isPlayerTeam);
            if (characterBattle != null && !characterBattle.IsDead()) {
                if (closestCharacterBattle == null) {
                    closestCharacterBattle = characterBattle;
                    closestDistance = Mathf.Abs(row - i);
                } else {
                    int distance = Mathf.Abs(row - i);
                    if (distance < closestDistance) {
                        closestCharacterBattle = characterBattle;
                        closestDistance = distance;
                    }
                }
            }
        }
        return closestCharacterBattle;
    }

    private CharacterBattle GetClosestAliveInSameRow(int row, int col, bool isPlayerTeam) {
        Debug.Log("GetClosestAliveInSameRow: " + row + " " + col + " " + isPlayerTeam);
        if (row < 0 || row > 7) return null;
        CharacterBattle closestCharacterBattle = null;
        int closestDistance = 10;
        for (int i = 0; i < 8; i++) {
            LanePosition lanePosition = (LanePosition)(row * 8 + i);
            CharacterBattle characterBattle = GetCharacterBattleAt(lanePosition, isPlayerTeam);
            if (characterBattle != null && !characterBattle.IsDead()) {
                if (closestCharacterBattle == null) {
                    closestCharacterBattle = characterBattle;
                    closestDistance = Mathf.Abs(col - i);
                } else {
                    int distance = Mathf.Abs(col - i);
                    if (distance < closestDistance) {
                        closestCharacterBattle = characterBattle;
                        closestDistance = distance;
                    }
                }
            }
        }
        return closestCharacterBattle;
    }


    private CharacterBattle GetNextCharacterBattle(LanePosition lanePosition, MoveDirection moveDir, bool isPlayerTeam) {
        List<CharacterBattle> characterBattleList = GetAliveTeamCharacterBattleList(isPlayerTeam);
        if (characterBattleList.Count == 0) {
            return null;
        }


        CharacterBattle characterBattle = null;
        do {
            switch (moveDir) 
            {
                default:
                case MoveDirection.Left:
                    characterBattle = GetClosestAliveInSameColumn((int)lanePosition / 8, (8 + ((int)lanePosition % 8) - 1) % 8, isPlayerTeam);
                    break;
                case MoveDirection.Right:
                    characterBattle = GetClosestAliveInSameColumn((int)lanePosition / 8, (((int)lanePosition % 8) + 1) % 8, isPlayerTeam);
                    break;
                case MoveDirection.Up:
                    characterBattle = GetClosestAliveInSameRow( (8 + ((int)lanePosition / 8) - 1) % 8, (int)lanePosition % 8, isPlayerTeam);
                    break;
                case MoveDirection.Down:
                    characterBattle = GetClosestAliveInSameRow( (((int)lanePosition / 8) + 1) %8, (int)lanePosition % 8, isPlayerTeam);
                    break;
            }
            lanePosition = GetNextLanePosition(lanePosition, moveDir);
        } while (characterBattle == null);

        Debug.Log("GetNextCharacterBattle: " + characterBattle.GetLanePosition());
        return characterBattle;
    }

    private CharacterBattle GetCharacterBattleAt(LanePosition lanePosition, bool isPlayerTeam) {
        foreach (CharacterBattle characterBattle in characterBattleList) {
            if (characterBattle.IsPlayerTeam() == isPlayerTeam && characterBattle.GetLanePosition() == lanePosition) {
                return characterBattle;
            }
        }
        return null;
    }

    private List<CharacterBattle> GetTeamCharacterBattleList(bool isPlayerTeam) {
        List<CharacterBattle> ret = new List<CharacterBattle>();
        foreach (CharacterBattle characterBattle in characterBattleList) {
            if (characterBattle.IsPlayerTeam() == isPlayerTeam) {
                ret.Add(characterBattle);
            }
        }
        return ret;
    }

    private List<CharacterBattle> GetAliveTeamCharacterBattleList(bool isPlayerTeam) {
        List<CharacterBattle> ret = new List<CharacterBattle>();
        foreach (CharacterBattle characterBattle in characterBattleList) {
            if (characterBattle.IsDead()) continue; // Character is Dead
            if (characterBattle.IsPlayerTeam() == isPlayerTeam) {
                ret.Add(characterBattle);
            }
        }
        return ret;
    }

    public void Update() {
        switch (state) {
        case State.WaitingForPlayer:
            // Player Turn

            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) {
                // Select Target Up
                SetSelectedTargetCharacterBattle(GetNextCharacterBattle(selectedTargetCharacterBattle.GetLanePosition(), MoveDirection.Up, false));
            }
            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) {
                // Select Target Down
                SetSelectedTargetCharacterBattle(GetNextCharacterBattle(selectedTargetCharacterBattle.GetLanePosition(), MoveDirection.Down, false));
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) {
                // Select Target Left
                SetSelectedTargetCharacterBattle(GetNextCharacterBattle(selectedTargetCharacterBattle.GetLanePosition(), MoveDirection.Left, false));
            }
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) {
                // Select Target Right
                SetSelectedTargetCharacterBattle(GetNextCharacterBattle(selectedTargetCharacterBattle.GetLanePosition(), MoveDirection.Right, false));
            }
            
            if (Input.GetKeyDown(KeyCode.Space)) {
                // Attack
                state = State.Busy;
                SetCamera(selectedTargetCharacterBattle.GetPosition() + new Vector3(-5f, 0), 30f);
                activeCharacterBattle.AttackTarget(selectedTargetCharacterBattle.GetPosition(), () => {
                    SoundManager.PlaySound(SoundManager.Sound.CharacterHit);
                    float attackMultiplier = 1f;
                    if (isHill(activeCharacterBattle.GetLanePosition()) || isMountain(activeCharacterBattle.GetLanePosition())) {
                        attackMultiplier = 0.9f;
                    }
                    if (isHill(selectedTargetCharacterBattle.GetLanePosition()) || isMountain(selectedTargetCharacterBattle.GetLanePosition())) {
                        attackMultiplier = 0.9f;
                    }

                    int damageBase = activeCharacterBattle.GetAttack(attackMultiplier);
                    int damageMin = (int)(damageBase * 0.8f);
                    int damageMax = (int)(damageBase * 1.2f);
                    selectedTargetCharacterBattle.Damage(activeCharacterBattle, UnityEngine.Random.Range(damageMin, damageMax));
                    UtilsClass.ShakeCamera(.75f, .1f);
                    if (selectedTargetCharacterBattle.IsDead()) {
                        TestEvilMonsterKilled();
                    }
                }, () => {
                    ResetCamera();
                    activeCharacterBattle.SlideBack(() => FunctionTimer.Create(ChooseNextActiveCharacter, .2f));
                });
            }
            if (Input.GetKeyDown(KeyCode.R)) {
                // Special
                if (activeCharacterBattle.TrySpendSpecial()) {
                    // Spend Special
                    switch (activeCharacterBattle.GetCharacterType()) {
                    default:
                    case Character.Type.Player:
                        state = State.Busy;
                        SetCamera(selectedTargetCharacterBattle.GetPosition() + new Vector3(-5f, 0), 30f);
                        Vector3 slideToPosition = selectedTargetCharacterBattle.GetPosition() + new Vector3(-8f, 0);
                        activeCharacterBattle.SlideToPosition(slideToPosition, () => {
                            activeCharacterBattle.PlayAnim(UnitAnim.GetUnitAnim("dSwordShield_SpartanKickRight"), (UnitAnim u) => {
                                ResetCamera();
                                activeCharacterBattle.SlideBack(() => FunctionTimer.Create(ChooseNextActiveCharacter, .2f));
                            }, (string trigger) => {
                                // Massive Single Enemy Damage
                                SoundManager.PlaySound(SoundManager.Sound.CharacterHit);
                                UtilsClass.ShakeCamera(2f, .15f);
                                int damageAmount = 100;
                                selectedTargetCharacterBattle.Damage(activeCharacterBattle, damageAmount);
                                if (selectedTargetCharacterBattle.IsDead()) {
                                    TestEvilMonsterKilled();
                                }
                            });
                        });
                        break;
                    case Character.Type.Tank:
                        state = State.Busy;
                        activeCharacterBattle.SlideToPosition(GetPosition(LanePosition.Square_3_6, false) + new Vector3(-15, 0), () => {
                            activeCharacterBattle.PlayAnim(UnitAnim.GetUnitAnim("dDagger_GroundPound"), (UnitAnim u) => {
                                activeCharacterBattle.SlideBack(() => FunctionTimer.Create(ChooseNextActiveCharacter, .2f));
                            }, (string trigger) => {
                                // Damage All Enemies
                                SoundManager.PlaySound(SoundManager.Sound.GroundPound);
                                UtilsClass.ShakeCamera(2f, .15f);
                                int damageAmount = 30;
                                List<CharacterBattle> characterBattleList = GetAliveTeamCharacterBattleList(false);
                                foreach (CharacterBattle characterBattle in characterBattleList) {
                                    characterBattle.Damage(activeCharacterBattle, damageAmount);
                                }
                                if (selectedTargetCharacterBattle.IsDead()) {
                                    TestEvilMonsterKilled();
                                }
                            });
                        });
                        break;
                    case Character.Type.Healer:
                        state = State.Busy;
                        SetCamera(activeCharacterBattle.GetPosition(), 30f);
                        activeCharacterBattle.PlayAnim(UnitAnim.GetUnitAnim("dBareHands_Victory"), (UnitAnim u) => {
                            activeCharacterBattle.PlayIdleAnim();
                            ResetCamera();
                            FunctionTimer.Create(ChooseNextActiveCharacter, .2f);
                        }, (string trigger) => {
                        });

                        FunctionTimer.Create(() => {
                            // Heal all
                            SoundManager.PlaySound(SoundManager.Sound.Heal);
                            List<CharacterBattle> characterBattleList = GetAliveTeamCharacterBattleList(true);
                            foreach (CharacterBattle characterBattle in characterBattleList) {
                                characterBattle.Heal(50);
                            }
                        }, 1.2f);
                        break;
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.T)) {
                // Heal
                if (GameData.TrySpendHealthPotion()) {
                    state = State.Busy;
                    activeCharacterBattle.Heal(100);
                    FunctionTimer.Create(ChooseNextActiveCharacter, .2f);
                }
            }
            break;
        case State.Busy:
            break;
        }
    }

    public CharacterBattle GetActiveCharacterBattle() {
        return activeCharacterBattle;
    }

    private int GetAliveCount(bool isPlayerTeam) {
        return GetAliveTeamCharacterBattleList(isPlayerTeam).Count;
    }

    private void TestEvilMonsterKilled() {
        switch (GameData.state) {
        case GameData.State.FightingEvilMonster_1:
            // Player Lost to Evil Monster
            character.isDead = true;
            GameData.GetCharacter(Character.Type.Player).stats.health = 1;
            GameData.GetCharacter(Character.Type.Tank).stats.health = 1;
            GameData.GetCharacter(Character.Type.Healer).stats.health = 1;
            GameData.state = GameData.State.LostToEvilMonster_1;

            Loader.Load(Loader.Scene.Loading);
            //OvermapHandler.LoadBackToOvermap();
            break;
        case GameData.State.FightingEvilMonster_2:
            // Player Lost to Evil Monster
            character.isDead = true;
            GameData.GetCharacter(Character.Type.Player).stats.health = 1;
            GameData.GetCharacter(Character.Type.Tank).stats.health = 1;
            GameData.GetCharacter(Character.Type.Healer).stats.health = 1;
            GameData.state = GameData.State.LostToEvilMonster_2;

            Loader.Load(Loader.Scene.Loading);
            //OvermapHandler.LoadBackToOvermap();
            break;
        case GameData.State.FightingEvilMonster_3:
            // Player Defeated Evil Monster
            character.isDead = true;
            GameData.state = GameData.State.DefeatedEvilMonster;
            
            Loader.Load(Loader.Scene.Loading);
            //OvermapHandler.LoadBackToOvermap();
            break;
        }
    }

    public bool isHill(LanePosition lanePosition) {
        if (lanePosition == LanePosition.Square_0_0 || lanePosition == LanePosition.Square_0_1 || lanePosition == LanePosition.Square_0_2 || lanePosition == LanePosition.Square_0_3 || lanePosition == LanePosition.Square_0_4 || lanePosition == LanePosition.Square_0_5 || lanePosition == LanePosition.Square_0_6 || lanePosition == LanePosition.Square_0_7) {
            return true;
        }
        return false;
    }

    public bool isMountain(LanePosition lanePosition) {
        if (lanePosition == LanePosition.Square_3_5 || lanePosition == LanePosition.Square_3_6 ) {
            return true;
        }
        return false;
    }


    private void ChooseNextActiveCharacter() {
        // Choose the next character to act
        if (GetAliveTeamCharacterBattleList(false).Count == 0) {
            // Battle Over, Player Wins!
            Debug.Log("Battle Over, Player Wins!");
            // Kill Enemy Character in Overmap
            character.isDead = true;
            // Update Player Team Values
            foreach (CharacterBattle characterBattle in GetTeamCharacterBattleList(true)) {
                if (Character.IsUniqueCharacterType(characterBattle.GetCharacterType())) {
                    // Unique Character
                    Character uniqueCharacter = GameData.GetCharacter(characterBattle.GetCharacterType());
                    if (uniqueCharacter != null) {
                        uniqueCharacter.stats.health = characterBattle.GetHealthAmount();
                        if (uniqueCharacter.isInPlayerTeam) {
                            if (uniqueCharacter.stats.health < 1) uniqueCharacter.stats.health = 1;
                        }
                    }
                }
            }
            Debug.Log("GameData.state: " + GameData.state);
            switch (GameData.state) {
            case GameData.State.Start:
                break;
            case GameData.State.FightingHurtMeDaddy:
                GameData.state = GameData.State.DefeatedHurtMeDaddy;
                break;
            case GameData.State.FightingHurtMeDaddy_2:
                GameData.state = GameData.State.DefeatedHurtMeDaddy_2;
                break;
            case GameData.State.FightingTank:
                GameData.state = GameData.State.DefeatedTank;
                // Heal Tank
                character.isDead = false;
                character.isInPlayerTeam = true;
                character.stats.health = character.stats.healthMax;
                character.subType = Character.SubType.Tank_Friendly;
                GameData.GetCharacter(Character.Type.Sleezer).subType = Character.SubType.Sleezer_Friendly;
                // Heal Player
                Character uniqueCharacter = GameData.GetCharacter(Character.Type.Player);
                uniqueCharacter.stats.health = uniqueCharacter.stats.healthMax;
                break;
            case GameData.State.FightingTavernAmbush:
                GameData.state = GameData.State.SurvivedTavernAmbush;
                break;
            case GameData.State.FightingEvilMonster_1:
                // Player Lost to Evil Monster
                GameData.GetCharacter(Character.Type.Player).stats.health = 1;
                GameData.GetCharacter(Character.Type.Tank).stats.health = 1;
                GameData.GetCharacter(Character.Type.Healer).stats.health = 1;
                GameData.state = GameData.State.LostToEvilMonster_1;
                break;
            case GameData.State.FightingEvilMonster_2:
                // Player Lost to Evil Monster
                GameData.state = GameData.State.LostToEvilMonster_2;
                break;
            case GameData.State.FightingEvilMonster_3:
                // Player Defeated Evil Monster
                GameData.state = GameData.State.DefeatedEvilMonster;
                break;
            }
            //SoundManager.PlaySound(SoundManager.Sound.BattleWin);
            FunctionTimer.Create(() => {
<<<<<<< HEAD
                Loader.Load(Loader.Scene.Victory);
=======
                Loader.Load(Loader.Scene.Win);
>>>>>>> mergeCombat
            }, .7f);
            return;
        }
        if (GetAliveTeamCharacterBattleList(true).Count == 0) {
            // Battle Over, Enemy Wins!
            Debug.Log("#### Battle Over, Enemy Wins!");
            
            foreach (CharacterBattle characterBattle in GetTeamCharacterBattleList(true)) {
                if (Character.IsUniqueCharacterType(characterBattle.GetCharacterType())) {
                    // Unique Character
                    Character uniqueCharacter = GameData.GetCharacter(characterBattle.GetCharacterType());
                    if (uniqueCharacter != null) {
                        uniqueCharacter.stats.health = characterBattle.GetHealthAmount();
                    }
                }
            }

            FunctionTimer.Create(OvermapHandler.LoadBackToOvermap, .7f);
            return;
        }

        // Choose Next Character
        if (activeCharacterBattle.IsPlayerTeam()) {
            // Next is Enemy
            List<CharacterBattle> characterBattleList = GetAliveTeamCharacterBattleList(false);

            MoveDirection moveDir = MoveDirection.Up;
            LanePosition LeftNearestEnemyLanePosition = GetNextLanePosition(activeCharacterBattle.GetLanePosition(), MoveDirection.Left);
            LanePosition UpNearestEnemyLanePosition = GetNextLanePosition(activeCharacterBattle.GetLanePosition(), MoveDirection.Up);

            float distanceLeft = GetDistance(lastEnemyActiveLanePosition, LeftNearestEnemyLanePosition);
            float distanceUp = GetDistance(lastEnemyActiveLanePosition, UpNearestEnemyLanePosition);
            
            if (distanceLeft < distanceUp) {
                moveDir = MoveDirection.Right;
            } else {
                moveDir = MoveDirection.Down;
            }

            SetActiveCharacterBattle(
                GetNextCharacterBattle(lastEnemyActiveLanePosition, moveDir, false)
            );
            lastEnemyActiveLanePosition = activeCharacterBattle.GetLanePosition();

            FunctionTimer.Create(() => {
                // Get the closest player character
                float closestDistance = 999999f;
                CharacterBattle aiTargetCharacterBattle = GetAliveTeamCharacterBattleList(true)[Random.Range(0, GetAliveTeamCharacterBattleList(true).Count)];
                for (int i = 0; i < GetAliveTeamCharacterBattleList(true).Count; i++) {
                    float distance = GetDistance(activeCharacterBattle.GetLanePosition(), GetAliveTeamCharacterBattleList(true)[i].GetLanePosition());
                    if (distance < closestDistance) {
                        closestDistance = distance;
                        aiTargetCharacterBattle = GetAliveTeamCharacterBattleList(true)[i];
                    }
                }
                
                SetCamera(aiTargetCharacterBattle.GetPosition() + new Vector3(+5f, 0), 30f);
                activeCharacterBattle.AttackTarget(aiTargetCharacterBattle.GetPosition(), () => {
                    LanePosition characterPosition = activeCharacterBattle.GetLanePosition();
                    // if position is in the hill/mountain the attack will decrease
                    float attackMultiplier = 1f;
                    if (isHill(characterPosition) || isMountain(characterPosition)) {
                        attackMultiplier = 0.9f;
                    }
                    if (isHill(selectedTargetCharacterBattle.GetLanePosition()) || isMountain(selectedTargetCharacterBattle.GetLanePosition())) {
                        attackMultiplier = 0.9f;
                    }
                    SoundManager.PlaySound(SoundManager.Sound.CharacterHit);
                    int damageBase = activeCharacterBattle.GetAttack(attackMultiplier);
                    int damageMin = (int)(damageBase * 0.8f);
                    int damageMax = (int)(damageBase * 1.2f);
                    int damageAmount = Random.Range(damageMin, damageMax);
                    int damageChance = 90;
                    if (isHill(selectedTargetCharacterBattle.GetLanePosition()) || isMountain(selectedTargetCharacterBattle.GetLanePosition())) {
                        damageChance = 70;
                    }
                    if (GetAliveCount(true) == 1) {
                        // Only one character alive on the Players Team
                        CharacterBattle lastSurvivingCharacterBattle = GetAliveTeamCharacterBattleList(true)[0];
                        if (lastSurvivingCharacterBattle.GetHealthAmount() <= damageAmount) {
                            // This hit would kill the last player character, miss
                            damageChance = 0;
                        }
                    }
                    if (Random.Range(0, 100) < damageChance) {
                        // Hit
                        aiTargetCharacterBattle.Damage(activeCharacterBattle, damageAmount);
                        UtilsClass.ShakeCamera(.75f, .1f);
                    } else {
                        // Miss
                        DamagePopup.Create(activeCharacterBattle.GetPosition(), "MISS", UtilsClass.GetColorFromString("00B4FF"));
                    }
                }, () => {
                    ResetCamera();
                    activeCharacterBattle.SlideBack(() => FunctionTimer.Create(ChooseNextActiveCharacter, .2f));
                });
            }, .3f);
        } else {
            // Next is player character
            //here
            List<CharacterBattle> characterBattleList = GetAliveTeamCharacterBattleList(true);
            //SetActiveCharacterBattle(characterBattleList[Random.Range(0, characterBattleList.Count)]);

            int nextCharacter = (currentCharacter + 1) % characterBattleList.Count;
            //Debug.Log(lastPlayerActiveLanePosition);
            SetActiveCharacterBattle(characterBattleList[nextCharacter]);
            currentCharacter = nextCharacter;
            lastPlayerActiveLanePosition = activeCharacterBattle.GetLanePosition();
            //Debug.Log("2 " + lastPlayerActiveLanePosition);

            activeCharacterBattle.TickSpecialCooldown();

            RefreshSelectedTargetCharacterBattle();
            state = State.WaitingForPlayer;
        }
    }

    private void RefreshSelectedTargetCharacterBattle() {
        if (selectedTargetCharacterBattle != null && selectedTargetCharacterBattle.IsDead()) {
            selectedTargetCharacterBattle.HideSelectionCircle();
            selectedTargetCharacterBattle = null;
        }
        if (selectedTargetCharacterBattle == null) {
            // Select next valid target
            List<CharacterBattle> characterBattleList = GetAliveTeamCharacterBattleList(false);
            if (characterBattleList.Count == 0) {
                // No more targets available
                return;
            } else {
                // There are still targets available
                CharacterBattle newTargetCharacterBattle = characterBattleList[Random.Range(0, characterBattleList.Count)];
                SetSelectedTargetCharacterBattle(newTargetCharacterBattle);
            }
        } else {
            SetSelectedTargetCharacterBattle(selectedTargetCharacterBattle);
        }
    }

}
