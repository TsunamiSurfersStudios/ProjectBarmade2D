using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

// Make sure this script is in an Editor folder to work
[ExecuteInEditMode]
public class FloorTilePainter2D : EditorWindow
{
    // Reference to your floor tile sprites
    public List<Sprite> floorTileSprites = new List<Sprite>();
    
    // Parent object for organization
    private GameObject tileParent;
    
    // Settings
    private float tileSize = 1f;
    private bool randomRotation = false;
    private string parentName = "Floor Tiles";
    private int orderInLayer = 0;
    
    // Used for tracking the last drawn position to avoid duplicates
    private Vector2 lastTilePos = new Vector2(-99999, -99999);
    
    // Store sorting layer names
    private string[] sortingLayerNames;
    private int selectedSortingLayer = 0;
    
    [MenuItem("Tools/Floor Tile Painter 2D")]
    public static void ShowWindow()
    {
        GetWindow<FloorTilePainter2D>("Floor Tile Painter 2D");
    }
    
    void OnEnable()
    {
        // Get all sorting layer names for the dropdown
        sortingLayerNames = GetSortingLayerNames();
        
        // Subscribe to scene view events
        SceneView.duringSceneGui += OnSceneGUI;
    }
    
    void OnDisable()
    {
        // Unsubscribe to avoid memory leaks
        SceneView.duringSceneGui -= OnSceneGUI;
    }
    
    private string[] GetSortingLayerNames()
    {
        // Get all sorting layers defined in the project
        List<string> layers = new List<string>();
        
        SerializedObject tagManager = new SerializedObject(
            AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]
        );
        
        SerializedProperty layersProp = tagManager.FindProperty("m_SortingLayers");
        
        if (layersProp != null)
        {
            for (int i = 0; i < layersProp.arraySize; i++)
            {
                SerializedProperty layer = layersProp.GetArrayElementAtIndex(i);
                layers.Add(layer.FindPropertyRelative("name").stringValue);
            }
        }
        
        return layers.ToArray();
    }
    
    void OnGUI()
    {
        GUILayout.Label("Floor Tile Painter 2D", EditorStyles.boldLabel);
        
        // Settings section
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Settings", EditorStyles.boldLabel);
        tileSize = EditorGUILayout.FloatField("Tile Size", tileSize);
        randomRotation = EditorGUILayout.Toggle("Random Rotation", randomRotation);
        parentName = EditorGUILayout.TextField("Parent Object Name", parentName);
        
        // Rendering settings
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Rendering Settings", EditorStyles.boldLabel);
        
        // Sorting Layer dropdown
        selectedSortingLayer = EditorGUILayout.Popup("Sorting Layer", selectedSortingLayer, sortingLayerNames);
        orderInLayer = EditorGUILayout.IntField("Order in Layer", orderInLayer);
        
        // Sprite selection section
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Floor Tile Sprites", EditorStyles.boldLabel);
        
        // Show current sprite count
        EditorGUILayout.LabelField($"Number of Sprites: {floorTileSprites.Count}");
        
        // Button to add new sprite field
        if (GUILayout.Button("Add Sprite Slot"))
        {
            floorTileSprites.Add(null);
        }
        
        // Display all sprite fields
        for (int i = 0; i < floorTileSprites.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            floorTileSprites[i] = (Sprite)EditorGUILayout.ObjectField($"Sprite {i+1}", floorTileSprites[i], typeof(Sprite), false);
            
            // Button to remove this sprite
            if (GUILayout.Button("X", GUILayout.Width(20)))
            {
                floorTileSprites.RemoveAt(i);
                i--;
            }
            EditorGUILayout.EndHorizontal();
        }
        
        // Button to clear all non-null sprites 
        EditorGUILayout.Space();
        if (GUILayout.Button("Clear Empty Sprite Slots"))
        {
            for (int i = floorTileSprites.Count - 1; i >= 0; i--)
            {
                if (floorTileSprites[i] == null)
                    floorTileSprites.RemoveAt(i);
            }
        }
        
        // Instructions
        EditorGUILayout.Space();
        EditorGUILayout.HelpBox("Hold Ctrl and left-click in Scene view to place random floor tiles. Tiles will be positioned on a grid based on the Tile Size.", MessageType.Info);
    }
    
    void OnSceneGUI(SceneView sceneView)
    {
        // Only act if Control (Ctrl) key is pressed
        Event e = Event.current;
        if (e.control && e.type == EventType.MouseDown && e.button == 0)
        {
            // Get the position of the mouse in world space
            Vector3 mousePosition = Event.current.mousePosition;
            mousePosition = HandleUtility.GUIPointToWorldRay(mousePosition).origin;
            mousePosition.z = 0; // Set z to 0 for 2D
            
            // Calculate grid-aligned position for 2D (XY plane)
            Vector2 tilePos = new Vector2(
                Mathf.Floor(mousePosition.x / tileSize) * tileSize,
                Mathf.Floor(mousePosition.y / tileSize) * tileSize
            );
            
            // Only create a tile if we're at a new position
            if (tilePos != lastTilePos)
            {
                PlaceTile(tilePos);
                lastTilePos = tilePos;
                
                // Mark the scene as dirty to ensure it saves
                if (tileParent != null)
                {
                    EditorUtility.SetDirty(tileParent);
                }
                e.Use(); // Handle the event so it doesn't propagate
            }
        }
        else if (e.type == EventType.MouseUp)
        {
            // Reset last position when mouse is released
            lastTilePos = new Vector2(-99999, -99999);
        }
    }
    
    void PlaceTile(Vector2 position)
    {
        // Ensure we have sprites to use
        if (floorTileSprites.Count == 0 || floorTileSprites.TrueForAll(s => s == null))
        {
            Debug.LogWarning("No sprites assigned to the Floor Tile Painter!");
            return;
        }
        
        // Filter out null sprites
        List<Sprite> validSprites = floorTileSprites.FindAll(s => s != null);
        if (validSprites.Count == 0) return;
        
        // Get a random sprite
        Sprite randomSprite = validSprites[Random.Range(0, validSprites.Count)];
        
        // Create or get the parent object
        if (tileParent == null)
        {
            tileParent = GameObject.Find(parentName);
            if (tileParent == null)
            {
                tileParent = new GameObject(parentName);
                Undo.RegisterCreatedObjectUndo(tileParent, "Create Tile Parent");
            }
        }
        
        // Create a new GameObject with a sprite renderer
        GameObject newTile = new GameObject(randomSprite.name);
        Undo.RegisterCreatedObjectUndo(newTile, "Place Floor Tile");
        
        // Set position - for 2D we're using XY plane
        newTile.transform.position = new Vector3(position.x, position.y, 0);
        
        // Apply random rotation if enabled (for 2D, we rotate around Z axis)
        if (randomRotation)
        {
            int randomRotAmount = Random.Range(0, 4);
            newTile.transform.rotation = Quaternion.Euler(0, 0, randomRotAmount * 90);
        }
        
        // Set parent
        newTile.transform.parent = tileParent.transform;
        
        // Add sprite renderer and set the sprite
        SpriteRenderer renderer = newTile.AddComponent<SpriteRenderer>();
        renderer.sprite = randomSprite;
        
        // Set sorting layer and order
        renderer.sortingLayerName = sortingLayerNames[selectedSortingLayer];
        renderer.sortingOrder = orderInLayer;
        
        // Optionally add a Box Collider 2D for future detection
        BoxCollider2D collider = newTile.AddComponent<BoxCollider2D>();
        collider.size = new Vector2(tileSize, tileSize);
    }
}