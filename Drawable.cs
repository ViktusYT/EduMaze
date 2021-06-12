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
}