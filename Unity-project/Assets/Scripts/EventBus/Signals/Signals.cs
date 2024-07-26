using UnityEngine;

namespace CustomEventBus.Signals
{
    public class IsGroundState
    {
        public readonly bool isGround;
        public IsGroundState(bool IsGround)
        {
            isGround = IsGround;
        }
    }

    public class IsRoofUpState
    {
        public readonly bool isRoofUp;
        public IsRoofUpState(bool IsRoofUp)
        {
            isRoofUp = IsRoofUp;
        }
    }
    public class IsWallState
    {
        public readonly bool isWall;
        public readonly bool canWallJump = false;

        public IsWallState(bool IsWall, bool CanWallJump)
        {
            isWall = IsWall;
            canWallJump = CanWallJump;
        }
    }

    public class CheckState
    {
        public readonly bool flagState;
        public CheckState(bool FlagState)
        {
            flagState = FlagState;
        }
    }
    public class PushSignal
    {
    }
    public class StartClimb
    {
    }
    public class PlayerObj
    {
        public readonly PlayerMovement PlayerMovement;
        public PlayerObj(PlayerMovement playerMovement)
        {
            PlayerMovement = playerMovement;
        }
    }
    public class LevelBoundaries
    {
        public readonly CompositeCollider2D level;
        public LevelBoundaries(CompositeCollider2D Level)
        {
            level = Level;
        }
    }

}
