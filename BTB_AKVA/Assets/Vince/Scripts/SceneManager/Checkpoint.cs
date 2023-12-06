using AKVA.Assets.Vince.Scripts.SceneManager;
using UnityEngine;
using UnityEngine.Serialization;

namespace AKVA.Player
{
    public class Checkpoint : MonoBehaviour
    {
        [SerializeField] private SavedCheckpoint savedCheckpoint;
        [SerializeField] private LevelManager.LevelStatesEnum levelState;
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                LevelManager.Instance.SetCurrentLevel(levelState);
                
                savedCheckpoint.position = transform.position;
                savedCheckpoint.forward = Vector3.forward;
                savedCheckpoint.isSaved = true;
                savedCheckpoint.stateEnum = levelState;
            }
        }
    }
}