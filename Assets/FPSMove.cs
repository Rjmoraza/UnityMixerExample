using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class FPSMove : MonoBehaviour
{
    Rigidbody rb;
    AudioSource source;
    public GameObject pauseMenu;
    bool pause = false;

    public AudioMixer mixer;
    AudioMixerSnapshot mainSnapshot;
    AudioMixerSnapshot pauseSnapshot;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        source = GetComponent<AudioSource>();

        mainSnapshot = mixer.FindSnapshot("MAIN");
        pauseSnapshot = mixer.FindSnapshot("PAUSE");
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
                mainSnapshot.TransitionTo(0.1f);
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
                source.Stop();
            }
            else
            {
                Vector3 forward = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;
                Vector3 right = new Vector3(transform.right.x, 0, transform.right.z).normalized;
                Vector3 newVelocity = forward * z + right * x;
                rb.velocity = newVelocity;

                if (!source.isPlaying)
                    source.Play();
            }

            if (Input.GetButtonUp("Cancel"))
            {
                pause = true;
                pauseSnapshot.TransitionTo(0.1f);
            }
        }
    }
}
