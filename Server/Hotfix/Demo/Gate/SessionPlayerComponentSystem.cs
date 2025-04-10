namespace ET.Server
{
    [EntitySystemOf(typeof(SessionPlayerComponent))]
    public static partial class SessionPlayerComponentSystem
    {
        [EntitySystem]
        private static void Destroy(this SessionPlayerComponent self)
        {
            Scene root = self.Root();
            if (root.IsDisposed)
            {
                return;
            }
            // 发送断线消息
            G2M_SessionDisconnect msg = new G2M_SessionDisconnect();
            root.GetComponent<MessageLocationSenderComponent>().Get(LocationType.Unit).Send(self.Player.Id, msg);
        }
        
        [EntitySystem]
        private static void Awake(this SessionPlayerComponent self)
        {

        }
    }
}