using Microsoft.Xna.Framework;


namespace Core;


internal class ActionManager {
    // FIELDS
    private Character character;
    private Vector2 velocity;
    private float gravity;


    // CONSTRUCTORS
    internal ActionManager(Character character) {
        this.character = character;
        IsGrounded = true;
        gravity = 900f;
    }


    // PROPERTIES
    private Vector2 ActualPosition {
        get => character.ActualPosition;
        set => character.ActualPosition = value;
    }
    private float MovementSpeed {
        get => character.MovementSpeed;
    }
    private float JumpPower {
        get => character.JumpPower;
    }
    private bool IsGrounded {
        get => character.IsGrounded;
        set => character.IsGrounded = value;
    }


    // METHODS
    internal void Update(InputManager input, GameTime gameTime) {
        float deltaTime = (float) gameTime.ElapsedGameTime.TotalSeconds;

        if (input.MoveLeft) {
            velocity.X = -MovementSpeed;
        } else if (input.MoveRight) {
            velocity.X = MovementSpeed;
        } else {
            velocity.X = 0;
        }

        if (input.MoveJump && IsGrounded) {
            velocity.Y = -JumpPower;
            IsGrounded = false;
        }
        
        velocity.Y += gravity * deltaTime;
    }

    internal void Apply(float deltaTime) {
        ActualPosition += velocity * deltaTime;
    }
}
