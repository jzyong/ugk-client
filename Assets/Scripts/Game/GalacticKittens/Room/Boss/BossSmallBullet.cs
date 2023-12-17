using Game.GalacticKittens.Manager;
using Game.GalacticKittens.Player;
using Game.GalacticKittens.Utility;
using Network.Sync;

namespace Game.GalacticKittens.Room.Boss
{
    /// <summary>
    /// 小子弹
    /// </summary>
    public class BossSmallBullet:SnapTransform,IObjectDestory
    {
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

            SyncManager.Instance.RemoveSyncObject(GetComponent<SnapTransform>().Id);
            Destroy(gameObject);
        }
    }
}