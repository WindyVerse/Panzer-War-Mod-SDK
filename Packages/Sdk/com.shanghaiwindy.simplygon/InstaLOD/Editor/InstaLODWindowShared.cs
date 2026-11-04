/**
 * InstaLODWindowShared.cs (InstaLODUnity)
 *
 * Copyright © 2019 InstaLOD GmbH - All Rights Reserved.
 *
 * Unauthorized copying of this file, via any medium is strictly prohibited.
 * This file and all it's contents are proprietary and confidential.
 *
 * @file InstaLODWindowShared.cs 
 * @copyright 2017-2019 InstaLOD GmbH. All rights reserved.
 * @section License
 */

using System;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Globalization;
using UnityEngine.SceneManagement;

namespace InstaLOD
{
	/// <summary>
	/// The InstaLODEditorWindow provides a base class for all windows created
	/// by the InstaLOD integration.
	/// </summary>
	public abstract class InstaLODEditorWindow : EditorWindow
	{
		public bool isVisible = false;

		public virtual void OnBecameVisible()
		{
			isVisible = true;
		}

		public virtual void OnBecameInvisible()
		{
			isVisible = false;
		}

		/// <summary>
		/// Notifies this instance.
		/// </summary>
		/// <returns>The notify.</returns>
		/// <param name="message">Message.</param>
		public abstract void Notify(string message);
	}

	/// <summary>
	/// The InstaLODMainThreadAction class manages code that should be run 
	/// on the main thread. Every open InstaLODEditorWindow will execute
	/// the scheduled main thread actions in it's Update method.
	/// </summary>
	public static class InstaLODMainThreadAction
	{
		private static List<Action> mainThreadActions = new List<Action>();

		/// <summary>
		/// Runs the main thread actions.
		/// </summary>
		public static void RunMainThreadActions()
		{
			if (System.Threading.Thread.CurrentThread.IsBackground)
				throw new Exception("Cannot run main thread actions on background thread");

			if (InstaLODMainThreadAction.mainThreadActions.Count > 0)
			{
				List<Action> pendingActions = null;

				lock (InstaLODMainThreadAction.mainThreadActions)
				{
					if (InstaLODMainThreadAction.mainThreadActions.Count > 0)
					{
						pendingActions = InstaLODMainThreadAction.mainThreadActions;
						InstaLODMainThreadAction.mainThreadActions = new List<Action>();
					}
				}

				if (pendingActions != null)
				{
					foreach (Action action in pendingActions)
					{
						action();
					}
				}
			}
		}

		/// <summary>
		/// Enqueues the main thread action.
		/// </summary>
		/// <param name="theAction">The action.</param>
		public static void EnqueueMainThreadAction(Action theAction)
		{
			lock (InstaLODMainThreadAction.mainThreadActions)
			{
				InstaLODMainThreadAction.mainThreadActions.Add(theAction);
			}
		}
	}

	/// <summary>
	/// The InstaLODJSONProfileAnalyzeUtilities class contains regex and string functions
	/// to analyze InstaLOD JSON profiles.
	/// </summary>
	public static class InstaLODJSONProfileAnalyzeUtilities
	{
		/// <summary>
		/// Retrieves the value for the specified identifier.
		/// </summary>
		/// <typeparam name="T">The type of the value.</typeparam>
		/// <param name="jsonContent">The JSON content.</param>
		/// <param name="identifier">The identifier.</param>
		/// <param name="defaultValue">The defaultValue if retrieving fails.</param>
		/// <returns>The value.</returns>
		public static T RetrieveJSONValueForIdentifier<T>(string jsonContent, string identifier, T defaultValue)
		{
			if (string.IsNullOrEmpty(jsonContent) || string.IsNullOrEmpty(identifier))
				return defaultValue;

			Func<string, string, string> fnGetRegexCollection = delegate(string regex, string jsonContent)
			{
				Regex check = new Regex(regex);
				MatchCollection match = check.Matches(jsonContent);
				if (match.Count != 1 || !match[0].Success || match[0].Groups.Count != 2)
					return null;
				return match[0].Groups[1].Value;
			};

			if (typeof(T) == typeof(string))
			{
				string result = fnGetRegexCollection(string.Format("\"{0}\"\\s*:\\s\"([^\"]*)\"", identifier), jsonContent);
				return result == null ? defaultValue : (T)Convert.ChangeType(result, typeof(T));
			}
			else if (typeof(T) == typeof(int))
			{
				string result = fnGetRegexCollection(string.Format("\"{0}\"\\s*:\\s([-0-9.]*)", identifier), jsonContent);
				int value = -1;
				if (result == null || !int.TryParse(result, out value))
					return defaultValue;
				return (T)Convert.ChangeType(value, typeof(T));
			}
			else if (typeof(T) == typeof(float))
			{
				string result = fnGetRegexCollection(string.Format("\"{0}\"\\s*:\\s([-0-9.]*)", identifier), jsonContent);
				float value = -1.0f;
				if (result == null || !float.TryParse(result, NumberStyles.Any, CultureInfo.InvariantCulture, out value))
					return defaultValue;
				return (T)Convert.ChangeType(value, typeof(T));
			}
			else if (typeof(T) == typeof(bool))
			{
				string result = fnGetRegexCollection(string.Format("\"{0}\"\\s*:\\s([a-u]*)", identifier), jsonContent);
				bool value = false;
				if (result == null || !bool.TryParse(result, out value))
					return defaultValue;
				return (T)Convert.ChangeType(value, typeof(T));
			}
			else
			{
				Debug.LogError("InstaLOD: Invalid Type.");
			}
			return (T)Convert.ChangeType(null, typeof(T));
		}

		/// <summary>
		/// Determines whether the specified regex matches.
		/// </summary>
		/// <param name="jsonContent">The JSON content.</param>
		/// <param name="regex">The regex.</param>
		/// <returns>True upon success.</returns>
		public static bool HasMatch(string jsonContent, string regex)
		{
			if (string.IsNullOrEmpty(jsonContent) || string.IsNullOrEmpty(regex))
				return false;

			Regex check = new Regex(regex);
			MatchCollection match = check.Matches(jsonContent);

			return match.Count != 0;
		}

		/// <summary>
		/// Gets the matching string for the specified regex.
		/// </summary>
		/// <param name="jsonContent">The JSON content.</param>
		/// <param name="regex">The regex.</param>
		/// <returns>The value or string.empty</returns>
		public static string GetMatch(string jsonContent, string regex)
		{
			if (string.IsNullOrEmpty(jsonContent) || string.IsNullOrEmpty(regex))
				return string.Empty;

			Regex check = new Regex(regex);
			MatchCollection match = check.Matches(jsonContent);
			if (match.Count != 1)
				return string.Empty;

			return match[0].Value;
		}

		/// <summary>
		/// Determines whether the mesh operation entry array has multiple values.
		/// </summary>
		/// <param name="jsonContent">The JSON content.</param>
		/// <returns>True upon success.</returns>
		public static bool HasSingleMeshoperationEntryCount(string jsonContent)
		{
			int curlyBrackets = 0;
			int brackets = 0;

			foreach (char character in jsonContent)
			{
				if (character == '{')
				{
					curlyBrackets++;
				}
				else if (character == '}')
				{
					curlyBrackets--;
				}
				else if (character == '[')
				{
					brackets++;
				}
				else if (character == ']')
				{
					brackets--;
				}

				if (curlyBrackets < 0 || brackets < 0)
				{
					Debug.LogError("InstaLOD: Invalid InstaLOD Profile");
					return false;
				}

				if (brackets == 1 && curlyBrackets == 0 && character == ',')
					return false;
			}

			return true;
		}
	}

	/// <summary>
	/// The InstaLODJSONProfileReadingUtilities class contains functions for reading
	/// InstaLOD JSON profiles.
	/// </summary>
	public static class InstaLODJSONProfileReadingUtilities
	{
		/// <summary>
		/// Determines whether a profile is valid.
		/// </summary>
		/// <param name="jsonContent">The JSON content.</param>
		/// <param name="outErrorMessage">out error message.</param>
		/// <returns>True upon validity.</returns>
		public static bool IsProfileValid(string jsonContent, out string outErrorMessage)
		{
			if (!jsonContent.Contains("\"Version\"") || !jsonContent.Contains("\"Operation\"") || !jsonContent.Contains("\"MeshOperation\"") || !jsonContent.Contains("\"Export\""))
			{
				outErrorMessage = "Invalid InstaLOD Profile.";
				return false;
			}

			try
			{
				string meshOperationEntry = GetMeshOperationEntriesArray(jsonContent);
				if (!InstaLODJSONProfileAnalyzeUtilities.HasSingleMeshoperationEntryCount(meshOperationEntry))
				{
					outErrorMessage = "Only JSON profiles with a single mesh operation are supported.";
					return false;
				}
			}
			catch (Exception exception)
			{
				outErrorMessage = exception.Message;
				return false;
			}

			// check mesh operation type
			string operationType = InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<string>(jsonContent, "Operation", string.Empty);

			if (string.IsNullOrEmpty(operationType))
			{
				outErrorMessage = "Could not retrieve value for key 'Operation'.";
				return false;
			}

			if (!operationType.Equals("MeshOperation", StringComparison.InvariantCultureIgnoreCase))
			{
				outErrorMessage = "Only 'Operation' of type 'MeshOperation' is supported.";
				return false;
			}

			outErrorMessage = string.Empty;
			return true;
		}

		/// <summary>
		/// Gets the type of the mesh operation entry.
		/// </summary>
		/// <param name="jsonContent">The JSON content</param>
		/// <returns>The type.</returns>
		public static string GetMeshOperationEntryType(string jsonContent)
		{
			if (string.IsNullOrEmpty(jsonContent))
				return string.Empty;

			// NOTE: Imposterize billboard type also uses "Type" identifier
			if (InstaLODJSONProfileAnalyzeUtilities.HasMatch(jsonContent, "\"Type\"\\s*:\\s*\"Imposterize\""))
				return "Imposterize";

			string meshoperationType = InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<string>(jsonContent, "Type", string.Empty);
			return string.IsNullOrEmpty(meshoperationType) ? string.Empty : meshoperationType;
		}

		/// <summary>
		/// Gets the mesh operation entry
		/// </summary>
		/// <param name="jsonContent">The JSON content</param>
		/// <returns>The content.</returns>
		public static string GetMeshOperationEntry(string jsonContent)
		{
			if (string.IsNullOrEmpty(jsonContent))
				return string.Empty;

			string regex = "\"Entries\"\\s*:\\s*\\[\\s*{\\s*([\\s|\\S]+)}\\s*\\]";
			Match match = Regex.Match(jsonContent, regex);

			if (!match.Success || match.Groups.Count != 2)
				return string.Empty;

			// NOTE: first group is full match, second group is capture group
			return match.Groups[1].Value;
		}

		/// <summary>
		/// Gets the mesh operation entries array.
		/// </summary>
		/// <param name="jsonContent">THe JSON content</param>
		/// <returns>The content.</returns>
		public static string GetMeshOperationEntriesArray(string jsonContent)
		{
			if (string.IsNullOrEmpty(jsonContent))
				return string.Empty;

			string regex = "\"Entries\"\\s*:\\s*(\\[\\s*{\\s*[\\s|\\S]+}\\s*\\])";
			Match match = Regex.Match(jsonContent, regex);

			if (!match.Success || match.Groups.Count != 2)
				return string.Empty;

			// NOTE: first group is full match, second group is capture group
			return match.Groups[1].Value;
		}

		/// <summary>
		/// Casts an int to an Enum in a safe manner.
		/// </summary>
		/// <typeparam name="T">The Enum type.</typeparam>
		/// <param name="value">The int value to cast.</param>
		/// <param name="defaultValue">Default value if cast fails</param>
		/// <returns>The Enum value.</returns>
		public static T CastIntToEnumSafe<T>(int value, T defaultValue) where T : System.Enum
		{
			if (!Enum.IsDefined(typeof(T), value))
				return defaultValue;

			T result;
			try
			{
				result = (T)Enum.ToObject(typeof(T), value);
			}
			catch (Exception exception)
			{
				Debug.LogError(exception.Message);
				result = defaultValue;
			}

			return result;
		}

		/// <summary>
		/// Casts a string to an Enum in a safe manner.
		/// </summary>
		/// <typeparam name="T">The Enum type.</typeparam>
		/// <param name="value">The string value to cast.</param>
		/// <param name="defaultValue">Default value if cast fails</param>
		/// <returns>The Enum value.</returns>
		public static T CastStringToEnumSafe<T>(string value, T defaultValue) where T : System.Enum
		{
			if (string.IsNullOrEmpty(value))
				return defaultValue;

			T result;
			try
			{
				result = (T)Enum.Parse(typeof(T), value, ignoreCase: true);
			}
			catch (Exception exception)
			{
				Debug.LogError(exception.Message);
				result = defaultValue;
			}

			return result;
		}

		/// <summary>
		/// Reads the profile.
		/// </summary>
		/// <param name="jsonContent">The JSON content.</param>
		/// <param name="settings">The settings struct reference.</param>
		/// <param name="outErrorMessage">Out string for error messages.</param>
		/// <returns>True upon success.</returns>
		public static bool ReadOptimizeProfile(string jsonContent, ref InstaLODOptimizeSettings settings, out string outErrorMessage)
		{
			if (string.IsNullOrEmpty(jsonContent))
			{
				outErrorMessage = "InstaLOD MeshOperation Entry is invalid";
				return false;
			}

			settings.AutomaticQuality = CastStringToEnumSafe<MeshFeatureImportance>(InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<string>(jsonContent, "AutomaticQuality", "Off"), MeshFeatureImportance.Off);
			settings.PercentTriangles = InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<float>(jsonContent, "PercentTriangles", 0.5f) * 100.0f;
			settings.AbsoluteTriangles = (UInt32) InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<int>(jsonContent, "AbsoluteTriangles", 0);
			settings.MaxDeviation = InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<float>(jsonContent, "MaxDeviation", 0.0f);
			settings.WeldingThreshold = InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<float>(jsonContent, "WeldingThreshold", 0.0f);
			settings.ScreenSizeInPixels = (UInt32) InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<int>(jsonContent, "ScreenSizeInPixels", 0);
			settings.RecalculateNormals = InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<bool>(jsonContent, "RecalculateNormals", false);
			settings.HardAngleThreshold = InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<float>(jsonContent, "HardAngleThreshold", 80.0f);
			settings.SilhouetteImportance = CastStringToEnumSafe<MeshFeatureImportance>(InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<string>(jsonContent, "SilhouetteImportance", "Normal"), MeshFeatureImportance.Normal);
			settings.TextureImportance = CastStringToEnumSafe<MeshFeatureImportance>(InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<string>(jsonContent, "TextureImportance", "Normal"), MeshFeatureImportance.Normal);
			settings.ShadingImportance = CastStringToEnumSafe<MeshFeatureImportance>(InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<string>(jsonContent, "ShadingImportance", "Normal"), MeshFeatureImportance.Normal);
			settings.SkinningImportance = CastStringToEnumSafe<MeshFeatureImportance>(InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<string>(jsonContent, "SkinningImportance", "Normal"), MeshFeatureImportance.Normal);

			settings.SkeletonSettings.LeafBoneWeldDistance = InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<float>(jsonContent, "LeafBoneWeldDistance", 0.0f);
			settings.SkeletonSettings.MaximumBoneDepth = (UInt32) InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<int>(jsonContent, "MaximumBoneDepth", 0);
			settings.SkeletonSettings.MaximumBoneInfluencesPerVertex = (UInt32) InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<int>(jsonContent, "MaximumBoneInfluencesPerVertex", 4);
			settings.SkeletonSettings.MinimumBoneInfluenceThreshold = InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<float>(jsonContent, "MinimumBoneInfluenceThreshold", 0.0f);

			settings.Deterministic = InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<bool>(jsonContent, "Deterministic", false);

			outErrorMessage = string.Empty;
			return true;
		}

