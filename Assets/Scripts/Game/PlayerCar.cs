using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Events;
using Audio;

namespace Game {

    public class PlayerCar : Car {

        [SerializeField]
        private EventListener _touchEventListener;

        [SerializeField]
        private ScriptableIntValue _touchSide;

        [SerializeField]
        private ScriptableFloatValue _roadWidth;

        [SerializeField]
        private ScriptableFloatValue _playerPositionZ;

        [SerializeField]
        private float _dodgeDuration;

        [SerializeField]
        private GameObject _leftLight;

        [SerializeField]
        private GameObject _rightLight;

        [SerializeField]
        private GameObject _rightBackLight;

        [SerializeField]
        private GameObject _leftBackLight;

        [SerializeField]
        private Color _gizmosColor = Color.white;

        private MusicManager _musicManager;
        private int _currentRoad;
        private bool _inDodge;


        private void Awake() {
            _musicManager = GameObject.FindObjectOfType<MusicManager>();
            if (PlayerPrefs.GetInt("SavedLight") == 0) {
                _leftLight.SetActive(false);
                _rightLight.SetActive(false);
                _rightBackLight.SetActive(false);
                _leftBackLight.SetActive(false);
            }
            else {
                _leftLight.SetActive(true);
                _rightLight.SetActive(true);
                _rightBackLight.SetActive(true);
                _leftBackLight.SetActive(true);
            }
        }
        protected override void SubscribeToEvents() {

            base.SubscribeToEvents();
            _touchEventListener.OnEventHappened += OnPlayerTouch;
        }

        protected override void UnsubscribeToEvents() {

            base.UnsubscribeToEvents();
            _touchEventListener.OnEventHappened -= OnPlayerTouch;
            _musicManager.PlayCrashSound();

        }

        protected override void Move() {

            base.Move();
            _playerPositionZ.value = transform.position.z;
        }

        private void OnPlayerTouch() {

            var nextRoad = Mathf.Clamp(_currentRoad + _touchSide.value, -1, 1);
            var canDodge = !_inDodge && _carrentSpeed >= _carSettings.maxSpeed && nextRoad!=_currentRoad;
            if (!canDodge) {
                return;
            }

            StartCoroutine(DodgeCoroutine(nextRoad));
            _musicManager.PlayDodgeSound();
            
        }

        private IEnumerator DodgeCoroutine(int nextRoad) {

            _inDodge = true;
            var timer = 0f;
            var targetPosX = transform.position.x + _roadWidth.value * (nextRoad > _currentRoad ? 1 : -1);

            while (timer <= _dodgeDuration) {
                timer += Time.deltaTime;
                var posX = Mathf.Lerp(transform.position.x, targetPosX, timer / _dodgeDuration);
                transform.position = new Vector3(posX, transform.position.y, transform.position.z);
                yield return null;
            }

            _inDodge = false;
            _currentRoad = nextRoad;
            
        }

        private void OnDrawGizmos() {
            //Gizmos.color = Color.red;

            //Gizmos.DrawSphere(transform.position, 5f);
        }

        private void OnDrawGizmosSelected() {

            //Gizmos.color = _gizmosColor;

            //Gizmos.DrawWireSphere(transform.position, 5f);
            //Gizmos.DrawIcon(transform.position + Vector3.up * 4f, "car_gizmo");
            //Gizmos.DrawFrustum(transform.position + transform.forward * 2, 45f, 15f, 50f, .5f);
            //var mesh = GetComponent<MeshFilter>().sharedMesh;
            //Gizmos.DrawWireMesh(mesh,0,transform.position + transform.forward*5);
        }
    }
}

