using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager manager;

    [HideInInspector] public GameObject sandWitch;

    Vector3 _SandWitchInitPos;
    [HideInInspector] public Transform ingredientsInit;

    [Header("Prefabs")]
    public GameObject sandWitchPrefab;

    [Header("Warning")]
    public GameObject warningText;
    public GameObject limitText;

    [Header("Sounds")]
    public AudioMixer audioMixer;
    public AudioClip bite;
    public AudioClip grab;
    public AudioClip addIngredient;
    AudioSource audioSource;


    private void Awake()
    {
        manager = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        audioSource = GameObject.Find("Player").GetComponent<AudioSource>();
        _SandWitchInitPos = GameObject.Find("Tray").transform.position;
        ingredientsInit = GameObject.Find("Ingredient Pos").transform;
        warningText.SetActive(false);

    }
    public void InitSandwitch()
    {    
        GameObject newSandWitch = Instantiate(sandWitchPrefab, _SandWitchInitPos, Quaternion.identity);
        newSandWitch.transform.parent = ingredientsInit;
        sandWitch = newSandWitch;
    }

    public IEnumerator Warning(GameObject warning)
    {
        warning.SetActive(true);
        yield return new WaitForSeconds(3f);
        warning.SetActive(false);
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

}
