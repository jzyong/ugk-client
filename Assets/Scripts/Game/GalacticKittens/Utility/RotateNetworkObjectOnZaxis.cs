using UnityEngine;

namespace Game.GalacticKittens.Utility
{
    /// <summary>
    /// Z轴旋转对象
    /// </summary>
    public class RotateNetworkObjectOnZaxis : MonoBehaviour
    {
        public float m_rotationSpeed = 1f;

        private void Update()
        {
        
            transform.Rotate(m_rotationSpeed * Time.deltaTime * Vector3.forward);
        }
    }
}
