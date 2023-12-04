using Common;
using Common.Tools;
using Game.GalacticKittens.Player;
using Lobby;
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
                Debug.Log($"{spawnInfo.Id} ConfigID={spawnInfo.ConfigId} 出生于 {spawnInfo.Position}");

                switch (spawnInfo.ConfigId)
                {
                    case 0: //玩家
                    case 1: //玩家
                    case 2: //玩家
                    case 3: //玩家
                        SpawnSpaceShip(spawnInfo);
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
            var snapTransform = spaceship.GetComponent<SnapTransform>();
            snapTransform.Id = spawnInfo.Id;
            var position = new Vector3(spawnInfo.Position.X, spawnInfo.Position.Y, spawnInfo.Position.Y);
            spaceship.transform.position = position;
            if (snapTransform.Id == DataManager.Instance.PlayerInfo.PlayerId)
            {
                snapTransform.Onwer = true;
            }
            else
            {
                // 其他玩家需要初始化 SnapTransform 的初始坐标
                snapTransform.SetLastDeserializedPositon(position);
            }

            SyncManager.Instance.AddSnapTransform(snapTransform);
            DataManager.Instance.GalacticKittens.Spaceships[snapTransform.Id] = spaceship;
        }
    }
}