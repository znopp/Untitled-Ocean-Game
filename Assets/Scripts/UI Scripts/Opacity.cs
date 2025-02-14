using UnityEngine;
using UnityEngine.UI;

namespace UI_Scripts
{
    public class Opacity : MonoBehaviour
    {
        public void SetAlpha(Graphic graphic, float alpha)
        {
            if (!graphic) return;
            graphic.color = new Color(graphic.color.r, graphic.color.g, graphic.color.b, alpha);
        }

    }
}
