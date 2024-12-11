using System.Collections;
using System.Collections.Generic;
using UnityEngine;
<<<<<<< HEAD
using UnityEngine.Events;
using UnityEngine.UI;
=======
>>>>>>> mergeCombat
using V_AnimationSystem;
using CodeMonkey.Utils;
using CodeMonkey.MonoBehaviours;

public class MoveHandler {

    private static MoveHandler instance;
    public static MoveHandler GetInstance() => instance;

    public static Character character;
    public static GameData.EnemyEncounter enemyEncounter;

    public static void LoadEnemyEncounter(Character character, GameData.EnemyEncounter enemyEncounter) {
        OvermapHandler.SaveAllCharacterPositions();
        MoveHandler.character = character;
        MoveHandler.enemyEncounter = enemyEncounter;
        Loader.Load(Loader.Scene.SetupScene);
    }


    private int turn;
    private CameraFollow cameraFollow;
    private List<CharacterBattle> characterBattleList;
    private CharacterBattle activeCharacterBattle;
    private CharacterBattle selectedTargetCharacterBattle;

    private BattleHandler.LanePosition lastPlayerActiveLanePosition;
    private BattleHandler.LanePosition lastEnemyActiveLanePosition;

    private State state;

    private enum State {
        ChooseCharacter,
        MoveCharacter,
        Busy,
<<<<<<< HEAD
        BeforeStart,
=======
>>>>>>> mergeCombat
    }

    public enum MoveDirection
    {
        Up,
        Down,
        Left,
        Right
    }

    public MoveHandler() {
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

    public static Vector3 GetPosition(BattleHandler.LanePosition lanePosition, bool isPlayerTeam)
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


    public static BattleHandler.LanePosition GetNextLanePosition(BattleHandler.LanePosition lanePosition, MoveDirection moveDirection)
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

        return (BattleHandler.LanePosition)(row * 8 + col); // Convert back to LanePosition
    }

