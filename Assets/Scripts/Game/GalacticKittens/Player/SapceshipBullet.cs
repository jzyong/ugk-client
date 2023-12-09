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
        [SerializeField] AudioClip m_shootClip;


        [SerializeField] [Tooltip("射击特效")] GameObject m_shootVfx;

        /// <summary>
        /// 
        /// </summary>
        public void StartShoot(Spaceship spaceship)
        {
            GetComponent<SpriteRenderer>().color = spaceship._characterDataSo.color;
            
            //收到子弹消息播放音效 
            GalacticKittensAudioManager.Instance.PlaySoundEffect(m_shootClip);
            //播放特效
            var shootVfx = Instantiate(m_shootVfx, spaceship.transform).GetComponent<ParticleSystem>();
            var shipPosition = spaceship.transform.position;
            shootVfx.transform.position = new Vector3(shipPosition.x+0.9f, shipPosition.y - 0.3f, shipPosition.z);
            shootVfx.Play();

           
        }
    }
}