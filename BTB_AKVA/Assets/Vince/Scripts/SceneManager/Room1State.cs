using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AKVA.Player;
using AKVA.Assets.Vince.Scripts.AI;
using AKVA.Assets.Vince.Scripts.Environment;
using PlasticGui.WorkspaceWindow;
using UnityEditor;
using UnityEditor.Build;

namespace AKVA.Assets.Vince.Scripts.SceneManager
{
    public class Room1State : SceneState
    {
        bool playerInPosition;
        bool aiActive; //Initiates the AI task
        bool enableAI; //Initiate Each AI to be activated
        bool[] taskDone;
        public override void OnEnterState(SceneStateManager state)
        {
            taskDone = new bool[6];
        }

        public override void OnUpdateState(SceneStateManager state)
        {
            CheckIfPlayerIsInThePlaceHolder(state);
            ActivateAI(state);
        }

        public override void OnExitState(SceneStateManager state)
        {
        }

        private void CheckIfPlayerIsInThePlaceHolder(SceneStateManager state)
        {
            if (Vector3.Distance(state.playerTransform.position, state.room1PlayerPos.position) < 1.5f && !playerInPosition) // if player has positioned to its place holder
            {
                state.room1TutorialMonitor.turnOnTV = true;
                PlayerInput.Instance.DisablePlayerMovement();
                state.StartCoroutine(StartAITask(state, 0));
                playerInPosition = true;
            }
        }

        private void ActivateAI(SceneStateManager state)
        {
            if (aiActive)
            {
                if (GetNumberOfActiveSockets(state) == 1 && !taskDone[0] && !enableAI)
                {
                    state.StartCoroutine(StartAITask(state, 1));
                    enableAI = true;
                    taskDone[0] = true;
                }
                else if (GetNumberOfActiveSockets(state) == 2 && !taskDone[1] && !enableAI)
                {
                    state.StartCoroutine(StartAITask(state, 2));
                    enableAI = true;
                    taskDone[1] = true;
                }
                else if (GetNumberOfActiveSockets(state) == 3 && !taskDone[2] && !enableAI)
                {
                    state.playerPicking.enabled = true;
                    taskDone[2] = true;
                    PlayerInput.Instance.EnablePlayerMovement();
                }
                else if (GetNumberOfActiveSockets(state) == 4 && Vector3.Distance(state.playerTransform.position, state.room1PlayerPos.position) < 1.5f && !taskDone[4])
                {
                    PlayerInput.Instance.DisablePlayerMovement();
                    state.StartCoroutine(LineUP(state));
                    taskDone[4] = true;
                }
                else if (taskDone[4] && !taskDone[5] && Vector3.Distance(state.playerTransform.position, state.room1PlayerPos2.position) < 1.5f)
                {
                    state.room1TutorialMonitor.ProceedToNextRoomText();
                    state.room1Door.EnableDoor = true;
                    for (int i = 0; i < state.listOfAI.Length; i++)
                    {
                        AIStateManager ai = state.listOfAI[i].GetComponent<AIStateManager>();
                        ai.targetIndex++;
                        ai.moveOnly = true;
                        ai.currentTarget = ai.pathPoints[ai.targetIndex];
                        state.StartCoroutine(ProceedToNextRoom(state, ai));
                    }
                    taskDone[5] = true;
                }
            }
        }

        IEnumerator LineUP(SceneStateManager state, float lineUpDelay = 3f)
        {
            state.room1TutorialMonitor.LineUPTxt();
            for (int i = 0; i < state.listOfAI.Length; i++)
            {
                AIStateManager ai = state.listOfAI[i].GetComponent<AIStateManager>();
                ai.targetIndex++;
                ai.moveOnly = true;
                ai.currentTarget = ai.pathPoints[ai.targetIndex];
                ai.SwitchState(ai.moveState);
                yield return new WaitForSeconds(lineUpDelay);
            }
            PlayerInput.Instance.EnablePlayerMovement();
        }


        IEnumerator StartAITask(SceneStateManager state, int aiIndex) //Starting AI to do its task
        {
            yield return new WaitForSeconds(3f);
            state.listOfAI[aiIndex].GetComponent<AIStateManager>().activateAI = true;
            aiActive = true;
            enableAI = false;
        }

        public int GetNumberOfActiveSockets(SceneStateManager state)
        {
            int activeSockets = 0;

            foreach (var socket in state.room1Buttons.Items)
            {
                if (socket.GetComponent<BatterySocket>().socketIsActive)
                {
                    activeSockets++;
                }
            }
            return activeSockets;
        }


        IEnumerator ProceedToNextRoom(SceneStateManager state, AIStateManager ai)
        {
            yield return new WaitForSeconds(0f);
            ai.SwitchState(ai.moveState);
            state.SwitchState(state.room2State);
        }
    }
}