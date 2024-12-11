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
using CodeMonkey;
using CodeMonkey.Utils;
using CodeMonkey.MonoBehaviours;
using GridPathfindingSystem;

public class GameHandler_Battle : MonoBehaviour {

    [SerializeField] private CameraFollow cameraFollow;

    private void Awake() {
        new BattleHandler();

        if (BattleHandler.enemyEncounter == null) {
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
            BattleHandler.LoadEnemyEncounter(new Character(Character.Type.Enemy_SwordShied1), enemyEncounter);
        }
    }

    private void Start() {
        // Set up Battle Scene
        //BattleHandler.SpawnCharacter(BattleHandler.LanePosition.Middle, true);
        //BattleHandler.SpawnCharacter(BattleHandler.LanePosition.Up, true);
        //BattleHandler.SpawnCharacter(BattleHandler.LanePosition.Down, true);

        /*
        GameData.EnemyEncounter enemyEncounter = BattleHandler.enemyEncounter;
        for (int i = 0; i < enemyEncounter.enemyBattleArray.Length; i++) {
            GameData.EnemyEncounter.EnemyBattle enemyBattle = enemyEncounter.enemyBattleArray[i];
            BattleHandler.SpawnCharacter(enemyBattle, false);
        }
        */

        BattleHandler.GetInstance().Start(cameraFollow);
    }

    private void Update() {
        BattleHandler.GetInstance().Update();
    }

}
