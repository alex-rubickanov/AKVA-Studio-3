using EZCameraShake;
using UnityEngine;
using AKVA.Assets.Vince.Scripts.SceneManager;

namespace AKVA.Player
{
    public class PlayerCheckpointManager : MonoBehaviour
    {
        [SerializeField] private Transform playerTransform;
        [SerializeField] private Transform playerCameraTransform;
        [SerializeField] private SavedCheckpoint savedCheckpoint;

        [SerializeField] private bool enableCheckpoint = true;

        private LevelManager levelManager;
        
        private void Awake()
        {
            levelManager = GetComponent<LevelManager>();
            
            if (enableCheckpoint)
            {
                StartGameOnSavedCheckpoint();
            }
        }

        private void StartGameOnSavedCheckpoint()
        {
            if (!savedCheckpoint.isSaved) return;


            playerTransform.position = savedCheckpoint.position;
            playerTransform.forward = savedCheckpoint.forward;
            levelManager.SetCurrentLevel(savedCheckpoint.stateEnum);

            playerCameraTransform.GetComponent<CameraShaker>().enabled = false;
            playerTransform.GetComponent<QTEEscape>().Cancel();
        }
    }
}