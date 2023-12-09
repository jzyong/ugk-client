using Common;
using Common.Tools;
using Game.GalacticKittens.Player;
using Lobby;
using Network;
using Network.Messages;
using Network.Sync;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.GalacticKittens.Manager
{
    /// <summary>
    /// 房间管理
    /// </summary>
    public class GalacticKittensRoomManager : SingletonPersistent<GalacticKittensRoomManager>
    {
        [SerializeField] private Spaceship[] spaceships;
        [SerializeField] private SapceshipBullet _shipShootBullet;


        private void OnEnable()
        {
            MessageEventManager.Singleton.AddEvent<GalacticKittensObjectSpawnResponse>(
                MessageEvent.GalacticKittensObjectSpawn, SpawnObject);
        }

        private void OnDisable()
        {
            MessageEventManager.Singleton.RemoveEvent<GalacticKittensObjectSpawnResponse>(
                MessageEvent.GalacticKittensObjectSpawn, SpawnObject);
        }


        //TODO 向场景中添加对象
        private void SpawnObject(GalacticKittensObjectSpawnResponse response)
        {
            foreach (var spawnInfo in response.Spawn)
            {
                Debug.Log($"{spawnInfo.Id} ConfigID={spawnInfo.ConfigId} 出生于 {spawnInfo.Position}");

                switch (spawnInfo.ConfigId)
                {
                    case 0: //玩家
                    case 1: //玩家
                    case 2: //玩家
                    case 3: //玩家
                        SpawnSpaceShip(spawnInfo);
                        break;
                    case 30: //玩家发射子弹
                        spawnPlayerBullet(spawnInfo);
                        break;
                    default:
                        Debug.Log($"{spawnInfo.ConfigId} spawn 未实现");
                        break;
                }


                //TODO 创建玩家或敌人对象，并且添加同步组件
            }
        }


        /// <summary>
        /// 出生飞船
        /// </summary>
        /// <param name="spawnInfo"></param>
        private void SpawnSpaceShip(GalacticKittensObjectSpawnResponse.Types.SpawnInfo spawnInfo)
        {
            var spaceship = Instantiate(spaceships[spawnInfo.ConfigId], null); //TODO 完善飞船对象
            spaceship.name = $"Spaceship{spawnInfo.Id}";
            var snapTransform = spaceship.GetComponent<SnapTransform>();
            snapTransform.Id = spawnInfo.Id;
            var position = ProtoUtil.BuildVector3(spawnInfo.Position);
            spaceship.transform.position = position;
            if (snapTransform.Id == DataManager.Instance.PlayerInfo.PlayerId)
            {
                snapTransform.IsOnwer = true;
            }
            // SnapTransform 的初始坐标
            snapTransform.InitTransform(position,null);

            SyncManager.Instance.AddSnapTransform(snapTransform);
            DataManager.Instance.GalacticKittens.Spaceships[snapTransform.Id] = spaceship;
        }

        /// <summary>
        /// 产生玩家子弹 //TODO 待测试
        /// </summary>
        /// <param name="spawnInfo"></param>
        private void spawnPlayerBullet(GalacticKittensObjectSpawnResponse.Types.SpawnInfo spawnInfo)
        {
            var spaceship = DataManager.Instance.GalacticKittens.Spaceships[spawnInfo.OwnerId];
            if (spaceship == null)
            {
                Debug.Log($"子弹 {spawnInfo.Id} 主人{spawnInfo.OwnerId} 不存在");
                return;
            }

            var sapceshipBullet = Instantiate(_shipShootBullet, spaceship.transform);
            sapceshipBullet.name = $"SpaceshipBullet{spawnInfo.Id}";
            PredictionTransform predictionTransform = spaceship.GetComponent<PredictionTransform>();
            predictionTransform.LinearVelocity = ProtoUtil.BuildVector3(spawnInfo.LinearVelocity);
            sapceshipBullet.transform.position = ProtoUtil.BuildVector3(spawnInfo.Position);
            // predictionTransform.InitLastVector3LongPositon(spaceship.transform.position);
            // predictionTransform.SetLastDeserializedLinearVelocity(predictionTransform.LinearVelocity);

            sapceshipBullet.PlayShootBulletSound();
        }


        /// <summary>
        /// 退出到大厅
        /// </summary>
        public void quitToLobby()
        {
            SceneManager.LoadScene("Lobby");
            Destroy(GalacticKittensAudioManager.Instance);
            Destroy(Instance);
        }
        
    }
}