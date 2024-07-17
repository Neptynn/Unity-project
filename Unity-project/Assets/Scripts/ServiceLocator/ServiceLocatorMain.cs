using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomEventBus;

public class ServiceLocatorMain : MonoBehaviour
{
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private PlayerVisual _playerVisual;
    private EventBus _eventBus;
    private void Awake()
    {
        ServiceLocator.Initialize();

        _eventBus = new EventBus();
        ServiceLocator.Current.Register<PlayerMovement>(_playerMovement);
        ServiceLocator.Current.Register<PlayerVisual>(_playerVisual);

        RegisterServices();
    }

    private void RegisterServices()
    {
        ServiceLocator.Initialize();

        ServiceLocator.Current.Register(_eventBus);
        ServiceLocator.Current.Register(_playerMovement);
        ServiceLocator.Current.Register(_playerVisual);
    }

    private void Init()
    {

    }
}

