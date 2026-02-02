using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Interactable : MonoBehaviour
{
    StageManager stageManager;

    public enum Type
    {
        Fly,
        Stinger,
        Debris
    }
    public Type currentType;

    public void Awake()
    {
        stageManager = StageManager.Instance;
    }

    public void OnDisable()
    {
        StopAllCoroutines();
    }

    public void DealWithCollision()
    {
        switch (currentType)
        {
            case Type.Fly:
                StartCoroutine(DealWithFly());
                break;
            case Type.Stinger:
                StartCoroutine(DealWithStinger());
                break;
            case Type.Debris:
                StartCoroutine(DealWithDebris());
                break;
            default:
                break;
        }
    }

    //To use when colliding with TongueTip
    IEnumerator DealWithFly()
    {
        yield return new WaitForSeconds(1);
    }
    IEnumerator DealWithStinger()
    {
        yield return new WaitForSeconds(1);
    }
    IEnumerator DealWithDebris()
    {
        yield return new WaitForSeconds(1);
    }

    //To use for behaviour depending on type
    public void InteractableBehaviour()
    {
        if (stageManager == null)
            stageManager = StageManager.Instance;

        StartCoroutine(Behave(currentType));
    }

    public IEnumerator Behave(Type interactableType)
    {
        switch (interactableType)
        {
            case Type.Fly:
                Debug.Log("Hello i am a fly");
                break;
            case Type.Stinger:
                break;
            case Type.Debris:
                break;
            default:
                break;
        }

        float delay = (3 / Mathf.Max(1, stageManager.playerMultiplier));
        yield return new WaitForSeconds(delay);
        FlagExitbehaviour(interactableType);
    }

    //Tell StageManager you are done and let it place you back on the respective list
    public void FlagExitbehaviour(Type interactableType)
    {
        switch (interactableType)
        {
            case Type.Fly:
                //Animate fly for exit
                stageManager.ResetInteractable(stageManager.flyList, gameObject);
                break;
            case Type.Stinger:
                break;
            case Type.Debris:
                break;
            default:
                break;
        }

    }
}
