using AKVA.Assets.Vince.Scripts.Environment;
using AKVA.Assets.Vince.Scripts.AI;
using AKVA.Controls;
using AKVA.Player;
using AKVA.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AKVA.Assets.Vince.SO;

namespace AKVA.Assets.Vince.Scripts.SceneManager
{
    public class SceneStateManager : MonoBehaviour
    {
        SceneState currentState;
        public MovementTutorialState movementTutorial = new MovementTutorialState();
        public Room1State room1State = new Room1State();
        public Room2State room2State = new Room2State();

        [HideInInspector] public Transform playerTransform;
        [HideInInspector] public Picking playerPicking;
        public RuntimeList listOfAI;


        [Header("Movement Tutorial")]
        public float timeDelayBeforePlayerMovement;
        public float timeDelayDuringTutorial;
        public DoubleDoor roomDoor;


        [Header("Room 1 Scene")]
        public DoubleDoor room1Door;
        public Transform room1PlayerPos;
        public RuntimeList room1Buttons;
        public RuntimeList room1Boxes;
        public RuntimeList room1PositionPoints;


        [Header("Room 2 Scene")]
        public Transform room2PlayerPos;
        public RuntimeList room2Buttons;

        private void Start()
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            playerPicking = playerTransform.GetComponent<Picking>();
            playerPicking.enabled = false;
            MovementTutorial();
        }

        private void Update()
        {
            if (currentState != null)
            {
                currentState.OnUpdateState(this);
            }
        }

        public void SwitchState(SceneState state)
        {
            currentState = state;
            state.OnEnterState(this);
        }

        private void MovementTutorial()
        {
            PlayerInput.Instance.DisablePlayerMovement(false);
            StartCoroutine(StartMovementTutorial());
        }

        IEnumerator StartMovementTutorial()
        {
            yield return new WaitForSeconds(timeDelayBeforePlayerMovement);
            Debug.Log("Movement Tutorial Started");
            currentState = movementTutorial;
            currentState.OnEnterState(this);
        }
    }
}