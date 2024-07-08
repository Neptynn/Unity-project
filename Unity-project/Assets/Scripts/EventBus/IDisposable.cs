using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomEventBus
{
    /// <summary>
    /// Интерфейс для отписки от сигнальной шины
    /// </summary>
    public interface IDisposable
    {
        /// <summary>
        /// В этом методе надо отписаться от всех ивентов сигнальной шины
        /// </summary>
        public void Dispose();
    }
}