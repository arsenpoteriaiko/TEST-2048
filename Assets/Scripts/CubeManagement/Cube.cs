using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using Zenject;

namespace Game2048.CubeManagement
{
    public class Cube : MonoBehaviour
    {
        public static event Action<Cube> Merged;
        public static event Action<Cube> Spawned;
        public static event Action<Cube> Destroyed;

        public int Value { get; private set; }
        public Vector3 Velocity => _body.linearVelocity;
        public Collider Collider => _collider;

        [SerializeField] [Range(0, 100)] private int _chanceToSpawn4;
        [SerializeField] private float _minMergeForce;
        [SerializeField] private float _mergeJumpForce;

        [Header("Other components")]
        [SerializeField] private TextMeshPro[] _valueTextList;
        [SerializeField] private Renderer _renderer;
        [SerializeField] private Collider _collider;
        [SerializeField] private Rigidbody _body;
        [SerializeField] private GameObject _mergeParticles;

        [Inject] private CubeColorPalette _colorPalette;
        private bool _justSpawned = true;

        public void Setup(int value)
        {
            if (value == -1)
                Value = UnityEngine.Random.Range(0, 100) < _chanceToSpawn4 ? 4 : 2;
            else if (!Mathf.IsPowerOfTwo(value))
                throw new ArgumentException("Value must be power of 2");
            else
                Value = value;

            UpdateText();
            UpdateColor();

            if (_justSpawned)
            {
                _justSpawned = false;
                Spawned?.Invoke(this);
            }
        }

        private void UpdateText()
        {
            string newValue = Value.ToString();

            foreach (var textObject in _valueTextList)
                textObject.text = newValue;
        }

        private void UpdateColor()
        {
            var propBlock = new MaterialPropertyBlock();

            _renderer.GetPropertyBlock(propBlock);
            propBlock.SetColor("_BaseColor", _colorPalette.GetColor(Value));
            _renderer.SetPropertyBlock(propBlock);
        }

        public void Push(Vector3 direction, float force)
        {
            _body.AddForce(direction * force, ForceMode.Impulse);
        }

        private void OnCollisionEnter(Collision collision)
        {
            Cube anotherCube;
            if (!collision.transform.TryGetComponent(out anotherCube)) return;

            if (Value == anotherCube.Value && collision.impulse.magnitude > _minMergeForce && Velocity.magnitude < anotherCube.Velocity.magnitude)
            {
                Destroy(anotherCube.gameObject);
                Merge();
            }
        }

        private void Merge()
        {
            Value *= 2;
            Push(new Vector3(0, 1, 0), _mergeJumpForce);

            Merged?.Invoke(this);
            UpdateText();
            UpdateColor();
            PlayParticles();
        }

        public void SetPhysics(bool condition)
        {
            _body.isKinematic = !condition;
        }

        public void PlayParticles()
        {
            Instantiate(_mergeParticles, transform.position, Quaternion.identity);
        }

        private void OnDestroy()
        {
            Destroyed?.Invoke(this);
        }
    }
}
