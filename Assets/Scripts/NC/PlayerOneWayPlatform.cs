using System.Collections;
using UnityEngine;

//most of this code was pulled from this video https://www.youtube.com/watch?v=7rCUt6mqqE8
public class PlayerOneWayPlatform : MonoBehaviour
{
    private const string GROUND_LAYER = "GroundLayer";
    
    private GameObject _currentOneWayPlatform;
    private CapsuleCollider2D _playerCollider;
    private GameObject _player;

    private void Start()
    {
        _currentOneWayPlatform = gameObject;
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerCollider = _player.GetComponent<CapsuleCollider2D>();
    }

    private void Update()
    {
        if (_player.GetComponent<PlayerRefactor>().IsDropping())
        {
            Debug.Log("Platform_Update_Dropping");
            if (_currentOneWayPlatform != null)
            {
                StartCoroutine(DisableCollision());
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("Platform_CollisionEnter");
        if (collision.gameObject.layer == LayerMask.NameToLayer(GROUND_LAYER))
        {
            //Debug.Log("Platform_CollisionEnter_Ground");
            _currentOneWayPlatform = collision.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log("Platform_CollisionExit");
        if (collision.gameObject.layer == LayerMask.NameToLayer(GROUND_LAYER))
        {
            _currentOneWayPlatform = null;
            _player.GetComponent<PlayerRefactor>().SetIsDropping(false); //player should not drop after exiting one of the blocks
        }
    }

    private IEnumerator DisableCollision()
    {
        Debug.Log("Platform_DisableCollision");
        BoxCollider2D platformCollider = _currentOneWayPlatform.GetComponent<BoxCollider2D>();

        Physics2D.IgnoreCollision(_playerCollider, platformCollider);
        yield return new WaitForSeconds(0.2f);
        Physics2D.IgnoreCollision(_playerCollider, platformCollider, false);
        _player.GetComponent<PlayerRefactor>().SetIsDropping(false);
    }
}
