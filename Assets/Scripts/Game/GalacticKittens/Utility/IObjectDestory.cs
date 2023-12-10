namespace Game.GalacticKittens.Utility
{
    /// <summary>
    /// 对象死亡
    /// </summary>
    public interface IObjectDestory
    {
        public void Despawn(GalacticKittensObjectDieResponse response);
    }
}