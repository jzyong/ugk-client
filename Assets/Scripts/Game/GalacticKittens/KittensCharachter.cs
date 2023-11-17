using Network.Sync;

namespace Game.GalacticKittens
{
    /// <summary>
    /// Kittens角色
    /// </summary>
    public class KittensCharacter : NetworkTransform
    {
        
        public long Id { set; get; }
        
        protected override void SyncTransform()
        {
            //TODO 发送同步消息

        }
    }
}