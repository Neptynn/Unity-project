using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServiceLocatorMain : MonoBehaviour
{
    [SerializeField] private PlayerModel _playerModel;
    [SerializeField] private PlayerVisual _playerVisual;

    private void Awake()
    {
        ServiceLocator.Initialize();

        ServiceLocator.Current.Register<PlayerModel>(_playerModel);
        ServiceLocator.Current.Register<PlayerVisual>(_playerVisual);
    }
} 
