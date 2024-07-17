namespace CustomEventBus.Signals
{
    public class StartMoveAnim
    {
        public readonly bool isRunning;
        public StartMoveAnim(bool IsRunning)
        {
            isRunning = IsRunning;
        }
    }

    public class StartAnims
    {
        public readonly bool isDashing;
        public readonly bool isGround;
        public readonly bool isTouchingWall;
        public readonly bool cantJump;
        public StartAnims(bool IsJump, bool IsDashing, bool IsGround, bool IsTouchingWall, bool CantJump)
        {
            isDashing = IsDashing;
            isGround = IsGround;
            isTouchingWall = IsTouchingWall;
            cantJump = CantJump;
        }
    }

    public class StartAnimJump
    {

    }
}