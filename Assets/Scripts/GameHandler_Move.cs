using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using CodeMonkey;
using CodeMonkey.Utils;
using CodeMonkey.MonoBehaviours;
using GridPathfindingSystem;

public class GameHandler_Move : MonoBehaviour {

    [SerializeField] private CameraFollow cameraFollow;
    public Button saveButton;
<<<<<<< HEAD
    private void Awake() {
        UnityEngine.Rendering.DebugManager.instance.enableRuntimeUI = false;
=======

    private void Awake() {
>>>>>>> mergeCombat
        new MoveHandler();

        if (MoveHandler.enemyEncounter == null) {
            Debug.Log("#### Debug Enemy Encounter");
            GameData.EnemyEncounter enemyEncounter = new GameData.EnemyEncounter {
                enemyBattleArray = new GameData.EnemyEncounter.EnemyBattle[] { 
                    new GameData.EnemyEncounter.EnemyBattle { characterType = Character.Type.Enemy_SwordShied1, lanePosition = BattleHandler.LanePosition.Square_0_6 },
                    new GameData.EnemyEncounter.EnemyBattle { characterType = Character.Type.Enemy_SwordShied2, lanePosition = BattleHandler.LanePosition.Square_3_7 },
                    new GameData.EnemyEncounter.EnemyBattle { characterType = Character.Type.Enemy_SwordShied1, lanePosition = BattleHandler.LanePosition.Square_6_6 },
                    new GameData.EnemyEncounter.EnemyBattle { characterType = Character.Type.Enemy_SwordShied1, lanePosition = BattleHandler.LanePosition.Square_1_5 },
                    new GameData.EnemyEncounter.EnemyBattle { characterType = Character.Type.Enemy_SwordShied3, lanePosition = BattleHandler.LanePosition.Square_4_6 }, 
                },
            };
            MoveHandler.LoadEnemyEncounter(new Character(Character.Type.Enemy_SwordShied1), enemyEncounter);
        }
    }

    private void Start() {
<<<<<<< HEAD
        
=======
>>>>>>> mergeCombat
        // Set up Battle Scene
        //MoveHandler.SpawnCharacter(MoveHandler.LanePosition.Middle, true);
        //MoveHandler.SpawnCharacter(MoveHandler.LanePosition.Up, true);
        //MoveHandler.SpawnCharacter(MoveHandler.LanePosition.Down, true);

        /*
        GameData.EnemyEncounter enemyEncounter = MoveHandler.enemyEncounter;
        for (int i = 0; i < enemyEncounter.enemyBattleArray.Length; i++) {
            GameData.EnemyEncounter.EnemyBattle enemyBattle = enemyEncounter.enemyBattleArray[i];
            MoveHandler.SpawnCharacter(enemyBattle, false);
        }
        */

        saveButton = GameObject.Find("btnSave").GetComponent<Button>();
        UnityAction action = MoveHandler.GetInstance().SaveGame;
        saveButton.onClick.AddListener(action);

        MoveHandler.GetInstance().Start(cameraFollow);

    }

    private void Update() {

        MoveHandler.GetInstance().Update();
    }

}