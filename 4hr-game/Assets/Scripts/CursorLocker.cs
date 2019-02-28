using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CursorLocker
{
    private static bool _locked;

    // The current state of the cursor.
    public static bool Locked
    {
        // Return the value of _locked.
        get {
            return _locked;
        }
        set {
            if(value)
                // Lock the cursor if needed.
                LockCursor();
            else
                // Un-lock the cursor if needed.
                UnlcokCursor();
        }
    }

    /// Locks and hides the cursor.
    public static void LockCursor()
    {
        //Make the cursor locked to the middle of the screen.
        Cursor.lockState = CursorLockMode.Locked;
        
        // Make the cursor invisible.
        Cursor.visible = false;

        // Update the internal for CursorLocker.Locked.
        _locked = true;
    }
    /// Shows and unlocks the cursor.
    public static void UnlcokCursor()
    {
        //Make the cursor locked to the middle of the screen.
        Cursor.lockState = CursorLockMode.None;

        // Make the cursor visibe.
        Cursor.visible = true;

        // Update the internal for CursorLocker.Locked.
        _locked = false;
    }
}
