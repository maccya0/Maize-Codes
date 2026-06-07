using UnityEngine;
using UnityEngine.InputSystem;
using static MazeGame.MazeGameConstants;

public class WarpController : MonoBehaviour
{

    private GameObject warpObject;
    private InputSystem_Actions actions;
    private GameObject player;

    public void SetWarpPoint(GameObject _warpObject)
    {
        warpObject = _warpObject;
        actions = new InputSystem_Actions();
        player = null;
    }

    public void RunWarp(InputAction.CallbackContext context)
    {
        if (player == null || warpObject == null) return;
        player.gameObject.transform.position = warpObject.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (warpObject == null) return;
        if (other.gameObject.layer != LayerMask.NameToLayer(PlayerConstants.Layer)) return;
        player = other.gameObject;
        actions.Player.SelectWarp.performed += RunWarp;
        actions.Enable();
    }
    private void OnTriggerExit(Collider other)
    {
        if (warpObject == null) return;
        if (other.gameObject.layer != LayerMask.NameToLayer(PlayerConstants.Layer)) return;
        actions.Disable();
        actions.Player.SelectWarp.performed -= RunWarp;
    }
}
