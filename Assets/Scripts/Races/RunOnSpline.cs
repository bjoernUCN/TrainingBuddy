using System;
using UnityEngine;
using UnityEngine.Splines;

namespace TrainingBuddy
{
    public class RunOnSpline : MonoBehaviour
    {
        [field:SerializeField] public SplineContainer SplineContainer { get; set; }
        [field:SerializeField] public float targetMoveSpeed { get; set; }
        [field:SerializeField] public float acceleration { get; set; }
        
        private float currentMoveSpeed;
        private float totalLength;
        private float t;
        private float targetT = 1;

        private void Start()
        {
            totalLength = SplineContainer.CalculateLength();
        }

        private void Update()
        {
            UpdateT();
            UpdateTransform();
        }
        
        private void UpdateT()
        {
            float dt = Time.deltaTime;
            currentMoveSpeed = Mathf.MoveTowards(currentMoveSpeed, targetMoveSpeed, dt * acceleration);
            t = Mathf.MoveTowards(t, targetT, dt * (currentMoveSpeed / totalLength));
            Debug.Log(currentMoveSpeed);
        }

        private void UpdateTransform()
        {
            EvaluatePositionAndRotation(out var position, out var rotation);
            
            transform.position = position;
            transform.rotation = rotation;
        }
        
        private void EvaluatePositionAndRotation(out Vector3 position, out Quaternion rotation)
        {
            position = Vector3.zero;
            rotation = new Quaternion();
            
            position = SplineContainer.EvaluatePosition(t);
                                
            Vector3 forward = Vector3.Normalize(SplineContainer.EvaluateTangent(t));
            Vector3 up = SplineContainer.EvaluateUpVector(t);
            rotation = Quaternion.LookRotation(forward, up);
        }
    }
}