		/// <summary>
		/// Reads the profile.
		/// </summary>
		/// <param name="jsonContent">The JSON content.</param>
		/// <param name="settings">The settings struct reference.</param>
		/// <param name="outErrorMessage">Out string for error messages.</param>
		/// <returns>True upon success.</returns>
		public static bool ReadRemeshProfile(string jsonContent, ref InstaLODRemeshingSettings settings, out string outErrorMessage)
		{
			if (string.IsNullOrEmpty(jsonContent))
			{
				outErrorMessage = "InstaLOD MeshOperation Entry is invalid";
				return false;
			}

			settings.SurfaceMode = CastStringToEnumSafe<RemeshSurfaceMode>(InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<string>(jsonContent, "SurfaceMode", "Reconstruct"), RemeshSurfaceMode.Reconstruct);
			settings.SurfaceConstructionIgnoreBackface = InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<bool>(jsonContent, "IgnoreBackface", false);
			settings.Resolution = CastStringToEnumSafe<RemeshResolution>(InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<string>(jsonContent, "Resolution", "normal"), RemeshResolution.Normal);
			settings.FaceCountTarget = CastStringToEnumSafe<RemeshFaceCountTarget>(InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<string>(jsonContent, "FaceCountTarget", "normal"), RemeshFaceCountTarget.Normal);
			settings.MaximumTriangles = (UInt32) InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<int>(jsonContent, "MaximumTriangles", 0);
			settings.ScreenSizeInPixels = (UInt32) InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<int>(jsonContent, "ScreenSizeInPixels", 0);
			settings.ScreenSizeInPixelsAutomaticTextureSize = InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<bool>(jsonContent, "ScreenSizeInPixelsAutomaticTextureSize", true);
			settings.ScreenSizePixelMergeDistance = (UInt32) InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<int>(jsonContent, "ScreenSizePixelMergeDistance", 0);
			settings.HardAngleThreshold = InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<float>(jsonContent, "HardAngleThreshold", 80.0f);
			settings.WeldDistance = InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<float>(jsonContent, "WeldDistance", 0.0f);
			settings.GutterSizeInPixels = (UInt32) InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<int>(jsonContent, "GutterSizeInPixels", 5);
			settings.ScreenSizeInPixelsAutomaticTextureSize = InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<bool>(jsonContent, "ScreenSizeInPixelsAutomaticTextureSize", true);
			settings.UnwrapStrategy = CastStringToEnumSafe<UVUnwrapStrategy>(InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<string>(jsonContent, "UnwrapStrategy", "auto"), UVUnwrapStrategy.Auto);
			settings.Deterministic = InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<bool>(jsonContent, "Deterministic", false);

			outErrorMessage = string.Empty;
			return true;
		}

		/// <summary>
		/// Reads the profile.
		/// </summary>
		/// <param name="jsonContent">The JSON content.</param>
		/// <param name="settings">The settings struct reference.</param>
		/// <param name="outErrorMessage">Out string for error messages.</param>
		/// <returns>True upon success.</returns>
		public static bool ReadMaterialMergeProfile(string jsonContent, ref InstaLODMeshMergeSettings settings, out string outErrorMessage)
		{
			if (string.IsNullOrEmpty(jsonContent))
			{
				outErrorMessage = "InstaLOD MeshOperation Entry is invalid";
				return false;
			}

			settings.GutterSizeInPixels = (UInt32) InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<int>(jsonContent, "GutterSizeInPixels", 5);
			settings.SuperSampling = CastStringToEnumSafe<SuperSampling>(InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<string>(jsonContent, "SuperSampling", "Reconstruct"), SuperSampling.x2);
			settings.ShellRotation = CastStringToEnumSafe<UVPackShellRotation>(InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<string>(jsonContent, "ShellRotation", "normal"), UVPackShellRotation.Arbitrary);
			settings.UVImportance = CastStringToEnumSafe<MeshFeatureImportance>(InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<string>(jsonContent, "UVImportance", "normal"), MeshFeatureImportance.Normal);
			settings.GeometricImportance = CastStringToEnumSafe<MeshFeatureImportance>(InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<string>(jsonContent, "GeometricImportance", "normal"), MeshFeatureImportance.Normal);
			settings.TextureImportance = CastStringToEnumSafe<MeshFeatureImportance>(InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<string>(jsonContent, "TextureImportance", "normal"), MeshFeatureImportance.Normal);
			settings.VisualImportance = CastStringToEnumSafe<MeshFeatureImportance>(InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<string>(jsonContent, "VisualImportance", "normal"), MeshFeatureImportance.Normal);
			settings.WorldspaceNormalizeShells = InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<bool>(jsonContent, "WorldspaceNormalizeShells", false);

			outErrorMessage = string.Empty;
			return true;
		}

		/// <summary>
		/// Reads the profile.
		/// </summary>
		/// <param name="jsonContent">The JSON content.</param>
		/// <param name="settings">The settings struct reference.</param>
		/// <param name="outErrorMessage">Out string for error messages.</param>
		/// <returns>True upon success.</returns>
		public static bool ReadImposterizeProfile(string jsonContent, ref InstaLODImposterizeSettings settings, out string outErrorMessage)
		{
			if (string.IsNullOrEmpty(jsonContent))
			{
				outErrorMessage = "InstaLOD MeshOperation Entry is invalid";
				return false;
			}

			settings.GutterSizeInPixels = (UInt32) InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<int>(jsonContent, "GutterSizeInPixels", 5);
		 
			settings.Type = CastStringToEnumSafe<ImposterType>(InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<string>(jsonContent, "Type", "Billboard"), ImposterType.Billboard);
			settings.TwoSidedQuads = InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<bool>(jsonContent, "TwoSidedQuads", true);

			settings.QuadXYCount = (UInt32) InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<int>(jsonContent, "QuadXYCount", 1);
			settings.QuadXZCount = (UInt32) InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<int>(jsonContent, "QuadXZCount", 1);
			settings.QuadYZCount = (UInt32) InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<int>(jsonContent, "QuadYZCount", 1);
			settings.EnableCloudQuadXY = InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<bool>(jsonContent, "EnableCloudQuadXY", true);
			settings.EnableCloudQuadXZ = InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<bool>(jsonContent, "EnableCloudQuadXZ", true);
			settings.EnableCloudQuadYZ = InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<bool>(jsonContent, "EnableCloudQuadYZ", true);
			settings.CloudFaceCount = (UInt32) InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<int>(jsonContent, "CloudFaceCount", 400);
			settings.CloudPolyFaceFactor = InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<float>(jsonContent, "CloudPolyFaceFactor", 0.5f);
			settings.CloudNormalConformity = InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<float>(jsonContent, "CloudNormalConformity", 0.5f);
			settings.QuadSubdivisionsU = (UInt32) InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<int>(jsonContent, "QuadSubdivisionsU", 1);
			settings.QuadSubdivisionsV = (UInt32) InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<int>(jsonContent, "QuadSubdivisionsV", 1);

			settings.AABBDisplacement = InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<float>(jsonContent, "AABBDisplacement", 0.0f);
			settings.FlipbookFramesPerAxis = (UInt32) InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<int>(jsonContent, "FlipbookFramesPerAxis", 8);
			settings.AlphaCutOutResolution = (UInt32) InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<int>(jsonContent, "AlphaCutOutResolution", 8);
			settings.AlphaCutOut = InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<bool>(jsonContent, "AlphaCutOut", false);
			settings.Deterministic = InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<bool>(jsonContent, "Deterministic", false);

			string cloudNormal = InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<string>(jsonContent, "CloudNormal", "0.0 1.0 0.0");
			string[] tokens = cloudNormal.Split(' ');

			if (tokens.Length != 3 ||
				!float.TryParse(tokens[0], out settings.CloudNormal.x) ||
				!float.TryParse(tokens[1], out settings.CloudNormal.y) ||
				!float.TryParse(tokens[2], out settings.CloudNormal.z))
			{
				Debug.LogError("InstaLOD: Value for Identifier 'CloudNormal' is invalid.");
			}

			outErrorMessage = string.Empty;
			return true;
		}

		/// <summary>
		/// Reads the profile.
		/// </summary>
		/// <param name="jsonContent">The JSON content.</param>
		/// <param name="settings">The settings struct reference.</param>
		/// <param name="outErrorMessage">Out string for error messages.</param>
		/// <returns>True upon success.</returns>
		public static bool ReadMTKProfile(string jsonContent, ref InstaLODMeshToolKitSettings settings, out string outErrorMessage)
		{
			if (string.IsNullOrEmpty(jsonContent))
			{
				outErrorMessage = "InstaLOD MeshOperation Entry is invalid";
				return false;
			}

			settings.WeldingThreshold = InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<float>(jsonContent, "WeldingThreshold", 0.0f);
			settings.WeldingNormalAngleThreshold = InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<float>(jsonContent, "WeldingNormalAngleThreshold", 80.0f);
			settings.WeldingBoundaries = InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<bool>(jsonContent, "WeldingBoundaries", false);
			settings.HealTJunctionThreshold = InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<float>(jsonContent, "HealTJunctionThreshold", 0.0f);
			settings.RemoveDegenerateFacesThreshold = InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<float>(jsonContent, "RemoveDegenerateFacesThreshold", 0.0f);
			settings.FixNonManifold = InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<bool>(jsonContent, "FixNonManifold", false);
			settings.ConformNormals = InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<bool>(jsonContent, "ConformNormals", false);
			settings.ConformWindingOrder = InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<bool>(jsonContent, "ConformWindingOrder", false);
			settings.FlipNormals = InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<bool>(jsonContent, "FlipNormals", false);
			settings.FlipWindingOrder = InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<bool>(jsonContent, "FlipWindingOrder", false);
			settings.FillHoles = InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<bool>(jsonContent, "FillHoles", false);
			settings.NormalHealingModeSettings = CastStringToEnumSafe<NormalHealingMode>(InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<string>(jsonContent, "NormalHealingMode", "Off"), NormalHealingMode.Off);
			settings.RecalculateNormals = InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<bool>(jsonContent, "RecalculateNormals", false);
			settings.HardAngleThreshold = InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<float>(jsonContent, "HardAngleThreshold", 80.0f);
			settings.WeightedNormals = InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<bool>(jsonContent, "WeightedNormals", true);
			settings.MinimumSubMeshBoundingSphereRadius = InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<float>(jsonContent, "MinimumSubMeshBoundingSphereRadius", 0.0f);
			settings.MinimumFaceArea = InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<float>(jsonContent, "MinimumFaceArea", 0.0f);
			settings.RemoveSubmeshIndices = InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<bool>(jsonContent, "RemoveSubmeshIndices", false);
			settings.CalculateSubmeshIndicesByAdjacency = InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<bool>(jsonContent, "CalculateSubmeshIndicesByAdjacency", false);
			settings.CalculateSubmeshIndicesByMaterial = InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<bool>(jsonContent, "CalculateSubmeshIndicesByMaterial", false);

			outErrorMessage = string.Empty;
			return true;
		}

		/// <summary>
		/// Reads the profile.
		/// </summary>
		/// <param name="jsonContent">The JSON content.</param>
		/// <param name="settings">The settings struct reference.</param>
		/// <param name="outErrorMessage">Out string for error messages.</param>
		/// <returns>True upon success.</returns>
		public static bool ReadOcclusionCullProfile(string jsonContent, ref InstaLODOcclusionCullSettings settings, out string outErrorMessage)
		{
			if (string.IsNullOrEmpty(jsonContent))
			{
				outErrorMessage = "InstaLOD MeshOperation Entry is invalid";
				return false;
			}

			settings.Mode = CastStringToEnumSafe<OcclusionCullMode>(InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<string>(jsonContent, "Mode", "AutomaticInterior"), OcclusionCullMode.AutomaticInterior);
			settings.CullingStrategy = CastStringToEnumSafe<OcclusionCullingStrategy>(InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<string>(jsonContent, "CullingStrategy", "Face"), OcclusionCullingStrategy.PerFace);
			settings.DataUsage = CastStringToEnumSafe<OcclusionCullDataUsage>(InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<string>(jsonContent, "DataUsage", "RemoveGeometry"), OcclusionCullDataUsage.RemoveGeometry);
			settings.AutomaticPrecision = CastStringToEnumSafe<MeshFeatureImportance>(InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<string>(jsonContent, "AutomaticPrecision", "Normal"), MeshFeatureImportance.Normal);
			settings.Resolution = (UInt32)InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<int>(jsonContent, "Resolution", /*defaultValue:*/1024);
			settings.AdjacencyDepth = (UInt32) InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<int>(jsonContent, "AdjacencyDepth", /*defaultValue:*/5);
			settings.SubMeshVisbilityFaceThreshold = (UInt32) InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<int>(jsonContent, "SubMeshVisbilityFaceThreshold", /*defaultValue:*/1);
			settings.IgnoreBackface = InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<bool>(jsonContent, "IgnoreBackface", /*defaultValue:*/false);
			settings.AlphaMaskThreshold = InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<float>(jsonContent, "AlphaMaskThreshold", /*defaultValue:*/0.5f);
			settings.OptimizerWeight = InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<float>(jsonContent, "OptimizerWeight", /*defaultValue:*/-0.8f);
			settings.Deterministic = InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<bool>(jsonContent, "Deterministic", /*defaultValue:*/false);

			outErrorMessage = string.Empty;
			return true;
		}

		/// <summary>
		/// Reads the profile.
		/// </summary>
		/// <param name="jsonContent">The JSON content.</param>
		/// <param name="settings">The settings struct reference.</param>
		/// <param name="outErrorMessage">Out string for error messages.</param>
		/// <returns>True upon success.</returns>
		public static bool ReadUnwrapProfile(string jsonContent, ref InstaLODUnwrapSettings settings, out string outErrorMessage)
		{
			if (string.IsNullOrEmpty(jsonContent))
			{
				outErrorMessage = "InstaLOD MeshOperation Entry is invalid";
				return false;
			}

			settings.UnwrapStrategy = CastStringToEnumSafe<UVUnwrapStrategy>(InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<string>(jsonContent, "UnwrapStrategy", "Auto"), UVUnwrapStrategy.Auto);
			settings.StretchImportance = CastStringToEnumSafe<MeshFeatureImportance>(InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<string>(jsonContent, "StretchImportance", "Normal"), MeshFeatureImportance.Normal);
			settings.TextureWidth = (UInt32)InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<int>(jsonContent, "TextureWidth", /*defaultValue:*/1024);
			settings.TextureHeight = (UInt32)InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<int>(jsonContent, "TextureHeight", /*defaultValue:*/1024);
			settings.GutterSizeInPixels = (UInt32) InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<int>(jsonContent, "GutterSizeInPixels", /*defaultValue:*/5);
			settings.ShellStitching = InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<bool>(jsonContent, "ShellStitching", /*defaultValue:*/true);
			settings.InsertNormalSplits = InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<bool>(jsonContent, "InsertNormalSplits", /*defaultValue:*/true);
			settings.UVScale = InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<float>(jsonContent, "UVScale", /*defaultValue:*/1.0f);
			settings.PostProcessLayout = InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<bool>(jsonContent, "PostProcessLayout", /*defaultValue:*/true);
			settings.Deterministic = InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<bool>(jsonContent, "Deterministic", /*defaultValue:*/false);

			outErrorMessage = string.Empty;
			return true;
		}

		/// <summary>
		/// Reads the profile.
		/// </summary>
		/// <param name="jsonContent">The JSON content.</param>
		/// <param name="settings">The settings struct reference.</param>
		/// <param name="outErrorMessage">Out string for error messages.</param>
		/// <returns>True upon success.</returns>
		public static bool ReadIsotropicRemeshProfile(string jsonContent, ref InstaLODIsotropicRemeshSettings settings, out string outErrorMessage)
		{
			if (string.IsNullOrEmpty(jsonContent))
			{
				outErrorMessage =  "InstaLOD MeshOperation Entry is invalid";
				return false;
			}

			settings.EdgeMode = CastStringToEnumSafe<IsotropicEdgeMode>(InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<string>(jsonContent, "EdgeMode", "Automatic"), IsotropicEdgeMode.Automatic);
			settings.TargetEdgeSize = InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<float>(jsonContent, "TargetEdgeSize", 1.0f);
			settings.Precision = CastStringToEnumSafe<MeshFeatureImportance>(InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<string>(jsonContent, "Precision", "Normal"), MeshFeatureImportance.Normal);
			settings.WeldingThreshold = InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<float>(jsonContent, "WeldingThreshold", 0.0f);
			settings.GeometricFeatureAngleInDegrees = InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<float>(jsonContent, "GeometricFeatureAngleInDegrees", 0.0f);
			settings.CollapseThreshold = InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<float>(jsonContent, "CollapseThreshold", 75.0f);
			settings.PreserveColorSplits = InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<bool>(jsonContent, "PreserveColorSplits", false);
			settings.PreserveTexCoordSplits = InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<bool>(jsonContent, "PreserveTexCoordSplits", true);
			settings.PreserveNormalSplits = InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<bool>(jsonContent, "PreserveNormalSplits", false);
			settings.PreserveVolume = InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<bool>(jsonContent, "PreserveVolume", false);
			settings.RecalculateNormals = InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<bool>(jsonContent, "RecalculateNormals", false);
			settings.HardAngleThreshold = InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<float>(jsonContent, "HardAngleThreshold", 0.0f);
			settings.WeightedNormals = InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<bool>(jsonContent, "WeightedNormals", false);

			outErrorMessage = string.Empty;
			return true;
		}

