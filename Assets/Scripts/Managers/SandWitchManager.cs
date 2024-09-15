using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using static UnityEngine.GraphicsBuffer;

public class SandWitchManager : MonoBehaviour
{
    public bool complete, bitten;
    public ParticleSystem biteParticle;

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

    // Update is called once per frame
    void Update()
    {
        if (_isSelected != _prevIsSelected && !_istrigger)
        {
            GameManager.manager.PlaySound(GameManager.manager.grab);
            _prevIsSelected = _isSelected;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && complete)
        {
            GameManager.manager.PlaySound(GameManager.manager.bite);
            if (!bitten)
            {
                bitten = true;
                bite();
            }
            else
                Destroy(gameObject);

            ParticleSystem newParticle = Instantiate(biteParticle, transform.position, Quaternion.identity);
        }
    }
    void bite()
    {
        int currentIngredietns = transform.childCount;
        for (int i = 0; i < currentIngredietns; i++)
        {
            print(transform.GetChild(i).gameObject);
            GameObject child = transform.GetChild(i).gameObject;

            var ingredientController = child.GetComponent<IngredientController>();
            if (ingredientController == null)
            {
                return;
            }

            GameObject bittenObj = ingredientController.BittenObj;
            if (bittenObj == null)
            {
                return;
            }

            GameObject newIngredient = Instantiate(bittenObj, transform);
            if (newIngredient != null)
            {
                newIngredient.transform.position = child.transform.position;
            }

            var lookPos = Camera.main.transform.position - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            Destroy(child.gameObject);
        }
    }

    public bool LimitChecker()
    {
        if (transform.childCount == 0)
            return false;
        else if (transform.GetChild(transform.childCount - 1).transform.localPosition.y >= 0.16f)
            return true;
        else
            return false;

    }

}
