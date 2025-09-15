using UnityEngine;

public class JoystickDebugger : MonoBehaviour
{
    // Lista de botones comunes para joysticks genéricos
    private string[] buttons =
    {
        "joystick button 0",
        "joystick button 1",
        "joystick button 2",
        "joystick button 3",
        "joystick button 4",
        "joystick button 5",
        "joystick button 6",
        "joystick button 7",
        "joystick button 8",
        "joystick button 9",
        "joystick button 10",
        "joystick button 11",
        "joystick button 12",
        "joystick button 13",
        "joystick button 14",
        "joystick button 15",
        "joystick button 16",
        "joystick button 17",
        "joystick button 18",
        "joystick button 19"
    };

    void Update()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (Input.GetKeyDown(buttons[i]))
            {
                Debug.Log("Botón presionado: " + buttons[i]);
            }

            if (Input.GetKeyUp(buttons[i]))
            {
                Debug.Log("Botón soltado: " + buttons[i]);
            }
        }
    }
}
