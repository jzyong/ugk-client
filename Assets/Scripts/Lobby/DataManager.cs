using System.Collections.Generic;
using Common.Tools;
using Game.GalacticKittens;

namespace Lobby
{
    /// <summary>
    /// 玩家数据存储
    /// </summary>
    public class DataManager : SingletonInstance<DataManager>
    {
        /// <summary>
        /// 玩家级别信息
        /// </summary>
        public PlayerInfo PlayerInfo { get; set; }
        /// <summary>
        /// 游戏列表
        /// </summary>
        public List<GameInfo> GameList { get; set; }
        
        /// <summary>
        /// GalacticKittens 游戏数据
        /// </summary>
        public GalacticKittens GalacticKittens { get; set; }
    }
}