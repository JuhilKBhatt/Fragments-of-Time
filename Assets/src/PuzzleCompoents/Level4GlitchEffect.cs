using UnityEngine;

[ExecuteInEditMode]
//[RequireComponent(typeof(Camera))]
public class Level4GlitchEffect : MonoBehaviour
{
    public Material glitch;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if(glitch != null)
        {
        Graphics.Blit(source, destination);
        return;

        }
        Graphics.Blit(source, destination, glitch);
        
    }
}