		/// <summary>
		/// Reads the profile.
		/// </summary>
		/// <param name="jsonContent">The JSON content.</param>
		/// <param name="settings">The settings struct reference.</param>
		/// <param name="outErrorMessage">Out string for error messages.</param>
		/// <returns>True upon success.</returns>
		public static bool ReadBakeOutputProfile(string jsonContent, ref InstaLODBakeOutputSettings settings, out string outErrorMessage)
		{
			if (string.IsNullOrEmpty(jsonContent))
			{
				outErrorMessage = "InstaLOD MeshOperation Entry is invalid";
				return false;
			}

			settings.SuperSampling = CastStringToEnumSafe<SuperSampling>(InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<string>(jsonContent, "SuperSampling", "x2"), SuperSampling.x2);
			settings.SolidifyTexturePages = InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<bool>(jsonContent, "SolidifyTexturePages", true);
			settings.NormalizePositionByAABB = InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<bool>(jsonContent, "NormalizePositionByAABB", true);

			settings.TexturePageNormalTangentSpace = InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<bool>(jsonContent, "TexturePageNormalTangentSpace", true);
			settings.TexturePageNormalObjectSpace = InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<bool>(jsonContent, "TexturePageNormalObjectSpace", false);
			settings.TexturePageMeshID = InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<bool>(jsonContent, "TexturePageMeshID", false);
			settings.TexturePageVertexColor = InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<bool>(jsonContent, "TexturePageVertexColor", false);
			settings.TexturePageAmbientOcclusion = InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<bool>(jsonContent, "TexturePageAmbientOcclusion", false);
			settings.TexturePageBentNormals = InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<bool>(jsonContent, "TexturePageBentNormals", false);
			settings.TexturePageThickness = InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<bool>(jsonContent, "TexturePageThickness", false);
			settings.TexturePageDisplacement = InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<bool>(jsonContent, "TexturePageDisplacement", false);
			settings.TexturePagePosition = InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<bool>(jsonContent, "TexturePagePosition", false);
			settings.TexturePageCurvature = InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<bool>(jsonContent, "TexturePageCurvature", false);
			settings.TexturePageTransfer = InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<bool>(jsonContent, "TexturePageTransfer", false);
			settings.TexturePageReflectance = InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<bool>(jsonContent, "TexturePageReflectance", false);
			settings.TexturePageOpacity = InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<bool>(jsonContent, "TexturePageOpacity", false);
			settings.TexturePageCustom = InstaLODJSONProfileAnalyzeUtilities.RetrieveJSONValueForIdentifier<bool>(jsonContent, "TexturePageCustom", false);

			outErrorMessage = string.Empty;
			return true;
		}
	}

	/// <summary>
	/// The InstaLODWindowShared class provides functionality that is commonly shared accross
	/// multiple InstaLODEditorWindows.
	/// </summary>
	public static class InstaLODWindowShared
	{
		public static void HelpBox(string message, UnityEditor.MessageType messageType)
		{
			EditorGUILayout.BeginVertical(InstaLODStyle.helpBoxBackgroundStyle);
			EditorGUILayout.HelpBox(message, messageType);
			EditorGUILayout.EndVertical();
		}

		static string currentUsername = String.Empty;
		static string currentPassword = String.Empty;

		public static void OnGUIForNotAuthorized()
		{
			InstaLODWindowShared.HelpBox("InstaLOD requires a valid license.\n" +
				"Please enter your licensing information in the fields below. " +
				"In order to acquire a license an active internet connection is required. " +
				"InstaLOD periodically connects to the InstaLOD servers to validate and refresh the license.", MessageType.Warning);

			GUILayout.Space(10.0f);

			currentUsername = EditorGUILayout.TextField("Account", currentUsername, EditorStyles.textField).Trim();
			currentPassword = EditorGUILayout.PasswordField("Serial/Password", currentPassword, EditorStyles.textField).Trim();

			GUILayout.Space(10.0f);

			if (InstaLODStyle.SecondaryButton("Authorize Workstation"))
			{
				if (string.IsNullOrEmpty(currentUsername) || string.IsNullOrEmpty(currentPassword))
				{
					EditorUtility.DisplayDialog("InstaLOD Authorization Failed",
						"Please enter your license data.", "Ok");
					return;
				}

				if (!InstaLODNative.AuthorizeMachine(currentUsername, currentPassword))
				{
					EditorUtility.DisplayDialog("InstaLOD Authorization Failed",
						"Failed to authorize workstation.\n\nAuthorization Log:\n" + InstaLODNative.GetAuthorizationInformationString(), "Ok");
					return;
				}

				EditorUtility.DisplayDialog("InstaLOD Authorization",
					"This workstation has been successfully authorized.\n\nLicense Information:\n" + InstaLODNative.GetAuthorizationInformationString(), "Ok");

				// clear license data
				currentPassword = "";
			}
		}

		public static void OnGUIForDeauthorize()
		{
			if (InstaLODNative.GetAuthorizationInformationString().Contains("[InstaLOD_NodeLocked]", StringComparison.InvariantCultureIgnoreCase))
			{
				InstaLODWindowShared.HelpBox("Please contact your account manager at InstaLOD to deauthorize your node locked workstation.", MessageType.Warning);
				return;
			}

			InstaLODWindowShared.HelpBox("Deauthorization takes 24 hours to complete. This node will remain locked until the deauthorization is finished.", MessageType.Warning);

			GUILayout.Space(10.0f);

			currentUsername = EditorGUILayout.TextField("Account", currentUsername, EditorStyles.textField).Trim();
			currentPassword = EditorGUILayout.PasswordField("Serial/Password", currentPassword, EditorStyles.textField).Trim();

			GUILayout.Space(10.0f);

			if (InstaLODStyle.SecondaryButton("Deauthorize Workstation"))
			{
				if (string.IsNullOrEmpty(currentUsername) || string.IsNullOrEmpty(currentPassword))
				{
					EditorUtility.DisplayDialog("InstaLOD Deauthorization Failed",
						"Please enter your license data.", "Ok");
					return;
				}

				if (!InstaLODNative.DeauthorizeMachine(currentUsername, currentPassword))
				{
					EditorUtility.DisplayDialog("InstaLOD Deauthorization Failed",
						"Failed to deauthorize workstation.\n\nAuthorization Log:\n" + InstaLODNative.GetAuthorizationInformationString(), "Ok");
					return;
				}

				EditorUtility.DisplayDialog("InstaLOD Deauthorization",
					"This workstation has been successfully deauthorized.\n\nPlease note that it will take 24h until the deauthorization has been completed and the node is activatable on another machine.", "Ok");

				// clear license data
				currentPassword = "";
			}
		}

		#region About GUI
		class AboutGUIState
		{
			public bool licenseInfo = true;
			public bool deauthorize = false;
			public bool authorize = true;
			public bool sdkVersion = false;
		}

		public static void LoadProfile()
		{
			string path = EditorUtility.OpenFilePanel("Open InstaLOD profile", "", "json");

			if (string.IsNullOrEmpty(path))
				return;

			string jsonContent = File.ReadAllText(path);

			if (string.IsNullOrEmpty(jsonContent))
			{
				Debug.LogError(String.Format("InstaLOD: Could not read file '{0}'", Path.GetFileName(path)));
				return;
			}

			string errorMessage = string.Empty;
			if (!InstaLODJSONProfileReadingUtilities.IsProfileValid(jsonContent, out errorMessage))
			{
				Debug.LogError(string.Format("InstaLOD: {0}", errorMessage));
				return;
			}

			string meshOperationType = InstaLODJSONProfileReadingUtilities.GetMeshOperationEntryType(jsonContent);

			if (string.IsNullOrEmpty(meshOperationType))
			{
				Debug.LogError("InstaLOD: Could not retrieve mesh operation type.");
				return;
			}

			string meshoperationEntry = InstaLODJSONProfileReadingUtilities.GetMeshOperationEntry(jsonContent);
			
			if (string.IsNullOrEmpty(meshoperationEntry))
			{
				Debug.LogError("InstaLOD: Could not retrieve mesh operation entry.");
				return;
			}

			InstaLODToolkitWindow window = EditorWindow.GetWindow<InstaLODToolkitWindow>();

			if (window == null)
			{
				Debug.LogError("InstaLOD: Could not open InstaLOD Toolkit window.");
				return;
			}

			if (meshOperationType.Equals("Optimize", StringComparison.InvariantCultureIgnoreCase))
			{
				if (!InstaLODJSONProfileReadingUtilities.ReadOptimizeProfile(meshoperationEntry, ref window.optimizeSettings, out errorMessage))
				{
					Debug.LogError("InstaLOD: Failed to load profile");
					Debug.LogError(string.Format("InstaLOD: {0}", errorMessage));
				}
				window.currentMeshOperationType = InstaLODToolkitWindow.MeshOperationType.Optimize;
				window.operationInformation = InstaLODToolkitWindow.optimizeOperationInformation;
				window.mainComboBoxText = InstaLODToolkitWindow.optimizeOperationButtonLabel;
			}
			else if (meshOperationType.Equals("Remesh", StringComparison.InvariantCultureIgnoreCase))
			{
				if (!InstaLODJSONProfileReadingUtilities.ReadRemeshProfile(meshoperationEntry, ref window.remeshSettings, out errorMessage))
				{
					Debug.LogError("InstaLOD: Failed to load profile");
					Debug.LogError(string.Format("InstaLOD: {0}", errorMessage));
				}
				else
				{
					if (!InstaLODJSONProfileReadingUtilities.ReadBakeOutputProfile(meshoperationEntry, ref window.remeshSettings.BakeOutput, out errorMessage))
					{
						Debug.LogError("InstaLOD: Failed to load Bake Output settings");
						Debug.LogError(string.Format("InstaLOD: {0}", errorMessage));
					}
				}
				window.currentMeshOperationType = InstaLODToolkitWindow.MeshOperationType.Remesh;
				window.operationInformation = InstaLODToolkitWindow.remeshOperationInformation;
				window.mainComboBoxText = InstaLODToolkitWindow.remeshOperationButtonLabel;
			}
			else if (meshOperationType.Equals("Imposterize", StringComparison.InvariantCultureIgnoreCase))
			{
				// NOTE: we need to remove the Type:"Imposterize" key value pair from content
				string matchingString = InstaLODJSONProfileAnalyzeUtilities.GetMatch(meshoperationEntry, "\"Type\"\\s*:\\s*\"Imposterize\"");

				if (string.IsNullOrEmpty(matchingString))
				{
					Debug.LogError("InstaLOD: Failed to load imposterize profile");
					return;
				}
				meshoperationEntry = meshoperationEntry.Replace(matchingString, "");

				if (!InstaLODJSONProfileReadingUtilities.ReadImposterizeProfile(meshoperationEntry, ref window.imposterizeSettings, out errorMessage))
				{
					Debug.LogError("InstaLOD: Failed to load profile");
					Debug.LogError(string.Format("InstaLOD: {0}", errorMessage));
				}
				else
				{
					if (!InstaLODJSONProfileReadingUtilities.ReadBakeOutputProfile(meshoperationEntry, ref window.imposterizeSettings.BakeOutput, out errorMessage))
					{
						Debug.LogError("InstaLOD: Failed to load Bake Output settings");
						Debug.LogError(string.Format("InstaLOD: {0}", errorMessage));
					}
				}
				window.currentMeshOperationType = InstaLODToolkitWindow.MeshOperationType.Imposterize;
				window.operationInformation = InstaLODToolkitWindow.imposterizeOperationInformation;
				window.mainComboBoxText = InstaLODToolkitWindow.imposterizeOperationButtonLabel;
			}
			else if (meshOperationType.Equals("OcclusionCull", StringComparison.InvariantCultureIgnoreCase))
			{
				if (!InstaLODJSONProfileReadingUtilities.ReadOcclusionCullProfile(meshoperationEntry, ref window.occlusionCullSettings, out errorMessage))
				{
					Debug.LogError("InstaLOD: Failed to load profile");
					Debug.LogError(string.Format("InstaLOD: {0}", errorMessage));
				}
				window.currentMeshOperationType = InstaLODToolkitWindow.MeshOperationType.OcclusionCull;
				window.operationInformation = InstaLODToolkitWindow.occlusionCullingOperationInformation;
				window.mainComboBoxText = InstaLODToolkitWindow.occlusionCullingOperationButtonLabel;
			}
			else if (meshOperationType.Equals("MeshToolKit", StringComparison.InvariantCultureIgnoreCase))
			{
				if (!InstaLODJSONProfileReadingUtilities.ReadMTKProfile(meshoperationEntry, ref window.mtkSettings, out errorMessage))
				{
					Debug.LogError("InstaLOD: Failed to load profile");
					Debug.LogError(string.Format("InstaLOD: {0}", errorMessage));
				}
				window.currentMeshOperationType = InstaLODToolkitWindow.MeshOperationType.MeshToolKit;
				window.operationInformation = InstaLODToolkitWindow.meshToolkitOperationInformation;
				window.mainComboBoxText = InstaLODToolkitWindow.meshToolkitOperationButtonLabel;
			}
			else if (meshOperationType.Equals("UVUnwrap", StringComparison.InvariantCultureIgnoreCase))
			{
				if (!InstaLODJSONProfileReadingUtilities.ReadUnwrapProfile(meshoperationEntry, ref window.unwrapSettings, out errorMessage))
				{
					Debug.LogError("InstaLOD: Failed to load profile");
					Debug.LogError(string.Format("InstaLOD: {0}", errorMessage));
				}
				window.currentMeshOperationType = InstaLODToolkitWindow.MeshOperationType.Unwrap;
				window.operationInformation = InstaLODToolkitWindow.unwrapOperationInformation;
				window.mainComboBoxText = InstaLODToolkitWindow.unwrapOperationButtonLabel;
			}
			else if (meshOperationType.Equals("MeshMerge", StringComparison.InvariantCultureIgnoreCase))
			{
				if (!InstaLODJSONProfileReadingUtilities.ReadMaterialMergeProfile(meshoperationEntry, ref window.meshMergeSettings, out errorMessage))
				{
					Debug.LogError("InstaLOD: Failed to load profile");
					Debug.LogError(string.Format("InstaLOD: {0}", errorMessage));
				}
				else
				{
					if (!InstaLODJSONProfileReadingUtilities.ReadBakeOutputProfile(meshoperationEntry, ref window.meshMergeSettings.BakeOutput, out errorMessage))
					{
						Debug.LogError("InstaLOD: Failed to load Bake Output settings");
						Debug.LogError(string.Format("InstaLOD: {0}", errorMessage));
					}
				}
				window.currentMeshOperationType = InstaLODToolkitWindow.MeshOperationType.MaterialMerge;
				window.operationInformation = InstaLODToolkitWindow.materialMergeOperationInformation;
				window.mainComboBoxText = InstaLODToolkitWindow.materialMergeOperationButtonLabel;
			}
			else if (meshOperationType.Equals("IsotropicRemesh", StringComparison.InvariantCultureIgnoreCase))
			{
				if (!InstaLODJSONProfileReadingUtilities.ReadIsotropicRemeshProfile(meshoperationEntry, ref window.isotropicRemeshingSettings, out errorMessage))
				{
					Debug.LogError("InstaLOD: Failed to load profile");
					Debug.LogError(string.Format("InstaLOD: {0}", errorMessage));
				}
				window.currentMeshOperationType = InstaLODToolkitWindow.MeshOperationType.IsotropicRemesh;
				window.operationInformation = InstaLODToolkitWindow.isotropicRemeshOperationInformation;
				window.mainComboBoxText = InstaLODToolkitWindow.isotropicRemeshOperationButtonLabel;
			 }

			if (!string.IsNullOrEmpty(errorMessage))
			{
				Debug.LogError(string.Format("InstaLOD: {0}", errorMessage));
			}
		}

