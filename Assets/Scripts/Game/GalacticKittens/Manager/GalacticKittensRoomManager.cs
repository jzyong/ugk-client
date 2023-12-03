using Common;
using Common.Tools;
using Game.GalacticKittens.Player;
using Network;
using Network.Sync;
using UnityEngine;

namespace Game.GalacticKittens.Manager
{
    /// <summary>
    /// 房间管理
    /// </summary>
    public class GalacticKittensRoomManager : SingletonPersistent<GalacticKittensRoomManager>
    {
        [SerializeField] private Spaceship[] spaceships;


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
                Debug.Log($"{spawnInfo.Id} ConfigID={spawnInfo.ConfigId} 出生");

                switch (spawnInfo.ConfigId)
                {
                    case 1: //玩家
                        SpawnSpaceShip(spawnInfo);
                        break;
                }

                if (spawnInfo.ConfigId == 1)
                {
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
            var spaceship = Instantiate(spaceships[0], null);
            var snapTransform = spaceship.GetComponent<SnapTransform>();
            snapTransform.Id = spawnInfo.Id;
            snapTransform.OnDeserialize(spawnInfo.SyncPayload, true);
            if (snapTransform.Id == DataManager.Singleton.PlayerInfo.PlayerId)
            {
                snapTransform.Onwer = true;
            }
            SyncManager.Instance.AddSnapTransform(snapTransform);
        }
    }
}