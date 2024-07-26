using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomEventBus.Signals;
using CustomEventBus;


public class PlayerMovement : MonoBehaviour, IService
{
    #region Vars

    [Header("Referencies")]
    public PlayerMovementStats MoveStats;
    [SerializeField] private Collider2D _feetColl;
    [SerializeField] private Collider2D _bodyColl;
    [SerializeField] private GameObject _cameraFollowGo;
    private PlayerFollowCam _followCam;
    private float _fallSpeedYDampingChangeTreshold;

    private Rigidbody2D _rb;
    private EventBus _eventBus;
    //movement vars
    public float HorizontalVelocity { get; private set; }
    private bool _isFacingRight;

    //collision check wars
    private RaycastHit2D _groundHit;
    private RaycastHit2D _headHit;
    private RaycastHit2D _wallHit;
    private RaycastHit2D _lastWallHit;
    private bool _isGrounded;
    private bool _bumpedHead;
    private bool _isTouchingWall;
    private bool _cantJump;

    //jump vars
    public float VerticalVelosity { get; private set; }
    private bool _isJumping;
    private bool _isFastFalling;
    private bool _isFalling;
    private float _fastFallTime;
    private float _fastFallRealiseSpeed;
    private int _numberOfJumpUsed;

    //apex vars
    private float _apexPoint;
    private float _timePastApexThreshold;
    private bool _isPastApexThreshold;

    //jump buffer vars
    private float _jumpBufferTimer;
    private bool _jumpReleasedDuringBuffer;

    //coyote time vars
    private float _coyoteTimer;

    //wall slide
    private bool _isWallSliding;
    private bool _isWallSlideFalling;

    //wall jump
    private bool _useWallJumpMoveStats;
    private bool _isWallJumping;
    private float _wallJumpTime;
    private bool _isWallJumpFastFalling;
    private bool _isWallJumpFalling;
    private float _wallJumpFastFallTime;
    private float _wallJumpFastFallReleaseSpeed;

    private float _wallJumpPostBufferTimer;

    private float _wallJumpApexPoint;
    private float _timePastWallJumpApexThreshold;
    private bool _isPastWallJumpApexThreshold;

    //dash vars
    private bool _isDashinng;
    private bool _isAirDashing;
    private float _dashTimer;
    private float _dashOnGroundTimer;
    private int _numberOfDashesUsed;
    private Vector2 _dashDirection;
    private bool _isDashFastFalling;
    private float _dashFastFallTime;
    private float _dashFastFallReleaseSpeed;

    private TrailDash _trailDash;

    #endregion

