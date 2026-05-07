using System.Collections.Generic;
using Meta.XR.MRUtilityKit;
using UnityEngine;

public class RoomObjectSpawner : MonoBehaviour
{
    [Header("Spawn")]
    [SerializeField] private GameObject objectToSpawn;
    [SerializeField] private MRUK.RoomFilter spawnOnStart = MRUK.RoomFilter.CurrentRoomOnly;
    [SerializeField] private bool clearBeforeSpawn = true;

    [Header("Door placement")]
    [SerializeField] private float forwardOffset = 0.05f;
    [SerializeField] private float heightOffset = 0f;
    [SerializeField] private bool faceAwayFromDoor = true;

    private readonly List<GameObject> spawnedObjects = new();

    public IReadOnlyList<GameObject> SpawnedObjects => spawnedObjects;

    private void Start()
    {
        if (MRUK.Instance == null)
        {
            Debug.LogWarning($"{nameof(RoomObjectSpawner)} requires an MRUK instance in the scene.");
            return;
        }

        MRUK.Instance.RegisterSceneLoadedCallback(SpawnAfterSceneLoaded);
        MRUK.Instance.RoomCreatedEvent.AddListener(SpawnInNewRoom);
    }

    public void SpawnAfterSceneLoaded()
    {
        switch (spawnOnStart)
        {
            case MRUK.RoomFilter.AllRooms:
                SpawnInAllRooms();
                break;
            case MRUK.RoomFilter.CurrentRoomOnly:
                SpawnInCurrentRoom();
                break;
        }
    }

    public void SpawnInAllRooms()
    {
        if (clearBeforeSpawn)
        {
            ClearSpawnedObjects();
        }

        foreach (var room in MRUK.Instance.Rooms)
        {
            SpawnInRoom(room, false);
        }
    }

    public void SpawnInCurrentRoom()
    {
        SpawnInRoom(MRUK.Instance.GetCurrentRoom(), clearBeforeSpawn);
    }

    public void SpawnInRoom(MRUKRoom room, bool clearFirst = true)
    {
        if (objectToSpawn == null)
        {
            Debug.LogWarning($"{nameof(RoomObjectSpawner)} has no object to spawn assigned.");
            return;
        }

        if (room == null)
        {
            Debug.LogWarning($"{nameof(RoomObjectSpawner)} could not find a room to spawn in.");
            return;
        }

        if (clearFirst)
        {
            ClearSpawnedObjects();
        }

        var spawnedCount = 0;
        foreach (var anchor in room.Anchors)
        {
            if (anchor == null || !anchor.HasAnyLabel(MRUKAnchor.SceneLabels.DOOR_FRAME))
            {
                continue;
            }

            SpawnOnDoor(anchor);
            spawnedCount++;
        }

        if (spawnedCount == 0)
        {
            Debug.LogWarning($"{nameof(RoomObjectSpawner)} did not find any door frame anchors in room {room.name}.");
        }
    }

    public void ClearSpawnedObjects()
    {
        for (var i = spawnedObjects.Count - 1; i >= 0; i--)
        {
            if (spawnedObjects[i] != null)
            {
                Destroy(spawnedObjects[i]);
            }
        }

        spawnedObjects.Clear();
    }

    private void SpawnInNewRoom(MRUKRoom room)
    {
        if (spawnOnStart == MRUK.RoomFilter.AllRooms)
        {
            SpawnInRoom(room, false);
        }
    }

    private void SpawnOnDoor(MRUKAnchor doorAnchor)
    {
        var doorNormal = doorAnchor.transform.forward.normalized;
        var spawnPosition = doorAnchor.GetAnchorCenter() + Vector3.up * heightOffset + doorNormal * forwardOffset;
        var forward = faceAwayFromDoor ? doorNormal : -doorNormal;
        var spawnRotation = Quaternion.LookRotation(ProjectOnHorizontalPlane(forward), Vector3.up);

        var spawnedObject = Instantiate(objectToSpawn, spawnPosition, spawnRotation, transform);
        spawnedObjects.Add(spawnedObject);
    }

    private static Vector3 ProjectOnHorizontalPlane(Vector3 direction)
    {
        direction.y = 0f;
        if (direction.sqrMagnitude < 0.001f)
        {
            return Vector3.forward;
        }

        return direction.normalized;
    }

    private void OnDestroy()
    {
        if (MRUK.Instance == null)
        {
            return;
        }

        MRUK.Instance.SceneLoadedEvent.RemoveListener(SpawnAfterSceneLoaded);
        MRUK.Instance.RoomCreatedEvent.RemoveListener(SpawnInNewRoom);
    }
}
