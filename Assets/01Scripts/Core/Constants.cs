
public static class Constants
{
    #region Parameters

    public const int CHARACTERS_ANIMATIONS_IDLE_COUNT = 7;
    public const string PLAYABLE_CHARACTER_IDENTIFY_MATERIAL_NAME = "PlayableCharacterIdentifyColor";

    #endregion

    public static class Prefabs
    {
        public const string PLAYABLE_CHARACTER = "PlayableCharacter";
        public const string COLELCTIBLE_WOOD = "CollectibleWood";
        public const string CANON_BALL_BULLET = "CanonBallBullet";
    }

    public static class Animations
    {
        public static class Clips
        {
            public const string ON_SELECTED = "OnSelected";
            public const string RUNNING = "Running";
            public const string LIFTING_OBJECT = "LiftingObject";
            public const string EMPTY_STATE = "EmptyState";
        }

        public static class Parameters
        {
            public const string IDLE = "Idle";
            public const string IDLE_SPEED = "IdleSpeed";
        }
    }

    public static class Layers
    {
        public const int PLAYABLE_CHARACTER = 8;
        public const int PLAYER_TOWER = 9;
    }

    public static class Particles
    {
        public const string COLLECTIBLE_TAKEN = "CollectibleTaken";
    }
}