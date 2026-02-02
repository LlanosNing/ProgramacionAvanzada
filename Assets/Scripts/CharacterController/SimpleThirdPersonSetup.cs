using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Setup automático para el sistema de tercera persona sin animaciones
/// Uso: Menú GameObject → Third Person Simple → Setup Complete Scene
/// </summary>
public class SimpleThirdPersonSetup : MonoBehaviour
{
#if UNITY_EDITOR
    
    [MenuItem("GameObject/Third Person Simple/Setup Complete Scene", false, 0)]
    static void SetupCompleteScene()
    {
        
        // 1. CREAR EL PLAYER (CÁPSULA)
        GameObject player = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        player.name = "Player";
        player.tag = "Player";
        player.transform.position = new Vector3(0, 1, 0);

        // Eliminar el Capsule Collider (usaremos Character Controller)
        DestroyImmediate(player.GetComponent<CapsuleCollider>());

        // Añadir Character Controller
        CharacterController controller = player.AddComponent<CharacterController>();
        controller.height = 2f;
        controller.radius = 0.5f;
        controller.center = new Vector3(0, 0, 0);

        // Crear la capa Player si no existe
        CreateLayer("Player");
        player.layer = LayerMask.NameToLayer("Player");

        // 2. CREAR EL SUELO
        GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
        ground.name = "Ground";
        ground.transform.position = Vector3.zero;
        ground.transform.localScale = new Vector3(5, 1, 5);

        // Crear la capa Ground
        CreateLayer("Ground");
        ground.layer = LayerMask.NameToLayer("Ground");

        // Darle un color al suelo
        Material groundMaterial = new Material(Shader.Find("Standard"));
        groundMaterial.color = new Color(0.3f, 0.5f, 0.3f);
        ground.GetComponent<Renderer>().material = groundMaterial;

        // 3. CREAR ALGUNOS OBSTÁCULOS DE PRUEBA
        CreateTestObstacle("Wall_1", new Vector3(3, 1, 0), new Vector3(1, 2, 4));
        CreateTestObstacle("Wall_2", new Vector3(-3, 1, 0), new Vector3(1, 2, 4));
        CreateTestObstacle("Platform_1", new Vector3(0, 0.5f, 5), new Vector3(3, 1, 3));

        // 4. CREAR LA CÁMARA
        GameObject cameraObj = new GameObject("ThirdPersonCamera");
        cameraObj.tag = "MainCamera";
        Camera cam = cameraObj.AddComponent<Camera>();
        cameraObj.transform.position = new Vector3(0, 3, -5);
        cameraObj.transform.LookAt(player.transform.position + Vector3.up);

        // 5. AÑADIR EL AUDIO LISTENER (si no existe)
        if (cameraObj.GetComponent<AudioListener>() == null)
        {
            cameraObj.AddComponent<AudioListener>();
        }

        // 6. CONFIGURAR LA ILUMINACIÓN
        SetupLighting();

        // Seleccionar el player
        Selection.activeGameObject = player;

        // Mensaje de éxito
        EditorUtility.DisplayDialog(
            "✓ Configuración Completa",
            "¡Escena configurada con éxito!\n\n" +
            "Próximos pasos:\n" +
            "1. Añade SimpleThirdPersonController.cs al Player\n" +
            "2. Añade SimpleCameraController.cs a la cámara\n" +
            "3. Configura los scripts según la guía\n" +
            "4. ¡Presiona Play y disfruta!\n\n" +
            "Tip: Lee GUIA_RAPIDA.md para instrucciones detalladas",
            "Entendido"
        );

        Debug.Log("=== SETUP COMPLETO ===");
        Debug.Log("✓ Player creado en (0, 1, 0)");
        Debug.Log("✓ Suelo creado");
        Debug.Log("✓ Obstáculos de prueba creados");
        Debug.Log("✓ Cámara configurada");
        Debug.Log("✓ Capas creadas: Player y Ground");
        Debug.Log("\nAhora añade los scripts a los GameObjects correspondientes.");
    }

    [MenuItem("GameObject/Third Person Simple/Add Scripts to Selected", false, 1)]
    static void AddScriptsToSelected()
    {
        GameObject selected = Selection.activeGameObject;
        
        if (selected == null)
        {
            EditorUtility.DisplayDialog("Error", "Por favor selecciona un GameObject primero", "OK");
            return;
        }

        // Preguntar qué tipo de objeto es
        int option = EditorUtility.DisplayDialogComplex(
            "¿Qué tipo de GameObject es?",
            $"GameObject seleccionado: {selected.name}\n\n¿Es el Player o la Cámara?",
            "Es el Player",
            "Cancelar",
            "Es la Cámara"
        );

        switch (option)
        {
            case 0: // Player
                SetupPlayer(selected);
                break;
            case 2: // Cámara
                SetupCamera(selected);
                break;
        }
    }

