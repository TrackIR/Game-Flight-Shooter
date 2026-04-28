using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class CockpitRadar : MonoBehaviour
{
    public Transform playerShip;

    public RectTransform radarArea;
    public GameObject blipPrefab;

    public float radarRange = 500f;

    private List<GameObject> blips = new List<GameObject>();
    private List<Transform> asteroids = new List<Transform>();

    void Start()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Asteroid");

        foreach (GameObject obj in objs)
        {
            asteroids.Add(obj.transform);

            GameObject blip = Instantiate(blipPrefab, radarArea);
            blips.Add(blip);
        }
    }

    void Update()
    { 
    GameObject[] asteroidObjects = GameObject.FindGameObjectsWithTag("Asteroid");

    List<Transform> closest = new List<Transform>();

    foreach (GameObject obj in asteroidObjects)
    {
        closest.Add(obj.transform);
    }

    closest.Sort((a, b) =>
        Vector3.Distance(playerShip.position, a.position)
        .CompareTo(Vector3.Distance(playerShip.position, b.position))
    );

    int count = Mathf.Min(10, closest.Count);

    while (blips.Count < count)
    {
        GameObject newBlip = Instantiate(blipPrefab, radarArea);
        blips.Add(newBlip);
    }

    for (int i = 0; i < blips.Count; i++)
    {
        if (i >= count)
        {
            blips[i].SetActive(false);
            continue;
        }

        Transform asteroid = closest[i];

        Vector3 offset = asteroid.position - playerShip.position;
        float distance = offset.magnitude;

        if (distance > radarRange)
        {
            blips[i].SetActive(false);
            continue;
        }

        blips[i].SetActive(true);

        Vector3 localDir = playerShip.InverseTransformDirection(offset);

        float x = localDir.x / radarRange;
        float y = localDir.z / radarRange;

        Vector2 radarPos = new Vector2(x, y) * (radarArea.sizeDelta.x / 2f);

        blips[i].GetComponent<RectTransform>().anchoredPosition = radarPos;
    }
    }
}