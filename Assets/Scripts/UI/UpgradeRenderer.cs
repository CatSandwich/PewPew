using Singletons;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UpgradeRenderer : MonoBehaviour
    {
        public RectTransform Content;
        public GameObject Prefab;
        public static GameManager Manager => GameManager.Instance;
        public void Start()
        {
            var offset = 20f;
            foreach (var upgrade in Manager.Upgrades)
            {
                var component = Instantiate(Prefab, transform, false).GetComponent<Upgrade>();
                component.gameObject.name = upgrade.Name;
                component.transform.position += Vector3.down * offset;
                component.UpgradeScriptable = upgrade;
                offset += ((RectTransform) Prefab.transform).sizeDelta.y + 20f;
            }

            Content.sizeDelta = new Vector2(Content.sizeDelta.x, offset);
        }
    }
}
