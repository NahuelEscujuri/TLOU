using Unity.VisualScripting;
using UnityEngine;

namespace Automata
{
    public class _test_Scipt : MonoBehaviour
    {
        [SerializeField] CoversHandler coverSystem;
        [SerializeField] float radius = 4f;
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.O))
                Debug.Log("Cover count" + coverSystem.GetCoverPointsInArea(transform.position, radius).Count);
        }
        void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}