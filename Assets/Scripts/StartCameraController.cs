using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class StartCameraController : MonoBehaviour
{
    public float targetAtmosphericThickness = 0f; // Change this to desired atmospheric thickness
    public float timer = 0.0f;
    public bool hasReachedTarget = false;
    public bool skip = false;
    public GameObject gameTitle;
    public GameObject creatorsTitle;
    public GameObject skipBtn;
    public ParticleSystem particleSystem; // Reference to the Particle System
    public static Color newStartColor = new Color(0.294f, 0.082f, 0.082f, 0.75f); // Default start color for the Particle System

    public AudioClip audioClip; // Reference to the audio clip to play
    public AudioSource audioSource; // Reference to the AudioSource component
    public AudioSource narrative;

    public Camera mainCamera; // Reference to the main camera
    public string newLayerName = "postproc";


    public float playAtTime = 19.5f; // Time at which to play the audio (in seconds)

    public GameObject menuPanel; //The gam  e menu panel
    public GameObject instructionsPanel;

    

    // Get the PostProcessVolume component
    // private PostProcessVolume postProcessVolume;

    //private ColorGrading colorGrading;
    //private Grain grain;

    //private float a = 0f;
    // Start is called before the first frame update
    void Start()
    {
        RenderSettings.skybox.SetFloat("_AtmosphereThickness", 1.35f);
        targetAtmosphericThickness = 1.35f;
        creatorsTitle.SetActive(false);
        gameTitle.SetActive(false);
        menuPanel.SetActive(false);
        instructionsPanel.SetActive(false);

        // Start the coroutine to play audio at the specified time
        StartCoroutine(PlayAudioAtTime(playAtTime));

        if (mainCamera == null)
            mainCamera = Camera.main;

        //postProcessVolume = mainCamera.GetComponent<PostProcessVolume>();
        // Add ColorGrading and DepthOfField effects to the PostProcessVolume
        //postProcessVolume.profile.TryGetSettings(out grain);
        //postProcessVolume.profile.TryGetSettings(out colorGrading);

        //colorGrading.colorFilter.value = new Color(23f, 1f, 1f, 0f);

        // Set initial post exposure value
        //colorGrading.postExposure.value = 5f;

        // Set initial saturation value
        //colorGrading.saturation.value = 30f;
    }

    public void DoSkip()
    {
        // var tCameraPosn = transform.localPosition;
        // tCameraPosn.z = 1100.0f;
        //AssignNewLayer();
        targetAtmosphericThickness = 4f;
        RenderSettings.skybox.SetFloat("_AtmosphereThickness", targetAtmosphericThickness);
        hasReachedTarget = true;
        DynamicGI.UpdateEnvironment();
        if (narrative != null) narrative.Stop();
        timer = 17;
        skip = true;
        StopCoroutine(PlayAudioAtTime(0f));
    }

    public void AssignNewLayer()
    {
        // Find the layer ID by name
        int newLayerID = LayerMask.NameToLayer(newLayerName);

        // Assign the new layer to the camera
        mainCamera.gameObject.layer = newLayerID;
    }

    IEnumerator PlayAudioAtTime(float time)
    {
        // Wait until the specified time
        yield return new WaitForSeconds(time);

        // Play the audio clip
        audioSource.PlayOneShot(audioClip);
    }

    // Update is called once per frame
    void Update()
    {

        // Get the camera's current position
        var tCameraPosn = transform.localPosition;

        // Set the trigger altitude (above this we'll move the camera forward)
        var tTriggerAltitude = 20.0f;

        // Increment timer
        timer += Time.deltaTime;

        // Check the camera's altitude (y-position) against the trigger altitude
        if (timer < 23)
        {
            if (tCameraPosn.y < tTriggerAltitude)
            {
                // Move the camera up
                transform.Translate((Vector3.up * (Time.deltaTime * 5.0f)), Space.World);
            }
            else
            {
                if (tCameraPosn.z >= 1100.0f)
                {
                    // Move the camera forward
                    transform.Translate((Vector3.forward * (Time.deltaTime * 10.0f)));
                }
            }


            // Check if 2 minutes have passed and atmospheric thickness hasn't been changed yet
            if (!hasReachedTarget && targetAtmosphericThickness <= 4f)
            {
                // Change atmospheric thickness
                targetAtmosphericThickness += 0.001f;
                RenderSettings.skybox.SetFloat("_AtmosphereThickness", targetAtmosphericThickness);
                if (targetAtmosphericThickness >= 4f)
                {
                    hasReachedTarget = true; // Set flag to avoid multiple changes
                }
                DynamicGI.UpdateEnvironment();
            }
            // Check if it's time to show the game title
            if (timer >= 2)
            {
                creatorsTitle.SetActive(true);
                if (timer < 23) skipBtn.SetActive(true);
            }
            if (timer >= 15)
            {
                creatorsTitle.SetActive(false);
            }
        }
        if(timer >= 17)
        {
            var mainModule = particleSystem.main;
            mainModule.startColor = new ParticleSystem.MinMaxGradient(newStartColor);
            if(skip)
            {
                transform.Translate(new Vector3(1100f, 0f, 0f) * Time.deltaTime);
                timer = 23;
                gameTitle.SetActive(true);
                creatorsTitle.SetActive(false);
                skip = false;
            }
        }
        if (timer >= 19 && timer < 23 && !gameTitle.activeSelf)
        {
            gameTitle.SetActive(true);
        }
        // Check if it's time to hide the game title
        if (timer >= 23 && gameTitle.activeSelf)
        {
            skipBtn.SetActive(false);
            gameTitle.SetActive(false);
            AssignNewLayer();
            menuPanel.SetActive(true);
        }
    }
}
