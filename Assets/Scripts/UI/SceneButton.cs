using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class SceneButton : MonoBehaviour
    {
        public string Scene;
    
        public void Load()
        {
            SceneManager.LoadScene(Scene);
        }
    }
}
