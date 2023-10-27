using Common.Tools;

namespace Common
{
    /// <summary>
    /// 玩家数据存储
    /// </summary>
    public class DataManager : SingletonInstance<DataManager>
    {
        public PlayerInfo PlayerInfo { get; set; }
    }
}