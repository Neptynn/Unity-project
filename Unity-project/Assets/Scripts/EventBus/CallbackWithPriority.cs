using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CustomEventBus
{
    public class CallbackWithPriority
    {
        /// <summary>
        /// ��� ���� Priority, ��� ������ ��������� �����
        /// </summary>
        public readonly int Priority;
        public readonly object Callback;

        public CallbackWithPriority(int priority, object callback)
        {
            Priority = priority;
            Callback = callback;
        }
    }
}