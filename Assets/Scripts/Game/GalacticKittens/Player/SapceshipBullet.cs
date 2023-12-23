using Game.GalacticKittens.Manager;
using Game.GalacticKittens.Room.Boss;
using Game.GalacticKittens.Room.Enemy;
using Game.GalacticKittens.Utility;
using Network;
using Network.Sync;
using UnityEngine;

namespace Game.GalacticKittens.Player
{
    /// <summary>
    /// 玩家子弹 
    /// </summary>
    public class SapceshipBullet : MonoBehaviour, IObjectDestory
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

        public void Despawn(GalacticKittensObjectDieResponse response)
        {
            //播放命中效果
            var sceneObject = GalacticKittensRoomManager.Instance.GetSceneObject(response.KillerId);
            if (sceneObject != null)
            {
                var baseEnemy = sceneObject.GetComponent<BaseEnemy>();
                if (baseEnemy != null)
                {
                    baseEnemy.PlayerHitEffect();
                    if (baseEnemy is Boss)
                    {
                        GalacticKittensRoomManager.Instance.BossUI.UpdateHealth(1);
                    }
                }

            }


            SyncManager.Instance.RemoveSyncObject(GetComponent<PredictionTransform>().Id);
            Destroy(gameObject);
        }
    }
}