    static void SetupPlayer(GameObject player)
    {
        // Verificar/añadir Character Controller
        CharacterController controller = player.GetComponent<CharacterController>();
        if (controller == null)
        {
            controller = player.AddComponent<CharacterController>();
            controller.height = 2f;
            controller.radius = 0.5f;
            controller.center = new Vector3(0, 0, 0);
        }

        // Asignar tag
        if (!player.CompareTag("Player"))
        {
            player.tag = "Player";
        }

        Debug.Log($"✓ Player '{player.name}' configurado");
        Debug.Log("Ahora añade manualmente el script SimpleThirdPersonController.cs");
        
        EditorUtility.DisplayDialog(
            "Player Configurado",
            "Character Controller añadido correctamente.\n\n" +
            "Siguiente paso:\n" +
            "Arrastra SimpleThirdPersonController.cs al Inspector del Player\n\n" +
            "Luego configura:\n" +
            "- Ground Mask = Ground layer",
            "Entendido"
        );
    }

    static void SetupCamera(GameObject camera)
    {
        // Verificar/añadir Camera component
        Camera cam = camera.GetComponent<Camera>();
        if (cam == null)
        {
            cam = camera.AddComponent<Camera>();
        }

        // Asignar tag
        if (!camera.CompareTag("MainCamera"))
        {
            camera.tag = "MainCamera";
        }

        // Añadir AudioListener si no existe
        if (camera.GetComponent<AudioListener>() == null)
        {
            camera.AddComponent<AudioListener>();
        }

        Debug.Log($"✓ Cámara '{camera.name}' configurada");
        Debug.Log("Ahora añade manualmente el script SimpleCameraController.cs");
        
        EditorUtility.DisplayDialog(
            "Cámara Configurada",
            "Camera component añadido correctamente.\n\n" +
            "Siguiente paso:\n" +
            "Arrastra SimpleCameraController.cs al Inspector de la cámara\n\n" +
            "Luego configura:\n" +
            "- Target = GameObject del Player\n" +
            "- Collision Layers = Everything EXCEPTO Player",
            "Entendido"
        );
    }

    static void CreateTestObstacle(string name, Vector3 position, Vector3 scale)
    {
        GameObject obstacle = GameObject.CreatePrimitive(PrimitiveType.Cube);
        obstacle.name = name;
        obstacle.transform.position = position;
        obstacle.transform.localScale = scale;

        // Asignar a la capa Ground
        obstacle.layer = LayerMask.NameToLayer("Ground");

        // Material
        Material mat = new Material(Shader.Find("Standard"));
        mat.color = new Color(0.5f, 0.5f, 0.5f);
        obstacle.GetComponent<Renderer>().material = mat;
    }

    static void SetupLighting()
    {
        // Buscar luz direccional
        Light dirLight = FindObjectOfType<Light>();
        
        if (dirLight == null)
        {
            GameObject lightObj = new GameObject("Directional Light");
            dirLight = lightObj.AddComponent<Light>();
            dirLight.type = LightType.Directional;
        }

        dirLight.transform.rotation = Quaternion.Euler(50, -30, 0);
        dirLight.color = Color.white;
        dirLight.intensity = 1f;
    }

    static void CreateLayer(string layerName)
    {
        // Verificar si la capa ya existe
        if (LayerMask.NameToLayer(layerName) != -1)
            return;

        SerializedObject tagManager = new SerializedObject(
            AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]
        );
        SerializedProperty layers = tagManager.FindProperty("layers");

        // Buscar un slot vacío
        for (int i = 8; i < layers.arraySize; i++)
        {
            SerializedProperty layer = layers.GetArrayElementAtIndex(i);
            if (string.IsNullOrEmpty(layer.stringValue))
            {
                layer.stringValue = layerName;
                tagManager.ApplyModifiedProperties();
                Debug.Log($"✓ Capa '{layerName}' creada en el slot {i}");
                return;
            }
        }

        Debug.LogWarning($"No se pudo crear la capa '{layerName}'. Todos los slots están ocupados.");
    }

#endif
}
