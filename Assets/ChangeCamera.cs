using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DoubleEngine;
using DoubleEngine.Atom;

public class ChangeCamera : MonoBehaviour
{
    public Cinemachine.CinemachineVirtualCameraBase[] cameras;
    public static QuaternionD[] rotations = CubeSides.ToZNeg;
    public int StartCameraIndex = 0;
    [SerializeField] public static int _cameraIndex = 0; //[SerializeField] for debug only

    private void Start()
    {
        StartCoroutine(IterariveChangeCameraTo(StartCameraIndex, 0.6f));
    }

    private IEnumerator IterariveChangeCameraTo(int cameraIndex, float timeBetweenIterations = 1.0f)
    {
        cameraIndex = BoundedCameraIndex(cameraIndex);
        while (cameraIndex!= _cameraIndex)
        {
            if (cameraIndex > _cameraIndex)
                _cameraIndex++;
            else
                _cameraIndex--;

            TryChangeCam(_cameraIndex);
            yield return new WaitForSeconds(timeBetweenIterations);
        }
    }

    public void TryNextCam()
    {
        TryChangeCam(_cameraIndex+1);
    }
    public void TryPrevCam()
    {
        TryChangeCam(_cameraIndex-1);
    }


    public void TryChangeCam(int cameraIndex)
    {
        try
        {
            _cameraIndex = BoundedCameraIndex(cameraIndex);
            UpdateCam();
        }
        catch
        {
            Debug.LogWarning($"Error changing camera to Virtual Camera {cameraIndex}. {this.gameObject.name}");
        }
    }

    private int BoundedCameraIndex(int cameraIndex)
    {
        if (cameraIndex < 0)
            return cameras.Length - 1;

        if (cameraIndex >= cameras.Length)
            return 0;

        return cameraIndex;
    }

    private void UpdateCam()
    {
        cameras[_cameraIndex].MoveToTopOfPrioritySubqueue();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryNextCam();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            TryPrevCam();
        }
    }
}
