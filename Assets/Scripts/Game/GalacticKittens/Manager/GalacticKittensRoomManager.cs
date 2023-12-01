using System;
using Common.Tools;
using Network;

namespace Game.GalacticKittens
{
    /// <summary>
    /// 房间管理
    /// </summary>
    public class GalacticKittensRoomManager : SingletonPersistent<GalacticKittensRoomManager>
    {
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
                //TODO 创建玩家或敌人对象，并且添加同步组件
            }
        }
    }
}