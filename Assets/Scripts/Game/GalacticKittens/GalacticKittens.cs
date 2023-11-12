using System;

namespace Game.GalacticKittens
{
    /// <summary>
    /// 游戏数据
    /// </summary>
    public class GalacticKittens
    {
        /**
         * 房间状态 0匹配；1准备；2加载；3游戏中；4完成；5结束
         */
        public uint RoomState { get; set; }
    }


    /// <summary>
    /// 游戏状态
    /// </summary>
    public enum RoomState :uint
    {
        Match=0,
        Prepare=1,
        Load=2,
        Gameing=3,
        Finish=4,
        Close=5,
    }
}