using UnityEngine;
using Mapbox.Directions;
using System.Collections.Generic;
using Mapbox.Unity.Map;
using Mapbox.Unity.Utilities;
using Mapbox.Unity;
using System;
using Mapbox.Utils;

namespace Mapbox.Examples
{
    public class AstronautDirections : MonoBehaviour
    {
        private AbstractMap _map;
        private Mapbox.Directions.Directions _directions;
        private Action<List<Vector3>> _callback;
        private bool _isMapInitialized = false;

        public bool isMapInitialized => _isMapInitialized;

        void Awake()
        {
            _directions = MapboxAccess.Instance.Directions;
            if (_directions == null)
            {
                Debug.LogError("Directions service not available. Check Mapbox access token.");
            }
            else
            {
                Debug.Log("Directions service available.");
            }

            _map = FindObjectOfType<AbstractMap>();

            if (_map != null)
            {
                _map.OnInitialized += () => 
                {
                    _isMapInitialized = true;
                    Debug.Log("Map initialized.");
                };
            }
            else
            {
                Debug.LogError("AbstractMap not found!");
            }
        }

        public void Query(Action<List<Vector3>> vecs, Vector3 startPosition, Vector3 endPosition)
        {
            Debug.Log("Query called with start: " + startPosition + ", end: " + endPosition);

            if (!_isMapInitialized)
            {
                Debug.LogError("Map is not initialized yet. Cannot query directions.");
                return;
            }

            if (_callback == null)
                _callback = vecs;

            Vector2d[] waypoints = new Vector2d[2];
            waypoints[0] = _map.WorldToGeoPosition(startPosition);
            waypoints[1] = _map.WorldToGeoPosition(endPosition);

            var directionResource = new DirectionResource(waypoints, RoutingProfile.Walking)
            {
                Steps = true
            };

            _directions.Query(directionResource, (response) =>
            {
                if (response != null && response.Routes != null && response.Routes.Count > 0)
                {
                    HandleDirectionsResponse(response);
                }
                else
                {
                    Debug.LogError("Directions request failed or no routes found.");
                }
            });
        }

        void HandleDirectionsResponse(DirectionsResponse response)
        {
            if (response == null || response.Routes == null || response.Routes.Count < 1)
            {
                Debug.LogError("No directions found.");
                return;
            }

            Debug.Log("Directions response received with " + response.Routes[0].Geometry.Count + " points.");

            var routePoints = new List<Vector3>();
            foreach (var point in response.Routes[0].Geometry)
            {
                routePoints.Add(Conversions.GeoToWorldPosition(
                    point.x, point.y, _map.CenterMercator, _map.WorldRelativeScale).ToVector3xz());
            }

            if (_callback != null)
            {
                _callback(routePoints); // Ensure callback is invoked
            }
            else
            {
                Debug.LogWarning("Callback is null, route points not processed.");
            }
        }
    }
}