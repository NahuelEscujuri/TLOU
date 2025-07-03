using UnityEngine;

namespace HeartRateSystem
{
    public class MovementHeartRateMediator: MonoBehaviour
    {
        [SerializeField] RateSounds[] rateSounds;
        [SerializeField] HeartRateController heartRateController;
        [SerializeField] GameObject movementController;
        RunHeartRateState runState;

        private void Awake()
        {
            runState = new RunHeartRateState(movementController.GetComponent<IMovement>(), heartRateController, new RateSoundsLibrary(rateSounds));
            heartRateController.AddState(runState);
        }
    }
}

