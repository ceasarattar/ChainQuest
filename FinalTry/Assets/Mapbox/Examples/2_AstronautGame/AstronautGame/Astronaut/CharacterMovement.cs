using UnityEngine;
using Mapbox.Unity.Map;
using Mapbox.Unity.Utilities;

namespace Mapbox.Examples
{
    public class CharacterMovement : MonoBehaviour
    {
        public Material[] Materials;
        public Transform Target;
        public Animator CharacterAnimator;
        public float Speed = 5f;
        private AstronautMouseController _controller;
        private AbstractMap _map;
        private bool _mapLoaded = false;

        void Start()
        {
            _controller = GetComponent<AstronautMouseController>();
            _map = FindObjectOfType<AbstractMap>();

            if (_map != null)
            {
                _map.OnInitialized += () =>
                {
                    Debug.Log("Mapbox map initialized.");
                    _mapLoaded = true;
                };
            }
            else
            {
                Debug.LogError("AbstractMap not found! Character movement may not work properly.");
            }

            if (Target == null)
            {
                Debug.LogError("Target is NULL! Assign a target in the Inspector.");
            }
        }

        void Update()
        {
            if (_controller != null && _controller.enabled)
            {
                return;
            }

            foreach (var item in Materials)
            {
                item.SetVector("_CharacterPosition", transform.position);
            }

            if (Vector3.Distance(transform.position, Target.position) > 0.1f)
            {
                transform.LookAt(Target.position);
                transform.position = Vector3.MoveTowards(transform.position, Target.position, Speed * Time.deltaTime);
                CharacterAnimator.SetBool("IsWalking", true);
            }
            else
            {
                CharacterAnimator.SetBool("IsWalking", false);
            }
        }

        public void MoveToPosition(Vector3 position)
        {
            if (!_mapLoaded)
            {
                Debug.LogWarning("Map is not loaded yet. Cannot move character.");
                return;
            }

            Target.position = position;
            Debug.Log("Target position set to: " + position);
        }
    }
}