    public void Start(CameraFollow cameraFollow) {
        this.cameraFollow = cameraFollow;
        cameraFollow.Setup(() => Vector3.zero, () => 50f, true, true);
        cameraFollow.SetCameraMoveSpeed(10f);
        cameraFollow.SetCameraZoomSpeed(10f);
        ResetCamera();

        for (int i = 0; i < 64; i++) {
            BattleHandler.LanePosition lanePosition = (BattleHandler.LanePosition)i;
            SpawnTexture(GameAssets.i.s_TankPortrait, lanePosition, true);
        }

        // Set up Battle Scene
        foreach (Character character in GameData.characterList) {
            if (character.isInPlayerTeam) {
                BattleHandler.LanePosition lanePosition;
                switch (character.type) {
                default:
                case Character.Type.Player: lanePosition = BattleHandler.LanePosition.Square_3_0; break;
                case Character.Type.Tank:   lanePosition = BattleHandler.LanePosition.Square_1_2;   break;
                case Character.Type.Healer: lanePosition = BattleHandler.LanePosition.Square_5_2;     break;
                }
                SpawnCharacter(character.type, lanePosition, true, character.stats);
            }
        }

        GameData.EnemyEncounter enemyEncounter = MoveHandler.enemyEncounter;
        for (int i = 0; i < enemyEncounter.enemyBattleArray.Length; i++) {
            GameData.EnemyEncounter.EnemyBattle enemyBattle = enemyEncounter.enemyBattleArray[i];
            SpawnCharacter(enemyBattle.characterType, enemyBattle.lanePosition, false, new Character(enemyBattle.characterType).stats);
        }

        SetActiveCharacterBattle(GetAliveTeamCharacterBattleList(true)[0]);
        
<<<<<<< HEAD
        state = State.BeforeStart;
=======
        state = State.ChooseCharacter;
>>>>>>> mergeCombat
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

    private float GetDistance(BattleHandler.LanePosition src, BattleHandler.LanePosition des) {
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
            BattleHandler.LanePosition lanePosition = (BattleHandler.LanePosition)(i * 8 + col);
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
            BattleHandler.LanePosition lanePosition = (BattleHandler.LanePosition)(row * 8 + i);
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


    private CharacterBattle GetNextCharacterBattle(BattleHandler.LanePosition lanePosition, MoveDirection moveDir, bool isPlayerTeam) {
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

    private CharacterBattle GetCharacterBattleAt(BattleHandler.LanePosition lanePosition, bool isPlayerTeam) {
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
<<<<<<< HEAD
        case State.BeforeStart:
            break;
        case State.ChooseCharacter:
            
=======
        case State.ChooseCharacter:
            // Player Turn

>>>>>>> mergeCombat
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) {
                // Select Target Up
                SetActiveCharacterBattle(GetNextCharacterBattle(activeCharacterBattle.GetLanePosition(), MoveDirection.Up, true));
            }
            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) {
                // Select Target Down
                SetActiveCharacterBattle(GetNextCharacterBattle(activeCharacterBattle.GetLanePosition(), MoveDirection.Down, true));
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) {
                // Select Target Left
                SetActiveCharacterBattle(GetNextCharacterBattle(activeCharacterBattle.GetLanePosition(), MoveDirection.Left, true));
            }
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) {
                // Select Target Right
                SetActiveCharacterBattle(GetNextCharacterBattle(activeCharacterBattle.GetLanePosition(), MoveDirection.Right, true));
            }
            
            if (Input.GetKeyDown(KeyCode.M)) {
                state = State.MoveCharacter;
                CodeMonkey.CMDebug.TextPopupMouse("Change to MOVE CHARACTER mode");
            }
            
            break;
        case State.Busy:
            break;
        case State.MoveCharacter:
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) {
                state = State.Busy;
                // Move the character up (if possible)
                BattleHandler.LanePosition nextLanePosition = GetNextLanePosition(activeCharacterBattle.GetLanePosition(), MoveDirection.Up);
                if (nextLanePosition > activeCharacterBattle.GetLanePosition()) {
                    // at the top of the lane
                    state = State.MoveCharacter;
                    return;
                }
                if (GetCharacterBattleAt(nextLanePosition, true) == null && GetCharacterBattleAt(nextLanePosition, false) == null) {
                    activeCharacterBattle.Move(nextLanePosition, true, () => {
                        state = State.MoveCharacter;
                    });
                }
            }
            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) {
                state = State.Busy;
                // Move the character down (if possible)
                BattleHandler.LanePosition nextLanePosition = GetNextLanePosition(activeCharacterBattle.GetLanePosition(), MoveDirection.Down);
                if (nextLanePosition < activeCharacterBattle.GetLanePosition()) {
                    // at the bottom of the lane
                    state = State.MoveCharacter;
                    return;
                }
                if (GetCharacterBattleAt(nextLanePosition, true) == null && GetCharacterBattleAt(nextLanePosition, false) == null) {
                    activeCharacterBattle.Move(nextLanePosition, true, () => {
                        state = State.MoveCharacter;
                    });
                }
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) {
                state = State.Busy;
                // Move the character left (if possible)
                BattleHandler.LanePosition nextLanePosition = GetNextLanePosition(activeCharacterBattle.GetLanePosition(), MoveDirection.Left);
                if ((int)nextLanePosition % 8 > (int) activeCharacterBattle.GetLanePosition() % 8) {
                    // at the left of the lane
                    state = State.MoveCharacter;
                    return;
                }
                if (GetCharacterBattleAt(nextLanePosition, true) == null && GetCharacterBattleAt(nextLanePosition, false) == null) {
                    activeCharacterBattle.Move(nextLanePosition, false, () => {
                        state = State.MoveCharacter;
                        activeCharacterBattle.FlipCharacter(true);
                    });
                }
            }
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) {
                state = State.Busy;
                // Move the character right (if possible)
                BattleHandler.LanePosition nextLanePosition = GetNextLanePosition(activeCharacterBattle.GetLanePosition(), MoveDirection.Right);
                if ((int) nextLanePosition % 8 < (int) activeCharacterBattle.GetLanePosition() % 8) {
                    // at the right of the lane
                    state = State.MoveCharacter;
                    return;
                }
                if (GetCharacterBattleAt(nextLanePosition, true) == null && GetCharacterBattleAt(nextLanePosition, false) == null) {
                    activeCharacterBattle.Move(nextLanePosition, true, () => {
                        state = State.MoveCharacter;
                    });

                }
            }
            state = State.MoveCharacter;
            if (Input.GetKeyDown(KeyCode.C)) {
                state = State.ChooseCharacter;
                CodeMonkey.CMDebug.TextPopupMouse("Change to CHOOSE CHARACTER mode");
            }

            break;
        }
