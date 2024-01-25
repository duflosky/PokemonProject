using UI;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreen : MonoBehaviour
{
    [SerializeField] private ImageScroller scroller;
    [SerializeField] private UILogger logger;

    private PlayerInputs _inputs;
    
    private bool isSplashscreen;

    private void Awake()
    {
        _inputs = new PlayerInputs();

        _inputs.Interactions.Any.performed += AnyKeyCallback;
        
        _inputs.Enable();
    }

    private void AnyKeyCallback(InputAction.CallbackContext obj)
    {
        
        scroller.StartSequence();
        _inputs.Interactions.Any.performed -= AnyKeyCallback;
        _inputs.Interactions.A.performed += ScrollPanel;
        _inputs.Interactions.B.performed += UnscrollPanel;
    }

    private void ScrollPanel(InputAction.CallbackContext obj)
    {
        scroller = scroller.GoNext();
        if (scroller == null)
        {
            _inputs.Interactions.A.performed -= ScrollPanel;
            _inputs.Interactions.B.performed -= UnscrollPanel;
            SceneManager.LoadScene("GameScene");
            _inputs.Interactions.A.performed += PassText;
        }
    }

    private void PassText(InputAction.CallbackContext obj)
    {
        if (logger.isLogging) return;
        
    }

    private void UnscrollPanel(InputAction.CallbackContext obj)=> scroller.GoPrevious();
}
