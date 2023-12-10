using System.Collections.Generic;
using Common;
using Common.Tools;
using Game.GalacticKittens.Player;
using Game.GalacticKittens.Room.Enemy;
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
        [SerializeField] private GhostEnemy ghostEnemy;
        [SerializeField] private ShooterEnemy shooterEnemy;
        [SerializeField] private Meteor _meteor;

        /// <summary>
        /// 场景所有对象
        /// </summary>
        private Dictionary<long, GameObject> sceneObjects = new Dictionary<long, GameObject>();


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

                // 0-3玩家飞船；20Boss；30玩家子弹，31敌人子弹；40射击敌人、41幽灵敌人、42陨石
                switch (spawnInfo.ConfigId)
                {
                    case 0: //玩家
                    case 1: //玩家
                    case 2: //玩家
                    case 3: //玩家
                        SpawnSpaceShip(spawnInfo);
                        break;
                    case 30: //玩家发射子弹
                        SpawnPlayerBullet(spawnInfo);
                        break;
                    case 40:
                        SpawnShooterEnemy(spawnInfo);
                        break;
                    case 41:
                        SpawnGhostEnemy(spawnInfo);
                        break;
                    case 42:
                        SpawnMeteor(spawnInfo);
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
            var spaceship = Instantiate(spaceships[spawnInfo.ConfigId], Instance.transform);
            spaceship.name = $"Spaceship{spawnInfo.Id}";
            spaceship.Id = spawnInfo.Id;
            var snapTransform = spaceship.GetComponent<SnapTransform>();
            snapTransform.Id = spawnInfo.Id;
            var position = ProtoUtil.BuildVector3(spawnInfo.Position);
            spaceship.transform.position = position;
            if (snapTransform.Id == DataManager.Instance.PlayerInfo.PlayerId)
            {
                snapTransform.IsOnwer = true;
            }

            // SnapTransform 的初始坐标
            snapTransform.InitTransform(position, null);
            sceneObjects[spaceship.Id] = spaceship.gameObject;
            SyncManager.Instance.AddSnapTransform(snapTransform);
            DataManager.Instance.GalacticKittens.Spaceships[snapTransform.Id] = spaceship;
        }

        /// <summary>
        /// 产生玩家子弹 
        /// </summary>
        /// <param name="spawnInfo"></param>
        private void SpawnPlayerBullet(GalacticKittensObjectSpawnResponse.Types.SpawnInfo spawnInfo)
        {
            var spaceship = DataManager.Instance.GalacticKittens.Spaceships[spawnInfo.OwnerId];
            if (spaceship == null)
            {
                Debug.Log($"子弹 {spawnInfo.Id} 主人{spawnInfo.OwnerId} 不存在");
                return;
            }

            var sapceshipBullet = Instantiate(_shipShootBullet, Instance.transform);
            sapceshipBullet.name = $"SpaceshipBullet{spawnInfo.Id}";
            PredictionTransform predictionTransform = sapceshipBullet.GetComponent<PredictionTransform>();
            predictionTransform.LinearVelocity = ProtoUtil.BuildVector3(spawnInfo.LinearVelocity);
            sapceshipBullet.transform.position = ProtoUtil.BuildVector3(spawnInfo.Position);
            predictionTransform.Id = spawnInfo.Id;

            sapceshipBullet.StartShoot(spaceship);
            sceneObjects[spawnInfo.Id] = sapceshipBullet.gameObject;
            SyncManager.Instance.AddPredictionTransform(predictionTransform);
        }

        /// <summary>
        /// 产生幽灵敌人
        /// </summary>
        /// <param name="spawnInfo"></param>
        private void SpawnGhostEnemy(GalacticKittensObjectSpawnResponse.Types.SpawnInfo spawnInfo)
        {
            var enemy = Instantiate(ghostEnemy, Instance.transform);
            enemy.name = $"GhostEnemy{spawnInfo.Id}";
            var snapTransform = enemy.GetComponent<SnapTransform>();
            var spawnPosition = ProtoUtil.BuildVector3(spawnInfo.Position);
            enemy.transform.position = spawnPosition;
            snapTransform.Id = spawnInfo.Id;
            snapTransform.InitTransform(spawnPosition, null);
            sceneObjects[spawnInfo.Id] = enemy.gameObject;
            SyncManager.Instance.AddSnapTransform(snapTransform);
        }

        /// <summary>
        /// 产生攻击敌人
        /// </summary>
        /// <param name="spawnInfo"></param>
        private void SpawnShooterEnemy(GalacticKittensObjectSpawnResponse.Types.SpawnInfo spawnInfo)
        {
            var enemy = Instantiate(shooterEnemy, Instance.transform);
            enemy.name = $"ShooterEnemy{spawnInfo.Id}";
            var snapTransform = enemy.GetComponent<SnapTransform>();
            var spawnPosition = ProtoUtil.BuildVector3(spawnInfo.Position);
            enemy.transform.position = spawnPosition;
            snapTransform.InitTransform(spawnPosition, null);
            snapTransform.Id = spawnInfo.Id;
            sceneObjects[spawnInfo.Id] = enemy.gameObject;
            SyncManager.Instance.AddSnapTransform(snapTransform);
        }
        
       /// <summary>
       /// 产生陨石
       /// </summary>
       /// <param name="spawnInfo"></param>
        private void SpawnMeteor(GalacticKittensObjectSpawnResponse.Types.SpawnInfo spawnInfo)
        {
            var meteor = Instantiate(_meteor, Instance.transform);
            meteor.name = $"Meteor{spawnInfo.Id}";
            var predictionTransform = meteor.GetComponent<PredictionTransform>();
            var spawnPosition = ProtoUtil.BuildVector3(spawnInfo.Position);
            meteor.transform.position = spawnPosition;
            meteor.transform.localScale = ProtoUtil.BuildVector3(spawnInfo.Scale);
            predictionTransform.LinearVelocity = ProtoUtil.BuildVector3(spawnInfo.LinearVelocity);
            predictionTransform.Id = spawnInfo.Id;
            sceneObjects[spawnInfo.Id] = meteor.gameObject;
            SyncManager.Instance.AddPredictionTransform(predictionTransform);
        }


        public void DespawnObject(GalacticKittensObjectDieResponse response)
        {
            SyncManager.Instance.RemoveSyncObject(response.Id);

            if (sceneObjects.Remove(response.Id, out GameObject gameObject))
            {
                Destroy(gameObject);
            }
            else
            {
                Debug.Log($"销毁对象 {response.Id} 未找到");
            }
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