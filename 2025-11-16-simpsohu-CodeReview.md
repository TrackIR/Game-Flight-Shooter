## Branch Metadata

**Title:**
Leo-ShipMovement-TrackIR

**Scope:**
A first-person implementation of the spaceship with full TrackIR functionality.

**Date:**
2025-11-05 (NOTE: Not currently merged with main, waiting until features are finished before making pull requests).

**Author:**
Leo Yudelson

## Review Comments

**For Reference:** Reviews that reference specific files and lines will be marked as `<name>.<extension>:<line>-<line>`, i.e. `TrackIRComponent.cs:44-54`

### Initial Comments before Code Review:
- The current implementation has a parent camera and a child camera. The parent camera reads and performs a smooth lerp to the child cameras rotation. The child camera could be made into an empty object with the same effect, preventing any bugs in the future where the child camera is accidentally made into the current viewing camera mid-game.
- The head-tracking controls are not precise enough to be able to reliably shoot asteroids. Not necessarily a code review, which is why I'm writing this in the comments before the code review.
- None of the code has comments, which makes reading the code more difficult than it needs to be.

### `Third Person Camera Movement.cs:6-63`:

**Code Snippet:**
```cs
public float acceleration = 5f;
public float maxSpeed = 10f;
public float minSpeed = 1f;
public float sideSpeed = 5f;
public float smoothing = 5f;

public float neutralZone = 5f;
public float pitchSensitivityDown = 2.0f;
public float pitchSensitivityUp = 0.5f;
public float yawSensitivity = 1.0f;

private float currentSpeed = 0f;
private Vector3 currentVelocity = Vector3.zero;

void Update()
{
    float pitch = transform.eulerAngles.x;
    float yaw = transform.eulerAngles.y;

    if (pitch > 180f) pitch -= 360f;
    if (yaw > 180f) yaw -= 360f;

    float targetSpeed;

    if (Mathf.Abs(pitch) <= neutralZone)
    {
        targetSpeed = minSpeed;
    }
    else
    {
        float effectivePitch = Mathf.Abs(pitch) - neutralZone;
        float normalizedPitch = Mathf.Clamp(effectivePitch / (90f - neutralZone), 0f, 1f);

        if (pitch < 0)
        {
            targetSpeed = Mathf.Lerp(minSpeed, maxSpeed * 0.5f, normalizedPitch * pitchSensitivityUp);
        }
        else
        {
            targetSpeed = Mathf.Lerp(minSpeed, maxSpeed, normalizedPitch * pitchSensitivityDown);
        }
    }

    currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, acceleration * Time.deltaTime);

    float sidewaysSpeed = 0f;
    if (Mathf.Abs(yaw) > neutralZone)
    {
        float effectiveYaw = (Mathf.Abs(yaw) - neutralZone) * Mathf.Sign(yaw);
        float normalizedYaw = Mathf.Clamp(effectiveYaw / (90f - neutralZone), -1f, 1f);
        sidewaysSpeed = normalizedYaw * sideSpeed * yawSensitivity;
    }

    Vector3 moveDir = (transform.forward * currentSpeed) + (transform.right * sidewaysSpeed);
    currentVelocity = Vector3.Lerp(currentVelocity, moveDir, smoothing * Time.deltaTime);

    transform.position += currentVelocity * Time.deltaTime;
}
```

**Notes:**
  
No comments made reading the code and understanding the desired result considerably harder, to the point that I was unsure what the code was supposed to do until I read through and analyzed it. Even now, I am still partially confused.

**Disection:**
- Lines 25-26 reduce the `pitch` and `yaw` variables by `360f`, which sets up the comparison of their absolute value with the `neturalZone` on lines 30 & 52.
- If the `pitch` is less than or equal to the `neutralZone`, then the `targetSpeed` -- which will come into play later -- will be set to `minSpeed`. 
- If `pitch` is greater than the `neutralZone`, then some logic will be executed in lines 36-46:
  - First, the `pitch` has its absolute value subtracted by the `neutralZone`, which results in the `effectivePitch`. 
  - That value is then divided by `90f - neturalZone` and clamped between `0f` and `1f` to get the `normalizedPitch`. 
  - If the `pitch` is less than `0f`, then the `targetSpeed` is lerped between the `minSpeed` and `maxSpeed * 0.5f` (notably half of `maxSpeed`) at an interpolation rate of `normalizedPitch * pitchSensitivityUp`.
  - If the `pitch` is greater than or equal to `0f`, then the `targetSpeed` is lerped between the `minSpeed` and `maxSpeed` at an interpolation rate of `normalizedPitch * pitchSensitivityDown`.
- If the absolute value of `yaw` is greater than `neutralZone`, then similar logic will be performed:
  - The `effectiveYaw` is taken in a similar way as the `effectivePitch`, but is then given whatever sign `yaw` has.
  - The `normalizedYaw` is taken in a similar way as the `normalizedPitch`, but is clamped between `-1f` and `1f` instead.
  - `sidewaysSpeed` is then set to the product of `normalizedYaw`, `sideSpeed`, and `yawSensitivity`.
- The `moveDir` is then taken from the current forward direction of the camera + the current right direction of the camera, multiplied by `currentSpeed` and `sidewaysSpeed` respectively.

**Review:**

The goal of the script is to rotate the camera indefinetly while the user's head is beyond a certain rotation threshold, so that the user can control the spaceship without needing to rotate their head 180 degrees in both directions.

Currently this script works, but lacks the precision that would be required to make aiming at asteroids accurate and reliable.

This could potentially be solved by using two seperate logics for rotation depending on where in the threshold the user's head rotation is. If inside the threshold, allow the user to accurately point the camera in the direciton they are facing (avoiding the use of `Mathf.Lerp` may help here), and if they are outside the threshold then rotate the camera in the direction the user is looking until they are back in the threshold.

### `ParentCameraRotation.cs:7-17`:

**Code Snippet**
```cs
public Transform childCamera;

public float rotationSpeed = 5f;

void LateUpdate()
{
    if (childCamera == null)
        return;

    transform.rotation = Quaternion.Slerp(transform.rotation, childCamera.rotation, rotationSpeed * Time.deltaTime);
}
```

**Notes:**

Since this script is significantly shorter than the last one, there isn't much to disect, and we'll skip straight to the review.

**Review**

This script is good and works as desired. The only things to note is that the check of `childCamera == null` can be done in the `Start()` function instead of checking it every frame. If theres a chance that `childCamera` can be set to null, then keeping the check in `LateUpdate()` will still work fine. 

[Link to `Start()` documentation](https://docs.unity3d.com/6000.2/Documentation/ScriptReference/MonoBehaviour.Start.html)

## What Changed?

At no ones fault but my own, I wrote this review too late before the deadline and was unable to get into contact with Leo Yudelson for replies or changes. I hope that this review was substantial enough to show what problems I have with the current implementation, and that Leo is able to make changes that he sees fit based on the review. I understand that this portion of the review is a large part of the grade, which is unfortunate for me and I will take this as a sign to start and complete my work earlier than I have been recently.