		public static void OnGUIForAbout(InstaLODEditorWindow owner)
		{
			AboutGUIState state = GetEditorGuiState<AboutGUIState>(owner, "AboutGUIState");

			EditorGUILayout.BeginVertical();

			InstaLODStyle.CollapsiblePanel("License Info", ref state.licenseInfo, delegate ()
				{
					GUILayout.TextArea(InstaLODNative.GetAuthorizationInformationString(), InstaLOD.InstaLODStyle.informationTextBoxStyle);
				});

			InstaLODStyle.CollapsiblePanel("SDK Version", ref state.sdkVersion, delegate ()
				{
					GUILayout.Label(InstaLODNative.GetSDKVersionString());
				});

			InstaLODStyle.CollapsiblePanel("Deauthorize", ref state.deauthorize, delegate ()
				{
					InstaLODWindowShared.OnGUIForDeauthorize();
				});

			if (owner is InstaLODToolkitWindow)
			{
				if (InstaLODStyle.SecondaryButton("Reset Settings"))
				{
					owner.Notify("ResetSettings");
				}
			}

			GUILayout.Space(10);

			if (InstaLODStyle.SecondaryButton("Online Help"))
			{
				Application.OpenURL("http://www.InstaLOD.com/GettingStartedWithUnity");
			}

			GUILayout.Space(10);

			if (InstaLODStyle.SecondaryButton("Load Profile"))
			{
				LoadProfile();
			}

			GUILayout.Space(10);

			if (InstaLODStyle.SecondaryButton("Submit Feedback"))
			{
				bool isAuthorized = InstaLODNative.IsAuthorized();
				string licenseInfo = InstaLODNative.GetAuthorizationInformationString();

				if (isAuthorized && licenseInfo.Contains("[InstaLOD_FSL]"))
				{
					Application.OpenURL("https://www.InstaLOD.com/questions");
				}
				else
				{
					string feedbackEmail = "mailto:hello@InstaLOD.com" +
										   "?Subject=InstaLOD Feedback" +
										   "&Body=Please enter your feedback below this line:\n" +
										   "----------------------------------------------------------\n" +
										   "\n\n\n" +
										   "\n\n\n" +
										   "\n\n\n" +
										   "----------------------------------------------------------\n" +
										   "Thank you for submitting feedback!\n" +
										   "\n\n\n" +
										   "App: InstaLOD for Unity\n" +
										   "Version: " + InstaLODNative.PluginVersion + "\n" +
										   "SDK Version: " + InstaLODNative.GetBuildDateString() + "\n" +
										   "License Info:\n" + (isAuthorized ? licenseInfo : "N.A.") + "\n";

					feedbackEmail = feedbackEmail.Replace("\r\n", "%0D%0A");
					feedbackEmail = feedbackEmail.Replace("\n", "%0D%0A");
					feedbackEmail = feedbackEmail.Replace(" ", "%20");
					Application.OpenURL(feedbackEmail);
				}
			}

			EditorGUILayout.EndVertical();
		}
		#endregion

		#region Texture/Material Management
		public struct TexturePageData
		{
			public string texturePageName;
			public Texture texture;
			public bool isNormalMap;
			public bool isSRGB;
		}

		public static List<TexturePageData> GetMaterialTextures(Material material)
		{
			List<TexturePageData> textures = new List<TexturePageData>();
			string assetPath = AssetDatabase.GetAssetPath(material);

			if (String.IsNullOrEmpty(assetPath))
				return textures;

			Shader shader = material.shader;
			for (int i = 0; i < ShaderUtil.GetPropertyCount(shader); i++)
			{
				if (ShaderUtil.GetPropertyType(shader, i) == ShaderUtil.ShaderPropertyType.TexEnv)
				{
					string textureName = ShaderUtil.GetPropertyName(shader, i);
					Texture texture = material.GetTexture(textureName);

					if (texture == null)
						continue;

					string textureAssetPath = AssetDatabase.GetAssetPath(texture);
					bool isNormalMap = false;
					bool isSRGB = false;

					if (!string.IsNullOrEmpty(textureAssetPath) && AssetImporter.GetAtPath(textureAssetPath) is TextureImporter)
					{
						TextureImporter importer = (TextureImporter)TextureImporter.GetAtPath(textureAssetPath);
						isNormalMap = importer.textureType == TextureImporterType.NormalMap;
						isSRGB = importer.sRGBTexture;
					}

					// NOTE: importer may return wrong value for some normal maps (especially if not configured correctly)
					// to avoid this we let the shader property override this 
					if (textureName == "_BumpMap")
					{
						isNormalMap = true;
					}

					if (textureName == "_MainTex" && InstaLODNative.IsRenderPipelineAssetEnabled())
						continue;

					textures.Add(new TexturePageData() { texture = texture, texturePageName = textureName, isNormalMap = isNormalMap, isSRGB = isSRGB });
				}
			}

			return textures;
		}

		/// <summary>
		/// This dictionary conatins states for specific UI pages for each window that is rendering the contents.
		/// </summary>
		private static Dictionary<EditorWindow, Dictionary<string, object>> _editorWindowGUIState = new Dictionary<EditorWindow, Dictionary<string, object>>();

		/// <summary>
		/// Returns the state object for the specified owner with at the key.
		/// </summary>
		/// <typeparam name="T">The type of the state object.</typeparam>
		/// <param name="owner">The owner object.</param>
		/// <param name="key">The key for the state.</param>
		/// <returns>The state object.</returns>
		private static T GetEditorGuiState<T>(EditorWindow owner, string key) where T : new()
		{
			if (_editorWindowGUIState.ContainsKey(owner) == false)
			{
				_editorWindowGUIState[owner] = new Dictionary<string, object>();
			}

			if (_editorWindowGUIState[owner].ContainsKey(key) == false)
			{
				_editorWindowGUIState[owner][key] = new T();
			}

			return (T)_editorWindowGUIState[owner][key];
		}

		class GameObjectSelectionMaterialGUIState
		{
			public GameObjectSelectionMaterialGUIState()
			{
				materialScrollView = new Vector2();
				textureListCollapsiblePanelState = true;
			}

			public Vector2 materialScrollView;
			public bool textureListCollapsiblePanelState;
		}

		class ResultGameObjectGenerationGUIState
		{
			public OperationResultUsage operationResultUsage = OperationResultUsage.Replace;
			public bool resultGameObjectGUIState_UtilityFoldout = true;
			public bool selectChildGameObjects = false;
			public GameObject rootObject = null;
		}

		/// <summary>
		/// The ExpandSelectionGUIState class holds the GUI state of the expand radius setting.
		/// </summary>
		class ExpandSelectionGUIState
		{ 
			public float expandRadius = 0.0f;
		}

		/// <summary>
		/// Collects the LOD group entries from the specified gameobject.
		/// </summary>
		/// <param name="gameObject">The game object.</param>
		/// <param name="list">The list receiving the entry objects.</param>
		/// <param name="isUsingLastLOD">Whether to use the last or first LOD index.</param>
		/// <returns>True upon success.</returns>
		private static bool CollectLODGroupEntries(GameObject gameObject, List<GameObject> list, bool isUsingLastLOD = false)
		{
			if (list == null || gameObject == null)
				return false;

			if (gameObject.GetComponent<LODGroup>() == null)
				return false;

			LODGroup group = gameObject.GetComponent<LODGroup>();

			LOD[] lods = group.GetLODs();

			if (lods.Length == 0)
				return false;

			int lodSourceIndex = isUsingLastLOD ? lods.Length - 1 : 0;
			foreach (Renderer renderer in lods[lodSourceIndex].renderers)
			{
				if (renderer == null)
					continue;

				if (renderer.gameObject != null)
				{
					list.Add(renderer.gameObject);
				}
			}

			return list.Count == 0 ? false : true;
		}

		/// <summary>
		/// Collects the children of a hierarchy beginning at gameObject recursively.
		/// </summary>
		/// <param name="gameObject">The root object</param>
		/// <param name="list">The list receiving the game objects</param>
		/// <param name="iteration">The depth of the recursion</param>
		/// <returns>True upon success.</returns>
		public static bool CollectChildrenRecursive(GameObject gameObject, List<GameObject> list, ref uint iteration)
		{
			if (list == null)
				throw new Exception("List must not be null");

			if (iteration >= 300 || gameObject == null)
				return true;

			iteration++;
			Renderer renderer = gameObject.GetComponent<Renderer>();

			if (renderer != null && gameObject.activeInHierarchy)
			{
				list.Add(gameObject);
			}

			foreach (Transform childTransform in gameObject.transform)
			{
				CollectChildrenRecursive(childTransform.gameObject, list, ref iteration);
			}

			return true;
		}

		/// <summary>
		/// Collects the LODGroup entries of the children.
		/// </summary>
		/// <param name="gameObject">The root object</param>
		/// <param name="list">The list receiving the game objects</param>
		/// <returns>True upon success.</returns>
		private static bool CollectChildLODGroupEntries(GameObject gameObject, List<GameObject> list)
		{
			if (gameObject == null || list == null)
				return false;

			for (int i = 0; i < gameObject.transform.childCount; i++)
			{
				Transform childTransform = gameObject.transform.GetChild(i);
				GameObject childGameObject = childTransform.gameObject;
				
				if (childGameObject == null)
					continue;

				LODGroup group = childGameObject.GetComponent<LODGroup>();
				if (group == null)
					continue;
				
				LOD[] lods = group.GetLODs();
				if (lods == null || lods.Length == 0)
					continue;

				foreach (Renderer renderer in lods[0].renderers)
				{
					if (renderer == null || renderer.gameObject == null)
						continue;

					list.Add(renderer.gameObject);
				}
			}

			return list.Count != 0;
		}

		/// <summary>
		/// Collects the children without LODGroups.
		/// </summary>
		/// <param name="gameObject">The root object</param>
		/// <param name="list">The list receiving the game objects</param>
		/// <returns>True upon success.</returns>
		private static bool CollectFirstChildrenWithoutValidLODGroup(GameObject gameObject, List<GameObject> list)
		{
			if (gameObject == null || list == null)
				return false;

			for (int i = 0; i < gameObject.transform.childCount; i++)
			{
				Transform childTransform = gameObject.transform.GetChild(i);
				GameObject childGameObject = childTransform.gameObject;

				if (childGameObject == null)
					continue;

				LODGroup group = childGameObject.GetComponent<LODGroup>();
				if (group == null)
				{
					list.Add(childGameObject);
					continue;
				}
				LOD[] lods = group.GetLODs();
				if (lods == null || lods.Length == 0)
				{
					list.Add(childGameObject);
					continue;
				}
			}
			
			return true;
		}

		/// <summary>
		/// Retrieves the result usage setting selected in the UI.
		/// </summary>
		/// <param name="owner">The owner object.</param>
		/// <returns>The result usage.</returns>
		public static OperationResultUsage GetResultUsageSetting(EditorWindow owner)
		{
			return GetEditorGuiState<ResultGameObjectGenerationGUIState>(owner, "OnGUIForGameObjectSelectionChildren").operationResultUsage;
		}

		/// <summary>
		/// Retrieves the selected root object.
		/// </summary>
		/// <param name="owner">The owner object.</param>
		/// <returns>The root object.</returns>
		public static GameObject GetRootObject(EditorWindow owner)
		{
			return GetEditorGuiState<ResultGameObjectGenerationGUIState>(owner, "OnGUIForGameObjectSelectionChildren").rootObject;
		}

		/// <summary>
		/// Creates the shared operation view with a list of textures and the utility panel.
		/// </summary>
		/// <param name="owner">The owner.</param>
		/// <param name="spawnWarningsForCameras">Whether to spawn warnings for cameras</param>
		/// <param name="spawnWarningsForMixedRenderers">Whether to spawn warnings for Mixed Renderers</param>
		/// <returns>The list of renderers</returns>
		public static List<Renderer> OnGUIForSharedOperationViews(EditorWindow owner, bool spawnWarningsForCameras, bool spawnWarningsForMixedRenderers, bool showUsageOption)
		{
			HashSet<Material> uniqueMaterials = new HashSet<Material>();
			Shader primaryShader = null;
			string invalidMaterialNames = string.Empty;
			bool isSameShaderForAllMaterials = true;
			List<Renderer> renderers = new List<Renderer>();

			bool hasSkeletalMeshRenderer = false;
			bool hasStaticMeshRenderer = false;

			GameObject[] gameObjectSelection = Selection.gameObjects;

			ExpandSelectionGUIState expandSelection = GetEditorGuiState<ExpandSelectionGUIState>(owner, "OnGUIForExpandSelection");

			ResultGameObjectGenerationGUIState childSelectionState = GetEditorGuiState<ResultGameObjectGenerationGUIState>(owner, "OnGUIForGameObjectSelectionChildren");
			childSelectionState.rootObject = null;

			InstaLODStyle.CollapsiblePanel("Utilities", ref childSelectionState.resultGameObjectGUIState_UtilityFoldout, delegate ()
			{
				childSelectionState.selectChildGameObjects = EditorGUILayout.Toggle(new GUIContent("Select Child Content", "Selects all children inside the selected object"), childSelectionState.selectChildGameObjects);
				
				if (childSelectionState.selectChildGameObjects)
				{
					if (showUsageOption)
					{
						childSelectionState.operationResultUsage = (OperationResultUsage)EditorGUILayout.EnumPopup(new GUIContent("Save As", "Result Usage."), childSelectionState.operationResultUsage);
					}
					else
					{
						childSelectionState.operationResultUsage = OperationResultUsage.Replace;
					}

					if (gameObjectSelection.Length > 0)
					{
						childSelectionState.rootObject = gameObjectSelection[0];
					}

					if (childSelectionState.operationResultUsage == OperationResultUsage.AppendToRootLODChain)
					{
						InstaLODWindowShared.HelpBox("If the root object contains already a LOD Group, InstaLOD selects the meshes in the first LOD Group entry.\n\nPlease note that complex hierarchies may not yield the desired results.", MessageType.Info);

						if (gameObjectSelection.Length > 1)
						{
							InstaLODWindowShared.HelpBox("Selecting multiple root nodes is not possible if the target is the root object.", MessageType.Error);
						}
					}
					else if (childSelectionState.operationResultUsage == OperationResultUsage.AppendToObjectLODChain)
					{
						InstaLODWindowShared.HelpBox("Complex Hierarchies will be ignored, this feature works best on flat hierarchies.", MessageType.Info);
					}
				}

				GUILayout.BeginHorizontal();
				expandSelection.expandRadius = EditorGUILayout.Slider(new GUIContent("Expand Radius", "The expand radius."), expandSelection.expandRadius, 0.0f, 10.0f);
				GUILayout.Space(20);

				bool isGUIEnabled = GUI.enabled;
				GUI.enabled = gameObjectSelection.Length > 0;
				if (InstaLODStyle.SecondaryButton("Expand Selection") )
				{
					Scene activeScene = SceneManager.GetActiveScene();
					GameObject[] sceneObjects = activeScene.GetRootGameObjects();
					bool isSelectionExpanded = InstaLODNative.ExpandSelection(ref gameObjectSelection, expandSelection.expandRadius, sceneObjects);

					if(isSelectionExpanded)
					{
						Selection.objects = gameObjectSelection;
					}
					else
					{
						InstaLODWindowShared.HelpBox("Please select at least one game object with renderer.", MessageType.Warning);
					}
				}
				GUI.enabled = isGUIEnabled;
				GUILayout.EndHorizontal();
			});

			if (childSelectionState.selectChildGameObjects)
			{
				List<GameObject> selectedChildren = new List<GameObject>();
				uint iteration = 0u;

				// NOTE: if we append to root, we need to check if a lod group is already active
				// we will prefer the lod group entries
				if (childSelectionState.operationResultUsage == OperationResultUsage.AppendToObjectLODChain)
				{
					bool needSelectTheChildred = gameObjectSelection.Length != 1;
					if (!needSelectTheChildred)
					{
						// check if this is our valid node
						needSelectTheChildred = gameObjectSelection[0].GetComponent<Renderer>() == null;
						if (!needSelectTheChildred)
						{
							selectedChildren.Add(gameObjectSelection[0]);
						}
					}

					if (needSelectTheChildred)
					{
						CollectFirstChildrenWithoutValidLODGroup(childSelectionState.rootObject, selectedChildren);
						CollectChildLODGroupEntries(childSelectionState.rootObject, selectedChildren);
					}
				}
				else if (childSelectionState.operationResultUsage == OperationResultUsage.Replace ||
					(childSelectionState.operationResultUsage == OperationResultUsage.AppendToRootLODChain &&
					!CollectLODGroupEntries(childSelectionState.rootObject, selectedChildren)))
				{
					foreach (GameObject gameObject in gameObjectSelection)
					{
						CollectChildrenRecursive(gameObject, selectedChildren, ref iteration);
					}
				} 

				gameObjectSelection = selectedChildren.ToArray();
			}

			foreach (GameObject gameObject in gameObjectSelection)
			{
				Renderer renderer = gameObject.GetComponent<Renderer>();

				if (renderer == null)
				{
					// NOTE: we do not display the warning for cameras
					if (gameObject.GetComponent<Camera>() == null || spawnWarningsForCameras)
					{
						InstaLODWindowShared.HelpBox(string.Format("Ignoring game object '{0}' without 'Renderer' component.", gameObject.name), MessageType.Warning);
					}

					continue;
				}

				hasSkeletalMeshRenderer = renderer is SkinnedMeshRenderer;
				hasStaticMeshRenderer = !hasSkeletalMeshRenderer;

				bool isValid = true;

				foreach (Material material in renderer.sharedMaterials)
				{
					if (primaryShader != null && material != null && primaryShader != material.shader)
					{
						invalidMaterialNames += string.Format(" - shader '{0}' used by material '{1}' on game object '{2}'\n", material.shader.name, material.name, gameObject.name);
						isSameShaderForAllMaterials = false;
						isValid = false;
						continue;
					}

					if (material == null)
					{
						invalidMaterialNames += string.Format(" - null material on game object '{0}'\n", gameObject.name);
						isSameShaderForAllMaterials = false;
						isValid = false;
						continue;
					}

					primaryShader = material.shader;
					uniqueMaterials.Add(material);
				}

				if (isValid)
				{
					renderers.Add(renderer);
				}
			}

			if (!isSameShaderForAllMaterials)
			{
				InstaLODWindowShared.HelpBox("All materials applied to selected game objects must use the same shader (" + (primaryShader != null ? primaryShader.name : "null") + ").\n\n" +
					"The following shaders, materials and game objects will be excluded from the operation:\n\n" +
					invalidMaterialNames, MessageType.Warning);
			}

			if (renderers.Count == 0)
				return renderers;

			GameObjectSelectionMaterialGUIState state = GetEditorGuiState<GameObjectSelectionMaterialGUIState>(owner, "OnGUIForGameObjectSelectionMaterials");

			int textureCount = 0;

			foreach (Material material in uniqueMaterials)
			{
				List<TexturePageData> textures = GetMaterialTextures(material);
				textureCount += textures.Count;
			}

			state.textureListCollapsiblePanelState = InstaLODStyle.CollapsiblePanelHeader(state.textureListCollapsiblePanelState, string.Format("{0} {1} and {2} {3} in the selection",
					textureCount, textureCount == 1 ? "Texture" : "Textures",
					renderers.Count, renderers.Count == 1 ? "Mesh" : "Meshes"));

			if (state.textureListCollapsiblePanelState)
			{
				InstaLODStyle.BeginCollapsiblePanelBody();
				
				state.materialScrollView = GUILayout.BeginScrollView(state.materialScrollView, false, false, GUILayout.MinHeight(100));
				foreach (Material material in uniqueMaterials)
				{
					List<TexturePageData> textures = GetMaterialTextures(material);

					GUILayout.Space(5.0f);
					foreach (TexturePageData textureData in textures)
					{
						EditorGUILayout.BeginHorizontal(InstaLODStyle.listItemGroupStyle);
						GUILayout.Label(string.Format("{0}\n     {1} on {2}", textureData.texture.name, textureData.texturePageName, material.name));

						GUILayout.Label(new GUIContent(textureData.texture), new GUIStyle() { fixedHeight = 32, fixedWidth = 32 });

						EditorGUILayout.EndHorizontal();
						GUILayout.Space(5.0f);
					}
				}
				GUILayout.EndScrollView();

				InstaLODStyle.EndCollapsiblePanelBody();
			}

			if (spawnWarningsForMixedRenderers && hasStaticMeshRenderer && hasSkeletalMeshRenderer)
			{
				InstaLODWindowShared.HelpBox("Combining static meshes and skinned meshes into a single output mesh results in the combined static mesh geometry being invisible due to missing skinning weights for the static mesh geometry.", MessageType.Error);
			}

			return renderers;
		}

