using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectPooler : MonoBehaviour
{
    /// <summary>
    /// THIS WAS A MASSIVE WASTE OF TIME
    /// JUST DONT USE IT
    /// EVER
    /// </summary>
    [SerializeField] GameObject itemAfterEffectPrefab;
    List<GameObject> itemEffects;
    List<GameObject> activeItems;
    WaitForSeconds secs = new WaitForSeconds(2);
    private void Start()
    {
        itemEffects = new List<GameObject>();
        activeItems = new List<GameObject>();
    }
    public void MakeEffect(SpriteRenderer sp, bool sank)
    {
        print(itemEffects.Count + ":-:" + activeItems.Count);
        if(itemEffects.Count > 0)
        {
            ReuseOld(sp, sank);
        }
        else
        {
            MakeNew(sp, sank);
        }
    }
    void MakeNew(SpriteRenderer sp, bool sank)
    {
        print("madenew");
        //APPEARS TO BE SCALED INCORRECTLY
        Transform spTrans = sp.transform;
        SpriteRenderer effect = Instantiate(itemAfterEffectPrefab, spTrans.position, spTrans.rotation).GetComponentInChildren<SpriteRenderer>();
        TriggerDestroy dest = effect.GetComponentInParent<TriggerDestroy>();
        dest.sank = sank;
        effect.transform.localScale = spTrans.localScale;
        effect.sprite = sp.sprite;
        effect.color = sp.color;
        StartCoroutine(addToInactivesAfterDelay(dest.transform.parent.gameObject));
    }
    void ReuseOld(SpriteRenderer sp, bool sank)
    {
        print("reused");
        Transform spTrans = sp.transform;
        GameObject afterEffect = itemEffects[0];
        afterEffect.SetActive(true);
        SpriteRenderer effect = afterEffect.GetComponentInChildren<SpriteRenderer>();
        afterEffect.GetComponentInChildren<Animator>().SetTrigger("Go");
        afterEffect.GetComponentInChildren<TriggerDestroy>().sank = sank;
        effect.transform.position = spTrans.position;
        effect.transform.localScale = spTrans.localScale;
        effect.sprite = sp.sprite;
        effect.color = sp.color;

        StartCoroutine(addToInactivesAfterDelay(afterEffect));
    }
    IEnumerator addToInactivesAfterDelay(GameObject effect)
    {
        activeItems.Add(effect.gameObject);
        itemEffects.Remove(effect.gameObject);
        yield return secs;
        itemEffects.Add(effect);
        activeItems.Remove(effect);
    }
}
