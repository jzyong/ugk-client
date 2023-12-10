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
            var color = spaceship._characterDataSo.color;
            color.a = 255;
            GetComponent<SpriteRenderer>().color = color;

            //坐标向左移动一点，对准枪口
            if (spaceship._characterDataSo.clientId != 0)
            {
                transform.Translate(Vector3.left * 0.5f);
            }


            //收到子弹消息播放音效 
            GalacticKittensAudioManager.Instance.PlaySoundEffect(m_shootClip);
            //播放特效
            var shootVfx = Instantiate(m_shootVfx, spaceship.transform).GetComponent<ParticleSystem>();
            shootVfx.transform.position = transform.position;
            shootVfx.Play();
        }
    }
}