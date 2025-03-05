using UnityEngine;
using UnityEngine.AddressableAssets;
[CreateAssetMenu(menuName = "GameScene/GameSceneSO")]
public class GameSceneSO : ScriptableObject
{
    public SceneType sceneType;
    public AssetReference sceneReference;
    public Vector3 positionToGo;
    public Vector3 portalPosition;
    public Sprite image;
}