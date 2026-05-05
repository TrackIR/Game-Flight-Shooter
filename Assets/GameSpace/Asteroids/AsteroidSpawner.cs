using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class AsteroidSpawner : MonoBehaviour
{
    [Header("Spawn Position")]    // Parent of all asteroids spawned
    [SerializeField] private Transform parentOfAsteroids;
    // [SerializeField] private int spawnRadius = 100;    // Radius of perimiter where asteroids can spawn


    [Header("Asteroid Types")]    // Different types of asteroids to spawn
    [SerializeField] private GameObject basicAsteroidPrefab;
    [SerializeField] private GameObject healingAsteroidPrefab;
    [SerializeField] private GameObject bombAsteroidPrefab;


    [Header("Asteroid Size")]    // Size range of asteroids
    [SerializeField] private float minAsteroidSize = 20f;
    [SerializeField] private float maxAsteroidSize = 50f;


    [Header("Asteroid Move Speed")]    // Move speed range of asteroids
    [SerializeField] private float minAsteroidMoveSpeed = 20f;
    [SerializeField] private float maxAsteroidMoveSpeed = 30f;
 

    [Header("Asteroid Rotation Speed")]    // Rotation speed range of asteroids
    [SerializeField] private float minAsteroidRotSpeed = 1.0f;
    [SerializeField] private float maxAsteroidRotSpeed = 100.0f;


    [Header("Game Mode Scripts")]
    public GameObject TradeShowMode;
    public GameObject EndlessMode;
    public GameObject WaveMode;


    [Header("Radar")]
    public GameObject spaceship;
    public RectTransform radarIconPrefab;   // Assign your UI icon prefab here
    public Camera playerCamera;
    public float screenEdgeBuffer = 40f;
    public int maxRadarIcons = 3;           // How many asteroids to track
    public float maxRadarDistance = 150f;   // Hide icons past this distance


    [Header("Radar Warning Visuals")]

    public float minRadarScale = 0.3f;
    // Size of icon when asteroid is far away
    // Lower = smaller distant warning
    // Higher = distant asteroids stay more noticeable

    public float maxRadarScale = 1.3f;
    // Size of icon when asteroid is very close
    // Higher = stronger "danger" feeling
    // Controls how dramatic the growth is

    public float minRadarAlpha = 0.1f;
    // Transparency when asteroid is far away
    // Lower = very faint warning
    // Higher = always visible even at distance

    public float maxRadarAlpha = 1.0f;
    // Transparency when asteroid is close
    // Usually leave at 1.0 (fully visible)

    public float radarVisualMaxDistance = 100f;
    // Distance range used to scale size and transparency
    // Larger = slower visual change
    // Smaller = faster visual change

    public float blinkStartDistance = 70f;
    // Distance where blinking begins
    // Larger = blinking starts earlier
    // Smaller = blinking only when very close

    public float slowBlinkSpeed = 1f;
    // Blink speed when asteroid first enters danger range
    // Lower = slow pulse
    // Higher = faster blinking

    public float fastBlinkSpeed = 15f;
    // Blink speed when asteroid is extremely close
    // Creates aggressive warning behavior

    public float blinkMinAlphaMultiplier = 0.05f;
    // How dim the icon gets during blink
    // Lower = stronger flash
    // Higher = softer blink

    private List<RectTransform> radarIcons = new List<RectTransform>();
    
    // keeps a count of how many asteroids there are
    // incs in SpawnXAsteroids function and Basic Asteroid's Split function, decs in each Asteroid's Die function
    public static int asteroidCount = 0;
    
    private int gameModeSetting = GameModeMenu.gameModeSetting;

    void Start()
    {
        // for testing different game modes in scene
        // gameModeSetting = 2;

        switch (gameModeSetting)
        {
            case 0:
                TradeShowModeOn();
                break;
            
            case 1:
                EndlessModeOn();
                break;

            case 2:
                WaveModeOn();
                break;
        }
    }

    void Update()
    {
        UpdateRadar();
    }

    private void TradeShowModeOn()
    {
        TradeShowMode.SetActive(true);
    }

    private void EndlessModeOn()
    {
        EndlessMode.SetActive(true);
    }

    private void WaveModeOn()
    {
        WaveMode.SetActive(true);
    }

    // Spawns X amount of asteroids in the spawn radius
    public void SpawnXAsteroids(int x)
    {
        for (int i = 0; i < x; i++)
        {
            // Prepare asteroid to be instantiated
            GameObject asteroid = null;
            int randomSize = (int)UnityEngine.Random.Range(minAsteroidSize, maxAsteroidSize);
            float randomMoveSpeed = UnityEngine.Random.Range(minAsteroidMoveSpeed, maxAsteroidMoveSpeed);
            float randomRotSpeed = UnityEngine.Random.Range(minAsteroidRotSpeed, maxAsteroidRotSpeed);
            Vector3 randomMoveDir = new Vector3(UnityEngine.Random.Range(-1.0f, 1.0f), UnityEngine.Random.Range(-1.0f, 1.0f), UnityEngine.Random.Range(-1.0f, 1.0f)).normalized;
            Vector3 randomRotDir = new Vector3(UnityEngine.Random.Range(-1.0f, 1.0f), UnityEngine.Random.Range(-1.0f, 1.0f), UnityEngine.Random.Range(-1.0f, 1.0f)).normalized;
            // Vector3 randomPosition = parentOfAsteroids.position + (UnityEngine.Random.onUnitSphere * spawnRadius);
            //Vector3 randomPosition = new Vector3(UnityEngine.Random.Range(-170f, 170f), UnityEngine.Random.Range(-170f, 170f), UnityEngine.Random.Range(-170f, 170f));
            float half = 200f;

            int face = UnityEngine.Random.Range(0, 6);
            
            Vector3 randomPosition = Vector3.zero;
            
            switch (face)
            {
                case 0: // +X face
                    randomPosition = new Vector3(half, Random.Range(-half, half), Random.Range(-half, half));
                    break;
                case 1: // -X face
                    randomPosition = new Vector3(-half, Random.Range(-half, half), Random.Range(-half, half));
                    break;
                case 2: // +Y face
                    randomPosition = new Vector3(Random.Range(-half, half), half, Random.Range(-half, half));
                    break;
                case 3: // -Y face
                    randomPosition = new Vector3(Random.Range(-half, half), -half, Random.Range(-half, half));
                    break;
                case 4: // +Z face
                    randomPosition = new Vector3(Random.Range(-half, half), Random.Range(-half, half), half);
                    break;
                case 5: // -Z face
                    randomPosition = new Vector3(Random.Range(-half, half), Random.Range(-half, half), -half);
                    break;
            }
            if (GameModeMenu.gameModeSetting == 0)
                randomPosition /= 2;

            // Generate a random number [0-20) and use that number for chance calculations
            int randChance = UnityEngine.Random.Range(0, 20);

            // Spawn asteroids based on chance variable, set their position and rotation, and add them as a child of the chosen parent
            switch (randChance)
            {
                case 1: // Healing Asteroid, 5% of the time
                    asteroid = Instantiate(healingAsteroidPrefab, randomPosition, Quaternion.identity, parentOfAsteroids);
                    break;
                case 2: // Bomb asteroid, 5% of the time
                    asteroid = Instantiate(bombAsteroidPrefab, randomPosition, Quaternion.identity, parentOfAsteroids);
                    break;
                default: // Basic asteroid, 90% of the time
                    asteroid = Instantiate(basicAsteroidPrefab, randomPosition, Quaternion.identity, parentOfAsteroids);
                    break;
            }

            // Initialize asteroid, which is the same process for all asteroid types
            asteroid.GetComponent<AsteroidClass>().Init(
                /* iSize = */       randomSize,
                /* iMoveSpeed = */  randomMoveSpeed,
                /* iRotSpeed = */   randomRotSpeed,
                /* iMoveDir = */    randomMoveDir,
                /* iRotDir = */     randomRotDir
            );

            // Apply the saved color scheme to the newly spawned asteroid
            var apply = asteroid.GetComponent<ApplySavedColors>();
            if (apply != null && asteroid.GetComponent<AsteroidClass>().GetAsteroidType() == AsteroidClass.InheritanceType.Basic)
                apply.ApplyNow();
            else
                Debug.LogError("Cannot find ApplySavedColors component on " + asteroid);
            
            // Debug.Log(asteroid.GetComponent<AsteroidClass>().GetAsteroidType());

            asteroidCount++;
        }
    }

    // ---------------- RADAR SYSTEM ----------------

    private void EnsureRadarIcons()
    {
        if (radarIcons.Count == maxRadarIcons)
            return;

        foreach (var icon in radarIcons)
        {
            if (icon != null)
                Destroy(icon.gameObject);
        }

        radarIcons.Clear();

        for (int i = 0; i < maxRadarIcons; i++)
        {
            RectTransform icon = Instantiate(radarIconPrefab, radarIconPrefab.parent);
            radarIcons.Add(icon);
        }
    }

    private void UpdateRadar()
    {
        if (radarIconPrefab == null || playerCamera == null || spaceship == null)
            return;

        EnsureRadarIcons();

        // Build a live list of asteroids from the parent transform
        // (Main branch parents all asteroids under parentOfAsteroids instead of keeping a List<GameObject>)
        List<GameObject> validAsteroids = new List<GameObject>();
        if (parentOfAsteroids != null)
        {
            foreach (Transform child in parentOfAsteroids)
            {
                if (child != null && child.gameObject.activeInHierarchy)
                    validAsteroids.Add(child.gameObject);
            }
        }

        validAsteroids.Sort((a, b) =>
            Vector3.Distance(spaceship.transform.position, a.transform.position)
            .CompareTo(
            Vector3.Distance(spaceship.transform.position, b.transform.position)));

        for (int i = 0; i < radarIcons.Count; i++)
        {
            if (i >= validAsteroids.Count)
            {
                radarIcons[i].gameObject.SetActive(false);
                continue;
            }

            GameObject target = validAsteroids[i];
            float distance = Vector3.Distance(spaceship.transform.position, target.transform.position);

            if (distance > maxRadarDistance)
            {
                radarIcons[i].gameObject.SetActive(false);
                continue;
            }

            Vector3 viewportPos = playerCamera.WorldToViewportPoint(target.transform.position);

            bool isVisible =
                viewportPos.z > 0 &&
                viewportPos.x > 0 && viewportPos.x < 1 &&
                viewportPos.y > 0 && viewportPos.y < 1;

            if (isVisible)
            {
                radarIcons[i].gameObject.SetActive(false);
                continue;
            }

            radarIcons[i].gameObject.SetActive(true);

            Vector3 direction = (target.transform.position - spaceship.transform.position).normalized;
            Vector3 screenDir = playerCamera.WorldToViewportPoint(spaceship.transform.position + direction * 100f) -
                    new Vector3(0.5f, 0.5f, 0f);
            screenDir.z = 0f;

            float angle = Mathf.Atan2(screenDir.y, screenDir.x) * Mathf.Rad2Deg;
            radarIcons[i].rotation = Quaternion.Euler(0, 0, angle - 90f);

            Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0);
            Vector3 edgePosition = screenCenter + screenDir.normalized * (Screen.height / 2f - screenEdgeBuffer);

            radarIcons[i].position = edgePosition;

            // Scale icon based on distance
            float t = Mathf.Clamp01(1f - (distance / radarVisualMaxDistance));
            float scale = Mathf.Lerp(minRadarScale, maxRadarScale, t);

            radarIcons[i].localScale = Vector3.one * scale;

            // Change icon transparency based on distance
            Graphic radarGraphic = radarIcons[i].GetComponent<Graphic>();

            if (radarGraphic != null)
            {
                float alpha = Mathf.Lerp(minRadarAlpha, maxRadarAlpha, t);

                // Blink icon when asteroid is very close
                if (distance <= blinkStartDistance)
                {
                    float blinkT = Mathf.Clamp01(1f - (distance / blinkStartDistance));
                    float blinkSpeed = Mathf.Lerp(slowBlinkSpeed, fastBlinkSpeed, blinkT);
                    float blink = Mathf.Abs(Mathf.Sin(Time.time * blinkSpeed));

                    alpha *= Mathf.Lerp(blinkMinAlphaMultiplier, 1f, blink);
                }

                Color color = radarGraphic.color;
                color.a = alpha;
                radarGraphic.color = color;
            }
        }
    }
}