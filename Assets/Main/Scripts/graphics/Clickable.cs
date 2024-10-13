using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TurryWoods
{
        public class Clickable : MonoBehaviour
    {
        public Texture2D image;
        public CursorMode cursorMode = CursorMode.Auto;
        private void OnMouseEnter()
        {
            Vector2 targetForTheImage = new Vector2(image.width / 2 , image.height / 2);
            Cursor.SetCursor(image,targetForTheImage,cursorMode);
        }

        private void OnMouseExit()
        {
            Cursor.SetCursor(null,Vector2.zero,cursorMode);
        }
    }
}

