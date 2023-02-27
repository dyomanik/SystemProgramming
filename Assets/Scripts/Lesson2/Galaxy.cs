using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

namespace Lesson2
{
    public class Galaxy : MonoBehaviour
    {
        [SerializeField]
        private int _numberOfEntities;
        [SerializeField]
        private float _startDistance;
        [SerializeField]
        private float _startVelocity;
        [SerializeField]
        private float _startMass;
        [SerializeField]
        private float _gravitationModifier;

        [SerializeField]
        private GameObject _celestialBodyPrefab;
        private TransformAccessArray _transformAccessArray;

        private NativeArray<Vector3> _positions;
        private NativeArray<Vector3> _velocities;
        private NativeArray<Vector3> _accelerations;
        private NativeArray<float> _masses;

        private void Start()
        {
            _positions = new NativeArray<Vector3>(_numberOfEntities, Allocator.Persistent);
            _velocities = new NativeArray<Vector3>(_numberOfEntities, Allocator.Persistent);
            _accelerations = new NativeArray<Vector3>(_numberOfEntities, Allocator.Persistent);
            _masses = new NativeArray<float>(_numberOfEntities, Allocator.Persistent);

            Transform[] transforms = new Transform[_numberOfEntities];

            for (int i = 0; i < _numberOfEntities; i++)
            {
                _positions[i] = Random.insideUnitSphere * Random.Range(0, _startDistance);
                _velocities[i] = Random.insideUnitSphere * Random.Range(0, _startVelocity);
                _accelerations[i] = new Vector3();
                _masses[i] = Random.Range(1, _startMass);
                transforms[i] = Instantiate(_celestialBodyPrefab).transform;
            }
            _transformAccessArray = new TransformAccessArray(transforms);
        }

        private void Update()
        {
            GravitationJob gravitationJob = new GravitationJob()
            {
                Positions = _positions,
                Velocities = _velocities,
                Accelerations = _accelerations,
                Masses = _masses,
                GravitationModifier = _gravitationModifier,
                DeltaTime = Time.deltaTime
            };
            JobHandle gravitationHandle = gravitationJob.Schedule(_numberOfEntities, 0);

            MoveJob moveJob = new MoveJob()
            {
                Positions = _positions,
                Velocities = _velocities,
                Accelerations = _accelerations,
                DeltaTime = Time.deltaTime
            };
            JobHandle moveHandle = moveJob.Schedule(_transformAccessArray, gravitationHandle);
            moveHandle.Complete();
        }

        private void OnDestroy()
        {
            _positions.Dispose();
            _velocities.Dispose();
            _accelerations.Dispose();
            _masses.Dispose();
            _transformAccessArray.Dispose();
        }

    }
}