		#endregion

		class MeshOperationSettingsGUIState
		{
			public bool optimizeUIState_Settings = true;
			public bool optimizeUIState_FeatureImportance = true;
			public bool optimizeUIState_NormalRecalculation = false;
			public bool optimizeUIState_VertexWelding = false;
			public bool optimizeUIState_Advanced = true;
			public bool optimizeUIState_SkeletonOptimization = true;

			public bool imposterizeUIState_SettingsFoldout = true;
			public bool imposterizeUIState_DetailSettingsFoldout = true;
			public bool imposterizeUIState_AlphaCutoutSettingsFoldout = false;
			public bool imposterizeUIState_AdvancedSettingsFoldout = true;

			public bool occlusionCullUIState_SettingsFoldout = true;

			public bool materialMergeUIState_FeatureImportanceFoldout = true;
			public bool materialMergeUIState_AdvancedSettingsFoldout = true;
			public bool occlusionCullUIState_AdvancedSettingsFoldout = true;

			public bool materialMergeUIState_SettingsFoldout = true;

			public bool bakeOutputUIState_SettingsFoldout = true;

			public bool remeshUIState_SettingsFoldout = true;
			public bool remeshUIState_DetailSettingsFoldout = true;
			public bool remeshUIState_UVSettingsFoldout = true;
			public bool remeshUIState_AdvancedSettingsFoldout = true;

			public bool mtkUIState_SettingsFoldout = true;
			public bool mtkUIState_AdvancedFoldout = true;

			public bool unwrapUIState_SettingsFoldout = true;
			public bool unwrapUIState_DetailSettingsFoldout = true;
			public bool unwrapUIState_AdvancedSettingsFoldout = true;

			public bool isoremeshUIState_SettingsFoldout = true;
			public bool isoremeshUIState_FeaturePreservationFoldout = true;
			public bool isoremeshUIState_NormalRecalculationFoldout = true;
			public bool isoremeshUIState_AdvancedFoldout = true;
		}

		public static void OnGUIForFooter(EditorWindow owner)
		{
			GUILayout.Space(30.0f);

			GUIStyle footerStyle = new GUIStyle() { fontSize = 10, alignment = TextAnchor.MiddleCenter };
			footerStyle.normal.textColor = InstaLODStyle.controlGreyColor;

			GUILayout.Label("InstaLOD GmbH 2016-2023", footerStyle);
			if (GUILayout.Button("http://www.InstaLOD.com", footerStyle))
			{
				Application.OpenURL("Http://www.InstaLOD.com");
			}
			GUILayout.Label(InstaLODNative.PluginVersion, footerStyle);
		}

		public static void OnUIForBakeOutput(EditorWindow owner, ref InstaLODBakeOutputSettings bakeOutput)
		{
			MeshOperationSettingsGUIState state = GetEditorGuiState<MeshOperationSettingsGUIState>(owner, "OptimizeSettingsGUIState");
			InstaLODBakeOutputSettings tempBakeOutput = bakeOutput;

			InstaLODStyle.CollapsiblePanel("Bake Output Settings", ref state.bakeOutputUIState_SettingsFoldout, delegate ()
				{
					tempBakeOutput.Width = (uint)(Resolution)EditorGUILayout.EnumPopup(new GUIContent("Width", "The output texture width."), InstaLODJSONProfileReadingUtilities.CastIntToEnumSafe<Resolution>( (int) tempBakeOutput.Width, Resolution._2048));
					tempBakeOutput.Height = (uint)(Resolution)EditorGUILayout.EnumPopup(new GUIContent("Height", "The output texture height."), InstaLODJSONProfileReadingUtilities.CastIntToEnumSafe<Resolution>( (int) tempBakeOutput.Height, Resolution._2048));

					tempBakeOutput.SourceMeshUVChannelIndex = (uint)EditorGUILayout.IntSlider(new GUIContent("Source Mesh UV Channel", "The UV channel of the source mesh used when sampling texture pages."), (int)tempBakeOutput.SourceMeshUVChannelIndex, 0, 3);
					tempBakeOutput.ExportFormat = (ExportFormats)EditorGUILayout.EnumPopup(new GUIContent("ExportFormat", "The texture output format when exporting textures of the specified depth."), tempBakeOutput.ExportFormat);

					if (tempBakeOutput.ExportFormat == ExportFormats.JPEG)
					{
						tempBakeOutput.CompressionQuality = (uint)EditorGUILayout.IntSlider(new GUIContent("Compression Quality", "The JPEG quality"), (int)tempBakeOutput.CompressionQuality, 0, 100);
					}

					tempBakeOutput.SuperSampling = (SuperSampling)EditorGUILayout.EnumPopup(new GUIContent("SuperSampling", "Super sampling."), tempBakeOutput.SuperSampling);
					tempBakeOutput.SolidifyTexturePages = EditorGUILayout.Toggle(new GUIContent("Solidify TexturePages", "Enables solidification of texture pages to avoid pixel bleed."), tempBakeOutput.SolidifyTexturePages);

					if (tempBakeOutput.TexturePagePosition)
					{
						tempBakeOutput.NormalizePositionByAABB = EditorGUILayout.Toggle(new GUIContent("Normalize Position by AABB", "Normalizes the Position map by the AABB"), tempBakeOutput.NormalizePositionByAABB);
					}

					GUILayout.Space(10.0f);

					EditorGUILayout.BeginHorizontal();
					tempBakeOutput.TexturePageNormalTangentSpace = EditorGUILayout.Toggle(new GUIContent("NormalTangentSpace", "8bpp dithered RGB, Name: NormalTangentSpace"), tempBakeOutput.TexturePageNormalTangentSpace);
					tempBakeOutput.TexturePageNormalObjectSpace = EditorGUILayout.Toggle(new GUIContent("NormalObjectSpace", "8bpp dithered RGB, Name: NormalObjectSpace"), tempBakeOutput.TexturePageNormalObjectSpace);
					EditorGUILayout.EndHorizontal();

					EditorGUILayout.BeginHorizontal();
					tempBakeOutput.TexturePageMeshID = EditorGUILayout.Toggle(new GUIContent("MeshID", "8bpp RGB, Name: MeshID"), tempBakeOutput.TexturePageMeshID);
					tempBakeOutput.TexturePageVertexColor = EditorGUILayout.Toggle(new GUIContent("VertexColor", "8bpp dithered RGBA, Name: VertexColor"), tempBakeOutput.TexturePageVertexColor);
					EditorGUILayout.EndHorizontal();

					EditorGUILayout.BeginHorizontal();
					tempBakeOutput.TexturePageAmbientOcclusion = EditorGUILayout.Toggle(new GUIContent("AmbientOcclusion", "8bpp LUMINANCE, Name: AmbientOcclusion"), tempBakeOutput.TexturePageAmbientOcclusion);
					tempBakeOutput.TexturePageThickness = EditorGUILayout.Toggle(new GUIContent("Thickness", "8bpp dithered LUMINANCE, Name: Thickness"), tempBakeOutput.TexturePageThickness);
					EditorGUILayout.EndHorizontal();

					EditorGUILayout.BeginHorizontal();
					tempBakeOutput.TexturePageCustom = EditorGUILayout.Toggle(new GUIContent("Custom", "Output textures configured in materials."), tempBakeOutput.TexturePageCustom);
					tempBakeOutput.TexturePagePosition = EditorGUILayout.Toggle(new GUIContent("Position", "8bpp dithered RGB, Name: Position"), tempBakeOutput.TexturePagePosition);
					EditorGUILayout.EndHorizontal();

					EditorGUILayout.BeginHorizontal();
					tempBakeOutput.TexturePageDisplacement = EditorGUILayout.Toggle(new GUIContent("Displacement", "8bpp dithered LUMINANCE, Name: Displacement "), tempBakeOutput.TexturePageDisplacement);
					tempBakeOutput.TexturePageCurvature = EditorGUILayout.Toggle(new GUIContent("Curvature", "8bpp dithered LUMINANCE, Name: Curvature"), tempBakeOutput.TexturePageCurvature);
					EditorGUILayout.EndHorizontal();

					EditorGUILayout.BeginHorizontal();
					tempBakeOutput.TexturePageTransfer = EditorGUILayout.Toggle(new GUIContent("Transfer", "8bpp dithered RGB, Name: Transfer"), tempBakeOutput.TexturePageTransfer);
					tempBakeOutput.TexturePageOpacity = EditorGUILayout.Toggle(new GUIContent("Opacity", "8bpp LUMINANCE, Name: Opacity"), tempBakeOutput.TexturePageOpacity);
					EditorGUILayout.EndHorizontal();

					EditorGUILayout.BeginHorizontal();
					tempBakeOutput.TexturePageBentNormals = EditorGUILayout.Toggle(new GUIContent("BentNormals", "8bpp dithered RGB, Name: BentNormals"), tempBakeOutput.TexturePageBentNormals);
					tempBakeOutput.TexturePageReflectance = EditorGUILayout.Toggle(new GUIContent("Reflectance", "8bpp LUMINANCE, Name: Reflectance"), tempBakeOutput.TexturePageReflectance);
					EditorGUILayout.EndHorizontal();
				});

			bakeOutput = tempBakeOutput;
		}

		public static void OnGUIForRemeshSettings(EditorWindow owner, ref InstaLODRemeshingSettings inRemeshSettings)
		{
			MeshOperationSettingsGUIState state = GetEditorGuiState<MeshOperationSettingsGUIState>(owner, "OptimizeSettingsGUIState");

			InstaLODRemeshingSettings remeshSettings = inRemeshSettings;

			bool wasGUIEnabled = GUI.enabled;
			bool isUVModeEnabled = false;

			InstaLODStyle.CollapsiblePanel("Settings", ref state.remeshUIState_SettingsFoldout, delegate ()
				{
					remeshSettings.SurfaceMode = (RemeshSurfaceMode)EditorGUILayout.EnumPopup(new GUIContent("Mode", "The surface construction mode."), remeshSettings.SurfaceMode);
					isUVModeEnabled = remeshSettings.SurfaceMode == RemeshSurfaceMode.UV;

					if (!isUVModeEnabled)
					{
						GUI.enabled = remeshSettings.ScreenSizeInPixels == 0 && remeshSettings.MaximumTriangles == 0;
						remeshSettings.FaceCountTarget = (RemeshFaceCountTarget)EditorGUILayout.EnumPopup(new GUIContent("Fuzzy Face Count Target", "The fuzzy face count target of the output geometry."), remeshSettings.FaceCountTarget);

						GUI.enabled = remeshSettings.ScreenSizeInPixels == 0;
						remeshSettings.MaximumTriangles = (uint)EditorGUILayout.IntField(new GUIContent("Maximum Triangles", "The maximum amount of polygons for the target mesh. If set, this value precedes Face Count Target."), (int)remeshSettings.MaximumTriangles);

						GUI.enabled = remeshSettings.MaximumTriangles == 0;
						remeshSettings.ScreenSizeInPixels = (uint)EditorGUILayout.IntSlider(new GUIContent("Screensize in Pixels", "If set, InstaLOD calculates the amount of polygons to remove based on the display size of the output model. If set, this value precedes Face Count Target."), (int)remeshSettings.ScreenSizeInPixels, 0, 1024);

						if (remeshSettings.ScreenSizeInPixels > 0)
						{
							if (remeshSettings.SurfaceMode == RemeshSurfaceMode.Reconstruct)
							{
								remeshSettings.ScreenSizePixelMergeDistance = (uint)EditorGUILayout.IntSlider(new GUIContent("Pixel Merge Distance", "When 'Mode' is set to 'Reconstruct' and 'ScreenSizeInPixels' is enabled, the pixel distance in screen size that will be merged together.\n" +
															 "This can be used to avoid the construction of unnecessary geometrical detail.\n" +
															 "NOTE: When 'ScreenSizeInPixels' is enabled, the resolution is automatically based on 'ScreenSizeInPixels' and 'ScreenSizePixelMergeDistance'."), (int)remeshSettings.ScreenSizePixelMergeDistance, 1, 16);
							}
							remeshSettings.ScreenSizeInPixelsAutomaticTextureSize = EditorGUILayout.Toggle(new GUIContent("Automatic Texture Size", "When 'ScreenSizeInPixels' is enabled, automatically compute output texture dimensions based on screen size in pixels."), remeshSettings.ScreenSizeInPixelsAutomaticTextureSize);
						}
					}

					GUI.enabled = wasGUIEnabled;
				});

			if (!isUVModeEnabled)
			{
				InstaLODStyle.CollapsiblePanel("Surface Construction", ref state.remeshUIState_DetailSettingsFoldout, delegate ()
				{
					if (remeshSettings.SurfaceMode == RemeshSurfaceMode.Reconstruct)
					{
						GUI.enabled = wasGUIEnabled && remeshSettings.ScreenSizeInPixels == 0;
						remeshSettings.Resolution = (RemeshResolution)EditorGUILayout.EnumPopup(new GUIContent("Resolution", "The resolution of the remeshing process."), remeshSettings.Resolution);
						GUI.enabled = wasGUIEnabled;

						remeshSettings.SurfaceConstructionIgnoreBackface = EditorGUILayout.Toggle(new GUIContent("Ignore Backfaces", "When 'Mode' is set to 'Reconstruct', ignores backfaces during surface construction.\n" +
													 "NOTE: This can cause holes in the constructed geometry if face normals are not pointing outwards."), remeshSettings.SurfaceConstructionIgnoreBackface);
					}
					else if (remeshSettings.SurfaceMode == RemeshSurfaceMode.Optimize)
					{
						remeshSettings.SurfaceModeOptimizeLockBoundaries = EditorGUILayout.Toggle(new GUIContent("Lock Boundaries", "Locks boundary vertices when surface construction mode is set to 'Optimize'."), remeshSettings.SurfaceModeOptimizeLockBoundaries);
					}

					remeshSettings.BakeAverageNormalsAsRayDirections = EditorGUILayout.Toggle(new GUIContent("Bake Average Normals", "Uses average normals as ray direction for baking."), remeshSettings.BakeAverageNormalsAsRayDirections);
					remeshSettings.HardAngleThreshold = EditorGUILayout.Slider(new GUIContent("Hard Angle Threshold", "Smooth faces if the normal angle is below this value (in degrees)."), remeshSettings.HardAngleThreshold, 0, 180);
					remeshSettings.WeldDistance = Mathf.Max(0.0f, EditorGUILayout.FloatField(new GUIContent("Weld Distance", "The welding distance can be used to weld holes in the input geometry. The welded mesh is only used for the surface construction it is not used during baking."), remeshSettings.WeldDistance));
				});
			}

			InstaLODStyle.CollapsiblePanel("UV", ref state.remeshUIState_UVSettingsFoldout, delegate ()
			{
				remeshSettings.GutterSizeInPixels = (uint)EditorGUILayout.IntSlider(new GUIContent("Gutter Size In Pixels", "The minimum distance between two UV shells in pixels."), (int)remeshSettings.GutterSizeInPixels, 1, 32);
				remeshSettings.UnwrapStrategy = (UVUnwrapStrategy)EditorGUILayout.EnumPopup(new GUIContent("Unwrap Strategy", "Strategy used during unwrap."), remeshSettings.UnwrapStrategy);
			});

			InstaLODWindowShared.OnUIForBakeOutput(owner, ref remeshSettings.BakeOutput);

            InstaLODStyle.CollapsiblePanel("Advanced", ref state.remeshUIState_AdvancedSettingsFoldout, delegate ()
            {
                remeshSettings.Deterministic = EditorGUILayout.Toggle(new GUIContent("Deterministic Execution", "Makes the algorithm deterministic at the cost of speed."), remeshSettings.Deterministic);
            });

			inRemeshSettings = remeshSettings;
		}

