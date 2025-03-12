namespace Mapbox.Unity.Map
{
    using System.Collections;
    using Mapbox.Unity.Location;
    using UnityEngine;
    using Mapbox.Utils; // Added for Vector2d

    public class InitializeMapWithLocationProvider : MonoBehaviour
    {
        [SerializeField]
        AbstractMap _map;

        ILocationProvider _locationProvider;

        private bool _isMapInitialized = false; // Custom flag to track initialization

        private void Awake()
        {
            // Prevent double initialization of the map
            _map.InitializeOnStart = false;
        }

        protected virtual IEnumerator Start()
        {
            yield return null;
            _locationProvider = LocationProviderFactory.Instance.DefaultLocationProvider;
            Debug.Log("Location Provider: " + _locationProvider.GetType().Name);
            _locationProvider.OnLocationUpdated += LocationProvider_OnLocationUpdated;

            // Fallback: Initialize with a static location if no update is received
            StartCoroutine(CheckLocationTimeout());
        }

        private IEnumerator CheckLocationTimeout()
        {
            yield return new WaitForSeconds(1f); // Wait 5 seconds for location
            if (!_isMapInitialized) // Use custom flag instead of IsInitialized
            {
                Debug.LogWarning("No location update received, initializing with default location.");
                _map.Initialize(new Vector2d(37.7749, -122.4194), _map.AbsoluteZoom); // Default: San Francisco
                _isMapInitialized = true;
            }
        }

        void LocationProvider_OnLocationUpdated(Unity.Location.Location location)
        {
            _locationProvider.OnLocationUpdated -= LocationProvider_OnLocationUpdated;
            Debug.Log("Location updated: " + location.LatitudeLongitude.ToString());
            _map.Initialize(location.LatitudeLongitude, _map.AbsoluteZoom);
            _isMapInitialized = true; // Set flag on successful initialization
        }
    }
}