using Microsoft.Xna.Framework.Input;


namespace Core;


internal class InputManager {
    // FIELDS
    private bool moveLeft;
    private bool moveRight;
    private bool moveJump;


    // CONSTRUCTORS



    // PROPERTIES
    internal bool MoveLeft {
        get => moveLeft;
    }
    internal bool MoveRight {
        get => moveRight;
    }
    internal bool MoveJump {
        get => moveJump;
    }


    // METHODS
    internal void Update(KeyboardState keyState) {
        moveLeft = keyState.IsKeyDown(Keys.A);
        moveRight = keyState.IsKeyDown(Keys.D);
        moveJump = keyState.IsKeyDown(Keys.Space);
    }
}