		public static void OnGUIForMeshMergeSettings(EditorWindow owner, ref InstaLODMeshMergeSettings inMeshMergeSettings)
		{
			MeshOperationSettingsGUIState state = GetEditorGuiState<MeshOperationSettingsGUIState>(owner, "OptimizeSettingsGUIState");

			InstaLODMeshMergeSettings meshMergeSettings = inMeshMergeSettings;

			InstaLODStyle.CollapsiblePanel("Settings", ref state.materialMergeUIState_SettingsFoldout, delegate ()
				{
					meshMergeSettings.BakeOutput.Width = (uint)(Resolution)EditorGUILayout.EnumPopup(new GUIContent("Width", "The output texture width."), InstaLODJSONProfileReadingUtilities.CastIntToEnumSafe<Resolution>( (int) meshMergeSettings.BakeOutput.Width, Resolution._2048));
					meshMergeSettings.BakeOutput.Height = (uint)(Resolution)EditorGUILayout.EnumPopup(new GUIContent("Height", "The output texture height."), InstaLODJSONProfileReadingUtilities.CastIntToEnumSafe<Resolution>( (int) meshMergeSettings.BakeOutput.Height, Resolution._2048));
					meshMergeSettings.GutterSizeInPixels = (uint)EditorGUILayout.IntSlider(new GUIContent("Gutter Size In Pixels", "The minimum distance between two UV shells in pixels."), (int)meshMergeSettings.GutterSizeInPixels, 1, 32);
					meshMergeSettings.WorldspaceNormalizeShells = EditorGUILayout.Toggle(new GUIContent("Worldpsace Normalize", "Ensure that output uv shells are scaled according to worldspace geometry."), meshMergeSettings.WorldspaceNormalizeShells);
					meshMergeSettings.GenerateZeroAreaUV = EditorGUILayout.Toggle(new GUIContent("Generate Zero Area UV", "Creates a basic UV for 0-area triangle to allow copying solid color information."), meshMergeSettings.GenerateZeroAreaUV);
					
					if (meshMergeSettings.GenerateZeroAreaUV)
					{
						meshMergeSettings.ZeroAreaUVThreshold = Mathf.Max(0.0f, EditorGUILayout.FloatField(new GUIContent("Zero Area UV Threshold", "UV faces with an area smaller or equal to this value will be considered zero area."), meshMergeSettings.ZeroAreaUVThreshold));
					}

					meshMergeSettings.SuperSampling = (SuperSampling)EditorGUILayout.EnumPopup(new GUIContent("SuperSampling", "Super sampling."), meshMergeSettings.SuperSampling);
					meshMergeSettings.ShellRotation = (UVPackShellRotation)EditorGUILayout.EnumPopup(new GUIContent("ShellRotation", "Enable to allow shells to be rotated for improved packing. Disabling rotations can cause certain scenarios to be unpackable."), meshMergeSettings.ShellRotation);
					meshMergeSettings.StackDuplicateShells = EditorGUILayout.Toggle(new GUIContent("StackDuplicateShells", "Places duplicate UV shells referencing the same material ID on top of each other to save UV space."), meshMergeSettings.StackDuplicateShells);
				});

			InstaLODStyle.CollapsiblePanel("Feature Importance", ref state.materialMergeUIState_FeatureImportanceFoldout, delegate ()
				{
					meshMergeSettings.UVImportance = (MeshFeatureImportance)EditorGUILayout.EnumPopup(new GUIContent("UV Importance", "Determines importance of weights generated by evaluating occupied UV space."), meshMergeSettings.UVImportance);
					meshMergeSettings.GeometricImportance = (MeshFeatureImportance)EditorGUILayout.EnumPopup(new GUIContent("Geometric Importance", "Determines importance of weights generated by evaluating worldspace geometry."), meshMergeSettings.GeometricImportance);
					meshMergeSettings.TextureImportance = (MeshFeatureImportance)EditorGUILayout.EnumPopup(new GUIContent("Texture Importance", "Determines importance of weights generated by evaluating input texture dimensions."), meshMergeSettings.TextureImportance);
					meshMergeSettings.VisualImportance = (MeshFeatureImportance)EditorGUILayout.EnumPopup(new GUIContent("Visual Importance", "Determines importance of weights generated by evaluating visual importance."), meshMergeSettings.VisualImportance);
				});

			InstaLODStyle.CollapsiblePanel("Advanced", ref state.materialMergeUIState_AdvancedSettingsFoldout, delegate ()
			{
				meshMergeSettings.Deterministic = EditorGUILayout.Toggle(new GUIContent("Deterministic Execution", "Makes the algorithm deterministic at the cost of speed."), meshMergeSettings.Deterministic);
			});

			inMeshMergeSettings = meshMergeSettings;
        }

		public static List<Camera> OnGUIForOcclusionCullSettings(EditorWindow owner, ref InstaLODOcclusionCullSettings inOcclusionCullSettings)
		{
			MeshOperationSettingsGUIState state = GetEditorGuiState<MeshOperationSettingsGUIState>(owner, "OptimizeSettingsGUIState");

			List<Camera> cameras = new List<Camera>();

			InstaLODOcclusionCullSettings occlusionCullSettings = inOcclusionCullSettings;

			bool isSkeletalMeshSelected = new List<GameObject>(Selection.gameObjects).Find(item => item.GetComponent<SkinnedMeshRenderer>() != null) ? true : false;

			foreach (GameObject gameObject in Selection.gameObjects)
			{
				Camera camera = gameObject.GetComponent<Camera>();

				if (camera == null)
					continue;

				cameras.Add(camera);
			}

			if (occlusionCullSettings.Mode == OcclusionCullMode.CameraBased && cameras.Count == 0)
			{
				InstaLODWindowShared.HelpBox("Select at least one game object with a 'Camera' component.", MessageType.Error);
			}

			if (isSkeletalMeshSelected)
			{
				InstaLODWindowShared.HelpBox("Performing occlusion culling on 'SkinnedMeshRenderer' components requires baking the mesh. A new bindpose will be created for 'SkinnedMeshRenderer' components based on the current animation state.", MessageType.Info);
			}

			InstaLODStyle.CollapsiblePanel("Settings", ref state.occlusionCullUIState_SettingsFoldout, delegate ()
				{
					occlusionCullSettings.Mode = (OcclusionCullMode)EditorGUILayout.EnumPopup(new GUIContent("Mode", "The operation mode."), occlusionCullSettings.Mode);
					occlusionCullSettings.CullingStrategy = (OcclusionCullingStrategy)EditorGUILayout.EnumPopup(new GUIContent("Culling Strategy", "The culling strategy."), occlusionCullSettings.CullingStrategy);
					occlusionCullSettings.DataUsage = (OcclusionCullDataUsage)EditorGUILayout.EnumPopup(new GUIContent("Data Usage", "The occlusion data usage."), occlusionCullSettings.DataUsage);

					occlusionCullSettings.Resolution = (uint)EditorGUILayout.IntField(new GUIContent("Resolution", "The resolution of the rasterization. Higher resolutions improve quality by reducing the amount of missed faces due to subpixel inaccuracies.\n" +
												 "Dense geometry benefits from a higher resolution. However, a low resolution can be accommodated by increasing 'AdjacencyDepth' or 'AutomaticPrecision'."), (int)occlusionCullSettings.Resolution);

					if (occlusionCullSettings.Mode == OcclusionCullMode.AutomaticInterior)
						occlusionCullSettings.AutomaticPrecision = (MeshFeatureImportance)EditorGUILayout.EnumPopup(new GUIContent("Precision", "When 'Mode' is set to 'AutomaticInterior': the precision of the automatic interior culling."), occlusionCullSettings.AutomaticPrecision);

					if (occlusionCullSettings.DataUsage == OcclusionCullDataUsage.WriteOptimizerWeightsToVertexColors)
						occlusionCullSettings.OptimizerWeight = EditorGUILayout.Slider(new GUIContent("Optimizer Weight Value", "When 'DataUsage' is set to 'WriteOptimizerWeights' or 'WriteOptimizerWeightsToWedgeColors':\n" +
												 "Optimizer weight value[-1...1] assigned to occluded faces."), occlusionCullSettings.OptimizerWeight, -1.0f, 1.0f);

					occlusionCullSettings.AdjacencyDepth = (uint)EditorGUILayout.IntSlider(new GUIContent("Adjacency Depth", "When 'CullingStrategy' is set to 'Face': the recursion depth when protecting neighbors of visible faces." +
												"This can be used to prevent holes in outside geometry that occured due to a low resolution or rotation count.\n" +
												 "0: all faces that are not visible during rasterization will be culled.\n" +
												 "1: direct neighbors of faces that are visible will also be marked visible.\n" +
												 "2..: neighbors of neighbors will also be marked visible."), (int)occlusionCullSettings.AdjacencyDepth, 0, 32);

					if (occlusionCullSettings.CullingStrategy != OcclusionCullingStrategy.PerFace)
						occlusionCullSettings.SubMeshVisbilityFaceThreshold = (uint)EditorGUILayout.IntField(new GUIContent("SubMesh Visbility Face Threshold", "A submesh is marked visible if it's visible face count is greater or equal this value."), (int)occlusionCullSettings.SubMeshVisbilityFaceThreshold);

					occlusionCullSettings.AlphaMaskThreshold = EditorGUILayout.Slider(new GUIContent("AlphaMask Threshold", "Alpha mask values equal or below this threshold will be considered transparent."), occlusionCullSettings.AlphaMaskThreshold, 0.0f, 1.0f);
				});

			InstaLODStyle.CollapsiblePanel("Advanced", ref state.occlusionCullUIState_AdvancedSettingsFoldout, delegate ()
			{
				occlusionCullSettings.Deterministic = EditorGUILayout.Toggle(new GUIContent("Deterministic Execution", "Makes the algorithm deterministic at the cost of speed."), occlusionCullSettings.Deterministic);
			});

			inOcclusionCullSettings = occlusionCullSettings;

			return cameras;
		}

