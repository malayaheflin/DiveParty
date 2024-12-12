using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JamoDrumCanvasCalibrator : MonoBehaviour
{
    #region CONSTANTS

    private const float EPILSON = 1e-6f;

    private const float SHIFT_KEY_MUTLIPLIER = 0.1f;

    private const float OFFSET_SPEED = 10f;

    private const float SCALE_SPEED = 0.1f;

    private const float ROTATE_SPEED = 5f;

    #endregion

    #region PROPERTIES

    [SerializeField]
    public RectTransform tableWrapper;

    [SerializeField]
    public Text calibrateInfoText;

    private bool isCalibrating = false;
    private bool isCalibrated = false;

    public bool IsCalibrated
    {
        get { return isCalibrated; }
    }

    #endregion

    private void Start()
    {
        Calibrate();
    }

    public void Calibrate()
    {
        isCalibrating = true;
        isCalibrated = false;
        calibrateInfoText.gameObject.SetActive(true);
    }

    private void FinishCalibration()
    {
        isCalibrated = true;
        calibrateInfoText.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (isCalibrating) {
            bool shiftKey = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

            // Offset

            float xInput = Input.GetAxisRaw("Horizontal");
            float yInput = Input.GetAxisRaw("Vertical");

            Vector3 offset = Vector3.zero;
            if (Mathf.Abs(xInput) > EPILSON) {
                offset.x += (shiftKey ? SHIFT_KEY_MUTLIPLIER : 1f) * OFFSET_SPEED * Time.deltaTime * xInput;
            }
            if (Mathf.Abs(yInput) > EPILSON) {
                offset.y += (shiftKey ? SHIFT_KEY_MUTLIPLIER : 1f) * OFFSET_SPEED * Time.deltaTime * yInput;
            }
            tableWrapper.localPosition += offset;

            // Rotate

            float rotateInput = 0.0f;
            bool rotateClockwiseKey = Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.LeftBracket) || Input.GetKey(KeyCode.KeypadDivide);
            bool rotateCounterClockwiseKey = Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.RightBracket) || Input.GetKey(KeyCode.KeypadMultiply);

            if (rotateClockwiseKey) rotateInput = 1.0f;
            else if (rotateCounterClockwiseKey) rotateInput = -1.0f;

            if (Mathf.Abs(rotateInput) > EPILSON) {
                tableWrapper.Rotate(0f, 0f, (shiftKey ? SHIFT_KEY_MUTLIPLIER : 1f) * ROTATE_SPEED * Time.deltaTime * rotateInput);
            }

            // Scale

            float scaleInput = 0.0f;
            bool scaleUpKey = Input.GetKey(KeyCode.R) || Input.GetKey(KeyCode.Equals) || Input.GetKey(KeyCode.KeypadPlus);
            bool scaleDownKey = Input.GetKey(KeyCode.T) || Input.GetKey(KeyCode.Minus) || Input.GetKey(KeyCode.KeypadMinus);

            if (scaleUpKey) scaleInput = 1.0f;
            else if (scaleDownKey) scaleInput = -1.0f;

            if (Mathf.Abs(scaleInput) > EPILSON) {
                tableWrapper.localScale = Vector3.one * Mathf.Clamp(
                    tableWrapper.localScale.x +
                    (shiftKey ? SHIFT_KEY_MUTLIPLIER : 1f) * SCALE_SPEED * Time.deltaTime * scaleInput, 0.1f, 2.0f);
            }

            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) {
                FinishCalibration();
            }

            UpdateCalibrateInfo();
        }
    }

    private void UpdateCalibrateInfo()
    {
        calibrateInfoText.text = string.Format("Current Transform: \n" +
            "Position: ({0:0.00}, {1:0.00})\n" +
            "Rotation: ({2:0.00}, {3:0.00}, {4:0.00})\n" +
            "Scale: {5:0.00}",
            tableWrapper.localPosition.x,
            tableWrapper.localPosition.y,
            tableWrapper.localRotation.eulerAngles.x,
            tableWrapper.localRotation.eulerAngles.y,
            tableWrapper.localRotation.eulerAngles.z,
            tableWrapper.localScale.x);
    }
}
