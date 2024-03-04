////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) Martin Bustos @FronkonGames <fronkongames@gmail.com>. All rights reserved.
//
// THIS FILE CAN NOT BE HOSTED IN PUBLIC REPOSITORIES.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Unity.Profiling;

namespace FronkonGames.Artistic.TiltShift
{
  ///------------------------------------------------------------------------------------------------------------------
  /// <summary> Render Pass. </summary>
  /// <remarks> Only available for Universal Render Pipeline. </remarks>
  ///------------------------------------------------------------------------------------------------------------------
  public sealed partial class TiltShift
  {
    private sealed class RenderPass : ScriptableRenderPass
    {
      private readonly Settings settings;

      private RenderTargetIdentifier colorBuffer;
      private RenderTextureDescriptor renderTextureDescriptor;

#if UNITY_2022_1_OR_NEWER
      private RTHandle renderTextureHandle0, renderTextureHandle1;

      private readonly ProfilingSampler profilingSamples = new(Constants.Asset.AssemblyName);
      private ProfilingScope profilingScope;
#else
      private int renderTextureHandle0, renderTextureHandle1;
#endif
      private readonly Material material;

      private static readonly ProfilerMarker ProfilerMarker = new($"{Constants.Asset.AssemblyName}.Pass.Execute");

      private const string CommandBufferName = Constants.Asset.AssemblyName;

      private static class ShaderIDs
      {
        internal static readonly int Intensity = Shader.PropertyToID("_Intensity");
        internal static readonly int DeltaTime = Shader.PropertyToID("_DeltaTime");

        internal static readonly int Angle = Shader.PropertyToID("_Angle");
        internal static readonly int Aperture = Shader.PropertyToID("_Aperture");
        internal static readonly int Offset = Shader.PropertyToID("_Offset");
        internal static readonly int Blur = Shader.PropertyToID("_Blur");
        internal static readonly int BlurCurve = Shader.PropertyToID("_BlurCurve");
        internal static readonly int Distortion = Shader.PropertyToID("_Distortion");
        internal static readonly int DistortionScale = Shader.PropertyToID("_DistortionScale");
        internal static readonly int FocusedBrightness = Shader.PropertyToID("_FocusedBrightness");
        internal static readonly int FocusedContrast = Shader.PropertyToID("_FocusedContrast");
        internal static readonly int FocusedGamma = Shader.PropertyToID("_FocusedGamma");
        internal static readonly int FocusedHue = Shader.PropertyToID("_FocusedHue");
        internal static readonly int FocusedSaturation = Shader.PropertyToID("_FocusedSaturation");
        internal static readonly int UnfocusedBrightness = Shader.PropertyToID("_UnfocusedBrightness");
        internal static readonly int UnfocusedContrast = Shader.PropertyToID("_UnfocusedContrast");
        internal static readonly int UnfocusedGamma = Shader.PropertyToID("_UnfocusedGamma");
        internal static readonly int UnfocusedHue = Shader.PropertyToID("_UnfocusedHue");
        internal static readonly int UnfocusedSaturation = Shader.PropertyToID("_UnfocusedSaturation");

        internal static readonly int Brightness = Shader.PropertyToID("_Brightness");
        internal static readonly int Contrast = Shader.PropertyToID("_Contrast");
        internal static readonly int Gamma = Shader.PropertyToID("_Gamma");
        internal static readonly int Hue = Shader.PropertyToID("_Hue");
        internal static readonly int Saturation = Shader.PropertyToID("_Saturation");
      }

      private static class Keywords
      {
        internal static readonly string DebugView = "DEBUG_VIEW";
      }

      /// <summary> Render pass constructor. </summary>
      public RenderPass(Settings settings)
      {
        this.settings = settings;

        string shaderPath = $"Shaders/{Constants.Asset.ShaderName}_URP";
        Shader shader = Resources.Load<Shader>(shaderPath);
        if (shader != null)
        {
          if (shader.isSupported == true)
            material = CoreUtils.CreateEngineMaterial(shader);
          else
            Log.Warning($"'{shaderPath}.shader' not supported");
        }
      }