		public static Camera OnGUIForImposterizeSettings(EditorWindow owner, ref InstaLODImposterizeSettings inImposterizeSettings)
		{
			MeshOperationSettingsGUIState state = GetEditorGuiState<MeshOperationSettingsGUIState>(owner, "OptimizeSettingsGUIState");

			InstaLODImposterizeSettings imposterizeSettings = inImposterizeSettings;
			Camera camera = null;

			InstaLODStyle.CollapsiblePanel("Settings", ref state.imposterizeUIState_SettingsFoldout, delegate ()
				{
					imposterizeSettings.Type = (ImposterType)EditorGUILayout.EnumPopup(new GUIContent("Type", "The type of the imposter."), imposterizeSettings.Type);
					imposterizeSettings.GutterSizeInPixels = (uint)EditorGUILayout.IntSlider(new GUIContent("Gutter Size In Pixels", "The minimum distance between two UV shells in pixels."), (int)imposterizeSettings.GutterSizeInPixels, 1, 32);
				});
			if (imposterizeSettings.Type == ImposterType.Billboard)
			{
				InstaLODStyle.CollapsiblePanel("Billboard Settings", ref state.imposterizeUIState_DetailSettingsFoldout, delegate ()
					{
						imposterizeSettings.QuadXYCount = (uint)EditorGUILayout.IntSlider(new GUIContent("QuadXY Count", "The amount of quads to spawn for the XY axis of the input geometry AABB. Additional quads will be rotated by 180°/Count along the Y axis."), (int)imposterizeSettings.QuadXYCount, 0, 32);
						imposterizeSettings.QuadXZCount = (uint)EditorGUILayout.IntSlider(new GUIContent("QuadXZ Count", "The amount of quads to spawn for the XZ axis of the input geometry AABB. Additional quads will be rotated by 180°/Count along the Z axis."), (int)imposterizeSettings.QuadXZCount, 0, 32);
						imposterizeSettings.QuadYZCount = (uint)EditorGUILayout.IntSlider(new GUIContent("QuadYZ Count", "The amount of quads to spawn for the YZ axis of the input geometry AABB. Additional quads will be rotated by 180°/Count along the Z axis."), (int)imposterizeSettings.QuadYZCount, 0, 32);
						imposterizeSettings.TwoSidedQuads = EditorGUILayout.Toggle(new GUIContent("Two Sided Quads", "Generate Two Sided Quads."), imposterizeSettings.TwoSidedQuads);
						EditorGUILayout.Space();

						if (!imposterizeSettings.AlphaCutOut)
						{
							imposterizeSettings.QuadSubdivisionsU = (uint)EditorGUILayout.IntSlider(new GUIContent("Subdivisions U", "Subdivisions along the U axis for billboard imposters."), (int)imposterizeSettings.QuadSubdivisionsU, 1, 32);
							imposterizeSettings.QuadSubdivisionsV = (uint)EditorGUILayout.IntSlider(new GUIContent("Subdivisions V", "Subdivisions along the V axis for billboard imposters"), (int)imposterizeSettings.QuadSubdivisionsV, 1, 32);
						}
					});
			}
			else if (imposterizeSettings.Type == ImposterType.HybridBillboardCloud)
			{
				InstaLODStyle.CollapsiblePanel("Hybrid Billboard Cloud Settings", ref state.imposterizeUIState_DetailSettingsFoldout, delegate ()
					{
						imposterizeSettings.CloudFaceCount = (uint)EditorGUILayout.IntSlider(new GUIContent("Maximum Face Count", "The amount of faces of the resulting mesh."), (int)imposterizeSettings.CloudFaceCount, 4, 10000);
						imposterizeSettings.CloudPolyFaceFactor = EditorGUILayout.Slider(new GUIContent("CloudPolyFaceFactor", "The amount of faces in percentage of the CloudFaceCount to allocate for the polygon part of hybrid billboard cloud imposters."), imposterizeSettings.CloudPolyFaceFactor, 0f, 0.5f);
						imposterizeSettings.EnableCloudQuadXY = EditorGUILayout.Toggle(new GUIContent("XY Quads", "Spawn XY quads for cloud imposters."), imposterizeSettings.EnableCloudQuadXY);
						imposterizeSettings.EnableCloudQuadXZ = EditorGUILayout.Toggle(new GUIContent("XZ Quads", "Spawn XZ quads for cloud imposters."), imposterizeSettings.EnableCloudQuadXZ);
						imposterizeSettings.EnableCloudQuadYZ = EditorGUILayout.Toggle(new GUIContent("YZ Quads", "Spawn YZ quads for cloud imposters."), imposterizeSettings.EnableCloudQuadYZ);
						imposterizeSettings.TwoSidedQuads = EditorGUILayout.Toggle(new GUIContent("Two Sided Quads", "Generate Two Sided Quads."), imposterizeSettings.TwoSidedQuads);

						bool wasGUIEnabled = GUI.enabled;
						GUI.enabled = false;
						EditorGUILayout.TextField(new GUIContent("Hybrid Poly Mesh Suffix", "Mesh names matching the specified suffix will be used for polygonal mesh output when creating a hybrid billboard cloud imposter."), "_cloudpoly");
						GUI.enabled = wasGUIEnabled;

						GUILayout.Space(10.0f);

						imposterizeSettings.CloudNormal = EditorGUILayout.Vector3Field(new GUIContent("Cloud Normal", "The target normal for cloud quads."), imposterizeSettings.CloudNormal);
						imposterizeSettings.CloudNormal.x = Mathf.Clamp(imposterizeSettings.CloudNormal.x, -1.0f, 1.0f);
						imposterizeSettings.CloudNormal.y = Mathf.Clamp(imposterizeSettings.CloudNormal.y, -1.0f, 1.0f);
						imposterizeSettings.CloudNormal.z = Mathf.Clamp(imposterizeSettings.CloudNormal.z, -1.0f, 1.0f);
						imposterizeSettings.CloudNormalConformity = EditorGUILayout.Slider(new GUIContent("Cloud Normal Conformity", "Determines how much the quads normals align to the target vector."), imposterizeSettings.CloudNormalConformity, 0.0f, 1.0f);
					});
			}
			else if (imposterizeSettings.Type == ImposterType.AABB)
			{
				InstaLODStyle.CollapsiblePanel("AABB Settings", ref state.imposterizeUIState_DetailSettingsFoldout, delegate ()
					{
						imposterizeSettings.AABBDisplacement = EditorGUILayout.Slider(new GUIContent("Displacement", "AABB imposter faces will be displaced along the face normal by the specified value in world space units.\n" + "This is useful to create hay or hedges with overhanging faces."), imposterizeSettings.AABBDisplacement, -10f, 10.0f);
					});
			}
			else if (imposterizeSettings.Type == ImposterType.Flipbook)
			{
				InstaLODStyle.CollapsiblePanel("Flipbook Settings", ref state.imposterizeUIState_DetailSettingsFoldout, delegate ()
					{
						imposterizeSettings.FlipbookFramesPerAxis = (uint)EditorGUILayout.IntSlider(new GUIContent("Flipbook Frames Per Axis", "The amount of flipbook frames to generates for each rotation axis."), (int)imposterizeSettings.FlipbookFramesPerAxis, 2, 32);
					});
			}

			else if (imposterizeSettings.Type == ImposterType.Vista)
			{
				InstaLODStyle.CollapsiblePanel("Vista Settings", ref state.imposterizeUIState_DetailSettingsFoldout, delegate ()
				{
					imposterizeSettings.VistaDistance = EditorGUILayout.Slider(new GUIContent("Vista Distance", "The distance of the imposter from the camera within the bounding box."), (float)imposterizeSettings.VistaDistance, 0.0f, 1.0f);
					imposterizeSettings.ParallelImposterPlane = EditorGUILayout.Toggle(new GUIContent("Parallel Imposter Plane With Camera", "Makes the imposter plane parallel with the camera."), imposterizeSettings.ParallelImposterPlane);
				});
				List<Camera> cameras = new List<Camera>();
				foreach (GameObject gameObject in Selection.gameObjects)
				{
					Camera cameraComponent = gameObject.GetComponent<Camera>();

					if (cameraComponent == null)
						continue;

					cameras.Add(cameraComponent);
				}
				if (Selection.objects.Length < 2 || cameras.Count != 1)
				{
					InstaLODWindowShared.HelpBox("Select at least two game objects, one of which with a 'Camera' component.", MessageType.Error);
				}
				if (cameras.Count == 1)
				{
					camera = cameras[0];
				}
			}
			InstaLODStyle.CollapsiblePanel("Alpha Cutout", ref state.imposterizeUIState_AlphaCutoutSettingsFoldout, delegate () 
			{
				imposterizeSettings.AlphaCutOut = EditorGUILayout.Toggle(new GUIContent("Enable", "Removes transparent areas by cutting out the opaque shape."), imposterizeSettings.AlphaCutOut);
				imposterizeSettings.AlphaCutOutSubdivide = EditorGUILayout.Toggle(new GUIContent("Subdivide", "Subdividews the cutout result based on the Resolution."), imposterizeSettings.AlphaCutOutSubdivide);
				imposterizeSettings.AlphaCutOutResolution = (uint)EditorGUILayout.IntSlider(new GUIContent("Resolution", "The resolution for the alpha cutout."), (int)imposterizeSettings.AlphaCutOutResolution, 4, 128);
			});

			InstaLODWindowShared.OnUIForBakeOutput(owner, ref imposterizeSettings.BakeOutput);

			InstaLODStyle.CollapsiblePanel("Advanced", ref state.imposterizeUIState_AdvancedSettingsFoldout, delegate ()
			{
				imposterizeSettings.Deterministic = EditorGUILayout.Toggle(new GUIContent("Deterministic Execution", "Makes the algorithm deterministic at the cost of speed."), imposterizeSettings.Deterministic);
			});

			inImposterizeSettings = imposterizeSettings;
			return camera;
        }

		public static void OnGUIForOptimizeSettings(EditorWindow owner, ref InstaLODOptimizeSettings inOptimizeSettings, bool isSkeletalMesh)
		{
			MeshOperationSettingsGUIState state = GetEditorGuiState<MeshOperationSettingsGUIState>(owner, "OptimizeSettingsGUIState");
			bool isGUIEnabled = GUI.enabled;

			InstaLODOptimizeSettings optimizeSettings = inOptimizeSettings;

			InstaLODStyle.CollapsiblePanel("Settings", ref state.optimizeUIState_Settings, delegate ()
				{
					optimizeSettings.AutomaticQuality = (MeshFeatureImportance)EditorGUILayout.EnumPopup(new GUIContent("Automatic Quality", "Determines the output quality automatically based on the fuzzy target."), optimizeSettings.AutomaticQuality);

					if (optimizeSettings.AutomaticQuality == MeshFeatureImportance.Off)
					{
						GUI.enabled = optimizeSettings.AbsoluteTriangles == 0 && optimizeSettings.ScreenSizeInPixels == 0 && isGUIEnabled;
						optimizeSettings.PercentTriangles = EditorGUILayout.Slider(new GUIContent("Percent Triangles", "The amount of polygons in percent of the output mesh in relation to the input mesh polygon count."), optimizeSettings.PercentTriangles * 100.0f, 0f, 100.0f) / 100.0f;
						GUI.enabled = optimizeSettings.PercentTriangles == 1 && optimizeSettings.ScreenSizeInPixels == 0 && isGUIEnabled;
						optimizeSettings.AbsoluteTriangles = (uint)EditorGUILayout.IntField(new GUIContent("Absolute Triangles", "The absolute amount of polygons for the target mesh. If set, this value precedes PercentTriangles."), (int)optimizeSettings.AbsoluteTriangles);
						GUI.enabled = optimizeSettings.PercentTriangles == 1 && optimizeSettings.AbsoluteTriangles == 0 && isGUIEnabled;
						optimizeSettings.ScreenSizeInPixels = (uint)EditorGUILayout.IntField(new GUIContent("Screen Size In Pixels", "If set calculates the amount of polygons to remove based on the bounds of the input model."), (int)optimizeSettings.ScreenSizeInPixels);
						GUI.enabled = isGUIEnabled;
						optimizeSettings.MaxDeviation = EditorGUILayout.FloatField(new GUIContent("Maximum Deviation", "Controls the maximum allowed mesh error. The optimizer stops before exceeding this value."), optimizeSettings.MaxDeviation);
					}
				});
			InstaLODStyle.CollapsiblePanel("Feature Importance", ref state.optimizeUIState_FeatureImportance, delegate ()
				{
					optimizeSettings.BoundaryImportance = (MeshFeatureImportance)EditorGUILayout.EnumPopup(new GUIContent("Boundary Importance", "Determines importance of weights generated by evaluating the boundaries on the geometry."), optimizeSettings.BoundaryImportance);
					optimizeSettings.TextureImportance = (MeshFeatureImportance)EditorGUILayout.EnumPopup(new GUIContent("Texture Importance", "Determines importance of weights generated by evaluating the UV space."), optimizeSettings.TextureImportance);
					optimizeSettings.ShadingImportance = (MeshFeatureImportance)EditorGUILayout.EnumPopup(new GUIContent("Shading Importance", "Determines importance of weights generated by evaluating the tangent space."), optimizeSettings.ShadingImportance);
					optimizeSettings.SilhouetteImportance = (MeshFeatureImportance)EditorGUILayout.EnumPopup(new GUIContent("Silhouette Importance", "Determines importance of weights generated by evaluating the geometry."), optimizeSettings.SilhouetteImportance);
					{
						bool wasEnabled = GUI.enabled;
						GUI.enabled = isSkeletalMesh;
						optimizeSettings.SkinningImportance = (MeshFeatureImportance)EditorGUILayout.EnumPopup(new GUIContent("Skinning Importance", "Recommended to keep at Normal and only tweak if necessary."), optimizeSettings.SkinningImportance);
						GUI.enabled = wasEnabled;
					}
				});
			InstaLODStyle.CollapsiblePanel("Normal Recalculation", ref state.optimizeUIState_NormalRecalculation, delegate ()
				{
					optimizeSettings.RecalculateNormals = EditorGUILayout.Toggle(new GUIContent("Recalculate Normals", "Recalculate normals of the output mesh."), optimizeSettings.RecalculateNormals);
					optimizeSettings.HardAngleThreshold = EditorGUILayout.Slider(new GUIContent("Hard Angle Threshold", "When recalculating normals: smooth faces if the normal angle is below this value (in degrees)."), optimizeSettings.HardAngleThreshold, 0, 180);
				});
			InstaLODStyle.CollapsiblePanel("Skeleton Optimization", ref state.optimizeUIState_SkeletonOptimization, delegate ()
				{
					optimizeSettings.SkeletonSettings.LeafBoneWeldDistance = EditorGUILayout.Slider(new GUIContent("Leaf Bone Weld Distance", "Leaf bone weld distance. Leaf bones with a distance to their parent under the specified threshold will be culled."), optimizeSettings.SkeletonSettings.LeafBoneWeldDistance, 0f, 100.0f);
					optimizeSettings.SkeletonSettings.MaximumBoneDepth = (uint)EditorGUILayout.IntSlider(new GUIContent("Maximum Bone Depth", "Bones that are below the specified level in the hierarchy will be removed."), (int)optimizeSettings.SkeletonSettings.MaximumBoneDepth, 0, 64);
					optimizeSettings.SkeletonSettings.MaximumBoneInfluencesPerVertex = (uint)EditorGUILayout.IntSlider(new GUIContent("Bone Weigths Per Vertex", "The maximum amount of bone influences per vertex. If a vertex references more bones than allowed, the lowest bone influences will be removed first."), (int)optimizeSettings.SkeletonSettings.MaximumBoneInfluencesPerVertex, 1, 32);
					optimizeSettings.SkeletonSettings.MinimumBoneInfluenceThreshold = EditorGUILayout.Slider(new GUIContent("Minimum Bone Influence", "The minimum threshold for bone influences. Influences that fall below the threshold will be removed."), optimizeSettings.SkeletonSettings.MinimumBoneInfluenceThreshold, 0, 1);
				});
			InstaLODStyle.CollapsiblePanel("Vertex Welding", ref state.optimizeUIState_VertexWelding, delegate ()
				{
					optimizeSettings.WeldingThreshold = EditorGUILayout.FloatField(new GUIContent("Welding Threshold", "Controls the welding distance. Set to 0 to disable welding."), optimizeSettings.WeldingThreshold);
					optimizeSettings.WeldingProtectDistinctUVShells = EditorGUILayout.Toggle(new GUIContent("Protect Distinct UV-Shells", "Prevents welding of UVs that are not in the same UV shell."), optimizeSettings.WeldingProtectDistinctUVShells);
				});
			InstaLODStyle.CollapsiblePanel("Advanced", ref state.optimizeUIState_Advanced, delegate ()
				{
					optimizeSettings.OptimizerVertexWeights = EditorGUILayout.Toggle(new GUIContent("Vertex Colors As Weights", "Generate additional optimizer vertex weights based on vertex color data."), optimizeSettings.OptimizerVertexWeights);
					optimizeSettings.LockSplits = EditorGUILayout.Toggle(new GUIContent("Lock Splits", "Prevents optimization of split vertices. This is useful if UV and normal splits need to be preserved as is."), optimizeSettings.LockSplits);
					optimizeSettings.LockBoundaries = EditorGUILayout.Toggle(new GUIContent("Lock Boundaries", "Prevents optimization of vertices on the mesh boundary. This is useful if the mesh contains holes that need to be preserved as is."), optimizeSettings.LockBoundaries);
					optimizeSettings.ProtectSplits = EditorGUILayout.Toggle(new GUIContent("Protect Splits", "Prevents topological violations. Disabling can cause holes, but can enable better optimization for low poly models."), optimizeSettings.ProtectSplits);
					optimizeSettings.ProtectBoundaries = EditorGUILayout.Toggle(new GUIContent("Protect Boundaries", "Prevents vertices on mesh boundaries from moving into the surface."), optimizeSettings.ProtectBoundaries);
					optimizeSettings.OptimalPlacement = EditorGUILayout.Toggle(new GUIContent("Optimal Placement", "Enable optimal placement of vertices to reduce mesh error. If disabled vertices will always move along edges."), optimizeSettings.OptimalPlacement);
					optimizeSettings.NormalizeMeshScale = EditorGUILayout.Toggle(new GUIContent("Normalize Mesh Scale", "Enables normalization of the mesh scale to fit a 10000u bounding sphere. When enabled, Maximum Deviation is specified as a percentage value."), optimizeSettings.NormalizeMeshScale);
					optimizeSettings.Deterministic = EditorGUILayout.Toggle(new GUIContent("Deterministic Execution", "Makes the algorithm deterministic at the cost of speed."), optimizeSettings.Deterministic);

				});

			inOptimizeSettings = optimizeSettings;
		}

