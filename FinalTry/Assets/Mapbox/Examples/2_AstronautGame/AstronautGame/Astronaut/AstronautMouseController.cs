using UnityEngine;
using Mapbox.Unity.Map;
using Mapbox.Unity.Utilities;
using System.Collections.Generic;

namespace Mapbox.Examples
{
    public class AstronautMouseController : MonoBehaviour
    {
        [Header("Character")]
        [SerializeField] GameObject character;
        [SerializeField] float characterSpeed = 5f;
        [SerializeField] Animator characterAnimator;

        [Header("References")]
        [SerializeField] AstronautDirections directions;
        [SerializeField] Transform startPoint;
        [SerializeField] Transform endPoint;
        [SerializeField] AbstractMap map;
        [SerializeField] GameObject rayPlane; // Plane for raycasting
        [SerializeField] Transform _movementEndPoint;

        [SerializeField] LayerMask layerMask; // Set to "Map" layer in Inspector

        Ray ray;
        RaycastHit hit;
        bool moving;
        bool characterDisabled;
        bool interruption;
        private Vector3 nextPos; // Moved to class level to fix scope issue

        void Update()
        {
            if (characterDisabled || directions == null || !directions.isMapInitialized)
            {
                Debug.LogWarning("Movement blocked: Check character, directions, or map initialization.");
                return;
            }

            // Use GetMouseButtonUp for reliable WebGL click detection
            if (Input.GetMouseButtonUp(0))
            {
                Debug.Log("Mouse clicked, casting ray.");
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Debug.DrawRay(ray.origin, ray.direction * 1000f, Color.red, 5f);

                if (Physics.Raycast(ray, out hit, 1000f, layerMask))
                {
                    Debug.Log("Raycast hit at: " + hit.point);
                    startPoint.position = transform.localPosition;
                    endPoint.position = hit.point;
                    _movementEndPoint.position = new Vector3(hit.point.x, 0.2f, hit.point.z);
                    _movementEndPoint.gameObject.SetActive(true);

                    directions.Query(GetPositions, startPoint.position, endPoint.position);
                    Debug.Log("Requesting directions from API.");
                }
                else
                {
                    Debug.LogWarning("Raycast missed. Check layerMask, rayPlane position, and camera setup.");
                }
            }
        }

        #region Movement Logic
        List<Vector3> futurePositions;

        void GetPositions(List<Vector3> vecs)
        {
            Debug.Log("Received " + (vecs != null ? vecs.Count : 0) + " waypoints.");
            futurePositions = vecs;

            if (futurePositions != null && futurePositions.Count > 0 && !moving)
            {
                MoveToNextPlace();
            }
        }

        void MoveToNextPlace()
        {
            if (futurePositions != null && futurePositions.Count > 0)
            {
                nextPos = futurePositions[0]; // Assign to class-level variable
                futurePositions.RemoveAt(0);
                moving = true;
                interruption = false; // Reset interruption flag from original script
                StartCoroutine(MoveTo(nextPos));
            }
            else
            {
                moving = false;
                if (characterAnimator != null) characterAnimator.SetBool("IsWalking", false);
                _movementEndPoint.gameObject.SetActive(false);
            }
        }

        System.Collections.IEnumerator MoveTo(Vector3 nextPosParam)
        {
            Vector3 prevPos = transform.localPosition;

            // Set initial rotation to face the current waypoint (from original script)
            Vector3 direction = (nextPosParam - transform.localPosition).normalized;
            direction.y = 0;
            if (direction != Vector3.zero && characterAnimator != null)
            {
                transform.rotation = Quaternion.LookRotation(direction);
            }

            float time = CalculateTime();
            float t = 0;

            // Start walking animation when movement begins
            if (characterAnimator != null) characterAnimator.SetBool("IsWalking", true);

            // Move to the current waypoint
            while (t < 1 && !interruption)
            {
                t += Time.deltaTime / time;
                transform.localPosition = Vector3.Lerp(prevPos, nextPosParam, t);
                yield return null;
            }

            // If interrupted, stop moving and reset animation
            if (interruption)
            {
                interruption = false;
                moving = false;
                if (characterAnimator != null) characterAnimator.SetBool("IsWalking", false);
                yield break;
            }

            // Check if there are more waypoints and rotate to face the next one (from original script)
            if (futurePositions != null && futurePositions.Count > 0)
            {
                Vector3 nextWaypoint = futurePositions[0];
                yield return StartCoroutine(RotateToFace(nextWaypoint));
            }

            MoveToNextPlace();
        }

        System.Collections.IEnumerator RotateToFace(Vector3 targetPosition)
        {
            Vector3 direction = (targetPosition - transform.localPosition).normalized;
            direction.y = 0;
            Quaternion targetRotation = direction != Vector3.zero ? Quaternion.LookRotation(direction) : transform.rotation;
            Quaternion startRotation = transform.rotation;

            float t = 0;
            while (t < 1)
            {
                t += Time.deltaTime / 0.1f; // Adjust 0.1f for rotation speed (from original script)
                transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);
                yield return null;
            }
        }

        float CalculateTime()
        {
            return Vector3.Distance(transform.localPosition, nextPos) / characterSpeed; // Use class-level nextPos
        }
        #endregion

        #region Utility
        public void DisableCharacter()
        {
            characterDisabled = true;
            moving = false;
            StopAllCoroutines();
            if (characterAnimator != null) characterAnimator.SetBool("IsWalking", false);
            character.SetActive(false);
        }

        public void EnableCharacter()
        {
            characterDisabled = false;
            character.SetActive(true);
        }
        #endregion
    }
}