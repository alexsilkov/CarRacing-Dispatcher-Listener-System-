using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI {

    public class RenderManager : MonoBehaviour {

        public static RenderManager Instance;

        [SerializeField]
        private Camera _renderCamera;

        [SerializeField]
        private Transform _rootTransform;

        private RenderTexture _texture;

        private void Awake() {
            if (Instance!=null) {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        public RenderTexture Render(GameObject prefab, Vector3 cameraPosition) {
            var carInstance = Instantiate(prefab,_rootTransform);
            _texture = RenderTexture.GetTemporary(64,64,16);
            _texture.antiAliasing = 8;
            _texture.Create();
            _renderCamera.transform.position = cameraPosition;
            _renderCamera.targetTexture = _texture;
            _renderCamera.Render();
            _renderCamera.targetTexture = null;
            Destroy(carInstance);
            return _texture;
        }

        public void ReleaseTexture() {

            RenderTexture.ReleaseTemporary(_texture);
        }
    }
}

