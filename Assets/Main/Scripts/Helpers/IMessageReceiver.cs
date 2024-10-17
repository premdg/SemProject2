using System;

namespace TurryWoods
{
    public interface IMessageReceiver
    {
        public enum MessageType
        {
            DAMAGED, DEAD
        }
        void OnRecieveMessage(MessageType type, object sender, object message);
    }
}