    #region Main
    private void Awake()
    {
        _trailDash = GetComponentInChildren<TrailDash>();
        _isFacingRight = true;

        _rb = GetComponent<Rigidbody2D>();
        _followCam = _cameraFollowGo.GetComponent<PlayerFollowCam>();
    }
    private void Start()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _eventBus.Invoke(new PlayerObj(this));
        _fallSpeedYDampingChangeTreshold = CameraManager.instance._fallSpeedDampingChargeTheshold;
    }
    private void Update()
    {
        CountTimers();
        JumpChecks();
        WallSlideCheck();
        WallJumpCheck();
        DashCheck();
        LandCheck();

        if (_numberOfJumpUsed >= MoveStats.NumberOfJumpsAllowed && !_isWallJumping && !_isWallSliding)
        {
            _cantJump = true;
        }
        else
        {
            _cantJump = false;
        }
        _eventBus.Invoke(new StartAnims(_isJumping, _isDashinng, _isGrounded, _isTouchingWall, _cantJump));
    }

    private void FixedUpdate()
    {
        CollisionChecks();
        JumpDown();
        Jump();
        Fall();
        WallSlide();
        WallJump();
        Dash();

        if (_isGrounded)
        {
            Move(MoveStats.GroundAcceleration, MoveStats.GroundDeceleration, InputManager.Movement);
        }
        else
        {
            //wall jumping 
            if (_useWallJumpMoveStats)
            {
                Move(MoveStats.WallJumpMoveAcceleration, MoveStats.WallJumpMoveDeceleration, InputManager.Movement);
            }
            //airborne
            else
            {
                Move(MoveStats.AirAcceleration, MoveStats.AirDeceleration, InputManager.Movement);
            }

        }

        ApplyVelocity();
        CamaraFall();
    }

    public float GetRbVelocityY()
    {
        return _rb.velocity.y;
    }

    private void ApplyVelocity()
    {
        if (!_isDashinng)
        {
            //CLAMP FALL SPEED
            VerticalVelosity = Mathf.Clamp(VerticalVelosity, -MoveStats.MaxFallSpeed, 50f);
        }
        else
        {
            VerticalVelosity = Mathf.Clamp(VerticalVelosity, -50f, 50f);
        }

        _rb.velocity = new Vector2(HorizontalVelocity, VerticalVelosity);
    }

    private void OnDrawGizmos()
    {
        if (MoveStats.ShowWalkJumpArc)
        {
            DrawJumpArc(MoveStats.MaxWalkSpeed, Color.white);
        }
    }

    #endregion

    #region Movement

    private void Move(float acceleration, float deceleration, Vector2 moveInput)
    {
        if (!_isDashinng)
        {
            if (Mathf.Abs(moveInput.x) >= MoveStats.MoveTreshold)
            {
                TurnCheck(moveInput);

                float targetVelocity = 0f;
                targetVelocity = moveInput.x * MoveStats.MaxWalkSpeed;

                HorizontalVelocity = Mathf.Lerp(HorizontalVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);

                _eventBus.Invoke(new StartMoveAnim(true));
            }
            else if (Mathf.Abs(moveInput.x) < MoveStats.MoveTreshold)
            {
                HorizontalVelocity = Mathf.Lerp(HorizontalVelocity, 0f, deceleration * Time.fixedDeltaTime);
                _eventBus.Invoke(new StartMoveAnim(false));
            }
        }
    }
    private void TurnCheck(Vector2 moveInput)
    {
        if (moveInput.x < 0 && _isFacingRight)
        {
            Turn(false);
        }
        else if (moveInput.x > 0 && !_isFacingRight)
        {
            Turn(true);
        }
    }
    private void Turn(bool turnRight)
    {
        if (turnRight)
        {
            _isFacingRight = true;
            Vector3 _rotate = new Vector3(transform.rotation.x, 0f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(_rotate);

            _followCam.CallTurn();
        }
        else
        {
            _isFacingRight = false;
            Vector3 _rotate = new Vector3(transform.rotation.x, 180f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(_rotate);

            _followCam.CallTurn();
        }
    }

    public bool GetIsFacingRight()
    {
        return _isFacingRight;
    }
    #endregion

    #region Land/Fall

    private void LandCheck()
    {
        //LANDED
        if ((_isJumping || _isFalling || _isWallJumpFalling || _isWallJumping || _isWallSlideFalling || _isWallSliding || _isDashFastFalling) && _isGrounded && VerticalVelosity <= 0f)
        {
            ResetJumpValues();
            StopWallSlide();
            ResetWallJumpValues();
            ResetDashes();

            _numberOfJumpUsed = 0;

            VerticalVelosity = Physics2D.gravity.y;

            if (_isDashFastFalling && _isGrounded)
            {
                ResetDashValues();
                return;
            }
            ResetDashValues();
        }
    }
    private void Fall()
    {
        //NORMAL GRIVITY WHILE FALLING
        if (!_isGrounded && !_isJumping && !_isWallSliding && !_isWallJumping && !_isDashinng && !_isDashFastFalling)
        {
            if (!_isFalling)
            {
                _isFalling = true;
            }
            VerticalVelosity += MoveStats.Gravity * Time.fixedDeltaTime;
        }
    }

    #endregion

    #region Jump

    private void ResetJumpValues()
    {
        _isJumping = false;
        _isFalling = false;
        _isFastFalling = false;
        _fastFallTime = 0f;
        _isPastApexThreshold = false;
    }

    private void JumpChecks()
    {
        //WHEN PRESS THE JUMP BUTTON
        if (InputManager.JumpWasPressed)
        {
            if (_isWallSlideFalling && _wallJumpPostBufferTimer >= 0f)
            {
                return;
            }
            else if (_isWallSliding || (_isTouchingWall && !_isGrounded))
            {
                return;
            }

            _jumpBufferTimer = MoveStats.JumpBufferTime;
            _jumpReleasedDuringBuffer = false;
        }
        //WHEN REALESE THE JUMP BUTTON
        if (InputManager.JumpWasReleased)
        {
            if (_jumpBufferTimer > 0f)
            {
                _jumpReleasedDuringBuffer = true;
            }

            if (_isJumping && VerticalVelosity > 0f)
            {
                if (_isPastApexThreshold)
                {
                    _isPastApexThreshold = false;
                    _isFastFalling = true;
                    _fastFallTime = MoveStats.TimeForUpwardsCancel;
                    VerticalVelosity = 0f;
                }
                else
                {
                    _isFastFalling = true;
                    _fastFallRealiseSpeed = VerticalVelosity;
                }
            }
        }
        //INITIATE JUMP WITH JUMP BUFFERING AND COYOTE TIME
        if (_jumpBufferTimer > 0f && !_isJumping && (_isGrounded || _coyoteTimer > 0f))
        {
            InitiateJump(1);

            if (_jumpReleasedDuringBuffer)
            {
                _isFastFalling = true;
                _fastFallRealiseSpeed = VerticalVelosity;
            }
        }
        //DOUBLE JUMP
        else if (_jumpBufferTimer > 0f && (_isJumping || _isWallJumping || _isWallSlideFalling || _isAirDashing || _isDashFastFalling) && !_isTouchingWall && _numberOfJumpUsed < MoveStats.NumberOfJumpsAllowed)
        {
            _isFastFalling = false;
            InitiateJump(1);

            if (_isDashFastFalling)
            {
                _isDashFastFalling = false;
            }
        }
        //AIR JUMP AFTER COYOTE TIME LAPSED
        else if (_jumpBufferTimer > 0f && _isFalling && !_isWallSlideFalling && _numberOfJumpUsed < MoveStats.NumberOfJumpsAllowed - 1)
        {
            InitiateJump(1);
            _isFastFalling = false;
        }
    }

    private void InitiateJump(int numberOfJumpsUsed)
    {
        if (!_isJumping)
        {
            _isJumping = true;
        }

        ResetWallJumpValues();
        _eventBus.Invoke(new StartAnimJump());

        _jumpBufferTimer = 0f;
        _numberOfJumpUsed += numberOfJumpsUsed;
        VerticalVelosity = MoveStats.InitialJumpVelosity;
    }

    private void Jump()
    {
        //APPLY GRAVITY WHILE JUMPIND
        if (_isJumping)
        {
            //CHECK FOR HEAD JUMP
            if (_bumpedHead)
            {
                _isFastFalling = true;
            }
            //GRAVITY ON ASCENDING
            if (VerticalVelosity >= 0f)
            {
                //APEX CONTROL
                _apexPoint = Mathf.InverseLerp(MoveStats.InitialJumpVelosity, 0f, VerticalVelosity);

                if (_apexPoint > MoveStats.ApexTheshold)
                {
                    if (!_isPastApexThreshold)
                    {
                        _isPastApexThreshold = true;
                        _timePastApexThreshold = 0f;
                    }
                    if (_isPastApexThreshold)
                    {
                        _timePastApexThreshold += Time.fixedDeltaTime;
                        if (_timePastApexThreshold < MoveStats.ApexHendTime)
                        {
                            VerticalVelosity = 0f;
                        }
                        else
                        {
                            VerticalVelosity = -0.01f;
                        }
                    }
                }
                //GRAVITY ON ASCENDING BUT NOT PAST APEX THRESHOLD
                else if (!_isFastFalling)
                {
                    VerticalVelosity += MoveStats.Gravity * Time.fixedDeltaTime;
                    if (_isPastApexThreshold)
                    {
                        _isPastApexThreshold = false;
                    }
                }
            }
            //GRAVITY ON DESCENDING
            else if (!_isFastFalling)
            {
                VerticalVelosity += MoveStats.Gravity * MoveStats.GravityOnRealeseMulripluer * Time.fixedDeltaTime;
            }
            else if (VerticalVelosity < 0f)
            {
                if (!_isFalling)
                {
                    _isFalling = true;
                }
            }
            //JUMP CUT
            if (_isFastFalling)
            {
                if (_fastFallTime >= MoveStats.TimeForUpwardsCancel)
                {
                    VerticalVelosity += MoveStats.Gravity * MoveStats.GravityOnRealeseMulripluer * Time.fixedDeltaTime;
                }
                else if (_fastFallTime < MoveStats.TimeForUpwardsCancel)
                {
                    VerticalVelosity = Mathf.Lerp(_fastFallRealiseSpeed, 0f, (_fastFallTime / MoveStats.TimeForUpwardsCancel));
                }
                _fastFallTime += Time.fixedDeltaTime;
            }
        }
    }
    #endregion

    #region Wall Slide

    private void WallSlideCheck()
    {
        if (_isTouchingWall && !_isGrounded && !_isDashinng)
        {
            if (VerticalVelosity < 0f && !_isWallSliding)
            {
                ResetJumpValues();
                ResetWallJumpValues();
                ResetJumpValues();

                if (MoveStats.ResetDashOnWallSlide)
                {
                    ResetDashes();
                }

                _isWallSlideFalling = false;
                _isWallSliding = true;

                if (MoveStats.ResetDashOnWallSlide)
                {
                    _numberOfJumpUsed = 0;
                }
            }
        }

        else if (_isWallSliding && !_isTouchingWall && !_isGrounded && !_isWallSlideFalling)
        {
            _isWallSlideFalling = true;
            StopWallSlide();
        }
        else
        {
            StopWallSlide();
        }
    }

    private void StopWallSlide()
    {
        if (_isWallSliding)
        {
            _numberOfJumpUsed++;

            _isWallSliding = false;
        }
    }

    private void WallSlide()
    {
        if (_isWallSliding)
        {
            VerticalVelosity = Mathf.Lerp(VerticalVelosity, -MoveStats.WallSlideSpeed, MoveStats.WallSlideDecelerationSpeed * Time.fixedDeltaTime);
        }
    }

    #endregion

    #region Wall Jump

    private void WallJumpCheck()
    {
        if (ShouldApplyPostWallJumpBuffer())
        {
            _wallJumpPostBufferTimer = MoveStats.WallJumpPostBufferTime;
        }

        //wall jump fast falling
        if (InputManager.JumpWasReleased && !_isWallSliding && !_isTouchingWall && _isWallJumping)
        {
            if (VerticalVelosity > 0f)
            {
                if (_isPastWallJumpApexThreshold)
                {
                    _isPastWallJumpApexThreshold = false;
                    _isWallJumpFastFalling = true;
                    _wallJumpFastFallTime = MoveStats.TimeForUpwardsCancel;

                    VerticalVelosity = 0f;
                }
                else
                {
                    _isWallJumpFastFalling = true;
                    _wallJumpFastFallReleaseSpeed = VerticalVelosity;
                }
            }
        }
        //actual jump with post wall jump buffer time
        if (InputManager.JumpWasPressed && _wallJumpPostBufferTimer > 0f)
        {
            InitiateWallJump();
        }
    }
    private void InitiateWallJump()
    {
        if (!_isWallJumping)
        {
            _isWallJumping = true;
            _useWallJumpMoveStats = true;
        }

        StopWallSlide();
        ResetJumpValues();
        _wallJumpTime = 0f;
        _eventBus.Invoke(new StartAnimJump());

        VerticalVelosity = MoveStats.InitialWallJumpVelosity;

        int dirMultiplier = 0;
        Vector2 hitPoint = _lastWallHit.collider.ClosestPoint(_bodyColl.bounds.center);

        if (hitPoint.x > transform.position.x)
        {
            dirMultiplier = -1;
        }
        else { dirMultiplier = 1; }

        HorizontalVelocity = Mathf.Abs(MoveStats.WallJumpDirection.x) * dirMultiplier;
    }

    private void WallJump()
    {
        //APPLY WALL JUMP GRAVITY
        if (_isWallJumping)
        {
            //TIME TO TAKE OVER MOVEMENT CONTROLS WHILE WALL JUMPING
            _wallJumpTime += Time.fixedDeltaTime;
            if (_wallJumpTime >= MoveStats.TimeTillJumpApex)
            {
                _useWallJumpMoveStats = false;
            }

            //HIT HEAD
            if (_bumpedHead)
            {
                _isWallJumpFastFalling = true;
                _useWallJumpMoveStats = false;
            }

            //GRAVITY IN ASCENDING
            if (VerticalVelosity >= 0f)
            {
                //APEX CONTROLS
                _wallJumpApexPoint = Mathf.InverseLerp(MoveStats.WallJumpDirection.y, 0f, VerticalVelosity);

                if (_wallJumpApexPoint > MoveStats.ApexTheshold)
                {
                    if (!_isPastWallJumpApexThreshold)
                    {
                        _isPastWallJumpApexThreshold = true;
                        _timePastWallJumpApexThreshold = 0f;
                    }

                    if (_isPastWallJumpApexThreshold)
                    {
                        _timePastWallJumpApexThreshold += Time.fixedDeltaTime;
                        if (_timePastWallJumpApexThreshold < MoveStats.ApexHendTime)
                        {
                            VerticalVelosity = 0f;
                        }
                        else
                        {
                            VerticalVelosity = -0.01f;
                        }
                    }
                }

                //GRAVITY IN ASCENDING BUT NOT PAST APEX THRESHOLD
                else if (!_isWallJumpFastFalling)
                {
                    VerticalVelosity += MoveStats.WallJumpGravity * Time.fixedDeltaTime;

                    if (_isPastWallJumpApexThreshold)
                    {
                        _isPastWallJumpApexThreshold = false;
                    }
                }
            }

            //GRAVITY ON DESENDING
            else if (!_isWallJumpFastFalling)
            {
                VerticalVelosity += MoveStats.WallJumpGravity * Time.fixedDeltaTime;
            }

            else if (VerticalVelosity < 0f)
            {
                if (!_isWallJumpFalling)
                {
                    _isWallJumpFalling = true;
                }
            }
        }

        //HANDLE WALL JUMP CUT TIME
        if (_isWallJumpFastFalling)
        {
            if (_wallJumpFastFallTime >= MoveStats.TimeForUpwardsCancel)
            {
                VerticalVelosity += MoveStats.WallJumpGravity * MoveStats.WallJumpGravityOnRealeaseMultiplier * Time.fixedDeltaTime;
            }
            else if (_wallJumpFastFallTime < MoveStats.TimeForUpwardsCancel)
            {
                VerticalVelosity = Mathf.Lerp(_wallJumpFastFallReleaseSpeed, 0f, (_wallJumpFastFallTime / MoveStats.TimeForUpwardsCancel));
            }

            _wallJumpFastFallTime += Time.fixedDeltaTime;
        }
    }

    private bool ShouldApplyPostWallJumpBuffer()
    {
        if (!_isGrounded && (_isTouchingWall || _isWallSliding))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void ResetWallJumpValues()
    {
        _isWallSlideFalling = false;
        _useWallJumpMoveStats = false;
        _isWallJumping = false;
        _isWallJumpFastFalling = false;
        _isWallJumpFalling = false;
        _isPastApexThreshold = false;

        _wallJumpFastFallTime = 0f;
        _wallJumpTime = 0f;
    }

    #endregion

    #region Dash

    private void DashCheck()
    {
        if (InputManager.DashWasPressed)
        {
            //ground dash
            if (_isGrounded && _dashOnGroundTimer < 0 && !_isDashinng)
            {
                InitiateDash();
            }

            //air dash
            else if (!_isGrounded && !_isDashinng && _numberOfDashesUsed < MoveStats.NumberOfDashes)
            {
                _isAirDashing = true;
                InitiateDash();
                //you keft a wallslide but dashed within the wall jump post buffer timer
                if (_wallJumpPostBufferTimer > 0f)
                {
                    _numberOfJumpUsed--;
                    if (_numberOfJumpUsed < 0f)
                    {
                        _numberOfJumpUsed = 0;
                    }
                }
            }

        }
    }

    private void InitiateDash()
    {
        _dashDirection = InputManager.Movement;

        Vector2 clossestDirection = Vector2.zero;
        float minDistance = Vector2.Distance(_dashDirection, MoveStats.DashDirections[0]);

        for (int i = 0; i < MoveStats.DashDirections.Length; i++)
        {
            //skip if we hit it bang on
            if (_dashDirection == MoveStats.DashDirections[i])
            {
                clossestDirection = _dashDirection;
                break;
            }

            float distance = Vector2.Distance(_dashDirection, MoveStats.DashDirections[i]);

            //check if this is a diagonal direction and apply bias
            bool isDiagonal = (Mathf.Abs(MoveStats.DashDirections[i].x) == 1 && Mathf.Abs(MoveStats.DashDirections[i].y) == 1);
            if (isDiagonal)
            {
                distance -= MoveStats.DashDiagonalBias;
            }
            else if (distance < minDistance)
            {
                minDistance = distance;
                clossestDirection = MoveStats.DashDirections[i];
            }
        }

        //handle direction with NO inputs
        if (clossestDirection == Vector2.zero)
        {
            if (_isFacingRight)
            {
                clossestDirection = Vector2.right;
            }
            else
            {
                clossestDirection = Vector2.left;
            }
        }

        _dashDirection = clossestDirection;
        _numberOfDashesUsed++;
        _isDashinng = true;
        _dashTimer = 0f;
        _dashOnGroundTimer = MoveStats.TimeBtwDashedOnGround;

        ResetJumpValues();
        ResetWallJumpValues();
        StopWallSlide();
    }

    private void Dash()
    {
        if (_isDashinng)
        {
            _trailDash.enabled = true;
            //stop the dash after the timer
            _dashTimer += Time.fixedDeltaTime;
            if (_dashTimer >= MoveStats.DashTime)
            {
                _trailDash.enabled = false;
                if (_isGrounded)
                {
                    ResetDashes();
                }

                _isAirDashing = false;
                _isDashinng = false;

                if (!_isJumping && !_isWallJumping)
                {
                    _dashFastFallTime = 0f;
                    _dashFastFallReleaseSpeed = VerticalVelosity;

                    if (!_isGrounded)
                    {
                        _isDashFastFalling = true;
                    }
                }

                return;
            }

            HorizontalVelocity = MoveStats.DashSpeed * _dashDirection.x;

            if (_dashDirection.y != 0f || _isAirDashing)
            {
                VerticalVelosity = MoveStats.DashSpeed * _dashDirection.y;
            }
        }

        //HANDLE DASH CUT TIME
        else if (_isDashFastFalling)
        {
            if (VerticalVelosity > 0f)
            {
                if (_dashFastFallTime < MoveStats.DashTimeForUpwardsCancel)
                {
                    VerticalVelosity = Mathf.Lerp(_dashFastFallReleaseSpeed, 0f, (_dashFastFallTime / MoveStats.DashTimeForUpwardsCancel));
                }
                else if (_dashFastFallTime >= MoveStats.DashTimeForUpwardsCancel)
                {
                    VerticalVelosity += MoveStats.Gravity * MoveStats.DashGravityOnReleaseMultiplier * Time.fixedDeltaTime;
                }

                _dashFastFallTime += Time.fixedDeltaTime;
            }

            else
            {
                VerticalVelosity += MoveStats.Gravity * MoveStats.DashGravityOnReleaseMultiplier * Time.fixedDeltaTime;
            }
        }

    }

    private void ResetDashValues()
    {
        _isDashFastFalling = false;
        _dashOnGroundTimer = -0.01f;
    }
    private void ResetDashes()
    {
        _numberOfDashesUsed = 0;
    }


    #endregion

    #region Jump Off

    public void StaticRBType(bool Static)
    {
        if(Static)
        {
            _rb.bodyType = RigidbodyType2D.Static;
        }
        else
        {
            _rb.bodyType = RigidbodyType2D.Dynamic;
        }

    }

    public void JumpDown()
    {
        if (InputManager.JumpDownWasPressed)
        {
            StartCoroutine("JumpOff");
        }

    }
    IEnumerator JumpOff()
    {
        Physics2D.IgnoreLayerCollision(10, 11, true);
        yield return new WaitForSeconds(0.3f);
        Physics2D.IgnoreLayerCollision(10, 11, false);
    }
    #endregion

    #region Collision Checks

    private void IsGrounded()
    {

        Vector2 boxCastOrigin = new Vector2(_feetColl.bounds.center.x, _feetColl.bounds.min.y);
        Vector2 boxCastSize = new Vector2(_feetColl.bounds.size.x, MoveStats.GroundDetectionRayLength);

        _groundHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.down, MoveStats.GroundDetectionRayLength, MoveStats.GroundLayer);
        if (_groundHit.collider != null)
        {
            _isGrounded = true;
        }
        else { _isGrounded = false; }

        #region Debag Visualisation
        if (MoveStats.DebugShowIsGroundedBox)
        {
            Color rayColor;
            if (_isGrounded)
            {
                rayColor = Color.green;
            }
            else { rayColor = Color.red; }
            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2, boxCastOrigin.y), Vector2.down * MoveStats.GroundDetectionRayLength, rayColor);
            Debug.DrawRay(new Vector2(boxCastOrigin.x + boxCastSize.x / 2, boxCastOrigin.y), Vector2.down * MoveStats.GroundDetectionRayLength, rayColor);
            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2, boxCastOrigin.y - MoveStats.GroundDetectionRayLength), Vector2.right * boxCastSize.x, rayColor);
        }
        #endregion
    }
    public bool GetIsGrounded()
    {
        return _isGrounded;
    }

    private void BumpedHead()
    {
        Vector2 boxCastOrigin = new Vector2(_feetColl.bounds.center.x, _bodyColl.bounds.max.y);
        Vector2 boxCastSize = new Vector2(_feetColl.bounds.size.x * MoveStats.HeadWidth, MoveStats.HeadDetectionRayLenght);

        _headHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.up, MoveStats.HeadDetectionRayLenght, MoveStats.WallLayer);
        if (_headHit.collider != null)
        {
            _bumpedHead = true;
        }
        else
        {
            _bumpedHead = false;
        }

        #region Debug Visualisation

        if (MoveStats.DebagShowHeadBumpBox)
        {
            float headWidth = MoveStats.HeadWidth;

            Color rayColor;
            if (_bumpedHead)
            {
                rayColor = Color.green;
            }
            else
            {
                rayColor = Color.red;

                Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2 * headWidth, boxCastOrigin.y), Vector2.up * MoveStats.HeadDetectionRayLenght, rayColor);
                Debug.DrawRay(new Vector2(boxCastOrigin.x + (boxCastSize.x / 2) * headWidth, boxCastOrigin.y), Vector2.up * MoveStats.HeadDetectionRayLenght, rayColor);
                Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2 * headWidth, boxCastOrigin.y + MoveStats.HeadDetectionRayLenght), Vector2.right * boxCastSize.x * headWidth, rayColor);
            }
            #endregion
        }

    }

    private void IsTouchingWall()
    {
        float originEndPoint = 0f;
        if (_isFacingRight)
        {
            originEndPoint = _bodyColl.bounds.max.x;
        }
        else { originEndPoint = _bodyColl.bounds.min.x; }

        float adjustedHeight = _bodyColl.bounds.size.y * MoveStats.WallDetectionRayHeightMultiplier;

        Vector2 boxCastOrigin = new Vector2(originEndPoint, _bodyColl.bounds.center.y);
        Vector2 boxCastSize = new Vector2(MoveStats.WallDetectionRayLength, adjustedHeight);

        _wallHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, transform.right, MoveStats.WallDetectionRayLength, MoveStats.WallLayer);
        if (_wallHit.collider != null)
        {
            _lastWallHit = _wallHit;
            _isTouchingWall = true;
        }
        else { _isTouchingWall = false; }

        #region Debag Visualization

        if (MoveStats.DebagShowWallHitBox)
        {
            Color rayColor;
            if (_isTouchingWall)
            {
                rayColor = Color.green;
            }
            else { rayColor = Color.red; }

            Vector2 boxBottomLeft = new Vector2(boxCastOrigin.x - boxCastSize.x / 2, boxCastOrigin.y - boxCastSize.y / 2);
            Vector2 boxBottomRight = new Vector2(boxCastOrigin.x + boxCastSize.x / 2, boxCastOrigin.y - boxCastSize.y / 2);
            Vector2 boxTopLeft = new Vector2(boxCastOrigin.x - boxCastSize.x / 2, boxCastOrigin.y + boxCastSize.y / 2);
            Vector2 boxTopRight = new Vector2(boxCastOrigin.x + boxCastSize.x / 2, boxCastOrigin.y + boxCastSize.y / 2);

            Debug.DrawLine(boxBottomLeft, boxBottomRight, rayColor);
            Debug.DrawLine(boxBottomRight, boxTopRight, rayColor);
            Debug.DrawLine(boxTopRight, boxTopLeft, rayColor);
            Debug.DrawLine(boxTopLeft, boxBottomLeft, rayColor);
        }
        #endregion
    }

    private void CollisionChecks()
    {
        IsGrounded();
        BumpedHead();
        IsTouchingWall();
    }

    #endregion

    #region Timers

    private void CountTimers()
    {
        //jump buffer
        _jumpBufferTimer -= Time.deltaTime;

        //jump coyote time
        if (!_isGrounded)
        {
            _coyoteTimer -= Time.deltaTime;
        }
        else { _coyoteTimer = MoveStats.JumpCoyoteTime; }

        //wall jump buffer timer
        if (!ShouldApplyPostWallJumpBuffer())
        {
            _wallJumpPostBufferTimer -= Time.deltaTime;
        }

        //dash timer
        if (_isGrounded)
        {
            _dashOnGroundTimer -= Time.deltaTime;
        }
    }
    #endregion

    #region Jump Visualization

    private void DrawJumpArc(float moveSpeed, Color gismoColor)
    {
        Vector2 startPosition = new Vector2(_feetColl.bounds.center.x, _feetColl.bounds.min.y);
        Vector2 previousPosition = startPosition;
        float speed = 0f;
        if (MoveStats.DrawRight)
        {
            speed = moveSpeed;
        }
        else { speed = -moveSpeed; }
        Vector2 velocity = new Vector2(speed, MoveStats.InitialJumpVelosity);

        Gizmos.color = gismoColor;

        float timeStep = 2 * MoveStats.TimeTillJumpApex / MoveStats.ArcResolution;
        //float totaltime = (2 * MoveStats.TimeTillJumpApex) + MoveStats.ApexHendTime;

        for (int i = 0; i < MoveStats.VisualisationSteps; i++)
        {
            float simulationTime = i * timeStep;
            Vector2 displacement;
            Vector2 drawPoint;

            if (simulationTime < MoveStats.TimeTillJumpApex)//Ascending
            {
                displacement = velocity * simulationTime + 0.5f * new Vector2(0, MoveStats.Gravity) * simulationTime * simulationTime;
            }
            else if (simulationTime < MoveStats.TimeTillJumpApex + MoveStats.ApexHendTime)//Apex hang time
            {
                float apexTime = simulationTime - MoveStats.TimeTillJumpApex;
                displacement = velocity * MoveStats.TimeTillJumpApex + 0.5f * new Vector2(0, MoveStats.Gravity) * MoveStats.TimeTillJumpApex * MoveStats.TimeTillJumpApex;
                displacement += new Vector2(speed, 0) * apexTime;//No vertical movement during hang time
            }
            else //Descending
            {
                float descendTime = simulationTime - (MoveStats.TimeTillJumpApex + MoveStats.ApexHendTime);
                displacement = velocity * MoveStats.TimeTillJumpApex + 0.5f * new Vector2(0, MoveStats.Gravity) * MoveStats.TimeTillJumpApex * MoveStats.TimeTillJumpApex;
                displacement += new Vector2(speed, 0) * MoveStats.ApexHendTime;//Horisontal movement during hang time;
                displacement += new Vector2(speed, 0) * descendTime + 0.5f * new Vector2(0, MoveStats.Gravity) * descendTime * descendTime;
            }
            drawPoint = startPosition + displacement;
            if (MoveStats.StopOnCollision)
            {
                RaycastHit2D hit = Physics2D.Raycast(previousPosition, drawPoint - previousPosition, Vector2.Distance(previousPosition, drawPoint), MoveStats.GroundLayer);
                if (hit.collider != null)
                {
                    // If a hit is detected, stop drawing the arc at hit point
                    Gizmos.DrawLine(previousPosition, hit.point);
                    break;
                }
                Gizmos.DrawLine(previousPosition, drawPoint);
                previousPosition = drawPoint;
            }
        }
    }
    #endregion

    #region Camera

    private void CamaraFall()
    {
        if (_rb.velocity.y < _fallSpeedYDampingChangeTreshold && !CameraManager.instance.IsLerpingYDaming && !CameraManager.instance.LerpedFromPlayerFalling)
        {
            CameraManager.instance.LerpYDamping(true);
        }
        if (_rb.velocity.y > 0f && !CameraManager.instance.IsLerpingYDaming && CameraManager.instance.LerpedFromPlayerFalling)
        {
            CameraManager.instance.LerpedFromPlayerFalling = false;
            CameraManager.instance.LerpYDamping(false);
        }
    }

    #endregion
}
