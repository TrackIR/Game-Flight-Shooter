using UnityEngine;
using System.Collections.Generic;

public class GhostBoundary : MonoBehaviour
{
    [Header("Ghost Settings")]
    public int ghostCount = 1;
    public float renderDistance = 250f;
    
    private Vector3 boxSize;
    private List<GhostData> ghosts = new List<GhostData>();

    private class GhostData
    {
        public GameObject obj;
        public Vector3 direction;
    }

    void Start()
    {
        Boundary boundary = FindFirstObjectByType<Boundary>();
        if (boundary != null)
        {
            boxSize = boundary.boxSize;
        }
        else
        {
            boxSize = new Vector3(400f, 400f, 400f);
        }

        CreateGhosts();
    }

    void CreateGhosts()
    {
        Vector3[] directions = 
        {
            Vector3.right, Vector3.left,
            Vector3.up, Vector3.down,
            Vector3.forward, Vector3.back
        };

        Mesh asteroidMesh = null;
        Material asteroidMaterial = null;

        MeshFilter[] childMF = GetComponentsInChildren<MeshFilter>();
        MeshRenderer[] childMR = GetComponentsInChildren<MeshRenderer>();
        
        foreach (var filter in childMF)
        {
            if (filter.sharedMesh != null)
            {
                asteroidMesh = filter.sharedMesh;
                break;
            }
        }
        
        foreach (var renderer in childMR)
        {
            if (renderer.sharedMaterial != null)
            {
                asteroidMaterial = renderer.sharedMaterial;
                break;
            }
        }

        foreach (Vector3 dir in directions)
        {
            for (int i = 0; i < ghostCount; i++)
            {
                GameObject ghost = new GameObject($"Ghost_{dir}_{i}");
                
                MeshFilter ghostMF = ghost.AddComponent<MeshFilter>();
                ghostMF.mesh = asteroidMesh;

                MeshRenderer ghostMR = ghost.AddComponent<MeshRenderer>();
                ghostMR.material = new Material(asteroidMaterial);
                // ghostMR.material.color = Color.red;         // for testing ghosts

                GhostData data = new GhostData { obj = ghost, direction = dir };
                ghosts.Add(data);
            }
        }
    }

    void Update()
    {
        Vector3 halfSize = boxSize * 0.5f;
        Vector3 pos = transform.position;
        Quaternion rot = transform.rotation;
        Vector3 scale = transform.localScale;

        Quaternion rotationOffset = Quaternion.Euler(270, 0, 0);

        foreach (var ghost in ghosts)
        {
            Vector3 dir = ghost.direction;

            Quaternion adjustedRot = rot * rotationOffset;      // adjust ghost rotation to be accurate

            ghost.obj.transform.rotation = adjustedRot;
            ghost.obj.transform.localScale = scale;
        
            // Calculate the ghost position
            Vector3 targetPos = pos;
            
            if (dir == Vector3.right)
            {
                // Object is near right edge, ghost goes near left edge (outside boundary)
                float distFromEdge = halfSize.x - pos.x;
                targetPos.x = -halfSize.x - distFromEdge;
            }
            else if (dir == Vector3.left)
            {
                // Object is near left edge, ghost goes near right edge (outside boundary)
                float distFromEdge = pos.x + halfSize.x;
                targetPos.x = halfSize.x + distFromEdge;
            }
            else if (dir == Vector3.up)
            {
                float distFromEdge = halfSize.y - pos.y;
                targetPos.y = -halfSize.y - distFromEdge;
            }
            else if (dir == Vector3.down)
            {
                float distFromEdge = pos.y + halfSize.y;
                targetPos.y = halfSize.y + distFromEdge;
            }
            else if (dir == Vector3.forward)
            {
                float distFromEdge = halfSize.z - pos.z;
                targetPos.z = -halfSize.z - distFromEdge;
            }
            else if (dir == Vector3.back)
            {
                float distFromEdge = pos.z + halfSize.z;
                targetPos.z = halfSize.z + distFromEdge;
            }

            ghost.obj.transform.position = targetPos;
            
            // Show/hide based on distance to this edge
            float distToEdge = 0;
            if (dir == Vector3.right) distToEdge = halfSize.x - pos.x;
            else if (dir == Vector3.left) distToEdge = pos.x + halfSize.x;
            else if (dir == Vector3.up) distToEdge = halfSize.y - pos.y;
            else if (dir == Vector3.down) distToEdge = pos.y + halfSize.y;
            else if (dir == Vector3.forward) distToEdge = halfSize.z - pos.z;
            else if (dir == Vector3.back) distToEdge = pos.z + halfSize.z;

            ghost.obj.SetActive(distToEdge < renderDistance * 2f);
        }
    }

    void OnDestroy()
    {
        foreach (var ghost in ghosts)
        {
            if (ghost.obj != null)
            {
                Destroy(ghost.obj);
            }
        }
        ghosts.Clear();
    }
}