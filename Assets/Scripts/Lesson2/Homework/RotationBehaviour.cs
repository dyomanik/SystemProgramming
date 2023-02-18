using System.Collections;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

namespace Lesson2.Homework
{
    public class RotationBehaviour : MonoBehaviour
    {
        [SerializeField] private GameObject _prefab;
        [SerializeField] private Vector3 _direction;
        [SerializeField] private int _numberOfObjects;
        [SerializeField] private float _spawnRadius;
        [SerializeField] private float _multiplayerOfRotation;

        private NativeArray<Color> _colorsInput;
        private NativeArray<Color> _colorsOutput;
        private TransformAccessArray _transformAccessArray;
        private NativeArray<int> _angles;
        private Material[] _materials;

        // Start is called before the first frame update
        void Start()
        {
            _materials = new Material[_numberOfObjects];
            _colorsInput = new NativeArray<Color>(_numberOfObjects, Allocator.Persistent);
            _colorsOutput = new NativeArray<Color>(_numberOfObjects, Allocator.Persistent);
            _angles = new NativeArray<int>(_numberOfObjects, Allocator.Persistent);
            _transformAccessArray = new TransformAccessArray(CreateObjects(_prefab, _numberOfObjects, _spawnRadius));
            for (var i = 0; i < _numberOfObjects; i++)
            {
                _colorsInput[i] = Random.ColorHSV();
                _colorsOutput[i] = Random.ColorHSV();
                _angles[i] = Random.Range(0, 180);
            }
            StartCoroutine(ChangeDirection());
        }

        IEnumerator ChangeDirection()
        {
            while (true)
            {
                ShuffleJob shuffleJob = new ShuffleJob()
                {
                    Colors= _colorsInput,
                    Seed = (uint)(UnityEngine.Random.Range(0.01f, 1f) * _numberOfObjects)
                };

                JobHandle jobHandle = shuffleJob.Schedule();
                jobHandle.Complete();
                yield return new WaitForSeconds(1f);

            }
        }
        // Update is called once per frame
        void Update()
        {
            RotationJob rotationJob = new RotationJob()
            {
                Direction = _direction,
                DeltaTime = Time.deltaTime,
                Random = Random.Range(-1f, 1f),
                ColorInput = _colorsInput,
                ColorOutput = _colorsOutput,
                Angles = _angles,
                Multiplayer = _multiplayerOfRotation
            };

            JobHandle jobHandle = rotationJob.Schedule(_transformAccessArray);
            jobHandle.Complete();

            for (var i = 0; i < _numberOfObjects; i++)
            {
                _materials[i].color = _colorsOutput[i];
            }
        }

        private void OnDestroy()
        {
            if (_transformAccessArray.isCreated)
            {
                _transformAccessArray.Dispose();
                _colorsInput.Dispose();
                _colorsOutput.Dispose();
                _angles.Dispose();
            }
        }

        private Transform[] CreateObjects(GameObject prefab, int count, float spawnRadius)
        {
            Transform[] objects = new Transform[count];

            for (var i = 0; i < count; i++)
            {
                objects[i] = Instantiate(prefab).transform;
                _materials[i] = objects[i].gameObject.GetComponent<MeshRenderer>().material;
                objects[i].position = Random.insideUnitSphere * spawnRadius;
            }
            return objects;
        }

        public NativeArray<Color> Shuffle(NativeArray<Color> colors)
        {
            for (var i = 0; i < colors.Length; i++)
            {
                Color tempColor;
                var random = Random.Range(0, colors.Length);
                tempColor = colors[random];
                colors[random] = colors[i];
                colors[i] = tempColor;
            }
            return colors;
        }
    }
}