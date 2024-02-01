using UnityEngine;

public class Torch : MonoBehaviour
{
    public Light torchLight; // Assign this in the inspector
    public ParticleSystem fireParticleSystem; // Assign this in the inspector
    private float burnOutTime = 0f; // Total time in seconds for the torch to burn out

    private float initialLightIntensity;
    private float initialEmissionRate;
    private float burnOutTimer;

    public AudioSource audioSource;
    public AudioSource audioTourch;
    private bool countdownAudioPlayed = false;

    public void RestoreLight(float timeToAdd) {
        burnOutTime += timeToAdd;
        burnOutTimer = burnOutTime;            
        Debug.Log($"Current burn time: {burnOutTimer}");
        Debug.Log($"New burn time after adding {timeToAdd}: {burnOutTimer}");
        countdownAudioPlayed = false;
        audioTourch.Play();
    }


    public void SetBurnOutTime(float time) {
        Debug.Log($"Current burn time: {burnOutTime}");
        this.burnOutTimer = time;
        //burnOutTimer = time;
        this.burnOutTime = time;
    }

    public void BurnOut(){
        var emissionModule = fireParticleSystem.emission;

        torchLight.enabled = false;
        emissionModule.rateOverTime = 0;
        fireParticleSystem.Stop(); // Stop the particle system
        audioTourch.Stop();
    }

    void Start()
    {
        if (torchLight == null)
            torchLight = GetComponentInChildren<Light>();

        initialLightIntensity = torchLight.intensity;

        if (fireParticleSystem == null)
            fireParticleSystem = GetComponentInChildren<ParticleSystem>();

        var emissionModule = fireParticleSystem.emission;
        initialEmissionRate = emissionModule.rateOverTime.constant;

        burnOutTimer = burnOutTime;
        
    }

    void Update()
    {
        if (burnOutTimer > 0)
        {
            // Calculate the new intensity and emission rate based on the elapsed time
            burnOutTimer -= Time.deltaTime;
            float ratio = burnOutTimer / burnOutTime;

            if (!torchLight.enabled)
            {
                torchLight.enabled = true;
            }

            if (!fireParticleSystem.isPlaying)
            {
                fireParticleSystem.Play();
            }

            // Set the new intensity and emission rate
            torchLight.intensity = initialLightIntensity * ratio;
            var emissionModule = fireParticleSystem.emission;
            emissionModule.rateOverTime = initialEmissionRate * ratio;


            if (burnOutTimer <= 10.0f && !countdownAudioPlayed)
            {
                audioSource.Play(); // Play the audio clip
                countdownAudioPlayed = true;
            }

            // If the torch has burned out, ensure the light and particle system are turned off
            if (burnOutTimer < 0)
            {
                if (torchLight.enabled || fireParticleSystem.isPlaying)
                {
                    torchLight.enabled = false;
                    emissionModule.rateOverTime = 0;
                    fireParticleSystem.Stop(); // Stop the particle system
                    audioTourch.Stop();
                }
            }

        }
        if (burnOutTimer < 0)
        {
            
            GameObject playerGameObject = GameObject.FindGameObjectWithTag("Player"); 
            PlayerHealth playerHealth = playerGameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                    playerHealth.TakeDamage(0.05f);
            }
            else{
                Debug.Log($"playerHealth not found!");
            }
        }
    }

}
