/**
 * InstaLODStyle.cs (InstaLODUnity)
 *
 * Copyright © 2019 InstaLOD GmbH - All Rights Reserved.
 *
 * Unauthorized copying of this file, via any medium is strictly prohibited.
 * This file and all it's contents are proprietary and confidential.
 *
 * @file InstaLODStyle.cs
 * @copyright 2017-2019 InstaLOD GmbH. All rights reserved.
 * @section License
 */

using UnityEngine;
using UnityEditor;
using System;

namespace InstaLOD
{
	public static class InstaLODStyleUtilities
	{
		public static GUIStyle GetEditorStyle(string name)
		{
			GUISkin skinScene = EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector);

			foreach (GUIStyle style in skinScene.customStyles)
			{
				if (style.name == name)
					return style;
			}
			return null;
		}
	}

	/// <summary>
	/// The InstaLODStyle class manages style for the integration.
	/// </summary>
	public static class InstaLODStyle
	{
		public static GUIStyle collapsiblePanelHeaderStyle;
		public static GUIStyle collapsiblePanelBodyStyle;
		public static GUIStyle darkStyleDefault;
		public static GUIStyle collapsiblePanelFoldOutHeaderStyle;
		public static GUIStyle sliderStyle;
		public static GUIStyle primaryButtonStyle, secondaryButtonStyle, mainComboBoxButtonStyle, informationTextBoxStyle;
		public static GUIStyle toolbarButtonStyle;

		public static GUIStyle listItemStyle;
		public static GUIStyle listItemStyleIndentend;
		public static GUIStyle listItemStyleWarning;
		public static GUIStyle listItemStyleIndentendWarning;
		public static GUIStyle listItemGroupStyle;

		public static GUIStyle helpBoxBackgroundStyle;

		public static Color darkGreyColor = new Color(0.17f, 0.17f, 0.17f);
		public static Color lightGreyColor = new Color(0.51f, 0.51f, 0.51f);
		public static Color veryLightGreyColor = new Color(0.8f, 0.8f, 0.8f);
		public static Color instaLODAccentColor = new Color(1.0f, 0.18f, 0.55f);
		public static Color controlGreyColor = new Color(0.36f, 0.36f, 0.36f);
		public static Color windowBackgroundColorBrightSkin = new Color(0.941f, 0.941f, 0.941f);
		public static Color windowBackgroundColorDarkSkin = new Color(0.196f, 0.196f, 0.196f);
		public static Color tabContentColor = new Color(0.27f, 0.27f, 0.27f);
		public static Color collapsiblePanelHeaderBackgroundColorBrightSkin = new Color(0.886f, 0.886f, 0.886f);
		public static Color collapsiblePanelHeaderBackgroundColorDarkSkin = new Color(0.157f, 0.157f, 0.157f);
		public static Color mainComboBoxButtonBackgroundColorDarkSkin = new Color(0.176f, 0.176f, 0.176f);
		public static Color mainComboBoxButtonBackgroundColorBrightSkin = new Color(0.824f, 0.824f, 0.824f);

		public static bool darkStyleEnabled = EditorGUIUtility.isProSkin;

		public static Texture2D logoForCurrentStyleType
		{
			get
			{
				return darkStyleEnabled ? logoWhite : logoBlack;
			}
		}

		/// <summary>
		/// Returns InstaLOD window color.
		/// </summary>
		/// <returns>The current window color.</returns>
		public static Color GetWindowBackgroundColor()
		{
			return EditorGUIUtility.isProSkin ? windowBackgroundColorDarkSkin : windowBackgroundColorBrightSkin;
		}

		/// <summary>
		/// Gets the background color for the main combobox button.
		/// </summary>
		/// <returns>Combobox button color.</returns>
		public static Color GetMainComboboxButtonColor()
		{
			return EditorGUIUtility.isProSkin ? mainComboBoxButtonBackgroundColorDarkSkin : mainComboBoxButtonBackgroundColorBrightSkin;
		}

		/// <summary>
		/// Returns InstaLOD information text color.
		/// </summary>
		/// <returns>The text color.</returns>
		public static Color GetTextColor()
		{
			return EditorGUIUtility.isProSkin ? Color.white : Color.black;
		}

		/// <summary>
		/// Returns the collapsible panel header icon size.
		/// </summary>
		/// <returns>The icon size.</returns>
		public static Vector2 GetCollapsiblePanelHeaderIconSize()
		{
			return new Vector2(10, 10);
		}

		/// <summary>
		/// Gets the collapsible panel header background color.
		/// </summary>
		/// <returns>The background color.</returns>
		public static Color GetCollapsiblePanelHeaderBackgroundColor()
		{
			return EditorGUIUtility.isProSkin ? collapsiblePanelHeaderBackgroundColorDarkSkin : collapsiblePanelHeaderBackgroundColorBrightSkin;
		}

		/// <summary>
		/// Gets the resource path for the specified resource.
		/// </summary>
		/// <param name="resource">The resource name.</param>
		/// <returns>The Resource path.</returns>
		public static string GetResourcePath(string resource)
		{
			return String.Format("Packages/com.shanghaiwindy.simplygon/InstaLOD/Editor/Resources/{0}", resource);
		}

		/// <summary>
		/// Loads the texture for the specified resource.
		/// </summary>
		/// <param name="resource">The resource name.</param>
		/// <returns>The texture.</returns>
		public static Texture2D LoadTextureFromResource(string resource)
		{
			Texture2D texture = new Texture2D(1, 1);
			texture.LoadImage(System.IO.File.ReadAllBytes(GetResourcePath(resource)));
			texture.Apply();
			return texture;
		}

		public static uint CalculateTextWidth(string text, GUIStyle style)
		{
			Vector2 size = style.CalcSize(new GUIContent(text));
			uint width = (uint)size.x;
			return width;
		}

		private static Texture2D _mainComboBoxDropDownIcon;
		public static Texture2D mainComboBoxDropDownIcon
		{
			get
			{
				if (_mainComboBoxDropDownIcon == null)
				{
					_mainComboBoxDropDownIcon = InstaLODStyle.LoadTextureFromResource("MainComboBoxButton.png");
				}
				return _mainComboBoxDropDownIcon;
			}
		}

		private static Texture2D _branchClosedIcon;
		public static Texture2D branchClosedIcon
		{
			get
			{
				if (_branchClosedIcon == null)
				{
					_branchClosedIcon = InstaLODStyle.LoadTextureFromResource("CaretRight.png");
				}
				return _branchClosedIcon;
			}
		}

		private static Texture2D _branchOpenIcon;
		public static Texture2D branchOpenIcon
		{
			get
			{
				if (_branchOpenIcon == null)
				{
					_branchOpenIcon = InstaLODStyle.LoadTextureFromResource("CaretDown.png");
				}
				return _branchOpenIcon;
			}
		}

		private static Texture2D _logoBlack;
		public static Texture2D logoBlack 
		{
			get 
			{
				if (_logoBlack == null)
				{
					_logoBlack = InstaLODStyle.LoadTextureFromResource("InstaLOD_Logo-Black_151x32.png"); 
				}
				return _logoBlack;
			}
		}

		private static Texture2D _logoWhite;
		public static Texture2D logoWhite 
		{
			get 
			{
				if (_logoWhite == null)
				{
					_logoWhite = InstaLODStyle.LoadTextureFromResource("InstaLOD_Logo-White_151x32.png");
				}
				return _logoWhite;
			}
		}

		private static Texture2D _headerIcon;
		public static Texture2D headerIcon 
		{
			get 
			{
				if (_headerIcon == null)
				{
					_headerIcon = InstaLODStyle.LoadTextureFromResource("InstaLOD-Button-Black_32x32.png");
				}
				return _headerIcon;
			}
		}

		private static Texture2D _tabbarInactiveTexture = null;
		public static Texture2D tabbarInactiveTexture 
		{
			get 
			{
				if (_tabbarInactiveTexture == null)
				{
					_tabbarInactiveTexture = InstaLODStyle.LoadTextureFromResource("TabBar_Inactive" + (darkStyleEnabled ? "" : "-Light") + ".png"); 

				}
				return _tabbarInactiveTexture;
			}
		}

		private static Texture2D _tabbarActiveTexture = null;
		public static Texture2D tabbarActiveTexture
		{
			get 
			{
				if (_tabbarActiveTexture == null)
				{
					_tabbarActiveTexture = InstaLODStyle.LoadTextureFromResource("TabBar_Active" + (darkStyleEnabled ? "" : "-Light") + ".png"); 
				}
				return _tabbarActiveTexture;
			}
		}

		private static Texture2D _borderTexture = null;
		public static Texture2D borderTexture
		{
			get
			{
				if (_borderTexture == null)
				{
					_borderTexture = InstaLODStyle.LoadTextureFromResource("Border.png");
				}
				return _borderTexture;
			} 
		}

		static InstaLODStyle()
		{
		}

		public static void InitializeStyle()
		{
			if (!darkStyleEnabled)
			{
				tabContentColor = new Color(0.69f, 0.69f, 0.69f);
				windowBackgroundColorDarkSkin = new Color(0.76f, 0.76f, 0.76f);
			}

			Texture2D lightGreyBackground = new Texture2D(1, 1);
			lightGreyBackground.SetPixel(0, 0, veryLightGreyColor);
			
			Texture2D darkGreyBackground = new Texture2D(1, 1);
			darkGreyBackground.SetPixel(0, 0, darkGreyColor);

			const int helpBoxBackgroundMargin = 5;
			helpBoxBackgroundStyle = new GUIStyle();
			helpBoxBackgroundStyle.padding = new RectOffset(0, 0, 0, 0);
			helpBoxBackgroundStyle.margin = new RectOffset(helpBoxBackgroundMargin, helpBoxBackgroundMargin, 0, 0);
			helpBoxBackgroundStyle.overflow = new RectOffset(-helpBoxBackgroundMargin, -helpBoxBackgroundMargin, -helpBoxBackgroundMargin, -helpBoxBackgroundMargin);
			helpBoxBackgroundStyle.normal.background = new Texture2D(1, 1);
			helpBoxBackgroundStyle.normal.background.SetPixel(0, 0, GetWindowBackgroundColor());
			helpBoxBackgroundStyle.normal.background.Apply();
			helpBoxBackgroundStyle.normal.scaledBackgrounds = new Texture2D[] { helpBoxBackgroundStyle.normal.background };

			Color textColor = InstaLODStyle.GetTextColor();
			GUIStyle menuItem = new GUIStyle();
			listItemStyle = new GUIStyle(menuItem);
			listItemStyle.padding = new RectOffset(5, 0, 1, 2);
			listItemStyle.normal.textColor = textColor;
			listItemStyle.onActive.textColor = Color.white;
			listItemStyle.onActive.background = new Texture2D(1, 1);
			listItemStyle.onActive.background.SetPixel(0, 0, new Color(0.243f, 0.49f, 0.906f));
			listItemStyle.onActive.background.Apply();
			listItemStyle.active.background = listItemStyle.onActive.background;
			listItemStyle.active.textColor = listItemStyle.onActive.textColor;
			listItemStyle.onNormal.background = listItemStyle.onActive.background;
			listItemStyle.onNormal.textColor = listItemStyle.onActive.textColor;
			listItemStyle.onFocused.background = listItemStyle.onActive.background;
			listItemStyle.onFocused.textColor = listItemStyle.onActive.textColor;

			listItemGroupStyle = new GUIStyle() { padding = new RectOffset(5, 5, 0, 5) };

			listItemStyleIndentend = new GUIStyle(menuItem);
			listItemStyleIndentend.padding = new RectOffset(25, 0, 1, 2);
			listItemStyleIndentend.onNormal = listItemStyle.onNormal;
			listItemStyleIndentend.normal = listItemStyle.normal;
			listItemStyleIndentend.onFocused = listItemStyle.onFocused;

			Color warningTextColor = new Color(0.8f, 0.4f, 0);
			listItemStyleWarning = new GUIStyle(listItemStyle);
			listItemStyleWarning.normal.textColor = listItemStyleWarning.onNormal.textColor = warningTextColor;

			listItemStyleIndentendWarning = new GUIStyle(listItemStyleIndentend);
			listItemStyleIndentendWarning.fontStyle = FontStyle.Bold;
			listItemStyleIndentendWarning.normal.textColor = listItemStyleIndentendWarning.onNormal.textColor = warningTextColor;

			GUIStyle template = InstaLODStyleUtilities.GetEditorStyle("grey_border");

			collapsiblePanelHeaderStyle = new GUIStyle(GUIStyle.none);
			collapsiblePanelHeaderStyle.padding = new RectOffset(1, 1, 1, 1);
			collapsiblePanelHeaderStyle.border = template.border;
			collapsiblePanelHeaderStyle.focused = new GUIStyleState();
			collapsiblePanelHeaderStyle.active = new GUIStyleState();
			collapsiblePanelHeaderStyle.overflow = new RectOffset(0, 0, 0, 0);

			collapsiblePanelBodyStyle = new GUIStyle();
			collapsiblePanelBodyStyle.margin = new RectOffset(1, 1, -1, 1);
			collapsiblePanelBodyStyle.padding = new RectOffset(5, 5, 5, 5);
			collapsiblePanelBodyStyle.border = template.border;
			collapsiblePanelBodyStyle.normal = template.normal;
			collapsiblePanelBodyStyle.normal.textColor = textColor;
			collapsiblePanelBodyStyle.active.textColor = textColor;
			collapsiblePanelBodyStyle.hover.textColor = textColor;
			collapsiblePanelBodyStyle.focused.textColor = textColor;

			collapsiblePanelBodyStyle.normal.background = template.normal.background;
			collapsiblePanelBodyStyle.active.background = template.active.background;
			collapsiblePanelBodyStyle.hover.background = template.hover.background;
			collapsiblePanelBodyStyle.focused.background = template.focused.background;
			collapsiblePanelBodyStyle.onActive.background = template.onActive.background;
			collapsiblePanelBodyStyle.onFocused.background = template.onFocused.background;
			collapsiblePanelBodyStyle.onNormal.background = template.onNormal.background;
			collapsiblePanelBodyStyle.onHover.background = template.onHover.background;

			collapsiblePanelBodyStyle.normal.scaledBackgrounds = new Texture2D[] { InstaLODStyle.borderTexture };
			collapsiblePanelBodyStyle.active.scaledBackgrounds = new Texture2D[] { InstaLODStyle.borderTexture };
			collapsiblePanelBodyStyle.hover.scaledBackgrounds = new Texture2D[] { InstaLODStyle.borderTexture };
			collapsiblePanelBodyStyle.focused.scaledBackgrounds = new Texture2D[] { InstaLODStyle.borderTexture };
			collapsiblePanelBodyStyle.onNormal.scaledBackgrounds = new Texture2D[] { InstaLODStyle.borderTexture };
			collapsiblePanelBodyStyle.onHover.scaledBackgrounds = new Texture2D[] { InstaLODStyle.borderTexture };
			collapsiblePanelBodyStyle.onFocused.scaledBackgrounds = new Texture2D[] { InstaLODStyle.borderTexture };
			collapsiblePanelBodyStyle.onActive.scaledBackgrounds = new Texture2D[] { InstaLODStyle.borderTexture };

			collapsiblePanelBodyStyle.overflow = new RectOffset(0, 0, 0, 0);

			sliderStyle = new GUIStyle(EditorStyles.foldout);
			sliderStyle.fontStyle = FontStyle.Bold;
			sliderStyle.normal.textColor = textColor;
			sliderStyle.active.textColor = textColor;
			sliderStyle.hover.textColor = textColor;
			sliderStyle.focused.textColor = textColor;
			sliderStyle.onNormal.textColor = textColor;
			sliderStyle.onHover.textColor = textColor;
			sliderStyle.onFocused.textColor = textColor;
			sliderStyle.onActive.textColor = textColor;
			sliderStyle.normal.background = new Texture2D(1, 1);
			sliderStyle.overflow = new RectOffset(0, 0, 0, 0);

			collapsiblePanelFoldOutHeaderStyle = new GUIStyle(EditorStyles.foldoutHeader);

			Texture2D foldoutBackground = InstaLODStyle.LoadTextureFromResource("CollapsiblePanel.png");
			foldoutBackground.alphaIsTransparency = true;
			foldoutBackground.Apply();

			collapsiblePanelFoldOutHeaderStyle.normal.scaledBackgrounds = new Texture2D[] { foldoutBackground };
			collapsiblePanelFoldOutHeaderStyle.active.scaledBackgrounds = new Texture2D[] { foldoutBackground };
			collapsiblePanelFoldOutHeaderStyle.hover.scaledBackgrounds = new Texture2D[] { foldoutBackground };
			collapsiblePanelFoldOutHeaderStyle.fontStyle = FontStyle.Bold;
			collapsiblePanelFoldOutHeaderStyle.normal.textColor = textColor;
			collapsiblePanelFoldOutHeaderStyle.active.textColor = textColor;
			collapsiblePanelFoldOutHeaderStyle.hover.textColor = textColor;
			collapsiblePanelFoldOutHeaderStyle.focused.textColor = textColor;
			collapsiblePanelFoldOutHeaderStyle.onNormal.textColor = textColor;
			collapsiblePanelFoldOutHeaderStyle.onHover.textColor = textColor;
			collapsiblePanelFoldOutHeaderStyle.onFocused.textColor = textColor;
			collapsiblePanelFoldOutHeaderStyle.onActive.textColor = textColor;
			collapsiblePanelFoldOutHeaderStyle.normal.background = foldoutBackground;
			collapsiblePanelFoldOutHeaderStyle.active.background = foldoutBackground;
			collapsiblePanelFoldOutHeaderStyle.hover.background = foldoutBackground;
			collapsiblePanelFoldOutHeaderStyle.margin = new RectOffset(1, 1, -1, 0);
			collapsiblePanelFoldOutHeaderStyle.padding = new RectOffset(15, 2, 2, 2);
			collapsiblePanelFoldOutHeaderStyle.overflow = new RectOffset(0, 0, 0, 0);
			collapsiblePanelFoldOutHeaderStyle.fixedHeight = 30;
			collapsiblePanelFoldOutHeaderStyle.fontSize = 12;

			primaryButtonStyle = new GUIStyle();
			primaryButtonStyle.wordWrap = true;
			primaryButtonStyle.alignment = TextAnchor.MiddleCenter;
			primaryButtonStyle.normal.textColor = textColor;
			primaryButtonStyle.active.textColor = textColor;
			primaryButtonStyle.hover.textColor = textColor;
			primaryButtonStyle.focused.textColor = textColor;
			primaryButtonStyle.onNormal.textColor = textColor;
			primaryButtonStyle.onHover.textColor = textColor;
			primaryButtonStyle.onFocused.textColor = textColor;
			primaryButtonStyle.onActive.textColor = textColor;
			primaryButtonStyle.margin = new RectOffset(5, 5, 5, 5);
			primaryButtonStyle.padding = new RectOffset(5, 5, 5, 5);

			textColor = EditorGUIUtility.isProSkin? Color.white : Color.black;
			secondaryButtonStyle = new GUIStyle("Button");
			secondaryButtonStyle.wordWrap = true;
			secondaryButtonStyle.normal.textColor = textColor;
			secondaryButtonStyle.active.textColor = textColor;
			secondaryButtonStyle.hover.textColor = textColor;
			secondaryButtonStyle.focused.textColor = textColor;
			secondaryButtonStyle.onNormal.textColor = textColor;
			secondaryButtonStyle.onHover.textColor = textColor;
			secondaryButtonStyle.onFocused.textColor = textColor;
			secondaryButtonStyle.onActive.textColor = textColor;

			Texture2D button = LoadTextureFromResource("MainComboBoxButtonBackground.png");
			button.alphaIsTransparency = true;
			button.Apply();

			mainComboBoxButtonStyle = new GUIStyle(GUI.skin.button);
			mainComboBoxButtonStyle.wordWrap = true;
			mainComboBoxButtonStyle.normal.scaledBackgrounds = new Texture2D[] { button };
			mainComboBoxButtonStyle.focused.scaledBackgrounds = new Texture2D[] { button };
			mainComboBoxButtonStyle.hover.scaledBackgrounds = new Texture2D[] { button };
			mainComboBoxButtonStyle.active.scaledBackgrounds = new Texture2D[] { button };
			mainComboBoxButtonStyle.onNormal.scaledBackgrounds = new Texture2D[] { button };
			mainComboBoxButtonStyle.onFocused.scaledBackgrounds = new Texture2D[] { button };
			mainComboBoxButtonStyle.onHover.scaledBackgrounds = new Texture2D[] { button };
			mainComboBoxButtonStyle.onActive.scaledBackgrounds = new Texture2D[] { button };
			mainComboBoxButtonStyle.normal.textColor = textColor;
			mainComboBoxButtonStyle.active.textColor = textColor;
			mainComboBoxButtonStyle.hover.textColor = textColor;
			mainComboBoxButtonStyle.focused.textColor = textColor;
			mainComboBoxButtonStyle.onNormal.textColor = textColor;
			mainComboBoxButtonStyle.onHover.textColor = textColor;
			mainComboBoxButtonStyle.onFocused.textColor = textColor;
			mainComboBoxButtonStyle.onActive.textColor = textColor;
			mainComboBoxButtonStyle.fontStyle = FontStyle.Bold;
			mainComboBoxButtonStyle.fontSize = 20;
			mainComboBoxButtonStyle.margin = new RectOffset(2, 2, 5, 5);
			mainComboBoxButtonStyle.imagePosition = ImagePosition.ImageLeft;
			mainComboBoxButtonStyle.normal.background = button;
			mainComboBoxButtonStyle.hover.background = button;
			mainComboBoxButtonStyle.active.background = button;
			mainComboBoxButtonStyle.focused.background = button;
			mainComboBoxButtonStyle.onNormal.background = button;
			mainComboBoxButtonStyle.onHover.background = button;
			mainComboBoxButtonStyle.onActive.background = button;
			mainComboBoxButtonStyle.onFocused.background = button;
			mainComboBoxButtonStyle.alignment = TextAnchor.MiddleLeft;

			informationTextBoxStyle = new GUIStyle(GUI.skin.textArea);
			informationTextBoxStyle.normal.background = new Texture2D(1, 1);
			informationTextBoxStyle.normal.background.SetPixel(0, 0, InstaLODStyle.GetWindowBackgroundColor());
			informationTextBoxStyle.normal.background.Apply();
			informationTextBoxStyle.normal.scaledBackgrounds = new Texture2D[] { informationTextBoxStyle.normal.background };
			informationTextBoxStyle.normal.textColor = InstaLODStyle.GetTextColor();

			darkStyleDefault = new GUIStyle(InstaLODStyleUtilities.GetEditorStyle("OL ToggleWhite"));
			darkStyleDefault.normal.textColor = textColor;

			toolbarButtonStyle = new GUIStyle();
			toolbarButtonStyle.fontSize = 10;
			toolbarButtonStyle.fixedHeight = 22;
			toolbarButtonStyle.border = new RectOffset(5, 5, 5, 0);
			toolbarButtonStyle.normal.textColor = textColor;
			toolbarButtonStyle.hover.textColor = textColor;
			toolbarButtonStyle.focused.textColor = textColor;
			toolbarButtonStyle.active.textColor = textColor;
			toolbarButtonStyle.onNormal.textColor = textColor;
			toolbarButtonStyle.onHover.textColor = textColor;
			toolbarButtonStyle.onFocused.textColor = textColor;
			toolbarButtonStyle.onActive.textColor = textColor;
			toolbarButtonStyle.alignment = TextAnchor.MiddleCenter;
			toolbarButtonStyle.overflow = new RectOffset(0, 0, 0, 0);
		}

		public static void EnsureStyleTextures()
		{
			InitializeStyle();

			collapsiblePanelHeaderStyle.normal = new GUIStyleState() { background = Texture2D.whiteTexture };
			primaryButtonStyle.normal = new GUIStyleState() { background = Texture2D.whiteTexture };

			toolbarButtonStyle.normal.background = tabbarInactiveTexture;
			toolbarButtonStyle.active.background = tabbarActiveTexture;
			toolbarButtonStyle.onNormal.background = tabbarActiveTexture;
		}

		public static bool CollapsiblePanelHeader(bool state, string label)
		{
			Color oldColor = GUI.backgroundColor;

			GUI.backgroundColor = InstaLODStyle.GetCollapsiblePanelHeaderBackgroundColor(); 
			Texture2D icon = state ? branchOpenIcon : branchClosedIcon;

			EditorGUIUtility.SetIconSize(InstaLODStyle.GetCollapsiblePanelHeaderIconSize());
			state = EditorGUILayout.BeginFoldoutHeaderGroup(state, new GUIContent("    " + label, icon), collapsiblePanelFoldOutHeaderStyle);
			EditorGUIUtility.SetIconSize(new Vector2(0,0));
			EditorGUILayout.EndFoldoutHeaderGroup();

			GUI.backgroundColor = oldColor;

			if (!state)
			{
				GUILayout.Space(5.0f);
			}
			
			return state;
		}

		public static void BeginCollapsiblePanelBody()
		{
			GUI.backgroundColor = InstaLODStyle.GetWindowBackgroundColor();
			EditorGUILayout.BeginVertical(collapsiblePanelBodyStyle);
			GUI.backgroundColor = Color.white;
		}

		public static void EndCollapsiblePanelBody()
		{
			EditorGUILayout.EndVertical();
			GUILayout.Space(5.0f);
		}

		public static bool PrimaryButton(string title)
		{
			GUI.backgroundColor = instaLODAccentColor;
			bool value = GUILayout.Button(title, primaryButtonStyle);

			GUI.backgroundColor = Color.white;

			return value;
		}

		public static bool PrimaryButton(GUIContent content)
		{
			GUI.backgroundColor = instaLODAccentColor;
			bool value = GUILayout.Button(content, primaryButtonStyle);

			GUI.backgroundColor = Color.white;

			return value;
		}

		public static Texture2D alertIcon
		{
			get
			{
				return (Texture2D)EditorGUIUtility.LoadRequired("icons/d_console.warnicon.sml.png");
			}
		}

		public static bool SecondaryButton(string title)
		{
			GUI.backgroundColor = Color.white;
			bool value = GUILayout.Button(title, secondaryButtonStyle);
			GUI.backgroundColor = Color.white;

			return value;
		}

		public static bool MainComboBoxButton(string title)
		{
			Color defaultColor =  GUI.backgroundColor;

			GUI.backgroundColor = InstaLODStyle.GetMainComboboxButtonColor();
			uint buttonWidth = InstaLODStyle.CalculateTextWidth(title, mainComboBoxButtonStyle);
			bool result = GUILayout.Button(new GUIContent(title, mainComboBoxDropDownIcon), mainComboBoxButtonStyle, GUILayout.MaxWidth(buttonWidth + mainComboBoxDropDownIcon.width), GUILayout.MinHeight(40));

			GUI.backgroundColor = defaultColor;

			return result;
		}

		public static void CollapsiblePanel(string title, ref bool value, Action childControlAction)
		{
			value = InstaLODStyle.CollapsiblePanelHeader(value, title);
			if (value)
			{
				InstaLODStyle.BeginCollapsiblePanelBody();
				childControlAction();
				InstaLODStyle.EndCollapsiblePanelBody();
			}
		}

		public class VerticalLayout : IDisposable
		{
			public VerticalLayout(int padding)
			{
				EditorGUILayout.BeginVertical(new GUIStyle() { padding = new RectOffset(padding, padding, 0, 0) });
			}

			public void Dispose()
			{
				EditorGUILayout.EndVertical();
			}
		}

		public class FooterContent : IDisposable
		{
			Action ContentDelegate;

			public FooterContent(Action func)
			{
				ContentDelegate = func;
			}

			public void Dispose()
			{
				ContentDelegate();
			}
		}

		public class ComboBoxTabLayout: IDisposable
		{
			public ComboBoxTabLayout()
			{
				GUI.backgroundColor = InstaLODStyle.GetWindowBackgroundColor();
				GUILayout.BeginVertical(new GUIStyle() { margin = new RectOffset(5, 5, 0, 0), padding = new RectOffset(0, 0, 5, 0), normal = new GUIStyleState() { background = Texture2D.whiteTexture } });

				GUI.backgroundColor = Color.white;
			}

			public void Dispose()
			{
				GUILayout.EndVertical();
			}
		}

		public class TabLayout : IDisposable
		{
			public TabLayout(Action tabHeader)
			{
				GUI.backgroundColor = Color.white;
				GUILayout.BeginHorizontal(new GUIStyle() { margin = new RectOffset(5, 5, 5, 0), overflow = new RectOffset(0, 0, 0, 0)});
				tabHeader();
				GUILayout.EndHorizontal();
			
				GUI.backgroundColor = InstaLODStyle.GetWindowBackgroundColor();
				GUILayout.BeginVertical(new GUIStyle() { margin = new RectOffset(5, 5, 0, 0), padding = new RectOffset(0, 0, 5, 0), overflow = new RectOffset(0, 0, 0, 0), normal = new GUIStyleState() { background = Texture2D.whiteTexture } });

				GUI.backgroundColor = Color.white;
			}

			public void Dispose()
			{
				GUILayout.EndVertical();
			}
		}
	}
}