using SFML.Graphics;
using SFML.System;

namespace EduMaze {
    interface IDrawable {
        void Draw (ref RenderWindow window);
    }

    interface ISelectable {

        void OnClick();
        void OnHover();
        void OnUnhover();
        bool Contains (Vector2f coords);
    }
    public enum Nd {
        MAZE_UP = 0x1,
        MAZE_RIGHT = 0x2,
        MAZE_DOWN = 0x4,
        MAZE_LEFT = 0x8,
        MAZE_VISITED = 0x10,
        MAZE_QUESTION = 0x20
    }
}