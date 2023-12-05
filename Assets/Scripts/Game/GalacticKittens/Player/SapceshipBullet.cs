using Game.GalacticKittens.Manager;
using Network;
using UnityEngine;

namespace Game.GalacticKittens.Player
{
    /// <summary>
    /// 玩家子弹 
    /// </summary>
    public class SapceshipBullet : MonoBehaviour
    {
        
        [SerializeField]
        AudioClip m_shootClip;

        /// <summary>
        /// 收到子弹消息播放音效 
        /// </summary>
        public void PlayShootBulletSound()
        {
            GalacticKittensAudioManager.Instance.PlaySoundEffect(m_shootClip);
        }
       
    }
}
