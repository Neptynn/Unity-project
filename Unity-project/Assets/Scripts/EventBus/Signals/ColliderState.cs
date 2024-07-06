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

        public IsWallState(bool IsWall)
        {
            isWall = IsWall;
        }
    }

    public class CheckState
    {
    }
}