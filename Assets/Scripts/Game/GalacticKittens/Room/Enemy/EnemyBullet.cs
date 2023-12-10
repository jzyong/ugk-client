using Game.GalacticKittens.Manager;
using Game.GalacticKittens.Player;
using Game.GalacticKittens.Utility;
using Network.Sync;
using UnityEngine;

namespace Game.GalacticKittens.Room.Enemy
{
    /// <summary>
    /// 敌人子弹 
    /// </summary>
    public class EnemyBullet : MonoBehaviour, IObjectDestory
    {
        [SerializeField] AudioClip m_shootClip;




        /// <summary>
        /// 
        /// </summary>
        public void StartShoot()
        {
           
            //收到子弹消息播放音效 
            GalacticKittensAudioManager.Instance.PlaySoundEffect(m_shootClip);
          
        }

        public void Despawn(GalacticKittensObjectDieResponse response)
        {
            //播放命中效果
            var sceneObject = GalacticKittensRoomManager.Instance.GetSceneObject(response.KillerId);
            if (sceneObject != null)
            {
                var spaceship = sceneObject.GetComponent<Spaceship>();
                if (spaceship != null)
                {
                    spaceship.PlayHitEffect();
                }
            }

            SyncManager.Instance.RemoveSyncObject(GetComponent<PredictionTransform>().Id);
            Destroy(gameObject);
        }
    }
}