		public static void OnGUIForMeshToolKitSettings(EditorWindow owner, ref InstaLODMeshToolKitSettings inMeshToolKitSettings)
		{
			MeshOperationSettingsGUIState state = GetEditorGuiState<MeshOperationSettingsGUIState>(owner, "MTKSettingsGUIState");
			bool isGUIEnabled = GUI.enabled;

			InstaLODMeshToolKitSettings mtkSettings = inMeshToolKitSettings;

			InstaLODStyle.CollapsiblePanel("Settings", ref state.mtkUIState_SettingsFoldout, delegate ()
			{
				mtkSettings.WeldingThreshold = EditorGUILayout.Slider(new GUIContent("Vertex Welding", "If set to a value > 0, automatically welds vertices within the specified distance."), mtkSettings.WeldingThreshold, 0f, 100.0f);

				if (mtkSettings.WeldingThreshold > 0.0f)
				{
					mtkSettings.WeldingNormalAngleThreshold = EditorGUILayout.Slider(new GUIContent("Welding Normal Angle", "Prevents welding of normals if the angle between the welded normal and the wedge normal is above the specified value in degrees."), mtkSettings.WeldingNormalAngleThreshold, 0f, 180.0f);
					mtkSettings.WeldingBoundaries = EditorGUILayout.Toggle(new GUIContent("Weld Boundaries", "Enable to weld boundary vertices only."), mtkSettings.WeldingBoundaries);
				}

				mtkSettings.HealTJunctionThreshold = EditorGUILayout.Slider(new GUIContent("T-Junction Healing", "If set to a value > 0, automatically heals T-Junctions if the vertex-edge distance is below the specified threshold."), mtkSettings.HealTJunctionThreshold, 0f, 10.0f);
				mtkSettings.RemoveDegenerateFacesThreshold = EditorGUILayout.Slider(new GUIContent("Remove Degenerate Faces", "If set to a value > 0, automatically removes degenerates if two or more corners are shared for a polygon."), mtkSettings.RemoveDegenerateFacesThreshold, 0f, 10.0f);
				mtkSettings.FixNonManifold = EditorGUILayout.Toggle(new GUIContent("Fix Non-Manifold", "Rebuilds the mesh topology to be manifold."), mtkSettings.FixNonManifold);
				mtkSettings.ConformNormals = EditorGUILayout.Toggle(new GUIContent("Conform Normals", "Unifies the direction of the wedge normals."), mtkSettings.ConformNormals);
				mtkSettings.ConformWindingOrder = EditorGUILayout.Toggle(new GUIContent("Conform Winding Order", "Unifies the polygon winding order."), mtkSettings.ConformWindingOrder);
				mtkSettings.FlipNormals = EditorGUILayout.Toggle(new GUIContent("Flip Normals", "Flips the normals of the mesh."), mtkSettings.FlipNormals);
				mtkSettings.FillHoles = EditorGUILayout.Toggle(new GUIContent("Fill Holes", "Fill all holes in the mesh."), mtkSettings.FillHoles);
				mtkSettings.NormalHealingModeSettings = (NormalHealingMode)EditorGUILayout.EnumPopup(new GUIContent("Normal Healing Mode", "Invalid normals can automatically be fixed by InstaLOD without recomputing all normals."), mtkSettings.NormalHealingModeSettings);
				mtkSettings.RecalculateNormals = EditorGUILayout.Toggle(new GUIContent("Recalculate Normals", "Recalculate normals of the mesh."), mtkSettings.RecalculateNormals);

				if (mtkSettings.RecalculateNormals)
				{
					mtkSettings.HardAngleThreshold = EditorGUILayout.Slider(new GUIContent("Hard Angle Threshold", "When recalculating normals: smooth faces if the normal angle is below this value (in degrees)."), mtkSettings.HardAngleThreshold, 0f, 180.0f);
					mtkSettings.WeightedNormals = EditorGUILayout.Toggle(new GUIContent("Weighted Normals", "When recalculating normals: smoothed normals are weighted by various geometric properties."), mtkSettings.WeightedNormals);
				}

				// mtkSettings.RemoveSubmeshIndices = EditorGUILayout.Toggle(new GUIContent("Combine Meshes", "Remove face submesh indices."), mtkSettings.RemoveSubmeshIndices);
				// mtkSettings.CalculateSubmeshIndicesByAdjacency = EditorGUILayout.Toggle(new GUIContent("Split By Adjacency", "Calculate new face submesh indices based on adjacency."), mtkSettings.CalculateSubmeshIndicesByAdjacency);
				// mtkSettings.CalculateSubmeshIndicesByMaterial = EditorGUILayout.Toggle(new GUIContent("Split By Materials", "Calculate new face submesh indices based on assigned materials."), mtkSettings.CalculateSubmeshIndicesByMaterial);                
				mtkSettings.MinimumSubMeshBoundingSphereRadius = Mathf.Max(EditorGUILayout.FloatField(new GUIContent("Minimum Radius", "If set to a value > 0, automatically remove all submeshes (determined by adjacency) where the bounding sphere radius is below the threshold."), mtkSettings.MinimumSubMeshBoundingSphereRadius), 0.0f);
				mtkSettings.MinimumFaceArea = Mathf.Max(EditorGUILayout.FloatField(new GUIContent("Minimum Face Area", "If set to a value > 0, automatically remove all faces with an area smaller than the specified value."), mtkSettings.MinimumFaceArea), 0.0f);
			});

			InstaLODStyle.CollapsiblePanel("Advanced", ref state.mtkUIState_AdvancedFoldout, delegate ()
			{
				mtkSettings.Deterministic = EditorGUILayout.Toggle(new GUIContent("Deterministic Execution", "Makes the algorithm deterministic at the cost of speed."), mtkSettings.Deterministic);
			});

			inMeshToolKitSettings = mtkSettings;
		}

		public static void OnGUIForUnwrapSettings(EditorWindow owner, ref InstaLODUnwrapSettings inUnwrapSettings)
		{
			MeshOperationSettingsGUIState state = GetEditorGuiState<MeshOperationSettingsGUIState>(owner, "UnwrapSettingsGUIState");
			bool isGUIEnabled = GUI.enabled;

			InstaLODUnwrapSettings settings = inUnwrapSettings;

			InstaLODStyle.CollapsiblePanel("Settings", ref state.unwrapUIState_SettingsFoldout, delegate ()
			{
				settings.TextureWidth = (uint)(Resolution)EditorGUILayout.EnumPopup(new GUIContent("Texture Width", "The output texture width."), InstaLODJSONProfileReadingUtilities.CastIntToEnumSafe<Resolution>( (int) settings.TextureWidth, Resolution._2048));
				settings.TextureHeight = (uint)(Resolution)EditorGUILayout.EnumPopup(new GUIContent("Texture Height", "The output texture height."), InstaLODJSONProfileReadingUtilities.CastIntToEnumSafe<Resolution>( (int) settings.TextureHeight, Resolution._2048));
				settings.GutterSizeInPixels = (uint)EditorGUILayout.IntSlider(new GUIContent("Gutter Size In Pixels", "The minimum distance between two UV shells in pixels."), (int)settings.GutterSizeInPixels, 1, 32);
			});

			InstaLODStyle.CollapsiblePanel("Unwrap", ref state.unwrapUIState_DetailSettingsFoldout, delegate ()
			{
				settings.UnwrapStrategy = (UVUnwrapStrategy)EditorGUILayout.EnumPopup(new GUIContent("Unwrap Strategy", "Strategy used during unwrap."), settings.UnwrapStrategy);
				settings.StretchImportance = (MeshFeatureImportance)EditorGUILayout.EnumPopup(new GUIContent("Stretch Importance", "The importance of UV space stretch during the unwrap. Off: high levels of stretch allowed, Highest: no stretching allowed."), settings.StretchImportance);
				settings.ShellStitching = EditorGUILayout.Toggle(new GUIContent("Shell Stitching", "When 'Unwrap Strategy' is either 'HardSurfaceAxial' or 'HardSurfaceAngle', enables stitching of shells to reduce the amount of splits and shells."), settings.ShellStitching);
				settings.InsertNormalSplits = EditorGUILayout.Toggle(new GUIContent("Insert Normal Splits", "Insert UV shell splits for edges that do not share the same vertex normal. This improves the quality when rendering normal maps on normal split heavy geometry."), settings.InsertNormalSplits);
				settings.PostProcessLayout = EditorGUILayout.Toggle(new GUIContent("Post-Process Layout", "[Preview] Performs a post-process to minimize the UV shell count."), settings.PostProcessLayout);
				settings.UVScale = Mathf.Clamp(EditorGUILayout.FloatField(new GUIContent("Scale", "A scale factor to apply to the unwrap."), settings.UVScale), 0.001f, 10000.0f);
			});

			InstaLODStyle.CollapsiblePanel("Advanced", ref state.unwrapUIState_AdvancedSettingsFoldout, delegate ()
			{
				settings.Deterministic = EditorGUILayout.Toggle(new GUIContent("Deterministic Execution", "Makes the algorithm deterministic at the cost of speed."), settings.Deterministic);
			});

			inUnwrapSettings = settings;
        }

		public static void OnGUIForIsotropicRemeshSettings(EditorWindow owner, ref InstaLODIsotropicRemeshSettings inIsotropicRemeshSettings)
		{
			MeshOperationSettingsGUIState state = GetEditorGuiState<MeshOperationSettingsGUIState>(owner, "IsotropicRemeshSettingsGUIState");
			bool isGUIEnabled = GUI.enabled;

			InstaLODIsotropicRemeshSettings settings = inIsotropicRemeshSettings;

			InstaLODStyle.CollapsiblePanel("Settings", ref state.isoremeshUIState_SettingsFoldout, delegate ()
			{
				settings.EdgeMode = (IsotropicEdgeMode)EditorGUILayout.EnumPopup(new GUIContent("Edge Mode", "The target edge mode."), settings.EdgeMode);
				settings.TargetEdgeSize = Mathf.Clamp(EditorGUILayout.FloatField(new GUIContent("Target Edge Size", "Controls the size of the edges of the triangles."), settings.TargetEdgeSize), 0.001f, 999999.0f);
				settings.Precision = (MeshFeatureImportance)EditorGUILayout.EnumPopup(new GUIContent("Precision", "How many refinement iterations to execute, more iterations lead to a higher degree of isotropy."), settings.Precision);
				settings.WeldingThreshold = EditorGUILayout.Slider(new GUIContent("Welding Threshold", " If > 0.0 vertices will be welded as a pre-pass."), settings.WeldingThreshold, 0, 100.0f);
				settings.GeometricFeatureAngleInDegrees = EditorGUILayout.Slider(new GUIContent("Geometric Feature Angle In Degrees", "If > 0.0 geometrical features to preserve will be detected based on the dihedral angle."), settings.GeometricFeatureAngleInDegrees, 0, 180.0f);
				settings.CollapseThreshold = EditorGUILayout.Slider(new GUIContent("Collapse Threshold", "Edges smaller as the specified threshold will be collapsed. The value is specified of the computed Target Edge Size: 'AbsoluteThreshold = AbsoluteTargetEdgeSize * CollapseThreshold'."), settings.CollapseThreshold * 100.0f, 0, 100.0f)/100.0f;
			});

			InstaLODStyle.CollapsiblePanel("Feature Preservation", ref state.isoremeshUIState_FeaturePreservationFoldout, delegate ()
			{
				settings.PreserveColorSplits = EditorGUILayout.Toggle(new GUIContent("Preserve Color Splits", "Determines if color splits will be preserved."), settings.PreserveColorSplits);
				settings.PreserveTexCoordSplits =EditorGUILayout.Toggle(new GUIContent("Preserve UV Splits", "Determines if UV splits will be preserved."), settings.PreserveTexCoordSplits);
				settings.PreserveNormalSplits = EditorGUILayout.Toggle(new GUIContent("Preserve Normal Splits", "Determines if normal splits will be preserved."), settings.PreserveNormalSplits);
				settings.PreserveVolume = EditorGUILayout.Toggle(new GUIContent("Preserve Volume", "Determines if volume preservation is enabled."), settings.PreserveVolume);
			});

			InstaLODStyle.CollapsiblePanel("Normal Recalculation", ref state.isoremeshUIState_NormalRecalculationFoldout, delegate ()
			{
				settings.RecalculateNormals = EditorGUILayout.Toggle(new GUIContent("Recalculate Normals", "Recalculate normals of the output mesh."), settings.RecalculateNormals);
				settings.HardAngleThreshold = EditorGUILayout.Slider(new GUIContent("Hard Angle Threshold", "When recalculating normals: smooth polygons if the normal angle is below this value (in degrees)."), settings.HardAngleThreshold, 0.0f, 180.0f);
				settings.WeightedNormals = EditorGUILayout.Toggle(new GUIContent("Weighted Normals", "When recalculating normals: smoothed normals are weighted by various geometric properties."), settings.WeightedNormals);
			});

			InstaLODStyle.CollapsiblePanel("Advanced", ref state.isoremeshUIState_AdvancedFoldout, delegate ()
			{
				settings.Deterministic = EditorGUILayout.Toggle(new GUIContent("Deterministic Execution", "Makes the algorithm deterministic at the cost of speed."), settings.Deterministic);
			});

			inIsotropicRemeshSettings = settings;
		}

		public static void OnGUIForAsyncMeshOperationInProgress(EditorWindow owningWindow)
		{
			Rect progressRect = new Rect(5, 60, owningWindow.position.width - 10, 20);
			float progress = InstaLODNative.currentMeshOperationState.progress * 0.95f;
			Color progressBackgroundColor = InstaLODStyle.darkStyleEnabled ? Color.black : InstaLODStyle.darkGreyColor;

			// draw progressbar back fill
			{
				GUI.color = progressBackgroundColor;
				GUI.DrawTexture(progressRect, Texture2D.whiteTexture);
			}

			// draw progressbar progress fill
			{
				GUI.color = InstaLODStyle.instaLODAccentColor;
				progressRect.width *= progress;
				GUI.DrawTexture(progressRect, Texture2D.whiteTexture);
			}

			// draw progress bar info label
			{
				GUI.color = Color.white;
				GUI.Label(progressRect, string.Format("{0:0.00}%", progress * 100.0f), new GUIStyle()
				{
					normal = new GUIStyleState() { textColor = new Color(progressBackgroundColor.r, progressBackgroundColor.g, progressBackgroundColor.b, Mathf.Min(Mathf.Max(progress - 0.05f, 0.0f) * 10.0f, 1.0f)) },
					fontStyle = FontStyle.Bold,
					alignment = TextAnchor.MiddleCenter
				});
			}

			// draw status label
			{
				GUILayout.Space(70.0f);
				Color progressLabelTextColor = InstaLODStyle.darkStyleEnabled ? Color.white : Color.black;

				if (InstaLODNative.currentMeshOperationState.progress < 1.0f)
				{
					GUILayout.Label(string.Format("{0} currently in progress...", InstaLODNative.currentMeshOperationState.type), new GUIStyle()
					{
						alignment = TextAnchor.MiddleCenter,
						normal = new GUIStyleState()
						{
							textColor = progressLabelTextColor
						}
					});
				}
				else
				{
					GUILayout.Label(string.Format("Importing {0} results...", InstaLODNative.currentMeshOperationState.type), new GUIStyle()
					{
						alignment = TextAnchor.MiddleCenter,
						normal = new GUIStyleState()
						{
							textColor = progressLabelTextColor
						}
					});
				}
			}
		}
	}

	/// <summary>
	/// The InstaLODMeshUtilites class provides functionalities to create, and operate on Unity meshes.
	/// </summary>
	public static class InstaLODMeshUtilites
	{
		/// <summary>
		/// Creates a new plane.
		/// </summary>
		/// <returns>The new plane mesh.</returns>
		/// <param name="Extents">The plane extents.</param>
		public static Mesh CreatePlane(Vector2 Extents)
		{
			Mesh plane = new Mesh();

			if (Mathf.Approximately(Extents[0], 0.0f) || Mathf.Approximately(Extents[1], 0.0f))
				return plane;

			plane.vertices = new Vector3[] {new Vector3(-Extents[0], Extents[1], 0.0f),
											new Vector3(Extents[0], Extents[1], 0.0f),
											new Vector3(-Extents[0], -Extents[1], 0.0f),
											new Vector3(Extents[0], -Extents[1], 0.0f)
											};
			float xDimension = 1.0f;
			float yDimension = 1.0f;
			float xyRatio = Extents[0] / Extents[1];

			if (xyRatio > 1.0f)
			{
				yDimension = 1.0f/xyRatio;
			}
			else if(xyRatio < 1.0f)
			{
				xDimension = xyRatio;
			}

			plane.SetUVs(0, new Vector2[]{new Vector2(0.0f, yDimension), new Vector2(xDimension, yDimension),
							new Vector2(0.0f, 0.0f), new Vector2(xDimension, 0.0f)
							});

			plane.normals = new Vector3[] { Vector3.back, Vector3.back, Vector3.back, Vector3.back};
			plane.triangles = new int[] { 0, 1, 2, 2, 1, 3 };
			plane.subMeshCount = 2;
			return plane;
		}

		/// <summary>
		/// Applies transformation on a mesh.
		/// </summary>
		/// <param name="mesh">Reference to the mesh to be transformed.</param>
		/// <param name="transformationMatrix">The transformation matrix.</param>
		/// <param name="worldSpacePosition">The position of the mesh in world space.</param>
		public static void ApplyPostTransform(ref Mesh mesh, Matrix4x4 transformationMatrix, Vector3 worldSpacePosition)
		{
			Matrix4x4 originTransformation = new Matrix4x4();
			originTransformation.SetTRS(worldSpacePosition, Quaternion.identity, Vector3.one);
			Vector3[] vertices = mesh.vertices;
			Matrix4x4 completeTransformationMatrix = originTransformation * transformationMatrix * originTransformation.inverse;

			for (uint i = 0u; i < mesh.vertices.Length; i++)
			{
				vertices[i] = completeTransformationMatrix.MultiplyPoint(mesh.vertices[i]);
			}
			mesh.vertices = vertices;
		}

	}
}

