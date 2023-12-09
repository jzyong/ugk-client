using UnityEngine;

namespace Game.GalacticKittens.Utility
{
    public class AutoDespawn : MonoBehaviour
    {
        [Min(0f)]
        [SerializeField]
        [Header("Time alive in seconds (s)")]
        private float m_autoDestroyTime=2;


        private void Update()
        {

            m_autoDestroyTime -= Time.deltaTime;

            if(m_autoDestroyTime <= 0f)
            {
                Destroy(gameObject);
            }
        }
    }
}