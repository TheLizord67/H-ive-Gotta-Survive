using UnityEngine;
using PurrNet;
public class networkTest : NetworkIdentity
{
    [SerializeField] private Color _color;
    [SerializeField] private Renderer _renderer;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            SetColor();
        }
    }
    private void SetColor()
    {
        _renderer.material.color = _color;
    }
    //[SerializeField] private NetworkIdentity networkIdentity;
    //protected override void OnSpawned()
    //{
    //    base.OnSpawned();
    //    if (!isServer) return;
    //    Instantiate(networkIdentity, Vector3.zero, Quaternion.identity);
    //}
}
