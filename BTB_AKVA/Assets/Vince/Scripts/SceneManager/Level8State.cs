using AKVA.Assets.Vince.Scripts.AI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AKVA.Assets.Vince.Scripts.SceneManager
{
    public class Level8State : LevelState
    {
        bool robotActivated;
        public override void OnEnterState(LevelManager state)
        {
            Debug.Log("LevelState Entered");
        }

        public override void OnUpdateState(LevelManager state)
        {
            ActivateBangingDoor(state);
            MoveAllRobotsToExit(state);
        }

        private void ActivateBangingDoor(LevelManager state)
        {
            if(Vector3.Distance(state.player.transform.position, state.triggerQueue.position) < 2f)
            {
                state.scientistLevel8.SetActive(true);
                state.backSideDoor.enabled = true;
            }
        }

        private void MoveAllRobotsToExit(LevelManager state)
        {
            if (state.moveAllRobotsToExit)
            {
                if (!robotActivated)
                {
                    robotActivated = true;
                    
                    foreach(RobotMovement robot in state.level7Robots)
                    {
                        robot.moveToNextLocation = true;
                    }

                }
            }
        }
    }
}