<<<<<<< HEAD

        // catch Ctrl + S to save the game
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.S)) {
            if (state != State.BeforeStart){
                SaveGame();
            }
        }
    }

    public void SkipGuide()
    {
        Debug.Log("SkipGuide");
        state = State.ChooseCharacter;
=======
>>>>>>> mergeCombat
    }

    public void SaveGame()
    {
        Debug.Log("SaveGame");
        GameData.our_general_position1 = characterBattleList[0].GetLanePosition();
        GameData.our_general_position2 = characterBattleList[1].GetLanePosition();
        GameData.our_general_position3 = characterBattleList[2].GetLanePosition();
        Loader.Load(Loader.Scene.BattleScene);
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

<<<<<<< HEAD
            Loader.Load(Loader.Scene.MainMenu);
=======
            Loader.Load(Loader.Scene.Cinematic_1);
>>>>>>> mergeCombat
            //OvermapHandler.LoadBackToOvermap();
            break;
        case GameData.State.FightingEvilMonster_2:
            // Player Lost to Evil Monster
            character.isDead = true;
            GameData.GetCharacter(Character.Type.Player).stats.health = 1;
            GameData.GetCharacter(Character.Type.Tank).stats.health = 1;
            GameData.GetCharacter(Character.Type.Healer).stats.health = 1;
            GameData.state = GameData.State.LostToEvilMonster_2;

<<<<<<< HEAD
            Loader.Load(Loader.Scene.MainMenu);
=======
            Loader.Load(Loader.Scene.Cinematic_1);
>>>>>>> mergeCombat
            //OvermapHandler.LoadBackToOvermap();
            break;
        case GameData.State.FightingEvilMonster_3:
            // Player Defeated Evil Monster
            character.isDead = true;
            GameData.state = GameData.State.DefeatedEvilMonster;
            
<<<<<<< HEAD
            Loader.Load(Loader.Scene.MainMenu);
=======
            Loader.Load(Loader.Scene.Cinematic_SleezerWin);
>>>>>>> mergeCombat
            //OvermapHandler.LoadBackToOvermap();
            break;
        }
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
            FunctionTimer.Create(OvermapHandler.LoadBackToOvermap, .7f);
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
            BattleHandler.LanePosition LeftNearestEnemyLanePosition = GetNextLanePosition(activeCharacterBattle.GetLanePosition(), MoveDirection.Left);
            BattleHandler.LanePosition UpNearestEnemyLanePosition = GetNextLanePosition(activeCharacterBattle.GetLanePosition(), MoveDirection.Up);

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
                CharacterBattle aiTargetCharacterBattle = GetAliveTeamCharacterBattleList(true)[Random.Range(0, GetAliveTeamCharacterBattleList(true).Count)];
                SetCamera(aiTargetCharacterBattle.GetPosition() + new Vector3(+5f, 0), 30f);
                activeCharacterBattle.AttackTarget(aiTargetCharacterBattle.GetPosition(), () => {
                    SoundManager.PlaySound(SoundManager.Sound.CharacterHit);
                    int damageBase = activeCharacterBattle.GetAttack(1f);
                    int damageMin = (int)(damageBase * 0.8f);
                    int damageMax = (int)(damageBase * 1.2f);
                    int damageAmount = Random.Range(damageMin, damageMax);
                    int damageChance = 90;
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
            List<CharacterBattle> characterBattleList = GetAliveTeamCharacterBattleList(true);
            //SetActiveCharacterBattle(characterBattleList[Random.Range(0, characterBattleList.Count)]);

            //Debug.Log(lastPlayerActiveLanePosition);
            SetActiveCharacterBattle(
                GetNextCharacterBattle(lastPlayerActiveLanePosition, MoveDirection.Down, true)
            );
            lastPlayerActiveLanePosition = activeCharacterBattle.GetLanePosition();
            //Debug.Log("2 " + lastPlayerActiveLanePosition);

            activeCharacterBattle.TickSpecialCooldown();

            RefreshSelectedTargetCharacterBattle();
            state = State.ChooseCharacter;
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
