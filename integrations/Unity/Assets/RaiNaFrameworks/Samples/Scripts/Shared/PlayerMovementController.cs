using UnityEngine;

namespace RaiNa.Unity.Samples.Shared
{
    public class PlayerMovementController : MonoBehaviour
    {
        [SerializeField] private float _speed = 5f;
        [SerializeField] private float _jumpForce = 5f;
        [SerializeField] private LayerMask _groundLayer;
        [SerializeField] private float _raycastDistance = 0.5f;
        
        private Rigidbody2D _rigidbody;
        private bool _isGrounded;
        private bool _isTouchingWall;
        
        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }
        
        private void Update()
        {
            HandleMovement();
            HandleJump();
        }
        
        private void HandleMovement()
        {
            float moveInput = Input.GetAxis("Horizontal");
            
            // Rotate player to face the direction of movement
            if (moveInput != 0)
            {
                RotatePlayer(moveInput);
            }
            
            // Prevent moving into walls while in the air
            if (!_isGrounded)
            {
                CheckWall(moveInput);
                if (_isTouchingWall)
                {
                    moveInput = 0;
                }
            }
            
            _rigidbody.linearVelocity = new Vector2(moveInput * _speed, _rigidbody.linearVelocity.y);
        }
        
        private void HandleJump()
        {
            CheckGround();
            
            if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
            {
                _rigidbody.linearVelocity = new Vector2(_rigidbody.linearVelocity.x, _jumpForce);
            }
        }
        
        private void RotatePlayer(float moveInput)
        {
            // Flip the player sprite based on movement direction
            Vector3 scale = transform.localScale;
            scale.x = moveInput > 0 ? 1 : -1;
            transform.localScale = scale;
        }
        
        private void CheckGround()
        {
            // Cast a ray downward from the player to check for ground
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, _raycastDistance, _groundLayer);
            _isGrounded = hit.collider != null;
        }
        
        private void CheckWall(float moveDirection)
        {
            // Check for walls in the direction of movement
            Vector2 wallCheckDirection = moveDirection > 0 ? Vector2.right : Vector2.left;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, wallCheckDirection, _raycastDistance, _groundLayer);
            _isTouchingWall = hit.collider != null;
        }
    }
}