      /// <inheritdoc/>
      public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
      {
        renderTextureDescriptor = renderingData.cameraData.cameraTargetDescriptor;
        renderTextureDescriptor.depthBufferBits = 0;

#if UNITY_2022_1_OR_NEWER
        colorBuffer = renderingData.cameraData.renderer.cameraColorTargetHandle;

        renderTextureDescriptor.colorFormat = RenderTextureFormat.ARGB32;
        RenderingUtils.ReAllocateIfNeeded(ref renderTextureHandle0, renderTextureDescriptor, settings.filterMode, TextureWrapMode.Clamp, false, 1, 0, $"_RTHandle0_{Constants.Asset.Name}");
        RenderingUtils.ReAllocateIfNeeded(ref renderTextureHandle1, renderTextureDescriptor, settings.filterMode, TextureWrapMode.Clamp, false, 1, 0, $"_RTHandle1_{Constants.Asset.Name}");
#else
        colorBuffer = renderingData.cameraData.renderer.cameraColorTarget;

        renderTextureHandle0 = Shader.PropertyToID($"_RTHandle0_{Constants.Asset.Name}");
        cmd.GetTemporaryRT(renderTextureHandle0, renderTextureDescriptor.width, renderTextureDescriptor.height, renderTextureDescriptor.depthBufferBits, settings.filterMode, RenderTextureFormat.ARGB32);

        renderTextureHandle1 = Shader.PropertyToID($"_RTHandle1_{Constants.Asset.Name}");
        cmd.GetTemporaryRT(renderTextureHandle1, renderTextureDescriptor.width, renderTextureDescriptor.height, renderTextureDescriptor.depthBufferBits, settings.filterMode, RenderTextureFormat.ARGB32);
#endif
      }

      /// <inheritdoc/>
      public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
      {
        if (material == null ||
            renderingData.postProcessingEnabled == false ||
            settings.intensity == 0.0f ||
            settings.affectSceneView == false && renderingData.cameraData.isSceneViewCamera == true)
          return;

        CommandBuffer cmd = CommandBufferPool.Get(CommandBufferName);

        if (settings.enableProfiling == true)
#if UNITY_2022_1_OR_NEWER
          profilingScope = new ProfilingScope(cmd, profilingSamples);
#else
          ProfilerMarker.Begin();
#endif

        material.shaderKeywords = null;
        material.SetFloat(ShaderIDs.Intensity, settings.intensity);
        material.SetFloat(ShaderIDs.DeltaTime, settings.ignoreDeltaTimeScale == true ? Time.unscaledDeltaTime : Time.deltaTime);

#if UNITY_EDITOR
        if (settings.debugView == true)
          material.EnableKeyword(Keywords.DebugView);
#endif
        material.SetFloat(ShaderIDs.Angle, Mathf.Deg2Rad * settings.angle);
        material.SetFloat(ShaderIDs.Aperture, settings.aperture);
        material.SetFloat(ShaderIDs.Offset, settings.offset);

        material.SetFloat(ShaderIDs.BlurCurve, settings.blurCurve);
        material.SetFloat(ShaderIDs.Blur, settings.blur);
        material.SetFloat(ShaderIDs.Distortion, settings.distortion);
        material.SetFloat(ShaderIDs.DistortionScale, settings.distortionScale);

        material.SetFloat(ShaderIDs.FocusedBrightness, settings.focusedBrightness);
        material.SetFloat(ShaderIDs.FocusedContrast, settings.focusedContrast);
        material.SetFloat(ShaderIDs.FocusedGamma, 1.0f / settings.focusedGamma);
        material.SetFloat(ShaderIDs.FocusedHue, settings.focusedHue);
        material.SetFloat(ShaderIDs.FocusedSaturation, settings.focusedSaturation);

        material.SetFloat(ShaderIDs.UnfocusedBrightness, settings.unfocusedBrightness);
        material.SetFloat(ShaderIDs.UnfocusedContrast, settings.unfocusedContrast);
        material.SetFloat(ShaderIDs.UnfocusedGamma, 1.0f / settings.unfocusedGamma);
        material.SetFloat(ShaderIDs.UnfocusedHue, settings.unfocusedHue);
        material.SetFloat(ShaderIDs.UnfocusedSaturation, settings.unfocusedSaturation);

        material.SetFloat(ShaderIDs.Brightness, settings.brightness);
        material.SetFloat(ShaderIDs.Contrast, settings.contrast);
        material.SetFloat(ShaderIDs.Gamma, 1.0f / settings.gamma);
        material.SetFloat(ShaderIDs.Hue, settings.hue);
        material.SetFloat(ShaderIDs.Saturation, settings.saturation);

#if UNITY_2022_1_OR_NEWER
        cmd.Blit(colorBuffer, renderTextureHandle0, material, 0);
        cmd.Blit(renderTextureHandle0, renderTextureHandle1, material, 1);
        cmd.Blit(renderTextureHandle1, colorBuffer);
#else
        Blit(cmd, colorBuffer, renderTextureHandle0, material, 0);
        Blit(cmd, renderTextureHandle0, renderTextureHandle1, material, 1);
        Blit(cmd, renderTextureHandle1, colorBuffer);
#endif

        if (settings.enableProfiling == true)
#if UNITY_2022_1_OR_NEWER
          profilingScope.Dispose();
#else
          ProfilerMarker.End();
#endif

        context.ExecuteCommandBuffer(cmd);
        CommandBufferPool.Release(cmd);
      }
    }
  }
}
