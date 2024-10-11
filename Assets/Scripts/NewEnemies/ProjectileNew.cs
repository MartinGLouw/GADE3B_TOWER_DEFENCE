using UnityEngine;

namespace NewEnemies
{
    public abstract class ProjectileNew : MonoBehaviour
    {
        protected float Speed = 10f;

        public abstract void Initialize(float damage);

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Defender"))
            {
                // Handle the interaction with the defender.
            }
            Destroy(gameObject);
        }

        protected void MoveForward()
        {
            transform.Translate(Vector3.forward * Speed * Time.deltaTime);
        }

        private void Update()
        {
            MoveForward();
        }
    }
}
