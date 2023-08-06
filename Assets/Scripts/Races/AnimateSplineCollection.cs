using System.Collections.Generic;
using System.Linq;
using BedtimeCore;
using BedtimeCore.Utility;
using UnityEngine;
using UnityEngine.Splines;

namespace Arkenheim.Quest.Escort
{
	public sealed partial class AnimateSplineCollection : MonoBehaviour
	{
        public enum TravelMode
        {
            OnSpline,
            BetweenSplines,
        }

        [Header("Setup")]
        [SerializeField] private List<SplineContainer> splines;
        [SerializeField] private Transform transformToMove;
        private TravelMode _currentMode;
        
        [Header("Movement")]
        [SerializeField] private float maxSpeed = 6.25f;
        [SerializeField] private float acceleration = 3f;
        [SerializeField] private float decceleration = 10f;
        
        [Header("Others")]
        [SerializeField] private bool initializeOnAwake = true;
        [SerializeField] private bool startOnFirstSpline = true;
        
        private SplineContainer _currentSpline;
        private SplineContainer _previousSpline;
        private BezierKnot _previousSplineLastPoint;
        private BezierKnot _currentSplineFirstPoint;
        private Vector3 _betweenSplinesStartPos;
        private Vector3 _betweenSplinesEndPos;
        private Quaternion _betweenSplinesStartRotation;
        private Quaternion _betweenSplinesEndRotation;
        private int _currentSplineIndex;
        private float _totalLength;
        private float _currentMoveSpeed;
        private float _targetMoveSpeed;
        private float _currentAcceleration;
        private float _t;
        private float _targetT;
        private bool _areWeTravelingReverse;
        
        //For Debug
        private float _originalMaxSpeed;
        private float _originalAcceleration;
        private float _originalDecceleration;

        [GenerateEvent] private readonly SafeAction<SplineContainer> _currentSplineEndReached = new();
        [GenerateEvent] private readonly SafeAction<SplineContainer> _lastSplineEndReached = new();
        [GenerateEvent] private readonly SafeAction<SplineContainer, float, bool> _splineTChanged = new();     
        
        private bool IsLastSpline => _currentSplineIndex == splines.Count - 1;

        public bool IsMoving => _currentMoveSpeed > 0;
        
        public List<SplineContainer> Splines
        {
            get => splines;
            set => splines = value;
        }

        public SplineContainer CurrentSpline => _currentSpline;

        public float MaxSpeed => maxSpeed;

        public bool CalculateAreWeTravelingReverse()
        {
            var distanceToT0 = Vector3.Distance(_currentSpline.EvaluatePosition(0f), transformToMove.position);
            var distanceToT1 = Vector3.Distance(_currentSpline.EvaluatePosition(1f), transformToMove.position);

            return distanceToT1 < distanceToT0;
        }
        
        [GenerateEvent]
        private readonly SafeAction<SplineContainer> _routeStarted = new();

        

        private async void Awake()
        {
            _originalMaxSpeed = maxSpeed;
            _originalAcceleration = acceleration;
            _originalDecceleration = decceleration;
            
            await SessionManager.Instance.SessionStartedCallback();

            if (initializeOnAwake)
            {
                Initialize();
            }
        }

        public void Initialize()
        {
            if (splines.IsNullOrEmpty())
            {
                $"No splines asssigned".LogError(this);
                return;
            }

            GetNextDestination();
            
            if (startOnFirstSpline)
            {
                _currentMode = TravelMode.OnSpline;
                UpdateTransform();
            }
            else
            {
                _currentMode = TravelMode.BetweenSplines;
            }
        }

        private void GetNextDestination()
        {
            _t = 0;
            _targetT = 1;

            switch (_currentMode)
            {
                case TravelMode.OnSpline:
                    if (_currentSpline == null)
                    {
                        _currentSpline = splines.First();
                        _currentSplineIndex = 0;
                    }
                    else
                    {
                        _currentSplineIndex++;
                        _currentSpline = splines[_currentSplineIndex];
                    }
                    _totalLength = _currentSpline.CalculateLength();

                    _areWeTravelingReverse = CalculateAreWeTravelingReverse();
                    if (_areWeTravelingReverse)
                    {
                        _t = 1;
                        _targetT = 0;
                    }
                
                    _routeStarted.Invoke(_currentSpline);
                    break;

                case TravelMode.BetweenSplines:
                    _betweenSplinesStartPos = transformToMove.position;
                    _betweenSplinesStartRotation = transformToMove.rotation;
                    var nextSpline = splines[_currentSplineIndex + 1];

                    FindNearestEndPointOnSpline(nextSpline, out Vector3 pos, out Quaternion rot);
                    _betweenSplinesEndPos = pos;
                    _betweenSplinesEndRotation = rot;
                
                    _totalLength = Vector3.Distance(_betweenSplinesStartPos, _betweenSplinesEndPos);
                    break;
            }
        }



