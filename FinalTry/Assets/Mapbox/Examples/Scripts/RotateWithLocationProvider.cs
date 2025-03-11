using Mapbox.Unity.Location;
using UnityEngine;

namespace Mapbox.Examples
{
    public class RotateWithLocationProvider : MonoBehaviour
    {
        [SerializeField] private bool _useDeviceOrientation;
        [SerializeField] private bool _subtractUserHeading;
        [SerializeField] private float _rotationFollowFactor = 1;
        [SerializeField] private bool _rotateZ;
        [SerializeField] private bool _useNegativeAngle;
        [SerializeField] private bool _useTransformLocationProvider;

        private Quaternion _targetRotation;
        private ILocationProvider _locationProvider;

        public ILocationProvider LocationProvider
        {
            private get
            {
                if (_locationProvider == null)
                {
                    _locationProvider = _useTransformLocationProvider ?
                        LocationProviderFactory.Instance.TransformLocationProvider :
                        LocationProviderFactory.Instance.DefaultLocationProvider;
                }
                return _locationProvider;
            }
            set
            {
                if (_locationProvider != null)
                {
                    _locationProvider.OnLocationUpdated -= LocationProvider_OnLocationUpdated;
                }
                _locationProvider = value;
                _locationProvider.OnLocationUpdated += LocationProvider_OnLocationUpdated;
            }
        }

        void Start()
        {
            LocationProvider.OnLocationUpdated += LocationProvider_OnLocationUpdated;
        }

        void OnDestroy()
        {
            if (LocationProvider != null)
            {
                LocationProvider.OnLocationUpdated -= LocationProvider_OnLocationUpdated;
            }
        }

        void LocationProvider_OnLocationUpdated(Location location)
        {
            float rotationAngle = _useDeviceOrientation ? location.DeviceOrientation : location.UserHeading;
            if (_useNegativeAngle) { rotationAngle *= -1f; }

            if (_useDeviceOrientation)
            {
                if (_subtractUserHeading)
                {
                    rotationAngle = (rotationAngle - location.UserHeading + 360) % 360;
                }
                _targetRotation = Quaternion.Euler(GetNewEulerAngles(rotationAngle));
            }
            else if (location.IsUserHeadingUpdated)
            {
                _targetRotation = Quaternion.Euler(GetNewEulerAngles(rotationAngle));
            }
        }

        private Vector3 GetNewEulerAngles(float newAngle)
        {
            var euler = new Vector3();
            euler = _rotateZ ? new Vector3(0, 0, -newAngle) : new Vector3(0, -newAngle, 0);
            return euler;
        }

        void Update()
        {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, _targetRotation, Time.deltaTime * _rotationFollowFactor);
        }
    }
}
