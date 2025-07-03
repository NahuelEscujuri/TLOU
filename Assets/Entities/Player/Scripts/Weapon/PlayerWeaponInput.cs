using UnityEngine;

namespace Automata
{
    [RequireComponent(typeof(PlayerWeaponSystem))]
    public class PlayerWeaponInput : MonoBehaviour
    {
        private PlayerWeaponSystem weaponSystem;
        [SerializeField] Transform handTransform;

        private void Awake()
        {
            weaponSystem = GetComponent<PlayerWeaponSystem>();
        }

        private void Update()
        {
            // Disparo con clic izquierdo
            if (Input.GetMouseButtonDown(0))
            {
                weaponSystem.Shoot();
            }

            // Recarga con tecla R
            if (Input.GetKeyDown(KeyCode.R))
            {
                weaponSystem.Reload();
            }

            // Desequipar arma con Q (opcional)
            /*if (Input.GetKeyDown(KeyCode.Q))
            {
                weaponSystem.UnequipCurrentWeapon();
            }*/

            // Ejemplo de equipar arma nueva con E (deberás implementar lógica de pickup)

            /*if (Input.GetKeyDown(KeyCode.E))
            {
                // Lógica para detectar y equipar arma cercana

            }*/

        }
    }
}
