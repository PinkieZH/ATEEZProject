using NUnit.Framework.Interfaces;
using UnityEngine;

public class Items : MonoBehaviour, IInteractable
{
    public ItemData itemData;
    private bool aEteInteragi = false;

    public void Read()
    {
        if (itemData == null)
        {
            Debug.LogError("ItemData not assigned to item: " + gameObject.name);
            return;
        }

        // vérif si ObjectsManager existe et démarrer le texte
        if (ObjectsManager.Instance != null)
        {
            //Utiliser le nom personnalisé s'il est défini sinon celui du ItemData
            string objectName = itemData.itemName;
            ObjectsManager.Instance.StartText(objectName, itemData.itemInfo);
        }
        else
        {
            Debug.LogError("ObjectsManager not found in scene !");
        }

        if (aEteInteragi) return;

        aEteInteragi = true;

        // Tu peux mettre ici une animation, son, effet, etc.
        Debug.Log("Interagi avec : " + itemData.itemName);

        if (itemData.estSpecial)
        {
            ItemInteractionManager.Instance.CompterItemSpecial();
        }
    }

    public void Talk()
    {

    }
    public virtual void Interact()
    {

    }

    }
