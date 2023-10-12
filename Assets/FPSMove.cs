using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class FPSMove : MonoBehaviour
{
    Rigidbody rb;
    public GameObject pauseMenu;
    bool pause = false;

    AudioSource source;

    float sourceVolume;

    public AudioMixer musicMixer;
    public AudioMixer sfxMixer;

    AudioMixerSnapshot mainSnapshot;
    AudioMixerSnapshot pauseSnapshot;

    AudioMixerSnapshot sfxMainSnapshot;
    AudioMixerSnapshot sfxReverbSnapshot;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        source = GetComponent<AudioSource>();
        sourceVolume = source.volume;

        mainSnapshot = musicMixer.FindSnapshot("MAIN");
        pauseSnapshot = musicMixer.FindSnapshot("PAUSE");

        sfxMainSnapshot = sfxMixer.FindSnapshot("MAIN-SFX");
        sfxReverbSnapshot = sfxMixer.FindSnapshot("REVERB-SFX");
    }

    // Update is called once per frame
    void Update()
    {
        if(pause)
        {
            pauseMenu.SetActive(true);

            if(Input.GetButtonUp("Cancel"))
            {
                pause = false;
                mainSnapshot.TransitionTo(0.5f);
            }
        }
        else
        {
            pauseMenu.SetActive(false);

            float x = Input.GetAxis("Horizontal") * 10;
            float z = Input.GetAxis("Vertical") * 10;

            if (x == 0 && z == 0)
            {
                rb.velocity = Vector3.zero;
                source.volume = 0;
            }
            else
            {
                Vector3 forward = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;
                Vector3 right = new Vector3(transform.right.x, 0, transform.right.z).normalized;
                Vector3 newVelocity = forward * z + right * x;
                rb.velocity = newVelocity;
                source.volume = sourceVolume;
            }

            if (Input.GetButtonUp("Cancel"))
            {
                pause = true;
                pauseSnapshot.TransitionTo(0.5f);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "ReverbZone")
        {
            sfxReverbSnapshot.TransitionTo(0.1f);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.tag == "ReverbZone")
        {
            sfxMainSnapshot.TransitionTo(0.1f);
        }
    }
}