        private void FindNearestEndPointOnSpline(SplineContainer spline, out Vector3 position, out Quaternion rotation)
        {
            Vector3 t0Pos = spline.EvaluatePosition(0);
            Vector3 t1Pos = spline.EvaluatePosition(1);

            var wagonPos = transformToMove.position;
            bool t0PosIsCloser = Vector3.Distance(t0Pos, wagonPos) < Vector3.Distance(t1Pos, wagonPos);
            position = t0PosIsCloser ? t0Pos : t1Pos;
            
            Vector3 forward = Vector3.Normalize(spline.EvaluateTangent(t0PosIsCloser ? 0f : 1f));
            Vector3 up = spline.EvaluateUpVector(t0PosIsCloser ? 0f : 1f);
            rotation = Quaternion.LookRotation(t0PosIsCloser ? forward : -forward, up);
        }

        [ButtonGroup("Move Controls")]
        public void StartMoving()
        {
            StartMoving(maxSpeed);
        }

        public void StartMoving(float speed)
        {
            Updater.UpdateEvent += UpdateSplineAnimate;
            
            _targetMoveSpeed = speed;
            _currentAcceleration = acceleration;
        }
        
        [ButtonGroup("Move Controls")]
        public void StopMoving()
        {
            _targetMoveSpeed = 0f;
            _currentAcceleration = decceleration;
        }

        private void UpdateSplineAnimate()
        {
            UpdateT();
            UpdateTransform();

            if (_t == _targetT || _currentMoveSpeed == 0f)
            {
                Updater.UpdateEvent -= UpdateSplineAnimate;
            }
        }

        private void UpdateT()
        {
            float prevT = _t;
            
            float dt = Time.deltaTime;
            _currentMoveSpeed = Mathf.MoveTowards(_currentMoveSpeed, _targetMoveSpeed, dt * _currentAcceleration);
            _t = Mathf.MoveTowards(_t, _targetT, dt * (_currentMoveSpeed / _totalLength));
            
            if (_t != prevT)
            {
                _splineTChanged.Invoke(_currentSpline, _t, _areWeTravelingReverse);
            
                if (_t == _targetT)
                {
                    if (_currentMode == TravelMode.OnSpline)
                    {
                        if (IsLastSpline)
                        {
                            _lastSplineEndReached.Invoke(_currentSpline);
                        }
                        else
                        {
                            _currentMode = TravelMode.BetweenSplines;
                            _currentSplineEndReached.Invoke(_currentSpline);
                            GetNextDestination();
                        }
                    }
                    else
                    {
                        _currentMode = TravelMode.OnSpline;
                        GetNextDestination();
                    }
                }
            }
        }

        private void UpdateTransform()
        {
            EvaluatePositionAndRotation(out var position, out var rotation);
            
            transformToMove.position = position;
            transformToMove.rotation = rotation;
        }
        
        private void EvaluatePositionAndRotation(out Vector3 position, out Quaternion rotation)
        {
            position = Vector3.zero;
            rotation = new Quaternion();
            
            switch (_currentMode)
            {
                case TravelMode.OnSpline:
                    position = _currentSpline.EvaluatePosition(_t);
                    
                    Vector3 forward = Vector3.Normalize(_currentSpline.EvaluateTangent(_t));
                    Vector3 up = _currentSpline.EvaluateUpVector(_t);
                    rotation = Quaternion.LookRotation(_areWeTravelingReverse ? -forward : forward, up);
                    return;
                case TravelMode.BetweenSplines:
                    position = Vector3.Lerp(_betweenSplinesStartPos, _betweenSplinesEndPos, _t);
                    rotation = Quaternion.Lerp(_betweenSplinesStartRotation, _betweenSplinesEndRotation, _t);
                    return;
            }
        }


        public void ToggleSpeedDebug(bool state)
        {
            maxSpeed = state ? 30f : _originalMaxSpeed;
            acceleration = state ? 1000f : _originalAcceleration;
            decceleration = state ? 1000f : _originalDecceleration;
        }
    }
}