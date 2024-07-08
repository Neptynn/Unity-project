using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomEventBus
{
    /// <summary>
    /// ��������� ��� ������� �� ���������� ����
    /// </summary>
    public interface IDisposable
    {
        /// <summary>
        /// � ���� ������ ���� ���������� �� ���� ������� ���������� ����
        /// </summary>
        public void Dispose();
    }
}