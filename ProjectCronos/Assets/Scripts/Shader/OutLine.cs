using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using System;

[Serializable, VolumeComponentMenu("Post-processing/Custom/OutLine")]
public sealed class OutLine : CustomPostProcessVolumeComponent, IPostProcessComponent
{
    public ColorParameter edgeColorParameter = new ColorParameter(Color.white);
    public Vector2Parameter edgeThreshold = new Vector2Parameter(Vector2.zero);
    public ClampedFloatParameter fillOpacity = new ClampedFloatParameter(0f, 0f, 1f);
    public ColorParameter colorKeyParameter = new ColorParameter(Color.white);
    public ClampedFloatParameter ditherStrength = new ClampedFloatParameter(0f, 0f, 1f);
    Material m_Material;

    public bool IsActive() =>
        m_Material != null &&
        fillOpacity.value > 0f &&
        ditherStrength.value > 0f;

    public override CustomPostProcessInjectionPoint injectionPoint => CustomPostProcessInjectionPoint.AfterPostProcess;

    public override void Setup()
    {
        if (Shader.Find("Hidden/Shader/OutLine") != null)
            m_Material = new Material(Shader.Find("Hidden/Shader/OutLine"));
    }

    public override void Render(CommandBuffer cmd, HDCamera camera, RTHandle source, RTHandle destination)
    {
        if (m_Material == null)
            return;

        m_Material.SetColor("_EdgeColor", edgeColorParameter.value);
        m_Material.SetVector("_Position", edgeThreshold.value);
        m_Material.SetFloat("_FillOpacity", fillOpacity.value);
        m_Material.SetColor("_ColorKey0", colorKeyParameter.value);
        m_Material.SetFloat("_DitherStrength", ditherStrength.value);
        m_Material.SetTexture("_InputTexture", source);
        HDUtils.DrawFullScreen(cmd, m_Material, destination);
    }

    public override void Cleanup() => CoreUtils.Destroy(m_Material);
}
