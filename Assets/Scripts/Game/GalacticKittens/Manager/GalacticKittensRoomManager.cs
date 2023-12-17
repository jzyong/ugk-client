using System.Collections;
using System.Collections.Generic;
using Common.Tools;
using Game.GalacticKittens.Player;
using Game.GalacticKittens.Room;
using Game.GalacticKittens.Room.Boss;
using Game.GalacticKittens.Room.Enemy;
using Game.GalacticKittens.Utility;
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
        [SerializeField] private EnemyBullet _enemyShootBullet;
        [SerializeField] private BossCicularBullet _bossCicularBullet;
        [SerializeField] private BossSmallBullet _bossSmallBullet;
        [SerializeField] private BossSmallBulletCircular _bossSmallBulletCircular;
        [SerializeField] private BossHomingMisile _bossHomingMisile;
        [SerializeField] private GhostEnemy ghostEnemy;
        [SerializeField] private ShooterEnemy shooterEnemy;
        [SerializeField] private Meteor _meteor;
        [SerializeField] private Boss _boss;
        [SerializeField] private PowerUp _powerUp;
        [SerializeField] private GameObject bossWarningUI;
        [SerializeField] private AudioClip bossWarningClip;

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


        private void SpawnObject(GalacticKittensObjectSpawnResponse response)
        {
            foreach (var spawnInfo in response.Spawn)
            {
                Debug.Log($"{spawnInfo.Id} ConfigID={spawnInfo.ConfigId} 出生于 {spawnInfo.Position}");

                // 0-3玩家飞船；20Boss预警，21Boss；30玩家子弹，31敌人子弹，32 boss三角形小子弹，33 boss环形分裂后小子弹，34 boss环形分裂子弹，35 boss导弹；
                // 40射击敌人、41幽灵敌人、41陨石；50能量道具
                switch (spawnInfo.ConfigId)
                {
                    case 0: //玩家
                    case 1: //玩家
                    case 2: //玩家
                    case 3: //玩家
                        SpawnSpaceShip(spawnInfo);
                        break;
                    case 20: //boss 出生预警
                        StartCoroutine(SpawnBossWarning());
                        break;
                    case 21: //boss出现
                        SpawnBoss(spawnInfo);
                        break;
                    case 30: //玩家发射子弹
                        SpawnPlayerBullet(spawnInfo);
                        break;
                    case 31:
                        SpawnEnemyBullet(spawnInfo);
                        break;
                    case 32: //Boss 子弹
                    case 33:
                    case 34:
                    case 35:
                        SpawnBossBullet(spawnInfo);
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
                    case 50:
                        SpawnPowerUp(spawnInfo);
                        break;
                    default:
                        Debug.Log($"{spawnInfo.ConfigId} spawn 未实现");
                        break;
                }

                NetworkStatistics.sceneObjectCount = sceneObjects.Count;
            }
        }

        /// <summary>
        /// 播放boss预警
        /// </summary>
        /// <returns></returns>
        private IEnumerator SpawnBossWarning()
        {
            bossWarningUI.SetActive(true);
            GalacticKittensAudioManager.Instance.PlaySoundEffect(bossWarningClip);
            yield return new WaitForSeconds(bossWarningClip.length);
            bossWarningUI.SetActive(false);
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

            spaceship._characterDataSo.clientId = spawnInfo.ConfigId;
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
        /// 产生敌人子弹 
        /// </summary>
        /// <param name="spawnInfo"></param>
        private void SpawnBossBullet(GalacticKittensObjectSpawnResponse.Types.SpawnInfo spawnInfo)
        {
            // 32 boss三角形小子弹，33 boss环形分裂后小子弹，34 boss环形分裂子弹，35 boss导弹
            GameObject bullet = null;
            switch (spawnInfo.ConfigId)
            {
                case 32:
                    bullet = Instantiate(_bossSmallBullet, Instance.transform).gameObject;
                    break;
                case 33:
                    bullet = Instantiate(_bossSmallBulletCircular, Instance.transform).gameObject;
                    break;
                case 34:
                    bullet = Instantiate(_bossCicularBullet, Instance.transform).gameObject;
                    break;
                case 35:
                    bullet = Instantiate(_bossHomingMisile, Instance.transform).gameObject;
                    break;
            }

            if (bullet == null)
            {
                Debug.LogWarning($"{spawnInfo.ConfigId} 子弹类型错误");
                return;
            }

            bullet.name = $"BossBullet{spawnInfo.Id}";
            var snapTransform = bullet.GetComponent<SnapTransform>();
            var position = ProtoUtil.BuildVector3(spawnInfo.Position);
            bullet.transform.position = position;
            snapTransform.InitTransform(position, null);
            snapTransform.Id = spawnInfo.Id;
            sceneObjects[spawnInfo.Id] = bullet.gameObject;
            SyncManager.Instance.AddSnapTransform(snapTransform);
        }

        /// <summary>
        /// 产生Boss子弹 
        /// </summary>
        /// <param name="spawnInfo"></param>
        private void SpawnEnemyBullet(GalacticKittensObjectSpawnResponse.Types.SpawnInfo spawnInfo)
        {
            var bullet = Instantiate(_enemyShootBullet, Instance.transform);
            bullet.name = $"EnemyBullet{spawnInfo.Id}";
            PredictionTransform predictionTransform = bullet.GetComponent<PredictionTransform>();
            predictionTransform.LinearVelocity = ProtoUtil.BuildVector3(spawnInfo.LinearVelocity);
            bullet.transform.position = ProtoUtil.BuildVector3(spawnInfo.Position);
            predictionTransform.Id = spawnInfo.Id;

            bullet.StartShoot();
            sceneObjects[spawnInfo.Id] = bullet.gameObject;
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
        
        /// <summary>
        /// 产生能量
        /// </summary>
        /// <param name="spawnInfo"></param>
        private void SpawnPowerUp(GalacticKittensObjectSpawnResponse.Types.SpawnInfo spawnInfo)
        {
            var powerUp = Instantiate(_powerUp, Instance.transform);
            powerUp.name = $"PowerUp{spawnInfo.Id}";
            var predictionTransform = powerUp.GetComponent<PredictionTransform>();
            var spawnPosition = ProtoUtil.BuildVector3(spawnInfo.Position);
            powerUp.transform.position = spawnPosition;
            predictionTransform.LinearVelocity = ProtoUtil.BuildVector3(spawnInfo.LinearVelocity);
            predictionTransform.Id = spawnInfo.Id;
            sceneObjects[spawnInfo.Id] = powerUp.gameObject;
            SyncManager.Instance.AddPredictionTransform(predictionTransform);
        }

        /// <summary>
        /// 产生Boss
        /// </summary>
        /// <param name="spawnInfo"></param>
        private void SpawnBoss(GalacticKittensObjectSpawnResponse.Types.SpawnInfo spawnInfo)
        {
            var boss = Instantiate(_boss, Instance.transform);
            boss.name = $"Boss{spawnInfo.Id}";
            var snapTransform = boss.GetComponent<SnapTransform>();
            var spawnPosition = ProtoUtil.BuildVector3(spawnInfo.Position);
            boss.transform.position = spawnPosition;
            snapTransform.Id = spawnInfo.Id;
            snapTransform.InitTransform(spawnPosition, null);
            sceneObjects[spawnInfo.Id] = boss.gameObject;
            SyncManager.Instance.AddSnapTransform(snapTransform);
        }


        public void DespawnObject(GalacticKittensObjectDieResponse response)
        {
            if (sceneObjects.Remove(response.Id, out GameObject go))
            {
                var objectDestory = go.GetComponent<IObjectDestory>();
                if (objectDestory != null)
                {
                    objectDestory.Despawn(response);
                }
                else
                {
                    SyncManager.Instance.RemoveSyncObject(response.Id);
                    Destroy(go);
                }
            }
            else
            {
                Debug.Log($"销毁对象 {response.Id} 未找到");
            }

            NetworkStatistics.sceneObjectCount = sceneObjects.Count;
        }

        /// <summary>
        /// 获得场景对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public GameObject GetSceneObject(long id)
        {
            if (sceneObjects.TryGetValue(id, out GameObject go))
            {
                return go;
            }

            return null;
        }


        public void GameFinish()
        {
            SyncManager.Instance.ResetData();
            foreach (var kv in sceneObjects)
            {
                Destroy(kv.Value);
            }
            sceneObjects.Clear();

            SceneManager.LoadScene("GalacticKittensFinish");
        }
        

        /// <summary>
        /// 退出到大厅
        /// </summary>
        public void QuitToLobby()
        {
            SyncManager.Instance.ResetData();
            SceneManager.LoadScene("Lobby");
            Destroy(GalacticKittensAudioManager.Instance);
            Destroy(Instance);
        }
    }
}