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
using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace FronkonGames.Artistic.TiltShift
{
  ///------------------------------------------------------------------------------------------------------------------
  /// <summary> Settings. </summary>
  /// <remarks> Only available for Universal Render Pipeline. </remarks>
  ///------------------------------------------------------------------------------------------------------------------
  public sealed partial class TiltShift
  {
    /// <summary> Settings. </summary>
    [Serializable]
    public sealed class Settings
    {
      public Settings() => ResetDefaultValues();

      /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
      #region Common settings.

      /// <summary> Controls the intensity of the effect [0, 1]. Default 1. </summary>
      /// <remarks> An effect with Intensity equal to 0 will not be executed. </remarks>
      public float intensity = 1.0f;
      #endregion
      /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

      /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
      #region Tilt Shift settings.

      /// <summary> Angle [-90, 90]. Default 0. </summary>
      public float angle = 0.0f;

      /// <summary> Effect aperture [0.1, 5]. Default 0.5. </summary>
      public float aperture = 0.5f;

      /// <summary> Vertical offset [-1.5, 1.5]. Default 0. </summary>
      public float offset = 0.0f;

      /// <summary> Blur curve [1, 10]. Default 3.0. </summary>
      public float blurCurve = 3.0f;

      /// <summary> Blur multiplier [0, 10]. Default 1. </summary>
      public float blur = 1.0f;

      /// <summary> Distortion force [0, 20]. Default 5. </summary>
      public float distortion = 5.0f;

      /// <summary> Distortion scale [0.01, 2]. Default 1. </summary>
      public float distortionScale = 1.0f;

      /// <summary> Debug view, only available in the Editor. </summary>
      public bool debugView = false;

      /// <summary> Brightness of the focused area [-1.0, 1.0]. Default 0. </summary>
      public float focusedBrightness = 0.0f;

      /// <summary> Contrast of the focused area [0.0, 10.0]. Default 1. </summary>
      public float focusedContrast = 1.0f;

      /// <summary> Gamma of the focused area [0.1, 10.0]. Default 1. </summary>
      public float focusedGamma = 1.0f;

      /// <summary> The color wheel of the focused area [0.0, 1.0]. Default 0. </summary>
      public float focusedHue = 0.0f;

      /// <summary> Intensity of a colors of the focused area [0.0, 2.0]. Default 1. </summary>
      public float focusedSaturation = 1.0f;

      /// <summary> Brightness of the unfocused area [-1.0, 1.0]. Default 0. </summary>
      public float unfocusedBrightness = 0.0f;

      /// <summary> Contrast of the unfocused area [0.0, 10.0]. Default 1. </summary>
      public float unfocusedContrast = 1.0f;

      /// <summary> Gamma of the unfocused area [0.1, 10.0]. Default 1. </summary>
      public float unfocusedGamma = 1.0f;

      /// <summary> The color wheel of the unfocused area [0.0, 1.0]. Default 0. </summary>
      public float unfocusedHue = 0.0f;

      /// <summary> Intensity of a colors of the unfocused area [0.0, 2.0]. Default 1. </summary>
      public float unfocusedSaturation = 1.0f;

      #endregion
      /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

      /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
      #region Color settings.

      /// <summary> Brightness [-1.0, 1.0]. Default 0. </summary>
      public float brightness = 0.0f;

      /// <summary> Contrast [0.0, 10.0]. Default 1. </summary>
      public float contrast = 1.0f;

      /// <summary> Gamma [0.1, 10.0]. Default 1. </summary>
      public float gamma = 1.0f;

      /// <summary> The color wheel [0.0, 1.0]. Default 0. </summary>
      public float hue = 0.0f;

      /// <summary> Intensity of a colors [0.0, 2.0]. Default 1. </summary>
      public float saturation = 1.0f;
      #endregion
      /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

      /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
      #region Advanced settings.

      /// <summary> Does it affect the Scene View? </summary>
      public bool affectSceneView = false;

      /// <summary> Filter mode. Default Bilinear. </summary>
      public FilterMode filterMode = FilterMode.Bilinear;

      /// <summary> Render pass injection. Default BeforeRenderingPostProcessing. </summary>
      public RenderPassEvent whenToInsert = RenderPassEvent.BeforeRenderingPostProcessing;

      /// <summary> Set to true to ignore delta time scaling. </summary>
      public bool ignoreDeltaTimeScale = false;

      /// <summary> Enable render pass profiling. </summary>
      public bool enableProfiling = false;
      #endregion
      /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

      /// <summary> Reset to default values. </summary>
      public void ResetDefaultValues()
      {
        intensity = 1.0f;

        angle = 0.0f;
        aperture = 0.5f;
        offset = 0.0f;
        blurCurve = 3.0f;
        blur = 1.0f;
        distortion = 5.0f;
        distortionScale = 1.0f;
        focusedBrightness = 0.0f;
        focusedContrast = 1.0f;
        focusedGamma = 1.0f;
        focusedHue = 0.0f;
        focusedSaturation = 1.0f;
        unfocusedBrightness = 0.0f;
        unfocusedContrast = 1.0f;
        unfocusedGamma = 1.0f;
        unfocusedHue = 0.0f;
        unfocusedSaturation = 1.0f;
        debugView = false;

        brightness = 0.0f;
        contrast = 1.0f;
        gamma = 1.0f;
        hue = 0.0f;
        saturation = 1.0f;

        affectSceneView = false;
        filterMode = FilterMode.Bilinear;
        whenToInsert = RenderPassEvent.BeforeRenderingPostProcessing;
        ignoreDeltaTimeScale = false;
        enableProfiling = false;
      }
    }
  }
}
