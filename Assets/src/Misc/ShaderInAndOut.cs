using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class ShaderInAndOut : MonoBehaviour
{
    public float FadeInAndOutTime = 1;
    //public float fadeInToFadeOutRation = 0.25f;
    public Material swirlEffect;
    private Coroutine currentCoroutine;
    public float maxOppacity = 1f;
    public Color baseColor = new Color(1f, 1f, 1f);
    void Awake()
    {
        swirlEffect.SetColor("_BaseColor", new Color(baseColor.r, baseColor.g, baseColor.b, 0));
    }
   public void ActiateShader()
   {
    if(currentCoroutine != null)
    {
            StopCoroutine(currentCoroutine);
            swirlEffect.SetColor("_BaseColor", new Color(baseColor.r, baseColor.g, baseColor.b, 0));
        }
    currentCoroutine = StartCoroutine(shader());
   }

   private IEnumerator shader()
   {
        swirlEffect.SetColor("_BaseColor", new Color(baseColor.r, baseColor.g, baseColor.b, 0));
        //FadeInAndOutTime = 4;
        float time = Time.time;
    while(Time.time < (time + FadeInAndOutTime) /2)
    {
        swirlEffect.SetColor("_BaseColor", new Color(baseColor.r, baseColor.g, baseColor.b, maxOppacity * (Time.time - time) * 2));
        //Debug.Log("We are fading in here");
            yield return null;
        }
    while(Time.time < time + FadeInAndOutTime)
    {
            swirlEffect.SetColor("_BaseColor", new Color(baseColor.r, baseColor.g, baseColor.b, maxOppacity * ((time + FadeInAndOutTime *0.5f) - Time.time) * 2));
           // Debug.Log("We are fading out here");
            yield return null;
        }
        
        swirlEffect.SetColor("_BaseColor", new Color(baseColor.r, baseColor.g, baseColor.b, 0));
        yield return null;
    }
}
