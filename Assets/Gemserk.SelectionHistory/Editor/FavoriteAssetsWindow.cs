using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Gemserk
{
    public class FavoriteAssetsWindow : EditorWindow
    {
        [MenuItem("Window/Gemserk/Favorites")]
        public static void ShowExample()
        {
            var wnd = GetWindow<FavoriteAssetsWindow>();
            wnd.titleContent = new GUIContent("Favorites");
        }

        private Favorites _favorites;

        public StyleSheet styleSheet;

        public VisualTreeAsset favoriteElementTreeAsset;

        private void OnDisable()
        {
            if (_favorites != null)
            {
                _favorites.OnFavoritesUpdated -= OnFavoritesUpdated;
            }
        }

        public void OnEnable()
        {
            _favorites = FavoritesController.Favorites;
            _favorites.OnFavoritesUpdated += OnFavoritesUpdated;
            
            // var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/FavoriteAssetsWindow.uss");

            // Each editor window contains a root VisualElement object
            var root = rootVisualElement;
            root.styleSheets.Add(styleSheet);

            // // VisualElements objects can contain other VisualElement following a tree hierarchy.
            // VisualElement label = new Label("Hello World! From C#");
            // root.Add(label);
            //
            // // Import UXML

            ReloadRoot();
            // A stylesheet can be added to a VisualElement.
            // The style will be applied to the VisualElement and all of its children.
        }

        private void OnFavoritesUpdated(Favorites favorites)
        {
            var root = rootVisualElement;
            root.Clear();
        
            ReloadRoot();
        }

        private void ReloadRoot()
        {
            var root = rootVisualElement;
        
            // var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/FavoriteElement.uxml");
        
            var scroll = new ScrollView(ScrollViewMode.Vertical);
            root.Add(scroll);

            for (var i = 0; i < _favorites.favoritesList.Count; i++)
            {
                var assetReference = _favorites.favoritesList[i].reference;

                if (assetReference == null)
                    continue;
                
                var elementTree = favoriteElementTreeAsset.CloneTree();
            
                elementTree.RegisterCallback(delegate(MouseUpEvent e)
                {
                    // Debug.Log("callback");
                    EditorGUIUtility.PingObject(assetReference);
                });
                
                var icon = elementTree.Q<Image>("Icon");
                if (icon != null)
                {
                    icon.image = AssetPreview.GetMiniThumbnail(assetReference);
                }
                
                var label = elementTree.Q<Label>("Favorite");
                if (label != null)
                {
                    label.text = assetReference.name;
                }

                scroll.Add(elementTree);
            }

        }
    }
}