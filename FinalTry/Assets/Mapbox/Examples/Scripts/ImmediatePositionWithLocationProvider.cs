using Mapbox.Unity.Location;
using Mapbox.Unity.Map;
using UnityEngine;

namespace Mapbox.Examples
{
    public class ImmediatePositionWithLocationProvider : MonoBehaviour
    {
        private bool _isInitialized;
        private ILocationProvider _locationProvider;
        private AbstractMap _map;

        ILocationProvider LocationProvider
        {
            get
            {
                if (_locationProvider == null)
                {
                    _locationProvider = LocationProviderFactory.Instance.DefaultLocationProvider;
                }
                return _locationProvider;
            }
        }

        void Start()
        {
            _map = FindObjectOfType<AbstractMap>();
            if (_map == null)
            {
                Debug.LogError("AbstractMap not found!");
                return;
            }

            _map.OnInitialized += () =>
            {
                Debug.Log("Map initialized. Positioning character.");
                _isInitialized = true;
            };
        }

        void LateUpdate()
        {
            if (_isInitialized && _map != null)
            {
                transform.localPosition = _map.GeoToWorldPosition(LocationProvider.CurrentLocation.LatitudeLongitude);
            }
        }
    }
}
