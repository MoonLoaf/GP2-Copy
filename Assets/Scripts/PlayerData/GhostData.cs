using UnityEngine;

[CreateAssetMenu(fileName = "GhostData", menuName = "ScriptableObjects/PlayerData")]
public class GhostData : ScriptableObject {
    public float _moveSpeed;
    public float _rotationSpeed;
    [Tooltip("This value should be above 1 and is only used when the user doesnt give input")]
    public float _deaccel;
}
