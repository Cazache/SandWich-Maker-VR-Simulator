

using UnityEngine;

using UnityEngine.XR.Interaction.Toolkit;


public class IngredientController : MonoBehaviour
{
    [Header("BluePrint")]
    public GameObject BluePrintPrefab;
    public GameObject BittenObj;

    Rigidbody _rb;
    GameObject blueprint;
    private bool _isSelected
    {
        get
        {
            if (GetComponent<XRGrabInteractable>())
                return GetComponent<XRGrabInteractable>().isSelected;
            else
                return false;
        }
    }
    private bool _prevIsSelected, _istrigger;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        if (_isSelected != _prevIsSelected && !_istrigger)
        {
            GameManager.manager.PlaySound(GameManager.manager.grab);
            _prevIsSelected = _isSelected;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (GameManager.manager.sandWitch)
        {
            if (collision.gameObject.layer == 3 && transform.parent == GameManager.manager.sandWitch.transform)
            {
                //I need a little delay here for avoid errors
                Invoke("DoKinematic", 0.15f);
            }
        }

    }
    void OnTriggerStay(Collider other)
    {

        if (_isSelected != _prevIsSelected && other.transform.tag == "AddIngredient")
        {
            if (blueprint)
                Destroy(blueprint);

            if (!GameManager.manager.sandWitch && transform.tag == "Bread")
                GameManager.manager.InitSandwitch();

            AddToSandWitch(GameManager.manager.sandWitch);

            _prevIsSelected = _isSelected;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        _istrigger = true;

        if (transform.parent == null && _isSelected)
        {
            if (other.tag == "AddIngredient" && GameManager.manager.sandWitch || other.tag == "AddIngredient" && transform.tag == "Bread")
            {
                GameObject newBluePrint = Instantiate(BluePrintPrefab, GameManager.manager.ingredientsInit.position, Quaternion.identity);
                blueprint = newBluePrint;
            }
            else if (other.tag == "AddIngredient" && !GameManager.manager.sandWitch && transform.tag != "Bread" && !GameManager.manager.warningText.activeSelf)
            {
                print("WARNING");
                StartCoroutine(GameManager.manager.Warning(GameManager.manager.warningText));
            }
        }

    }
    private void OnTriggerExit(Collider other)
    {
        _istrigger = false;

        if (other.tag == "AddIngredient")
        {
            if (blueprint)
                Destroy(blueprint);
        }
    }
    void AddToSandWitch(GameObject sandwitch)
    {
        if (GameManager.manager.sandWitch)
        {

            if (!sandwitch.GetComponent<SandWitchManager>().LimitChecker() || transform.tag == "Bread")
            {
                _rb.velocity = Vector3.zero;
                transform.parent = GameManager.manager.sandWitch.transform;
                transform.position = GameManager.manager.ingredientsInit.position;
                transform.rotation = Quaternion.Euler(Vector3.zero);
                GameManager.manager.PlaySound(GameManager.manager.addIngredient);
                Destroy(GetComponent<XRGrabInteractable>());
            }
            else if (transform.tag != "Bread")
            {
                StartCoroutine(GameManager.manager.Warning(GameManager.manager.limitText));
            }

        }

    }
    void DoKinematic()
    {
        _rb.isKinematic = true;
        _rb.useGravity = false;
        GameObject sandWtich = GameManager.manager.sandWitch;
        if (sandWtich != null)
        {
            if (!sandWtich.GetComponent<SandWitchManager>().complete)
            {
                Transform Childs = sandWtich.transform;
                int breadsN = 0;
                for (int i = 0; i < GameManager.manager.sandWitch.transform.childCount; i++)
                {
                    if (Childs.GetChild(i).transform.name.Contains("Bread"))
                        breadsN++;
                }

                if (breadsN == 2) CompleteSandWitch(sandWtich);
            }
        }
    }

    void CompleteSandWitch(GameObject sandWitch)
    {
        print("COMPLETE");
        sandWitch.GetComponent<SandWitchManager>().complete = true;
        sandWitch.GetComponent<BoxCollider>().enabled = true;
        sandWitch.AddComponent<Rigidbody>();
        sandWitch.GetComponent<Rigidbody>().isKinematic = false;
        sandWitch.GetComponent<Rigidbody>().useGravity = true;
        sandWitch.AddComponent<XRGrabInteractable>();
        GameManager.manager.sandWitch = null;
    }


}
